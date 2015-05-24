using Examples.Extensions;
using System;
using System.Diagnostics;
using System.Text;
using System.Threading;
using System.Web.UI;

//------------------------------------------------------------------------------
// <copyright from='2013' to='2014' company='Polar Engineering and Consulting'>
//    Copyright (c) Polar Engineering and Consulting. All Rights Reserved.
//
//    This file contains confidential material.
//
// </copyright>
//------------------------------------------------------------------------------

// how to debug an azure website
// http://jessekallhoff.com/2014/07/09/remote-debugging-with-windows-azure/

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
            //responses_.Append("Debug");
            //Thread.Sleep(1000);
            WinWrapExecute(true);
        }

        protected void ButtonShow_Click(object sender, EventArgs e)
        {
            Log("");
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
                basicNoUIObj.Secret = new Guid(AzureOnlyStrings.GetNamedString("Secret", "00000000-0000-0000-0000-000000000000"));
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
                                Log("Debugging...");
                            }

                            // Execute script code via an event
                            ScriptingLanguage.TheIncident.Start("Default.aspx");

                            if (debug)
                            {
                                Log("Debugging complete.");
                            }
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
                    string[] commands2 = commands.Split(new string[] { "\r\n" }, StringSplitOptions.RemoveEmptyEntries);
                    foreach (string command in commands2)
                    {
                        string[] parts = command.Split(new char[] { ' ' }, 2);
                        int id = int.Parse(parts[0]);
                        string param = Encoding.UTF8.GetString(Convert.FromBase64String(parts[1]));
                        basicNoUIObj.Synchronize(param, id);
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
        }

        void basicNoUIObj_Pause_(object sender, EventArgs e)
        {
            WinWrap.Basic.BasicNoUIObj basicNoUIObj = sender as WinWrap.Basic.BasicNoUIObj;
            // timedout or paused while not debugging
            if (timedout_ || !basicNoUIObj.Synchronized)
            {
                if (basicNoUIObj.Error == null)
                {
                    LogError(Examples.SharedSource.WinWrapBasic.FormatTimeoutError(basicNoUIObj, timedout_));
                }

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
            string response = e.Id + " " + Convert.ToBase64String(Encoding.UTF8.GetBytes(e.Param)) + "\r\n";
            responses_.Append(response);
        }
    }
}