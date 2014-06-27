﻿using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.ServiceModel.Description;
using System.Text;
using System.Windows.Forms;

namespace QuickMon.Collectors
{
    public partial class DynamicWSCollectorEditEntry : Form, IEditConfigEntryWindow
    {
        private class ValueFormatMacroDisplay
        {
            public string DisplayName { get; set; }
            public WebServiceMacroFormatTypeEnum MacroFormatType { get; set; }
            public override string ToString()
            {
                return DisplayName;
            }
        }
        public DynamicWSCollectorEditEntry()
        {
            InitializeComponent();
        }

        private DynamicWSCollectorConfigEntry editingEntry;
        private DynamicProxyFactory factoryCache = null;

        #region IEditConfigEntryWindow Members
        public ICollectorConfigEntry SelectedEntry { get; set; }
        public QuickMonDialogResult ShowEditEntry()
        {
            return (QuickMonDialogResult)ShowDialog();
        }
        #endregion

        #region Form events
        private void DynamicWSCollectorEditEntry_Load(object sender, EventArgs e)
        {
            if (SelectedEntry != null)
            {
                editingEntry = (DynamicWSCollectorConfigEntry)SelectedEntry;
            }
            else
                editingEntry = new DynamicWSCollectorConfigEntry();
        }
        private void DynamicWSCollectorEditEntry_Shown(object sender, EventArgs e)
        {
            if (editingEntry != null)
            {
                txtServiceURL.Text = editingEntry.ServiceBaseURL;
                cboEndPoint.Text = editingEntry.ServiceBindingName;
                cboMethodName.Text = editingEntry.MethodName;
                txtParameters.Text = editingEntry.ToStringFromParameters();
                chkResultXOr.Checked = !editingEntry.ResultIsSuccess;
                cboExpectedValueType.SelectedIndex = (int)editingEntry.ValueExpectedReturnType;
                cboValueFormatMacro.SelectedItem = (from ValueFormatMacroDisplay v in cboValueFormatMacro.Items
                                                    where v.MacroFormatType == editingEntry.MacroFormatType
                                                    select v).FirstOrDefault();
                indexOrRowNumericUpDown.Value = editingEntry.CheckValueArrayIndex;
                dataSetColumnNumericUpDown.Value = editingEntry.CheckValueColumnIndex;
                cboValueOrMacro.Text = editingEntry.CheckValueOrMacro;
                chkUseRegEx.Checked = editingEntry.UseRegEx;
            }
        }
        #endregion

