﻿using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using HenIT.RTF;

namespace QuickMon
{
    public partial class ShowDetails : Form, ICollectorDetailView
    {
        private const int MAXPREVIEWDISPLAYCOUNT = 100; 

        public WMIConfig WmiConfig { get; set; }

        public ShowDetails()
        {
            InitializeComponent();
        }

        #region ICollectorDetailView Members
        public void ShowCollectorDetails(ICollector collector)
        {
            base.Show();
            WmiConfig = null;
            WmiConfig = ((WMIQuery)collector).WmiIConfig;
            LoadList();
            RefreshList();
            Application.DoEvents();
            lvwResults.AutoResizeColumns(ColumnHeaderAutoResizeStyle.HeaderSize);
            //ShowDetails_Resize(null, null);
        }
        public void RefreshConfig(ICollector collector)
        {
            WmiConfig = null;
            WmiConfig = ((WMIQuery)collector).WmiIConfig;
            LoadList();
            RefreshList();
            Application.DoEvents();
            lvwResults.AutoResizeColumns(ColumnHeaderAutoResizeStyle.HeaderSize);
            if (this.WindowState == FormWindowState.Minimized)
                this.WindowState = FormWindowState.Normal;
            this.Show();
        }
        public bool IsStillVisible()
        {
            return (!(this.Disposing || this.IsDisposed)) && this.Visible;
        }
        #endregion

        #region Form events
        private void ShowDetails_Load(object sender, EventArgs e)
        {
            
        }
        private void ShowDetails_Shown(object sender, EventArgs e)
        {
            splitContainerDetails.Panel2Collapsed = true;
        }
        #endregion

