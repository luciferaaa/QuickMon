﻿namespace QuickMon.Forms
{
    partial class RemoteAgentHosts
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
            this.components = new System.ComponentModel.Container();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(RemoteAgentHosts));
            this.shadePanel1 = new System.Windows.Forms.Panel();
            this.cmdRefresh = new System.Windows.Forms.Button();
            this.panel1 = new System.Windows.Forms.Panel();
            this.llblLocalServiceRegistered = new System.Windows.Forms.LinkLabel();
            this.llblStartLocalService = new System.Windows.Forms.LinkLabel();
            this.llblFirewallRule = new System.Windows.Forms.LinkLabel();
            this.cmdAdd = new System.Windows.Forms.Button();
            this.remoteportNumericUpDown = new System.Windows.Forms.NumericUpDown();
            this.label14 = new System.Windows.Forms.Label();
            this.txtComputer = new System.Windows.Forms.TextBox();
            this.lblComputer = new System.Windows.Forms.Label();
            this.panel2 = new System.Windows.Forms.Panel();
            this.lvwRemoteHosts = new QuickMon.ListViewEx();
            this.remoteAgentColumnHeader = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.portColumnHeader = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.versionColumnHeader = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.packsColumnHeader = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.remoteHostListContextMenuStrip = new System.Windows.Forms.ContextMenuStrip(this.components);
            this.monitorPacksToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.removeToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.testToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripMenuItem1 = new System.Windows.Forms.ToolStripSeparator();
            this.refreshToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.remoteHostStatusImageList = new System.Windows.Forms.ImageList(this.components);
            this.refreshTimer = new System.Windows.Forms.Timer(this.components);
            this.quickMonServiceOpenFileDialog = new System.Windows.Forms.OpenFileDialog();
            this.cmdEditMonitorPackList = new System.Windows.Forms.Button();
            this.toolTip1 = new System.Windows.Forms.ToolTip(this.components);
            this.shadePanel1.SuspendLayout();
            this.panel1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.remoteportNumericUpDown)).BeginInit();
            this.panel2.SuspendLayout();
            this.remoteHostListContextMenuStrip.SuspendLayout();
            this.SuspendLayout();
            // 
            // shadePanel1
            // 
            this.shadePanel1.BackgroundImage = global::QuickMon.Properties.Resources.BlueHeader1;
            this.shadePanel1.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch;
            this.shadePanel1.Controls.Add(this.cmdEditMonitorPackList);
            this.shadePanel1.Controls.Add(this.cmdRefresh);
            this.shadePanel1.Dock = System.Windows.Forms.DockStyle.Top;
            this.shadePanel1.Location = new System.Drawing.Point(0, 0);
            this.shadePanel1.Name = "shadePanel1";
            this.shadePanel1.Size = new System.Drawing.Size(701, 31);
            this.shadePanel1.TabIndex = 0;
            // 
            // cmdRefresh
            // 
            this.cmdRefresh.BackColor = System.Drawing.Color.Transparent;
            this.cmdRefresh.BackgroundImage = global::QuickMon.Properties.Resources.refresh24x24;
            this.cmdRefresh.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Center;
            this.cmdRefresh.FlatAppearance.BorderColor = System.Drawing.Color.Teal;
            this.cmdRefresh.FlatAppearance.BorderSize = 0;
            this.cmdRefresh.FlatStyle = System.Windows.Forms.FlatStyle.Popup;
            this.cmdRefresh.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.cmdRefresh.Location = new System.Drawing.Point(2, 1);
            this.cmdRefresh.Name = "cmdRefresh";
            this.cmdRefresh.Size = new System.Drawing.Size(28, 28);
            this.cmdRefresh.TabIndex = 0;
            this.toolTip1.SetToolTip(this.cmdRefresh, "Refresh list");
            this.cmdRefresh.UseVisualStyleBackColor = false;
            this.cmdRefresh.Click += new System.EventHandler(this.refreshToolStripMenuItem_Click);
            // 
            // panel1
            // 
            this.panel1.BackgroundImage = global::QuickMon.Properties.Resources.BlueHeader2;
            this.panel1.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch;
            this.panel1.Controls.Add(this.llblLocalServiceRegistered);
            this.panel1.Controls.Add(this.llblStartLocalService);
            this.panel1.Controls.Add(this.llblFirewallRule);
            this.panel1.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.panel1.Location = new System.Drawing.Point(0, 303);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(701, 31);
            this.panel1.TabIndex = 1;
            // 
            // llblLocalServiceRegistered
            // 
            this.llblLocalServiceRegistered.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.llblLocalServiceRegistered.AutoSize = true;
            this.llblLocalServiceRegistered.BackColor = System.Drawing.Color.Transparent;
            this.llblLocalServiceRegistered.LinkBehavior = System.Windows.Forms.LinkBehavior.HoverUnderline;
            this.llblLocalServiceRegistered.LinkColor = System.Drawing.Color.Black;
            this.llblLocalServiceRegistered.Location = new System.Drawing.Point(12, 9);
            this.llblLocalServiceRegistered.Name = "llblLocalServiceRegistered";
            this.llblLocalServiceRegistered.Size = new System.Drawing.Size(187, 13);
            this.llblLocalServiceRegistered.TabIndex = 0;
            this.llblLocalServiceRegistered.TabStop = true;
            this.llblLocalServiceRegistered.Text = "Register local \'Remote Agent/Service\'";
            this.llblLocalServiceRegistered.LinkClicked += new System.Windows.Forms.LinkLabelLinkClickedEventHandler(this.llblLocalServiceRegistered_LinkClicked);
            // 
            // llblStartLocalService
            // 
            this.llblStartLocalService.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.llblStartLocalService.AutoSize = true;
            this.llblStartLocalService.BackColor = System.Drawing.Color.Transparent;
            this.llblStartLocalService.LinkBehavior = System.Windows.Forms.LinkBehavior.HoverUnderline;
            this.llblStartLocalService.LinkColor = System.Drawing.Color.Black;
            this.llblStartLocalService.Location = new System.Drawing.Point(12, 9);
            this.llblStartLocalService.Name = "llblStartLocalService";
            this.llblStartLocalService.Size = new System.Drawing.Size(170, 13);
            this.llblStartLocalService.TabIndex = 2;
            this.llblStartLocalService.TabStop = true;
            this.llblStartLocalService.Text = "Start local \'Remote Agent/Service\'";
            this.llblStartLocalService.LinkClicked += new System.Windows.Forms.LinkLabelLinkClickedEventHandler(this.llblStartLocalService_LinkClicked);
            // 
            // llblFirewallRule
            // 
            this.llblFirewallRule.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.llblFirewallRule.AutoSize = true;
            this.llblFirewallRule.BackColor = System.Drawing.Color.Transparent;
            this.llblFirewallRule.LinkBehavior = System.Windows.Forms.LinkBehavior.HoverUnderline;
            this.llblFirewallRule.LinkColor = System.Drawing.Color.Black;
            this.llblFirewallRule.Location = new System.Drawing.Point(237, 9);
            this.llblFirewallRule.Name = "llblFirewallRule";
            this.llblFirewallRule.Size = new System.Drawing.Size(218, 13);
            this.llblFirewallRule.TabIndex = 1;
            this.llblFirewallRule.TabStop = true;
            this.llblFirewallRule.Text = "Add Remote Host Firewall rule for port 48181";
            this.llblFirewallRule.LinkClicked += new System.Windows.Forms.LinkLabelLinkClickedEventHandler(this.llblFirewallRule_LinkClicked);
            // 
            // cmdAdd
            // 
            this.cmdAdd.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.cmdAdd.Enabled = false;
            this.cmdAdd.FlatStyle = System.Windows.Forms.FlatStyle.Popup;
            this.cmdAdd.Location = new System.Drawing.Point(648, 247);
            this.cmdAdd.Name = "cmdAdd";
            this.cmdAdd.Size = new System.Drawing.Size(50, 23);
            this.cmdAdd.TabIndex = 5;
            this.cmdAdd.Text = "Add";
            this.cmdAdd.UseVisualStyleBackColor = true;
            this.cmdAdd.Click += new System.EventHandler(this.cmdAdd_Click);
            // 
            // remoteportNumericUpDown
            // 
            this.remoteportNumericUpDown.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.remoteportNumericUpDown.Location = new System.Drawing.Point(580, 250);
            this.remoteportNumericUpDown.Maximum = new decimal(new int[] {
            65536,
            0,
            0,
            0});
            this.remoteportNumericUpDown.Minimum = new decimal(new int[] {
            1,
            0,
            0,
            0});
            this.remoteportNumericUpDown.Name = "remoteportNumericUpDown";
            this.remoteportNumericUpDown.Size = new System.Drawing.Size(62, 20);
            this.remoteportNumericUpDown.TabIndex = 4;
            this.remoteportNumericUpDown.Value = new decimal(new int[] {
            48181,
            0,
            0,
            0});
            // 
            // label14
            // 
            this.label14.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.label14.AutoSize = true;
            this.label14.Location = new System.Drawing.Point(548, 252);
            this.label14.Name = "label14";
            this.label14.Size = new System.Drawing.Size(26, 13);
            this.label14.TabIndex = 3;
            this.label14.Text = "Port";
            // 
            // txtComputer
            // 
            this.txtComputer.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.txtComputer.Location = new System.Drawing.Point(86, 249);
            this.txtComputer.Name = "txtComputer";
            this.txtComputer.Size = new System.Drawing.Size(456, 20);
            this.txtComputer.TabIndex = 2;
            this.txtComputer.TextChanged += new System.EventHandler(this.txtComputer_TextChanged);
            this.txtComputer.KeyDown += new System.Windows.Forms.KeyEventHandler(this.txtComputer_KeyDown);
            // 
            // lblComputer
            // 
            this.lblComputer.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.lblComputer.AutoSize = true;
            this.lblComputer.Location = new System.Drawing.Point(7, 252);
            this.lblComputer.Name = "lblComputer";
            this.lblComputer.Size = new System.Drawing.Size(73, 13);
            this.lblComputer.TabIndex = 1;
            this.lblComputer.Text = "Add computer";
            this.lblComputer.DoubleClick += new System.EventHandler(this.lblComputer_DoubleClick);
            // 
            // panel2
            // 
            this.panel2.Controls.Add(this.lvwRemoteHosts);
            this.panel2.Controls.Add(this.cmdAdd);
            this.panel2.Controls.Add(this.txtComputer);
            this.panel2.Controls.Add(this.remoteportNumericUpDown);
            this.panel2.Controls.Add(this.lblComputer);
            this.panel2.Controls.Add(this.label14);
            this.panel2.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panel2.Location = new System.Drawing.Point(0, 31);
            this.panel2.Name = "panel2";
            this.panel2.Size = new System.Drawing.Size(701, 272);
            this.panel2.TabIndex = 21;
            // 
            // lvwRemoteHosts
            // 
            this.lvwRemoteHosts.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.lvwRemoteHosts.AutoResizeColumnEnabled = false;
            this.lvwRemoteHosts.AutoResizeColumnIndex = 3;
            this.lvwRemoteHosts.Columns.AddRange(new System.Windows.Forms.ColumnHeader[] {
            this.remoteAgentColumnHeader,
            this.portColumnHeader,
            this.versionColumnHeader,
            this.packsColumnHeader});
            this.lvwRemoteHosts.ContextMenuStrip = this.remoteHostListContextMenuStrip;
            this.lvwRemoteHosts.FullRowSelect = true;
            this.lvwRemoteHosts.Location = new System.Drawing.Point(0, 0);
            this.lvwRemoteHosts.Name = "lvwRemoteHosts";
            this.lvwRemoteHosts.Size = new System.Drawing.Size(701, 241);
            this.lvwRemoteHosts.SmallImageList = this.remoteHostStatusImageList;
            this.lvwRemoteHosts.TabIndex = 0;
            this.lvwRemoteHosts.UseCompatibleStateImageBehavior = false;
            this.lvwRemoteHosts.View = System.Windows.Forms.View.Details;
            this.lvwRemoteHosts.SelectedIndexChanged += new System.EventHandler(this.lvwRemoteHosts_SelectedIndexChanged);
            this.lvwRemoteHosts.DoubleClick += new System.EventHandler(this.lvwRemoteHosts_DoubleClick);
            // 
            // remoteAgentColumnHeader
            // 
            this.remoteAgentColumnHeader.Text = "Remote host";
            this.remoteAgentColumnHeader.Width = 150;
            // 
            // portColumnHeader
            // 
            this.portColumnHeader.Text = "Port";
            this.portColumnHeader.Width = 74;
            // 
            // versionColumnHeader
            // 
            this.versionColumnHeader.Text = "Version";
            this.versionColumnHeader.Width = 97;
            // 
            // packsColumnHeader
            // 
            this.packsColumnHeader.Text = "Packs";
            this.packsColumnHeader.Width = 102;
            // 
            // remoteHostListContextMenuStrip
            // 
            this.remoteHostListContextMenuStrip.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.monitorPacksToolStripMenuItem,
            this.removeToolStripMenuItem,
            this.testToolStripMenuItem,
            this.toolStripMenuItem1,
            this.refreshToolStripMenuItem});
            this.remoteHostListContextMenuStrip.Name = "contextMenuStrip1";
            this.remoteHostListContextMenuStrip.Size = new System.Drawing.Size(151, 98);
            // 
            // monitorPacksToolStripMenuItem
            // 
            this.monitorPacksToolStripMenuItem.Enabled = false;
            this.monitorPacksToolStripMenuItem.Name = "monitorPacksToolStripMenuItem";
            this.monitorPacksToolStripMenuItem.Size = new System.Drawing.Size(150, 22);
            this.monitorPacksToolStripMenuItem.Text = "Monitor packs";
            this.monitorPacksToolStripMenuItem.Click += new System.EventHandler(this.monitorPacksToolStripMenuItem_Click);
            // 
            // removeToolStripMenuItem
            // 
            this.removeToolStripMenuItem.Enabled = false;
            this.removeToolStripMenuItem.Name = "removeToolStripMenuItem";
            this.removeToolStripMenuItem.Size = new System.Drawing.Size(150, 22);
            this.removeToolStripMenuItem.Text = "Remove";
            this.removeToolStripMenuItem.Click += new System.EventHandler(this.removeToolStripMenuItem_Click);
            // 
            // testToolStripMenuItem
            // 
            this.testToolStripMenuItem.Enabled = false;
            this.testToolStripMenuItem.Name = "testToolStripMenuItem";
            this.testToolStripMenuItem.Size = new System.Drawing.Size(150, 22);
            this.testToolStripMenuItem.Text = "Test";
            this.testToolStripMenuItem.Click += new System.EventHandler(this.lvwRemoteHosts_DoubleClick);
            // 
            // toolStripMenuItem1
            // 
            this.toolStripMenuItem1.Name = "toolStripMenuItem1";
            this.toolStripMenuItem1.Size = new System.Drawing.Size(147, 6);
            // 
            // refreshToolStripMenuItem
            // 
            this.refreshToolStripMenuItem.Name = "refreshToolStripMenuItem";
            this.refreshToolStripMenuItem.ShortcutKeys = System.Windows.Forms.Keys.F5;
            this.refreshToolStripMenuItem.Size = new System.Drawing.Size(150, 22);
            this.refreshToolStripMenuItem.Text = "Refresh";
            this.refreshToolStripMenuItem.Click += new System.EventHandler(this.refreshToolStripMenuItem_Click);
            // 
            // remoteHostStatusImageList
            // 
            this.remoteHostStatusImageList.ImageStream = ((System.Windows.Forms.ImageListStreamer)(resources.GetObject("remoteHostStatusImageList.ImageStream")));
            this.remoteHostStatusImageList.TransparentColor = System.Drawing.Color.Transparent;
            this.remoteHostStatusImageList.Images.SetKeyName(0, "GUnknown.ico");
            this.remoteHostStatusImageList.Images.SetKeyName(1, "GRunning.ico");
            this.remoteHostStatusImageList.Images.SetKeyName(2, "GBusy.ico");
            this.remoteHostStatusImageList.Images.SetKeyName(3, "GStopped.ico");
            this.remoteHostStatusImageList.Images.SetKeyName(4, "GPaused.ico");
            // 
            // refreshTimer
            // 
            this.refreshTimer.Tick += new System.EventHandler(this.refreshTimer_Tick);
            // 
            // quickMonServiceOpenFileDialog
            // 
            this.quickMonServiceOpenFileDialog.DefaultExt = "exe";
            this.quickMonServiceOpenFileDialog.FileName = "QuickMonService.exe";
            this.quickMonServiceOpenFileDialog.Filter = "QuickMon 4 Service|QuickMonService.exe";
            this.quickMonServiceOpenFileDialog.Title = "Select QuickMon 4 Service";
            // 
            // cmdEditMonitorPackList
            // 
            this.cmdEditMonitorPackList.BackColor = System.Drawing.Color.Transparent;
            this.cmdEditMonitorPackList.BackgroundImage = global::QuickMon.Properties.Resources.doc_edit;
            this.cmdEditMonitorPackList.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Zoom;
            this.cmdEditMonitorPackList.FlatAppearance.BorderColor = System.Drawing.Color.Teal;
            this.cmdEditMonitorPackList.FlatAppearance.BorderSize = 0;
            this.cmdEditMonitorPackList.FlatStyle = System.Windows.Forms.FlatStyle.Popup;
            this.cmdEditMonitorPackList.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.cmdEditMonitorPackList.Location = new System.Drawing.Point(36, 1);
            this.cmdEditMonitorPackList.Name = "cmdEditMonitorPackList";
            this.cmdEditMonitorPackList.Size = new System.Drawing.Size(28, 28);
            this.cmdEditMonitorPackList.TabIndex = 1;
            this.toolTip1.SetToolTip(this.cmdEditMonitorPackList, "Edit MonitorPackList.txt");
            this.cmdEditMonitorPackList.UseVisualStyleBackColor = false;
            this.cmdEditMonitorPackList.Visible = false;
            this.cmdEditMonitorPackList.Click += new System.EventHandler(this.cmdEditMonitorPackList_Click);
            // 
            // RemoteAgentHosts
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.White;
            this.ClientSize = new System.Drawing.Size(701, 334);
            this.Controls.Add(this.panel2);
            this.Controls.Add(this.panel1);
            this.Controls.Add(this.shadePanel1);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.MinimumSize = new System.Drawing.Size(600, 320);
            this.Name = "RemoteAgentHosts";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Remote Hosts";
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.RemoteAgentHosts_FormClosing);
            this.Load += new System.EventHandler(this.RemoteAgentHosts_Load);
            this.Shown += new System.EventHandler(this.RemoteAgentHosts_Shown);
            this.shadePanel1.ResumeLayout(false);
            this.panel1.ResumeLayout(false);
            this.panel1.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.remoteportNumericUpDown)).EndInit();
            this.panel2.ResumeLayout(false);
            this.panel2.PerformLayout();
            this.remoteHostListContextMenuStrip.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Panel shadePanel1;
        private System.Windows.Forms.Panel panel1;
        private System.Windows.Forms.LinkLabel llblStartLocalService;
        private System.Windows.Forms.LinkLabel llblFirewallRule;
        private System.Windows.Forms.Button cmdAdd;
        private System.Windows.Forms.NumericUpDown remoteportNumericUpDown;
        private System.Windows.Forms.Label label14;
        private System.Windows.Forms.TextBox txtComputer;
        private System.Windows.Forms.Label lblComputer;
        private ListViewEx lvwRemoteHosts;
        private System.Windows.Forms.ColumnHeader remoteAgentColumnHeader;
        private System.Windows.Forms.ColumnHeader portColumnHeader;
        private System.Windows.Forms.ColumnHeader versionColumnHeader;
        private System.Windows.Forms.ColumnHeader packsColumnHeader;
        private System.Windows.Forms.Panel panel2;
        private System.Windows.Forms.ImageList remoteHostStatusImageList;
        private System.Windows.Forms.ContextMenuStrip remoteHostListContextMenuStrip;
        private System.Windows.Forms.ToolStripMenuItem monitorPacksToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem removeToolStripMenuItem;
        private System.Windows.Forms.ToolStripSeparator toolStripMenuItem1;
        private System.Windows.Forms.ToolStripMenuItem refreshToolStripMenuItem;
        private System.Windows.Forms.Timer refreshTimer;
        private System.Windows.Forms.LinkLabel llblLocalServiceRegistered;
        private System.Windows.Forms.OpenFileDialog quickMonServiceOpenFileDialog;
        private System.Windows.Forms.Button cmdRefresh;
        private System.Windows.Forms.ToolStripMenuItem testToolStripMenuItem;
        private System.Windows.Forms.Button cmdEditMonitorPackList;
        private System.Windows.Forms.ToolTip toolTip1;
    }
}