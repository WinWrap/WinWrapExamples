using Examples.ExtensionsImplements;
using System;
using System.IO;
using System.Windows.Forms;

namespace Example
{
    public partial class Form1 : Form
    {
        private bool timedout_;
        private DateTime timelimit_;
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
        }

        private void buttonRunScript_Click(object sender, EventArgs e)
        {
            TheIncident = new Incident();
            using (var basicNoUIObj = new WinWrap.Basic.BasicNoUIObj())
            {
                basicNoUIObj.Begin += basicNoUIObj_Begin;
                basicNoUIObj.DoEvents += basicNoUIObj_DoEvents;
                basicNoUIObj.ErrorAlert += basicNoUIObj__ErrorAlert;
                basicNoUIObj.Pause_ += basicNoUIObj__Pause_;
                basicNoUIObj.Secret = new System.Guid("00000000-0000-0000-0000-000000000000");
                basicNoUIObj.Initialize();
                /* Extend WinWrap Basic scripts with Examples.Extensions assembly
                 * Add "Imports Examples.Extensions" to all WinWrap Basic scripts
                 * Add "Imports Examples.Extensions.ScriptingLanguage" all WinWrap Basic scripts */
                basicNoUIObj.AddScriptableReference(typeof(ScriptingLanguage).Assembly,
                    "Examples.ExtensionsImplements Examples.Extensions.ScriptingLanguage");

                if (!basicNoUIObj.LoadModule(ScriptPath("Globals.bas")))
                    LogError(basicNoUIObj.Error);
                else
                {
                    try
                    {
                        using (var instance = basicNoUIObj.CreateInstance(ScriptPath(Script)))
                        {
                            // Execute script code via an interface
                            IIncidentAction action = instance.Cast<IIncidentAction>();
                            TheIncident.Start(action, this.Text);
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
