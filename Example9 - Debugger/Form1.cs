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
        private Form3 log_;

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

            log_ = new Form3();
            log_.Show();

            // start synchronizing with the remote
            basicIdeCtl1.Synchronized = true;
            timer1.Enabled = true;
        }

        private void basicIdeCtl1_Synchronizing(object sender, WinWrap.Basic.Classic.SynchronizingEventArgs e)
        {
            log_.Append(" >> " + e.Param);
            string command = e.Id + " " + Convert.ToBase64String(Encoding.UTF8.GetBytes(e.Param)) + "\r\n";
            lock (lock_)
                commands_.Append(command);
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
                string[] responses2 = responses.Split(new string[] { "\r\n" }, StringSplitOptions.RemoveEmptyEntries);
                foreach (string response in responses2)
                {
                    string[] parts = response.Split(new char[] { ' ' }, 2);
                    int id = int.Parse(parts[0]);
                    string param = Encoding.UTF8.GetString(Convert.FromBase64String(parts[1]));
                    log_.Append(" << " + param.Split(new string[] { "\r\n" }, 2, StringSplitOptions.None)[0]);
                    basicIdeCtl1.Synchronize(param, id);
                }
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

                byte[] data = Encoding.UTF8.GetBytes("Commands:\r\n" + commands);
                HttpWebRequest request = (HttpWebRequest)HttpWebRequest.Create(url_);
                request.Method = "POST";
                request.ContentType = "application/x-www-form-urlencoded";
                request.ContentLength = data.Length;
                request.Timeout = 5000;
                try
                {
                    using (Stream newStream = request.GetRequestStream())
                    {
                        newStream.Write(data, 0, data.Length);
                    }

                    // read the response asynchronously
                    response_pending_ = true;
                    ReadResponseAsync(request, commands);
                }
                catch
                {
                    // ignore failures, try again later
                    lock (lock_)
                        commands_.Insert(0, commands);
                }
            }
        }

        private async void ReadResponseAsync(HttpWebRequest request, string commands)
        {
            try
            {
                HttpWebResponse response = (HttpWebResponse)request.GetResponse();
                using (StreamReader reader = new StreamReader(response.GetResponseStream()))
                {
                    string text = await reader.ReadToEndAsync();
                    if (text.StartsWith("Responses:\r\n"))
                    {
                        lock (lock_)
                            responses_.Append(text.Substring(12));
                    }
                    else // try again later
                        commands_.Insert(0, commands);
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
