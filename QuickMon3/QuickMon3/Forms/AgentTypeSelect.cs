﻿using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace QuickMon.Forms
{
    public partial class AgentTypeSelect : Form
    {
        public AgentTypeSelect()
        {
            InitializeComponent();
        }
        public RegisteredAgent SelectedAgent { get; set; }
        public bool ImportConfigAfterSelect { get; set; }

        public DialogResult ShowNotifierSelection(string currentNotifierRegistrationName)
        {
            this.Text = "Select Notifier type";
            lvwAgentType.AutoResizeColumnIndex = 1;
            lvwAgentType.AutoResizeColumnEnabled = true;
            lvwAgentType.Items.Clear();
            ListViewItem lvi;
            foreach (RegisteredAgent ar in (from a in RegisteredAgentCache.Agents
                                            where a.IsNotifier
                                            orderby a.Name
                                            select a))
            {
                lvi = new ListViewItem(ar.DisplayName);
                string details = ar.ClassName;
                System.Reflection.Assembly a = System.Reflection.Assembly.LoadFrom(ar.AssemblyPath);
                details += ", Version: " + a.GetName().Version.ToString();
                details += ", Assembly: " + System.IO.Path.GetFileName(a.Location);

                lvi.SubItems.Add(details);
                lvi.Tag = ar;
                lvwAgentType.Items.Add(lvi);
                if (ar.Name == currentNotifierRegistrationName)
                    lvi.Selected = true;
            }
            return this.ShowDialog();
        }
        public DialogResult ShowCollectorSelection(string currentCollectorRegistrationName)
        {
            this.Text = "Select Collector type";
            lvwAgentType.AutoResizeColumnIndex = 1;
            lvwAgentType.AutoResizeColumnEnabled = true;
            lvwAgentType.Items.Clear();
            lvwAgentType.Groups.Clear();
            
            ListViewGroup generalGroup = new ListViewGroup("General");
            lvwAgentType.Groups.Add(generalGroup);
            foreach(string categoryName in (from a in RegisteredAgentCache.Agents
                                            where a.IsCollector && a.CategoryName != "Test"
                                            group a by a.CategoryName into g
                                            select g.Key))
            {
                lvwAgentType.Groups.Add(new ListViewGroup(categoryName));
            }
            ListViewGroup folderGroup = new ListViewGroup("Folder");
            lvwAgentType.Groups.Add(folderGroup);
            ListViewGroup testGroup = new ListViewGroup("Test");
            lvwAgentType.Groups.Add(testGroup);

            RegisteredAgent folder = new RegisteredAgent() { ClassName = "QuickMon.Collectors.Folder", Name = "Folder", IsCollector = true, DisplayName = "Folder" };
            ListViewItem lvi = new ListViewItem("Folder");
            lvi.Group = folderGroup;
            lvi.ImageIndex = 1;
            lvi.SubItems.Add("Container for child objects");
            lvi.Tag = folder;
            lvwAgentType.Items.Add(lvi);
            if (currentCollectorRegistrationName == "Folder")
                lvi.Selected = true;

            foreach (RegisteredAgent ar in (from a in RegisteredAgentCache.Agents
                                            where a.IsCollector
                                            orderby a.Name
                                            select a))
            {
                ListViewGroup agentGroup = (from ListViewGroup gr in lvwAgentType.Groups
                                            where gr.Header.ToLower() == ar.CategoryName.ToLower()
                                            select gr).FirstOrDefault();
                if (agentGroup == null)
                    agentGroup = generalGroup;

                lvi = new ListViewItem(ar.DisplayName);
                string details = ar.ClassName;
                System.Reflection.Assembly a = System.Reflection.Assembly.LoadFrom(ar.AssemblyPath);
                details += ", Version: " + a.GetName().Version.ToString();
                details += ", Assembly: " + System.IO.Path.GetFileName(a.Location);

                if (agentGroup == testGroup)
                    lvi.ImageIndex = 2;
                else
                    lvi.ImageIndex = 0;
                lvi.Group = agentGroup;
                lvi.SubItems.Add(details);
                lvi.Tag = ar;
                lvwAgentType.Items.Add(lvi);
                if (ar.Name == currentCollectorRegistrationName)
                    lvi.Selected = true;
            }
            return this.ShowDialog();
        }

        private void lvwAgentType_SelectedIndexChanged(object sender, EventArgs e)
        {
            cmdOK.Enabled = lvwAgentType.SelectedItems.Count == 1;
        }

        private void cmdOK_Click(object sender, EventArgs e)
        {
            if (lvwAgentType.SelectedItems.Count ==1)
            {
                SelectedAgent = (RegisteredAgent)lvwAgentType.SelectedItems[0].Tag;
                ImportConfigAfterSelect = chkImportConfigAfterSelect.Checked;
                DialogResult = System.Windows.Forms.DialogResult.OK;
                Close();
            }
        }

        private void lvwAgentType_EnterKeyPressed()
        {
            cmdOK_Click(null, null);
        }
    }
}
