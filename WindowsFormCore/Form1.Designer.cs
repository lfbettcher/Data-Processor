﻿namespace WindowsFormCore
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
            this.label1 = new System.Windows.Forms.Label();
            this.submitButton = new System.Windows.Forms.Button();
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
            this.skylineRadioButton.Location = new System.Drawing.Point(50, 90);
            this.skylineRadioButton.Name = "radioButton1";
            this.skylineRadioButton.Size = new System.Drawing.Size(133, 19);
            this.skylineRadioButton.TabIndex = 3;
            this.skylineRadioButton.TabStop = true;
            this.skylineRadioButton.Text = "Skyline export (.xlsx)";
            this.skylineRadioButton.UseVisualStyleBackColor = true;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(49, 57);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(83, 15);
            this.label1.TabIndex = 4;
            this.label1.Text = "Input file type:";
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
            // OpenFileForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 15F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(800, 450);
            this.Controls.Add(this.submitButton);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.skylineRadioButton);
            this.Controls.Add(this.fileNameBox);
            this.Controls.Add(this.filePathBox);
            this.Controls.Add(this.openFileButton);
            this.Name = "OpenFileForm";
            this.Text = "Open file";
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Button openFileButton;
        private System.Windows.Forms.TextBox filePathBox;
        private System.Windows.Forms.TextBox fileNameBox;
        private System.Windows.Forms.RadioButton skylineRadioButton;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Button submitButton;
    }
}

