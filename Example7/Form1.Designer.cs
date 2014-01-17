namespace Example7
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(Form1));
            this.tabControl1 = new System.Windows.Forms.TabControl();
            this.tabPage1 = new System.Windows.Forms.TabPage();
            this.textBox1 = new System.Windows.Forms.TextBox();
            this.panel1 = new System.Windows.Forms.Panel();
            this.buttonRun = new System.Windows.Forms.Button();
            this.buttonEdit = new System.Windows.Forms.Button();
            this.listBoxScripts = new System.Windows.Forms.ListBox();
            this.tabPage2 = new System.Windows.Forms.TabPage();
            this.basicIdeCtl1 = new WinWrap.Basic.BasicIdeCtl();
            this.tabControl1.SuspendLayout();
            this.tabPage1.SuspendLayout();
            this.panel1.SuspendLayout();
            this.tabPage2.SuspendLayout();
            this.SuspendLayout();
            // 
            // tabControl1
            // 
            this.tabControl1.Controls.Add(this.tabPage1);
            this.tabControl1.Controls.Add(this.tabPage2);
            this.tabControl1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tabControl1.Location = new System.Drawing.Point(0, 0);
            this.tabControl1.Margin = new System.Windows.Forms.Padding(2);
            this.tabControl1.Name = "tabControl1";
            this.tabControl1.SelectedIndex = 0;
            this.tabControl1.Size = new System.Drawing.Size(424, 292);
            this.tabControl1.TabIndex = 0;
            this.tabControl1.Deselecting += new System.Windows.Forms.TabControlCancelEventHandler(this.tabControl1_Deselecting);
            // 
            // tabPage1
            // 
            this.tabPage1.Controls.Add(this.textBox1);
            this.tabPage1.Controls.Add(this.panel1);
            this.tabPage1.Location = new System.Drawing.Point(4, 22);
            this.tabPage1.Margin = new System.Windows.Forms.Padding(2);
            this.tabPage1.Name = "tabPage1";
            this.tabPage1.Padding = new System.Windows.Forms.Padding(2);
            this.tabPage1.Size = new System.Drawing.Size(416, 266);
            this.tabPage1.TabIndex = 0;
            this.tabPage1.Text = "Example7";
            this.tabPage1.UseVisualStyleBackColor = true;
            // 
            // textBox1
            // 
            this.textBox1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.textBox1.Location = new System.Drawing.Point(2, 152);
            this.textBox1.Margin = new System.Windows.Forms.Padding(2);
            this.textBox1.Multiline = true;
            this.textBox1.Name = "textBox1";
            this.textBox1.Size = new System.Drawing.Size(412, 112);
            this.textBox1.TabIndex = 1;
            // 
            // panel1
            // 
            this.panel1.Controls.Add(this.buttonRun);
            this.panel1.Controls.Add(this.buttonEdit);
            this.panel1.Controls.Add(this.listBoxScripts);
            this.panel1.Dock = System.Windows.Forms.DockStyle.Top;
            this.panel1.Location = new System.Drawing.Point(2, 2);
            this.panel1.Margin = new System.Windows.Forms.Padding(2);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(412, 150);
            this.panel1.TabIndex = 0;
            // 
            // buttonRun
            // 
            this.buttonRun.Location = new System.Drawing.Point(196, 111);
            this.buttonRun.Margin = new System.Windows.Forms.Padding(2);
            this.buttonRun.Name = "buttonRun";
            this.buttonRun.Size = new System.Drawing.Size(135, 19);
            this.buttonRun.TabIndex = 10;
            this.buttonRun.Text = "Run Script";
            this.buttonRun.UseVisualStyleBackColor = true;
            this.buttonRun.Click += new System.EventHandler(this.buttonRun_Click);
            // 
            // buttonEdit
            // 
            this.buttonEdit.Location = new System.Drawing.Point(196, 79);
            this.buttonEdit.Margin = new System.Windows.Forms.Padding(2);
            this.buttonEdit.Name = "buttonEdit";
            this.buttonEdit.Size = new System.Drawing.Size(135, 19);
            this.buttonEdit.TabIndex = 9;
            this.buttonEdit.Text = "Edit Script";
            this.buttonEdit.UseVisualStyleBackColor = true;
            this.buttonEdit.Click += new System.EventHandler(this.buttonEdit_Click);
            // 
            // listBoxScripts
            // 
            this.listBoxScripts.FormattingEnabled = true;
            this.listBoxScripts.Location = new System.Drawing.Point(46, 13);
            this.listBoxScripts.Margin = new System.Windows.Forms.Padding(2);
            this.listBoxScripts.Name = "listBoxScripts";
            this.listBoxScripts.Size = new System.Drawing.Size(126, 121);
            this.listBoxScripts.TabIndex = 8;
            // 
            // tabPage2
            // 
            this.tabPage2.Controls.Add(this.basicIdeCtl1);
            this.tabPage2.Location = new System.Drawing.Point(4, 22);
            this.tabPage2.Margin = new System.Windows.Forms.Padding(2);
            this.tabPage2.Name = "tabPage2";
            this.tabPage2.Padding = new System.Windows.Forms.Padding(2);
            this.tabPage2.Size = new System.Drawing.Size(416, 266);
            this.tabPage2.TabIndex = 1;
            this.tabPage2.Text = "Scripts";
            this.tabPage2.UseVisualStyleBackColor = true;
            // 
            // basicIdeCtl1
            // 
            this.basicIdeCtl1.BackColor = System.Drawing.Color.White;
            this.basicIdeCtl1.DefaultMacroName = "Script";
            this.basicIdeCtl1.DefaultObjectName = "Object.obm|Object";
            this.basicIdeCtl1.DefaultProjectName = "Project.wbp|wbm";
            this.basicIdeCtl1.DesignModeVisible = true;
            this.basicIdeCtl1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.basicIdeCtl1.FileChangeDir = false;
            this.basicIdeCtl1.ForeColor = System.Drawing.Color.Black;
            this.basicIdeCtl1.LargeIcon = ((System.Drawing.Icon)(resources.GetObject("basicIdeCtl1.LargeIcon")));
            this.basicIdeCtl1.Location = new System.Drawing.Point(2, 2);
            this.basicIdeCtl1.Margin = new System.Windows.Forms.Padding(2);
            this.basicIdeCtl1.Name = "basicIdeCtl1";
            this.basicIdeCtl1.NegotiateMenus = false;
            this.basicIdeCtl1.Secret = new System.Guid("00000000-0000-0000-0000-000000000000");
            this.basicIdeCtl1.SelBackColor = System.Drawing.Color.FromArgb(((int)(((byte)(51)))), ((int)(((byte)(153)))), ((int)(((byte)(255)))));
            this.basicIdeCtl1.SelForeColor = System.Drawing.Color.White;
            this.basicIdeCtl1.Size = new System.Drawing.Size(412, 262);
            this.basicIdeCtl1.SmallIcon = ((System.Drawing.Icon)(resources.GetObject("basicIdeCtl1.SmallIcon")));
            this.basicIdeCtl1.TabIndex = 0;
            this.basicIdeCtl1.TaskbarIcon = ((System.Drawing.Icon)(resources.GetObject("basicIdeCtl1.TaskbarIcon")));
            this.basicIdeCtl1.EnterDesignMode += new System.EventHandler<WinWrap.Basic.Classic.DesignModeEventArgs>(this.basicIdeCtl1_EnterDesignMode);
            this.basicIdeCtl1.GetMacroName += new System.EventHandler<WinWrap.Basic.Classic.GetMacroNameEventArgs>(this.basicIdeCtl1_GetMacroName);
            this.basicIdeCtl1.HandleError += new System.EventHandler<System.EventArgs>(this.basicIdeCtl1_HandleError);
            this.basicIdeCtl1.LeaveDesignMode += new System.EventHandler<WinWrap.Basic.Classic.DesignModeEventArgs>(this.basicIdeCtl1_LeaveDesignMode);
            this.basicIdeCtl1.Pause_ += new System.EventHandler<System.EventArgs>(this.basicIdeCtl1_Pause_);
            this.basicIdeCtl1.ShowForm += new System.EventHandler<System.EventArgs>(this.basicIdeCtl1_ShowForm);
            // 
            // Form1
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(424, 292);
            this.Controls.Add(this.tabControl1);
            this.Margin = new System.Windows.Forms.Padding(2);
            this.Name = "Form1";
            this.Text = "Example7 - Virtual File System";
            this.Load += new System.EventHandler(this.Form1_Load);
            this.tabControl1.ResumeLayout(false);
            this.tabPage1.ResumeLayout(false);
            this.tabPage1.PerformLayout();
            this.panel1.ResumeLayout(false);
            this.tabPage2.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.TabControl tabControl1;
        private System.Windows.Forms.TabPage tabPage1;
        private System.Windows.Forms.TextBox textBox1;
        private System.Windows.Forms.Panel panel1;
        private System.Windows.Forms.TabPage tabPage2;
        private System.Windows.Forms.ListBox listBoxScripts;
        private System.Windows.Forms.Button buttonRun;
        private System.Windows.Forms.Button buttonEdit;
        private WinWrap.Basic.BasicIdeCtl basicIdeCtl1;
    }
}

