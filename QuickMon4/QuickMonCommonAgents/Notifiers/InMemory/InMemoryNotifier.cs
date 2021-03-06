﻿using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;

namespace QuickMon.Notifiers
{
    [Description("'In memory' Notifier")]
    public class InMemoryNotifier : NotifierAgentBase
    {
        public List<AlertRaised> Alerts = new List<AlertRaised>();
        public InMemoryNotifier()
        {
            AgentConfig = new InMemoryNotifierConfig();
        }

        public override void RecordMessage(AlertRaised alertRaised)
        {
            Alerts.Add(alertRaised);
            //Cleanup
            InMemoryNotifierConfig config = (InMemoryNotifierConfig)AgentConfig;
            if (config.MaxEntryCount > 0)
            {
                while (Alerts.Count > config.MaxEntryCount)
                {
                    Alerts.RemoveAt(0);
                }
            }
        }
        public override AttendedOption AttendedRunOption { get { return AttendedOption.OnlyAttended; } }
    }
}
