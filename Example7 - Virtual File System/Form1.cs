using Examples.Extensions;
using System;
using System.Windows.Forms;
using System.Diagnostics;

//------------------------------------------------------------------------------
// <copyright from='2013' to='2014' company='Polar Engineering and Consulting'>
//    Copyright (c) Polar Engineering and Consulting. All Rights Reserved.
//
//    This file contains confidential material.
//
// </copyright>
//------------------------------------------------------------------------------

namespace Example7
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
        private static readonly string[] Scripts =
        {
            "DOTNet.bas",
            "ParseError.bas",
            "RuntimeError.bas"
        };
        private string Script { get { return Scripts[listBoxScripts.SelectedIndex]; } }

        public Form1()
        {
            InitializeComponent();
            TheIncident = new Incident();
            foreach (string script in Scripts)
                listBoxScripts.Items.Add(script);
            listBoxScripts.SelectedIndex = 0;
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            // selecting tabPage2 initializes the BasicIdeCtl
            tabControl1.SelectedTab = tabPage2;
            tabControl1.SelectedTab = tabPage1;

            basicIdeCtl1.VirtualFileSystem = new MyFileSystem();

            basicIdeCtl1.AddScriptableReference(typeof(ScriptingLanguage).Assembly,
                "Examples.Extensions Examples.Extensions.ScriptingLanguage");

            // Initialize Language with this as the IHost
            ScriptingLanguage.SetHost(this);
        }

        private void basicIdeCtl1_LeaveDesignMode(object sender, WinWrap.Basic.Classic.DesignModeEventArgs e)
        {
            if (basicIdeCtl1.LoadModule("Globals.bas"))
            {
                // Load selected script (can access globals loaded above)
                module_ = basicIdeCtl1.ModuleInstance(Script, false);
            }

            if (module_ == null)
            {
                // Can't leave design mode because LoadModule or ModuleInstance were unsuccessful
                e.Cancel = true;
                basicIdeCtl1.ExecuteMenuCommand(WinWrap.Basic.CommandConstants.ShowError);
                basicIdeCtl1.UnloadModule("Globals.bas");
            }
        }

        private void basicIdeCtl1_EnterDesignMode(object sender, WinWrap.Basic.Classic.DesignModeEventArgs e)
        {
            if (module_ != null)
            {
                module_.Dispose();
                module_ = null;
                basicIdeCtl1.UnloadModule("Globals.bas");
            }
        }

        private void basicIdeCtl1_GetMacroName(object sender, WinWrap.Basic.Classic.GetMacroNameEventArgs e)
        {
            FileDialogForm dialog = new FileDialogForm();
            foreach (string script in Scripts)
                dialog.listBox1.Items.Add(script);

            dialog.listBox1.SelectedIndex = 0;
            dialog.Text = e.OpenDialog ? "Open" : "Save";
            dialog.ShowDialog(this);
            if (dialog.DialogResult == System.Windows.Forms.DialogResult.OK)
                e.FileName = Scripts[dialog.listBox1.SelectedIndex] + ".bas";

            e.Handled = true;
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
            basicIdeCtl1.FileName = Script;
            basicIdeCtl1.ActiveSheet = basicIdeCtl1.FindSheet(Script);
            tabControl1.SelectedTab = tabPage2;
        }

        private void buttonRun_Click(object sender, EventArgs e)
        {
            basicIdeCtl1.DesignMode_ = true; // enter design mode
            basicIdeCtl1.DesignMode_ = false;  // leave design mode
            ScriptingLanguage.TheIncident.Start("Form1");
        }
    }
}
