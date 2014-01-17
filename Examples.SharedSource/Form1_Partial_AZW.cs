using Examples.Extensions;
using System;
using System.IO;
using System.Text;
using System.Web;
using System.Web.UI;
using System.Collections.Generic;

namespace Example
{
    public partial class Form1
    {
        #region "IHost"

        public Incident TheIncident { get; private set; }

        public void Log(string text)
        {
            if (text.Length > 0)
                Session["text"] = text.Trim() + Environment.NewLine + Environment.NewLine + (string)Session["text"];
            LabelLog.Text = (string)Session["text"];
            //LabelScript.Text = Script;
            LabelCode.Text = File.ReadAllText(ScriptPath(Script));
        }

        #endregion

        private string Script { get { return scripts_[ListBoxScripts.SelectedIndex]; } }

        private void ListBoxScripts_Initialize()
        {
            foreach (string script in scripts_)
                ListBoxScripts.Items.Add(script);
            ListBoxScripts.SelectedIndex = 0;
        }

        private string ScriptPath(string script)
        {
            string dir = AppDomain.CurrentDomain.GetData("DataDirectory").ToString() + @"\Scripts";
            string path = string.Format(@"{0}\{1}", dir, script);
            // Visual Studio debugger does not deploy App_Data dir contents
            if (!File.Exists(path))
                path = string.Format(@"{0}\..\..\..\Scripts\{1}", dir, script);
            return path;
        }

        private void LogError(WinWrap.Basic.Error error)
        {
            Log(Examples.SharedSource.WinWrapBasic.FormatError(error));
        }

        private void LogError(string text)
        {
            Log(text);
        }
    }
}