using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using System.Web.Helpers;
using System.Windows.Forms;

namespace Example
{
    public partial class Form1 : Form
    {
        private string url_;
        private StringBuilder commands_ = new StringBuilder();
        private StringBuilder responses_ = new StringBuilder();
        private bool response_pending_;
        private static object lock_ = new object();

        public Form1()
        {
            InitializeComponent();
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            string server = "localhost:50406";
#if true
            Form2 form2 = new Form2();
            form2.ShowDialog();
            server = form2.textBox1.Text;
            if (string.IsNullOrEmpty(server))
            {
                Close();
                return;
            }
#endif
            url_ = "http://" + server + "/DebugPortal.ashx";

            // start synchronizing with the remote
            basicIdeCtl1.Synchronized = true;
            timer1.Enabled = true;
        }

        private void basicIdeCtl1_Synchronizing(object sender, WinWrap.Basic.Classic.SynchronizingEventArgs e)
        {
            // Json.Encode fails if the project's "Enable Visual Studio hosting process" is checked (Debug sheet)
            string command = Json.Encode(e);
            lock (lock_)
            {
                commands_.Append(command);
                commands_.Append(",\r\n");
            }
        }

        private void timer1_Tick(object sender, EventArgs e)
        {
            // process received responses
            string responses = null;
            lock (lock_)
            {
                responses = responses_.ToString();
                responses_.Clear();
            }

            if (!string.IsNullOrEmpty(responses))
            {
                Debug.Print("  <<<Responses<<<");
                Debug.Write(responses);
                dynamic syncs = Json.Decode("[" + responses.Substring(0, responses.Length - 3) + "]");
                foreach (dynamic sync in syncs)
                    basicIdeCtl1.Synchronize(sync.Param, sync.id);
            }

            if (!response_pending_)
            {
                // send the request
                string commands = null;
                lock (lock_)
                {
                    commands = commands_.ToString();
                    commands_.Clear();
                }

                if (!string.IsNullOrEmpty(commands))
                {
                    Debug.Print("  >>>Commands>>>");
                    Debug.Write(commands);
                }

                UTF8Encoding encoding = new UTF8Encoding();
                byte[] data = encoding.GetBytes(commands);
                HttpWebRequest request = (HttpWebRequest)HttpWebRequest.Create(url_);
                request.Method = "POST";
                request.ContentType = "application/x-www-form-urlencoded";
                request.ContentLength = data.Length;
                try
                {
                    using (Stream newStream = request.GetRequestStream())
                    {
                        newStream.Write(data, 0, data.Length);
                    }

                    HttpWebResponse response = (HttpWebResponse)request.GetResponse();
                    // read the response asynchronously
                    response_pending_ = true;
                    ReadResponseAsync(response, commands);
                }
                catch
                {
                    // ignore failures, try again later
                    lock (lock_)
                        commands_.Insert(0, commands);
                }
            }
        }

        private async void ReadResponseAsync(HttpWebResponse response, string commands)
        {
            try
            {
                using (StreamReader reader = new StreamReader(response.GetResponseStream()))
                {
                    string text = await reader.ReadToEndAsync();
                    lock (lock_)
                        responses_.Append(text);
                }
            }
            catch
            {
                // ignore failures, try again later
                lock (lock_)
                    commands_.Insert(0, commands);
            }

            response_pending_ = false;
        }

        private void Form1_FormClosed(object sender, FormClosedEventArgs e)
        {
            timer1.Enabled = false;
        }
    }
}
