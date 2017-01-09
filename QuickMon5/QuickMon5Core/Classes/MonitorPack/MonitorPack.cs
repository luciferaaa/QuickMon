using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace QuickMon
{
    public partial class MonitorPack
    {
        public MonitorPack()
        {
            ActionScripts = new List<ActionScript>();
            CollectorHosts = new List<CollectorHost>();
            NotifierHosts = new List<NotifierHost>();
            ConfigVariables = new List<ConfigVariable>();
            Enabled = true;
            PollingFreq = 1000;
            IsPollingEnabled = false;
            CurrentState = CollectorState.NotAvailable;
            PreviousState = CollectorState.NotAvailable;
            ConcurrencyLevel = 1;
            IsBusyPolling = false;
            CollectorStateHistorySize = 100;
            CorrectiveScriptsEnabled = true;
            RunningAttended = AttendedOption.AttendedAndUnAttended;
            //AgentLoadingErrors = "";
            LastMPLoadError = "";
            BlockedCollectorAgentTypes = new List<string>();
            LoggingCollectorCategories = new List<string>();

            LoggingKeepLogFilesXDays = 180;
            lastLoggingCleanupEvent = new DateTime(2000, 1, 1);
        }

        #region Properties

        #region User configurable
        public string Name { get; set; }
        public string Version { get; set; }
        public bool Enabled { get; set; }
        public string TypeName { get; set; }
        public bool CorrectiveScriptsEnabled { get; set; }
        public string MonitorPackPath { get; set; }
        public int CollectorStateHistorySize { get; set; }
        /// <summary>
        /// Polling frequency for background operations. Measured in milliseconds. Default 1 Second
        /// </summary>
        public int PollingFreq { get; set; }
        /// <summary>
        /// Overridding polling frequency as specified in config. If 0 then PollingFreq is used.
        /// </summary>
        public int PollingFrequencyOverrideSec { get; set; }
        public bool PreloadCollectorInstances { get; set; }
        public List<ActionScript> ActionScripts { get; set; }
        public List<CollectorHost> CollectorHosts { get; private set; }
        public List<NotifierHost> NotifierHosts { get; private set; }
        //public NotifierHost DefaultViewerNotifier { get; set; }

        #region Dynamic Config Variables
        public List<ConfigVariable> ConfigVariables { get; set; }
        #endregion
        #endregion

        #region Run-time properties
        //public string AgentLoadingErrors { get; set; }
        public string LastMPLoadError { get; set; }
        public CollectorState CurrentState { get; set; }
        public CollectorState PreviousState { get; set; }
        #region Polling related properties
        /// <summary>
        /// Is background polling active
        /// </summary>
        public bool IsPollingEnabled { get; set; }
        public bool IsBusyPolling { get; private set; }
        public bool AbortPolling { get; set; }
        #endregion
        public long LastRefreshDurationMS { get; private set; }
        #endregion

        #region Settings set by the hosting application
        /// <summary>
        /// Degree of parallelism
        /// </summary>
        public int ConcurrencyLevel { get; set; }
        public AttendedOption RunningAttended { get; set; }
        #endregion        

        #region Security
        public string UserNameCacheMasterKey { get; set; }
        public string UserNameCacheFilePath { get; set; }
        //Run time setting only
        public string ApplicationUserNameCacheMasterKey { get; set; }
        public string ApplicationUserNameCacheFilePath { get; set; }
        #endregion

        #region Globally Disable/block Agent types if it is not supported for some reason
        public List<string> BlockedCollectorAgentTypes { get; set; }
        #endregion

        #endregion

        #region Refreshing states
        public CollectorState RefreshStates(bool disablePollingOverrides = false)
        {
            AbortPolling = false;
            IsBusyPolling = true;
            CollectorState globalState = CollectorState.Good;
            ResetAllOverrides();
            Stopwatch sw = new Stopwatch();
            sw.Start();
            //First get collectors with no dependancies
            List<CollectorHost> rootCollectorHosts = (from c in CollectorHosts
                                                      where c.ParentCollectorId.Length == 0
                                                      select c).ToList();
            if (ConcurrencyLevel > 1)
            {
                ParallelOptions po = new ParallelOptions()
                {
                    MaxDegreeOfParallelism = ConcurrencyLevel
                };
                ParallelLoopResult parResult = Parallel.ForEach(rootCollectorHosts, po, collectorHost =>
                        CollectorHostRefreshCurrentState(collectorHost, disablePollingOverrides));
                if (!parResult.IsCompleted)
                {
                    SendNotifierAlert(AlertLevel.Error, DetailLevel.All, "Error querying collectors in parralel");
                }
            }
            else //use old single threaded way
            {
                //Refresh states
                foreach (CollectorHost collectorHost in rootCollectorHosts)
                {
                    CollectorHostRefreshCurrentState(collectorHost, disablePollingOverrides);
                }
            }
            sw.Stop();
            PCSetCollectorsQueryTime(sw.ElapsedMilliseconds);
            LastRefreshDurationMS = sw.ElapsedMilliseconds;
#if DEBUG
            Trace.WriteLine(string.Format("RefreshStates - Global time: {0}ms", sw.ElapsedMilliseconds));
#endif

            #region Get Global state
            //All disabled
            if (CollectorHosts.Count == CollectorHosts.Count(c => c.CurrentState.State == CollectorState.Disabled))
                globalState = CollectorState.Disabled;
            //All NotAvailable
            else if (CollectorHosts.Count == CollectorHosts.Count(c => c.CurrentState.State == CollectorState.NotAvailable))
                globalState = CollectorState.NotAvailable;
            //All good
            else if (CollectorHosts.Count == CollectorHosts.Count(c => c.CurrentState.State == CollectorState.Good ||
                                                                  c.CurrentState.State == CollectorState.None ||
                                                                  c.CurrentState.State == CollectorState.Disabled))
                globalState = CollectorState.Good;
            //Error state
            else if (CollectorHosts.Count == CollectorHosts.Count(c => c.CurrentState.State == CollectorState.Error ||
                                                                  c.CurrentState.State == CollectorState.ConfigurationError ||
                                                                  c.CurrentState.State == CollectorState.Disabled))
                globalState = CollectorState.Error;
            else
                globalState = CollectorState.Warning;

            AlertLevel globalAlertLevel = AlertLevel.Info;
            if (globalState == CollectorState.Error)
                globalAlertLevel = AlertLevel.Error;
            else if (globalState == CollectorState.Warning)
                globalAlertLevel = AlertLevel.Warning;
            #endregion

            sw.Restart();
            SendNotifierAlert(globalAlertLevel, DetailLevel.Summary, "GlobalState");
            sw.Stop();
            PCSetNotifiersSendTime(sw.ElapsedMilliseconds);
            IsBusyPolling = false;
            CurrentState = globalState;
            return globalState;
        }
        private void CollectorHostRefreshCurrentState(CollectorHost collectorHost, bool disablePollingOverrides = false)
        {
            if (!AbortPolling)
            {
                RaiseCollectorHostCalled(collectorHost);
                try
                {
                    MonitorState chms = null;

                    collectorHost.RunTimeMasterKey = "";
                    collectorHost.RunTimeUserNameCacheFile = "";
                    collectorHost.BlockedCollectorAgentTypes = new List<string>();
                    collectorHost.BlockedCollectorAgentTypes.AddRange(BlockedCollectorAgentTypes.ToArray());

                    if (collectorHost.RunAsEnabled && collectorHost.RunAs != null && collectorHost.RunAs.Length > 0)
                    {
                        if (UserNameCacheMasterKey.Length > 0 && System.IO.File.Exists(UserNameCacheFilePath) &&
                                QuickMon.Security.CredentialManager.IsAccountPersisted(UserNameCacheFilePath, UserNameCacheMasterKey, collectorHost.RunAs) &&
                                QuickMon.Security.CredentialManager.IsAccountDecryptable(UserNameCacheFilePath, UserNameCacheMasterKey, collectorHost.RunAs))
                        {
                            collectorHost.RunTimeMasterKey = UserNameCacheMasterKey;
                            collectorHost.RunTimeUserNameCacheFile = UserNameCacheFilePath;
                        }
                        else if (ApplicationUserNameCacheMasterKey != null && ApplicationUserNameCacheFilePath != null &&
                            ApplicationUserNameCacheMasterKey.Length > 0 && System.IO.File.Exists(ApplicationUserNameCacheFilePath) &&
                            QuickMon.Security.CredentialManager.IsAccountPersisted(ApplicationUserNameCacheFilePath, ApplicationUserNameCacheMasterKey, collectorHost.RunAs) &&
                            QuickMon.Security.CredentialManager.IsAccountDecryptable(ApplicationUserNameCacheFilePath, ApplicationUserNameCacheMasterKey, collectorHost.RunAs))
                        {
                            collectorHost.RunTimeMasterKey = ApplicationUserNameCacheMasterKey;
                            collectorHost.RunTimeUserNameCacheFile = ApplicationUserNameCacheFilePath;
                        }
                    }
                    //collectorHost.ApplyConfigVarsNow(ConfigVariables);                    
                    chms = collectorHost.RefreshCurrentState(disablePollingOverrides);

                    #region Do/Check/Set dependant CollectorHosts
                    if (chms.State == CollectorState.Error && collectorHost.ChildCheckBehaviour != ChildCheckBehaviour.ContinueOnWarningOrError)
                        SetDependantCollectorHostStates(collectorHost, CollectorState.NotAvailable);
                    else if (chms.State == CollectorState.Warning && collectorHost.ChildCheckBehaviour == ChildCheckBehaviour.OnlyRunOnSuccess)
                        SetDependantCollectorHostStates(collectorHost, CollectorState.NotAvailable);
                    else if (chms.State == CollectorState.Disabled || chms.State == CollectorState.ConfigurationError || !collectorHost.IsEnabledNow())
                        SetDependantCollectorHostStates(collectorHost, CollectorState.Disabled);
                    else
                    {
                        //Remote execute and dependant Collector Hosts
                        if (collectorHost.ForceRemoteExcuteOnChildCollectors)
                        {
                            collectorHost.OverrideForceRemoteExcuteOnChildCollectors = true;
                            SetChildCollectorHostRemoteExecuteDetails(collectorHost, collectorHost.RemoteAgentHostAddress, collectorHost.RemoteAgentHostPort);
                        }
                        //Polling overrides and dependant Collector Hosts
                        if (collectorHost.EnabledPollingOverride)
                        {
                            SetChildCollectorHostPollingOverrides(collectorHost);
                        }

                        #region Loop through dependant CollectorHosts
                        if (ConcurrencyLevel > 1)
                        {
                            ParallelOptions po = new ParallelOptions()
                            {
                                MaxDegreeOfParallelism = ConcurrencyLevel
                            };
                            ParallelLoopResult parResult = Parallel.ForEach((from c in CollectorHosts
                                                                             where c.ParentCollectorId == collectorHost.UniqueId
                                                                             select c),
                                        po, dependentCollectorHost =>
                                    CollectorHostRefreshCurrentState(dependentCollectorHost, disablePollingOverrides));
                            if (!parResult.IsCompleted)
                            {
                                SendNotifierAlert(AlertLevel.Error, DetailLevel.All, "Error querying collectors in parralel");
                            }
                        }
                        else //use old single threaded way
                        {
                            //Refresh states
                            foreach (CollectorHost dependentCollectorHost in (from c in CollectorHosts
                                                                              where c.ParentCollectorId == collectorHost.UniqueId
                                                                              select c))
                            {
                                CollectorHostRefreshCurrentState(dependentCollectorHost, disablePollingOverrides);
                            }
                        }
                        #endregion
                    }
                    #endregion

                    LoggingCollectorEvent(string.Format("Collector '{0}' return state '{1}'", collectorHost.Name, collectorHost.CurrentState), collectorHost);
                }
                catch (Exception ex)
                {
                    WriteLogging(string.Format("CollectorHostRefreshCurrentState error: {0}", ex.Message));
                    if (!ex.Message.Contains("Collection was modified; enumeration operation may not execute"))
                        RaiseMonitorPackError("Internal error. Collector config was modified while in use!");
                    else
                        RaiseCollectorError(collectorHost, ex.Message);
                }
            }
        }
        #endregion

        #region Async refreshing
        public void StartPolling()
        {
            IsPollingEnabled = true;
            if (PollingFrequencyOverrideSec > 0)
                PollingFreq = PollingFrequencyOverrideSec * 1000;
            ThreadPool.QueueUserWorkItem(new WaitCallback(BackgroundPolling));
        }
        private void BackgroundPolling(object o)
        {
            DateTime lastMonitorPackFileUpdate = DateTime.Now;
            System.IO.FileInfo mfi = new System.IO.FileInfo(MonitorPackPath);
            if (mfi.Exists)
            {
                lastMonitorPackFileUpdate = mfi.LastWriteTime;
            }
            while (IsPollingEnabled)
            {
                try
                {
                    RefreshStates();
                }
                catch (Exception ex)
                {
                    RaiseMonitorPackError(ex.Message);
                    WriteLogging(string.Format("Error in BackgroundPolling: {0}", ex.Message));
                }
                BackgroundWaitIsPolling(PollingFreq);
                try
                {
                    //Update FileInfo object
                    mfi.Refresh();
                    if (mfi.Exists && (lastMonitorPackFileUpdate.AddSeconds(1) < mfi.LastWriteTime))
                    {
                        //Load everything over again
                        Load();
                        lastMonitorPackFileUpdate = mfi.LastWriteTime;
                        RaiseMonitorPackEventReported(string.Format("The MonitorPack '{0}' was reloaded because the definition file ({1}) was updated ({2})!", Name, MonitorPackPath, lastMonitorPackFileUpdate));
                        WriteLogging(string.Format("The MonitorPack '{0}' was reloaded because the definition file ({1}) was updated ({2})!", Name, MonitorPackPath, lastMonitorPackFileUpdate));
                    }
                }
                catch (Exception ex)
                {
                    RaiseMonitorPackError(ex.Message);
                    WriteLogging(string.Format("Error in BackgroundPolling: {0}", ex.Message));
                }
            }
            ClosePerformanceCounters();
        }
        private void BackgroundWaitIsPolling(int nextWaitInterval)
        {
            int waitTimeRemaining;
            int decrementBy = 2000;
            if (IsPollingEnabled)
            {
                try
                {
                    if ((nextWaitInterval <= decrementBy) && (nextWaitInterval > 0))
                    {
                        Thread.Sleep(nextWaitInterval);
                    }
                    else
                    {
                        waitTimeRemaining = nextWaitInterval;
                        while (IsPollingEnabled && (waitTimeRemaining > 0))
                        {
                            if (waitTimeRemaining <= decrementBy)
                            {
                                waitTimeRemaining = 0;
                            }
                            else
                            {
                                waitTimeRemaining -= decrementBy;
                            }
                            if (decrementBy > 0)
                            {
                                Thread.Sleep(decrementBy);
                            }
                        }
                    }
                }
                catch { }
            }
        }
        #endregion

        #region Recursively set child properties
        private void ResetAllOverrides(CollectorHost parentCollector = null)
        {
            List<CollectorHost> collectors = null;
            if (parentCollector == null)
                collectors = (from c in CollectorHosts
                              where c.ParentCollectorId.Length == 0
                              select c).ToList();
            else
                collectors = (from c in CollectorHosts
                              where c.ParentCollectorId == parentCollector.UniqueId
                              select c).ToList();
            foreach (CollectorHost childCollector in collectors)
            {
                childCollector.MaxStateHistorySize = CollectorStateHistorySize;
                //Remote agent host
                childCollector.OverrideForceRemoteExcuteOnChildCollectors = false;
                childCollector.OverrideRemoteAgentHost = false;
                childCollector.OverrideRemoteAgentHostAddress = "";
                childCollector.OverrideRemoteAgentHostPort = GlobalConstants.DefaultRemoteHostPort;
                ResetAllOverrides(childCollector);
            }
        }
        private void SetDependantCollectorHostStates(CollectorHost collectorHost, CollectorState collectorState)
        {
            //Set states without adding to history
            try
            {
                foreach (CollectorHost childCollectorHost in (from c in CollectorHosts
                                                              where c.ParentCollectorId == collectorHost.UniqueId
                                                              select c))
                {
                    childCollectorHost.UpdateCurrentCollectorState(collectorState);
                    SetDependantCollectorHostStates(childCollectorHost, collectorState);
                }
            }
            catch (Exception ex)
            {
                if (!ex.Message.Contains("Collection was modified; enumeration operation may not execute"))
                    throw;
            }
        }
        private void SetChildCollectorHostRemoteExecuteDetails(CollectorHost collectorHost, string remoteAgentHostAddress, int remoteAgentHostPort)
        {
            foreach (CollectorHost childCollector in (from c in CollectorHosts
                                                      where c.ParentCollectorId == collectorHost.UniqueId
                                                      select c))
            {
                childCollector.OverrideForceRemoteExcuteOnChildCollectors = collectorHost.OverrideForceRemoteExcuteOnChildCollectors;
                childCollector.OverrideRemoteAgentHost = remoteAgentHostAddress.Length > 0;
                childCollector.OverrideRemoteAgentHostAddress = remoteAgentHostAddress;
                childCollector.OverrideRemoteAgentHostPort = remoteAgentHostPort;

                //Set grand children
                SetChildCollectorHostRemoteExecuteDetails(childCollector, remoteAgentHostAddress, remoteAgentHostPort);
            }
        }
        private void SetChildCollectorHostPollingOverrides(CollectorHost parentCollectorHost)
        {
            foreach (CollectorHost childCollector in (from c in CollectorHosts
                                                      where c.ParentCollectorId == parentCollectorHost.UniqueId
                                                      select c))
            {
                if (!childCollector.EnabledPollingOverride) //check that child does not have its own 
                {
                    childCollector.OnlyAllowUpdateOncePerXSec = parentCollectorHost.OnlyAllowUpdateOncePerXSec;
                    //to make sure child collector does not miss the poll event
                    childCollector.LastStateUpdate = parentCollectorHost.LastStateUpdate;
                }
                else
                {
                    if (childCollector.OnlyAllowUpdateOncePerXSec < parentCollectorHost.OnlyAllowUpdateOncePerXSec)
                    {
                        childCollector.OnlyAllowUpdateOncePerXSec = parentCollectorHost.OnlyAllowUpdateOncePerXSec;
                        //to make sure child collector does not miss the poll event
                        childCollector.LastStateUpdate = parentCollectorHost.LastStateUpdate;
                    }
                }
                //force child settings permanently
                childCollector.EnabledPollingOverride = true;

                //Set grand children
                SetChildCollectorHostPollingOverrides(childCollector);
            }
        }
        #endregion

        #region Sending Alerts
        /// <summary>
        /// For sending generic alerts where no Collector is in volved
        /// </summary>
        /// <param name="alertLevel"></param>
        /// <param name="detailLevel"></param>
        /// <param name="statusMessage"></param>
        /// <param name="collectorState"></param>
        private void SendNotifierAlert(AlertLevel alertLevel, DetailLevel detailLevel, string messageRaw, string messageHtml = "")
        {
            if (messageHtml == null || messageHtml.Length == 0)
                messageHtml = string.Format("<p>{0}</p>", messageRaw.EscapeXml());
            SendNotifierAlert(new AlertRaised()
            {
                Level = alertLevel,
                DetailLevel = detailLevel,
                RaisedFor = null,
                MessageRaw = messageRaw,
                MessageHTML = messageHtml
            });
        }
        /// <summary>
        /// For use when a CollectorHost raise an alert
        /// </summary>
        /// <param name="alertLevel"></param>
        /// <param name="detailLevel"></param>
        /// <param name="raisedFor"></param>
        private void SendNotifierAlert(AlertLevel alertLevel, DetailLevel detailLevel, CollectorHost raisedFor)
        {
            if (raisedFor != null)
            {
                SendNotifierAlert(new AlertRaised()
                {
                    Level = alertLevel,
                    DetailLevel = detailLevel,
                    RaisedFor = raisedFor,
                    MessageRaw = raisedFor.CurrentState.ReadAllRawDetails(),
                    MessageHTML = raisedFor.CurrentState.ReadAllHtmlDetails()
                });
            }
        }
        private void SendNotifierAlert(AlertRaised alertRaised)
        {
            Stopwatch sw = new Stopwatch();
            sw.Start();
            if (alertRaised != null && alertRaised.RaisedFor != null && alertRaised.RaisedFor.CurrentState != null)
            {
                alertRaised.RaisedFor.CurrentState.AlertsRaised = new List<string>();
                if (alertRaised.MessageRaw == null)
                    alertRaised.MessageRaw = "";
                if (alertRaised.MessageHTML == null)
                    alertRaised.MessageHTML = "";
                if (alertRaised.MessageRaw.Length == 0)
                    alertRaised.MessageRaw = alertRaised.RaisedFor.CurrentState.ReadAllRawDetails();
                if (alertRaised.MessageHTML.Length == 0)
                    alertRaised.MessageHTML = alertRaised.RaisedFor.CurrentState.ReadAllHtmlDetails();

                /*
                if (alertRaised.Level != AlertLevel.Debug &&
                        (
                            (alertRaised.RaisedFor.AlertHeaderText != null && alertRaised.RaisedFor.AlertHeaderText.Length > 0) ||
                            (alertRaised.RaisedFor.AlertFooterText != null && alertRaised.RaisedFor.AlertFooterText.Length > 0) ||
                            (alertRaised.Level == AlertLevel.Error && alertRaised.RaisedFor.ErrorAlertText != null && alertRaised.RaisedFor.ErrorAlertText.Length > 0) ||
                            (alertRaised.Level == AlertLevel.Warning && alertRaised.RaisedFor.WarningAlertText != null && alertRaised.RaisedFor.WarningAlertText.Length > 0) ||
                            (alertRaised.Level == AlertLevel.Info && alertRaised.RaisedFor.GoodAlertText != null && alertRaised.RaisedFor.GoodAlertText.Length > 0)
                        )
                    )
                {
                    string additionalHeaderText = "";
                    string additionalFooterText = "";
                    string additionalLevelText = "";

                    if (alertRaised.RaisedFor.AlertHeaderText != null && alertRaised.RaisedFor.AlertHeaderText.Length > 0)
                    {
                        additionalHeaderText = alertRaised.RaisedFor.AlertHeaderText;
                    }
                    if (alertRaised.RaisedFor.AlertFooterText != null && alertRaised.RaisedFor.AlertFooterText.Length > 0)
                    {
                        additionalFooterText = alertRaised.RaisedFor.AlertFooterText;
                    }

                    if (alertRaised.Level == AlertLevel.Error && alertRaised.RaisedFor.ErrorAlertText != null && alertRaised.RaisedFor.ErrorAlertText.Length > 0)
                    {
                        additionalLevelText = alertRaised.RaisedFor.ErrorAlertText;
                    }
                    if (alertRaised.Level == AlertLevel.Warning && alertRaised.RaisedFor.WarningAlertText != null && alertRaised.RaisedFor.WarningAlertText.Length > 0)
                    {
                        additionalLevelText = alertRaised.RaisedFor.WarningAlertText;
                    }
                    if (alertRaised.Level == AlertLevel.Info && alertRaised.RaisedFor.GoodAlertText != null && alertRaised.RaisedFor.GoodAlertText.Length > 0)
                    {
                        additionalLevelText = alertRaised.RaisedFor.GoodAlertText;
                    }

                    if (additionalLevelText.Length > 0)
                    {
                        alertRaised.MessageRaw = additionalLevelText + "\r\n" + alertRaised.MessageRaw;
                        alertRaised.MessageHTML = "<p>" + additionalLevelText + "</p>" + alertRaised.MessageHTML;
                    }
                    if (additionalHeaderText.Length > 0)
                    {
                        alertRaised.MessageRaw = additionalHeaderText + "\r\n" + alertRaised.MessageRaw;
                        alertRaised.MessageHTML = "<p>" + additionalHeaderText + "</p>" + alertRaised.MessageHTML;
                    }
                    if (additionalFooterText.Length > 0)
                    {
                        alertRaised.MessageRaw = alertRaised.MessageRaw.TrimEnd('\r', '\n') + "\r\n" + additionalFooterText;
                        alertRaised.MessageHTML += "<p>" + additionalFooterText + "</p>";
                    }
                }
                */
            }

            //If alerts are paused for CollectorHost...
            if (alertRaised != null && alertRaised.RaisedFor != null && alertRaised.RaisedFor.AlertsPaused)
            {

            }
            else
            {
                //First check each NotifierHost if it is enabled and should apply to the alert
                foreach (NotifierHost notifierEntry in (from n in NotifierHosts
                                                        where
                                                           n.IsEnabledNow() &&
                                                           (int)n.AlertLevel <= (int)alertRaised.Level &&
                                                           (alertRaised.DetailLevel == DetailLevel.All || alertRaised.DetailLevel == n.DetailLevel) &&
                                                           (n.OnlyRecordAlertOnHosts == null || n.OnlyRecordAlertOnHosts.Count == 0 || n.OnlyRecordAlertOnHosts.Any(h => h.ToLower() == System.Net.Dns.GetHostName().ToLower())) &&
                                                           (alertRaised.RaisedFor == null || n.AlertForCollectors.Count == 0 || n.AlertForCollectors.Contains(alertRaised.RaisedFor.Name)) &&
                                                           (alertRaised.RaisedFor == null || n.Categories == null || n.IsCollectorInCategory(alertRaised.RaisedFor))

                                                        select n))
                {
                    try
                    {
                        bool allowedToRun = true;
                        List<string> alertsRecorded = new List<string>();

                        notifierEntry.ApplyConfigVarsNow();
                        foreach (INotifier notifierAgent in notifierEntry.NotifierAgents)
                        {
                            if (RunningAttended == AttendedOption.AttendedAndUnAttended) //no Attended option set on MonitorPack
                            {
                                allowedToRun = true;
                            }
                            else if (RunningAttended == AttendedOption.OnlyAttended) //Running in Attended mode
                            {
                                if (notifierAgent.AttendedRunOption == AttendedOption.OnlyUnAttended || notifierEntry.AttendedOptionOverride == AttendedOption.OnlyUnAttended)
                                    allowedToRun = false;
                            }
                            else //unattended mode
                            {
                                if (notifierAgent.AttendedRunOption == AttendedOption.OnlyAttended || notifierEntry.AttendedOptionOverride == AttendedOption.OnlyAttended)
                                    allowedToRun = false;
                            }

                            if (allowedToRun)
                            {
                                PCRaiseNotifierAlertSend();
                                
                                notifierAgent.RecordMessage(alertRaised);

                                if (notifierAgent.AgentConfig != null)
                                {
                                    string configSummary = ((INotifierConfig)notifierAgent.AgentConfig).ConfigSummary;
                                    alertsRecorded.Add(string.Format("{0} ({1})", notifierAgent.AgentClassDisplayName, configSummary));
                                    LoggingAlertsRaisedEvent(string.Format("Alert raised for Collector '{0}'\r\nNotifier: '{1}'\r\n{2}", alertRaised.RaisedFor.Name, notifierAgent.Name, alertRaised.MessageRaw));
                                }
                                else
                                {
                                    LoggingAlertsRaisedEvent(string.Format("Alert raised notifier: '{0}'\r\n{1}", notifierAgent.Name, alertRaised.MessageRaw));
                                }

                            }
                        }
                        if (alertsRecorded.Count > 0 && alertRaised.RaisedFor != null && alertRaised.RaisedFor.CurrentState != null)
                        {
                            StringBuilder sbAlertsRaisedSummary = new StringBuilder();
                            sbAlertsRaisedSummary.AppendLine(notifierEntry.Name);
                            alertsRecorded.ForEach(araised => sbAlertsRaisedSummary.AppendLine("  " + araised));
                            alertRaised.RaisedFor.CurrentState.AlertsRaised.Add(sbAlertsRaisedSummary.ToString());
                        }
                        LoggingNotifierEvent(string.Format("Notifier '{0}' called for {1} alert on '{2}'", notifierEntry.Name, alertRaised.Level, (alertRaised.RaisedFor == null ? "Unknown Collector" : alertRaised.RaisedFor.Name)));
                    }
                    catch (Exception ex)
                    {
                        RaiseNotifierError(notifierEntry, ex.ToString());
                        WriteLogging(string.Format("Error in SendNotifierAlert: {0}\r\nNotifier: {1}", ex.Message, notifierEntry.Name));
                    }
                }
            }
            sw.Stop();
            PCSetNotifiersSendTime(sw.ElapsedMilliseconds);
            PCRaiseNotifiersCalled();
        }
        #endregion

        #region Corrective script actions
        private void LogRestorationScriptAction(CollectorHost collectorEntry, string script)
        {
            LoggingCorrectiveScriptRunEvent(string.Format("Due to an alert raised on the collector '{0}' the following restoration script was executed: '{1}'", collectorEntry.Name, script));
        }
        private void LogRestorationScriptFailedAction(CollectorHost collectorEntry, string script, string errorMessage)
        {
            LoggingCorrectiveScriptRunEvent(string.Format("The restoration script '{0}' for the collector '{1}' failed to run: '{2}'", script, collectorEntry.Name, errorMessage));
        }
        private void LogCorrectiveScriptAction(CollectorHost collectorEntry, string script)
        {
            LoggingCorrectiveScriptRunEvent(string.Format("Due to an alert raised on the collector '{0}' the following corrective script was executed: '{1}'", collectorEntry.Name, script));
        }
        private void LogCorrectiveScriptFailedAction(CollectorHost collectorEntry, string script, string errorMessage)
        {
            LoggingCorrectiveScriptRunEvent(string.Format("The corrective script '{0}' for the collector '{1}' failed to run: '{2}'", script, collectorEntry.Name, errorMessage));
        }


        //private void LogRestorationScriptAction(CollectorHost collectorEntry)
        //{
        //    collectorEntry.CurrentState.RawDetails += "\r\n" + string.Format("Due to an earlier alert raised on the collector '{0}' the following restoration script was executed: '{1}'",
        //        collectorEntry.Name, collectorEntry.RestorationScriptPath);
        //    SendNotifierAlert(new AlertRaised()
        //    {
        //        Level = collectorEntry.PreviousState.State == CollectorState.Warning ? AlertLevel.Warning : AlertLevel.Error,
        //        DetailLevel = DetailLevel.Detail,
        //        RaisedFor = collectorEntry
        //    });
        //    LoggingCorrectiveScriptRunEvent(string.Format("Due to an earlier alert raised on the collector '{0}' the following restoration script was executed: '{1}'",
        //        collectorEntry.Name, collectorEntry.RestorationScriptPath));
        //}
        //private void LogCorrectiveScriptAction(CollectorHost collectorEntry, bool error)
        //{
        //    collectorEntry.CurrentState.RawDetails += "\r\n" + string.Format("Due to an alert raised on the collector '{0}' the following corrective script was executed: '{1}'",
        //        collectorEntry.Name, error ? collectorEntry.CorrectiveScriptOnErrorPath : collectorEntry.CorrectiveScriptOnWarningPath);
        //    SendNotifierAlert(new AlertRaised()
        //    {
        //        Level = error ? AlertLevel.Error : AlertLevel.Warning,
        //        DetailLevel = DetailLevel.Detail,
        //        RaisedFor = collectorEntry
        //    });
        //    LoggingCorrectiveScriptRunEvent(string.Format("Due to an alert raised on the collector '{0}' the following corrective script was executed: '{1}'",
        //        collectorEntry.Name, error ? collectorEntry.CorrectiveScriptOnErrorPath : collectorEntry.CorrectiveScriptOnWarningPath));
        //}
        private void LogCorrectiveScriptMinRepeatTimeBlockedEvent(CollectorHost collectorEntry, string message)
        {
            LoggingCorrectiveScriptRunEvent(string.Format("Collector host '{0}': {1}", collectorEntry.Name, message));
        }
        #endregion

        #region GetCollectorLists
        public List<CollectorHost> GetRootCollectorHosts()
        {
            return (from c in CollectorHosts
                    where c.ParentCollectorId.Length == 0
                    select c).ToList();
        }
        public List<CollectorHost> GetChildCollectorHosts(CollectorHost parentCE)
        {
            return (from c in CollectorHosts
                    where c.ParentCollectorId == parentCE.UniqueId
                    select c).ToList();
        }
        public List<CollectorHost> GetAllChildCollectorHosts(CollectorHost parentCE)
        {
            List<CollectorHost> list = new List<CollectorHost>();
            List<CollectorHost> listChildren = new List<CollectorHost>();
            listChildren = GetChildCollectorHosts(parentCE);
            foreach (CollectorHost child in listChildren)
            {
                list.Add(child);
                list.AddRange(GetAllChildCollectorHosts(child));
            }
            return list;
        }
        #endregion

        #region Sorting/Swapping
        internal void SwapCollectorEntries(CollectorHost c1, CollectorHost c2)
        {
            int index1 = CollectorHosts.FindIndex(c => c.UniqueId == c1.UniqueId);
            int index2 = CollectorHosts.FindIndex(c => c.UniqueId == c2.UniqueId);

            if (index1 < index2)
            {
                int tmp = index1;
                index1 = index2;
                index2 = tmp;
            }

            if (index1 > -1 && index2 > -1 && index1 != index2)
            {

                CollectorHosts.RemoveAt(index1);
                CollectorHosts.RemoveAt(index2);
                CollectorHosts.Insert(index2, c1);
                CollectorHosts.Insert(index1, c2);
            }
        }
        internal void SwapNotifierEntries(NotifierHost n1, NotifierHost n2)
        {
            int index1 = NotifierHosts.FindIndex(c => c.Name == n1.Name);
            int index2 = NotifierHosts.FindIndex(c => c.Name == n2.Name);

            if (index1 > -1 && index2 > -1 && index1 != index2)
            {
                NotifierHosts.RemoveAt(index1);
                NotifierHosts.RemoveAt(index2);
                NotifierHosts.Insert(index2, n1);
                NotifierHosts.Insert(index1, n2);
            }
        }
        #endregion

        #region Closing
        public void CloseMonitorPack()
        {
            ClosePerformanceCounters();
            LoggingMonitorPackClosed();
        }
        #endregion
    }
}
