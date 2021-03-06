﻿namespace QuickMon
{
    partial class EditConfig
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(EditConfig));
            this.cmdCancel = new System.Windows.Forms.Button();
            this.cmdOK = new System.Windows.Forms.Button();
            this.tabControl1 = new System.Windows.Forms.TabControl();
            this.tabPage1 = new System.Windows.Forms.TabPage();
            this.txtDisplayOrchestrations = new System.Windows.Forms.TextBox();
            this.txtDisplaySendPorts = new System.Windows.Forms.TextBox();
            this.txtDisplayReceiveLocations = new System.Windows.Forms.TextBox();
            this.chkDisplayAllOchestrations = new System.Windows.Forms.CheckBox();
            this.chkDisplayAllSendPorts = new System.Windows.Forms.CheckBox();
            this.chkDisplayAllReceiveLocations = new System.Windows.Forms.CheckBox();
            this.cmdTestDB = new System.Windows.Forms.Button();
            this.txtDatabase = new System.Windows.Forms.TextBox();
            this.label2 = new System.Windows.Forms.Label();
            this.txtSQLServer = new System.Windows.Forms.TextBox();
            this.label1 = new System.Windows.Forms.Label();
            this.tabPage2 = new System.Windows.Forms.TabPage();
            this.lstReceiveLocations = new System.Windows.Forms.ListBox();
            this.cmdAddReceiveLocation = new System.Windows.Forms.Button();
            this.cmdRemoveReceiveLocation = new System.Windows.Forms.Button();
            this.chkAllReceiveLocations = new System.Windows.Forms.CheckBox();
            this.tabPage3 = new System.Windows.Forms.TabPage();
            this.lstSendPorts = new System.Windows.Forms.ListBox();
            this.cmdAddSendPort = new System.Windows.Forms.Button();
            this.cmdRemoveSendPort = new System.Windows.Forms.Button();
            this.chkAllSendPorts = new System.Windows.Forms.CheckBox();
            this.tabPage4 = new System.Windows.Forms.TabPage();
            this.lstOrchestrations = new System.Windows.Forms.ListBox();
            this.cmdAddOrchestration = new System.Windows.Forms.Button();
            this.cmdRemoveOrchestrations = new System.Windows.Forms.Button();
            this.chkAllOrchestrations = new System.Windows.Forms.CheckBox();
            this.tabControl1.SuspendLayout();
            this.tabPage1.SuspendLayout();
            this.tabPage2.SuspendLayout();
            this.tabPage3.SuspendLayout();
            this.tabPage4.SuspendLayout();
            this.SuspendLayout();
            // 
            // cmdCancel
            // 
            this.cmdCancel.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.cmdCancel.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.cmdCancel.FlatStyle = System.Windows.Forms.FlatStyle.Popup;
            this.cmdCancel.Location = new System.Drawing.Point(397, 297);
            this.cmdCancel.Name = "cmdCancel";
            this.cmdCancel.Size = new System.Drawing.Size(75, 23);
            this.cmdCancel.TabIndex = 2;
            this.cmdCancel.Text = "Cancel";
            this.cmdCancel.UseVisualStyleBackColor = true;
            // 
            // cmdOK
            // 
            this.cmdOK.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.cmdOK.FlatStyle = System.Windows.Forms.FlatStyle.Popup;
            this.cmdOK.Location = new System.Drawing.Point(316, 297);
            this.cmdOK.Name = "cmdOK";
            this.cmdOK.Size = new System.Drawing.Size(75, 23);
            this.cmdOK.TabIndex = 1;
            this.cmdOK.Text = "OK";
            this.cmdOK.UseVisualStyleBackColor = true;
            this.cmdOK.Click += new System.EventHandler(this.cmdOK_Click);
            // 
            // tabControl1
            // 
            this.tabControl1.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.tabControl1.Controls.Add(this.tabPage1);
            this.tabControl1.Controls.Add(this.tabPage2);
            this.tabControl1.Controls.Add(this.tabPage3);
            this.tabControl1.Controls.Add(this.tabPage4);
            this.tabControl1.Location = new System.Drawing.Point(12, 12);
            this.tabControl1.Name = "tabControl1";
            this.tabControl1.SelectedIndex = 0;
            this.tabControl1.Size = new System.Drawing.Size(460, 279);
            this.tabControl1.TabIndex = 0;
            // 
            // tabPage1
            // 
            this.tabPage1.BackColor = System.Drawing.SystemColors.Control;
            this.tabPage1.Controls.Add(this.txtDisplayOrchestrations);
            this.tabPage1.Controls.Add(this.txtDisplaySendPorts);
            this.tabPage1.Controls.Add(this.txtDisplayReceiveLocations);
            this.tabPage1.Controls.Add(this.chkDisplayAllOchestrations);
            this.tabPage1.Controls.Add(this.chkDisplayAllSendPorts);
            this.tabPage1.Controls.Add(this.chkDisplayAllReceiveLocations);
            this.tabPage1.Controls.Add(this.cmdTestDB);
            this.tabPage1.Controls.Add(this.txtDatabase);
            this.tabPage1.Controls.Add(this.label2);
            this.tabPage1.Controls.Add(this.txtSQLServer);
            this.tabPage1.Controls.Add(this.label1);
            this.tabPage1.Location = new System.Drawing.Point(4, 22);
            this.tabPage1.Name = "tabPage1";
            this.tabPage1.Padding = new System.Windows.Forms.Padding(3);
            this.tabPage1.Size = new System.Drawing.Size(452, 253);
            this.tabPage1.TabIndex = 0;
            this.tabPage1.Text = "BizTalk group settings";
            // 
            // txtDisplayOrchestrations
            // 
            this.txtDisplayOrchestrations.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.txtDisplayOrchestrations.Location = new System.Drawing.Point(28, 217);
            this.txtDisplayOrchestrations.Name = "txtDisplayOrchestrations";
            this.txtDisplayOrchestrations.ReadOnly = true;
            this.txtDisplayOrchestrations.Size = new System.Drawing.Size(418, 20);
            this.txtDisplayOrchestrations.TabIndex = 10;
            // 
            // txtDisplaySendPorts
            // 
            this.txtDisplaySendPorts.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.txtDisplaySendPorts.Location = new System.Drawing.Point(28, 165);
            this.txtDisplaySendPorts.Name = "txtDisplaySendPorts";
            this.txtDisplaySendPorts.ReadOnly = true;
            this.txtDisplaySendPorts.Size = new System.Drawing.Size(418, 20);
            this.txtDisplaySendPorts.TabIndex = 8;
            // 
            // txtDisplayReceiveLocations
            // 
            this.txtDisplayReceiveLocations.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.txtDisplayReceiveLocations.Location = new System.Drawing.Point(28, 116);
            this.txtDisplayReceiveLocations.Name = "txtDisplayReceiveLocations";
            this.txtDisplayReceiveLocations.ReadOnly = true;
            this.txtDisplayReceiveLocations.Size = new System.Drawing.Size(418, 20);
            this.txtDisplayReceiveLocations.TabIndex = 6;
            // 
            // chkDisplayAllOchestrations
            // 
            this.chkDisplayAllOchestrations.AutoSize = true;
            this.chkDisplayAllOchestrations.Checked = true;
            this.chkDisplayAllOchestrations.CheckState = System.Windows.Forms.CheckState.Checked;
            this.chkDisplayAllOchestrations.FlatStyle = System.Windows.Forms.FlatStyle.Popup;
            this.chkDisplayAllOchestrations.Location = new System.Drawing.Point(15, 194);
            this.chkDisplayAllOchestrations.Name = "chkDisplayAllOchestrations";
            this.chkDisplayAllOchestrations.Size = new System.Drawing.Size(107, 17);
            this.chkDisplayAllOchestrations.TabIndex = 9;
            this.chkDisplayAllOchestrations.Text = "All Orchestrations";
            this.chkDisplayAllOchestrations.UseVisualStyleBackColor = true;
            this.chkDisplayAllOchestrations.CheckedChanged += new System.EventHandler(this.chkAllOrchestrations_CheckedChanged);
            // 
            // chkDisplayAllSendPorts
            // 
            this.chkDisplayAllSendPorts.AutoSize = true;
            this.chkDisplayAllSendPorts.Checked = true;
            this.chkDisplayAllSendPorts.CheckState = System.Windows.Forms.CheckState.Checked;
            this.chkDisplayAllSendPorts.FlatStyle = System.Windows.Forms.FlatStyle.Popup;
            this.chkDisplayAllSendPorts.Location = new System.Drawing.Point(15, 142);
            this.chkDisplayAllSendPorts.Name = "chkDisplayAllSendPorts";
            this.chkDisplayAllSendPorts.Size = new System.Drawing.Size(91, 17);
            this.chkDisplayAllSendPorts.TabIndex = 7;
            this.chkDisplayAllSendPorts.Text = "All Send Ports";
            this.chkDisplayAllSendPorts.UseVisualStyleBackColor = true;
            this.chkDisplayAllSendPorts.CheckedChanged += new System.EventHandler(this.chkAllSendPorts_CheckedChanged);
            // 
            // chkDisplayAllReceiveLocations
            // 
            this.chkDisplayAllReceiveLocations.AutoSize = true;
            this.chkDisplayAllReceiveLocations.Checked = true;
            this.chkDisplayAllReceiveLocations.CheckState = System.Windows.Forms.CheckState.Checked;
            this.chkDisplayAllReceiveLocations.FlatStyle = System.Windows.Forms.FlatStyle.Popup;
            this.chkDisplayAllReceiveLocations.Location = new System.Drawing.Point(15, 90);
            this.chkDisplayAllReceiveLocations.Name = "chkDisplayAllReceiveLocations";
            this.chkDisplayAllReceiveLocations.Size = new System.Drawing.Size(128, 17);
            this.chkDisplayAllReceiveLocations.TabIndex = 5;
            this.chkDisplayAllReceiveLocations.Text = "All Receive Locations";
            this.chkDisplayAllReceiveLocations.UseVisualStyleBackColor = true;
            this.chkDisplayAllReceiveLocations.CheckedChanged += new System.EventHandler(this.chkAllReceiveLocations_CheckedChanged);
            // 
            // cmdTestDB
            // 
            this.cmdTestDB.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.cmdTestDB.FlatStyle = System.Windows.Forms.FlatStyle.Popup;
            this.cmdTestDB.Location = new System.Drawing.Point(371, 64);
            this.cmdTestDB.Name = "cmdTestDB";
            this.cmdTestDB.Size = new System.Drawing.Size(75, 23);
            this.cmdTestDB.TabIndex = 4;
            this.cmdTestDB.Text = "Test";
            this.cmdTestDB.UseVisualStyleBackColor = true;
            this.cmdTestDB.Click += new System.EventHandler(this.cmdTestDB_Click);
            // 
            // txtDatabase
            // 
            this.txtDatabase.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.txtDatabase.Location = new System.Drawing.Point(150, 38);
            this.txtDatabase.Name = "txtDatabase";
            this.txtDatabase.Size = new System.Drawing.Size(296, 20);
            this.txtDatabase.TabIndex = 3;
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(12, 41);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(116, 13);
            this.label2.TabIndex = 2;
            this.label2.Text = "Management database";
            // 
            // txtSQLServer
            // 
            this.txtSQLServer.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.txtSQLServer.Location = new System.Drawing.Point(150, 12);
            this.txtSQLServer.Name = "txtSQLServer";
            this.txtSQLServer.Size = new System.Drawing.Size(296, 20);
            this.txtSQLServer.TabIndex = 1;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(12, 15);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(119, 13);
            this.label1.TabIndex = 0;
            this.label1.Text = "Management DB server";
            // 
            // tabPage2
            // 
            this.tabPage2.BackColor = System.Drawing.SystemColors.Control;
            this.tabPage2.Controls.Add(this.lstReceiveLocations);
            this.tabPage2.Controls.Add(this.cmdAddReceiveLocation);
            this.tabPage2.Controls.Add(this.cmdRemoveReceiveLocation);
            this.tabPage2.Controls.Add(this.chkAllReceiveLocations);
            this.tabPage2.Location = new System.Drawing.Point(4, 22);
            this.tabPage2.Name = "tabPage2";
            this.tabPage2.Padding = new System.Windows.Forms.Padding(3);
            this.tabPage2.Size = new System.Drawing.Size(452, 253);
            this.tabPage2.TabIndex = 1;
            this.tabPage2.Text = "Receive Locations";
            // 
            // lstReceiveLocations
            // 
            this.lstReceiveLocations.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.lstReceiveLocations.Enabled = false;
            this.lstReceiveLocations.FormattingEnabled = true;
            this.lstReceiveLocations.Location = new System.Drawing.Point(11, 35);
            this.lstReceiveLocations.Name = "lstReceiveLocations";
            this.lstReceiveLocations.SelectionMode = System.Windows.Forms.SelectionMode.MultiExtended;
            this.lstReceiveLocations.Size = new System.Drawing.Size(391, 199);
            this.lstReceiveLocations.TabIndex = 1;
            // 
            // cmdAddReceiveLocation
            // 
            this.cmdAddReceiveLocation.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.cmdAddReceiveLocation.Enabled = false;
            this.cmdAddReceiveLocation.FlatStyle = System.Windows.Forms.FlatStyle.Popup;
            this.cmdAddReceiveLocation.Location = new System.Drawing.Point(408, 35);
            this.cmdAddReceiveLocation.Name = "cmdAddReceiveLocation";
            this.cmdAddReceiveLocation.Size = new System.Drawing.Size(34, 23);
            this.cmdAddReceiveLocation.TabIndex = 2;
            this.cmdAddReceiveLocation.Text = "+";
            this.cmdAddReceiveLocation.UseVisualStyleBackColor = true;
            this.cmdAddReceiveLocation.Click += new System.EventHandler(this.cmdAddReceiveLocation_Click);
            // 
            // cmdRemoveReceiveLocation
            // 
            this.cmdRemoveReceiveLocation.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.cmdRemoveReceiveLocation.Enabled = false;
            this.cmdRemoveReceiveLocation.FlatStyle = System.Windows.Forms.FlatStyle.Popup;
            this.cmdRemoveReceiveLocation.Location = new System.Drawing.Point(408, 64);
            this.cmdRemoveReceiveLocation.Name = "cmdRemoveReceiveLocation";
            this.cmdRemoveReceiveLocation.Size = new System.Drawing.Size(34, 23);
            this.cmdRemoveReceiveLocation.TabIndex = 3;
            this.cmdRemoveReceiveLocation.Text = "-";
            this.cmdRemoveReceiveLocation.UseVisualStyleBackColor = true;
            this.cmdRemoveReceiveLocation.Click += new System.EventHandler(this.cmdRemoveReceiveLocation_Click);
            // 
            // chkAllReceiveLocations
            // 
            this.chkAllReceiveLocations.AutoSize = true;
            this.chkAllReceiveLocations.Checked = true;
            this.chkAllReceiveLocations.CheckState = System.Windows.Forms.CheckState.Checked;
            this.chkAllReceiveLocations.Location = new System.Drawing.Point(11, 12);
            this.chkAllReceiveLocations.Name = "chkAllReceiveLocations";
            this.chkAllReceiveLocations.Size = new System.Drawing.Size(129, 17);
            this.chkAllReceiveLocations.TabIndex = 0;
            this.chkAllReceiveLocations.Text = "All Receive Locations";
            this.chkAllReceiveLocations.UseVisualStyleBackColor = true;
            this.chkAllReceiveLocations.CheckedChanged += new System.EventHandler(this.chkAllReceiveLocations_CheckedChanged);
            // 
            // tabPage3
            // 
            this.tabPage3.BackColor = System.Drawing.SystemColors.Control;
            this.tabPage3.Controls.Add(this.lstSendPorts);
            this.tabPage3.Controls.Add(this.cmdAddSendPort);
            this.tabPage3.Controls.Add(this.cmdRemoveSendPort);
            this.tabPage3.Controls.Add(this.chkAllSendPorts);
            this.tabPage3.Location = new System.Drawing.Point(4, 22);
            this.tabPage3.Name = "tabPage3";
            this.tabPage3.Padding = new System.Windows.Forms.Padding(3);
            this.tabPage3.Size = new System.Drawing.Size(452, 253);
            this.tabPage3.TabIndex = 2;
            this.tabPage3.Text = "Send Ports";
            // 
            // lstSendPorts
            // 
            this.lstSendPorts.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.lstSendPorts.Enabled = false;
            this.lstSendPorts.FormattingEnabled = true;
            this.lstSendPorts.Location = new System.Drawing.Point(11, 35);
            this.lstSendPorts.Name = "lstSendPorts";
            this.lstSendPorts.SelectionMode = System.Windows.Forms.SelectionMode.MultiExtended;
            this.lstSendPorts.Size = new System.Drawing.Size(391, 199);
            this.lstSendPorts.TabIndex = 1;
            // 
            // cmdAddSendPort
            // 
            this.cmdAddSendPort.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.cmdAddSendPort.Enabled = false;
            this.cmdAddSendPort.FlatStyle = System.Windows.Forms.FlatStyle.Popup;
            this.cmdAddSendPort.Location = new System.Drawing.Point(408, 35);
            this.cmdAddSendPort.Name = "cmdAddSendPort";
            this.cmdAddSendPort.Size = new System.Drawing.Size(34, 23);
            this.cmdAddSendPort.TabIndex = 2;
            this.cmdAddSendPort.Text = "+";
            this.cmdAddSendPort.UseVisualStyleBackColor = true;
            this.cmdAddSendPort.Click += new System.EventHandler(this.cmdAddSendPort_Click);
            // 
            // cmdRemoveSendPort
            // 
            this.cmdRemoveSendPort.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.cmdRemoveSendPort.Enabled = false;
            this.cmdRemoveSendPort.FlatStyle = System.Windows.Forms.FlatStyle.Popup;
            this.cmdRemoveSendPort.Location = new System.Drawing.Point(408, 64);
            this.cmdRemoveSendPort.Name = "cmdRemoveSendPort";
            this.cmdRemoveSendPort.Size = new System.Drawing.Size(34, 23);
            this.cmdRemoveSendPort.TabIndex = 3;
            this.cmdRemoveSendPort.Text = "-";
            this.cmdRemoveSendPort.UseVisualStyleBackColor = true;
            this.cmdRemoveSendPort.Click += new System.EventHandler(this.cmdRemoveSendPort_Click);
            // 
            // chkAllSendPorts
            // 
            this.chkAllSendPorts.AutoSize = true;
            this.chkAllSendPorts.Checked = true;
            this.chkAllSendPorts.CheckState = System.Windows.Forms.CheckState.Checked;
            this.chkAllSendPorts.Location = new System.Drawing.Point(11, 12);
            this.chkAllSendPorts.Name = "chkAllSendPorts";
            this.chkAllSendPorts.Size = new System.Drawing.Size(92, 17);
            this.chkAllSendPorts.TabIndex = 0;
            this.chkAllSendPorts.Text = "All Send Ports";
            this.chkAllSendPorts.UseVisualStyleBackColor = true;
            this.chkAllSendPorts.CheckedChanged += new System.EventHandler(this.chkAllSendPorts_CheckedChanged);
            // 
            // tabPage4
            // 
            this.tabPage4.BackColor = System.Drawing.SystemColors.Control;
            this.tabPage4.Controls.Add(this.lstOrchestrations);
            this.tabPage4.Controls.Add(this.cmdAddOrchestration);
            this.tabPage4.Controls.Add(this.cmdRemoveOrchestrations);
            this.tabPage4.Controls.Add(this.chkAllOrchestrations);
            this.tabPage4.Location = new System.Drawing.Point(4, 22);
            this.tabPage4.Name = "tabPage4";
            this.tabPage4.Padding = new System.Windows.Forms.Padding(3);
            this.tabPage4.Size = new System.Drawing.Size(452, 253);
            this.tabPage4.TabIndex = 3;
            this.tabPage4.Text = "Orchestrations";
            // 
            // lstOrchestrations
            // 
            this.lstOrchestrations.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.lstOrchestrations.Enabled = false;
            this.lstOrchestrations.FormattingEnabled = true;
            this.lstOrchestrations.Location = new System.Drawing.Point(11, 35);
            this.lstOrchestrations.Name = "lstOrchestrations";
            this.lstOrchestrations.SelectionMode = System.Windows.Forms.SelectionMode.MultiExtended;
            this.lstOrchestrations.Size = new System.Drawing.Size(391, 199);
            this.lstOrchestrations.TabIndex = 1;
            // 
            // cmdAddOrchestration
            // 
            this.cmdAddOrchestration.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.cmdAddOrchestration.Enabled = false;
            this.cmdAddOrchestration.FlatStyle = System.Windows.Forms.FlatStyle.Popup;
            this.cmdAddOrchestration.Location = new System.Drawing.Point(408, 35);
            this.cmdAddOrchestration.Name = "cmdAddOrchestration";
            this.cmdAddOrchestration.Size = new System.Drawing.Size(34, 23);
            this.cmdAddOrchestration.TabIndex = 2;
            this.cmdAddOrchestration.Text = "+";
            this.cmdAddOrchestration.UseVisualStyleBackColor = true;
            this.cmdAddOrchestration.Click += new System.EventHandler(this.cmdAddOrchestration_Click);
            // 
            // cmdRemoveOrchestrations
            // 
            this.cmdRemoveOrchestrations.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.cmdRemoveOrchestrations.Enabled = false;
            this.cmdRemoveOrchestrations.FlatStyle = System.Windows.Forms.FlatStyle.Popup;
            this.cmdRemoveOrchestrations.Location = new System.Drawing.Point(408, 64);
            this.cmdRemoveOrchestrations.Name = "cmdRemoveOrchestrations";
            this.cmdRemoveOrchestrations.Size = new System.Drawing.Size(34, 23);
            this.cmdRemoveOrchestrations.TabIndex = 3;
            this.cmdRemoveOrchestrations.Text = "-";
            this.cmdRemoveOrchestrations.UseVisualStyleBackColor = true;
            this.cmdRemoveOrchestrations.Click += new System.EventHandler(this.cmdRemoveOrchestrations_Click);
            // 
            // chkAllOrchestrations
            // 
            this.chkAllOrchestrations.AutoSize = true;
            this.chkAllOrchestrations.Checked = true;
            this.chkAllOrchestrations.CheckState = System.Windows.Forms.CheckState.Checked;
            this.chkAllOrchestrations.Location = new System.Drawing.Point(11, 12);
            this.chkAllOrchestrations.Name = "chkAllOrchestrations";
            this.chkAllOrchestrations.Size = new System.Drawing.Size(108, 17);
            this.chkAllOrchestrations.TabIndex = 0;
            this.chkAllOrchestrations.Text = "All Orchestrations";
            this.chkAllOrchestrations.UseVisualStyleBackColor = true;
            this.chkAllOrchestrations.CheckedChanged += new System.EventHandler(this.chkAllOrchestrations_CheckedChanged);
            // 
            // EditConfig
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.CancelButton = this.cmdCancel;
            this.ClientSize = new System.Drawing.Size(484, 332);
            this.Controls.Add(this.tabControl1);
            this.Controls.Add(this.cmdCancel);
            this.Controls.Add(this.cmdOK);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.MinimizeBox = false;
            this.MinimumSize = new System.Drawing.Size(500, 370);
            this.Name = "EditConfig";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Edit Config";
            this.Shown += new System.EventHandler(this.EditConfig_Shown);
            this.tabControl1.ResumeLayout(false);
            this.tabPage1.ResumeLayout(false);
            this.tabPage1.PerformLayout();
            this.tabPage2.ResumeLayout(false);
            this.tabPage2.PerformLayout();
            this.tabPage3.ResumeLayout(false);
            this.tabPage3.PerformLayout();
            this.tabPage4.ResumeLayout(false);
            this.tabPage4.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Button cmdCancel;
        private System.Windows.Forms.Button cmdOK;
        private System.Windows.Forms.TabControl tabControl1;
        private System.Windows.Forms.TabPage tabPage1;
        private System.Windows.Forms.TabPage tabPage2;
        private System.Windows.Forms.CheckBox chkAllReceiveLocations;
        private System.Windows.Forms.Button cmdTestDB;
        private System.Windows.Forms.TextBox txtDatabase;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.TextBox txtSQLServer;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.ListBox lstReceiveLocations;
        private System.Windows.Forms.Button cmdAddReceiveLocation;
        private System.Windows.Forms.Button cmdRemoveReceiveLocation;
        private System.Windows.Forms.TabPage tabPage3;
        private System.Windows.Forms.ListBox lstSendPorts;
        private System.Windows.Forms.Button cmdAddSendPort;
        private System.Windows.Forms.Button cmdRemoveSendPort;
        private System.Windows.Forms.CheckBox chkAllSendPorts;
        private System.Windows.Forms.TabPage tabPage4;
        private System.Windows.Forms.ListBox lstOrchestrations;
        private System.Windows.Forms.Button cmdAddOrchestration;
        private System.Windows.Forms.Button cmdRemoveOrchestrations;
        private System.Windows.Forms.CheckBox chkAllOrchestrations;
        private System.Windows.Forms.TextBox txtDisplayOrchestrations;
        private System.Windows.Forms.TextBox txtDisplaySendPorts;
        private System.Windows.Forms.TextBox txtDisplayReceiveLocations;
        private System.Windows.Forms.CheckBox chkDisplayAllOchestrations;
        private System.Windows.Forms.CheckBox chkDisplayAllSendPorts;
        private System.Windows.Forms.CheckBox chkDisplayAllReceiveLocations;
    }
}