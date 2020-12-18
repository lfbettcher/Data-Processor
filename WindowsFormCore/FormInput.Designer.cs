namespace WindowsFormCore
{
    partial class FormInput
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
            this.submitButton = new System.Windows.Forms.Button();
            this.groupBoxInputFile = new System.Windows.Forms.GroupBox();
            this.panelFileType = new System.Windows.Forms.Panel();
            this.label2 = new System.Windows.Forms.Label();
            this.multiquantTxtRadioButton = new System.Windows.Forms.RadioButton();
            this.excelRadioButton = new System.Windows.Forms.RadioButton();
            this.listBoxFiles = new System.Windows.Forms.ListBox();
            this.labelFile = new System.Windows.Forms.Label();
            this.groupBoxMissingValues = new System.Windows.Forms.GroupBox();
            this.replaceMissingValueCheckBox = new System.Windows.Forms.CheckBox();
            this.replaceMissingValueTextBox = new System.Windows.Forms.TextBox();
            this.label1 = new System.Windows.Forms.Label();
            this.missingValueBox = new System.Windows.Forms.TextBox();
            this.removeMissingCheckBox = new System.Windows.Forms.CheckBox();
            this.label3 = new System.Windows.Forms.Label();
            this.radioButton3 = new System.Windows.Forms.RadioButton();
            this.radioButton4 = new System.Windows.Forms.RadioButton();
            this.panel2 = new System.Windows.Forms.Panel();
            this.panel3 = new System.Windows.Forms.Panel();
            this.labelSamples = new System.Windows.Forms.Label();
            this.radioButton5 = new System.Windows.Forms.RadioButton();
            this.radioButton6 = new System.Windows.Forms.RadioButton();
            this.panel4 = new System.Windows.Forms.Panel();
            this.label5 = new System.Windows.Forms.Label();
            this.radioButton7 = new System.Windows.Forms.RadioButton();
            this.radioButton8 = new System.Windows.Forms.RadioButton();
            this.label6 = new System.Windows.Forms.Label();
            this.textBox1 = new System.Windows.Forms.TextBox();
            this.button1 = new System.Windows.Forms.Button();
            this.groupBoxOutputFile = new System.Windows.Forms.GroupBox();
            this.groupBoxInputFile.SuspendLayout();
            this.panelFileType.SuspendLayout();
            this.groupBoxMissingValues.SuspendLayout();
            this.panel2.SuspendLayout();
            this.groupBoxOutputFile.SuspendLayout();
            this.SuspendLayout();
            // 
            // openFileButton
            // 
            this.openFileButton.Location = new System.Drawing.Point(299, 88);
            this.openFileButton.Name = "openFileButton";
            this.openFileButton.Size = new System.Drawing.Size(70, 23);
            this.openFileButton.TabIndex = 0;
            this.openFileButton.Text = "Open File";
            this.openFileButton.UseVisualStyleBackColor = true;
            this.openFileButton.Click += new System.EventHandler(this.openFileButton_Click);
            // 
            // filePathBox
            // 
            this.filePathBox.Location = new System.Drawing.Point(500, 391);
            this.filePathBox.Name = "filePathBox";
            this.filePathBox.Size = new System.Drawing.Size(99, 23);
            this.filePathBox.TabIndex = 1;
            // 
            // fileNameBox
            // 
            this.fileNameBox.Location = new System.Drawing.Point(72, 88);
            this.fileNameBox.Name = "fileNameBox";
            this.fileNameBox.Size = new System.Drawing.Size(221, 23);
            this.fileNameBox.TabIndex = 2;
            // 
            // submitButton
            // 
            this.submitButton.Location = new System.Drawing.Point(682, 414);
            this.submitButton.Name = "submitButton";
            this.submitButton.Size = new System.Drawing.Size(90, 24);
            this.submitButton.TabIndex = 5;
            this.submitButton.Text = "Submit";
            this.submitButton.UseVisualStyleBackColor = true;
            this.submitButton.Click += new System.EventHandler(this.submitButton_Click);
            // 
            // groupBoxInputFile
            // 
            this.groupBoxInputFile.Controls.Add(this.panel2);
            this.groupBoxInputFile.Controls.Add(this.panelFileType);
            this.groupBoxInputFile.Controls.Add(this.listBoxFiles);
            this.groupBoxInputFile.Controls.Add(this.labelFile);
            this.groupBoxInputFile.Controls.Add(this.fileNameBox);
            this.groupBoxInputFile.Controls.Add(this.openFileButton);
            this.groupBoxInputFile.Location = new System.Drawing.Point(12, 12);
            this.groupBoxInputFile.Name = "groupBoxInputFile";
            this.groupBoxInputFile.Size = new System.Drawing.Size(375, 312);
            this.groupBoxInputFile.TabIndex = 6;
            this.groupBoxInputFile.TabStop = false;
            this.groupBoxInputFile.Text = "Input Data";
            // 
            // panelFileType
            // 
            this.panelFileType.Controls.Add(this.label2);
            this.panelFileType.Controls.Add(this.multiquantTxtRadioButton);
            this.panelFileType.Controls.Add(this.excelRadioButton);
            this.panelFileType.Location = new System.Drawing.Point(6, 21);
            this.panelFileType.Name = "panelFileType";
            this.panelFileType.Size = new System.Drawing.Size(363, 30);
            this.panelFileType.TabIndex = 12;
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(5, 8);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(55, 15);
            this.label2.TabIndex = 9;
            this.label2.Text = "File Type:";
            // 
            // multiquantTxtRadioButton
            // 
            this.multiquantTxtRadioButton.AutoSize = true;
            this.multiquantTxtRadioButton.Location = new System.Drawing.Point(171, 6);
            this.multiquantTxtRadioButton.Name = "multiquantTxtRadioButton";
            this.multiquantTxtRadioButton.Size = new System.Drawing.Size(172, 19);
            this.multiquantTxtRadioButton.TabIndex = 8;
            this.multiquantTxtRadioButton.TabStop = true;
            this.multiquantTxtRadioButton.Text = "MultiQuant Text File(s) (.txt)";
            this.multiquantTxtRadioButton.UseVisualStyleBackColor = true;
            // 
            // excelRadioButton
            // 
            this.excelRadioButton.AutoSize = true;
            this.excelRadioButton.Location = new System.Drawing.Point(79, 6);
            this.excelRadioButton.Name = "excelRadioButton";
            this.excelRadioButton.Size = new System.Drawing.Size(86, 19);
            this.excelRadioButton.TabIndex = 4;
            this.excelRadioButton.TabStop = true;
            this.excelRadioButton.Text = "Excel (.xlsx)";
            this.excelRadioButton.UseVisualStyleBackColor = true;
            // 
            // listBoxFiles
            // 
            this.listBoxFiles.FormattingEnabled = true;
            this.listBoxFiles.ItemHeight = 15;
            this.listBoxFiles.Location = new System.Drawing.Point(6, 153);
            this.listBoxFiles.Name = "listBoxFiles";
            this.listBoxFiles.Size = new System.Drawing.Size(363, 154);
            this.listBoxFiles.TabIndex = 10;
            // 
            // labelFile
            // 
            this.labelFile.AutoSize = true;
            this.labelFile.Location = new System.Drawing.Point(11, 91);
            this.labelFile.Name = "labelFile";
            this.labelFile.Size = new System.Drawing.Size(55, 15);
            this.labelFile.TabIndex = 9;
            this.labelFile.Text = "Data File:";
            // 
            // groupBoxMissingValues
            // 
            this.groupBoxMissingValues.Controls.Add(this.replaceMissingValueCheckBox);
            this.groupBoxMissingValues.Controls.Add(this.replaceMissingValueTextBox);
            this.groupBoxMissingValues.Controls.Add(this.label1);
            this.groupBoxMissingValues.Controls.Add(this.missingValueBox);
            this.groupBoxMissingValues.Controls.Add(this.removeMissingCheckBox);
            this.groupBoxMissingValues.Location = new System.Drawing.Point(435, 12);
            this.groupBoxMissingValues.Name = "groupBoxMissingValues";
            this.groupBoxMissingValues.Size = new System.Drawing.Size(337, 82);
            this.groupBoxMissingValues.TabIndex = 7;
            this.groupBoxMissingValues.TabStop = false;
            this.groupBoxMissingValues.Text = "Missing values";
            // 
            // replaceMissingValueCheckBox
            // 
            this.replaceMissingValueCheckBox.AutoSize = true;
            this.replaceMissingValueCheckBox.Location = new System.Drawing.Point(17, 50);
            this.replaceMissingValueCheckBox.Name = "replaceMissingValueCheckBox";
            this.replaceMissingValueCheckBox.Size = new System.Drawing.Size(230, 19);
            this.replaceMissingValueCheckBox.TabIndex = 5;
            this.replaceMissingValueCheckBox.Text = "Replace remaining missing values with";
            this.replaceMissingValueCheckBox.UseVisualStyleBackColor = true;
            // 
            // replaceMissingValueTextBox
            // 
            this.replaceMissingValueTextBox.Location = new System.Drawing.Point(253, 48);
            this.replaceMissingValueTextBox.Name = "replaceMissingValueTextBox";
            this.replaceMissingValueTextBox.PlaceholderText = "0";
            this.replaceMissingValueTextBox.Size = new System.Drawing.Size(50, 23);
            this.replaceMissingValueTextBox.TabIndex = 4;
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
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(5, 8);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(67, 15);
            this.label3.TabIndex = 9;
            this.label3.Text = "Samples in:";
            // 
            // radioButton3
            // 
            this.radioButton3.AutoSize = true;
            this.radioButton3.Location = new System.Drawing.Point(171, 6);
            this.radioButton3.Name = "radioButton3";
            this.radioButton3.Size = new System.Drawing.Size(73, 19);
            this.radioButton3.TabIndex = 8;
            this.radioButton3.TabStop = true;
            this.radioButton3.Text = "Columns";
            this.radioButton3.UseVisualStyleBackColor = true;
            // 
            // radioButton4
            // 
            this.radioButton4.AutoSize = true;
            this.radioButton4.Location = new System.Drawing.Point(79, 6);
            this.radioButton4.Name = "radioButton4";
            this.radioButton4.Size = new System.Drawing.Size(53, 19);
            this.radioButton4.TabIndex = 4;
            this.radioButton4.TabStop = true;
            this.radioButton4.Text = "Rows";
            this.radioButton4.UseVisualStyleBackColor = true;
            // 
            // panel2
            // 
            this.panel2.Controls.Add(this.label3);
            this.panel2.Controls.Add(this.radioButton3);
            this.panel2.Controls.Add(this.radioButton4);
            this.panel2.Location = new System.Drawing.Point(6, 52);
            this.panel2.Name = "panel2";
            this.panel2.Size = new System.Drawing.Size(363, 30);
            this.panel2.TabIndex = 12;
            // 
            // panel3
            // 
            this.panel3.Location = new System.Drawing.Point(6, 52);
            this.panel3.Name = "panel3";
            this.panel3.Size = new System.Drawing.Size(363, 30);
            this.panel3.TabIndex = 12;
            // 
            // labelSamples
            // 
            this.labelSamples.AutoSize = true;
            this.labelSamples.Location = new System.Drawing.Point(5, 8);
            this.labelSamples.Name = "labelSamples";
            this.labelSamples.Size = new System.Drawing.Size(67, 15);
            this.labelSamples.TabIndex = 9;
            this.labelSamples.Text = "Samples in:";
            // 
            // radioButton5
            // 
            this.radioButton5.AutoSize = true;
            this.radioButton5.Location = new System.Drawing.Point(171, 6);
            this.radioButton5.Name = "radioButton5";
            this.radioButton5.Size = new System.Drawing.Size(73, 19);
            this.radioButton5.TabIndex = 8;
            this.radioButton5.TabStop = true;
            this.radioButton5.Text = "Columns";
            this.radioButton5.UseVisualStyleBackColor = true;
            // 
            // radioButton6
            // 
            this.radioButton6.AutoSize = true;
            this.radioButton6.Location = new System.Drawing.Point(79, 6);
            this.radioButton6.Name = "radioButton6";
            this.radioButton6.Size = new System.Drawing.Size(53, 19);
            this.radioButton6.TabIndex = 4;
            this.radioButton6.TabStop = true;
            this.radioButton6.Text = "Rows";
            this.radioButton6.UseVisualStyleBackColor = true;
            // 
            // panel4
            // 
            this.panel4.Location = new System.Drawing.Point(6, 21);
            this.panel4.Name = "panel4";
            this.panel4.Size = new System.Drawing.Size(363, 30);
            this.panel4.TabIndex = 12;
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Location = new System.Drawing.Point(5, 8);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(55, 15);
            this.label5.TabIndex = 9;
            this.label5.Text = "File Type:";
            // 
            // radioButton7
            // 
            this.radioButton7.AutoSize = true;
            this.radioButton7.Location = new System.Drawing.Point(171, 6);
            this.radioButton7.Name = "radioButton7";
            this.radioButton7.Size = new System.Drawing.Size(172, 19);
            this.radioButton7.TabIndex = 8;
            this.radioButton7.TabStop = true;
            this.radioButton7.Text = "MultiQuant Text File(s) (.txt)";
            this.radioButton7.UseVisualStyleBackColor = true;
            // 
            // radioButton8
            // 
            this.radioButton8.AutoSize = true;
            this.radioButton8.Location = new System.Drawing.Point(79, 6);
            this.radioButton8.Name = "radioButton8";
            this.radioButton8.Size = new System.Drawing.Size(86, 19);
            this.radioButton8.TabIndex = 4;
            this.radioButton8.TabStop = true;
            this.radioButton8.Text = "Excel (.xlsx)";
            this.radioButton8.UseVisualStyleBackColor = true;
            // 
            // label6
            // 
            this.label6.AutoSize = true;
            this.label6.Location = new System.Drawing.Point(11, 91);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(55, 15);
            this.label6.TabIndex = 9;
            this.label6.Text = "Data File:";
            // 
            // textBox1
            // 
            this.textBox1.Location = new System.Drawing.Point(72, 88);
            this.textBox1.Name = "textBox1";
            this.textBox1.Size = new System.Drawing.Size(221, 23);
            this.textBox1.TabIndex = 2;
            // 
            // button1
            // 
            this.button1.Location = new System.Drawing.Point(299, 88);
            this.button1.Name = "button1";
            this.button1.Size = new System.Drawing.Size(70, 23);
            this.button1.TabIndex = 0;
            this.button1.Text = "Open File";
            this.button1.UseVisualStyleBackColor = true;
            // 
            // groupBoxOutputFile
            // 
            this.groupBoxOutputFile.Controls.Add(this.panel3);
            this.groupBoxOutputFile.Controls.Add(this.panel4);
            this.groupBoxOutputFile.Controls.Add(this.label6);
            this.groupBoxOutputFile.Controls.Add(this.textBox1);
            this.groupBoxOutputFile.Controls.Add(this.button1);
            this.groupBoxOutputFile.Location = new System.Drawing.Point(12, 340);
            this.groupBoxOutputFile.Name = "groupBoxOutputFile";
            this.groupBoxOutputFile.Size = new System.Drawing.Size(375, 120);
            this.groupBoxOutputFile.TabIndex = 6;
            this.groupBoxOutputFile.TabStop = false;
            this.groupBoxOutputFile.Text = "Output File";
            // 
            // FormInput
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 15F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(800, 472);
            this.Controls.Add(this.groupBoxOutputFile);
            this.Controls.Add(this.groupBoxMissingValues);
            this.Controls.Add(this.groupBoxInputFile);
            this.Controls.Add(this.submitButton);
            this.Controls.Add(this.filePathBox);
            this.Name = "FormInput";
            this.Text = "Open file";
            this.groupBoxInputFile.ResumeLayout(false);
            this.groupBoxInputFile.PerformLayout();
            this.panelFileType.ResumeLayout(false);
            this.panelFileType.PerformLayout();
            this.groupBoxMissingValues.ResumeLayout(false);
            this.groupBoxMissingValues.PerformLayout();
            this.panel2.ResumeLayout(false);
            this.panel2.PerformLayout();
            this.groupBoxOutputFile.ResumeLayout(false);
            this.groupBoxOutputFile.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Button openFileButton;
        private System.Windows.Forms.TextBox filePathBox;
        private System.Windows.Forms.TextBox fileNameBox;
        private System.Windows.Forms.Button submitButton;
        private System.Windows.Forms.GroupBox groupBoxInputFile;
        private System.Windows.Forms.RadioButton excelRadioButton;
        private System.Windows.Forms.GroupBox groupBoxMissingValues;
        private System.Windows.Forms.TextBox replaceMissingValueTextBox;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.TextBox missingValueBox;
        private System.Windows.Forms.CheckBox removeMissingCheckBox;
        private System.Windows.Forms.CheckBox replaceMissingValueCheckBox;
        private System.Windows.Forms.RadioButton multiquantTxtRadioButton;
        private System.Windows.Forms.ListBox listBoxFiles;
        private System.Windows.Forms.Label labelFile;
        private System.Windows.Forms.Panel panelFileType;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Panel panel2;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.RadioButton radioButton3;
        private System.Windows.Forms.RadioButton radioButton4;
        private System.Windows.Forms.Panel panel3;
        private System.Windows.Forms.Label labelSamples;
        private System.Windows.Forms.RadioButton radioButton5;
        private System.Windows.Forms.RadioButton radioButton6;
        private System.Windows.Forms.Panel panel4;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.RadioButton radioButton7;
        private System.Windows.Forms.RadioButton radioButton8;
        private System.Windows.Forms.Label label6;
        private System.Windows.Forms.TextBox textBox1;
        private System.Windows.Forms.Button button1;
        private System.Windows.Forms.GroupBox groupBoxOutputFile;
    }
}