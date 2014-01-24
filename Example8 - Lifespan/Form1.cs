using Examples.ExtensionsImplements;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Windows.Forms;

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
            public WinWrap.Basic.Instance instance;
            public DateTime timestamp;
        }

        private static readonly string[] scripts_ =
        {
            "Good.cls",
            "ParseError.cls",
            "RuntimeError.cls",
            "Stop.cls",
            "TooLong.cls"
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
            basicNoUIObj.ErrorAlert += basicNoUIObj__ErrorAlert;
            basicNoUIObj.Pause_ += basicNoUIObj__Pause_;
            basicNoUIObj.Secret = new System.Guid("00000000-0000-0000-0000-000000000000");
            basicNoUIObj.Initialize();
            // automatically disconnect BasicNoUIObj when form closes
            basicNoUIObj.AttachToForm(this, WinWrap.Basic.ManageConstants.All);
            /* Extend WinWrap Basic scripts with Examples.Extensions assembly
             * Add "Imports Examples.Extensions" to all WinWrap Basic scripts
             * Add "Imports Examples.Extensions.ScriptingLanguage" all WinWrap Basic scripts */
            basicNoUIObj.AddScriptableReference(typeof(ScriptingLanguage).Assembly,
                "Examples.ExtensionsImplements Examples.Extensions.ScriptingLanguage");
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
                mts.Remove(path);
                mt = new InstanceAndTimestamp();
            }

            try
            {
                if (mt.instance == null)
                {
                    // parse the script and create an instance
                    mt.instance = basicNoUIObj.CreateInstance(path);
                    if (mt.instance == null)
                        LogError(basicNoUIObj.Error);
                    else
                    {
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
                mt.instance.Dispose();

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

        void basicNoUIObj__ErrorAlert(object sender, EventArgs e)
        {
            WinWrap.Basic.BasicNoUIObj basicNoUIObj = sender as WinWrap.Basic.BasicNoUIObj;
            LogError(basicNoUIObj.Error);
        }

        void basicNoUIObj__Pause_(object sender, EventArgs e)
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
