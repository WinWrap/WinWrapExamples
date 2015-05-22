using Examples.Extensions;
using System;
using System.Diagnostics;
using System.IO;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading;
using System.Web.Helpers;
using System.Web.UI;

//------------------------------------------------------------------------------
// <copyright from='2013' to='2014' company='Polar Engineering and Consulting'>
//    Copyright (c) Polar Engineering and Consulting. All Rights Reserved.
//
//    This file contains confidential material.
//
// </copyright>
//------------------------------------------------------------------------------

namespace Example
{
    public partial class Form1 : Page, IHost
    {
        private ApplicationQueue commands_ = ApplicationQueue.Create("commands");
        private ApplicationQueue responses_ = ApplicationQueue.Create("responses");

        private static readonly string[] scripts_ =
        {
            "DOTNet.bas",
            "BlockedParseError.bas",
            "BlockedRuntimeError.bas",
            "ParseError.bas",
            "RuntimeError.bas",
            "Stop.bas",
            "TooLong.bas"
        };
        private bool timedout_;
        private DateTime timelimit_;
        private bool halted_;

        protected void Page_Load(object sender, EventArgs e)
        {
            ScriptingLanguage.SetHost(this);
            if (!Page.IsPostBack)
            {
                Session["Text"] = "";
                ListBoxScripts_Initialize();
            }
        }

        protected void ButtonRun_Click(object sender, EventArgs e)
        {
            WinWrapExecute(false);
        }

        protected void ButtonDebug_Click(object sender, EventArgs e)
        {
            WinWrapExecute(true);
        }

        protected void ButtonShow_Click(object sender, EventArgs e)
        {
            Log("");
        }

        private string DataDirectory()
        {
            return AppDomain.CurrentDomain.GetData("DataDirectory").ToString();
        }

        private string GetPatternString(string pattern)
        {
            // put GitHub\Working-business\WinWrapExamples\examples\examples-string-a675bb8c.txt
            //   in "C:\Users\Public\Public Documents\examples"
            // download Application-a675bb8c.htm to C:\Polar Engineering\WinWrap Basic\Certificates
            string path = DataDirectory() + @"\WinWrapBasic10\examples-strings-a675bb8c.txt";
            if (!File.Exists(path)) return "00000000-0000-0000-0000-000000000000";
            string strings = File.ReadAllText(path);
            Regex rgx = new Regex(pattern, RegexOptions.Multiline);
            MatchCollection matches = rgx.Matches(strings);
            string smatch = rgx.Match(strings).Groups[1].Value.ToString();
            return smatch;
        }

        private void WinWrapExecute(bool debug)
        {
            TheIncident = new Incident();
            using (var basicNoUIObj = new WinWrap.Basic.BasicNoUIObj())
            {
                basicNoUIObj.Begin += basicNoUIObj_Begin;
                basicNoUIObj.DoEvents += basicNoUIObj_DoEvents;
                basicNoUIObj.End += basicNoUIObj_End;
                basicNoUIObj.ErrorAlert += basicNoUIObj_ErrorAlert;
                basicNoUIObj.Pause_ += basicNoUIObj_Pause_;
                basicNoUIObj.Resume += basicNoUIObj_Resume;
                basicNoUIObj.Synchronizing += basicNoUIObj_Synchronizing;
                basicNoUIObj.Secret = new Guid(GetPatternString("Guid[(]\"(.*)\"[)]"));
                basicNoUIObj.Initialize();
                basicNoUIObj.AddScriptableObjectModel(typeof(ScriptingLanguage));
                if (!basicNoUIObj.LoadModule(ScriptPath("Globals.bas")))
                    LogError(basicNoUIObj.Error);
                else
                {
                    using (var module = basicNoUIObj.ModuleInstance(ScriptPath(Script), false))
                    {
                        if (module == null)
                            LogError(basicNoUIObj.Error);
                        else
                        {
                            if (debug)
                            {
                                // prepare for debugging
                                basicNoUIObj.Synchronized = true;
                                module.StepInto = true;
                                timelimit_ = DateTime.Now + new TimeSpan(0, 0, 30); // timeout in 30 seconds
                            }

                            // Execute script code via an event
                            ScriptingLanguage.TheIncident.Start("Default.aspx");
                        }
                    }
                }
            }
            TheIncident = null;
        }

        void basicNoUIObj_DoEvents(object sender, EventArgs e)
        {
            WinWrap.Basic.BasicNoUIObj basicNoUIObj = sender as WinWrap.Basic.BasicNoUIObj;
            if (basicNoUIObj.Synchronized)
            {
                // process pending debugging commands
                string commands = commands_.ReadAll();
                if (!string.IsNullOrEmpty(commands))
                {
                    timelimit_ = DateTime.Now + new TimeSpan(0, 0, 10); // timeout in ten seconds
                    dynamic syncs = Json.Decode("[" + commands.Substring(0, commands.Length - 3) + "]");
                    foreach (dynamic sync in syncs)
                    {
                        if (sync.Param.StartsWith("end "))
                            halted_ = true;

                        basicNoUIObj.Synchronize(sync.Param, sync.id);
                    }
                }
            }

            if (DateTime.Now >= timelimit_)
            {
                timedout_ = true;
                // time timelimit has been reached, terminate the script
                basicNoUIObj.Run = false;
            }
        }

        void basicNoUIObj_Begin(object sender, EventArgs e)
        {
            WinWrap.Basic.BasicNoUIObj basicNoUIObj = sender as WinWrap.Basic.BasicNoUIObj;
            timedout_ = false;
            timelimit_ = DateTime.Now + new TimeSpan(0, 0, 1); // timeout in one second
        }

        void basicNoUIObj_End(object sender, EventArgs e)
        {
            WinWrap.Basic.BasicNoUIObj basicNoUIObj = sender as WinWrap.Basic.BasicNoUIObj;
            if (timedout_ && basicNoUIObj.Error == null)
            {
                // timedout
                LogError(Examples.SharedSource.WinWrapBasic.FormatTimeoutError(basicNoUIObj, timedout_));
            }
            else if (halted_)
            {
                Log("Execution halted during debugging.");
            }

            halted_ = false;
        }

        void basicNoUIObj_Pause_(object sender, EventArgs e)
        {
            WinWrap.Basic.BasicNoUIObj basicNoUIObj = sender as WinWrap.Basic.BasicNoUIObj;
            if (timedout_ || !basicNoUIObj.Synchronized)
            {
                // timedout or not debugging
                // Script execution has paused, terminate the script
                basicNoUIObj.Run = false;
            }
        }

        void basicNoUIObj_Resume(object sender, EventArgs e)
        {
        }

        void basicNoUIObj_ErrorAlert(object sender, EventArgs e)
        {
            WinWrap.Basic.BasicNoUIObj basicNoUIObj = sender as WinWrap.Basic.BasicNoUIObj;
            LogError(basicNoUIObj.Error);
        }

        void basicNoUIObj_Synchronizing(object sender, WinWrap.Basic.Classic.SynchronizingEventArgs e)
        {
            string response = Json.Encode(e);
            responses_.Append(response + ",\r\n");
        }
    }
}