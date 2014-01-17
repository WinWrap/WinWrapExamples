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
            this.panelControls = new System.Windows.Forms.Panel();
            this.richTextBoxOutput = new System.Windows.Forms.RichTextBox();
            this.panel1 = new System.Windows.Forms.Panel();
            this.listBoxScripts = new System.Windows.Forms.ListBox();
            this.buttonRunScript = new System.Windows.Forms.Button();
            this.textBoxScript = new System.Windows.Forms.TextBox();
            this.panelControls.SuspendLayout();
            this.panel1.SuspendLayout();
            this.SuspendLayout();
            // 
            // panelControls
            // 
            this.panelControls.Controls.Add(this.richTextBoxOutput);
            this.panelControls.Controls.Add(this.panel1);
            this.panelControls.Dock = System.Windows.Forms.DockStyle.Top;
            this.panelControls.Location = new System.Drawing.Point(0, 0);
            this.panelControls.Name = "panelControls";
            this.panelControls.Size = new System.Drawing.Size(512, 140);
            this.panelControls.TabIndex = 0;
            // 
            // richTextBoxOutput
            // 
            this.richTextBoxOutput.Dock = System.Windows.Forms.DockStyle.Fill;
            this.richTextBoxOutput.Location = new System.Drawing.Point(164, 0);
            this.richTextBoxOutput.Name = "richTextBoxOutput";
            this.richTextBoxOutput.Size = new System.Drawing.Size(348, 140);
            this.richTextBoxOutput.TabIndex = 1;
            this.richTextBoxOutput.Text = "";
            // 
            // panel1
            // 
            this.panel1.Controls.Add(this.listBoxScripts);
            this.panel1.Controls.Add(this.buttonRunScript);
            this.panel1.Dock = System.Windows.Forms.DockStyle.Left;
            this.panel1.Location = new System.Drawing.Point(0, 0);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(164, 140);
            this.panel1.TabIndex = 0;
            // 
            // listBoxScripts
            // 
            this.listBoxScripts.FormattingEnabled = true;
            this.listBoxScripts.Location = new System.Drawing.Point(9, 32);
            this.listBoxScripts.Name = "listBoxScripts";
            this.listBoxScripts.Size = new System.Drawing.Size(146, 95);
            this.listBoxScripts.TabIndex = 1;
            this.listBoxScripts.SelectedIndexChanged += new System.EventHandler(this.listBoxScripts_SelectedIndexChanged);
            // 
            // buttonRunScript
            // 
            this.buttonRunScript.Location = new System.Drawing.Point(9, 9);
            this.buttonRunScript.Name = "buttonRunScript";
            this.buttonRunScript.Size = new System.Drawing.Size(146, 19);
            this.buttonRunScript.TabIndex = 0;
            this.buttonRunScript.Text = "Run Script";
            this.buttonRunScript.UseVisualStyleBackColor = true;
            this.buttonRunScript.Click += new System.EventHandler(this.buttonRunScript_Click);
            // 
            // textBoxScript
            // 
            this.textBoxScript.Dock = System.Windows.Forms.DockStyle.Fill;
            this.textBoxScript.Location = new System.Drawing.Point(0, 140);
            this.textBoxScript.Multiline = true;
            this.textBoxScript.Name = "textBoxScript";
            this.textBoxScript.ScrollBars = System.Windows.Forms.ScrollBars.Both;
            this.textBoxScript.Size = new System.Drawing.Size(512, 282);
            this.textBoxScript.TabIndex = 1;
            // 
            // Form1
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(512, 422);
            this.Controls.Add(this.textBoxScript);
            this.Controls.Add(this.panelControls);
            this.Margin = new System.Windows.Forms.Padding(2);
            this.Name = "Form1";
            this.Text = "Example 8 - Lifespan";
            this.Load += new System.EventHandler(this.Form1_Load);
            this.panelControls.ResumeLayout(false);
            this.panel1.ResumeLayout(false);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Panel panelControls;
        private System.Windows.Forms.RichTextBox richTextBoxOutput;
        private System.Windows.Forms.Panel panel1;
        private System.Windows.Forms.ListBox listBoxScripts;
        private System.Windows.Forms.Button buttonRunScript;
        private System.Windows.Forms.TextBox textBoxScript;

    }
}

