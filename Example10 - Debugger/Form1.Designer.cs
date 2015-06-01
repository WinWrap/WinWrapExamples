namespace Example
{
    partial class Form1
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.components = new System.ComponentModel.Container();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(Form1));
            this.basicIdeCtl1 = new WinWrap.Basic.BasicIdeCtl();
            this.timer1 = new System.Windows.Forms.Timer(this.components);
            this.SuspendLayout();
            // 
            // basicIdeCtl1
            // 
            this.basicIdeCtl1.BackColor = System.Drawing.Color.White;
            this.basicIdeCtl1.DefaultMacroName = "Macro";
            this.basicIdeCtl1.DefaultObjectName = "Object.obm|Object";
            this.basicIdeCtl1.DefaultProjectName = "Project.wbp|wbm";
            this.basicIdeCtl1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.basicIdeCtl1.ForeColor = System.Drawing.Color.Black;
            this.basicIdeCtl1.LargeIcon = ((System.Drawing.Icon)(resources.GetObject("basicIdeCtl1.LargeIcon")));
            this.basicIdeCtl1.Location = new System.Drawing.Point(0, 0);
            this.basicIdeCtl1.Name = "basicIdeCtl1";
            this.basicIdeCtl1.Secret = new System.Guid("00000000-0000-0000-0000-000000000000");
            this.basicIdeCtl1.SelBackColor = System.Drawing.Color.FromArgb(((int)(((byte)(51)))), ((int)(((byte)(153)))), ((int)(((byte)(255)))));
            this.basicIdeCtl1.SelForeColor = System.Drawing.Color.White;
            this.basicIdeCtl1.Size = new System.Drawing.Size(519, 396);
            this.basicIdeCtl1.SmallIcon = ((System.Drawing.Icon)(resources.GetObject("basicIdeCtl1.SmallIcon")));
            this.basicIdeCtl1.TabIndex = 0;
            this.basicIdeCtl1.TaskbarIcon = ((System.Drawing.Icon)(resources.GetObject("basicIdeCtl1.TaskbarIcon")));
            this.basicIdeCtl1.Synchronizing += new System.EventHandler<WinWrap.Basic.Classic.SynchronizingEventArgs>(this.basicIdeCtl1_Synchronizing);
            // 
            // timer1
            // 
            this.timer1.Tick += new System.EventHandler(this.timer1_Tick);
            // 
            // Form1
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(519, 396);
            this.Controls.Add(this.basicIdeCtl1);
            this.Name = "Form1";
            this.Text = "Form1";
            this.FormClosed += new System.Windows.Forms.FormClosedEventHandler(this.Form1_FormClosed);
            this.Load += new System.EventHandler(this.Form1_Load);
            this.ResumeLayout(false);

        }

        #endregion

        private WinWrap.Basic.BasicIdeCtl basicIdeCtl1;
        private System.Windows.Forms.Timer timer1;
    }
}

