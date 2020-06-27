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
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.radioButton2 = new System.Windows.Forms.RadioButton();
            this.radioButton1 = new System.Windows.Forms.RadioButton();
            this.groupBox2 = new System.Windows.Forms.GroupBox();
            this.replaceMissingValueTextBox = new System.Windows.Forms.TextBox();
            this.label2 = new System.Windows.Forms.Label();
            this.label1 = new System.Windows.Forms.Label();
            this.missingValueBox = new System.Windows.Forms.TextBox();
            this.removeMissingCheckBox = new System.Windows.Forms.CheckBox();
            this.groupBox1.SuspendLayout();
            this.groupBox2.SuspendLayout();
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
            this.skylineRadioButton.Location = new System.Drawing.Point(17, 22);
            this.skylineRadioButton.Name = "skylineRadioButton";
            this.skylineRadioButton.Size = new System.Drawing.Size(130, 19);
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
            // groupBox1
            // 
            this.groupBox1.Controls.Add(this.radioButton2);
            this.groupBox1.Controls.Add(this.radioButton1);
            this.groupBox1.Controls.Add(this.skylineRadioButton);
            this.groupBox1.Location = new System.Drawing.Point(36, 52);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(411, 55);
            this.groupBox1.TabIndex = 6;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "Input type:";
            // 
            // radioButton2
            // 
            this.radioButton2.AutoSize = true;
            this.radioButton2.Location = new System.Drawing.Point(299, 22);
            this.radioButton2.Name = "radioButton2";
            this.radioButton2.Size = new System.Drawing.Size(94, 19);
            this.radioButton2.TabIndex = 5;
            this.radioButton2.TabStop = true;
            this.radioButton2.Text = "radioButton2";
            this.radioButton2.UseVisualStyleBackColor = true;
            // 
            // radioButton1
            // 
            this.radioButton1.AutoSize = true;
            this.radioButton1.Location = new System.Drawing.Point(169, 22);
            this.radioButton1.Name = "radioButton1";
            this.radioButton1.Size = new System.Drawing.Size(94, 19);
            this.radioButton1.TabIndex = 4;
            this.radioButton1.TabStop = true;
            this.radioButton1.Text = "radioButton1";
            this.radioButton1.UseVisualStyleBackColor = true;
            // 
            // groupBox2
            // 
            this.groupBox2.Controls.Add(this.replaceMissingValueTextBox);
            this.groupBox2.Controls.Add(this.label2);
            this.groupBox2.Controls.Add(this.label1);
            this.groupBox2.Controls.Add(this.missingValueBox);
            this.groupBox2.Controls.Add(this.removeMissingCheckBox);
            this.groupBox2.Location = new System.Drawing.Point(36, 122);
            this.groupBox2.Name = "groupBox2";
            this.groupBox2.Size = new System.Drawing.Size(411, 82);
            this.groupBox2.TabIndex = 7;
            this.groupBox2.TabStop = false;
            this.groupBox2.Text = "Missing values";
            // 
            // replaceMissingValueTextBox
            // 
            this.replaceMissingValueTextBox.Location = new System.Drawing.Point(234, 48);
            this.replaceMissingValueTextBox.Name = "replaceMissingValueTextBox";
            this.replaceMissingValueTextBox.PlaceholderText = "0";
            this.replaceMissingValueTextBox.Size = new System.Drawing.Size(69, 23);
            this.replaceMissingValueTextBox.TabIndex = 4;
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(17, 51);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(211, 15);
            this.label2.TabIndex = 3;
            this.label2.Text = "Replace remaining missing values with";
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(206, 24);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(97, 15);
            this.label1.TabIndex = 2;
            this.label1.Text = "% missing values";
            // 
            // missingValueBox
            // 
            this.missingValueBox.Location = new System.Drawing.Point(165, 21);
            this.missingValueBox.Name = "missingValueBox";
            this.missingValueBox.PlaceholderText = "20";
            this.missingValueBox.Size = new System.Drawing.Size(35, 23);
            this.missingValueBox.TabIndex = 1;
            this.missingValueBox.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            // 
            // removeMissingCheckBox
            // 
            this.removeMissingCheckBox.AutoSize = true;
            this.removeMissingCheckBox.Checked = true;
            this.removeMissingCheckBox.CheckState = System.Windows.Forms.CheckState.Checked;
            this.removeMissingCheckBox.Location = new System.Drawing.Point(17, 23);
            this.removeMissingCheckBox.Name = "removeMissingCheckBox";
            this.removeMissingCheckBox.Size = new System.Drawing.Size(151, 19);
            this.removeMissingCheckBox.TabIndex = 0;
            this.removeMissingCheckBox.Text = "Remove features with >";
            this.removeMissingCheckBox.UseVisualStyleBackColor = true;
            // 
            // OpenFileForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 15F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(800, 450);
            this.Controls.Add(this.groupBox2);
            this.Controls.Add(this.groupBox1);
            this.Controls.Add(this.submitButton);
            this.Controls.Add(this.fileNameBox);
            this.Controls.Add(this.filePathBox);
            this.Controls.Add(this.openFileButton);
            this.Name = "OpenFileForm";
            this.Text = "Open file";
            this.groupBox1.ResumeLayout(false);
            this.groupBox1.PerformLayout();
            this.groupBox2.ResumeLayout(false);
            this.groupBox2.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Button openFileButton;
        private System.Windows.Forms.TextBox filePathBox;
        private System.Windows.Forms.TextBox fileNameBox;
        private System.Windows.Forms.RadioButton skylineRadioButton;
        private System.Windows.Forms.Button submitButton;
        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.RadioButton radioButton2;
        private System.Windows.Forms.RadioButton radioButton1;
        private System.Windows.Forms.GroupBox groupBox2;
        private System.Windows.Forms.TextBox replaceMissingValueTextBox;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.TextBox missingValueBox;
        private System.Windows.Forms.CheckBox removeMissingCheckBox;
    }
}

