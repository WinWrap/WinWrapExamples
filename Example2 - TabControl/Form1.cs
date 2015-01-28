using Examples.Extensions;
using System;
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
    public partial class Form1 : Form, IHost
    {
        #region "IHost"

        public Incident TheIncident { get; private set; }

        public void Log(string text)
        {
            textBox1.AppendText(text);
        }

        #endregion

        private WinWrap.Basic.Module module_;
        private static readonly string[] scripts_ =
        {
            "Good.bas",
            "DOTNet.bas",
            "ParseError.bas",
            "RuntimeError.bas"
        };
        private string Script { get { return scripts_[listBoxScripts.SelectedIndex]; } }

        public Form1()
        {
            InitializeComponent();
            TheIncident = new Incident();
            foreach (string script in scripts_)
                listBoxScripts.Items.Add(script);
            listBoxScripts.SelectedIndex = 0;
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            // selecting tabPage2 initializes the BasicIdeCtl
            tabControl1.SelectedTab = tabPage2;
            tabControl1.SelectedTab = tabPage1;

            basicIdeCtl1.AddScriptableObjectModel(typeof(ScriptingLanguage));

            // Initialize Language with this as the IHost
            ScriptingLanguage.SetHost(this);
        }

        private void basicIdeCtl1_LeaveDesignMode(object sender, WinWrap.Basic.Classic.DesignModeEventArgs e)
        {
            if (basicIdeCtl1.LoadModule(ScriptPath("Globals.bas")))
            {
                // Load selected script (can access globals loaded above)
                module_ = basicIdeCtl1.ModuleInstance(ScriptPath(Script), false);
            }

            if (module_ == null)
            {
                // Can't leave design mode because LoadModule or ModuleInstance were unsuccessful
                e.Cancel = true;
                basicIdeCtl1.ExecuteMenuCommand(WinWrap.Basic.CommandConstants.ShowError);
                basicIdeCtl1.UnloadModule(ScriptPath("Globals.bas"));
            }
        }

        private void basicIdeCtl1_EnterDesignMode(object sender, WinWrap.Basic.Classic.DesignModeEventArgs e)
        {
            if (module_ != null)
            {
                module_.Dispose();
                module_ = null;
                basicIdeCtl1.UnloadModule(ScriptPath("Globals.bas"));
            }
        }

        // WinWrap Basic execution has encountered an unhandled run-time error
        // show the IDE so the error message box is not displayed
        private void basicIdeCtl1_HandleError(object sender, EventArgs e)
        {
            tabControl1.SelectedTab = tabPage2;
        }

        // WinWrap Basic execution has paused because a break point
        // has been encountered or a stop instruction has been
        // executed or an unhandled run-time error has occurred
        private void basicIdeCtl1_Pause_(object sender, EventArgs e)
        {
            tabControl1.SelectedTab = tabPage2;
        }

        // default AttachToForm behavior shows the correct form, now select the IDE's tab
        private void basicIdeCtl1_ShowForm(object sender, EventArgs e)
        {
            tabControl1.SelectedTab = tabPage2;
        }

        private void tabControl1_Deselecting(object sender, TabControlCancelEventArgs e)
        {
            if (e.TabPageIndex == 1)
                basicIdeCtl1.Run = false;
        }

        private void buttonEdit_Click(object sender, EventArgs e)
        {
            basicIdeCtl1.DesignMode_ = true; // enter design mode
            basicIdeCtl1.FileName = ScriptPath(Script);
            basicIdeCtl1.ActiveSheet = basicIdeCtl1.FindSheet(ScriptPath(Script));
            tabControl1.SelectedTab = tabPage2;
        }

        private void buttonRun_Click(object sender, EventArgs e)
        {
            basicIdeCtl1.DesignMode_ = true; // enter design mode
            basicIdeCtl1.DesignMode_ = false;  // leave design mode
            ScriptingLanguage.TheIncident.Start("Form1");
        }

        private string ScriptPath(string script)
        {
            string dir = @"..\..\..\Scripts";
            return string.Format(@"{0}\{1}", dir, script);
        }
    }
}
