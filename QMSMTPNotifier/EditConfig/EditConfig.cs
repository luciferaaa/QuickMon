﻿using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Net.Mail;

namespace QuickMon
{
    public partial class EditConfig : Form
    {
        public EditConfig()
        {
            InitializeComponent();
        }

        public MailSettings MailSettings { get; set; }

        private void chkUseDefultCredentials_CheckedChanged(object sender, EventArgs e)
        {
            groupBoxCredentials.Enabled = ! chkUseDefultCredentials.Checked;
        }

        public DialogResult ShowConfig()
        {
            txtSMTPServer.Text = MailSettings.HostServer;
            chkUseDefultCredentials.Checked = MailSettings.UseDefaultCredentials;
            txtDomain.Text = MailSettings.Domain;
            txtUserName.Text = MailSettings.UserName;
            txtPassword.Text = MailSettings.Password;
            txtFromAddress.Text = MailSettings.FromAddress;
            txtToAddress.Text = MailSettings.ToAddress;
            txtSubject.Text = MailSettings.Subject;
            chkIsBodyHtml.Checked = MailSettings.IsBodyHtml;
            txtBody.Text = MailSettings.Body;
            return ShowDialog();
        }

        private void cmdOK_Click(object sender, EventArgs e)
        {
            MailSettings.HostServer = txtSMTPServer.Text;
            MailSettings.UseDefaultCredentials = chkUseDefultCredentials.Checked;
            MailSettings.Domain = txtDomain.Text;
            MailSettings.UserName = txtUserName.Text;
            MailSettings.Password = txtPassword.Text;
            MailSettings.FromAddress = txtFromAddress.Text;
            MailSettings.ToAddress = txtToAddress.Text;
            MailSettings.Subject = txtSubject.Text;
            MailSettings.IsBodyHtml = chkIsBodyHtml.Checked;
            MailSettings.Body = txtBody.Text;
            DialogResult = System.Windows.Forms.DialogResult.OK;
            Close();
        }

        private void cmdTest_Click(object sender, EventArgs e)
        {
            string lastStep = "";
            try
            {
                if (txtSMTPServer.Text.Length == 0)
                {
                    throw new Exception("SMTP host server not specified!");
                }
                lastStep = "Setting up SMTP client details";
                using (SmtpClient smtpClient = new SmtpClient())
                {
                    smtpClient.Host = txtSMTPServer.Text;
                    smtpClient.UseDefaultCredentials = chkUseDefultCredentials.Checked;
                    if (!chkUseDefultCredentials.Checked)
                    {
                        lastStep = "Setting up non default credentials";
                        System.Net.NetworkCredential cr = new System.Net.NetworkCredential();
                        cr.Domain = txtDomain.Text;
                        cr.UserName = txtUserName.Text;
                        cr.Password = txtPassword.Text;
                        smtpClient.Credentials = cr;
                    }
                    MailMessage mailMessage = new MailMessage(txtFromAddress.Text, txtToAddress.Text);

                    lastStep = "Setting up mail body";
                    if (chkIsBodyHtml.Checked)
                        mailMessage.Body = "<b>Test</b><br />Test was successful";
                    else
                        mailMessage.Body = "Test was successful";
                    mailMessage.IsBodyHtml = chkIsBodyHtml.Checked;
                    mailMessage.Subject = txtSubject.Text;
                    lastStep = "Sending SMTP mail";
                    smtpClient.Send(mailMessage);
                    MessageBox.Show("Test completed successfully", "Test", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(string.Format("Test failed\r\nLast step: {0}\r\n{1}", lastStep, ex.Message), "Test", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
    }
}