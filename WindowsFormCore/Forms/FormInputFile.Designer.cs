namespace WindowsFormCore.Forms
{
    partial class FormInputFile
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
            this.buttonOpenFile = new System.Windows.Forms.Button();
            this.filePathBox = new System.Windows.Forms.TextBox();
            this.textBoxFileName = new System.Windows.Forms.TextBox();
            this.submitButton = new System.Windows.Forms.Button();
            this.groupBoxInputFile = new System.Windows.Forms.GroupBox();
            this.listBoxFiles = new System.Windows.Forms.ListBox();
            this.labelDataFile = new System.Windows.Forms.Label();
            this.panelSamplesIn = new System.Windows.Forms.Panel();
            this.labelSamplesIn = new System.Windows.Forms.Label();
            this.radioButtonColumnsIn = new System.Windows.Forms.RadioButton();
            this.radioButtonRowsIn = new System.Windows.Forms.RadioButton();
            this.panelFileType = new System.Windows.Forms.Panel();
            this.labelFileType = new System.Windows.Forms.Label();
            this.radioButtonMultiQuant = new System.Windows.Forms.RadioButton();
            this.radioButtonExcel = new System.Windows.Forms.RadioButton();
            this.labelOutputFileName = new System.Windows.Forms.Label();
            this.textBoxOutputFileName = new System.Windows.Forms.TextBox();
            this.buttonSelectOutputFolder = new System.Windows.Forms.Button();
            this.groupBoxOutputFile = new System.Windows.Forms.GroupBox();
            this.panelSamplesOut = new System.Windows.Forms.Panel();
            this.labelSamplesOut = new System.Windows.Forms.Label();
            this.radioButtonColumnsOut = new System.Windows.Forms.RadioButton();
            this.radioButtonRowsOut = new System.Windows.Forms.RadioButton();
            this.labelOutputFolder = new System.Windows.Forms.Label();
            this.textBoxOutputFolder = new System.Windows.Forms.TextBox();
            this.panelInput = new System.Windows.Forms.Panel();
            this.tableLayoutPanel1 = new System.Windows.Forms.TableLayoutPanel();
            this.panelInputFileBox = new System.Windows.Forms.Panel();
            this.label1 = new System.Windows.Forms.Label();
            this.groupBoxInputFile.SuspendLayout();
            this.panelSamplesIn.SuspendLayout();
            this.panelFileType.SuspendLayout();
            this.groupBoxOutputFile.SuspendLayout();
            this.panelSamplesOut.SuspendLayout();
            this.panelInput.SuspendLayout();
            this.tableLayoutPanel1.SuspendLayout();
            this.panelInputFileBox.SuspendLayout();
            this.SuspendLayout();
            // 
            // buttonOpenFile
            // 
            this.buttonOpenFile.BackColor = System.Drawing.Color.Teal;
            this.buttonOpenFile.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.buttonOpenFile.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.buttonOpenFile.Font = new System.Drawing.Font("Segoe UI Semibold", 10F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point);
            this.buttonOpenFile.ForeColor = System.Drawing.Color.White;
            this.buttonOpenFile.Location = new System.Drawing.Point(3, 3);
            this.buttonOpenFile.Name = "buttonOpenFile";
            this.buttonOpenFile.Size = new System.Drawing.Size(108, 30);
            this.buttonOpenFile.TabIndex = 0;
            this.buttonOpenFile.Text = "Select File(s)";
            this.buttonOpenFile.UseVisualStyleBackColor = false;
            this.buttonOpenFile.Click += new System.EventHandler(this.openFileButton_Click);
            // 
            // filePathBox
            // 
            this.filePathBox.Location = new System.Drawing.Point(500, 391);
            this.filePathBox.Name = "filePathBox";
            this.filePathBox.Size = new System.Drawing.Size(99, 23);
            this.filePathBox.TabIndex = 1;
            // 
            // textBoxFileName
            // 
            this.textBoxFileName.AllowDrop = true;
            this.textBoxFileName.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left)));
            this.textBoxFileName.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.textBoxFileName.Font = new System.Drawing.Font("Segoe UI", 10F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point);
            this.textBoxFileName.Location = new System.Drawing.Point(9, 6);
            this.textBoxFileName.Name = "textBoxFileName";
            this.textBoxFileName.ShortcutsEnabled = false;
            this.textBoxFileName.Size = new System.Drawing.Size(253, 18);
            this.textBoxFileName.TabIndex = 2;
            this.textBoxFileName.WordWrap = false;
            // 
            // submitButton
            // 
            this.submitButton.Location = new System.Drawing.Point(520, 427);
            this.submitButton.Name = "submitButton";
            this.submitButton.Size = new System.Drawing.Size(90, 24);
            this.submitButton.TabIndex = 5;
            this.submitButton.Text = "Submit";
            this.submitButton.UseVisualStyleBackColor = true;
            this.submitButton.Click += new System.EventHandler(this.submitButton_Click);
            // 
            // groupBoxInputFile
            // 
            this.groupBoxInputFile.Controls.Add(this.listBoxFiles);
            this.groupBoxInputFile.Controls.Add(this.labelDataFile);
            this.groupBoxInputFile.Location = new System.Drawing.Point(12, 12);
            this.groupBoxInputFile.Name = "groupBoxInputFile";
            this.groupBoxInputFile.Size = new System.Drawing.Size(375, 312);
            this.groupBoxInputFile.TabIndex = 6;
            this.groupBoxInputFile.TabStop = false;
            this.groupBoxInputFile.Text = "Input Data";
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
            // labelDataFile
            // 
            this.labelDataFile.AutoSize = true;
            this.labelDataFile.Location = new System.Drawing.Point(11, 92);
            this.labelDataFile.Name = "labelDataFile";
            this.labelDataFile.Size = new System.Drawing.Size(55, 15);
            this.labelDataFile.TabIndex = 9;
            this.labelDataFile.Text = "Data File:";
            // 
            // panelSamplesIn
            // 
            this.panelSamplesIn.Controls.Add(this.labelSamplesIn);
            this.panelSamplesIn.Controls.Add(this.radioButtonColumnsIn);
            this.panelSamplesIn.Controls.Add(this.radioButtonRowsIn);
            this.panelSamplesIn.Dock = System.Windows.Forms.DockStyle.Top;
            this.panelSamplesIn.Location = new System.Drawing.Point(0, 56);
            this.panelSamplesIn.Name = "panelSamplesIn";
            this.panelSamplesIn.Size = new System.Drawing.Size(395, 36);
            this.panelSamplesIn.TabIndex = 12;
            // 
            // labelSamplesIn
            // 
            this.labelSamplesIn.AutoSize = true;
            this.labelSamplesIn.Location = new System.Drawing.Point(5, 8);
            this.labelSamplesIn.Name = "labelSamplesIn";
            this.labelSamplesIn.Size = new System.Drawing.Size(77, 19);
            this.labelSamplesIn.TabIndex = 9;
            this.labelSamplesIn.Text = "Samples in:";
            // 
            // radioButtonColumnsIn
            // 
            this.radioButtonColumnsIn.AutoSize = true;
            this.radioButtonColumnsIn.Location = new System.Drawing.Point(177, 6);
            this.radioButtonColumnsIn.Name = "radioButtonColumnsIn";
            this.radioButtonColumnsIn.Size = new System.Drawing.Size(81, 23);
            this.radioButtonColumnsIn.TabIndex = 8;
            this.radioButtonColumnsIn.TabStop = true;
            this.radioButtonColumnsIn.Text = "Columns";
            this.radioButtonColumnsIn.UseVisualStyleBackColor = true;
            // 
            // radioButtonRowsIn
            // 
            this.radioButtonRowsIn.AutoSize = true;
            this.radioButtonRowsIn.Location = new System.Drawing.Point(85, 6);
            this.radioButtonRowsIn.Name = "radioButtonRowsIn";
            this.radioButtonRowsIn.Size = new System.Drawing.Size(59, 23);
            this.radioButtonRowsIn.TabIndex = 4;
            this.radioButtonRowsIn.TabStop = true;
            this.radioButtonRowsIn.Text = "Rows";
            this.radioButtonRowsIn.UseVisualStyleBackColor = true;
            // 
            // panelFileType
            // 
            this.panelFileType.Controls.Add(this.labelFileType);
            this.panelFileType.Controls.Add(this.radioButtonMultiQuant);
            this.panelFileType.Controls.Add(this.radioButtonExcel);
            this.panelFileType.Dock = System.Windows.Forms.DockStyle.Top;
            this.panelFileType.Font = new System.Drawing.Font("Segoe UI", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point);
            this.panelFileType.Location = new System.Drawing.Point(0, 20);
            this.panelFileType.Name = "panelFileType";
            this.panelFileType.Size = new System.Drawing.Size(395, 36);
            this.panelFileType.TabIndex = 12;
            // 
            // labelFileType
            // 
            this.labelFileType.AutoSize = true;
            this.labelFileType.Location = new System.Drawing.Point(5, 8);
            this.labelFileType.Name = "labelFileType";
            this.labelFileType.Size = new System.Drawing.Size(61, 17);
            this.labelFileType.TabIndex = 9;
            this.labelFileType.Text = "File Type:";
            // 
            // radioButtonMultiQuant
            // 
            this.radioButtonMultiQuant.AutoSize = true;
            this.radioButtonMultiQuant.Location = new System.Drawing.Point(177, 6);
            this.radioButtonMultiQuant.Name = "radioButtonMultiQuant";
            this.radioButtonMultiQuant.Size = new System.Drawing.Size(183, 21);
            this.radioButtonMultiQuant.TabIndex = 8;
            this.radioButtonMultiQuant.TabStop = true;
            this.radioButtonMultiQuant.Text = "MultiQuant Text File(s) (.txt)";
            this.radioButtonMultiQuant.UseVisualStyleBackColor = true;
            // 
            // radioButtonExcel
            // 
            this.radioButtonExcel.AutoSize = true;
            this.radioButtonExcel.Location = new System.Drawing.Point(85, 6);
            this.radioButtonExcel.Name = "radioButtonExcel";
            this.radioButtonExcel.Size = new System.Drawing.Size(91, 21);
            this.radioButtonExcel.TabIndex = 4;
            this.radioButtonExcel.TabStop = true;
            this.radioButtonExcel.Text = "Excel (.xlsx)";
            this.radioButtonExcel.UseVisualStyleBackColor = true;
            // 
            // labelOutputFileName
            // 
            this.labelOutputFileName.AutoSize = true;
            this.labelOutputFileName.Location = new System.Drawing.Point(11, 63);
            this.labelOutputFileName.Name = "labelOutputFileName";
            this.labelOutputFileName.Size = new System.Drawing.Size(63, 15);
            this.labelOutputFileName.TabIndex = 9;
            this.labelOutputFileName.Text = "File Name:";
            // 
            // textBoxOutputFileName
            // 
            this.textBoxOutputFileName.Location = new System.Drawing.Point(80, 60);
            this.textBoxOutputFileName.Name = "textBoxOutputFileName";
            this.textBoxOutputFileName.Size = new System.Drawing.Size(201, 23);
            this.textBoxOutputFileName.TabIndex = 2;
            // 
            // buttonSelectOutputFolder
            // 
            this.buttonSelectOutputFolder.Location = new System.Drawing.Point(287, 60);
            this.buttonSelectOutputFolder.Name = "buttonSelectOutputFolder";
            this.buttonSelectOutputFolder.Size = new System.Drawing.Size(82, 23);
            this.buttonSelectOutputFolder.TabIndex = 0;
            this.buttonSelectOutputFolder.Text = "Select Folder";
            this.buttonSelectOutputFolder.UseVisualStyleBackColor = true;
            // 
            // groupBoxOutputFile
            // 
            this.groupBoxOutputFile.Controls.Add(this.panelSamplesOut);
            this.groupBoxOutputFile.Controls.Add(this.labelOutputFolder);
            this.groupBoxOutputFile.Controls.Add(this.textBoxOutputFolder);
            this.groupBoxOutputFile.Controls.Add(this.labelOutputFileName);
            this.groupBoxOutputFile.Controls.Add(this.textBoxOutputFileName);
            this.groupBoxOutputFile.Controls.Add(this.buttonSelectOutputFolder);
            this.groupBoxOutputFile.Location = new System.Drawing.Point(12, 340);
            this.groupBoxOutputFile.Name = "groupBoxOutputFile";
            this.groupBoxOutputFile.Size = new System.Drawing.Size(375, 120);
            this.groupBoxOutputFile.TabIndex = 6;
            this.groupBoxOutputFile.TabStop = false;
            this.groupBoxOutputFile.Text = "Output File";
            // 
            // panelSamplesOut
            // 
            this.panelSamplesOut.Controls.Add(this.labelSamplesOut);
            this.panelSamplesOut.Controls.Add(this.radioButtonColumnsOut);
            this.panelSamplesOut.Controls.Add(this.radioButtonRowsOut);
            this.panelSamplesOut.Location = new System.Drawing.Point(6, 22);
            this.panelSamplesOut.Name = "panelSamplesOut";
            this.panelSamplesOut.Size = new System.Drawing.Size(363, 30);
            this.panelSamplesOut.TabIndex = 12;
            // 
            // labelSamplesOut
            // 
            this.labelSamplesOut.AutoSize = true;
            this.labelSamplesOut.Location = new System.Drawing.Point(5, 8);
            this.labelSamplesOut.Name = "labelSamplesOut";
            this.labelSamplesOut.Size = new System.Drawing.Size(67, 15);
            this.labelSamplesOut.TabIndex = 9;
            this.labelSamplesOut.Text = "Samples in:";
            // 
            // radioButtonColumnsOut
            // 
            this.radioButtonColumnsOut.AutoSize = true;
            this.radioButtonColumnsOut.Location = new System.Drawing.Point(171, 6);
            this.radioButtonColumnsOut.Name = "radioButtonColumnsOut";
            this.radioButtonColumnsOut.Size = new System.Drawing.Size(73, 19);
            this.radioButtonColumnsOut.TabIndex = 8;
            this.radioButtonColumnsOut.TabStop = true;
            this.radioButtonColumnsOut.Text = "Columns";
            this.radioButtonColumnsOut.UseVisualStyleBackColor = true;
            // 
            // radioButtonRowsOut
            // 
            this.radioButtonRowsOut.AutoSize = true;
            this.radioButtonRowsOut.Location = new System.Drawing.Point(79, 6);
            this.radioButtonRowsOut.Name = "radioButtonRowsOut";
            this.radioButtonRowsOut.Size = new System.Drawing.Size(53, 19);
            this.radioButtonRowsOut.TabIndex = 4;
            this.radioButtonRowsOut.TabStop = true;
            this.radioButtonRowsOut.Text = "Rows";
            this.radioButtonRowsOut.UseVisualStyleBackColor = true;
            // 
            // labelOutputFolder
            // 
            this.labelOutputFolder.AutoSize = true;
            this.labelOutputFolder.Location = new System.Drawing.Point(11, 92);
            this.labelOutputFolder.Name = "labelOutputFolder";
            this.labelOutputFolder.Size = new System.Drawing.Size(43, 15);
            this.labelOutputFolder.TabIndex = 9;
            this.labelOutputFolder.Text = "Folder:";
            // 
            // textBoxOutputFolder
            // 
            this.textBoxOutputFolder.Location = new System.Drawing.Point(80, 89);
            this.textBoxOutputFolder.Name = "textBoxOutputFolder";
            this.textBoxOutputFolder.Size = new System.Drawing.Size(289, 23);
            this.textBoxOutputFolder.TabIndex = 2;
            // 
            // panelInput
            // 
            this.panelInput.Controls.Add(this.tableLayoutPanel1);
            this.panelInput.Controls.Add(this.panelSamplesIn);
            this.panelInput.Controls.Add(this.panelFileType);
            this.panelInput.Controls.Add(this.label1);
            this.panelInput.Font = new System.Drawing.Font("Segoe UI", 10F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point);
            this.panelInput.ForeColor = System.Drawing.Color.Black;
            this.panelInput.Location = new System.Drawing.Point(393, 12);
            this.panelInput.Name = "panelInput";
            this.panelInput.Size = new System.Drawing.Size(395, 207);
            this.panelInput.TabIndex = 7;
            // 
            // tableLayoutPanel1
            // 
            this.tableLayoutPanel1.ColumnCount = 2;
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 29.11392F));
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 70.88608F));
            this.tableLayoutPanel1.Controls.Add(this.buttonOpenFile, 0, 0);
            this.tableLayoutPanel1.Controls.Add(this.panelInputFileBox, 1, 0);
            this.tableLayoutPanel1.Dock = System.Windows.Forms.DockStyle.Top;
            this.tableLayoutPanel1.Location = new System.Drawing.Point(0, 92);
            this.tableLayoutPanel1.Name = "tableLayoutPanel1";
            this.tableLayoutPanel1.RowCount = 1;
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 50F));
            this.tableLayoutPanel1.Size = new System.Drawing.Size(395, 36);
            this.tableLayoutPanel1.TabIndex = 14;
            // 
            // panelInputFileBox
            // 
            this.panelInputFileBox.BackColor = System.Drawing.Color.White;
            this.panelInputFileBox.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.panelInputFileBox.Controls.Add(this.textBoxFileName);
            this.panelInputFileBox.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panelInputFileBox.ForeColor = System.Drawing.Color.Teal;
            this.panelInputFileBox.Location = new System.Drawing.Point(117, 3);
            this.panelInputFileBox.Name = "panelInputFileBox";
            this.panelInputFileBox.Size = new System.Drawing.Size(275, 30);
            this.panelInputFileBox.TabIndex = 1;
            this.panelInputFileBox.Paint += new System.Windows.Forms.PaintEventHandler(this.panelInputFileBox_Paint);
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Dock = System.Windows.Forms.DockStyle.Top;
            this.label1.Font = new System.Drawing.Font("Segoe UI Semibold", 11.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point);
            this.label1.Location = new System.Drawing.Point(0, 0);
            this.label1.Margin = new System.Windows.Forms.Padding(0);
            this.label1.Name = "label1";
            this.label1.Padding = new System.Windows.Forms.Padding(1, 0, 0, 0);
            this.label1.Size = new System.Drawing.Size(46, 20);
            this.label1.TabIndex = 13;
            this.label1.Text = "Input";
            // 
            // FormInputFile
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 15F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.White;
            this.ClientSize = new System.Drawing.Size(800, 472);
            this.Controls.Add(this.panelInput);
            this.Controls.Add(this.groupBoxOutputFile);
            this.Controls.Add(this.groupBoxInputFile);
            this.Controls.Add(this.submitButton);
            this.Controls.Add(this.filePathBox);
            this.Name = "FormInputFile";
            this.Text = "Open file";
            this.groupBoxInputFile.ResumeLayout(false);
            this.groupBoxInputFile.PerformLayout();
            this.panelSamplesIn.ResumeLayout(false);
            this.panelSamplesIn.PerformLayout();
            this.panelFileType.ResumeLayout(false);
            this.panelFileType.PerformLayout();
            this.groupBoxOutputFile.ResumeLayout(false);
            this.groupBoxOutputFile.PerformLayout();
            this.panelSamplesOut.ResumeLayout(false);
            this.panelSamplesOut.PerformLayout();
            this.panelInput.ResumeLayout(false);
            this.panelInput.PerformLayout();
            this.tableLayoutPanel1.ResumeLayout(false);
            this.panelInputFileBox.ResumeLayout(false);
            this.panelInputFileBox.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Button buttonOpenFile;
        private System.Windows.Forms.TextBox filePathBox;
        private System.Windows.Forms.TextBox textBoxFileName;
        private System.Windows.Forms.Button submitButton;
        private System.Windows.Forms.GroupBox groupBoxInputFile;
        private System.Windows.Forms.RadioButton radioButtonExcel;
        private System.Windows.Forms.RadioButton radioButtonMultiQuant;
        private System.Windows.Forms.ListBox listBoxFiles;
        private System.Windows.Forms.Label labelDataFile;
        private System.Windows.Forms.Panel panelFileType;
        private System.Windows.Forms.Label labelFileType;
        private System.Windows.Forms.Label labelOutputFileName;
        private System.Windows.Forms.TextBox textBoxOutputFileName;
        private System.Windows.Forms.Button buttonSelectOutputFolder;
        private System.Windows.Forms.GroupBox groupBoxOutputFile;
        private System.Windows.Forms.Label labelOutputFolder;
        private System.Windows.Forms.TextBox textBoxOutputFolder;
        private System.Windows.Forms.Panel panelSamplesIn;
        private System.Windows.Forms.Label labelSamplesIn;
        private System.Windows.Forms.RadioButton radioButtonColumnsIn;
        private System.Windows.Forms.RadioButton radioButtonRowsIn;
        private System.Windows.Forms.Panel panelSamplesOut;
        private System.Windows.Forms.Label labelSamplesOut;
        private System.Windows.Forms.RadioButton radioButtonColumnsOut;
        private System.Windows.Forms.RadioButton radioButtonRowsOut;
        private System.Windows.Forms.Panel panelInput;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.TableLayoutPanel tableLayoutPanel1;
        private System.Windows.Forms.Panel panelInputFileBox;
    }
}