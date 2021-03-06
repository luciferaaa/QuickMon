﻿namespace QuickMon.Collectors
{
    partial class WMIQueryCollectorEditEntry
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(WMIQueryCollectorEditEntry));
            this.cmdCancel = new System.Windows.Forms.Button();
            this.cmdOK = new System.Windows.Forms.Button();
            this.txtNamespace = new System.Windows.Forms.TextBox();
            this.label1 = new System.Windows.Forms.Label();
            this.txtMachines = new System.Windows.Forms.TextBox();
            this.label2 = new System.Windows.Forms.Label();
            this.txtStateQuery = new System.Windows.Forms.TextBox();
            this.label3 = new System.Windows.Forms.Label();
            this.chkIsReturnValueInt = new System.Windows.Forms.CheckBox();
            this.chkReturnValueNotInverted = new System.Windows.Forms.CheckBox();
            this.cboSuccessValue = new System.Windows.Forms.ComboBox();
            this.label4 = new System.Windows.Forms.Label();
            this.label5 = new System.Windows.Forms.Label();
            this.cboWarningValue = new System.Windows.Forms.ComboBox();
            this.cboErrorValue = new System.Windows.Forms.ComboBox();
            this.label6 = new System.Windows.Forms.Label();
            this.chkUseRowCountAsValue = new System.Windows.Forms.CheckBox();
            this.tabControl1 = new System.Windows.Forms.TabControl();
            this.tabPageSummary = new System.Windows.Forms.TabPage();
            this.cmdEditSummaryQuery = new System.Windows.Forms.Button();
            this.tabPageDetail = new System.Windows.Forms.TabPage();
            this.cmdEditDetailsQuery = new System.Windows.Forms.Button();
            this.chkCopyColumnNames = new System.Windows.Forms.CheckBox();
            this.cmdEditColumnNames = new System.Windows.Forms.Button();
            this.txtColumnNames = new System.Windows.Forms.TextBox();
            this.lblColumnNameSequence = new System.Windows.Forms.Label();
            this.txtDetailQuery = new System.Windows.Forms.TextBox();
            this.label7 = new System.Windows.Forms.Label();
            this.label9 = new System.Windows.Forms.Label();
            this.keyColumnNumericUpDown = new System.Windows.Forms.NumericUpDown();
            this.cmdTestDB = new System.Windows.Forms.Button();
            this.txtName = new System.Windows.Forms.TextBox();
            this.label8 = new System.Windows.Forms.Label();
            this.tabControl1.SuspendLayout();
            this.tabPageSummary.SuspendLayout();
            this.tabPageDetail.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.keyColumnNumericUpDown)).BeginInit();
            this.SuspendLayout();
            // 
            // cmdCancel
            // 
            this.cmdCancel.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.cmdCancel.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.cmdCancel.FlatStyle = System.Windows.Forms.FlatStyle.Popup;
            this.cmdCancel.Location = new System.Drawing.Point(463, 321);
            this.cmdCancel.Name = "cmdCancel";
            this.cmdCancel.Size = new System.Drawing.Size(75, 23);
            this.cmdCancel.TabIndex = 9;
            this.cmdCancel.Text = "Cancel";
            this.cmdCancel.UseVisualStyleBackColor = true;
            // 
            // cmdOK
            // 
            this.cmdOK.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.cmdOK.FlatStyle = System.Windows.Forms.FlatStyle.Popup;
            this.cmdOK.Location = new System.Drawing.Point(382, 321);
            this.cmdOK.Name = "cmdOK";
            this.cmdOK.Size = new System.Drawing.Size(75, 23);
            this.cmdOK.TabIndex = 8;
            this.cmdOK.Text = "OK";
            this.cmdOK.UseVisualStyleBackColor = true;
            this.cmdOK.Click += new System.EventHandler(this.cmdOK_Click);
            // 
            // txtNamespace
            // 
            this.txtNamespace.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.txtNamespace.Location = new System.Drawing.Point(152, 35);
            this.txtNamespace.Name = "txtNamespace";
            this.txtNamespace.Size = new System.Drawing.Size(386, 20);
            this.txtNamespace.TabIndex = 3;
            this.txtNamespace.Text = "root\\CIMV2";
            this.txtNamespace.TextChanged += new System.EventHandler(this.txtNamespace_TextChanged);
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(14, 38);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(114, 13);
            this.label1.TabIndex = 2;
            this.label1.Text = "WMI Root namespace";
            // 
            // txtMachines
            // 
            this.txtMachines.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.txtMachines.Location = new System.Drawing.Point(152, 61);
            this.txtMachines.Name = "txtMachines";
            this.txtMachines.Size = new System.Drawing.Size(386, 20);
            this.txtMachines.TabIndex = 5;
            this.txtMachines.Text = ".";
            this.txtMachines.TextChanged += new System.EventHandler(this.txtMachines_TextChanged);
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(14, 64);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(77, 13);
            this.label2.TabIndex = 4;
            this.label2.Text = "Machine name";
            // 
            // txtStateQuery
            // 
            this.txtStateQuery.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.txtStateQuery.Location = new System.Drawing.Point(13, 25);
            this.txtStateQuery.Multiline = true;
            this.txtStateQuery.Name = "txtStateQuery";
            this.txtStateQuery.ScrollBars = System.Windows.Forms.ScrollBars.Vertical;
            this.txtStateQuery.Size = new System.Drawing.Size(495, 105);
            this.txtStateQuery.TabIndex = 2;
            this.txtStateQuery.Text = "SELECT FreeSpace FROM Win32_LogicalDisk where Caption = \'C:\'";
            this.txtStateQuery.TextChanged += new System.EventHandler(this.txtStateQuery_TextChanged);
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(10, 9);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(118, 13);
            this.label3.TabIndex = 0;
            this.label3.Text = "WMI query for summary";
            // 
            // chkIsReturnValueInt
            // 
            this.chkIsReturnValueInt.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.chkIsReturnValueInt.AutoSize = true;
            this.chkIsReturnValueInt.Checked = true;
            this.chkIsReturnValueInt.CheckState = System.Windows.Forms.CheckState.Checked;
            this.chkIsReturnValueInt.FlatStyle = System.Windows.Forms.FlatStyle.Popup;
            this.chkIsReturnValueInt.Location = new System.Drawing.Point(13, 136);
            this.chkIsReturnValueInt.Name = "chkIsReturnValueInt";
            this.chkIsReturnValueInt.Size = new System.Drawing.Size(157, 17);
            this.chkIsReturnValueInt.TabIndex = 3;
            this.chkIsReturnValueInt.Text = "Value is in a range of values";
            this.chkIsReturnValueInt.UseVisualStyleBackColor = true;
            this.chkIsReturnValueInt.CheckedChanged += new System.EventHandler(this.chkIsReturnValueInt_CheckedChanged);
            // 
            // chkReturnValueNotInverted
            // 
            this.chkReturnValueNotInverted.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.chkReturnValueNotInverted.AutoSize = true;
            this.chkReturnValueNotInverted.Checked = true;
            this.chkReturnValueNotInverted.CheckState = System.Windows.Forms.CheckState.Checked;
            this.chkReturnValueNotInverted.FlatStyle = System.Windows.Forms.FlatStyle.Popup;
            this.chkReturnValueNotInverted.Location = new System.Drawing.Point(199, 136);
            this.chkReturnValueNotInverted.Name = "chkReturnValueNotInverted";
            this.chkReturnValueNotInverted.Size = new System.Drawing.Size(151, 17);
            this.chkReturnValueNotInverted.TabIndex = 4;
            this.chkReturnValueNotInverted.Text = "Success < Warning < Error";
            this.chkReturnValueNotInverted.UseVisualStyleBackColor = true;
            // 
            // cboSuccessValue
            // 
            this.cboSuccessValue.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.cboSuccessValue.FlatStyle = System.Windows.Forms.FlatStyle.Popup;
            this.cboSuccessValue.FormattingEnabled = true;
            this.cboSuccessValue.Items.AddRange(new object[] {
            "[null]",
            "[any]"});
            this.cboSuccessValue.Location = new System.Drawing.Point(102, 164);
            this.cboSuccessValue.Name = "cboSuccessValue";
            this.cboSuccessValue.Size = new System.Drawing.Size(102, 21);
            this.cboSuccessValue.TabIndex = 7;
            this.cboSuccessValue.TextChanged += new System.EventHandler(this.cboSuccessValue_TextChanged);
            // 
            // label4
            // 
            this.label4.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(10, 167);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(86, 13);
            this.label4.TabIndex = 6;
            this.label4.Text = "Values: Success";
            // 
            // label5
            // 
            this.label5.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.label5.AutoSize = true;
            this.label5.Location = new System.Drawing.Point(210, 167);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(47, 13);
            this.label5.TabIndex = 8;
            this.label5.Text = "Warning";
            // 
            // cboWarningValue
            // 
            this.cboWarningValue.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.cboWarningValue.FlatStyle = System.Windows.Forms.FlatStyle.Popup;
            this.cboWarningValue.FormattingEnabled = true;
            this.cboWarningValue.Items.AddRange(new object[] {
            "[null]",
            "[any]"});
            this.cboWarningValue.Location = new System.Drawing.Point(263, 164);
            this.cboWarningValue.Name = "cboWarningValue";
            this.cboWarningValue.Size = new System.Drawing.Size(96, 21);
            this.cboWarningValue.TabIndex = 9;
            this.cboWarningValue.TextChanged += new System.EventHandler(this.cboWarningValue_TextChanged);
            // 
            // cboErrorValue
            // 
            this.cboErrorValue.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.cboErrorValue.FlatStyle = System.Windows.Forms.FlatStyle.Popup;
            this.cboErrorValue.FormattingEnabled = true;
            this.cboErrorValue.Items.AddRange(new object[] {
            "[null]",
            "[any]"});
            this.cboErrorValue.Location = new System.Drawing.Point(400, 164);
            this.cboErrorValue.Name = "cboErrorValue";
            this.cboErrorValue.Size = new System.Drawing.Size(105, 21);
            this.cboErrorValue.TabIndex = 11;
            this.cboErrorValue.TextChanged += new System.EventHandler(this.cboErrorValue_TextChanged);
            // 
            // label6
            // 
            this.label6.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.label6.AutoSize = true;
            this.label6.Location = new System.Drawing.Point(365, 167);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(29, 13);
            this.label6.TabIndex = 10;
            this.label6.Text = "Error";
            // 
            // chkUseRowCountAsValue
            // 
            this.chkUseRowCountAsValue.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.chkUseRowCountAsValue.AutoSize = true;
            this.chkUseRowCountAsValue.Checked = true;
            this.chkUseRowCountAsValue.CheckState = System.Windows.Forms.CheckState.Checked;
            this.chkUseRowCountAsValue.FlatStyle = System.Windows.Forms.FlatStyle.Popup;
            this.chkUseRowCountAsValue.Location = new System.Drawing.Point(358, 136);
            this.chkUseRowCountAsValue.Name = "chkUseRowCountAsValue";
            this.chkUseRowCountAsValue.Size = new System.Drawing.Size(115, 17);
            this.chkUseRowCountAsValue.TabIndex = 5;
            this.chkUseRowCountAsValue.Text = "Row count is value";
            this.chkUseRowCountAsValue.UseVisualStyleBackColor = true;
            // 
            // tabControl1
            // 
            this.tabControl1.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.tabControl1.Controls.Add(this.tabPageSummary);
            this.tabControl1.Controls.Add(this.tabPageDetail);
            this.tabControl1.Location = new System.Drawing.Point(12, 87);
            this.tabControl1.Name = "tabControl1";
            this.tabControl1.SelectedIndex = 0;
            this.tabControl1.Size = new System.Drawing.Size(526, 228);
            this.tabControl1.TabIndex = 6;
            // 
            // tabPageSummary
            // 
            this.tabPageSummary.BackColor = System.Drawing.SystemColors.Control;
            this.tabPageSummary.Controls.Add(this.cmdEditSummaryQuery);
            this.tabPageSummary.Controls.Add(this.txtStateQuery);
            this.tabPageSummary.Controls.Add(this.label6);
            this.tabPageSummary.Controls.Add(this.chkUseRowCountAsValue);
            this.tabPageSummary.Controls.Add(this.label5);
            this.tabPageSummary.Controls.Add(this.label4);
            this.tabPageSummary.Controls.Add(this.label3);
            this.tabPageSummary.Controls.Add(this.cboErrorValue);
            this.tabPageSummary.Controls.Add(this.chkIsReturnValueInt);
            this.tabPageSummary.Controls.Add(this.chkReturnValueNotInverted);
            this.tabPageSummary.Controls.Add(this.cboWarningValue);
            this.tabPageSummary.Controls.Add(this.cboSuccessValue);
            this.tabPageSummary.Location = new System.Drawing.Point(4, 22);
            this.tabPageSummary.Name = "tabPageSummary";
            this.tabPageSummary.Padding = new System.Windows.Forms.Padding(3);
            this.tabPageSummary.Size = new System.Drawing.Size(518, 202);
            this.tabPageSummary.TabIndex = 0;
            this.tabPageSummary.Text = "Summary";
            // 
            // cmdEditSummaryQuery
            // 
            this.cmdEditSummaryQuery.FlatStyle = System.Windows.Forms.FlatStyle.Popup;
            this.cmdEditSummaryQuery.Image = global::QuickMon.Properties.Resources.settings_16;
            this.cmdEditSummaryQuery.Location = new System.Drawing.Point(136, 2);
            this.cmdEditSummaryQuery.Name = "cmdEditSummaryQuery";
            this.cmdEditSummaryQuery.Size = new System.Drawing.Size(36, 23);
            this.cmdEditSummaryQuery.TabIndex = 1;
            this.cmdEditSummaryQuery.UseVisualStyleBackColor = true;
            this.cmdEditSummaryQuery.Click += new System.EventHandler(this.cmdEditSummaryQuery_Click);
            // 
            // tabPageDetail
            // 
            this.tabPageDetail.BackColor = System.Drawing.SystemColors.Control;
            this.tabPageDetail.Controls.Add(this.cmdEditDetailsQuery);
            this.tabPageDetail.Controls.Add(this.chkCopyColumnNames);
            this.tabPageDetail.Controls.Add(this.cmdEditColumnNames);
            this.tabPageDetail.Controls.Add(this.txtColumnNames);
            this.tabPageDetail.Controls.Add(this.lblColumnNameSequence);
            this.tabPageDetail.Controls.Add(this.txtDetailQuery);
            this.tabPageDetail.Controls.Add(this.label7);
            this.tabPageDetail.Controls.Add(this.label9);
            this.tabPageDetail.Controls.Add(this.keyColumnNumericUpDown);
            this.tabPageDetail.Location = new System.Drawing.Point(4, 22);
            this.tabPageDetail.Name = "tabPageDetail";
            this.tabPageDetail.Padding = new System.Windows.Forms.Padding(3);
            this.tabPageDetail.Size = new System.Drawing.Size(518, 202);
            this.tabPageDetail.TabIndex = 1;
            this.tabPageDetail.Text = "Detail";
            // 
            // cmdEditDetailsQuery
            // 
            this.cmdEditDetailsQuery.FlatStyle = System.Windows.Forms.FlatStyle.Popup;
            this.cmdEditDetailsQuery.Image = global::QuickMon.Properties.Resources.settings_16;
            this.cmdEditDetailsQuery.Location = new System.Drawing.Point(136, 2);
            this.cmdEditDetailsQuery.Name = "cmdEditDetailsQuery";
            this.cmdEditDetailsQuery.Size = new System.Drawing.Size(36, 23);
            this.cmdEditDetailsQuery.TabIndex = 8;
            this.cmdEditDetailsQuery.UseVisualStyleBackColor = true;
            this.cmdEditDetailsQuery.Click += new System.EventHandler(this.cmdEditDetailsQuery_Click);
            // 
            // chkCopyColumnNames
            // 
            this.chkCopyColumnNames.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.chkCopyColumnNames.AutoSize = true;
            this.chkCopyColumnNames.Location = new System.Drawing.Point(298, 8);
            this.chkCopyColumnNames.Name = "chkCopyColumnNames";
            this.chkCopyColumnNames.Size = new System.Drawing.Size(214, 17);
            this.chkCopyColumnNames.TabIndex = 1;
            this.chkCopyColumnNames.Text = "Copy column names to clipboard on test";
            this.chkCopyColumnNames.UseVisualStyleBackColor = true;
            // 
            // cmdEditColumnNames
            // 
            this.cmdEditColumnNames.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.cmdEditColumnNames.FlatStyle = System.Windows.Forms.FlatStyle.Popup;
            this.cmdEditColumnNames.Location = new System.Drawing.Point(471, 171);
            this.cmdEditColumnNames.Name = "cmdEditColumnNames";
            this.cmdEditColumnNames.Size = new System.Drawing.Size(33, 23);
            this.cmdEditColumnNames.TabIndex = 7;
            this.cmdEditColumnNames.Text = "- - -";
            this.cmdEditColumnNames.UseVisualStyleBackColor = true;
            this.cmdEditColumnNames.Click += new System.EventHandler(this.cmdEditColumnNames_Click);
            // 
            // txtColumnNames
            // 
            this.txtColumnNames.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.txtColumnNames.Location = new System.Drawing.Point(136, 173);
            this.txtColumnNames.Name = "txtColumnNames";
            this.txtColumnNames.Size = new System.Drawing.Size(329, 20);
            this.txtColumnNames.TabIndex = 6;
            this.txtColumnNames.Text = ".";
            // 
            // lblColumnNameSequence
            // 
            this.lblColumnNameSequence.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.lblColumnNameSequence.AutoSize = true;
            this.lblColumnNameSequence.Location = new System.Drawing.Point(9, 176);
            this.lblColumnNameSequence.Name = "lblColumnNameSequence";
            this.lblColumnNameSequence.Size = new System.Drawing.Size(121, 13);
            this.lblColumnNameSequence.TabIndex = 5;
            this.lblColumnNameSequence.Text = "Column name sequence";
            this.lblColumnNameSequence.DoubleClick += new System.EventHandler(this.lblColumnNameSequence_DoubleClick);
            // 
            // txtDetailQuery
            // 
            this.txtDetailQuery.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.txtDetailQuery.Location = new System.Drawing.Point(13, 25);
            this.txtDetailQuery.Multiline = true;
            this.txtDetailQuery.Name = "txtDetailQuery";
            this.txtDetailQuery.ScrollBars = System.Windows.Forms.ScrollBars.Vertical;
            this.txtDetailQuery.Size = new System.Drawing.Size(495, 140);
            this.txtDetailQuery.TabIndex = 2;
            this.txtDetailQuery.Text = "SELECT Size, FreeSpace, Description FROM Win32_LogicalDisk where Caption = \'C:\'";
            this.txtDetailQuery.TextChanged += new System.EventHandler(this.txtDetailQuery_TextChanged);
            // 
            // label7
            // 
            this.label7.AutoSize = true;
            this.label7.Location = new System.Drawing.Point(10, 9);
            this.label7.Name = "label7";
            this.label7.Size = new System.Drawing.Size(118, 13);
            this.label7.TabIndex = 0;
            this.label7.Text = "WMI query for summary";
            // 
            // label9
            // 
            this.label9.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.label9.AutoSize = true;
            this.label9.Location = new System.Drawing.Point(13, 181);
            this.label9.Name = "label9";
            this.label9.Size = new System.Drawing.Size(62, 13);
            this.label9.TabIndex = 4;
            this.label9.Text = "Key column";
            this.label9.Visible = false;
            // 
            // keyColumnNumericUpDown
            // 
            this.keyColumnNumericUpDown.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.keyColumnNumericUpDown.Location = new System.Drawing.Point(136, 179);
            this.keyColumnNumericUpDown.Name = "keyColumnNumericUpDown";
            this.keyColumnNumericUpDown.Size = new System.Drawing.Size(59, 20);
            this.keyColumnNumericUpDown.TabIndex = 6;
            this.keyColumnNumericUpDown.Visible = false;
            // 
            // cmdTestDB
            // 
            this.cmdTestDB.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.cmdTestDB.FlatStyle = System.Windows.Forms.FlatStyle.Popup;
            this.cmdTestDB.Location = new System.Drawing.Point(301, 321);
            this.cmdTestDB.Name = "cmdTestDB";
            this.cmdTestDB.Size = new System.Drawing.Size(75, 23);
            this.cmdTestDB.TabIndex = 7;
            this.cmdTestDB.Text = "Test";
            this.cmdTestDB.UseVisualStyleBackColor = true;
            this.cmdTestDB.Click += new System.EventHandler(this.cmdTestDB_Click);
            // 
            // txtName
            // 
            this.txtName.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.txtName.Location = new System.Drawing.Point(152, 9);
            this.txtName.Name = "txtName";
            this.txtName.Size = new System.Drawing.Size(386, 20);
            this.txtName.TabIndex = 1;
            this.txtName.TextChanged += new System.EventHandler(this.txtName_TextChanged);
            // 
            // label8
            // 
            this.label8.AutoSize = true;
            this.label8.Location = new System.Drawing.Point(14, 12);
            this.label8.Name = "label8";
            this.label8.Size = new System.Drawing.Size(35, 13);
            this.label8.TabIndex = 0;
            this.label8.Text = "Name";
            // 
            // WMIQueryCollectorEditEntry
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.CancelButton = this.cmdCancel;
            this.ClientSize = new System.Drawing.Size(558, 356);
            this.Controls.Add(this.txtName);
            this.Controls.Add(this.label8);
            this.Controls.Add(this.cmdTestDB);
            this.Controls.Add(this.tabControl1);
            this.Controls.Add(this.txtMachines);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.txtNamespace);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.cmdCancel);
            this.Controls.Add(this.cmdOK);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.MinimumSize = new System.Drawing.Size(566, 372);
            this.Name = "WMIQueryCollectorEditEntry";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Edit WMI Query Config";
            this.Load += new System.EventHandler(this.EditConfigEntry_Load);
            this.Shown += new System.EventHandler(this.EditConfig_Shown);
            this.tabControl1.ResumeLayout(false);
            this.tabPageSummary.ResumeLayout(false);
            this.tabPageSummary.PerformLayout();
            this.tabPageDetail.ResumeLayout(false);
            this.tabPageDetail.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.keyColumnNumericUpDown)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Button cmdEditDetailsQuery;
        private System.Windows.Forms.Button cmdCancel;
        private System.Windows.Forms.CheckBox chkCopyColumnNames;
        private System.Windows.Forms.Button cmdEditColumnNames;
        private System.Windows.Forms.TabPage tabPageDetail;
        private System.Windows.Forms.TextBox txtColumnNames;
        private System.Windows.Forms.Label lblColumnNameSequence;
        private System.Windows.Forms.TextBox txtDetailQuery;
        private System.Windows.Forms.Label label7;
        private System.Windows.Forms.Label label9;
        private System.Windows.Forms.NumericUpDown keyColumnNumericUpDown;
        private System.Windows.Forms.Button cmdEditSummaryQuery;
        private System.Windows.Forms.Label label6;
        private System.Windows.Forms.TextBox txtName;
        private System.Windows.Forms.Button cmdTestDB;
        private System.Windows.Forms.TabPage tabPageSummary;
        private System.Windows.Forms.TextBox txtStateQuery;
        private System.Windows.Forms.CheckBox chkUseRowCountAsValue;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.ComboBox cboErrorValue;
        private System.Windows.Forms.CheckBox chkIsReturnValueInt;
        private System.Windows.Forms.CheckBox chkReturnValueNotInverted;
        private System.Windows.Forms.ComboBox cboWarningValue;
        private System.Windows.Forms.ComboBox cboSuccessValue;
        private System.Windows.Forms.TabControl tabControl1;
        private System.Windows.Forms.TextBox txtMachines;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.TextBox txtNamespace;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Button cmdOK;
        private System.Windows.Forms.Label label8;
    }
}