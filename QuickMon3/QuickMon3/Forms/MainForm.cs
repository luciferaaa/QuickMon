﻿using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using QuickMon.Forms;
using QuickMon.Controls;

namespace QuickMon
{
    public partial class MainForm : FadeSnapForm
    {
        public MainForm()
        {
            InitializeComponent();

            popedContainerForTreeView = new Controls.CollectorContextMenuControl();
            poperContainerForTreeView = new Controls.PoperContainer(popedContainerForTreeView);
            popedContainerForListView = new Controls.NotifierContextMenuControl();
            poperContainerForListView = new Controls.PoperContainer(popedContainerForListView);
        }

        #region TreeNodeImage contants
        private readonly int collectorRootImage = 0;
        private readonly int collectorFolderImage = 1;
        private readonly int collectorNAstateImage = 2;
        private readonly int collectorGoodStateImage1 = 3;
        private readonly int collectorGoodStateImage2 = 6;
        private readonly int collectorWarningStateImage1 = 4;
        private readonly int collectorWarningStateImage2 = 7;
        private readonly int collectorErrorStateImage1 = 5;
        private readonly int collectorErrorStateImage2 = 8;
        private readonly int collectorDisabled = 9;
        #endregion

        #region Private vars
        private int refreshCycleCounter = 0;
        private bool monitorPackChanged = false;
        private bool firstRefresh = false;
        private MonitorPack monitorPack;
        private string quickMonPCCategory = "QuickMon 3 UI Client";
        private Point collectorContextMenuLaunchPoint = new Point();
        private Point notifierContextMenuLaunchPoint = new Point();
        #region Performance Counter Vars
        private PerformanceCounter collectorErrorStatePerSec = null;
        private PerformanceCounter collectorWarningStatePerSec = null;
        private PerformanceCounter collectorInfoStatePerSec = null;
        private PerformanceCounter collectorsQueriedPerSecond = null;
        private PerformanceCounter collectorsQueryTime = null;
        private PerformanceCounter selectedCollectorsQueryTime = null;
        #endregion

        private QuickMon.Controls.CollectorContextMenuControl popedContainerForTreeView;//a user control that is derievd from PopedContainer; it can contain any type of controls and you design it as if you design a form!!!
        private QuickMon.Controls.PoperContainer poperContainerForTreeView;//the container... which displays previous user control as a context poped up menu
        private QuickMon.Controls.NotifierContextMenuControl popedContainerForListView;
        private QuickMon.Controls.PoperContainer poperContainerForListView;

        private List<CollectorEntry> copiedCollectorList = new List<CollectorEntry>();
        private List<CollectorStats> collectorStatsWindows = new List<CollectorStats>();
        #endregion       

        #region Form events
        private void MainForm_Load(object sender, EventArgs e)
        {
            cboRecentMonitorPacks.Visible = false;
            cmdRecentMonitorPacks.Visible = false;
            recentMonitorPackToolStripMenuItem2.Visible = false;
            lblNoNotifiersYet.Dock = DockStyle.Fill;
            popedContainerForTreeView.cmdCopy.Enabled = false;
            popedContainerForTreeView.cmdPaste.Enabled = false;
            popedContainerForTreeView.cmdPasteWithEdit.Enabled = false;
            popedContainerForTreeView.cmdStats.Enabled = false;
            popedContainerForTreeView.cmdCopy.Click += new System.EventHandler(collectorContextMenuCmdCopy_Click);
            popedContainerForTreeView.cmdPaste.Click += new System.EventHandler(collectorContextMenuCmdPaste_Click);
            popedContainerForTreeView.cmdPasteWithEdit.Click += new System.EventHandler(collectorContextMenuCmdPasteWithEdit_Click);
            popedContainerForTreeView.cmdViewDetails.Click += new System.EventHandler(collectorTreeViewDetailsToolStripMenuItem_Click);
            popedContainerForTreeView.cmdAddFolder.Click += new System.EventHandler(addCollectorFolderToolStripMenuItem_Click);
            popedContainerForTreeView.cmdAddCollector.Click += new System.EventHandler(addCollectorToolStripMenuItem_Click);
            popedContainerForTreeView.cmdEditCollector.Click += new System.EventHandler(collectorTreeEditConfigToolStripMenuItem_Click);
            popedContainerForTreeView.cmdDisableCollector.Click += new System.EventHandler(disableCollectorTreeToolStripMenuItem_Click);
            popedContainerForTreeView.cmdDeleteCollector.Click += new System.EventHandler(removeCollectorToolStripMenuItem_Click);
            popedContainerForTreeView.cmdStats.Click += new System.EventHandler(cmdStats_Click);
            popedContainerForTreeView.cmdRefresh.Click += new EventHandler(refreshToolStripButton_Click);
            popedContainerForTreeView.cmdNewMonitorPack.Click += new EventHandler(newMonitorPackToolStripMenuItem_Click);
            popedContainerForTreeView.cmdLoadMonitorPack.Click += new EventHandler(openMonitorPackToolStripButton_ButtonClick);
            popedContainerForTreeView.cmdLoadRecentMonitorPack.Click += new EventHandler(recentMonitorPackToolStripMenuItem1_Click);
            popedContainerForTreeView.cmdSaveMonitorPack.Click += new EventHandler(saveAsMonitorPackToolStripMenuItem_ButtonClick);
            popedContainerForTreeView.cmdGeneralSettings.Click += new EventHandler(generalSettingsToolStripSplitButton_ButtonClick);
            popedContainerForTreeView.cmdAbout.Click += new EventHandler(aboutToolStripMenuItem_Click);            

            popedContainerForListView.cmdViewDetails.Click += new System.EventHandler(notifierViewerToolStripMenuItem_Click);
            popedContainerForListView.cmdAddNotifier.Click += new System.EventHandler(addNotifierToolStripMenuItem_Click);
            popedContainerForListView.cmdEditNotifier.Click += new System.EventHandler(notifierConfigurationToolStripMenuItem_Click);
            popedContainerForListView.cmdDisableNotifier.Click += new System.EventHandler(disableNotifierToolStripMenuItem_Click);
            popedContainerForListView.cmdDeleteNotifier.Click += new System.EventHandler(removeNotifierToolStripMenuItem_Click);

            if ((Properties.Settings.Default.MainWindowLocation.X == 0) && (Properties.Settings.Default.MainWindowLocation.Y == 0)
                && (Properties.Settings.Default.MainWindowSize.Width == 0))
            {
                this.Location = new Point((Screen.PrimaryScreen.WorkingArea.Width - this.Width) / 2, (Screen.PrimaryScreen.WorkingArea.Height - this.Height) / 2);
            }
            else
            {
                this.Location = Properties.Settings.Default.MainWindowLocation;
                this.Size = Properties.Settings.Default.MainWindowSize;
            }
            SnappingEnabled = Properties.Settings.Default.MainFormSnap;            
            masterSplitContainer.Panel2Collapsed = true;
            MainForm_Resize(null, null);
            lblVersion.Text = string.Format("v{0}.{1}", System.Reflection.Assembly.GetExecutingAssembly().GetName().Version.Major, System.Reflection.Assembly.GetExecutingAssembly().GetName().Version.Minor);
            toolTip1.SetToolTip(lblVersion, string.Format("v{0} ({1})", System.Reflection.Assembly.GetExecutingAssembly().GetName().Version, new System.IO.FileInfo(System.Reflection.Assembly.GetExecutingAssembly().Location).LastWriteTime.ToString("yyyy-MM-dd")));
            tvwCollectors.EnableAutoScrollToSelectedNode = true;
            tvwCollectors.TreeNodeMoved += tvwCollectors_TreeNodeMoved;
            tvwCollectors.EnterKeyDown += tvwCollectors_EnterKeyDown;
            tvwCollectors.RootAlwaysExpanded = true;
            tvwCollectors.ContextMenuShowUp += tvwCollectors_ContextMenuShowUp;
            adminModeToolStripStatusLabel.Visible = HenIT.Security.AdminModeTools.IsInAdminMode();
            restartInAdminModeToolStripMenuItem.Visible = !HenIT.Security.AdminModeTools.IsInAdminMode();
        }
        private void MainForm_Resize(object sender, EventArgs e)
        {
            try
            {
                cboRecentMonitorPacks.DropDownWidth = this.ClientSize.Width - cboRecentMonitorPacks.Left - recentMonitorPacksPanel.Left;
                resizeRecentDropDownListWidthTimer.Enabled = false;
                resizeRecentDropDownListWidthTimer.Enabled = true;
            }
            catch { }
        }
        private void MainForm_Shown(object sender, EventArgs e)
        {
            try
            {
                InitializeGlobalPerformanceCounters();

                mainRefreshTimer.Interval = Properties.Settings.Default.PollFrequencySec * 1000;

                if (Properties.Settings.Default.LastMonitorPack != null && System.IO.File.Exists(Properties.Settings.Default.LastMonitorPack))
                {
                    LoadMonitorPack(Properties.Settings.Default.LastMonitorPack);
                    System.Threading.Thread.Sleep(100);
                    RefreshMonitorPack();
                }
                else
                {
                    monitorPack = new MonitorPack();
                    monitorPack.ConcurrencyLevel = Properties.Settings.Default.ConcurrencyLevel;
                    monitorPack.CollectorCurrentStateReported += monitorPack_CollectorCurrentStateReported;
                    monitorPack.OnNotifierError += monitorPack_RaiseNotifierError;
                    monitorPack.RunCollectorCorrectiveWarningScript += monitorPack_RunCollectorCorrectiveWarningScript;
                    monitorPack.RunCollectorCorrectiveErrorScript += monitorPack_RunCollectorCorrectiveErrorScript;
                    monitorPack.RunRestorationScript += monitorPack_RunRestorationScript;
                    monitorPack.CollectorCalled += monitorPack_CollectorCalled;
                    lblNoNotifiersYet.Visible = true;
                }
                monitorPack.ConcurrencyLevel = Properties.Settings.Default.ConcurrencyLevel;
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            tvwCollectors.Focus();
            SetPollingFrequency();
        }
        private void MainForm_FormClosing(object sender, FormClosingEventArgs e)
        {
            PerformCleanShutdown();
        }
        private void MainForm_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.F5)
            {
                e.Handled = true;
                e.SuppressKeyPress = true;
                RefreshMonitorPack(true, true);
            }
            else if (e.Control && e.KeyCode == Keys.O)
            {
                e.Handled = true;
                e.SuppressKeyPress = true;
                openMonitorPackToolStripButton_ButtonClick(sender, e);
            }
            else if (e.Control && e.KeyCode == Keys.T)
            {
                e.Handled = true;
                e.SuppressKeyPress = true;
                recentMonitorPackToolStripMenuItem1_Click(sender, e);
            }
            else if (e.Control && e.KeyCode == Keys.N)
            {
                e.Handled = true;
                e.SuppressKeyPress = true;
                newMonitorPackToolStripMenuItem_Click(sender, e);
            }
            else if (e.Control && e.KeyCode == Keys.E)
            {
                e.Handled = true;
                e.SuppressKeyPress = true;
                generalSettingsToolStripSplitButton_ButtonClick(sender, e);
            }
            
        }
        #endregion

