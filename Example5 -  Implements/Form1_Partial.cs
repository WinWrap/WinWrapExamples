using Examples.ExtensionsImplements;
using System;
using System.Drawing;
using System.Text;

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
    public partial class Form1 : IHost
    {
        #region "IHost"

        public Incident TheIncident { get; private set; }

        public void Log(string text)
        {
            if (outputIsError_) { richTextBoxOutput.Text = ""; }
            richTextBoxOutput.AppendText(text + Environment.NewLine);
            richTextBoxOutput.SelectionStart = richTextBoxOutput.Text.Length;
            richTextBoxOutput.ScrollToCaret();
            outputIsError_ = false;
        }

        #endregion

        private string Script { get { return scripts_[listBoxScripts.SelectedIndex]; } }
        private bool outputIsError_ = false;

        private void ListBoxScripts_Initialize()
        {
            foreach (string script in scripts_)
                listBoxScripts.Items.Add(script);
            listBoxScripts.SelectedIndex = 0;
        }

        private string ScriptPath(string script)
        {
            string dir = @"..\..\..\Scripts";
            return string.Format(@"{0}\{1}", dir, script);
        }

        private void LogError(WinWrap.Basic.Error error)
        {
            LogError(Examples.SharedSource.WinWrapBasic.FormatError(error));
        }

        private void LogError(string text)
        {
            var sb = new StringBuilder();
            sb.AppendLine(DateTime.Now.ToString());
            sb.AppendLine(text);
            richTextBoxOutput.Text = "";
            richTextBoxOutput.SelectionColor = Color.Red;
            richTextBoxOutput.SelectedText = sb.ToString();
            outputIsError_ = true;
        }
    }
}
