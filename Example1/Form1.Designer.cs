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
            this.panelRunScript = new System.Windows.Forms.Panel();
            this.listBoxScripts = new System.Windows.Forms.ListBox();
            this.buttonRunScript = new System.Windows.Forms.Button();
            this.textBoxScript = new System.Windows.Forms.TextBox();
            this.panelControls.SuspendLayout();
            this.panelRunScript.SuspendLayout();
            this.SuspendLayout();
            // 
            // panelControls
            // 
            this.panelControls.Controls.Add(this.richTextBoxOutput);
            this.panelControls.Controls.Add(this.panelRunScript);
            this.panelControls.Dock = System.Windows.Forms.DockStyle.Top;
            this.panelControls.Location = new System.Drawing.Point(0, 0);
            this.panelControls.Name = "panelControls";
            this.panelControls.Size = new System.Drawing.Size(682, 172);
            this.panelControls.TabIndex = 0;
            // 
            // richTextBoxOutput
            // 
            this.richTextBoxOutput.Dock = System.Windows.Forms.DockStyle.Fill;
            this.richTextBoxOutput.Location = new System.Drawing.Point(219, 0);
            this.richTextBoxOutput.Name = "richTextBoxOutput";
            this.richTextBoxOutput.Size = new System.Drawing.Size(463, 172);
            this.richTextBoxOutput.TabIndex = 1;
            this.richTextBoxOutput.Text = "";
            // 
            // panelRunScript
            // 
            this.panelRunScript.Controls.Add(this.listBoxScripts);
            this.panelRunScript.Controls.Add(this.buttonRunScript);
            this.panelRunScript.Dock = System.Windows.Forms.DockStyle.Left;
            this.panelRunScript.Location = new System.Drawing.Point(0, 0);
            this.panelRunScript.Name = "panelRunScript";
            this.panelRunScript.Size = new System.Drawing.Size(219, 172);
            this.panelRunScript.TabIndex = 0;
            // 
            // listBoxScripts
            // 
            this.listBoxScripts.FormattingEnabled = true;
            this.listBoxScripts.ItemHeight = 16;
            this.listBoxScripts.Location = new System.Drawing.Point(12, 40);
            this.listBoxScripts.Name = "listBoxScripts";
            this.listBoxScripts.Size = new System.Drawing.Size(194, 116);
            this.listBoxScripts.TabIndex = 1;
            this.listBoxScripts.SelectedIndexChanged += new System.EventHandler(this.listBoxScripts_SelectedIndexChanged);
            // 
            // buttonRunScript
            // 
            this.buttonRunScript.Location = new System.Drawing.Point(12, 11);
            this.buttonRunScript.Name = "buttonRunScript";
            this.buttonRunScript.Size = new System.Drawing.Size(194, 23);
            this.buttonRunScript.TabIndex = 0;
            this.buttonRunScript.Text = "Run Script";
            this.buttonRunScript.UseVisualStyleBackColor = true;
            this.buttonRunScript.Click += new System.EventHandler(this.buttonRunScript_Click);
            // 
            // textBoxScript
            // 
            this.textBoxScript.Dock = System.Windows.Forms.DockStyle.Fill;
            this.textBoxScript.Location = new System.Drawing.Point(0, 172);
            this.textBoxScript.Multiline = true;
            this.textBoxScript.Name = "textBoxScript";
            this.textBoxScript.ScrollBars = System.Windows.Forms.ScrollBars.Both;
            this.textBoxScript.Size = new System.Drawing.Size(682, 231);
            this.textBoxScript.TabIndex = 1;
            // 
            // Form1
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 16F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(682, 403);
            this.Controls.Add(this.textBoxScript);
            this.Controls.Add(this.panelControls);
            this.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.Name = "Form1";
            this.Text = "Example1 - NoUI";
            this.Load += new System.EventHandler(this.Form1_Load);
            this.panelControls.ResumeLayout(false);
            this.panelRunScript.ResumeLayout(false);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Panel panelControls;
        private System.Windows.Forms.Panel panelRunScript;
        private System.Windows.Forms.ListBox listBoxScripts;
        private System.Windows.Forms.Button buttonRunScript;
        private System.Windows.Forms.TextBox textBoxScript;
        private System.Windows.Forms.RichTextBox richTextBoxOutput;

    }
}