        #region Toolbar events
        private void openMonitorPackToolStripButton_ButtonClick(object sender, EventArgs e)
        {
            HideCollectorContextMenu();
            try
            {
                string startUpPath = MonitorPack.GetQuickMonUserDataDirectory();// System.IO.Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData, Environment.SpecialFolderOption.DoNotVerify), "Hen IT\\QuickMon");
                openFileDialogOpen.InitialDirectory = startUpPath;
                if (openFileDialogOpen.ShowDialog() == System.Windows.Forms.DialogResult.OK)
                {
                    CloseAllDetailWindows();
                    LoadMonitorPack(openFileDialogOpen.FileName);
                    RefreshMonitorPack(true,true);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Open", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
        private void recentMonitorPackToolStripMenuItem1_Click(object sender, EventArgs e)
        {
            HideCollectorContextMenu();
            RecentMonitorPacks recentMonitorPacks = new RecentMonitorPacks();
            if (recentMonitorPacks.ShowDialog() == System.Windows.Forms.DialogResult.OK)
            {
                CloseAllDetailWindows();
                LoadMonitorPack(recentMonitorPacks.SelectedPack);
                RefreshMonitorPack(true,true);
            }            
        }
        private void newMonitorPackToolStripMenuItem_Click(object sender, EventArgs e)
        {
            HideCollectorContextMenu();
            CloseAllDetailWindows();
            NewMonitorPack();            
        }
        private void saveAsMonitorPackToolStripMenuItem_Click(object sender, EventArgs e)
        {

        }
        private void saveAsToolStripMenuItem_Click(object sender, EventArgs e)
        {
            HideCollectorContextMenu();
            SaveAsMonitorPack();
        }
        private void saveAsMonitorPackToolStripMenuItem_ButtonClick(object sender, EventArgs e)
        {
            HideCollectorContextMenu();
            SaveMonitorPack();            
        }
        private void refreshToolStripButton_Click(object sender, EventArgs e)
        {
            HideCollectorContextMenu();
            RefreshMonitorPack(true, true);
        }
        private void showDefaultNotifierToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (monitorPack != null && monitorPack.DefaultViewerNotifier != null)
            {
                monitorPack.DefaultViewerNotifier.ShowViewer();
            }
        }
        private void showAllNotifiersToolStripMenuItem_Click(object sender, EventArgs e)
        {
            masterSplitContainer.Panel2Collapsed = false;
            llblNotifierViewToggle.Text = masterSplitContainer.Panel2Collapsed ? "Show Notifiers" : "Hide Notifiers";
        }
        private void generalSettingsToolStripSplitButton_ButtonClick(object sender, EventArgs e)
        {
            PausePolling(true);
            HideCollectorContextMenu();
            GeneralSettings generalSettings = new GeneralSettings();
            generalSettings.PollingFrequencySec = Properties.Settings.Default.PollFrequencySec;
            generalSettings.PollingEnabled = timerEnabled;
            if (generalSettings.ShowDialog() == System.Windows.Forms.DialogResult.OK)
            {
                LoadRecentMonitorPackList();
                this.SnappingEnabled = Properties.Settings.Default.MainFormSnap;
                if (monitorPack != null)
                    monitorPack.ConcurrencyLevel = Properties.Settings.Default.ConcurrencyLevel;

                Properties.Settings.Default.PollFrequencySec = generalSettings.PollingFrequencySec;                
                timerEnabled = generalSettings.PollingEnabled;                
            }
            ResumePolling();
        }
        private void pollingDisabledToolStripMenuItem_Click(object sender, EventArgs e)
        {
            mainRefreshTimer.Enabled = false;
            SetAppIcon(CollectorState.NotAvailable);
            UpdateStatusbar("Polling disabled");            
        }
        private void pollingSlowToolStripMenuItem_Click(object sender, EventArgs e)
        {
            mainRefreshTimer.Enabled = false;
            Properties.Settings.Default.OverridesMonitorPackFrequency = true;
            Properties.Settings.Default.PollFrequencySec = 60;
            SetPollingFrequency();
            UpdateStatusbar("Polling set to slow");
        }
        private void pollingNormalToolStripMenuItem_Click(object sender, EventArgs e)
        {
            mainRefreshTimer.Enabled = false;
            Properties.Settings.Default.OverridesMonitorPackFrequency = true;
            Properties.Settings.Default.PollFrequencySec = 30;
            SetPollingFrequency();
            UpdateStatusbar("Polling set to normal");
        }
        private void pollingFastToolStripMenuItem_Click(object sender, EventArgs e)
        {
            mainRefreshTimer.Enabled = false;
            Properties.Settings.Default.OverridesMonitorPackFrequency = true;
            Properties.Settings.Default.PollFrequencySec = 5;
            SetPollingFrequency();
            UpdateStatusbar("Polling set to fast");
        }
        private void customPollingFrequencyToolStripMenuItem_Click(object sender, EventArgs e)
        {
            HideCollectorContextMenu();
            generalSettingsToolStripSplitButton_ButtonClick(sender, e);
            //SetTimerConfig setTimerConfig = new SetTimerConfig();
            //setTimerConfig.FrequencySec = (mainRefreshTimer.Interval / 1000);
            //setTimerConfig.TimerEnabled = mainRefreshTimer.Enabled;
            //if (setTimerConfig.ShowDialog() == System.Windows.Forms.DialogResult.OK)
            //{
            //    mainRefreshTimer.Interval = setTimerConfig.FrequencySec * 1000;
            //    mainRefreshTimer.Enabled = setTimerConfig.TimerEnabled;
            //}
        }
        private void closeAllChildWindowsToolStripMenuItem_Click(object sender, EventArgs e)
        {
            CloseAllDetailWindows();
        }
        private void restartInAdminModeToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (!PerformCleanShutdown(true))
                return;
            HenIT.Security.AdminModeTools.RestartInAdminMode();
        }
        private void manageTemplatesToolStripMenuItem_Click(object sender, EventArgs e)
        {
            EditPresets editPresets = new EditPresets();
            editPresets.Show();
        }
        private void knownRemoteAgentsToolStripMenuItem_Click(object sender, EventArgs e)
        {
            QuickMon.Forms.RemoteAgentsManager ram = new Forms.RemoteAgentsManager();
            ram.ShowDialog();
        }
        private void aboutToolStripMenuItem_Click(object sender, EventArgs e)
        {
            HideCollectorContextMenu();
            AboutQuickMon aboutQuickMon = new AboutQuickMon();
            aboutQuickMon.ShowDialog();
        }      
        #endregion

        #region Label clicks
        private void llblMonitorPack_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            bool timerEnabled = mainRefreshTimer.Enabled;
            mainRefreshTimer.Enabled = false; //temporary stops it.
            EditMonitorPackConfig emc = new EditMonitorPackConfig();
            emc.SelectedMonitorPack = monitorPack;
            if (emc.ShowDialog() == System.Windows.Forms.DialogResult.OK)
            {
                SetMonitorChanged();
                SetMonitorPackNameDescription();
                DoAutoSave();
                if (emc.RequestCollectorsRefresh)
                {
                    foreach(CollectorEntry entry in monitorPack.Collectors)
                    {
                        entry.RefreshCollectorConfig(monitorPack.ConfigVariables);
                        entry.RefreshDetailsIfOpen();
                    }
                }
            }
            SetPollingFrequency(timerEnabled);
        }
        private void llblNotifierViewToggle_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            masterSplitContainer.Panel2Collapsed = !masterSplitContainer.Panel2Collapsed;
            llblNotifierViewToggle.Text = masterSplitContainer.Panel2Collapsed ? "Show Notifiers" : "Hide Notifiers";
        }
        #endregion

        #region Collector TreeView events
        private void tvwCollectors_TreeNodeMoved(TreeNode dragNode)
        {
            if (dragNode != null)
            {
                //set Collector Parent if needed
                if (dragNode.Tag is CollectorEntry)
                {
                    if (dragNode.Parent.Tag is CollectorEntry)
                    {
                        ((CollectorEntry)dragNode.Tag).ParentCollectorId = ((CollectorEntry)dragNode.Parent.Tag).UniqueId;
                    }
                    else
                        ((CollectorEntry)dragNode.Tag).ParentCollectorId = "";
                }
                SetMonitorChanged();
                DoAutoSave();
            }
        }
        private void tvwCollectors_NodeMouseDoubleClick(object sender, TreeNodeMouseClickEventArgs e)
        {
            if (Control.ModifierKeys == Keys.Control)
                ShowCollectorConfig();
            else
                ShowCollectorDetails();
        }
        private void tvwCollectors_AfterSelect(object sender, TreeViewEventArgs e)
        {
            CheckCollectorContextMenuEnables();
        }
        private void tvwCollectors_ContextMenuShowUp()
        {
            Point topabsolute = this.PointToScreen(tvwCollectors.Location);
            Point topRelative = this.PointToClient(tvwCollectors.Location);
            Point calcPoint;
            if (tvwCollectors.SelectedNode != null)
            {
                calcPoint = new Point(tvwCollectors.Location.X + tvwCollectors.SelectedNode.Bounds.Location.X, GetControlLocationWithinParent(tvwCollectors).Y + tvwCollectors.SelectedNode.Bounds.Location.Y + 20 - this.Top);
            }
            else
            {
                calcPoint = new Point(tvwCollectors.Location.X + (tvwCollectors.Width / 2), tvwCollectors.Location.Y + (tvwCollectors.Height / 2));
            }
            CheckCollectorContextMenuEnables();
            collectorContextMenuLaunchPoint = calcPoint;
            showCollectorContextMenuTimer.Enabled = false;
            showCollectorContextMenuTimer.Enabled = true;
        }
        private void tvwCollectors_MouseUp(object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Right)
            {
                Point topabsolute = this.PointToScreen(panel1.Location);
                Point topRelative = new Point(topabsolute.X - this.Location.X, topabsolute.Y - this.Location.Y);
                Point calcPoint = new Point(Cursor.Position.X - tvwCollectors.Location.X - this.Left, Cursor.Position.Y - topRelative.Y - this.Top + 10);
                collectorContextMenuLaunchPoint = calcPoint;
                CheckCollectorContextMenuEnables();

                showCollectorContextMenuTimer.Enabled = false;
                showCollectorContextMenuTimer.Enabled = true;
            }
        }
        private void tvwCollectors_EnterKeyDown(object sender, KeyEventArgs e)
        {
            if (e.Control)
            {
                collectorTreeEditConfigToolStripMenuItem_Click(null, null);
            }
            else
            {
                collectorTreeViewDetailsToolStripMenuItem_Click(null, null);
            }
        }
        private Point GetControlLocationWithinParent(Control control)
        {
            Point location;
            if (control.Parent != null)
            {
                Point parentLocation = GetControlLocationWithinParent(control.Parent);
                location = new Point(parentLocation.X + control.Location.X, parentLocation.Y + control.Location.Y);
            }
            else
            {
                location = control.Location;
            }
            return location;
        }
        private void tvwCollectors_DeleteKeyPressed()
        {
            DeleteCollector();
        }
        #endregion

        #region Notifier ListView events
        private void lvwNotifiers_SelectedIndexChanged(object sender, EventArgs e)
        {
            CheckNotifierContextMenuEnables();
        }
        private void lvwNotifiers_DoubleClick(object sender, EventArgs e)
        {
            if (Control.ModifierKeys == Keys.Control)
                notifierConfigurationToolStripMenuItem_Click(null, null);
            else
                notifierViewerToolStripMenuItem_Click(null, null);
        }
        private void lvwNotifiers_Resize(object sender, EventArgs e)
        {
            lvwNotifiers.Columns[0].Width = lvwNotifiers.ClientSize.Width;
        }
        private void lvwNotifiers_MouseUp(object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Right)
            {
                showNotifierContextMenuTimer.Enabled = false;
                showNotifierContextMenuTimer.Enabled = true;

                Point topabsolute = this.PointToScreen(panel1.Location);
                Point topRelative = new Point(topabsolute.X - this.Location.X, topabsolute.Y - this.Location.Y);
                Point calcPoint = new Point(Cursor.Position.X - lvwNotifiers.Location.X - this.Left, Cursor.Position.Y - topRelative.Y - this.Top + 10);
                notifierContextMenuLaunchPoint = calcPoint;
                CheckNotifierContextMenuEnables();
            }
        }
        private void lvwNotifiers_DeleteKeyPressed()
        {
            removeNotifierToolStripMenuItem_Click(null, null);
        }
        #endregion

        #region Private methods
        private void SetPollingFrequency(bool enabledAfterWards = true)
        {
            mainRefreshTimer.Enabled = false;
            if (Properties.Settings.Default.OverridesMonitorPackFrequency || monitorPack == null || monitorPack.PollingFrequencyOverrideSec == 0)
                mainRefreshTimer.Interval = Properties.Settings.Default.PollFrequencySec * 1000;
            else
                mainRefreshTimer.Interval = monitorPack.PollingFrequencyOverrideSec * 1000;
            mainRefreshTimer.Enabled = enabledAfterWards;
        }
        private void SetAppIcon(CollectorState state)
        {
            refreshCycleCounter++;
            if (refreshCycleCounter > 1)
                refreshCycleCounter = 0;

            try
            {
                Icon icon;
                if (state == CollectorState.Error)
                {
                    if (refreshCycleCounter == 0)
                        icon = Properties.Resources.QMstateErrorSml;
                    else
                        icon = Properties.Resources.QMstateError;
                }
                else if (state == CollectorState.Warning)
                {
                    if (refreshCycleCounter == 0)
                        icon = Properties.Resources.QMstateWarningSml;
                    else
                        icon = Properties.Resources.QMstateWarning;
                }
                else if (state == CollectorState.Good)
                {
                    if (refreshCycleCounter == 0)
                        icon = Properties.Resources.QMstateGoodSml;
                    else
                        icon = Properties.Resources.QMstateGood;
                }
                else
                {
                    if (refreshCycleCounter == 0)
                        icon = Properties.Resources.QMstateDisabledSml;
                    else
                        icon = Properties.Resources.QMstateDisabled;
                }
                Icon oldIcon = this.Icon;
                if (this.InvokeRequired)
                {
                    this.Invoke((MethodInvoker)delegate()
                    {
                        this.Icon = icon;
                    }
                    );
                }
                else
                {
                    this.Icon = icon;
                }
                oldIcon.Dispose();
            }
            catch (Exception)
            {
                //to be added
                if (refreshCycleCounter == 0)
                    this.Icon = Properties.Resources.QMstateNASml;
                else
                    this.Icon = Properties.Resources.QMstateNA;
            }
        }   
        private void UpdateAppTitle()
        {
            Text = "QuickMon 3";
            if (monitorPackChanged)
                Text += "*";
            if (monitorPack != null)
            {                
                if (!monitorPack.Enabled)
                    Text += " - [Disabled]";
                if (monitorPack.Name != null && monitorPack.Name.Length > 0)
                    Text += string.Format(" - [{0}]", monitorPack.Name);
            }
        }
        private void CloseAllDetailWindows()
        {
            if (monitorPack != null)
            {
                if (monitorPack.Collectors != null)
                {
                    foreach (CollectorEntry entry in monitorPack.Collectors)
                    {
                        entry.CloseDetails();
                    }
                }
                if (monitorPack.Notifiers != null)
                {
                    foreach (NotifierEntry entry in monitorPack.Notifiers)
                    {
                        entry.CloseViewer();
                    }
                }
            }
            foreach (var cs in collectorStatsWindows)
            {
                if (cs.IsStillVisible())
                    cs.Close();
            }
            collectorStatsWindows.Clear();
        }
        private void DoAutoSave()
        {
            if (Properties.Settings.Default.AutosaveChanges)
                SaveMonitorPack();
        }
        private bool SaveMonitorPack()
        {
            bool success = false;
            try
            {
                if (monitorPack != null && monitorPack.MonitorPackPath != null && monitorPack.MonitorPackPath.Length > 0 && System.IO.Directory.Exists(System.IO.Path.GetDirectoryName(monitorPack.MonitorPackPath)))
                {
                    Cursor.Current = Cursors.WaitCursor;
                    SortItemsByTreeView();
                    monitorPack.Save();
                    Properties.Settings.Default.LastMonitorPack = monitorPack.MonitorPackPath;
                    monitorPackChanged = false;
                    success = true;
                    AddMonitorPackFileToRecentList(monitorPack.MonitorPackPath);
                }
                else
                {
                    success = SaveAsMonitorPack();
                }
                UpdateAppTitle();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Save", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            Cursor.Current = Cursors.Default;
            return success;
        }
        private bool SaveAsMonitorPack()
        {
            bool success = false;
            try
            {
                bool canAutoSave = false;
                if (monitorPack == null)
                    monitorPack = new MonitorPack();
                if (monitorPack.MonitorPackPath != null && System.IO.File.Exists(monitorPack.MonitorPackPath))
                {
                    canAutoSave = Properties.Settings.Default.AutosaveChanges;
                    saveFileDialogSave.FileName = monitorPack.MonitorPackPath;
                    if (saveFileDialogSave.FileName.ToLower().EndsWith(".qmconfig"))
                    {
                        saveFileDialogSave.FileName = saveFileDialogSave.FileName.Replace(".qmconfig", ".qmp");
                    }
                    try
                    {
                        saveFileDialogSave.InitialDirectory = System.IO.Path.GetDirectoryName(monitorPack.MonitorPackPath);
                    }
                    catch { }
                }
                else
                {
                    saveFileDialogSave.InitialDirectory = MonitorPack.GetQuickMonUserDataDirectory();
                }
                if (saveFileDialogSave.ShowDialog() == System.Windows.Forms.DialogResult.OK)
                {
                    monitorPack.MonitorPackPath = saveFileDialogSave.FileName;
                    success = SaveMonitorPack();
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Save", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            return success;
        }
        private void SortItemsByTreeView()
        {
            TreeNode collectorRootNode = tvwCollectors.Nodes[0];
            List<CollectorEntry> sortedCollectors = new List<CollectorEntry>();
            AppendSortedCollectors(collectorRootNode, sortedCollectors);
            monitorPack.Collectors.Clear();
            foreach (CollectorEntry c in sortedCollectors)
            {
                monitorPack.Collectors.Add(c);
            }

        }
        private void AppendSortedCollectors(TreeNode treeNode, List<CollectorEntry> sortedCollectors)
        {
            foreach (TreeNode childNode in treeNode.Nodes)
            {
                if (childNode.Tag != null && childNode.Tag is CollectorEntry)
                {
                    sortedCollectors.Add((CollectorEntry)childNode.Tag);
                    AppendSortedCollectors(childNode, sortedCollectors);
                }
            }
        }
        private void NewMonitorPack()
        {
            if (monitorPack != null && System.IO.File.Exists(monitorPack.MonitorPackPath) && monitorPackChanged)
            {
                if (Properties.Settings.Default.AutosaveChanges || MessageBox.Show("Do you want to save changes to the current monitor pack?", "Save", MessageBoxButtons.YesNo, MessageBoxIcon.Exclamation) == System.Windows.Forms.DialogResult.Yes)
                {
                    if (!SaveMonitorPack())
                        return;
                }
            }

            mainRefreshTimer.Enabled = false;
            try
            {
                monitorPack.CollectorCurrentStateReported -= monitorPack_CollectorCurrentStateReported;
                monitorPack.ClosePerformanceCounters();
                monitorPack = null;
            }
            catch { }
            monitorPack = new MonitorPack();
            monitorPack.LoadXml(Properties.Resources.BlankMonitorPack);
            monitorPack.MonitorPackPath = "";
            LoadTreeFromMonitorPack();
            monitorPack.ConcurrencyLevel = Properties.Settings.Default.ConcurrencyLevel;
            monitorPack.CollectorCurrentStateReported += monitorPack_CollectorCurrentStateReported;
            if (monitorPack.Notifiers != null && monitorPack.Notifiers.Count == 0)
                lblNoNotifiersYet.Visible = true;
            mainRefreshTimer.Enabled = true;
            monitorPackChanged = false;
        }
        private void LoadMonitorPack(string monitorPackPath)
        {
            if (System.IO.File.Exists(monitorPackPath))
            {
                if (monitorPackChanged)
                {
                    if (Properties.Settings.Default.AutosaveChanges || MessageBox.Show("Do you want to save changes to the current monitor pack?", "Save", MessageBoxButtons.YesNo, MessageBoxIcon.Exclamation) == System.Windows.Forms.DialogResult.Yes)
                    {
                        if (!SaveMonitorPack())
                        {
                            return;
                        }
                    }
                }
                PausePolling(true);
                try
                {
                    WaitForPollingToFinish(5);
                    monitorPack.CollectorCurrentStateReported -= monitorPack_CollectorCurrentStateReported;
                    monitorPack.CollecterLoading -= monitorPack_CollecterLoading;
                    monitorPack.OnNotifierError -= monitorPack_RaiseNotifierError;
                    monitorPack.RunCollectorCorrectiveWarningScript -= monitorPack_RunCollectorCorrectiveWarningScript;
                    monitorPack.RunCollectorCorrectiveErrorScript -= monitorPack_RunCollectorCorrectiveErrorScript;
                    monitorPack.RunRestorationScript -= monitorPack_RunRestorationScript;
                    monitorPack.CollectorCalled -= monitorPack_CollectorCalled;
                    monitorPack.CollectorExecutionTimeEvent -= monitorPack_CollectorExecutionTimeEvent;
                    
                    //monitorPack.RaiseNotifierError -= new RaiseNotifierErrorDelegare(monitorPack_RaiseNotifierError);
                    //monitorPack.CollectorCalled -= new RaiseCollectorCalledDelegate(monitorPack_CollectorCalled);
                    //monitorPack.CollectorExecutionTimeEvent -= new CollectorExecutionTimeDelegate(monitorPack_CollectorExecutionTimeEvent);
                    monitorPack.ClosePerformanceCounters();
                }
                catch { }
                finally
                {
                    monitorPack = null;
                }

                TreeNode root = tvwCollectors.Nodes[0];
                root.Nodes.Clear();

                root.Text = "COLLECTORS - Loading...";
                Application.DoEvents();
                Cursor.Current = Cursors.WaitCursor;

                monitorPack = new MonitorPack();
                monitorPack.CollecterLoading += monitorPack_CollecterLoading;

                monitorPack.Load(monitorPackPath);
                LoadTreeFromMonitorPack();
                monitorPack.ConcurrencyLevel = Properties.Settings.Default.ConcurrencyLevel;
                monitorPack.CollectorCurrentStateReported += monitorPack_CollectorCurrentStateReported;
                monitorPack.OnNotifierError += monitorPack_RaiseNotifierError;
                monitorPack.RunCollectorCorrectiveWarningScript += monitorPack_RunCollectorCorrectiveWarningScript;
                monitorPack.RunCollectorCorrectiveErrorScript += monitorPack_RunCollectorCorrectiveErrorScript;
                monitorPack.RunRestorationScript += monitorPack_RunRestorationScript;
                monitorPack.CollectorCalled += monitorPack_CollectorCalled;
                monitorPack.CollectorExecutionTimeEvent += monitorPack_CollectorExecutionTimeEvent;
                monitorPack.RunningAttended = AttendedOption.OnlyAttended;

                Cursor.Current = Cursors.Default;
                tvwCollectors.Nodes[0].Text = "COLLECTORS";
                Application.DoEvents();

                AddMonitorPackFileToRecentList(monitorPackPath);

                lblNoNotifiersYet.Visible = monitorPack.Notifiers.Count == 0;
                ResumePolling();
                monitorPackChanged = false;
                UpdateAppTitle();
            }
        }

        private void WaitForPollingToFinish(int secondsToWait)
        {
            if (monitorPack != null && monitorPack.BusyPolling)
            {
                monitorPack.AbortPolling = true;
                DateTime abortStart = DateTime.Now;
                while (monitorPack.BusyPolling && abortStart.AddSeconds(secondsToWait) > DateTime.Now)
                {
                    Application.DoEvents();
                    Cursor.Current = Cursors.WaitCursor;
                }                
            }
        }        

        private void LoadTreeFromMonitorPack()
        {
            firstRefresh = true;
            SetMonitorPackNameDescription();
            TreeNode root = tvwCollectors.Nodes[0];
            root.Nodes.Clear();
            List<CollectorEntry> noDependantCollectors = (from c in monitorPack.Collectors
                                                          where c.ParentCollectorId.Length == 0
                                                          select c).ToList();
            foreach (CollectorEntry collector in noDependantCollectors)
            {
                LoadCollectorNode(root, collector);
            }
            root.Expand();

            UpdateAppTitle();
            LoadNotifiersList();
            try
            {
                showDefaultNotifierToolStripMenuItem.Enabled = false;
                if (monitorPack != null)
                {
                    UpdateStatusbar(string.Format("{0} collector(s), {1} notifier(s)",
                         monitorPack.Collectors.Count,
                         monitorPack.Notifiers.Count));
                    showDefaultNotifierToolStripMenuItem.Enabled = monitorPack.DefaultViewerNotifier != null;
                }
            }
            catch { }
            tvwCollectors.SelectedNode = root;
            root.EnsureVisible();
        }
        private void SetMonitorPackNameDescription()
        {
            toolTip1.SetToolTip(llblMonitorPack, "");
            llblMonitorPack.Text = "";
            if (monitorPack == null || ((monitorPack.Name == null || monitorPack.Name.Trim().Length == 0)))
            {
                llblMonitorPack.Text = "Click here to set the monitor pack name.";
            }
            else
            {                
                llblMonitorPack.Text = monitorPack.Name;
            }
            if (monitorPack != null)
            {
                if (monitorPack.MonitorPackPath != null)
                    toolTip1.SetToolTip(llblMonitorPack, monitorPack.MonitorPackPath);
                if (!monitorPack.Enabled)
                    llblMonitorPack.Text += " (Disabled)";
            }
            //if still after all of this...
            if (llblMonitorPack.Text == "")
                llblMonitorPack.Text = "Click here to set the monitor pack name.";
        }
        private void LoadNotifiersList()
        {
            lvwNotifiers.Items.Clear();
            if (monitorPack.Notifiers != null && monitorPack.Notifiers.Count > 0)
            {
                foreach (NotifierEntry n in monitorPack.Notifiers)
                {
                    ListViewItem lvi = new ListViewItem(n.Name);
                    lvi.ImageIndex = (n.Notifier != null && n.Notifier.HasViewer) ? 1 : 0;
                    lvi.Tag = n;
                    lvi.ForeColor = n.Enabled ? SystemColors.WindowText : Color.Gray;
                    lvwNotifiers.Items.Add(lvi);
                }
            }
            lblNoNotifiersYet.Visible = monitorPack.Notifiers.Count == 0;
        }
        private void llbl_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            if (sender is LinkLabel && ((LinkLabel)sender).Tag is NotifierEntry)
            {
                NotifierEntry n = (NotifierEntry)((LinkLabel)sender).Tag;
                Management.EditNotifierEntry editNotifierEntry = new Management.EditNotifierEntry();
                editNotifierEntry.SelectedEntry = n;
                if (editNotifierEntry.ShowDialog(monitorPack) == System.Windows.Forms.DialogResult.OK)
                {
                    n.CloseViewer();
                    ((LinkLabel)sender).Text = n.Name;                    
                }
            }
        }
        private void LoadCollectorNode(TreeNode root, CollectorEntry collector)
        {
            TreeNode collectorNode;
            if (collector.IsFolder)
            {
                collectorNode = new TreeNode(collector.Name, 1, 1);
                collectorNode.NodeFont = new System.Drawing.Font("Microsoft Sans Serif", 9F, System.Drawing.FontStyle.Bold);
            }
            else
                collectorNode = new TreeNode(collector.Name, 2, 2);
            collectorNode.Tag = collector;
            collector.Tag = collectorNode;
            collectorNode.ForeColor = collector.Enabled ? SystemColors.WindowText : Color.Gray;
            if (collector.EnableRemoteExecute || collector.ForceRemoteExcuteOnChildCollectors)
            {
                collectorNode.Text += string.Format(" [{0}:{1}]", (collector.ForceRemoteExcuteOnChildCollectors ? "!" : "") + collector.RemoteAgentHostAddress, collector.RemoteAgentHostPort);
            }
            foreach (CollectorEntry childCollector in (from c in monitorPack.Collectors
                                                       where c.ParentCollectorId == collector.UniqueId &&
                                                       c.ParentCollectorId != c.UniqueId
                                                       select c))
            {
                LoadCollectorNode(collectorNode, childCollector);
            }
            root.Nodes.Add(collectorNode);
            if (collector.Enabled && collector.ExpandOnStart)
                collectorNode.Expand();
        }
       
        private void AddMonitorPackFileToRecentList(string monitorPackPath)
        {
            if ((from string f in Properties.Settings.Default.RecentQMConfigFiles
                 where f.ToUpper() == monitorPackPath.ToUpper()
                 select f).Count() == 0)
            {
                Properties.Settings.Default.RecentQMConfigFiles.Add(monitorPackPath);
            }
            Properties.Settings.Default.LastMonitorPack = monitorPackPath;
            LoadRecentMonitorPackList();
        }
        private void UpdateStatusbar(string msg)
        {
            try
            {
                if (this != null)
                {
                    if (this.InvokeRequired)
                    {
                        this.Invoke((MethodInvoker)delegate
                        {
                            toolStripStatusLabelStatus.Text = msg;
                            toolTip1.SetToolTip(statusStrip1, msg);                            
                        });
                    }
                    else
                    {
                        toolStripStatusLabelStatus.Text = msg;
                        toolTip1.SetToolTip(statusStrip1, msg);
                    }
                }
            }
            catch { }
        }
        private void RemoveCollector(TreeNode parentNode)
        {
            foreach (TreeNode collectorNode in parentNode.Nodes)
            {
                RemoveCollector(collectorNode);
            }
            CollectorEntry ce = (CollectorEntry)parentNode.Tag;
            monitorPack.Collectors.Remove(ce);
        }
        private void ShowCollectorDetails()
        {
            try
            {
                if (tvwCollectors.SelectedNode != null && tvwCollectors.SelectedNode.Tag is CollectorEntry)
                {
                    CollectorEntry entry = (CollectorEntry)tvwCollectors.SelectedNode.Tag;
                    if (!entry.IsFolder)
                    {
                        if (entry.Collector == null)
                        {
                            //attempt to create it.
                            monitorPack.ApplyCollectorConfig(entry);
                        }
                        entry.ShowDetails();
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }        
        private void ShowCollectorConfig()
        {
            PausePolling();
            try
            {
                if (tvwCollectors.SelectedNode != null && tvwCollectors.SelectedNode.Tag is CollectorEntry)
                {                    
                    TreeNode currentNode = tvwCollectors.SelectedNode;
                    CollectorEntry collectorEntry = (CollectorEntry)currentNode.Tag;
                    AgentHelper.KnownRemoteHosts = (from string krh in Properties.Settings.Default.KnownRemoteHosts
                                                    select krh).ToList();
                    if (AgentHelper.EditCollectorEntry(collectorEntry, monitorPack) == System.Windows.Forms.DialogResult.OK)
                    {
                        SetMonitorChanged();
                        currentNode.Text = collectorEntry.Name;
                        if (collectorEntry.EnableRemoteExecute || collectorEntry.ForceRemoteExcuteOnChildCollectors)
                        {
                            tvwCollectors.SelectedNode.Text += string.Format(" [{0}:{1}]", (collectorEntry.ForceRemoteExcuteOnChildCollectors ? "!" : "") + collectorEntry.RemoteAgentHostAddress, collectorEntry.RemoteAgentHostPort);
                        }

                        //correcting for parent node changes
                        if (collectorEntry.ParentCollectorId == null || collectorEntry.ParentCollectorId == "")
                        {
                            if (currentNode.Parent != tvwCollectors.Nodes[0])
                            {
                                currentNode.Parent.Nodes.Remove(currentNode);
                                tvwCollectors.Nodes[0].Nodes.Add(currentNode);                               
                            }
                        }
                        else
                        {
                            TreeNode parentNode = GetNodeByCollectorId(collectorEntry.ParentCollectorId);
                            if (currentNode.Parent != parentNode)
                            {
                                currentNode.Parent.Nodes.Remove(currentNode);
                                parentNode.Nodes.Add(currentNode);
                            }
                        }
                        //Ensure it is still visible and selected
                        currentNode.EnsureVisible();
                        tvwCollectors.SelectedNode = currentNode;

                        if (!collectorEntry.IsFolder)
                            collectorEntry.RefreshDetailsIfOpen();

                        //if autosaving is enabled
                        DoAutoSave();

                        //add any new remote host entries
                        if (collectorEntry.RemoteAgentHostAddress != null && collectorEntry.RemoteAgentHostAddress.Length > 0 && collectorEntry.EnableRemoteExecute)
                        {
                            if (!Properties.Settings.Default.KnownRemoteHosts.Contains(collectorEntry.ToRemoteHostName()))
                                Properties.Settings.Default.KnownRemoteHosts.Add(collectorEntry.ToRemoteHostName());
                        }                        
                    }                    
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            ResumePolling();
        }

        private bool timerEnabled = false;
        private void PausePolling(bool forcePause = false)
        {
            if (forcePause || Properties.Settings.Default.PausePollingDuringEditConfig)
            {
                timerEnabled = mainRefreshTimer.Enabled;
                mainRefreshTimer.Enabled = false;
            }
        }
        private void ResumePolling()
        {
            SetPollingFrequency(timerEnabled);
        }
        private void CreateNewCollector(bool folder = false)
        {
            try
            {
                HideCollectorContextMenu();
                CollectorEntry newCollectorEntry = new CollectorEntry();
                AgentHelper.KnownRemoteHosts = (from string krh in Properties.Settings.Default.KnownRemoteHosts
                                                select krh).ToList();
                
                //In case there is a parent Collector
                CollectorEntry parentCollectorEntry = null;
                if (tvwCollectors.SelectedNode != null && tvwCollectors.SelectedNode.Tag is CollectorEntry)
                {
                    parentCollectorEntry = (CollectorEntry)tvwCollectors.SelectedNode.Tag;
                    newCollectorEntry.ParentCollectorId = parentCollectorEntry.UniqueId;
                }
                AgentHelper.LastCreateAgentOption = Properties.Settings.Default.LastCreateAgentOption;

                if (folder)
                {
                    newCollectorEntry.IsFolder = true;
                    newCollectorEntry.Collector = null;
                    newCollectorEntry.Name = "New Folder";
                    newCollectorEntry.CollectorRegistrationDisplayName = "Folder";
                    newCollectorEntry.CollectorRegistrationName = "Folder";
                    newCollectorEntry.InitialConfiguration = "";
                    
                    if (AgentHelper.EditCollectorEntry(newCollectorEntry, monitorPack) != System.Windows.Forms.DialogResult.OK)
                    {
                        newCollectorEntry = null;
                    }
                }
                else
                {                    
                    newCollectorEntry = AgentHelper.CreateAndEditNewCollector(monitorPack, parentCollectorEntry);
                }
                Properties.Settings.Default.LastCreateAgentOption = AgentHelper.LastCreateAgentOption;
                //if new collectorEntry is valid add it to monitorPack
                if (newCollectorEntry != null)
                {
                    SetMonitorChanged();
                    monitorPack.Collectors.Add(newCollectorEntry);
                    TreeNode parentNode = tvwCollectors.Nodes[0];
                    if (newCollectorEntry.ParentCollectorId != null && newCollectorEntry.ParentCollectorId.Length > 0)
                    {
                        parentNode = GetNodeByCollectorId(newCollectorEntry.ParentCollectorId);
                        if (parentNode == null)
                            parentNode = tvwCollectors.Nodes[0];
                    }
                    LoadCollectorNode(parentNode, newCollectorEntry);
                    parentNode.Expand();
                    DoAutoSave();
                } 
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "New Collector", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
        private void ShowAddCollector(bool folder = false)
        {
            try
            {
                QuickMon.Forms.AgentTypeSelect agentTypeSelect = new QuickMon.Forms.AgentTypeSelect();
                if (folder || agentTypeSelect.ShowCollectorSelection("") == System.Windows.Forms.DialogResult.OK)
                {
                    CollectorEntry newCollectorEntry = new CollectorEntry();
                   
                    if (folder)
                    {
                        newCollectorEntry.IsFolder = true;
                        newCollectorEntry.Collector = null;
                        newCollectorEntry.CollectorRegistrationDisplayName = "Folder";
                        newCollectorEntry.CollectorRegistrationName = "Folder";
                        newCollectorEntry.InitialConfiguration = "";
                    }
                    else
                    {
                        RegisteredAgent ar = agentTypeSelect.SelectedAgent;
                        if (ar.ClassName != "QuickMon.Collectors.Folder")
                        {
                            ICollector c = CollectorEntry.CreateCollectorEntry(ar);
                            newCollectorEntry.Collector = c;
                            newCollectorEntry.InitialConfiguration = c.GetDefaultOrEmptyConfigString();
                        }
                        else
                        {
                            newCollectorEntry.IsFolder = true;
                        }
                        newCollectorEntry.CollectorRegistrationDisplayName = ar.DisplayName;
                        newCollectorEntry.CollectorRegistrationName = ar.Name;
                    }

                    CollectorEntry parentCollectorEntry = null;
                    if (tvwCollectors.SelectedNode != null && tvwCollectors.SelectedNode.Tag is CollectorEntry)
                    {
                        parentCollectorEntry = (CollectorEntry)tvwCollectors.SelectedNode.Tag;
                        newCollectorEntry.ParentCollectorId = parentCollectorEntry.UniqueId;
                    }


                    QuickMon.Forms.EditCollectorConfig editCollectorEntry = new Forms.EditCollectorConfig();
                    editCollectorEntry.SelectedEntry = newCollectorEntry;
                    editCollectorEntry.KnownRemoteHosts = (from string krh in Properties.Settings.Default.KnownRemoteHosts
                                                          select krh).ToList();
                    editCollectorEntry.LaunchAddEntry = !agentTypeSelect.ImportConfigAfterSelect && !agentTypeSelect.UsePresetAfterSelect;
                    editCollectorEntry.ShowRawEditOnStart = agentTypeSelect.ImportConfigAfterSelect;
                    editCollectorEntry.ShowSelectPresetOnStart = agentTypeSelect.UsePresetAfterSelect;

                    if (editCollectorEntry.ShowDialog(monitorPack) == System.Windows.Forms.DialogResult.OK)
                    {
                        SetMonitorChanged();
                        monitorPack.Collectors.Add(editCollectorEntry.SelectedEntry);
                        TreeNode root = tvwCollectors.Nodes[0];
                        if (parentCollectorEntry != null)
                        {
                            root = tvwCollectors.SelectedNode;
                        }
                        LoadCollectorNode(root, editCollectorEntry.SelectedEntry);
                        root.Expand();
                        DoAutoSave();
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
        private void DeleteCollector()
        {
            TreeNode currentNode = tvwCollectors.SelectedNode;
            if (currentNode.Tag is CollectorEntry)
            {
                if (MessageBox.Show("Are you sure you want to remove this collector agent(and all possible dependants)?", "Remove", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == System.Windows.Forms.DialogResult.Yes)
                {
                    SetMonitorChanged();
                    RemoveCollector(currentNode);
                    RefreshMonitorPack(true, true);
                    if (currentNode.Parent != null)
                    {
                        currentNode.Parent.Nodes.Remove(currentNode);
                    }
                    DoAutoSave();
                }
            }
        }
        private void CheckCollectorContextMenuEnables()
        {
            if (tvwCollectors.SelectedNode != null && tvwCollectors.SelectedNode.Tag != null && tvwCollectors.SelectedNode.Tag is CollectorEntry)
            {
                CollectorEntry entry = (CollectorEntry)tvwCollectors.SelectedNode.Tag;

                popedContainerForTreeView.cmdViewDetails.Enabled = !entry.IsFolder;
                popedContainerForTreeView.cmdEditCollector.Enabled = true;
                popedContainerForTreeView.cmdDeleteCollector.Enabled = true;
                popedContainerForTreeView.cmdDisableCollector.Enabled = true;
                popedContainerForTreeView.cmdDisableCollector.BackColor = entry.Enabled ? SystemColors.Control : Color.WhiteSmoke;
                //popedContainerForTreeView.cmdDisableCollector.Text = entry.Enabled ? "Disable" : "Enable";
                popedContainerForTreeView.cmdDisableCollector.BackgroundImage = entry.Enabled ? global::QuickMon.Properties.Resources.Forbidden16x16 : global::QuickMon.Properties.Resources.ForbiddenNot16x16;

                collectorTreeViewDetailsToolStripMenuItem.Enabled = !entry.IsFolder;
                viewCollectorDetailsToolStripMenuItem.Enabled = !entry.IsFolder;
                collectorTreeEditConfigToolStripMenuItem.Enabled = true;
                editCollectorToolStripMenuItem.Enabled = true;
                removeCollectorToolStripMenuItem1.Enabled = true;
                disableCollectorTreeToolStripMenuItem.Enabled = true;
                removeCollectorToolStripMenuItem.Enabled = true;
                disableCollectorTreeToolStripMenuItem.Text = entry.Enabled ? "Disable" : "Enable";

                popedContainerForTreeView.cmdCopy.Enabled = true;
                popedContainerForTreeView.cmdStats.Enabled = !entry.IsFolder;
                collectorStatisticsToolStripMenuItem.Enabled = !entry.IsFolder;
            }
            else
            {
                popedContainerForTreeView.cmdViewDetails.Enabled = false;
                popedContainerForTreeView.cmdEditCollector.Enabled = false;
                popedContainerForTreeView.cmdDeleteCollector.Enabled = false;
                popedContainerForTreeView.cmdDisableCollector.Enabled = false;

                collectorTreeViewDetailsToolStripMenuItem.Enabled = false;
                viewCollectorDetailsToolStripMenuItem.Enabled = false;
                collectorTreeEditConfigToolStripMenuItem.Enabled = false;
                editCollectorToolStripMenuItem.Enabled = false;
                disableCollectorTreeToolStripMenuItem.Enabled = false;
                popedContainerForTreeView.cmdDisableCollector.BackgroundImage = global::QuickMon.Properties.Resources.ForbiddenNot16x16;
                removeCollectorToolStripMenuItem.Enabled = false;
                removeCollectorToolStripMenuItem1.Enabled = false;

                popedContainerForTreeView.cmdCopy.Enabled = false;
                popedContainerForTreeView.cmdStats.Enabled = false;
                collectorStatisticsToolStripMenuItem.Enabled = false;
            }
            popedContainerForTreeView.cmdPaste.Enabled = false;
            popedContainerForTreeView.cmdPasteWithEdit.Enabled = false;
            if (Clipboard.ContainsText())
            {
                string clipboardTest = Clipboard.GetText().Trim(' ', '\r', '\n');
                if (clipboardTest.StartsWith("<collectorEntries>") && clipboardTest.EndsWith("</collectorEntries>"))
                {
                    try
                    {
                        List<CollectorEntry> pastedCollectorEntries = CollectorEntry.GetCollectorEntriesFromString(clipboardTest);
                        popedContainerForTreeView.cmdPaste.Enabled = true;
                        popedContainerForTreeView.cmdPasteWithEdit.Enabled = true;
                        copiedCollectorList = pastedCollectorEntries;
                    }
                    catch (Exception ex)
                    {
                        System.Diagnostics.Trace.WriteLine(ex.ToString()); 
                    }
                }
            }
        }
        private void CheckNotifierContextMenuEnables()
        {
            NotifierEntry entry = null;
            if (lvwNotifiers.SelectedItems.Count > 0)
            {
                entry = (NotifierEntry)lvwNotifiers.SelectedItems[0].Tag;
                disableNotifierToolStripMenuItem.Text = entry.Enabled ? "Disable notifier" : "Enable notifier";
                popedContainerForListView.cmdDisableNotifier.BackgroundImage = entry.Enabled ? global::QuickMon.Properties.Resources.Forbidden32x32 : global::QuickMon.Properties.Resources.ForbiddenNot32x32;
            }
            else
            {
                popedContainerForListView.cmdDisableNotifier.BackgroundImage = global::QuickMon.Properties.Resources.ForbiddenNot32x32;
            }
            notifierViewerToolStripMenuItem.Enabled = lvwNotifiers.SelectedItems.Count > 0 && lvwNotifiers.SelectedItems[0].ImageIndex > 0;
            notifierConfigurationToolStripMenuItem.Enabled = lvwNotifiers.SelectedItems.Count == 1;
            editNotifierToolStripMenuItem.Enabled = lvwNotifiers.SelectedItems.Count == 1;
            removeNotifierToolStripMenuItem.Enabled = lvwNotifiers.SelectedItems.Count > 0;
            removeNotifierToolStripMenuItem1.Enabled = lvwNotifiers.SelectedItems.Count > 0;
            disableNotifierToolStripMenuItem.Enabled = lvwNotifiers.SelectedItems.Count == 1;

            popedContainerForListView.cmdDisableNotifier.Enabled = lvwNotifiers.SelectedItems.Count == 1;
            popedContainerForListView.cmdViewDetails.Enabled = lvwNotifiers.SelectedItems.Count > 0 && lvwNotifiers.SelectedItems[0].ImageIndex > 0;
            popedContainerForListView.cmdEditNotifier.Enabled = lvwNotifiers.SelectedItems.Count == 1;
            popedContainerForListView.cmdDisableNotifier.Enabled = lvwNotifiers.SelectedItems.Count == 1;
            popedContainerForListView.cmdDeleteNotifier.Enabled = lvwNotifiers.SelectedItems.Count > 0;
        }
        private void CopySelectedCollectorAndDependants()
        {
            if (tvwCollectors.SelectedNode != null && tvwCollectors.SelectedNode.Tag != null && tvwCollectors.SelectedNode.Tag is CollectorEntry)
            {
                CollectorEntry entry = (CollectorEntry)tvwCollectors.SelectedNode.Tag;
                List<CollectorEntry> sourceList = monitorPack.GetAllChildCollectors(entry);
                copiedCollectorList = new List<CollectorEntry>();
                copiedCollectorList.Add(entry.Clone());
                foreach (CollectorEntry en in sourceList)
                {
                    //Copy as is with same IDs
                    copiedCollectorList.Add(en.Clone());
                }
                Clipboard.SetText(CollectorEntry.EntriesConfigToString(copiedCollectorList));

                popedContainerForTreeView.cmdPaste.Enabled = true;
                popedContainerForTreeView.cmdPasteWithEdit.Enabled = true;
            }
        }
        private void PasteSelectedCollectorAndDependant(bool showEditList)
        {
            try
            {
                if (copiedCollectorList != null && copiedCollectorList.Count > 0)
                {
                    if (showEditList)
                    {
                        PasteCollectors pasteCollectors = new PasteCollectors();
                        pasteCollectors.SelectedCollectors = copiedCollectorList;
                        if (pasteCollectors.ShowDialog() == System.Windows.Forms.DialogResult.OK)
                        {
                            copiedCollectorList = pasteCollectors.SelectedCollectors;
                        }
                        else
                            return;
                    }

                    if (copiedCollectorList != null && copiedCollectorList.Count > 0)
                    {
                        SetMonitorChanged();
                        for (int i = 0; i < copiedCollectorList.Count; i++)
                        {
                            string newId = Guid.NewGuid().ToString();
                            string oldId = copiedCollectorList[i].UniqueId;
                            for (int j = 0; j < copiedCollectorList.Count; j++)
                            {
                                if (i != j && copiedCollectorList[j].ParentCollectorId == oldId)
                                {
                                    copiedCollectorList[j].ParentCollectorId = newId;
                                }
                            }
                            copiedCollectorList[i].UniqueId = newId;
                        }

                        if (tvwCollectors.SelectedNode != null && tvwCollectors.SelectedNode.Tag != null && tvwCollectors.SelectedNode.Tag is CollectorEntry)
                        {
                            copiedCollectorList[0].ParentCollectorId = ((CollectorEntry)tvwCollectors.SelectedNode.Tag).UniqueId;
                        }
                        else
                            copiedCollectorList[0].ParentCollectorId = "";
                        CollectorEntry rootChild = null;
                        for (int i = 0; i < copiedCollectorList.Count; i++)
                        {
                            CollectorEntry newChild = copiedCollectorList[i].Clone();
                            if (rootChild == null)
                                rootChild = newChild;
                            monitorPack.Collectors.Add(newChild);
                        }
                        TreeNode root = tvwCollectors.Nodes[0];
                        if (tvwCollectors.SelectedNode != null && tvwCollectors.SelectedNode.Tag != null && tvwCollectors.SelectedNode.Tag is CollectorEntry)
                        {
                            root = tvwCollectors.SelectedNode;
                        }

                        LoadCollectorNode(root, rootChild);
                        root.Expand();
                        DoAutoSave();
                    }
                }
            }
            catch(Exception ex)
            {
                MessageBox.Show(ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
        private void SetNodesToBeingRefreshed(TreeNode root = null)
        {
            if (root == null)
                root = tvwCollectors.Nodes[0];

            if (root.Tag != null && root.Tag is CollectorEntry)
            {
                CollectorEntry collector = (CollectorEntry)root.Tag;
                if (!collector.IsFolder && collector.Enabled)
                {
                    //root.ForeColor = Color.DarkRed; //13,14,15
                    if (root.ImageIndex == collectorGoodStateImage1)
                    {
                        root.ImageIndex = collectorGoodStateImage2;
                        root.SelectedImageIndex = collectorGoodStateImage2;
                    }
                    else if (root.ImageIndex == collectorWarningStateImage1)
                    {
                        root.ImageIndex = collectorWarningStateImage2;
                        root.SelectedImageIndex = collectorWarningStateImage2;
                    }
                    else if (root.ImageIndex == collectorErrorStateImage1)
                    {
                        root.ImageIndex = collectorErrorStateImage2;
                        root.SelectedImageIndex = collectorErrorStateImage2;
                    }
                }                
            }
            foreach (TreeNode childNode in root.Nodes)
                SetNodesToBeingRefreshed(childNode);
        }
        private void SetMonitorChanged()
        {
            monitorPackChanged = true;
            UpdateAppTitle();
        }
        private bool PerformCleanShutdown(bool abortAllowed = false)
        {
            bool notAborted = true;
            try
            {
                if (monitorPackChanged)
                {
                    if (Properties.Settings.Default.AutosaveChanges || MessageBox.Show("Do you want to save changes to the current monitor pack?", "Save", MessageBoxButtons.YesNo, MessageBoxIcon.Exclamation) == System.Windows.Forms.DialogResult.Yes)
                    {
                        if (!SaveMonitorPack() && abortAllowed)
                            return false;
                    }
                }

                UpdateStatusbar( "Shutting down...");
                Application.DoEvents();
                mainRefreshTimer.Enabled = false;
                CloseAllDetailWindows();
                if (monitorPack.BusyPolling)
                {
                    monitorPack.AbortPolling = true;
                    DateTime abortStart = DateTime.Now;
                    while (monitorPack.BusyPolling && abortStart.AddSeconds(5) > DateTime.Now)
                    {
                        Application.DoEvents();
                    }
                    Cursor.Current = Cursors.WaitCursor;
                    ClosePerformanceCounters();
                }

                if (WindowState == FormWindowState.Normal)
                {
                    Properties.Settings.Default.MainWindowLocation = this.Location;
                    Properties.Settings.Default.MainWindowSize = this.Size;
                }
                Properties.Settings.Default.Save();
            }
            catch { }
            return notAborted;
        }
        private void LoadRecentMonitorPackList()
        {
            cboRecentMonitorPacks.Items.Clear();
            cboRecentMonitorPacks.Items.Add(new QuickMon.Controls.ComboItem("",""));

            try
            {
                List<string> allowFilters = new List<string>();
                List<string> disallowFilters = new List<string>();
                string typeFilters = Properties.Settings.Default.RecentQMConfigFileFilters;
                if (typeFilters.Trim().Length == 0)
                    typeFilters = "*";
                foreach (string typeFilter in typeFilters.Split(','))
                {
                    if (typeFilter.Trim().StartsWith("!"))
                        disallowFilters.Add(typeFilter.Trim(' ', '!'));
                    else
                        allowFilters.Add(typeFilter.Trim());
                }

                foreach (string filePath in (from string s in Properties.Settings.Default.RecentQMConfigFiles
                                             orderby s
                                             select s))
                {
                    bool mpVisible = false;
                    if (System.IO.File.Exists(filePath))
                    {
                        //MonitorPack recentPack = new MonitorPack();
                        //recentPack.Load(filePath);
                        string typeName = MonitorPack.GetMonitorPackTypeName(filePath);
                        if ((from string s in allowFilters
                             where s == "*" || s.ToLower() == typeName.ToLower()
                             select s).Count() > 0)
                            mpVisible = true;
                        if ((from string s in disallowFilters
                             where s.ToLower() == typeName.ToLower()
                             select s).Count() > 0)
                            mpVisible = false;
                    }
                    else
                        mpVisible = false;

                    if (mpVisible)
                    {
                        string entryDisplayName = filePath;
                        if (!Properties.Settings.Default.ShowFullPathForQuickRecentist)
                            entryDisplayName = System.IO.Path.GetFileNameWithoutExtension(filePath);

                        if (cboRecentMonitorPacks.DropDownWidth < TextRenderer.MeasureText(entryDisplayName + "........", cboRecentMonitorPacks.Font).Width)
                        {
                            string ellipseText = entryDisplayName.Substring(0, 20) + "....";
                            string tmpStr = entryDisplayName.Substring(4);
                            while (TextRenderer.MeasureText(ellipseText + tmpStr, cboRecentMonitorPacks.Font).Width > cboRecentMonitorPacks.DropDownWidth)
                            {
                                tmpStr = tmpStr.Substring(1);
                            }
                            cboRecentMonitorPacks.Items.Add(new QuickMon.Controls.ComboItem(ellipseText + tmpStr, filePath));
                        }
                        else
                        {
                            cboRecentMonitorPacks.Items.Add(new QuickMon.Controls.ComboItem(entryDisplayName, filePath));
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
        private TreeNode GetNodeByCollectorId(string uniqueId, TreeNode root = null)
        {
            if (root == null)
            {
                root = tvwCollectors.Nodes[0];
            }
            foreach (TreeNode childNode in root.Nodes)
            {
                if (childNode.Tag != null && childNode.Tag is CollectorEntry)
                {
                    CollectorEntry theEntry = (CollectorEntry)childNode.Tag;
                    if (theEntry.UniqueId == uniqueId)
                        return childNode;
                }
                TreeNode testGrandChild = GetNodeByCollectorId(uniqueId, childNode);
                if (testGrandChild != null)
                    return testGrandChild;
            }
            return null;
        }
        #endregion

        #region Monitor pack events
        public void monitorPack_CollectorCurrentStateReported(CollectorEntry collectorEntry)
        {
            this.Invoke((MethodInvoker)delegate
                {
                    try
                    {
                        if (collectorEntry != null && collectorEntry.Tag is TreeNode)
                        {
                            System.Diagnostics.Trace.WriteLine("Updating " + collectorEntry.Name);
                            TreeNode currentTreeNode = (TreeNode)collectorEntry.Tag;                            

                            bool nodeChanged = false;
                            Color foreColor = currentTreeNode.ForeColor;
                            int imageIndex = currentTreeNode.ImageIndex;

                            if (collectorEntry.Enabled && currentTreeNode.ForeColor != SystemColors.WindowText)
                            {
                                nodeChanged = true;
                                foreColor = SystemColors.WindowText;                                
                            }
                            else if (!collectorEntry.Enabled && currentTreeNode.ForeColor != Color.Gray)
                            {
                                nodeChanged = true;
                                foreColor = Color.Gray;
                            }

                            if (collectorEntry.IsFolder || collectorEntry.CurrentState.State == CollectorState.Folder)
                            {
                                if (currentTreeNode.ImageIndex != collectorFolderImage)
                                {
                                    nodeChanged = true;
                                    imageIndex = collectorFolderImage;
                                }
                            }
                            else if (!collectorEntry.Enabled || collectorEntry.CurrentState.State == CollectorState.Disabled)
                            {
                                if (currentTreeNode.ImageIndex != collectorDisabled)
                                {
                                    nodeChanged = true;
                                    imageIndex = collectorDisabled;
                                }
                            }
                            else if (collectorEntry.CurrentState.State == CollectorState.Error || collectorEntry.CurrentState.State == CollectorState.ConfigurationError)
                            {
                                if (currentTreeNode.ImageIndex != collectorErrorStateImage1)
                                {
                                    nodeChanged = true;
                                    imageIndex = collectorErrorStateImage1;
                                    PCRaiseCollectorErrorState();
                                }
                            }
                            else if (collectorEntry.CurrentState.State == CollectorState.Warning)
                            {
                                if (currentTreeNode.ImageIndex != collectorWarningStateImage1)
                                {
                                    nodeChanged = true;
                                    imageIndex = collectorWarningStateImage1;
                                    PCRaiseCollectorWarningState();
                                }
                            }
                            else if (collectorEntry.CurrentState.State == CollectorState.Good)
                            {
                                if (currentTreeNode.ImageIndex != collectorGoodStateImage1)
                                {
                                    nodeChanged = true;
                                    imageIndex = collectorGoodStateImage1;
                                    PCRaiseCollectorSuccessState();
                                }
                            }
                            else if (currentTreeNode.ImageIndex != collectorNAstateImage)
                            {
                                nodeChanged = true;
                                imageIndex = collectorNAstateImage;
                            }
                            if (nodeChanged)
                            {
                                if (currentTreeNode.ForeColor != foreColor)
                                    currentTreeNode.ForeColor = foreColor;
                                if (currentTreeNode.ImageIndex != imageIndex)
                                {
                                    currentTreeNode.ImageIndex = imageIndex;
                                    currentTreeNode.SelectedImageIndex = imageIndex;
                                    if (firstRefresh && (imageIndex == collectorWarningStateImage1 || imageIndex == collectorErrorStateImage1))
                                    {
                                        TreeNode currentFocusNode = tvwCollectors.SelectedNode;
                                        try
                                        {   
                                            tvwCollectors.BeginUpdate();
                                            currentTreeNode.ExpandAllParents();
                                        }
                                        catch { }
                                        finally
                                        {
                                            tvwCollectors.EndUpdate();
                                        }
                                        currentFocusNode.EnsureVisible();
                                    }
                                }
                            }
                        }
                    }
                    catch (Exception ex)
                    {
                        System.Diagnostics.Trace.WriteLine("Error " + collectorEntry.Name + "->" + ex.Message);
                    }
                });
        }
        private void monitorPack_CollecterLoading(string collectorName)
        {
            UpdateStatusbar(collectorName);
            Application.DoEvents();
        }
        private void monitorPack_RaiseNotifierError(NotifierEntry notifier, string errorMessage)
        {
            UpdateStatusbar( string.Format("Notifier error: {0} - {1}", notifier.Name, errorMessage));
        }
        private void monitorPack_RunCollectorCorrectiveErrorScript(CollectorEntry collectorEntry)
        {
            if (collectorEntry != null && !collectorEntry.CorrectiveScriptDisabled)
                RunCorrectiveScript(collectorEntry.CorrectiveScriptOnErrorPath);
        }
        private void monitorPack_RunCollectorCorrectiveWarningScript(CollectorEntry collectorEntry)
        {
            if (collectorEntry != null && !collectorEntry.CorrectiveScriptDisabled)
                RunCorrectiveScript(collectorEntry.CorrectiveScriptOnWarningPath);
        }
        private void monitorPack_RunRestorationScript(CollectorEntry collectorEntry)
        {
            if (collectorEntry != null && !collectorEntry.CorrectiveScriptDisabled)
                RunCorrectiveScript(collectorEntry.RestorationScriptPath);
        }
        private void RunCorrectiveScript(string scriptPath)
        {
            try
            {
                if (System.IO.File.Exists(scriptPath))
                {
                    if (scriptPath.ToLower().EndsWith(".ps1"))
                    {
                        RunPSScript(scriptPath);
                    }
                    else
                    {
                        Process p = new Process();
                        p.StartInfo = new ProcessStartInfo(scriptPath);
                        p.StartInfo.WindowStyle = ProcessWindowStyle.Hidden;
                        p.Start();
                    }
                }
            }
            catch (Exception ex)
            {
                UpdateStatusbar("Corrective script error:" + ex.Message);
            }
        }
        private void monitorPack_CollectorCalled(CollectorEntry collectorEntry)
        {
            PCRaiseCollectorsQueried();
        }
        private void monitorPack_CollectorExecutionTimeEvent(CollectorEntry collector, long msTime)
        {
            try
            {
                if (this.InvokeRequired)
                {
                    this.Invoke((MethodInvoker)delegate()
                    {
                        if (tvwCollectors.SelectedNode != null && tvwCollectors.SelectedNode.Tag != null && tvwCollectors.SelectedNode.Tag is CollectorEntry)
                        {
                            CollectorEntry tvcollector = (CollectorEntry)tvwCollectors.SelectedNode.Tag;
                            if (tvcollector.UniqueId == collector.UniqueId)
                            {
                                PCSetSelectedCollectorsQueryTime(msTime);
                                return;
                            }
                        }
                        else
                            PCSetSelectedCollectorsQueryTime(0);
                    }
                    );
                }
                else
                {
                    if (tvwCollectors.SelectedNode != null && tvwCollectors.SelectedNode.Tag != null && tvwCollectors.SelectedNode.Tag is CollectorEntry)
                    {
                        CollectorEntry tvcollector = (CollectorEntry)tvwCollectors.SelectedNode.Tag;
                        if (tvcollector.UniqueId == collector.UniqueId)
                        {
                            PCSetSelectedCollectorsQueryTime(msTime);
                            return;
                        }
                    }
                    else
                        PCSetSelectedCollectorsQueryTime(0);
                }
            }
            catch { }
        }
        #endregion

        #region Collector context menus
        private void showCollectorContextMenuTimer_Tick(object sender, EventArgs e)
        {
            showCollectorContextMenuTimer.Enabled = false;
            poperContainerForTreeView.Show(this, collectorContextMenuLaunchPoint);
        }
        private void HideCollectorContextMenu()
        {
            try
            {
                if (poperContainerForTreeView != null)
                    poperContainerForTreeView.Close();
            }
            catch { }
        }

        #region Copy and paste
        private void collectorContextMenuCmdCopy_Click(object sender, EventArgs e)
        {
            HideCollectorContextMenu();
            CopySelectedCollectorAndDependants();
        }
        private void collectorContextMenuCmdPaste_Click(object sender, EventArgs e)
        {
            HideCollectorContextMenu();
            PasteSelectedCollectorAndDependant(false);
        }
        private void collectorContextMenuCmdPasteWithEdit_Click(object sender, EventArgs e)
        {
            HideCollectorContextMenu();
            PasteSelectedCollectorAndDependant(true);
        }   
        #endregion

        private void collectorTreeViewDetailsToolStripMenuItem_Click(object sender, EventArgs e)
        {
            HideCollectorContextMenu();
            ShowCollectorDetails();
        }
        private void collectorTreeEditConfigToolStripMenuItem_Click(object sender, EventArgs e)
        {
            HideCollectorContextMenu();
            ShowCollectorConfig();
        }
        private void disableCollectorTreeToolStripMenuItem_Click(object sender, EventArgs e)
        {
            HideCollectorContextMenu();
            try
            {
                if (tvwCollectors.SelectedNode != null && tvwCollectors.SelectedNode.Tag is CollectorEntry)
                {
                    SetMonitorChanged();
                    CollectorEntry entry = (CollectorEntry)tvwCollectors.SelectedNode.Tag;
                    entry.Enabled = !entry.Enabled;
                    disableCollectorTreeToolStripMenuItem.Text = entry.Enabled ? "Disable collector" : "Enable collector";
                    tvwCollectors.SelectedNode.ForeColor = entry.Enabled ? SystemColors.WindowText : Color.Gray;                    
                    if (!entry.Enabled)
                    {
                        tvwCollectors.SelectedNode.ImageIndex = collectorDisabled;
                        tvwCollectors.SelectedNode.SelectedImageIndex = collectorDisabled;
                    }
                    DoAutoSave();
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
        private void addCollectorToolStripMenuItem_Click(object sender, EventArgs e)
        {
            HideCollectorContextMenu();
            CreateNewCollector();
        }
        private void addCollectorFolderToolStripMenuItem_Click(object sender, EventArgs e)
        {
            HideCollectorContextMenu();
            CreateNewCollector(true);
        }
        private void removeCollectorToolStripMenuItem_Click(object sender, EventArgs e)
        {
            HideCollectorContextMenu();
            DeleteCollector();
        }
        private void cmdStats_Click(object sender, EventArgs e)
        {
            try
            {
                HideCollectorContextMenu();
                if (tvwCollectors.SelectedNode != null && tvwCollectors.SelectedNode.Tag is CollectorEntry)
                {
                    CollectorEntry ce = (CollectorEntry)tvwCollectors.SelectedNode.Tag;
                    CollectorStats collectorStats = (from cs in collectorStatsWindows
                                                     where cs.SelectedEntry.UniqueId == ce.UniqueId
                                                     select cs).FirstOrDefault();
                    if (collectorStats != null && !collectorStats.IsStillVisible())
                    {
                        collectorStatsWindows.Remove(collectorStats);
                        collectorStats = null;
                    }
                    if (collectorStats == null)
                    {
                        collectorStats = new CollectorStats();
                        collectorStats.SelectedEntry = ce;
                        collectorStatsWindows.Add(collectorStats);
                        collectorStats.Show();
                    }
                    else
                    {
                        if (collectorStats.WindowState == FormWindowState.Minimized)
                            collectorStats.WindowState = FormWindowState.Normal;
                        collectorStats.Show();
                        collectorStats.TopMost = true;
                        collectorStats.TopMost = false;
                    }
                    
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }            
        }
        #endregion

        #region Notifier context menu
        private void showNotifierContextMenuTimer_Tick(object sender, EventArgs e)
        {
            showNotifierContextMenuTimer.Enabled = false;
            poperContainerForListView.Show(this, notifierContextMenuLaunchPoint);
        }
        private void HideNotifierContextMenu()
        {
            try
            {
                if (poperContainerForListView != null)
                    poperContainerForListView.Close();
            }
            catch { }
        }
        private void notifierViewerToolStripMenuItem_Click(object sender, EventArgs e)
        {
            HideNotifierContextMenu();
            try
            {
                if (lvwNotifiers.SelectedItems.Count > 0 && lvwNotifiers.SelectedItems[0].Tag is NotifierEntry)
                {
                    NotifierEntry n = (NotifierEntry)lvwNotifiers.SelectedItems[0].Tag;
                    n.ShowViewer();
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        } 
        private void notifierConfigurationToolStripMenuItem_Click(object sender, EventArgs e)
        {
            HideNotifierContextMenu();
            PausePolling();
            if (lvwNotifiers.SelectedItems.Count > 0 && lvwNotifiers.SelectedItems[0].Tag is NotifierEntry)
            {
                NotifierEntry n = (NotifierEntry)lvwNotifiers.SelectedItems[0].Tag;
                if (AgentHelper.EditNotifierEntry(n, monitorPack) == System.Windows.Forms.DialogResult.OK)
                { 
                    SetMonitorChanged();
                    lvwNotifiers.SelectedItems[0].Text = n.Name;
                    lvwNotifiers.SelectedItems[0].ForeColor = n.Enabled ? SystemColors.WindowText : Color.Gray;
                    lvwNotifiers.SelectedItems[0].ImageIndex = (n.Notifier != null && n.Notifier.HasViewer) ? 1 : 0;
                    n.CloseViewer();
                    DoAutoSave();
                }
            }
            ResumePolling();
        }
        private void addNotifierToolStripMenuItem_Click(object sender, EventArgs e)
        {
            HideNotifierContextMenu();
            try
            {
                NotifierEntry newNotifierEntry = AgentHelper.CreateAndEditNewNotifier(monitorPack);
                if (newNotifierEntry != null)
                {
                    SetMonitorChanged();
                    monitorPack.Notifiers.Add(newNotifierEntry);
                    if (monitorPack.Notifiers.Count == 1)
                        monitorPack.DefaultViewerNotifier = newNotifierEntry;
                    LoadNotifiersList();
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
        private void removeNotifierToolStripMenuItem_Click(object sender, EventArgs e)
        {
            HideNotifierContextMenu();
            if (lvwNotifiers.SelectedItems.Count > 0 && MessageBox.Show("Are you sure you want to remove the selected notifiers?", "Remove", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == System.Windows.Forms.DialogResult.Yes)
            {
                SetMonitorChanged();
                ListView.SelectedIndexCollection idxes = lvwNotifiers.SelectedIndices;
                foreach (int i in (from int id in idxes
                                       orderby id descending
                                       select id))
                {
                    ListViewItem lvi = lvwNotifiers.Items[i];
                    if (lvi.Tag is NotifierEntry)
                    {
                        NotifierEntry n = (NotifierEntry)lvi.Tag;
                        n.CloseViewer();
                        monitorPack.Notifiers.Remove(n);
                    }
                    lvwNotifiers.Items.RemoveAt(i);
                }
                lblNoNotifiersYet.Visible = monitorPack.Notifiers.Count == 0;
            }
            
        }
        private void disableNotifierToolStripMenuItem_Click(object sender, EventArgs e)
        {
            HideNotifierContextMenu();
            if (lvwNotifiers.SelectedItems.Count == 1)
            {
                NotifierEntry entry = (NotifierEntry)lvwNotifiers.SelectedItems[0].Tag;
                entry.Enabled = !entry.Enabled;
                lvwNotifiers.SelectedItems[0].ForeColor = entry.Enabled ? SystemColors.WindowText : Color.Gray;

                CheckNotifierContextMenuEnables();
                SetMonitorChanged();
            }
        }
        #endregion

        #region Timer events
        private void mainRefreshTimer_Tick(object sender, EventArgs e)
        {
            RefreshMonitorPack();
        }
        #endregion

        #region Refreshing
        private void RefreshMonitorPack(bool disablePollingOverride = false, bool forceUpdateNow = false)
        {
            PausePolling();
            DateTime abortStart = DateTime.Now;
            try
            {
                while (!forceUpdateNow && refreshBackgroundWorker.IsBusy && abortStart.AddSeconds(5) > DateTime.Now)
                {
                    Application.DoEvents();
                }
                if (forceUpdateNow || !refreshBackgroundWorker.IsBusy)
                {
                    Cursor.Current = Cursors.WaitCursor;
                    refreshBackgroundWorker.RunWorkerAsync(disablePollingOverride);
                }
            }
            catch { }
            finally
            {
                mainRefreshTimer.Enabled = timerEnabled;
                ResumePolling();
            }
        }
        private void refreshBackgroundWorker_DoWork(object sender, DoWorkEventArgs e)
        {
            bool disablePollingOverride = false;
            if (e.Argument != null && e.Argument is bool)
                disablePollingOverride = (bool)e.Argument;
            try
            {
                Cursor.Current = Cursors.WaitCursor;
                if (monitorPack != null && monitorPack.Enabled)
                {
                    WaitForPollingToFinish(5);
                    Cursor.Current = Cursors.WaitCursor;                    

                    tvwCollectors.Invoke((MethodInvoker)delegate
                    {
                        try
                        {
                            tvwCollectors.BeginUpdate();
                            SetNodesToBeingRefreshed();
                        }
                        catch { }
                        finally
                        {
                            tvwCollectors.EndUpdate();
                            tvwCollectors.Refresh();
                            Application.DoEvents();
                            Cursor.Current = Cursors.WaitCursor;
                        }
                    });

                    Stopwatch sw = new Stopwatch();
                    sw.Start();
                    Cursor.Current = Cursors.WaitCursor;
                    CollectorState globalState = monitorPack.RefreshStates(disablePollingOverride);
                    sw.Stop();
                    Cursor.Current = Cursors.WaitCursor;
                    PCSetCollectorsQueryTime(sw.ElapsedMilliseconds);
                    SetAppIcon(monitorPack.CurrentState);
                    UpdateStatusbar(string.Format("Global state: {0}, Updated: {1}, Duration: {2} sec, Cur Freq: {3}", 
                        globalState, 
                        DateTime.Now.ToString("HH:mm:ss"), 
                        (sw.ElapsedMilliseconds / 1000.00).ToString("F2"),
                        (mainRefreshTimer.Interval / 1000).ToString()
                        ));
                }
                else
                {
                    SetAppIcon(CollectorState.NotAvailable);
                    UpdateStatusbar("Polling disabled");
                }
            }
            catch (Exception ex)
            {
                UpdateStatusbar("Error: " + ex.Message);
            }
            finally
            {
                Cursor.Current = Cursors.Default;
                firstRefresh = false;
            }
        }                 
        #endregion

        #region Performance counters
        private void InitializeGlobalPerformanceCounters()
        {
            try
            {
                CounterCreationData[] quickMonCreationData = new CounterCreationData[]
                    {
                        new CounterCreationData("Collector success states/Sec", "Collector successful states per second", PerformanceCounterType.RateOfCountsPerSecond32),
                        new CounterCreationData("Collector warning states/Sec", "Collector warning states per second", PerformanceCounterType.RateOfCountsPerSecond32),
                        new CounterCreationData("Collector error states/Sec", "Collector error states per second", PerformanceCounterType.RateOfCountsPerSecond32),                        
                        new CounterCreationData("Collectors queried/Sec", "Number of Collectors queried per second", PerformanceCounterType.RateOfCountsPerSecond32),
                        new CounterCreationData("Collectors query time", "Collector total query time (ms)", PerformanceCounterType.NumberOfItems32),
                        new CounterCreationData("Selected Collector query time", "Selected Collector query time (ms)", PerformanceCounterType.NumberOfItems32)
                    };

                if (PerformanceCounterCategory.Exists(quickMonPCCategory))
                {
                    PerformanceCounterCategory pcC = new PerformanceCounterCategory(quickMonPCCategory);
                    if (pcC.GetCounters().Count() != quickMonCreationData.Length)
                    {
                        PerformanceCounterCategory.Delete(quickMonPCCategory);
                    }
                }

                if (!PerformanceCounterCategory.Exists(quickMonPCCategory))
                {
                    PerformanceCounterCategory.Create(quickMonPCCategory, "QuickMon", PerformanceCounterCategoryType.SingleInstance, new CounterCreationDataCollection(quickMonCreationData));
                }
                try
                {
                    collectorErrorStatePerSec = InitializePerfCounterInstance(quickMonPCCategory, "Collector error states/Sec");
                    collectorWarningStatePerSec = InitializePerfCounterInstance(quickMonPCCategory, "Collector warning states/Sec");
                    collectorInfoStatePerSec = InitializePerfCounterInstance(quickMonPCCategory, "Collector success states/Sec");
                    collectorsQueriedPerSecond = InitializePerfCounterInstance(quickMonPCCategory, "Collectors queried/Sec");
                    collectorsQueryTime = InitializePerfCounterInstance(quickMonPCCategory, "Collectors query time");
                    selectedCollectorsQueryTime = InitializePerfCounterInstance(quickMonPCCategory, "Selected Collector query time");
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Error initializing application performance counters\r\n" + ex.Message, "Performance Counters", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
            catch (Exception ex)
            {
                if (ex.Message.Contains("Requested registry access is not allowed"))
                {
                    if (HenIT.Security.AdminModeTools.IsInAdminMode())
                        MessageBox.Show(string.Format("Could not create performance counters! Please use a user account that has the proper rights.\r\nMore details{0}:", ex.Message), "Performance Counters", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    else //try launching in admin mode
                    {
                        MessageBox.Show("QuickMon 3 needs to restart in 'Admin' mode to set up its performance counters on this computer.", "Restart in Admin mode", MessageBoxButtons.OK, MessageBoxIcon.Information);
                        Properties.Settings.Default.Save();
                        HenIT.Security.AdminModeTools.RestartInAdminMode();
                    }
                }
                else
                    MessageBox.Show("Error creating application performance counters\r\n" + ex.Message, "Performance Counters", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
        public void ClosePerformanceCounters()
        {
            SetCounterValue(collectorsQueryTime, 0, "Collector total query time (ms)");
            SetCounterValue(selectedCollectorsQueryTime, 0, "Selected collector query time (ms)");
        }
        private PerformanceCounter InitializePerfCounterInstance(string categoryName, string counterName)
        {
            PerformanceCounter counter = new PerformanceCounter(categoryName, counterName, false);
            counter.BeginInit();
            counter.RawValue = 0;
            counter.EndInit();
            return counter;
        }
        private void SetCounterValue(PerformanceCounter counter, long value, string description)
        {
            try
            {
                if (counter == null)
                {
                    UpdateStatusbar( "Performance counter not set up or installed! : " + description);
                }
                else
                {
                    counter.RawValue = value;
                }
            }
            catch (Exception ex)
            {
                UpdateStatusbar(string.Format("Increment performance counter error! : {0}\r\n{1}", description, ex.ToString()));
            }
        }
        private void IncrementCounter(PerformanceCounter counter, string description)
        {
            try
            {
                if (counter == null)
                {
                    UpdateStatusbar("Performance counter not set up or installed! : " + description);
                }
                else
                {
                    counter.Increment();
                }
            }
            catch (Exception ex)
            {
                UpdateStatusbar(string.Format("Increment performance counter error! : {0}\r\n{1}", description, ex.ToString()));
            }
        }
        private void PCRaiseCollectorSuccessState()
        {
            IncrementCounter(collectorInfoStatePerSec, "Collector successful states per second");
        }
        private void PCRaiseCollectorWarningState()
        {
            IncrementCounter(collectorWarningStatePerSec, "Collector warning states per second");
        }
        private void PCRaiseCollectorErrorState()
        {
            IncrementCounter(collectorErrorStatePerSec, "Collector error states per second");
        }
        private void PCRaiseCollectorsQueried()
        {
            IncrementCounter(collectorsQueriedPerSecond, "Collectors queried per second");
        }
        private void PCSetCollectorsQueryTime(long time)
        {
            SetCounterValue(collectorsQueryTime, time, "Collector total query time (ms)");
        }
        private void PCSetSelectedCollectorsQueryTime(long time)
        {
            SetCounterValue(selectedCollectorsQueryTime, time, "Selected collector query time (ms)");
        }
        #endregion

        #region PowerShell Runner
        /// <summary>
        /// Run PowerShell script. Cannot use System.Management.Automation as it may not be installed on older systems.
        /// </summary>
        /// <param name="testScript"></param>
        /// <returns></returns>
        private void RunPSScript(string testScript)
        {
            string psExe = System.Environment.GetFolderPath(Environment.SpecialFolder.Windows) + "\\system32\\WindowsPowerShell\\v1.0\\powershell.exe";
            if (System.IO.File.Exists(psExe))
            {
                Process p = new Process();
                p.StartInfo = new ProcessStartInfo(psExe);
                p.StartInfo.Arguments = testScript;
                p.StartInfo.WindowStyle = ProcessWindowStyle.Hidden;
                p.Start();
            }
            else
            {
                throw new Exception("PowerShell not found! It may not be installed on this computer.");
            }
        }
        #endregion

        #region Recent monitor packs drop down and toolbar effects
        private void HideRecentDropDownList (object sender, EventArgs e)
        {
            recentMonitorPacksHideTimer.Enabled = false;
            recentMonitorPacksHideTimer.Enabled = true;
            recentMonitorPacksShowTimer.Enabled = false;
        }
        private void mainToolStrip_MouseLeave(object sender, EventArgs e)
        {
            mainToolbarShrinkTimer.Enabled = false;
            mainToolbarShrinkTimer.Enabled = true;
        }
        private void mainToolStrip_MouseEnter(object sender, EventArgs e)
        {
            mainToolbarShrinkTimer.Enabled = false;
            mainToolStrip.BackColor = Color.FromArgb(64, Color.White);
            HideRecentDropDownList(sender, e);
        }
        private void mainToolbarShrinkTimer_Tick(object sender, EventArgs e)
        {
            mainToolbarShrinkTimer.Enabled = false;
            mainToolStrip.BackColor = Color.Transparent;
        }
        private void recentMonitorPacksVisibleTimer_Tick(object sender, EventArgs e)
        {
            recentMonitorPacksHideTimer.Enabled = false;
            System.Threading.Thread.Sleep(100);
            cboRecentMonitorPacks.Visible = false;
            cmdRecentMonitorPacks.Visible = false;
        }
        private void recentMonitorPacksPanel_MouseEnter(object sender, EventArgs e)
        {
            recentMonitorPacksHideTimer.Enabled = false;
            recentMonitorPacksShowTimer.Enabled = true;
        }
        private void lblMonitorPackPath_MouseEnter(object sender, EventArgs e)
        {
            recentMonitorPacksHideTimer.Enabled = false;
            recentMonitorPacksShowTimer.Enabled = true;
        }
        private void recentMonitorPacksPanel_MouseLeave(object sender, EventArgs e)
        {
            //recentMonitorPacksVisibleTimer.Enabled = false;

        }
        private void cboRecentMonitorPacks_MouseHover(object sender, EventArgs e)
        {
            recentMonitorPacksHideTimer.Enabled = false;
        }
        private void cboRecentMonitorPacks_MouseLeave(object sender, EventArgs e)
        {
            HideRecentDropDownList(sender, e);
        }
        private void cboRecentMonitorPacks_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (cboRecentMonitorPacks.SelectedIndex > 0 && cboRecentMonitorPacks.SelectedItem is QuickMon.Controls.ComboItem)
            {
                CloseAllDetailWindows();
                LoadMonitorPack(((QuickMon.Controls.ComboItem)cboRecentMonitorPacks.SelectedItem).Value.ToString());
                RefreshMonitorPack(true,true);
            }
            HideRecentDropDownList(sender, e);
        }
        private void tvwCollectors_MouseMove(object sender, MouseEventArgs e)
        {
            HideRecentDropDownList(sender, e);
        } 
        private void resizeRecentDropDownListWidthTimer_Tick(object sender, EventArgs e)
        {
            resizeRecentDropDownListWidthTimer.Enabled = false;
            LoadRecentMonitorPackList();
        }
        private void recentMonitorPacksShowTimer_Tick(object sender, EventArgs e)
        {
            recentMonitorPacksShowTimer.Enabled = false;
            cboRecentMonitorPacks.Visible = true;
            cmdRecentMonitorPacks.Visible = true;
        }
        private void llblMonitorPack_MouseEnter(object sender, EventArgs e)
        {
            HideRecentDropDownList(sender, e);
        }
        #endregion

    }
}
