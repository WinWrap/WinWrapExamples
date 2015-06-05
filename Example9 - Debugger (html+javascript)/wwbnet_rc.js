// WinWrap Basic WWB.NET remote control

//------------------------------------------------------------------------------
// <copyright from='2014' to='2014' company='Polar Engineering and Consulting'>
//	Copyright (c) Polar Engineering and Consulting. All Rights Reserved.
//
//	This file contains confidential material.
//
// </copyright>
//------------------------------------------------------------------------------

wwbnet_rc = (function () {
    var editor = null;
    var Range = null;
    var visual = null;
    var interval_id = null;
    var error_count = 0;
    var sync_id = 0;
    var currentfilename = '?A1';
    var currentcaption = null;
    var currentline = null;
    var currentlinemarker = null;
    var currentlineadjust = null;
    var commands = '';
    var commands_sent = null;
    var responses = '';
    var response_pending = false;
    var statecounter = 0;
    var trace = false;
    var sync_handlers = {
        // !attach params
        // Version=<version>
        // DesignModeVisible=<dmv>
        // EventMode=<em>
        // HoverEvaluation=<he>
        // FileName=<filename>
        //
        // Response to the ?attach IDE command which is the first command issued by the IDE when establishing remote control.
        // The engine's ver, dmv (0, 1), em (0, 1) and he (0, 1) are provided to the IDE. 
        sync_attach: function (param, id) {
            var filename = param.match(/\r\nFileName=(.*?)\r\n/m)[1];
            synchronize_read(filename);
        },
        // !break <params>
        // LineNum=<line>
        // OnOff=<on>
        //
        // Response to the ?break IDE command. The break point for line (>0) is set (on=1), cleared (on=1) or toggle (on=-1).
        sync_break: function (param, id) {
            var line = param.match(/\r\nLineNum=(.*?)\r\n/m)[1];
            var onoff = param.match(/\r\nOnOff=(.*?)\r\n/m)[1];
            if (onoff == '-1')
                onoff = !is_breakpoint(line - currentlineadjust);
            else
                onoff = onoff == '1';

            set_breakpoint(line - currentlineadjust, onoff);
        },
        // !breaks <params>
        // <name1>|<line1>|<line2>...
        // <name2>|<line1>|<line2>...
        // ...
        //
        // Response to the ?breaks or ?clearall IDE command. 
        sync_breaks: function (param, id) {
            editor.getSession().clearBreakpoints();
            var parts = param.split('\r\n');
            var i;
            for (i = 1; i < parts.length - 1; ++i) {
                var parts2 = parts[i].split('|');
                if (parts2[0].toLowerCase() == currentfilename.toLowerCase()) {
                    parts2.slice(1).forEach(function (line) { set_breakpoint(line - currentlineadjust, true); })
                }
            }
        },
        // !call
        // Number=<number>
        // Desc=<desc>
        // Source=<source>
        // HelpContext=<helpcontext>
        // HelpFile=<helpfile>
        // MacroName=<macroname>
        // MacroCaption=<macrocaption>
        // LineNum=<linenum>
        // Offset=<offset>
        // Line=<line>
        //
        // Response to the ?call IDE command. This only happens when the ?call causes an (error). 
        sync_call: function (param, id) {
        },
        // !changed "<name>"
        //
        // This is a notification that the macro (name) has changed and the IDE should make ?read request at some future time.
        sync_changed: function (param, id) {
        },
        // !detach
        //
        // Notification: the remote IDE should terminate remote control.
        sync_detach: function (param, id) {
            detach();
        },
        // !get <params>
        // Expr=<expr>
        // Value=<value>
        // Desc=<error>
        //
        // Response to the ?get IDE command. Either a (value) is returned or an (error) is indicated.
        sync_get: function (param, id) {
        },
        // !geth <params>
        // Expr=<expr>
        // Value=<value>
        // Desc=<error>
        //
        // Response to the ?geth IDE command. Either a (value) is returned or an (error) is indicated.
        sync_geth: function (param, id) {
        },
        // !getq <params>
        // Expr=<expr>
        // Value=<value>
        // Desc=<error>
        //
        // Response to the ?getq IDE command. Either a (value) is returned or an (error) is indicated.
        sync_getq: function (param, id) {
        },
        // !let <params>
        // Desc=<error>
        //
        // Response to the ?let IDE command. This only happens when the ?let causes an (error).
        sync_let: function (param, id) {
        },
        // !loaded
        // <name1>|<caption1>
        // <name2>|<caption2>
        // ...
        //
        // Response to the ?loaded IDE command. The names and captions of the loaded project/macro/modules are returned.
        sync_loaded: function (param, id) {
        },
        // !notify_begin
        //
        // Notification: execution has begun.
        sync_notify_begin: function (param, id) {
            synchronize_buttons();
            update_state("running...");
        },
        // !notify_debugclear
        //
        // Notification: clear the debug window.
        sync_notify_debugclear: function (param, id) {
            visual.output.each(function (index, item) { item.textContent = ""; });
        },
        // !notify_debugprint
        // <text>
        //
        // Notification: append (text) to the debug window.
        sync_notify_debugprint: function (param, id) {
            var index = param.search(/\r\n/m);
            var text = param.substr(index + 2);
            debugprint(text);
        },
        // !notify_end
        //
        // Notification: execution has ended. 
        sync_notify_end: function (param, id) {
            update_stack(param);
            synchronize_buttons();
            update_state("idle");
        },
        // !notify_errorlog <n>
        // Number=<number>
        // Desc=<desc>
        // Source=<source>
        // HelpContext=<helpcontext>
        // HelpFile=<helpfile>
        // MacroName=<macroname>
        // MacroCaption=<macrocaption>
        // LineNum=<linenum>
        // Offset=<offset>
        // Line=<line>
        //
        // Notification: error (n) encountered.
        sync_notify_errorlog: function (param, id) {
            // do nothing
        },
        // !notify_errors
        // Number=<number>
        // Desc=<desc>
        // Source=<source>
        // HelpContext=<helpcontext>
        // HelpFile=<helpfile>
        // MacroName=<macroname>
        // MacroCaption=<macrocaption>
        // LineNum=<linenum>
        // Offset=<offset>
        // Line=<line>
        //
        // Notification: error encountered.
        sync_notify_errors: function (param, id) {
        },
        // !notify_errorthrown
        //
        // Notification: an error has been thrown. 
        sync_notify_errorthrown: function (param, id) {
        },
        // !notify_eventmodechanged
        // DesignModeVisible=<dmv>
        // EventMode=<em>
        //
        // Notification: DesignModeVisible dmv (0, 1) or EventMode em (0, 1) changed. 
        sync_notify_eventmodechanged: function (param, id) {
        },
        // !notify_halted
        //
        // Notification: execution stopped. 
        sync_notify_halted: function (param, id) {
            // do nothing
        },
        // !notify_handleerror
        //
        // Notification: error needs to be handled. 
        sync_notify_handleerror: function (param, id) {
            // do nothing
        },
        // !notify_macrobegin "<name>"
        //
        // Notification: project/macro/module has been loaded. 
        sync_notify_macrobegin: function (param, id) {
        },
        // !notify_macroend "<name>"
        //
        // Notification: project/macro/module has been unloaded. 
        sync_notify_macroend: function (param, id) {
        },
        // !notify_pause
        // <depth1>: <callersline1>
        // <depth2>: <callersline2>
        // ...
        //
        // Notification: execution has paused. Returns up to 100 stack entries. 
        sync_notify_pause: function (param, id) {
            update_stack(param);
            synchronize_buttons();
            update_state("paused");
        },
        // !notify_prototypechange
        //
        // Notification: prototypes have changed.
        sync_notify_prototypechange: function (param, id) {
        },
        // !notify_resume
        //
        // Notification: execution has resumed. 
        sync_notify_resume: function (param, id) {
            synchronize_buttons();
            update_state("running...");
        },
        // !notify_showform
        //
        // Notification: the IDE's containing form needs to be shown. 
        sync_notify_showform: function (param, id) {
        },
        // !opendialog "<dir>"
        // filepath1|caption1
        // dirpath1\
        // ...
        //
        // Response to the ?opendialog IDE command. All the files (filepath1) with captions (caption1) and sub-directories (dirpath1) are returned. 
        sync_opendialog: function (param, id) {
        },
        // !queued <bool>
        //
        // Notify the IDE that it should queue (bool=true) or not queue (bool=false) all Synchronize method data.
        // When queuing is stopped all the previously queued Synchronize method data is processed. 
        sync_queued: function (param, id) {
        },
        // !prototypes "<name>|<prefix>"
        // prototype1
        // prototype2
        // ...
        //
        // Response to the ?prototypes IDE command. Returns all prototypes starting with (prefix) for macro (name). 
        sync_prototypes: function (param, id) {
        },
        // !read "<name>"
        // Caption=<caption>
        // Data=<data>
        //
        // Response to the ?read IDE command. Returns the macro (name) caption and code (data). 
        sync_read: function (param, id) {
            var filename = param.match(/^\!read "(.*?)"/m)[1];
            var caption = param.match(/\r\nCaption=(.*)\r\n/m)[1];
            update_filename(filename, caption);
            var index = param.search(/\r\nData=/);
            var code = param.substr(index + 7);
            var orglinecount = countlines(code);
            // remove hidden lines
            if (code.match(/^VERSION/)) {
                // remove: VERSION...\r\n...\r\nEND\r\n
                var x = code.indexOf('\r\nEND\r\n');
                if (x != -1)
                    code = code.substr(x + 7);
            }
            while (code.match(/^Attribute/)) {
                // remove: Attribute...\r\n
                var x = code.indexOf('\r\n');
                if (x != -1)
                    code = code.substr(x + 2);
            }
            while (code.match(/^\'\#Reference/)) {
                // remove: '#Reference ...\r\n
                var x = code.indexOf('\r\n');
                if (x != -1)
                    code = code.substr(x + 2);
            }
            currentlineadjust = orglinecount - countlines(code) + 1;
            editor.getSession().setValue(code);
            synchronize('?breaks "' + currentfilename + '"');
            synchronize('?stack 100');
            synchronize('?state "' + currentfilename + '"');
        },
        // !stack <maxdepth>
        // <depth1>: <callersline1>
        // <depth2>: <callersline2>
        // ...
        //
        // Response to the ?stack IDE command. Returns up to (maxdepth) stack entries. 
        sync_stack: function (param, id) {
            update_stack(param);
        },
        // !state "<name>"
        // MacroActive=<active>
        // MacroLoaded=<loaded>
        // DesignMode=<dm>
        // DesignModeVisible=<dmv>
        // EventMode=<em>
        // EventModeLoaded=<eml>
        // IsActive=<active>
        // IsIdle=<idle>
        // IsStopped=<stopped>
        // Depth=<depth>
        // Paused=<paused>
        // Running=<running>
        //
        // Response to the ?state IDE command.
        sync_state: function (param, id) {
            var filename = param.match(/^\!state "(.*?)"/m)[1];
            var isidle = param.match(/\r\nIsIdle=(.*?)\r\n/m)[1] == '1';
            var isstopped = param.match(/\r\nIsStopped=(.*?)\r\n/m)[1] == '1';
            var paused = param.match(/\r\nPaused=(.*?)\r\n/m)[1] == '1';
            var running = param.match(/\r\nRunning=(.*?)\r\n/m)[1] == '1';
            update_buttons(isidle, isstopped);
        },
        // !watch
        // <depth1>: <expr1> -> <value1>
        // <depth2>: <expr2> -> <value2>
        // ...
        //
        // Response to the ?watch IDE command. The values for all the watch expressions are returned. 
        sync_watch: function (param, id) {
        },
    };
    function process(data) {
        if (data != '') {
            var param = atob(data);
            var sync = 'sync_' + param.match(/^\!(.*?)( |$)/m)[1].toLowerCase();
            if (trace) debugprint(' << ' + param.split('\r\n', 2)[0] + '\r\n');
            sync_handlers[sync](param);
        }
    }
    function synchronize(data) {
        if (trace) debugprint(' >> ' + data.split('\r\n', 2)[0] + '\r\n');
        var s = sync_id + ' ' + btoa(data);
        commands += s + '\r\n';
    }
    function synchronizing(data) {
        commands_sent = null;
        response_pending = false;
        error_count = 0;
        if (data.substr(0, 12) == 'Responses:\r\n' && data.length > 12) {
            responses += data.substr(12);
        }
    }
    function synchronizing_error(jqxhr, settings, errorThrown) {
        commands = commands_sent + commands;
        commands_sent = null;
        response_pending = false;
        if (interval_id != null && ++error_count > 2) {
            detach();
        }
    }
    function synchronize_buttons() {
        statecounter = 0;
        synchronize('?state "' + currentfilename + '"');
    }
    function synchronize_read(filename) {
        editor.getSession().setValue("'reading from remote...\r\n");
        synchronize('?read "' + filename + '"');
    }
    function countlines(text) {
        var n = text.split('\n').length;
        return n;
    }
    function debugprint(text) {
        visual.output.each(function (index, item) { item.textContent += text; });
    }
    function is_breakpoint(row) {
        var found = false;
        editor.getSession().getBreakpoints().forEach(function (value, index) {
            if (index == row)
                found = true;
        });
        return found;
    }
    function set_breakpoint(row, onoff) {
        if (onoff != is_breakpoint(row)) {
            if (onoff)
                editor.getSession().setBreakpoint(row, "break");
            else
                editor.getSession().clearBreakpoint(row);
        }
    }
    function on_gutterclick(e) {
        var row = e.getDocumentPosition().row;
        var nextstate = is_breakpoint(row) ? '0' : '1';
        synchronize('break "' + currentfilename + '" ' + (row + currentlineadjust) + ' ' + nextstate);
    }
    function update_buttons(isidle, isstopped) {
        visual.attach.each(function (index, button) { button.disabled = interval_id != null; });
        visual.run.each(function (index, button) { button.disabled = interval_id == null || !(isidle || isstopped); });
        visual.pause.each(function (index, button) { button.disabled = interval_id == null || isidle || isstopped; });
        visual.end.each(function (index, button) { button.disabled = interval_id == null || isidle; });
        visual.into.each(function (index, button) { button.disabled = interval_id == null || !(isidle || isstopped); });
        visual.over.each(function (index, button) { button.disabled = interval_id == null || !(isidle || isstopped); });
        visual.out.each(function (index, button) { button.disabled = interval_id == null || !isstopped; });
    }
    function update_filename(filename, caption) {
        currentfilename = filename;
        currentcaption = caption;
        visual.filename.each(function (index, item) { item.textContent = currentfilename; });
        visual.caption.each(function (index, item) { item.textContent = currentcaption; });
    }
    function update_stack(param) {
        if (currentline) {
            editor.getSession().removeMarker(currentlinemarker);
            editor.getSession().removeGutterDecoration(currentline - currentlineadjust, 'gutter_current');
        }
        var stacktop = param.match(/\r\n(.*?): \[(.*?)\|(.*?)\|(.*?)# *?([0-9]*)\]/m);
        if (stacktop != null) {
            var filename = stacktop[2];
            if (filename != currentfilename)
                synchronize_read(filename);
            else {
                currentline = parseInt(stacktop[5]);
                currentlinemarker = editor.getSession().addMarker(new Range(currentline - currentlineadjust, 0, 0, 0), 'ace_highlight-marker', 'screenLine');
                editor.getSession().addGutterDecoration(currentline - currentlineadjust, 'gutter_current');
            }
        }
    }
    function update_state(text) {
        visual.state.each(function (index, item) { item.textContent = text; });
    }
    function detach() {
        clearInterval(interval_id);
        interval_id = null;
        update_buttons(true, false);
        update_state("unattached");
        error_count = 0;
        currentfilename = '?A1';
        currentcaption = null;
        currentline = null;
        currentlinemarker = null;
        currentlineadjust = null;
        commands = '';
        commands_sent = null;
        responses = '';
        response_pending = false;
        statecounter = 0;
    }
    return {
        initialize: function (debuggerid, sync_idA) {
            sync_id = sync_idA;
            editor = ace.edit(debuggerid);
            Range = ace.require('ace/range').Range;
            editor.setTheme('ace/theme/wwbnet');
            editor.getSession().setMode('ace/mode/wwbnet');
            editor.setHighlightActiveLine(false)
            editor.setReadOnly(true);
            editor.on("gutterclick", on_gutterclick)

            var alleditorids = $('[id^=' + debuggerid + '-]');
            visual = {
                attach: alleditorids.filter('.wwbnet_rc-attach-button'),
                run: alleditorids.filter('.wwbnet_rc-run-button'),
                pause: alleditorids.filter('.wwbnet_rc-pause-button'),
                end: alleditorids.filter('.wwbnet_rc-end-button'),
                into: alleditorids.filter('.wwbnet_rc-into-button'),
                over: alleditorids.filter('.wwbnet_rc-over-button'),
                out: alleditorids.filter('.wwbnet_rc-out-button'),
                filename: alleditorids.filter('.wwbnet_rc-filename'),
                caption: alleditorids.filter('.wwbnet_rc-caption'),
                state: alleditorids.filter('.wwbnet_rc-state'),
                output: alleditorids.filter('.wwbnet_rc-output'),
            };
            detach();
            visual.run.click(function () { synchronize('run "' + currentfilename + '"'); });
            visual.pause.click(function () { synchronize('pause'); });
            visual.end.click(function () { synchronize('end "' + currentfilename + '"'); });
            visual.into.click(function () { synchronize('into "' + currentfilename + '"'); });
            visual.over.click(function () { synchronize('over "' + currentfilename + '"'); });
            visual.out.click(function () { synchronize('out "' + currentfilename + '"'); });
        },
        attach: function (server, target) {
            if (interval_id == null) {
                error_count = 0;
                synchronize('?attach "10.30.072/32W{1600} - 6/2/2015 6:50:37 PM');
                interval_id = window.setInterval(function () {
                    if (++statecounter == 8) {
                        //synchronize_buttons();
                    }
                    if (responses != '') {
                        // process received responses
                        var temp = responses.split('\r\n');
                        responses = '';
                        temp.forEach(function (data) { process(data); });
                    }
                    if (!response_pending) {
                        // send pending commands
                        commands_sent = commands;
                        if (commands == '') {
                            synchronize('*'); // heart beat
                        }
                        temp = commands;
                        commands = '';
                        response_pending = true;
                        $.ajax({
                            url: 'http://' + server + '/DebugPortal.ashx',
                            type: 'POST',
                            contentType: 'application/x-www-form-urlencoded; charset=UTF-8',
                            data: 'Commands:' + target + '\r\n' + temp,
                            dataType: 'text',
                            success: synchronizing,
                            error: synchronizing_error
                        });
                    }
                }, 250);
            }
        }
    };
})();
