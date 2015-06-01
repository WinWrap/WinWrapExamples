using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Example
{
    public partial class Form1 : Form
    {
        Connection conn_;
        private StringBuilder commands_ = new StringBuilder();
        private Form3 log_;

        public Form1()
        {
            InitializeComponent();
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            log_ = new Form3();
            log_.Show();

            try
            {
                conn_ = new Connection("127.0.0.1", 10000);
            }
            catch (Exception ex)
            {
                MessageBox.Show("Can't connect to remote port.\r\n\r\n" + ex.ToString());
                Close();
                return;
            }

            basicIdeCtl1.Synchronized = true;
            timer1.Enabled = true;
        }

        private void basicIdeCtl1_Synchronizing(object sender, WinWrap.Basic.Classic.SynchronizingEventArgs e)
        {
            log_.Append(" >> " + e.Param);
            string command = Convert.ToBase64String(Encoding.UTF8.GetBytes(e.Param)) + "\r\n";
            commands_.Append(command);
        }

        private void timer1_Tick(object sender, EventArgs e)
        {
            // process received responses
            string[] responses = conn_.GetReceviedData("\r\n");
            foreach (string response in responses)
            {
                string param = Encoding.UTF8.GetString(Convert.FromBase64String(response));
                log_.Append(" << " + param.Split(new string[] { "\r\n" }, 2, StringSplitOptions.None)[0]);
                basicIdeCtl1.Synchronize(param, 0); // id is ignored
            }

            string commands = commands_.ToString();
            commands_.Clear();
                
            try
            {
                conn_.Send(commands);
            }
            catch
            {
                // ignore failures, try again later
                commands_.Insert(0, commands);
            }
        }

        private void Form1_FormClosed(object sender, FormClosedEventArgs e)
        {
            timer1.Enabled = false;
        }
    }
}
