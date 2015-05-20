using Examples.Extensions;
using System;
using System.IO;
using System.Text.RegularExpressions;
using System.Web.UI;
using System.Reflection;

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

        protected void Page_Load(object sender, EventArgs e)
        {
            ScriptingLanguage.SetHost(this);
            if (!Page.IsPostBack)
            {
                Session["Text"] = "";
                ListBoxScripts_Initialize();
                //((IPostBackEventHandler)ButtonRun).RaisePostBackEvent(null);
                
            }
        }

        protected void ButtonRun_Click(object sender, EventArgs e)
        {
            WinWrapExecute();
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
            // put GitHub\Working-business\WinWrapExamples\examples-string-a675bb8c.txt
            //   in "Public Documents\examples"
            string path = DataDirectory() + @"\WinWrapBasic10\examples-strings-a675bb8c.txt";
            if (!File.Exists(path)) return "00000000-0000-0000-0000-000000000000";
            string strings = File.ReadAllText(path);
            Regex rgx = new Regex(pattern, RegexOptions.Multiline);
            MatchCollection matches = rgx.Matches(strings);
            string smatch = rgx.Match(strings).Groups[1].Value.ToString();
            return smatch;
        }

        private void WinWrapExecute()
        {
            TheIncident = new Incident();
            using (var basicNoUIObj = new WinWrap.Basic.BasicNoUIObj())
            {
                basicNoUIObj.Begin += basicNoUIObj_Begin;
                basicNoUIObj.DoEvents += basicNoUIObj_DoEvents;
                basicNoUIObj.ErrorAlert += basicNoUIObj_ErrorAlert;
                basicNoUIObj.Pause_ += basicNoUIObj_Pause_;
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
                            // Execute script code via an event
                            ScriptingLanguage.TheIncident.Start("Default.aspx");
                        }
                    }
                }
            }
            TheIncident = null;
        }

        void basicNoUIObj_Begin(object sender, EventArgs e)
        {
            timedout_ = false;
            timelimit_ = DateTime.Now + new TimeSpan(0, 0, 1);
        }

        void basicNoUIObj_DoEvents(object sender, EventArgs e)
        {
            if (DateTime.Now >= timelimit_)
            {
                timedout_ = true;
                WinWrap.Basic.BasicNoUIObj basicNoUIObj = sender as WinWrap.Basic.BasicNoUIObj;
                // time timelimit has been reached, terminate the script
                basicNoUIObj.Run = false;
            }
        }

        void basicNoUIObj_Pause_(object sender, EventArgs e)
        {
            WinWrap.Basic.BasicNoUIObj basicNoUIObj = sender as WinWrap.Basic.BasicNoUIObj;
            if (basicNoUIObj.Error == null)
            {
                LogError(Examples.SharedSource.WinWrapBasic.FormatTimeoutError(basicNoUIObj, timedout_));
            }
            // Script execution has paused, terminate the script
            basicNoUIObj.Run = false;
        }

        void basicNoUIObj_ErrorAlert(object sender, EventArgs e)
        {
            WinWrap.Basic.BasicNoUIObj basicNoUIObj = sender as WinWrap.Basic.BasicNoUIObj;
            LogError(basicNoUIObj.Error);
        }
    }
}