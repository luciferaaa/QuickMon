﻿using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using QuickMon.Forms;
using QuickMon.MeansurementUnits;

namespace QuickMon.Collectors
{
    public partial class FileSystemCollectorEditFilterEntry : Form, IAgentConfigEntryEditWindow
    {
        public FileSystemCollectorEditFilterEntry()
        {
            InitializeComponent();
            SelectedFilterEntry = new FileSystemDirectoryFilterEntry();
            SelectedFilterEntry.FileFilter = "*.*";
        }

        #region IEditConfigEntryWindow Members
        public ICollectorConfigEntry SelectedEntry { get; set; }
        public QuickMonDialogResult ShowEditEntry()
        {
            return (QuickMonDialogResult)ShowDialog();
        }
        #endregion

        public FileSystemDirectoryFilterEntry SelectedFilterEntry { get; set; }

        private void FileSystemCollectorEditFilterEntry_Load(object sender, EventArgs e)
        {
            try
            {
                FileSystemDirectoryFilterEntry selectedEntry;
                if (SelectedEntry != null)
                    selectedEntry = (FileSystemDirectoryFilterEntry)SelectedEntry;
                else
                    selectedEntry = (FileSystemDirectoryFilterEntry)SelectedFilterEntry;

                txtDirectory.Text = selectedEntry.DirectoryPath;
                txtFilter.Text = selectedEntry.FileFilter;
                txtContains.Text = selectedEntry.ContainsText;
                chkUseRegEx.Checked = selectedEntry.UseRegEx;
                cboFileAgeUnit.SelectedIndex = (int)selectedEntry.FileAgeUnit;
                numericUpDownFileAgeMin.SaveValueSet(selectedEntry.FileMinAge);
                numericUpDownFileAgeMax.SaveValueSet(selectedEntry.FileMaxAge);
                cboFileSizeUnit.SelectedIndex = (int)selectedEntry.FileSizeUnit;
                numericUpDownFileSizeMin.SaveValueSet(selectedEntry.FileMinSize);
                numericUpDownFileSizeMax.SaveValueSet(selectedEntry.FileMaxSize);

                optDirectoryExistOnly.Checked = selectedEntry.DirectoryExistOnly;
                optCheckIfFilesExistOnly.Checked = selectedEntry.FilesExistOnly;
                optErrorOnFilesExist.Checked = selectedEntry.ErrorOnFilesExist;

                numericUpDownCountWarningIndicator.SaveValueSet(selectedEntry.CountWarningIndicator);
                numericUpDownCountErrorIndicator.SaveValueSet(selectedEntry.CountErrorIndicator);
                cboFileSizeIndicatorUnit.SelectedIndex = (int)selectedEntry.FileSizeIndicatorUnit;
                numericUpDownSizeWarningIndicator.SaveValueSet(selectedEntry.SizeWarningIndicator);
                numericUpDownSizeErrorIndicator.SaveValueSet(selectedEntry.SizeErrorIndicator);
                chkShowFilenamesInDetails.Checked = selectedEntry.ShowFilenamesInDetails;
            }
            catch(Exception ex)
            {
                MessageBox.Show(ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void cmdBrowse_Click(object sender, EventArgs e)
        {
            folderBrowserDialog1.SelectedPath = txtDirectory.Text;
            if (folderBrowserDialog1.ShowDialog() == DialogResult.OK)
            {
                txtDirectory.Text = folderBrowserDialog1.SelectedPath;
            }
        }        

        private void cmdOK_Click(object sender, EventArgs e)
        {
            if (txtFilter.Text.Length == 0)
            {
                txtFilter.Text = "*.*";
            }
            if (txtDirectory.Text.Contains("*"))
            {
                txtFilter.Text = FileSystemDirectoryFilterEntry.GetFilterFromPath(txtDirectory.Text);
                txtDirectory.Text = FileSystemDirectoryFilterEntry.GetDirectoryFromPath(txtDirectory.Text);
            }

            if (!Directory.Exists(txtDirectory.Text))
            {
                MessageBox.Show("Directory must exist and be accessible!", "Directory", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
            }
            else if (optCounts.Checked && numericUpDownCountWarningIndicator.Value == numericUpDownCountErrorIndicator.Value && numericUpDownCountWarningIndicator.Value > 0)
            {
                MessageBox.Show("Error and warning file count values cannot the same!", "Warnings/Errors", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
            }
            else if (optCounts.Checked && numericUpDownSizeWarningIndicator.Value == numericUpDownSizeErrorIndicator.Value && numericUpDownSizeWarningIndicator.Value > 0)
            {
                MessageBox.Show("Error and warning file size values cannot be the same!", "Warnings/Errors", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
            }
            else if (!optDirectoryExistOnly.Checked && numericUpDownFileAgeMin.Value > numericUpDownFileAgeMax.Value)
            {
                MessageBox.Show("File age warning filter value cannot be more than the error value!", "Filters", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
            }
            else if (!optDirectoryExistOnly.Checked && numericUpDownFileSizeMin.Value > numericUpDownFileSizeMax.Value)
            {
                MessageBox.Show("File size warning filter value cannot be more than the error value!", "Filters", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
            }
            else
            {
                FileSystemDirectoryFilterEntry selectedEntry;
                if (SelectedEntry != null)
                    selectedEntry = (FileSystemDirectoryFilterEntry)SelectedEntry;
                else if (SelectedFilterEntry != null)
                    selectedEntry = (FileSystemDirectoryFilterEntry)SelectedFilterEntry;
                else
                {
                    selectedEntry = new FileSystemDirectoryFilterEntry();
                }

                selectedEntry.DirectoryPath = txtDirectory.Text;
                selectedEntry.DirectoryExistOnly = optDirectoryExistOnly.Checked;
                selectedEntry.FileFilter = txtFilter.Text;
                selectedEntry.FilesExistOnly = optCheckIfFilesExistOnly.Checked;
                selectedEntry.ErrorOnFilesExist = optErrorOnFilesExist.Checked;
                selectedEntry.ContainsText = txtContains.Text;
                selectedEntry.UseRegEx = chkUseRegEx.Checked;
                selectedEntry.CountWarningIndicator = Convert.ToInt32(numericUpDownCountWarningIndicator.Value);
                selectedEntry.CountErrorIndicator = Convert.ToInt32(numericUpDownCountErrorIndicator.Value);
                selectedEntry.FileSizeIndicatorUnit = (FileSizeUnits)cboFileSizeIndicatorUnit.SelectedIndex;
                selectedEntry.SizeWarningIndicator = (int)numericUpDownSizeWarningIndicator.Value;
                selectedEntry.SizeErrorIndicator = (int)numericUpDownSizeErrorIndicator.Value;
                selectedEntry.FileAgeUnit = (TimeUnits)cboFileAgeUnit.SelectedIndex;
                selectedEntry.FileMinAge = (int)numericUpDownFileAgeMin.Value;
                selectedEntry.FileMaxAge = (int)numericUpDownFileAgeMax.Value;
                selectedEntry.FileSizeUnit = (FileSizeUnits)cboFileSizeUnit.SelectedIndex;
                selectedEntry.FileMinSize = (int)numericUpDownFileSizeMin.Value;
                selectedEntry.FileMaxSize = (int)numericUpDownFileSizeMax.Value;
                selectedEntry.ShowFilenamesInDetails = chkShowFilenamesInDetails.Checked;

                SelectedEntry = selectedEntry;

                DialogResult = DialogResult.OK;
                Close();
            }
        }

        #region Change tracking
        private void txtDirectory_TextChanged(object sender, EventArgs e)
        {
            CheckOKEnabled();
        }
        private void txtFilter_TextChanged(object sender, EventArgs e)
        {
            CheckOKEnabled();
        }
        private void numericUpDownCountWarningIndicator_ValueChanged(object sender, EventArgs e)
        {
            CheckOKEnabled();
        }
        private void numericUpDownCountErrorIndicator_ValueChanged(object sender, EventArgs e)
        {
            CheckOKEnabled();
        }
        private void numericUpDownSizeWarningIndicator_ValueChanged(object sender, EventArgs e)
        {
            CheckOKEnabled();
        }
        private void numericUpDownSizeErrorIndicator_ValueChanged(object sender, EventArgs e)
        {
            CheckOKEnabled();
        }
        private void numericUpDownFileAgeMin_ValueChanged(object sender, EventArgs e)
        {
            CheckOKEnabled();
        }
        private void numericUpDownFileAgeMax_ValueChanged(object sender, EventArgs e)
        {
            CheckOKEnabled();
        }
        private void numericUpDownFileSizeMin_ValueChanged(object sender, EventArgs e)
        {
            CheckOKEnabled();
        }
        private void numericUpDownFileSizeMax_ValueChanged(object sender, EventArgs e)
        {
            CheckOKEnabled();
        } 
        private void optCounts_CheckedChanged(object sender, EventArgs e)
        {
            SetCheckOptions();
        }
        private void optDirectoryExistOnly_CheckedChanged(object sender, EventArgs e)
        {
            SetCheckOptions();
        }
        private void optCheckIfFilesExistOnly_CheckedChanged(object sender, EventArgs e)
        {
            SetCheckOptions();
        }
        private void optErrorOnFilesExist_CheckedChanged(object sender, EventArgs e)
        {
            SetCheckOptions();
        }

        private void CheckOKEnabled()
        {
            cmdOK.Enabled = txtDirectory.Text.Length > 0 && System.IO.Directory.Exists(txtDirectory.Text) &&
                    txtFilter.Text.Length > 0 &&
                    (optDirectoryExistOnly.Checked || optCheckIfFilesExistOnly.Checked || optErrorOnFilesExist.Checked ||
                        (numericUpDownCountWarningIndicator.Value > 0 || numericUpDownCountErrorIndicator.Value > 0) ||
                        (numericUpDownSizeWarningIndicator.Value > 0 || numericUpDownSizeErrorIndicator.Value > 0)// ||
                        //(numericUpDownFileAgeMin.Value > 0 || numericUpDownFileAgeMax.Value > 0) ||
                        //(numericUpDownFileSizeMin.Value > 0 || numericUpDownFileSizeMax.Value > 0)
                        );
        }
        private void SetCheckOptions()
        {
            txtFilter.Enabled = optCounts.Checked || optErrorOnFilesExist.Checked || optCheckIfFilesExistOnly.Checked;
            txtContains.Enabled = optCounts.Checked || optErrorOnFilesExist.Checked || optCheckIfFilesExistOnly.Checked;
            chkUseRegEx.Enabled = optCounts.Checked || optErrorOnFilesExist.Checked || optCheckIfFilesExistOnly.Checked;
            cboFileAgeUnit.Enabled = optCounts.Checked || optErrorOnFilesExist.Checked || optCheckIfFilesExistOnly.Checked;
            numericUpDownFileAgeMin.Enabled = optCounts.Checked || optErrorOnFilesExist.Checked || optCheckIfFilesExistOnly.Checked;
            numericUpDownFileAgeMax.Enabled = optCounts.Checked || optErrorOnFilesExist.Checked || optCheckIfFilesExistOnly.Checked;
            cboFileSizeUnit.Enabled = optCounts.Checked || optErrorOnFilesExist.Checked || optCheckIfFilesExistOnly.Checked;
            numericUpDownFileSizeMin.Enabled = optCounts.Checked || optErrorOnFilesExist.Checked || optCheckIfFilesExistOnly.Checked;
            numericUpDownFileSizeMax.Enabled = optCounts.Checked || optErrorOnFilesExist.Checked || optCheckIfFilesExistOnly.Checked;
            numericUpDownCountWarningIndicator.Enabled = optCounts.Checked;
            numericUpDownCountErrorIndicator.Enabled = optCounts.Checked;
            cboFileSizeIndicatorUnit.Enabled = optCounts.Checked;
            numericUpDownSizeWarningIndicator.Enabled = optCounts.Checked;
            numericUpDownSizeErrorIndicator.Enabled = optCounts.Checked;
        }
        #endregion

 
    }
}