        #region Toolbar button events
        private void toolStripButtonRefresh_Click(object sender, EventArgs e)
        {
            RefreshList();
        } 
        private void exportToolStripButton_Click(object sender, EventArgs e)
        {
            try
            {
                if (lvwResults.SelectedItems.Count > 0)
                {
                    StringBuilder sb = new StringBuilder();
                    sb.Append(lvwDetails.Columns[0].Text);
                    for (int i = 1; i < lvwDetails.Columns.Count; i++)
                    {
                        ColumnHeader h = lvwDetails.Columns[i];
                        sb.Append("," + h.Text);
                    }
                    sb.AppendLine();
                    foreach (ListViewItem lvi in lvwDetails.Items)
                    {
                        sb.Append(lvi.Text);
                        for (int i = 1; i < lvwDetails.Columns.Count; i++)
                        {
                            if (lvi.SubItems[i].Text.Contains(','))
                                sb.Append(",\"" + lvi.SubItems[i].Text + "\"");
                            else
                                sb.Append("," + lvi.SubItems[i].Text);
                        }
                        sb.AppendLine();
                    }
                    if (saveFileDialogCSV.ShowDialog() == System.Windows.Forms.DialogResult.OK)
                    {
                        System.IO.File.WriteAllText(saveFileDialogCSV.FileName, sb.ToString());
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Export", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
        #endregion

        #region Private methods
        private void LoadList()
        {
            if (WmiConfig != null)
            {
                lvwResults.Items.Clear();
                foreach (WMIConfigEntry wmiConfigEntry in WmiConfig.Entries)
                {
                    ListViewItem lvi = new ListViewItem(wmiConfigEntry.Name);
                    lvi.SubItems.Add("");
                    lvi.Tag = wmiConfigEntry;
                    lvwResults.Items.Add(lvi);
                }

                //lvwResults.Columns.Clear();
                //List<string> columnNames = new List<string>();
                //List<DataColumn> dataColumns = WmiConfig.GetDetailQueryColumns();
                //if (WmiConfig.ColumnNames.Count > 0)
                //{
                //    if (!WmiConfig.ColumnNames.Contains("Machine"))
                //    {
                //        columnNames.Add("Machine");
                //    }
                //    columnNames.AddRange(WmiConfig.ColumnNames.ToArray());
                //}
                //else
                //{
                //    dataColumns.ForEach(c => columnNames.Add(c.ColumnName));
                //}

                //foreach (string columnName in columnNames)
                //{
                //    DataColumn currentDataColumn = (from dc in dataColumns
                //                                    where dc.ColumnName == columnName
                //                                    select dc).FirstOrDefault();
                //    if (currentDataColumn != null)
                //    {
                //        ColumnHeader newColumn = new ColumnHeader();
                //        newColumn.Tag = currentDataColumn;
                //        newColumn.Text = columnName;

                //        if ((currentDataColumn.DataType == typeof(UInt64)) || (currentDataColumn.DataType == typeof(UInt32)) || (currentDataColumn.DataType == typeof(UInt16)) ||
                //            (currentDataColumn.DataType == typeof(Int64)) || (currentDataColumn.DataType == typeof(Int32)) || (currentDataColumn.DataType == typeof(Int16)))
                //        {
                //            newColumn.TextAlign = HorizontalAlignment.Right;
                //        }
                //        else
                //        {
                //            newColumn.TextAlign = HorizontalAlignment.Left;
                //        }
                //        lvwResults.Columns.Add(newColumn);
                //    }
                //}
            }
        }
        private void RefreshList()
        {
            try
            {
                lvwResults.BeginUpdate();
                Cursor.Current = Cursors.WaitCursor;
                foreach (ListViewItem lvi in lvwResults.Items)
                {
                    WMIConfigEntry wmiConfigEntry = (WMIConfigEntry)lvi.Tag;
                    lvi.SubItems[1].Text = GetQIValue(lvi, wmiConfigEntry);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Refresh", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            finally
            {
                Cursor.Current = Cursors.Default;
                lvwResults.EndUpdate();
            }
            LoadDetailView();

            //if (WmiConfig != null)
            //{
            //    try
            //    {
            //        foreach (ListViewItem lvi in lvwResults.Items)
            //        {
            //            WMIConfigEntry wmiConfigEntry = (WMIConfigEntry)lvi.Tag;
            //            lvi.SubItems[1].Text = GetQIValue(lvi, wmiConfigEntry);
            //        }

            //        DataSet data = WmiConfig.RunDetailQuery();

            //        lvwResults.BeginUpdate();
            //        lvwResults.Items.Clear();
            //        if (data != null && data.Tables.Count > 0 && lvwResults.Columns.Count > 0)
            //        {
            //            foreach (DataRow r in data.Tables[0].Rows)
            //            {
            //                ListViewItem lvi = new ListViewItem();
            //                lvi.Text = r[lvwResults.Columns[0].Text].ToString();
            //                for (int i = 1; i < lvwResults.Columns.Count; i++)
            //                {
            //                    ColumnHeader columnHeader = lvwResults.Columns[i];
            //                    lvi.SubItems.Add(r[columnHeader.Text].ToString());
            //                }
            //                lvwResults.Items.Add(lvi);
            //            }
            //        }
            //    }
            //    catch (Exception ex)
            //    {
            //        MessageBox.Show(ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            //    }
            //    finally
            //    {
            //        lvwResults.EndUpdate();
            //        toolStripStatusLabel1.Text = "Last updated " + DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");
            //    }
            //}
        }

        private void LoadDetailView()
        {
            lvwDetails.BeginUpdate();
            lvwDetails.Items.Clear();
            lvwDetails.Columns.Clear();
            if (lvwResults.SelectedItems.Count == 1 && lvwResults.SelectedItems[0].Tag is WMIConfigEntry)
            {
                WMIConfigEntry wmiConfigEntry = (WMIConfigEntry)lvwResults.SelectedItems[0].Tag;
                try
                {
                    Cursor.Current = Cursors.WaitCursor;
                    DataSet ds = wmiConfigEntry.RunDetailQuery();
                    if (wmiConfigEntry.ColumnNames == null || wmiConfigEntry.ColumnNames.Count == 0)
                    {
                        foreach (DataColumn currentDataColumn in ds.Tables[0].Columns)
                        {
                            ColumnHeader newColumn = new ColumnHeader();
                            newColumn.Tag = currentDataColumn;
                            newColumn.Text = currentDataColumn.Caption;

                            if ((currentDataColumn.DataType == typeof(UInt64)) || (currentDataColumn.DataType == typeof(UInt32)) || (currentDataColumn.DataType == typeof(UInt16)) ||
                                (currentDataColumn.DataType == typeof(Int64)) || (currentDataColumn.DataType == typeof(Int32)) || (currentDataColumn.DataType == typeof(Int16)))
                            {
                                newColumn.TextAlign = HorizontalAlignment.Right;
                            }
                            else
                            {
                                newColumn.TextAlign = HorizontalAlignment.Left;
                            }
                            lvwDetails.Columns.Add(newColumn);
                        }
                        foreach (DataRow r in ds.Tables[0].Rows)
                        {
                            ListViewItem lvi = new ListViewItem(FormatUtils.N(r[0], "[Null]"));
                            for (int i = 1; i < lvwDetails.Columns.Count; i++)
                            {
                                lvi.SubItems.Add(FormatUtils.N(r[i], "[Null]"));
                            }
                            lvwDetails.Items.Add(lvi);
                        }
                    }
                    else
                    {
                        foreach (string colname in wmiConfigEntry.ColumnNames)
                        {
                            ColumnHeader newColumn = new ColumnHeader();
                            newColumn.Text = colname;
                            lvwDetails.Columns.Add(newColumn);
                        }
                        foreach (DataRow r in ds.Tables[0].Rows)
                        {
                            string firstColumnName = wmiConfigEntry.ColumnNames[0];
                            ListViewItem lvi = new ListViewItem(AttemptFieldRead(r, firstColumnName));
                            for (int i = 1; i < wmiConfigEntry.ColumnNames.Count; i++)
                            {
                                lvi.SubItems.Add(AttemptFieldRead(r, wmiConfigEntry.ColumnNames[i]));
                            }                            
                            lvwDetails.Items.Add(lvi);
                        }
                    }
                    lvwDetails.AutoResizeColumns(ColumnHeaderAutoResizeStyle.HeaderSize);
                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.Message, "View details", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
                finally
                {
                    Cursor.Current = Cursors.Default;
                }
            }
            lvwDetails.EndUpdate();
            exportToolStripButton.Enabled = lvwResults.SelectedItems.Count > 0;
        }

        private string AttemptFieldRead(DataRow r, string name)
        {
            string returnValue = "";
            try
            {
                returnValue = FormatUtils.N(r[name], "[Null]");
            }
            catch { }
            return returnValue;
        }
        private string GetQIValue(ListViewItem lvi, WMIConfigEntry wmiConfigEntry)
        {
            string results = "";
            try
            {
                object value = wmiConfigEntry.RunQuery();
                MonitorStates currentstate = wmiConfigEntry.GetState(value);

                results = FormatUtils.N(value, "[null]");
                if (currentstate == MonitorStates.Error)
                    lvi.ImageIndex = 3;
                else if (currentstate == MonitorStates.Warning)
                    lvi.ImageIndex = 2;
                else
                    lvi.ImageIndex = 1;
            }
            catch (Exception ex)
            {
                results = ex.Message;
            }
            return results;
        }
        private void DisplaySelectedItemDetails()
        {
            string oldStatusText = toolStripStatusLabelDetails.Text;
            try
            {
                RTFBuilder rtfBuilder = new RTFBuilder();

                //avoid cursor flickering when only a few items are selected
                //if (lvwDetails.SelectedItems.Count > MAXPREVIEWDISPLAYCOUNT)
                Cursor.Current = Cursors.WaitCursor;
                //have to limit the maximum number of selected items
                foreach (ListViewItem lvi in (from ListViewItem l in lvwDetails.SelectedItems
                                              select l).Take(MAXPREVIEWDISPLAYCOUNT))
                {
                    for (int i = 0; i < lvwDetails.Columns.Count; i++)
                    {
                        rtfBuilder.FontStyle(FontStyle.Bold).Append(lvwDetails.Columns[i].Text + ": ").AppendLine(lvi.SubItems[i].Text);
                    }
                    rtfBuilder.FontStyle(FontStyle.Underline).AppendLine(new String(' ', 250));
                    rtfBuilder.AppendLine();
                }
                if (lvwDetails.SelectedItems.Count > MAXPREVIEWDISPLAYCOUNT)
                {
                    rtfBuilder.FontStyle(FontStyle.Bold).AppendLine(string.Format("Only first {0} entries shown...", MAXPREVIEWDISPLAYCOUNT));
                }
                else if (lvwDetails.SelectedItems.Count == 0)
                    rtfBuilder.FontStyle(FontStyle.Bold).AppendLine("No entries selected");
                else
                {
                    rtfBuilder.FontStyle(FontStyle.Bold).AppendLine(string.Format("{0} entry(s)", lvwDetails.SelectedItems.Count));
                }
                rtxDetails.Rtf = rtfBuilder.ToString();
                rtxDetails.SelectionStart = 0;
                rtxDetails.SelectionLength = 0;
                rtxDetails.ScrollToCaret();

            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            finally
            {
                Cursor.Current = Cursors.Default;
                toolStripStatusLabelDetails.Text = oldStatusText;
            }

        } 
        #endregion

        #region Auto refreshing
        private void autoRefreshToolStripButton_CheckStateChanged(object sender, EventArgs e)
        {
            autoRefreshToolStripMenuItem.Checked = autoRefreshToolStripButton.Checked;
            if (autoRefreshToolStripButton.Checked)
            {
                refreshTimer.Enabled = false;
                refreshTimer.Enabled = true;
                autoRefreshToolStripButton.BackColor = Color.LightGreen;
            }
            else
            {
                refreshTimer.Enabled = false;
                autoRefreshToolStripButton.BackColor = SystemColors.Control;
            }
        }

        private void autoRefreshToolStripMenuItem_CheckStateChanged(object sender, EventArgs e)
        {
            autoRefreshToolStripButton.Checked = autoRefreshToolStripMenuItem.Checked;
        }
        #endregion

        #region ListView events
        private void lvwResults_SelectedIndexChanged(object sender, EventArgs e)
        {
            LoadDetailView();
        }
        private void lvwDetails_SelectedIndexChanged(object sender, EventArgs e)
        {
            timerSelectItem.Enabled = false;
            if (lvwDetails.SelectedItems.Count > 0)
                toolStripStatusLabelDetails.Text = lvwDetails.SelectedItems.Count.ToString() + " item(s) selected";
            else
                toolStripStatusLabelDetails.Text = "0 items selected";
            timerSelectItem.Enabled = true;
        }
        #endregion

        #region Button events
        private void cmdViewDetails_Click(object sender, EventArgs e)
        {
            splitContainerDetails.Panel2Collapsed = !splitContainerDetails.Panel2Collapsed;
            cmdViewDetails.Text = splitContainerDetails.Panel2Collapsed ? "ttt" : "uuu";
            splitContainerDetails.SplitterWidth = 8;
        }
        #endregion

        #region Timer events
        private void refreshTimer_Tick(object sender, EventArgs e)
        {
            RefreshList();
        }
        private void timerSelectItem_Tick(object sender, EventArgs e)
        {
            timerSelectItem.Enabled = false;
            DisplaySelectedItemDetails();
        }
        #endregion

        #region Context menu events
        private void copyToolStripMenuItem_Click(object sender, EventArgs e)
        {
            rtxDetails.Copy();
        }
        private void selectAllToolStripMenuItem_Click(object sender, EventArgs e)
        {
            rtxDetails.SelectAll();
        }
        #endregion

    }
}
