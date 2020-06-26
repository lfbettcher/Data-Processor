namespace WindowsFormCore
{
    partial class OpenFileForm
    {
        /// <summary>
        ///  Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        ///  Clean up any resources being used.
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
        ///  Required method for Designer support - do not modify
        ///  the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.openFileButton = new System.Windows.Forms.Button();
            this.filePathBox = new System.Windows.Forms.TextBox();
            this.fileNameBox = new System.Windows.Forms.TextBox();
            this.skylineRadioButton = new System.Windows.Forms.RadioButton();
            this.submitButton = new System.Windows.Forms.Button();
            this.radioButton1 = new System.Windows.Forms.RadioButton();
            this.radioButton2 = new System.Windows.Forms.RadioButton();
            this.inputTypeGroupBox = new System.Windows.Forms.GroupBox();
            this.inputTypeGroupBox.SuspendLayout();
            this.SuspendLayout();
            // 
            // openFileButton
            // 
            this.openFileButton.Location = new System.Drawing.Point(261, 322);
            this.openFileButton.Name = "openFileButton";
            this.openFileButton.Size = new System.Drawing.Size(90, 23);
            this.openFileButton.TabIndex = 0;
            this.openFileButton.Text = "Open file";
            this.openFileButton.UseVisualStyleBackColor = true;
            this.openFileButton.Click += new System.EventHandler(this.openFileButton_Click);
            // 
            // filePathBox
            // 
            this.filePathBox.Location = new System.Drawing.Point(36, 351);
            this.filePathBox.Name = "filePathBox";
            this.filePathBox.Size = new System.Drawing.Size(411, 23);
            this.filePathBox.TabIndex = 1;
            // 
            // fileNameBox
            // 
            this.fileNameBox.Location = new System.Drawing.Point(36, 322);
            this.fileNameBox.Name = "fileNameBox";
            this.fileNameBox.Size = new System.Drawing.Size(219, 23);
            this.fileNameBox.TabIndex = 2;
            // 
            // skylineRadioButton
            // 
            this.skylineRadioButton.AutoSize = true;
            this.skylineRadioButton.Checked = true;
            this.skylineRadioButton.Location = new System.Drawing.Point(19, 22);
            this.skylineRadioButton.Name = "skylineRadioButton";
            this.skylineRadioButton.Size = new System.Drawing.Size(133, 19);
            this.skylineRadioButton.TabIndex = 3;
            this.skylineRadioButton.TabStop = true;
            this.skylineRadioButton.Text = "Skyline export (.xlsx)";
            this.skylineRadioButton.UseVisualStyleBackColor = true;
            // 
            // submitButton
            // 
            this.submitButton.Location = new System.Drawing.Point(357, 322);
            this.submitButton.Name = "submitButton";
            this.submitButton.Size = new System.Drawing.Size(90, 24);
            this.submitButton.TabIndex = 5;
            this.submitButton.Text = "Submit";
            this.submitButton.UseVisualStyleBackColor = true;
            this.submitButton.Click += new System.EventHandler(this.submitButton_Click);
            // 
            // radioButton1
            // 
            this.radioButton1.AutoSize = true;
            this.radioButton1.Location = new System.Drawing.Point(181, 22);
            this.radioButton1.Name = "radioButton1";
            this.radioButton1.Size = new System.Drawing.Size(94, 19);
            this.radioButton1.TabIndex = 6;
            this.radioButton1.TabStop = true;
            this.radioButton1.Text = "radioButton1";
            this.radioButton1.UseVisualStyleBackColor = true;
            // 
            // radioButton2
            // 
            this.radioButton2.AutoSize = true;
            this.radioButton2.Location = new System.Drawing.Point(311, 22);
            this.radioButton2.Name = "radioButton2";
            this.radioButton2.Size = new System.Drawing.Size(94, 19);
            this.radioButton2.TabIndex = 7;
            this.radioButton2.TabStop = true;
            this.radioButton2.Text = "radioButton2";
            this.radioButton2.UseVisualStyleBackColor = true;
            // 
            // groupBox1
            // 
            this.inputTypeGroupBox.Controls.Add(this.radioButton2);
            this.inputTypeGroupBox.Controls.Add(this.radioButton1);
            this.inputTypeGroupBox.Controls.Add(this.skylineRadioButton);
            this.inputTypeGroupBox.Location = new System.Drawing.Point(36, 57);
            this.inputTypeGroupBox.Name = "groupBox1";
            this.inputTypeGroupBox.Size = new System.Drawing.Size(411, 61);
            this.inputTypeGroupBox.TabIndex = 8;
            this.inputTypeGroupBox.TabStop = false;
            this.inputTypeGroupBox.Text = "Input type:";
            // 
            // OpenFileForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 15F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(800, 450);
            this.Controls.Add(this.inputTypeGroupBox);
            this.Controls.Add(this.submitButton);
            this.Controls.Add(this.fileNameBox);
            this.Controls.Add(this.filePathBox);
            this.Controls.Add(this.openFileButton);
            this.Name = "OpenFileForm";
            this.Text = "Open file";
            this.inputTypeGroupBox.ResumeLayout(false);
            this.inputTypeGroupBox.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Button openFileButton;
        private System.Windows.Forms.TextBox filePathBox;
        private System.Windows.Forms.TextBox fileNameBox;
        private System.Windows.Forms.RadioButton skylineRadioButton;
        private System.Windows.Forms.Button submitButton;
        private System.Windows.Forms.RadioButton radioButton1;
        private System.Windows.Forms.RadioButton radioButton2;
        private System.Windows.Forms.GroupBox inputTypeGroupBox;
    }
}

