﻿namespace QuickMon.Notifiers
{
    partial class EventLogNotifierEditConfig
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(EventLogNotifierEditConfig));
            this.label10 = new System.Windows.Forms.Label();
            this.label3 = new System.Windows.Forms.Label();
            this.txtMachine = new System.Windows.Forms.TextBox();
            this.label2 = new System.Windows.Forms.Label();
            this.txtEventSource = new System.Windows.Forms.TextBox();
            this.label1 = new System.Windows.Forms.Label();
            this.label4 = new System.Windows.Forms.Label();
            this.successEventIDNumericUpDown = new System.Windows.Forms.NumericUpDown();
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.errorEventIDNumericUpDown = new System.Windows.Forms.NumericUpDown();
            this.label6 = new System.Windows.Forms.Label();
            this.warningEventIDNumericUpDown = new System.Windows.Forms.NumericUpDown();
            this.label5 = new System.Windows.Forms.Label();
            ((System.ComponentModel.ISupportInitialize)(this.successEventIDNumericUpDown)).BeginInit();
            this.groupBox1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.errorEventIDNumericUpDown)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.warningEventIDNumericUpDown)).BeginInit();
            this.SuspendLayout();
            // 
            // cmdCancel
            // 
            this.cmdCancel.Location = new System.Drawing.Point(511, 223);
            // 
            // cmdOK
            // 
            this.cmdOK.Location = new System.Drawing.Point(430, 223);
            // 
            // label10
            // 
            this.label10.Location = new System.Drawing.Point(105, 62);
            this.label10.Name = "label10";
            this.label10.Size = new System.Drawing.Size(325, 36);
            this.label10.TabIndex = 4;
            this.label10.Text = "The Event source may contain the following macros: %CollectorName% %CollectorType" +
    "% ";
            // 
            // label3
            // 
            this.label3.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.label3.ForeColor = System.Drawing.Color.Firebrick;
            this.label3.Location = new System.Drawing.Point(9, 161);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(543, 58);
            this.label3.TabIndex = 6;
            this.label3.Text = resources.GetString("label3.Text");
            // 
            // txtMachine
            // 
            this.txtMachine.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.txtMachine.Location = new System.Drawing.Point(108, 7);
            this.txtMachine.Name = "txtMachine";
            this.txtMachine.Size = new System.Drawing.Size(478, 20);
            this.txtMachine.TabIndex = 1;
            this.txtMachine.Text = ".";
            this.txtMachine.TextChanged += new System.EventHandler(this.txtMachine_TextChanged);
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(7, 10);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(52, 13);
            this.label2.TabIndex = 0;
            this.label2.Text = "Computer";
            // 
            // txtEventSource
            // 
            this.txtEventSource.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.txtEventSource.Location = new System.Drawing.Point(108, 33);
            this.txtEventSource.Name = "txtEventSource";
            this.txtEventSource.Size = new System.Drawing.Size(478, 20);
            this.txtEventSource.TabIndex = 3;
            this.txtEventSource.Text = "QuickMon";
            this.txtEventSource.TextChanged += new System.EventHandler(this.txtEventSource_TextChanged);
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(7, 36);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(70, 13);
            this.label1.TabIndex = 2;
            this.label1.Text = "Event source";
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(6, 21);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(48, 13);
            this.label4.TabIndex = 0;
            this.label4.Text = "Success";
            // 
            // successEventIDNumericUpDown
            // 
            this.successEventIDNumericUpDown.Location = new System.Drawing.Point(98, 19);
            this.successEventIDNumericUpDown.Maximum = new decimal(new int[] {
            -2147483648,
            0,
            0,
            0});
            this.successEventIDNumericUpDown.Name = "successEventIDNumericUpDown";
            this.successEventIDNumericUpDown.Size = new System.Drawing.Size(98, 20);
            this.successEventIDNumericUpDown.TabIndex = 1;
            // 
            // groupBox1
            // 
            this.groupBox1.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.groupBox1.Controls.Add(this.errorEventIDNumericUpDown);
            this.groupBox1.Controls.Add(this.label6);
            this.groupBox1.Controls.Add(this.warningEventIDNumericUpDown);
            this.groupBox1.Controls.Add(this.label5);
            this.groupBox1.Controls.Add(this.successEventIDNumericUpDown);
            this.groupBox1.Controls.Add(this.label4);
            this.groupBox1.Location = new System.Drawing.Point(12, 101);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(574, 53);
            this.groupBox1.TabIndex = 5;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "Event ID\'s";
            // 
            // errorEventIDNumericUpDown
            // 
            this.errorEventIDNumericUpDown.Location = new System.Drawing.Point(428, 19);
            this.errorEventIDNumericUpDown.Maximum = new decimal(new int[] {
            -2147483648,
            0,
            0,
            0});
            this.errorEventIDNumericUpDown.Name = "errorEventIDNumericUpDown";
            this.errorEventIDNumericUpDown.Size = new System.Drawing.Size(98, 20);
            this.errorEventIDNumericUpDown.TabIndex = 5;
            // 
            // label6
            // 
            this.label6.AutoSize = true;
            this.label6.Location = new System.Drawing.Point(382, 21);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(29, 13);
            this.label6.TabIndex = 4;
            this.label6.Text = "Error";
            // 
            // warningEventIDNumericUpDown
            // 
            this.warningEventIDNumericUpDown.Location = new System.Drawing.Point(278, 19);
            this.warningEventIDNumericUpDown.Maximum = new decimal(new int[] {
            -2147483648,
            0,
            0,
            0});
            this.warningEventIDNumericUpDown.Name = "warningEventIDNumericUpDown";
            this.warningEventIDNumericUpDown.Size = new System.Drawing.Size(98, 20);
            this.warningEventIDNumericUpDown.TabIndex = 3;
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Location = new System.Drawing.Point(211, 21);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(47, 13);
            this.label5.TabIndex = 2;
            this.label5.Text = "Warning";
            // 
            // EventLogNotifierEditConfig
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(600, 254);
            this.Controls.Add(this.groupBox1);
            this.Controls.Add(this.label10);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.txtMachine);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.txtEventSource);
            this.Controls.Add(this.label1);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.MinimumSize = new System.Drawing.Size(570, 270);
            this.Name = "EventLogNotifierEditConfig";
            this.Text = "EventLogNotifierEditConfig";
            this.Controls.SetChildIndex(this.cmdOK, 0);
            this.Controls.SetChildIndex(this.cmdCancel, 0);
            this.Controls.SetChildIndex(this.label1, 0);
            this.Controls.SetChildIndex(this.txtEventSource, 0);
            this.Controls.SetChildIndex(this.label2, 0);
            this.Controls.SetChildIndex(this.txtMachine, 0);
            this.Controls.SetChildIndex(this.label3, 0);
            this.Controls.SetChildIndex(this.label10, 0);
            this.Controls.SetChildIndex(this.groupBox1, 0);
            ((System.ComponentModel.ISupportInitialize)(this.successEventIDNumericUpDown)).EndInit();
            this.groupBox1.ResumeLayout(false);
            this.groupBox1.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.errorEventIDNumericUpDown)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.warningEventIDNumericUpDown)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label label10;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.TextBox txtMachine;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.TextBox txtEventSource;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.NumericUpDown successEventIDNumericUpDown;
        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.NumericUpDown errorEventIDNumericUpDown;
        private System.Windows.Forms.Label label6;
        private System.Windows.Forms.NumericUpDown warningEventIDNumericUpDown;
        private System.Windows.Forms.Label label5;
    }
}