        #region Control events
        private void cboExpectedValueType_SelectedIndexChanged(object sender, EventArgs e)
        {
            cboValueFormatMacro.Enabled = cboExpectedValueType.SelectedIndex > 0;
            cboValueFormatMacro.Items.Clear();
            if (cboExpectedValueType.SelectedIndex > 0)
            {
                switch ((WebServiceValueExpectedReturnTypeEnum)cboExpectedValueType.SelectedIndex)
                {
                    case WebServiceValueExpectedReturnTypeEnum.CheckAvailabilityOnly:
                        indexOrRowNumericUpDown.Enabled = false;
                        dataSetColumnNumericUpDown.Enabled = false;
                        cboValueOrMacro.Enabled = false;
                        chkUseRegEx.Enabled = false;
                        break;
                    case WebServiceValueExpectedReturnTypeEnum.SingleValue:
                        indexOrRowNumericUpDown.Enabled = false;
                        dataSetColumnNumericUpDown.Enabled = false;
                        cboValueOrMacro.Enabled = true;
                        chkUseRegEx.Enabled = true;
                        cboValueFormatMacro.Items.Add(new ValueFormatMacroDisplay() { DisplayName = "", MacroFormatType = WebServiceMacroFormatTypeEnum.None });
                        cboValueFormatMacro.Items.Add(new ValueFormatMacroDisplay() { DisplayName = "Null/Empty value", MacroFormatType = WebServiceMacroFormatTypeEnum.NoValueOnly });
                        cboValueFormatMacro.Items.Add(new ValueFormatMacroDisplay() { DisplayName = "Length (string characters)", MacroFormatType = WebServiceMacroFormatTypeEnum.Length });
                        break;
                    case WebServiceValueExpectedReturnTypeEnum.Array:
                        indexOrRowNumericUpDown.Enabled = true;
                        dataSetColumnNumericUpDown.Enabled = false;
                        cboValueOrMacro.Enabled = true;
                        chkUseRegEx.Enabled = true;
                        cboValueFormatMacro.Items.Add(new ValueFormatMacroDisplay() { DisplayName = "", MacroFormatType = WebServiceMacroFormatTypeEnum.None });
                        cboValueFormatMacro.Items.Add(new ValueFormatMacroDisplay() { DisplayName = "Count", MacroFormatType = WebServiceMacroFormatTypeEnum.Count });
                        cboValueFormatMacro.Items.Add(new ValueFormatMacroDisplay() { DisplayName = "First value", MacroFormatType = WebServiceMacroFormatTypeEnum.FirstValue });
                        cboValueFormatMacro.Items.Add(new ValueFormatMacroDisplay() { DisplayName = "Last value", MacroFormatType = WebServiceMacroFormatTypeEnum.LastValue });
                        cboValueFormatMacro.Items.Add(new ValueFormatMacroDisplay() { DisplayName = "Sum (only numbers)", MacroFormatType = WebServiceMacroFormatTypeEnum.Sum });
                        break;
                    case WebServiceValueExpectedReturnTypeEnum.DataSet:
                        indexOrRowNumericUpDown.Enabled = true;
                        dataSetColumnNumericUpDown.Enabled = true;
                        cboValueOrMacro.Enabled = true;
                        chkUseRegEx.Enabled = true;
                        cboValueFormatMacro.Items.Add(new ValueFormatMacroDisplay() { DisplayName = "", MacroFormatType = WebServiceMacroFormatTypeEnum.None });
                        cboValueFormatMacro.Items.Add(new ValueFormatMacroDisplay() { DisplayName = "Row count", MacroFormatType = WebServiceMacroFormatTypeEnum.Count });
                        cboValueFormatMacro.Items.Add(new ValueFormatMacroDisplay() { DisplayName = "First value", MacroFormatType = WebServiceMacroFormatTypeEnum.FirstValue });
                        cboValueFormatMacro.Items.Add(new ValueFormatMacroDisplay() { DisplayName = "Last value", MacroFormatType = WebServiceMacroFormatTypeEnum.LastValue });
                        break;
                    default:
                        indexOrRowNumericUpDown.Enabled = false;
                        dataSetColumnNumericUpDown.Enabled = false;
                        cboValueOrMacro.Enabled = true;
                        chkUseRegEx.Enabled = true;
                        break;
                }
            }
        }
        private void cboEndPoint_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (txtServiceURL.Text.Trim().Length > 0)
            {
                try
                {
                    cboMethodName.Items.Clear();
                    if (factoryCache == null || factoryCache.WsdlUri != txtServiceURL.Text)
                        factoryCache = new DynamicProxyFactory(txtServiceURL.Text);
                    ServiceEndpoint endpoint = (from ep in factoryCache.Endpoints
                                                where ep.Name == cboEndPoint.Text
                                                select ep).FirstOrDefault();
                    if (endpoint != null)
                    {
                        string contractName = endpoint.Contract.Name;
                        DynamicProxy proxy = factoryCache.CreateProxy(contractName);
                        Type proxyType = proxy.ProxyType;
                        foreach (OperationDescription operation in endpoint.Contract.Operations)
                        {
                            cboMethodName.Items.Add(operation.Name);
                        }
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
        }
        private void cboMethodName_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (txtServiceURL.Text.Trim().Length > 0 && cboEndPoint.Text.Trim().Length > 0 && cboMethodName.Text.Trim().Length > 0)
            {
                try
                {
                    lblParameterCount.Text = "Parameters info...";
                    lblParameterCount.BackColor = SystemColors.Control;
                    if (factoryCache == null || factoryCache.WsdlUri != txtServiceURL.Text)
                        factoryCache = new DynamicProxyFactory(txtServiceURL.Text);
                    ServiceEndpoint endpoint = (from ep in factoryCache.Endpoints
                                                where ep.Name.ToLower() == cboEndPoint.Text.ToLower()
                                                select ep).FirstOrDefault();
                    if (endpoint != null)
                    {
                        string contractName = endpoint.Contract.Name;
                        DynamicProxy proxy = factoryCache.CreateProxy(contractName);
                        Type proxyType = proxy.ProxyType;
                        OperationDescription operation = (from OperationDescription m in endpoint.Contract.Operations
                                                          where m.Name.ToLower() == cboMethodName.Text.ToLower()
                                                          select m).FirstOrDefault();
                        if (operation != null)
                        {
                            System.Reflection.MethodInfo method = (from m in proxyType.GetMethods()
                                             where m.Name == operation.Name
                                             select m).FirstOrDefault();
                            lblParameterCount.Text = "Count: " + method.GetParameters().Length.ToString() + " ";
                            foreach (System.Reflection.ParameterInfo parInfo in method.GetParameters())
                            {
                                if (!parInfo.ParameterType.IsValueType && parInfo.ParameterType.Name != "String" && parInfo.ParameterType.Name != "Object")
                                    lblParameterCount.BackColor = Color.Yellow;
                                lblParameterCount.Text += string.Format("<{0}:{1}>,", parInfo.Name, parInfo.ParameterType.Name);
                            }
                            lblParameterCount.Text = lblParameterCount.Text.TrimEnd(',');
                        }
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
            
        }
        private void chkUseRegEx_CheckedChanged(object sender, EventArgs e)
        {
            cboValueOrMacro.Items.Clear();
            if (!chkUseRegEx.Checked)
            {
                cboValueOrMacro.Items.AddRange(new string[] { 
                    "[Between] x [and] y",
                    "[LargerThan] x",
                    "[SmallerThan] x",
                    "[Contains] <stringValue>",
                    "[BeginsWith] <stringValue>",
                    "[EndsWith] <stringValue>"            
                });
            }
        }
        #endregion


        #region Change tracking/Input checking
        private void txtServiceURL_TextChanged(object sender, EventArgs e)
        {
            CheckOkTestEnabled();
        } 
        private void cboEndPoint_TextChanged(object sender, EventArgs e)
        {
            CheckOkTestEnabled();
        }
        private void cboMethodName_TextChanged(object sender, EventArgs e)
        {
            CheckOkTestEnabled();
        }
        private void CheckOkTestEnabled()
        {
            cmdOK.Enabled = txtServiceURL.Text.Trim().Length > 0 && cboEndPoint.Text.Trim().Length > 0 && cboMethodName.Text.Trim().Length > 0;
            cmdTestService.Enabled = txtServiceURL.Text.Trim().Length > 0 && cboEndPoint.Text.Trim().Length > 0 && cboMethodName.Text.Trim().Length > 0;
        }
        #endregion

        #region Button events
        private void cmdGetWSDL_Click(object sender, EventArgs e)
        {
            if (txtServiceURL.Text.Trim().Length > 0)
            {
                Cursor.Current = Cursors.WaitCursor;
                try
                {
                    cboEndPoint.Items.Clear();
                    if (factoryCache == null || factoryCache.WsdlUri != txtServiceURL.Text)
                        factoryCache = new DynamicProxyFactory(txtServiceURL.Text);
                    foreach (ServiceEndpoint endpoint in factoryCache.Endpoints)
                    {
                        cboEndPoint.Items.Add(endpoint.Name);
                    }
                }
                catch(Exception ex)
                {
                    if (ex.Message.Contains("There was an error downloading"))
                    {
                        MessageBox.Show("Specified web service invalid or not available!\r\n" + ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }
                    else
                        MessageBox.Show(ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
                Cursor.Current = Cursors.Default;
            }
        }
        private void cmdTestService_Click(object sender, EventArgs e)
        {
            if (txtServiceURL.Text.Trim().Length > 0)
            {
                string lastStep = "Creating entry";
                try
                {                    
                    DynamicWSCollectorConfigEntry textEntry = new DynamicWSCollectorConfigEntry();
                    textEntry.ServiceBaseURL = txtServiceURL.Text;
                    textEntry.ServiceBindingName = cboEndPoint.Text;
                    textEntry.MethodName = cboMethodName.Text;
                    textEntry.ParametersFromString(txtParameters.Text);
                    textEntry.ResultIsSuccess = !chkResultXOr.Checked;
                    textEntry.ValueExpectedReturnType = (WebServiceValueExpectedReturnTypeEnum)cboExpectedValueType.SelectedIndex;
                    if (cboValueFormatMacro.SelectedIndex == -1 || !(cboValueFormatMacro.SelectedItem is ValueFormatMacroDisplay))
                        textEntry.MacroFormatType = WebServiceMacroFormatTypeEnum.None;
                    else
                        textEntry.MacroFormatType = ((ValueFormatMacroDisplay)cboValueFormatMacro.SelectedItem).MacroFormatType;
                    textEntry.CheckValueArrayIndex = (int)indexOrRowNumericUpDown.Value;
                    textEntry.CheckValueColumnIndex = (int)dataSetColumnNumericUpDown.Value;
                    textEntry.CheckValueOrMacro = cboValueOrMacro.Text;
                    textEntry.UseRegEx = chkUseRegEx.Checked;

                    lastStep = "Running method";
                    object returnValue = textEntry.RunMethod();
                    lastStep = "Evaluating return value";
                    CollectorState state = textEntry.GetState(returnValue);
                    MessageBox.Show("Returned state: " + state.ToString() + "\r\nValue: " + textEntry.LastFormattedValue, "Test", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
                catch (Exception ex)
                {
                    if (ex.Message.Contains("Specified web service invalid or not available"))
                        MessageBox.Show(ex.ToString(), "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    else
                    {
                        MessageBox.Show("Last step: " + lastStep + "\r\n" +ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }
                }
            }
        }
        private void cmdOK_Click(object sender, EventArgs e)
        {
            if (txtServiceURL.Text.Trim().Length > 0 && cboEndPoint.Text.Trim().Length > 0 && cboMethodName.Text.Trim().Length > 0)
            {
                if (editingEntry == null)
                    editingEntry = new DynamicWSCollectorConfigEntry();
                try
                {
                    editingEntry.ServiceBaseURL = txtServiceURL.Text;
                    editingEntry.ServiceBindingName = cboEndPoint.Text;
                    editingEntry.MethodName = cboMethodName.Text;
                    editingEntry.ParametersFromString(txtParameters.Text);
                    editingEntry.ResultIsSuccess = !chkResultXOr.Checked;
                    editingEntry.ValueExpectedReturnType = (WebServiceValueExpectedReturnTypeEnum)cboExpectedValueType.SelectedIndex;
                    if (cboValueFormatMacro.SelectedIndex == -1 || !(cboValueFormatMacro.SelectedItem is ValueFormatMacroDisplay))
                        editingEntry.MacroFormatType = WebServiceMacroFormatTypeEnum.None;
                    else
                        editingEntry.MacroFormatType = ((ValueFormatMacroDisplay)cboValueFormatMacro.SelectedItem).MacroFormatType;
                    editingEntry.CheckValueArrayIndex = (int)indexOrRowNumericUpDown.Value;
                    editingEntry.CheckValueColumnIndex = (int)dataSetColumnNumericUpDown.Value;
                    editingEntry.CheckValueOrMacro = cboValueOrMacro.Text;
                    editingEntry.UseRegEx = chkUseRegEx.Checked;
                    SelectedEntry = editingEntry;
                    DialogResult = System.Windows.Forms.DialogResult.OK;
                    Close();
                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
        } 
        #endregion




    }
}
