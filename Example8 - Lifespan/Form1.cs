﻿using Examples.Extensions;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Windows.Forms;

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
    public partial class Form1 : Form
    {
        private WinWrap.Basic.BasicNoUIObj basicNoUIObj;
        private bool timedout_;
        private DateTime timelimit_;
        private Dictionary<string, InstanceAndTimestamp> mts = new Dictionary<string, InstanceAndTimestamp>();

        struct InstanceAndTimestamp
        {
            public WinWrap.Basic.Module module;
            public WinWrap.Basic.Instance instance;
            public DateTime timestamp;

            public void Dispose()
            {
                if (instance != null)
                {
                    instance.Dispose();
                    instance = null;
                }

                if (module != null)
                {
                    module.Dispose();
                    module = null;
                }
            }
        }

        private static readonly string[] scripts_ =
        {
            "Good2.bas",
            "ParseError2.bas",
            "RuntimeError2.bas",
            "Stop2.bas",
            "TooLong2.bas"
        };

        public Form1()
        {
            InitializeComponent();
            ListBoxScripts_Initialize();
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            // Initialize Language with this as the IHost
            ScriptingLanguage.SetHost(this);
            // Create and initialize BasicNoUIObj object
            basicNoUIObj = new WinWrap.Basic.BasicNoUIObj();
            basicNoUIObj.Begin += basicNoUIObj_Begin;
            basicNoUIObj.Disconnecting += basicNoUIObj_Disconnecting;
            basicNoUIObj.DoEvents += basicNoUIObj_DoEvents;
            basicNoUIObj.ErrorAlert += basicNoUIObj_ErrorAlert;
            basicNoUIObj.Pause_ += basicNoUIObj_Pause_;
            basicNoUIObj.Secret = new System.Guid("00000000-0000-0000-0000-000000000000");
            basicNoUIObj.Initialize();
            // automatically disconnect BasicNoUIObj when form closes
            basicNoUIObj.AttachToForm(this, WinWrap.Basic.ManageConstants.All);
            // Extend WinWrap Basic scripts with Examples.Extensions assembly
            // Add "Imports Examples.Extensions" to all WinWrap Basic scripts
            // Add "Imports Examples.Extensions.ScriptingLanguage" all WinWrap Basic scripts
            basicNoUIObj.AddScriptableObjectModel(typeof(ScriptingLanguage));
            if (!basicNoUIObj.LoadModule(ScriptPath("Globals.bas")))
            {
                LogError(basicNoUIObj.Error);
                buttonRunScript.Enabled = false;
            }
        }

        private void buttonRunScript_Click(object sender, EventArgs e)
        {
            TheIncident = new Incident();

            InstanceAndTimestamp mt = new InstanceAndTimestamp();
            string path = ScriptPath(Script);
            DateTime timestamp = File.GetLastWriteTimeUtc(path);
            if (mts.TryGetValue(path, out mt) && mt.timestamp != timestamp)
            {
                // file has changed, dump instance from cache
                mt.Dispose();
                mts.Remove(path);
                mt = new InstanceAndTimestamp();
            }

            try
            {
                if (mt.module == null)
                {
                    // module has not been parsed
                    mt.module = basicNoUIObj.ModuleInstance(path, false);
                    if (mt.module == null)
                    {
                        // script parsing error
                        LogError(basicNoUIObj.Error);
                    }
                    else
                    {
                        // create an instance
                        mt.instance = basicNoUIObj.CreateInstance(path + "<IncidentAction");
                        // cache the instance with a timestamp
                        mt.timestamp = File.GetLastWriteTimeUtc(path);
                        mts.Add(path, mt);
                    }
                }

                if (mt.instance != null)
                {
                    // Execute script code via an interface
                    DateTime tstart2 = DateTime.Now;
                    IIncidentAction action = mt.instance.Cast<IIncidentAction>();
                    ScriptingLanguage.TheIncident.Start(action, "Form1");
                    TimeSpan ts2 = DateTime.Now - tstart2;
                    Debug.Print(ts2.ToString());
                }
            }
            catch (WinWrap.Basic.TerminatedException)
            {
                // script execution terminated, ignore error
            }
            catch (Exception ex)
            {
                // script caused an exception
                basicNoUIObj.ReportError(ex);
            }

            if (mt.instance == null && mt.module != null)
            {
                // release module
                mt.Dispose();
            }

            TheIncident = null;
        }

        void basicNoUIObj_Begin(object sender, EventArgs e)
        {
            timedout_ = false;
            timelimit_ = DateTime.Now + new TimeSpan(0, 0, 1);
        }

        void basicNoUIObj_Disconnecting(object sender, EventArgs e)
        {
            // dispose of cached instances
            foreach (InstanceAndTimestamp mt in mts.Values)
            {
                mt.instance.Dispose();
                mt.module.Dispose();
            }

            mts.Clear();
        }

        void basicNoUIObj_DoEvents(object sender, EventArgs e)
        {
            if (DateTime.Now >= timelimit_)
            {
                timedout_ = true;
                WinWrap.Basic.BasicNoUIObj basicNoUIObj = sender as WinWrap.Basic.BasicNoUIObj;
                // time limit has been reached, terminate the script
                basicNoUIObj.Run = false;
            }
        }

        void basicNoUIObj_ErrorAlert(object sender, EventArgs e)
        {
            WinWrap.Basic.BasicNoUIObj basicNoUIObj = sender as WinWrap.Basic.BasicNoUIObj;
            LogError(basicNoUIObj.Error);
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

        private void listBoxScripts_SelectedIndexChanged(object sender, EventArgs e)
        {
            textBoxScript.Text = File.ReadAllText(ScriptPath(Script));
        }
    }
}
