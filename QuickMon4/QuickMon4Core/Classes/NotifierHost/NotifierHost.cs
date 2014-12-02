﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace QuickMon
{
    public partial class NotifierHost : AgentHostBase
    {
        public NotifierHost()
        {
            Enabled = true;
            NotifierAgents = new List<INotifier>();
            AlertForCollectors = new List<string>();
            ServiceWindows = new ServiceWindows();
            ConfigVariables = new List<ConfigVariable>();
            AlertLevel = QuickMon.AlertLevel.Warning;
            DetailLevel = QuickMon.DetailLevel.Detail;
            AttendedOptionOverride = AttendedOption.AttendedAndUnAttended;
        }
        public string NotifierRegistrationName { get; set; }
        public AlertLevel AlertLevel { get; set; }
        public DetailLevel DetailLevel { get; set; }
        public List<string> AlertForCollectors { get; private set; }
        public AttendedOption AttendedOptionOverride { get; set; }
        public List<INotifier> NotifierAgents { get; set; }
         
    }
}