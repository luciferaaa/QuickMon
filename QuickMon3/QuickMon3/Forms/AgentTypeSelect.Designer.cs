﻿namespace QuickMon.Forms
{
    partial class AgentTypeSelect
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(AgentTypeSelect));
            this.cmdCancel = new System.Windows.Forms.Button();
            this.cmdOK = new System.Windows.Forms.Button();
            this.lvwAgentType = new QuickMon.ListViewEx();
            this.nameColumnHeader = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.detailsColumnHeader = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.chkImportConfigAfterSelect = new System.Windows.Forms.CheckBox();
            this.imageList1 = new System.Windows.Forms.ImageList(this.components);
            this.SuspendLayout();
            // 
            // cmdCancel
            // 
            this.cmdCancel.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.cmdCancel.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.cmdCancel.FlatStyle = System.Windows.Forms.FlatStyle.Popup;
            this.cmdCancel.Location = new System.Drawing.Point(496, 445);
            this.cmdCancel.Name = "cmdCancel";
            this.cmdCancel.Size = new System.Drawing.Size(75, 23);
            this.cmdCancel.TabIndex = 2;
            this.cmdCancel.Text = "Cancel";
            this.cmdCancel.UseVisualStyleBackColor = true;
            // 
            // cmdOK
            // 
            this.cmdOK.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.cmdOK.Enabled = false;
            this.cmdOK.FlatStyle = System.Windows.Forms.FlatStyle.Popup;
            this.cmdOK.Location = new System.Drawing.Point(415, 445);
            this.cmdOK.Name = "cmdOK";
            this.cmdOK.Size = new System.Drawing.Size(75, 23);
            this.cmdOK.TabIndex = 1;
            this.cmdOK.Text = "OK";
            this.cmdOK.UseVisualStyleBackColor = true;
            this.cmdOK.Click += new System.EventHandler(this.cmdOK_Click);
            // 
            // lvwAgentType
            // 
            this.lvwAgentType.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.lvwAgentType.AutoResizeColumnEnabled = false;
            this.lvwAgentType.AutoResizeColumnIndex = 0;
            this.lvwAgentType.Columns.AddRange(new System.Windows.Forms.ColumnHeader[] {
            this.nameColumnHeader,
            this.detailsColumnHeader});
            this.lvwAgentType.FullRowSelect = true;
            this.lvwAgentType.HideSelection = false;
            this.lvwAgentType.Location = new System.Drawing.Point(0, 0);
            this.lvwAgentType.MultiSelect = false;
            this.lvwAgentType.Name = "lvwAgentType";
            this.lvwAgentType.Size = new System.Drawing.Size(583, 439);
            this.lvwAgentType.SmallImageList = this.imageList1;
            this.lvwAgentType.TabIndex = 0;
            this.lvwAgentType.UseCompatibleStateImageBehavior = false;
            this.lvwAgentType.View = System.Windows.Forms.View.Details;
            this.lvwAgentType.EnterKeyPressed += new System.Windows.Forms.MethodInvoker(this.lvwAgentType_EnterKeyPressed);
            this.lvwAgentType.SelectedIndexChanged += new System.EventHandler(this.lvwAgentType_SelectedIndexChanged);
            this.lvwAgentType.DoubleClick += new System.EventHandler(this.cmdOK_Click);
            // 
            // nameColumnHeader
            // 
            this.nameColumnHeader.Text = "Name";
            this.nameColumnHeader.Width = 213;
            // 
            // detailsColumnHeader
            // 
            this.detailsColumnHeader.Text = "Details";
            this.detailsColumnHeader.Width = 341;
            // 
            // chkImportConfigAfterSelect
            // 
            this.chkImportConfigAfterSelect.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.chkImportConfigAfterSelect.AutoSize = true;
            this.chkImportConfigAfterSelect.FlatStyle = System.Windows.Forms.FlatStyle.Popup;
            this.chkImportConfigAfterSelect.Location = new System.Drawing.Point(12, 449);
            this.chkImportConfigAfterSelect.Name = "chkImportConfigAfterSelect";
            this.chkImportConfigAfterSelect.Size = new System.Drawing.Size(268, 17);
            this.chkImportConfigAfterSelect.TabIndex = 3;
            this.chkImportConfigAfterSelect.Text = "Show custom config after selection (Import possible)";
            this.chkImportConfigAfterSelect.UseVisualStyleBackColor = true;
            // 
            // imageList1
            // 
            this.imageList1.ImageStream = ((System.Windows.Forms.ImageListStreamer)(resources.GetObject("imageList1.ImageStream")));
            this.imageList1.TransparentColor = System.Drawing.Color.Transparent;
            this.imageList1.Images.SetKeyName(0, "58.ico");
            this.imageList1.Images.SetKeyName(1, "5_50.ico");
            this.imageList1.Images.SetKeyName(2, "59.ico");
            // 
            // AgentTypeSelect
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(582, 477);
            this.Controls.Add(this.chkImportConfigAfterSelect);
            this.Controls.Add(this.lvwAgentType);
            this.Controls.Add(this.cmdCancel);
            this.Controls.Add(this.cmdOK);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.MinimizeBox = false;
            this.Name = "AgentTypeSelect";
            this.SizeGripStyle = System.Windows.Forms.SizeGripStyle.Show;
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Agent Type Select";
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Button cmdCancel;
        private System.Windows.Forms.Button cmdOK;
        private ListViewEx lvwAgentType;
        private System.Windows.Forms.ColumnHeader nameColumnHeader;
        private System.Windows.Forms.ColumnHeader detailsColumnHeader;
        private System.Windows.Forms.CheckBox chkImportConfigAfterSelect;
        private System.Windows.Forms.ImageList imageList1;
    }
}