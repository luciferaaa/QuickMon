﻿using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;

namespace QuickMon.Collectors
{
    [Description("WMI Query Collector"), Category("General")]
    public class WMIQueryCollector : CollectorBase
    {
        public WMIQueryCollector()
        {
            AgentConfig = new WMIQueryCollectorConfig();
        }
        public override MonitorState GetState()
        {
            MonitorState returnState = new MonitorState();
            StringBuilder plainTextDetails = new StringBuilder();
            StringBuilder htmlTextTextDetails = new StringBuilder();
            string lastAction = "";
            int errors = 0;
            int warnings = 0;
            int success = 0;
            double totalValue = 0;
            try
            {
                WMIQueryCollectorConfig WmiIConfig = (WMIQueryCollectorConfig)AgentConfig;
                plainTextDetails.AppendLine(string.Format("Running {0} WMI queries", WmiIConfig.Entries.Count));
                htmlTextTextDetails.AppendLine(string.Format("<i>Running {0} WMI queries'</i>", WmiIConfig.Entries.Count));
                htmlTextTextDetails.AppendLine("<ul>");

                foreach (WMIQueryEntry wmiConfigEntry in WmiIConfig.Entries)
                {
                    lastAction = string.Format("Running WMI query for {0} - ", wmiConfigEntry.Name);
                    plainTextDetails.Append(string.Format("\t\t{0} - ", wmiConfigEntry.Name));
                    htmlTextTextDetails.Append(string.Format("<li>{0} - ", wmiConfigEntry.Name));

                    object val = wmiConfigEntry.RunQuery();
                    CollectorState currentState = wmiConfigEntry.GetState(val);
                    if (currentState == CollectorState.Error)
                    {
                        errors++;
                        plainTextDetails.AppendLine(string.Format("Machine '{0}' - value '{1}' - Error (trigger {2})", wmiConfigEntry.Machinename, FormatUtils.N(val, "[null]"), wmiConfigEntry.ErrorValue));
                        htmlTextTextDetails.AppendLine(string.Format("<li>Machine '{0}' - Value '{1}' - <b>Error</b> (trigger {2})</li>", wmiConfigEntry.Machinename, FormatUtils.N(val, "[null]"), wmiConfigEntry.ErrorValue));
                    }
                    else if (currentState == CollectorState.Warning)
                    {
                        warnings++;
                        plainTextDetails.AppendLine(string.Format("Machine '{0}' - value '{1}' - Warning (trigger {2})", wmiConfigEntry.Machinename, FormatUtils.N(val, "[null]"), wmiConfigEntry.WarningValue));
                        htmlTextTextDetails.AppendLine(string.Format("<li>Machine '{0}' - Value '{1}' - <b>Warning</b> (trigger {2})</li>", wmiConfigEntry.Machinename, FormatUtils.N(val, "[null]"), wmiConfigEntry.WarningValue));
                    }
                    else
                    {
                        success++;
                        plainTextDetails.AppendLine(string.Format("Machine '{0}' - value '{1}'", wmiConfigEntry.Machinename, val));
                        htmlTextTextDetails.AppendLine(string.Format("<li>Machine '{0}' - Value '{1}'</li>", wmiConfigEntry.Machinename, val));
                    }
                    if (val != null && val.IsNumber())
                        totalValue += double.Parse(val.ToString());
                }

                htmlTextTextDetails.AppendLine("</ul>");
                returnState.RawDetails = plainTextDetails.ToString().TrimEnd('\r', '\n');
                returnState.HtmlDetails = htmlTextTextDetails.ToString();
                returnState.CurrentValue = totalValue;
                if (errors > 0 && warnings == 0)
                    returnState.State = CollectorState.Error;
                else if (warnings > 0)
                    returnState.State = CollectorState.Warning;
                else
                    returnState.State = CollectorState.Good;
            }
            catch (Exception ex)
            {
                returnState.RawDetails = ex.Message;
                returnState.HtmlDetails = string.Format("<p><b>Last action:</b> {0}</p><blockquote>{1}</blockquote>", lastAction, ex.Message);
                returnState.State = CollectorState.Error;
            }
            return returnState;
        }
        public override ICollectorDetailView GetCollectorDetailView()
        {
            return new WMIQueryCollectorShowDetails();
        }
        //public override IEditConfigWindow GetEditConfigWindow()
        //{
        //    return new WMIQueryCollectorEditConfig();
        //}
        public override string GetDefaultOrEmptyConfigString()
        {
            return Properties.Resources.WMIQueryCollectorDefaultConfig;
        }
        public override IEditConfigEntryWindow GetEditConfigEntryWindow()
        {
            return new WMIQueryCollectorEditEntry();
        }
        //public override List<AgentPresetConfig> GetPresets()
        //{
        //    List<AgentPresetConfig> list = new List<AgentPresetConfig>();
        //    list.Add(new AgentPresetConfig()
        //    {
        //        AgentClassName = this.GetType().Name,
        //        AgentDefaultName = "C drive free space",
        //        Description = "C drive free space",
        //        Config = "<config><wmiQueries><wmiQuery name=\"C drive space left\" namespace=\"root\\CIMV2\" machineName=\".\">\r\n" +
        //                "<stateQuery syntax=\"select FreeSpace from Win32_LogicalDisk where Caption = 'C:'\" returnValueIsInt=\"True\" returnValueInverted=\"True\" warningValue=\"16106127360\" errorValue=\"10737418240\" successValue=\"[any]\" useRowCountAsValue=\"False\" />\r\n" +
        //                "<detailQuery syntax=\"select FreeSpace from Win32_LogicalDisk where Caption = 'C:'\" columnNames=\"\" />\r\n" +
        //                "</wmiQuery>\r\n" +
        //                "</wmiQueries>\r\n" +
        //                "</config>"
        //    });

        //    list.Add(new AgentPresetConfig()
        //    {
        //        AgentClassName = this.GetType().Name,
        //        AgentDefaultName = "Is network up",
        //        Description = "Is network up",
        //        Config = "<config><wmiQueries>\r\n" +
        //        "<wmiQuery name=\"Is network up\" namespace=\"root\\CIMV2\" machineName=\".\">\r\n" +
        //        "		<stateQuery syntax=\"select NetConnectionStatus  from Win32_NetworkAdapter where NetConnectionID = 'Local Area Connection'\" returnValueIsInt=\"False\" returnValueInverted=\"False\" warningValue=\"0\" errorValue=\"[any]\" successValue=\"2\" useRowCountAsValue=\"False\" />\r\n" +
        //        "		<detailQuery syntax=\"select Caption, Speed, NetEnabled, NetConnectionStatus, MACAddress  from Win32_NetworkAdapter where NetConnectionID = 'Local Area Connection'\" columnNames=\"\" />\r\n" +
        //        "</wmiQuery></wmiQueries></config>"
        //    });
        //    return list;
        //}
    }
}
