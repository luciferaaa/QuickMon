﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml;
using System.Threading;
using System.Diagnostics;
using System.Collections;
using System.Reflection;
using System.Threading.Tasks;

namespace QuickMon
{
    #region Delegates
    public delegate void RaiseCollectorCalledDelegate(CollectorEntry collector);
    public delegate void RaiseCurrentStateDelegate(CollectorEntry collector, MonitorStates currentState);
    public delegate void RaiseNotifierErrorDelegare(NotifierEntry notifier, string errorMessage);
    public delegate void RaiseCollectorErrorDelegare(CollectorEntry collector, string errorMessage);
    public delegate void RaiseMonitorPackErrorDelegate(string errorMessage);
    public delegate void StateChangedDelegate(AlertLevel alertLevel, string collectorType, string category, MonitorStates oldState, MonitorStates currentState, CollectorMessage details);
    public delegate void CollectorExecutionTimeDelegate(CollectorEntry collector, long msTime);
    public delegate void MonitorPackPathChangedDelegate(string newMonitorPackPath); 
    #endregion

	public class MonitorPack
	{
		public MonitorPack()
		{
			Collectors = new List<CollectorEntry>();
			Notifiers = new List<NotifierEntry>();
			Enabled = true;
			PollingFreq = 1000;
			IsPolling = false;
			LastGlobalState = MonitorStates.NotAvailable;
			DefaultViewerNotifier = null;
			AgentRegistrations = new List<AgentRegistration>();
			agentsAssemblyPath = System.IO.Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location); //set as default
            BusyPolling = false;
		}

		private string quickMonPCCategory = "QuickMon";

		#region Events
		public event RaiseNotifierErrorDelegare RaiseNotifierError;
		private void RaiseRaiseNotifierError(NotifierEntry notifier, string errorMessage)
		{
			try
			{
				if (RaiseNotifierError != null)
					RaiseNotifierError(notifier, errorMessage);
			}
            catch (Exception ex)
            {
                System.Diagnostics.Trace.WriteLine(string.Format("Error in RaiseRaiseNotifierError: {0}", ex.Message));
            }
		}
		public event RaiseCollectorErrorDelegare RaiseCollectorError;
        private void RaiseRaiseCollectorError(CollectorEntry collector, string errorMessage)
        {
            try
            {
                if (RaiseCollectorError != null)
                {
                    RaiseCollectorError(collector, errorMessage);
                }
            }
            catch (Exception ex)
            {
                System.Diagnostics.Trace.WriteLine(string.Format("Error in RaiseRaiseCollectorError: {0}", ex.Message));
            }
        }
		public event RaiseMonitorPackErrorDelegate RaiseMonitorPackError;
        private void RaiseRaiseMonitorPackError(string errorMessage)
        {
            try
            {
                if (RaiseMonitorPackError != null)
                    RaiseMonitorPackError(errorMessage);
            }
            catch (Exception ex)
            {
                System.Diagnostics.Trace.WriteLine(string.Format("Error in RaiseRaiseMonitorPackError: {0}", ex.Message));
            }
        }
		public event RaiseCurrentStateDelegate RaiseCurrentState;
		private void RaiseRaiseCurrentStateDelegate(CollectorEntry collector, MonitorStates currentState)
		{
			try
			{
				if (RaiseCurrentState != null)
				{
					RaiseCurrentState(collector, currentState);
				}
			}
			catch (Exception ex)
			{
				System.Diagnostics.Trace.WriteLine(string.Format("Error in RaiseRaiseCurrentStateDelegate: {0}", ex.Message));
			}
		}
		public event StateChangedDelegate StateChanged;
        private void RaiseStateChanged(AlertLevel alertLevel, string collectorType, string collectorName, MonitorStates oldState, MonitorStates currentState, CollectorMessage details)
        {
            try
            {
                if (StateChanged != null)
                    StateChanged(alertLevel, collectorType, collectorName, oldState, currentState, details);
            }
            catch (Exception ex)
            {
                System.Diagnostics.Trace.WriteLine(string.Format("Error in RaiseStateChanged: {0}", ex.Message));
            }
        }
		public event RaiseCollectorCalledDelegate CollectorCalled;
        private void RaiseCollectorCalled(CollectorEntry collector)
        {
            try
            {
                if (CollectorCalled != null)
                {
                    CollectorCalled(collector);
                }
            }
            catch (Exception ex)
            {
                System.Diagnostics.Trace.WriteLine(string.Format("Error in RaiseCollectorCalled: {0}", ex.Message));
            }
        }
		public event CollectorExecutionTimeDelegate CollectorExecutionTimeEvent;
		private void RaiseCollectorExecutionTime(CollectorEntry collector, long msTime)
		{
            try
            {
                if (CollectorExecutionTimeEvent != null)
                {
                    CollectorExecutionTimeEvent(collector, msTime);
                }
            }
            catch (Exception ex)
            {
                System.Diagnostics.Trace.WriteLine(string.Format("Error in RaiseCollectorExecutionTime: {0}", ex.Message));
            }
		}
		public event RaiseCollectorCalledDelegate RunCollectorCorrectiveWarningScript;
        private void RaiseRunCollectorCorrectiveWarningScript(CollectorEntry collector)
        {
            try
            {
                if (RunCorrectiveScripts &&
                    RunCollectorCorrectiveWarningScript != null &&
                    collector != null &&
                    !collector.CorrectiveScriptDisabled &&
                    collector.CorrectiveScriptOnWarningPath.Length > 0)
                {
                    RunCollectorCorrectiveWarningScript(collector);
                    LogCorrectiveScriptAction(collector, false);
                }
            }
            catch (Exception ex)
            {
                System.Diagnostics.Trace.WriteLine(string.Format("Error in RaiseRunCollectorCorrectiveWarningScript: {0}", ex.Message));
            }
        }
		public event RaiseCollectorCalledDelegate RunCollectorCorrectiveErrorScript;
        private void RaiseRunCollectorCorrectiveErrorScript(CollectorEntry collector)
        {
            try
            {
                if (RunCorrectiveScripts &&
                    RunCollectorCorrectiveErrorScript != null &&
                    collector != null &&
                    !collector.CorrectiveScriptDisabled &&
                    collector.CorrectiveScriptOnErrorPath.Length > 0)
                {
                    RunCollectorCorrectiveErrorScript(collector);
                    LogCorrectiveScriptAction(collector, true);
                }
            }
            catch (Exception ex)
            {
                System.Diagnostics.Trace.WriteLine(string.Format("Error in RaiseRunCollectorCorrectiveErrorScript: {0}", ex.Message));
            }
        }
        public event MonitorPackPathChangedDelegate MonitorPackPathChanged;
        private void RaiseMonitorPackPathChanged(string newMonitorPackPath)
        {
            try 
            {
                if (MonitorPackPathChanged != null)
                    MonitorPackPathChanged(newMonitorPackPath);
            }
            catch (Exception ex)
            {
                System.Diagnostics.Trace.WriteLine(string.Format("Error in RaiseRunCollectorCorrectiveErrorScript: {0}", ex.Message));
            }
        }
		#endregion

		#region Properties
		public string Name { get; set; }
		public bool Enabled { get; set; }
        public bool BusyPolling { get; private set; }
        public bool AbortPolling { get; set; }
		private string agentsAssemblyPath = "";
		public string AgentsAssemblyPath
		{
			get { return agentsAssemblyPath; }
			set
			{
				if (value == null || value == "")
				{
					value = System.IO.Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location); //try to look in local directory
				}
				if (System.IO.Directory.Exists(value))
				{
					agentsAssemblyPath = value;
					LoadAgentAssemblies();
				}
			}
		}
		public string AgentLoadingErrors { get; set; }
		public bool RunCorrectiveScripts { get; set; }
		
		public List<AgentRegistration> AgentRegistrations { get; set; }
		public List<CollectorEntry> Collectors { get; private set; }
		public List<NotifierEntry> Notifiers { get; private set; }
		public NotifierEntry DefaultViewerNotifier { get; set; }
		public MonitorStates LastGlobalState { get; set; }
		public string MonitorPackPath { get; set; }
		private int concurrencyLevel = 1;
		public int ConcurrencyLevel 
		{
			get { return concurrencyLevel; }
			set { concurrencyLevel = value; } 
		}

		#region Async related properties
		/// <summary>
		/// Is background polling active
		/// </summary>
		public bool IsPolling { get; set; }
		/// <summary>
		/// Polling frequency for background operations. Measured in milliseconds. Default 1 Second
		/// </summary>
		public int PollingFreq { get; set; }
		#endregion
		#endregion

		#region Performance Counter Vars
		private PerformanceCounter collectorErrorStatePerSec = null;
		private PerformanceCounter collectorWarningStatePerSec = null;
		private PerformanceCounter collectorInfoStatePerSec = null;
		private PerformanceCounter notifierAlertSendPerSec = null;
		private PerformanceCounter collectorsQueriedPerSecond = null;
		private PerformanceCounter notifiersCalledPerSecond = null;
		private PerformanceCounter collectorsQueryTime = null;
		private PerformanceCounter notifiersSendTime = null;
		#endregion

		#region Refreshing states
		public MonitorStates RefreshStates()
		{
            AbortPolling = false;
            BusyPolling = true;
			MonitorStates globalState = MonitorStates.Good;
			Stopwatch sw = new Stopwatch();
			sw.Start();
			//First get collectors with no dependancies
			List<CollectorEntry> noDependantCollectors = (from c in Collectors
														  where c.ParentCollectorId.Length == 0
														  select c).ToList();

			//Using .Net 4 Parralel processing extensions
			int threads = concurrencyLevel;
#if DEBUG
			//threads = 5;
#endif
			if (concurrencyLevel > 1)
			{
				ParallelOptions po = new ParallelOptions()
				{
					MaxDegreeOfParallelism = threads
				};
				ParallelLoopResult parResult = Parallel.ForEach(noDependantCollectors, po, collector => RefreshCollectorState(collector));
				if (!parResult.IsCompleted)
				{
					SendNotifierAlert(AlertLevel.Error, DetailLevel.Summary, "N/A", "GlobalState", MonitorStates.NotAvailable, MonitorStates.Error, new CollectorMessage("Error querying collectors in parralel"));
				}
			}
			else //use old single threaded way
			{
				//Refresh states
				foreach (CollectorEntry collector in noDependantCollectors)
				{
					RefreshCollectorState(collector);
				}
			}
			sw.Stop();
			PCSetCollectorsQueryTime(sw.ElapsedMilliseconds);
#if DEBUG
			Trace.WriteLine(string.Format("RefreshStates - Global time: {0}ms", sw.ElapsedMilliseconds));
#endif

			//set global state
			//All disabled
			if (Collectors.Count == Collectors.Count(c => c.LastMonitorState == MonitorStates.Disabled || c.LastMonitorState == MonitorStates.Folder))
				globalState = MonitorStates.Disabled;
			//All NotAvailable
			else if (Collectors.Count == Collectors.Count(c => c.LastMonitorState == MonitorStates.NotAvailable || c.LastMonitorState == MonitorStates.Folder))
				globalState = MonitorStates.NotAvailable;
			//All good
			else if (Collectors.Count == Collectors.Count(c => c.LastMonitorState == MonitorStates.Good || c.LastMonitorState == MonitorStates.Disabled || c.LastMonitorState == MonitorStates.NotAvailable || c.LastMonitorState == MonitorStates.Folder))
				globalState = MonitorStates.Good;
			//Error state
			else if (Collectors.Count == Collectors.Count(c => c.LastMonitorState == MonitorStates.Error || c.LastMonitorState == MonitorStates.ConfigurationError || c.LastMonitorState == MonitorStates.Disabled || c.LastMonitorState == MonitorStates.Folder))
				globalState = MonitorStates.Error;
			else
				globalState = MonitorStates.Warning;

			AlertLevel globalAlertLevel = AlertLevel.Info;
			if (globalState == MonitorStates.Error || globalState == MonitorStates.ConfigurationError)
				globalAlertLevel = AlertLevel.Error;
			else if (globalState == MonitorStates.Warning)
				globalAlertLevel = AlertLevel.Warning;

			sw.Restart();
			SendNotifierAlert(globalAlertLevel, DetailLevel.Summary, "N/A", "GlobalState", MonitorStates.NotAvailable, MonitorStates.NotAvailable, new CollectorMessage());
			sw.Stop();
			PCSetNotifiersSendTime(sw.ElapsedMilliseconds);
#if DEBUG
			Trace.WriteLine(string.Format("RefreshStates - Global notification time: {0}ms", sw.ElapsedMilliseconds));
#endif

            BusyPolling = false;
			return globalState;
		}
		private void RefreshCollectorState(CollectorEntry collector)
		{
            if (!AbortPolling)
            {
                RaiseCollectorCalled(collector);
                MonitorStates currentState = MonitorStates.NotAvailable;
                Stopwatch sw = new Stopwatch();

                #region Getting current state
                sw.Start();
                try
                {
                    System.Diagnostics.Trace.WriteLine(string.Format("Starting: {0}", collector.Name));
                    currentState = collector.GetCurrentState();
                    if (currentState == MonitorStates.Good ||
                        currentState == MonitorStates.Warning ||
                        currentState == MonitorStates.Error)
                    {
                        PCRaiseCollectorsQueried();
                    }
                }
                catch (Exception ex)
                {
                    RaiseRaiseCollectorError(collector, ex.Message);
                    currentState = MonitorStates.Error;
                    collector.LastMonitorDetails = collector.Collector.LastDetailMsg;
                    System.Diagnostics.Trace.WriteLine(string.Format("Error: {0} - {1}", collector.Name, ex.Message));
                }
                finally
                {
                    System.Diagnostics.Trace.WriteLine(string.Format("Ending: {0}", collector.Name));
                }

                sw.Stop();
                RaiseCollectorExecutionTime(collector, sw.ElapsedMilliseconds);
#if DEBUG
                Trace.WriteLine(string.Format("RefreshCollectorState - {0}: {1}ms", collector.Name, sw.ElapsedMilliseconds));
#endif
                #endregion

                #region Raising alerts or events
                if (!collector.IsFolder)
                {
                    RaiseRaiseCurrentStateDelegate(collector, currentState);

                    AlertLevel alertLevel = AlertLevel.Debug;
                    if (currentState == MonitorStates.Good)
                    {
                        alertLevel = AlertLevel.Info;
                        PCRaiseCollectorSuccessState();
                    }
                    else if (currentState == MonitorStates.Disabled || currentState == MonitorStates.NotAvailable)
                    {
                        alertLevel = AlertLevel.Info;
                    }
                    else if (currentState == MonitorStates.Warning)
                    {
                        alertLevel = AlertLevel.Warning;
                        PCRaiseCollectorWarningState();
                    }
                    else if (currentState == MonitorStates.Error || currentState == MonitorStates.ConfigurationError)
                    {
                        alertLevel = AlertLevel.Error;
                        PCRaiseCollectorErrorState();
                    }

                    //Check if alert should be raised now
                    if (collector.RaiseAlert())
                    {
                        SendNotifierAlert(alertLevel, DetailLevel.Detail, collector.CollectorRegistrationName, collector.Name, collector.LastMonitorState, currentState,
                                collector.LastMonitorDetails);
                        PCRaiseNotifierAlertSend();
                    }
                    else //otherwise raise only Debug info
                    {
                        RaiseStateChanged(AlertLevel.Debug, collector.CollectorRegistrationName, collector.Name, collector.LastMonitorState, currentState, new CollectorMessage());
                        SendNotifierAlert(AlertLevel.Debug, DetailLevel.Detail, collector.CollectorRegistrationName, collector.Name, collector.LastMonitorState, currentState, collector.LastMonitorDetails); // new CollectorMessage());
                    }
                    //Did the state changed?
                    if (collector.StateChanged())
                    {
                        RaiseStateChanged(alertLevel, collector.CollectorRegistrationName, collector.Name, collector.LastMonitorState, currentState,
                                    collector.LastMonitorDetails);
                    }
                    collector.LastMonitorState = currentState;

                    try
                    {
                        //run corrective script only after alert has been raised
                        if (RunCorrectiveScripts)
                        {
                            if (currentState == MonitorStates.Warning)
                                RaiseRunCollectorCorrectiveWarningScript(collector);
                            else if (currentState == MonitorStates.Error)
                                RaiseRunCollectorCorrectiveErrorScript(collector);
                        }
                    }
                    catch (Exception ex)
                    {
                        RaiseRaiseCollectorError(collector, ex.Message);
                    }
                }
                #endregion

                #region Set or check dependants
                if (currentState == MonitorStates.Error)
                {
                    SetChildCollectorStates(collector, MonitorStates.Error);
                }
                else if ((currentState == MonitorStates.Warning && !collector.CollectOnParentWarning) || currentState == MonitorStates.NotAvailable)
                {
                    SetChildCollectorStates(collector, MonitorStates.NotAvailable);
                }
                else if (currentState == MonitorStates.Disabled || currentState == MonitorStates.ConfigurationError || (collector.IsFolder && !collector.Enabled))
                {
                    SetChildCollectorStates(collector, MonitorStates.Disabled);
                }
                else
                {
                    try
                    {
                        foreach (CollectorEntry childCollector in (from c in Collectors
                                                                   where c.ParentCollectorId == collector.UniqueId
                                                                   select c))
                        {
                            RefreshCollectorState(childCollector);
                        }
                    }
                    catch (Exception ex)
                    {
                        if (!ex.Message.Contains("Collection was modified; enumeration operation may not execute"))
                            throw;
                    }
                }
                #endregion
            }
		}
        private void SetChildCollectorStates(CollectorEntry collector, MonitorStates childState)
        {
            try
            {
                foreach (CollectorEntry childCollector in (from c in Collectors
                                                           where c.ParentCollectorId == collector.UniqueId
                                                           select c))
                {
                    RaiseRaiseCurrentStateDelegate(childCollector, childState);
                    childCollector.LastMonitorState = childState;
                    SetChildCollectorStates(childCollector, childState);
                }
            }
            catch (Exception ex)
            {
                if (!ex.Message.Contains("Collection was modified; enumeration operation may not execute"))
                    throw;
            }
        }
		private void SendNotifierAlert(AlertLevel alertLevel, DetailLevel detailLevel,
				string collectorType, string collectorName, MonitorStates oldState, MonitorStates currentState, CollectorMessage details)
		{
			Stopwatch sw = new Stopwatch();
			sw.Start();
			foreach (NotifierEntry notifierEntry in (from n in Notifiers
													 where n.Enabled && (int)n.AlertLevel <= (int)alertLevel &&
														(detailLevel == DetailLevel.All || detailLevel == n.DetailLevel) &&
														(n.AlertForCollectors.Count == 0 || n.AlertForCollectors.Contains(collectorName))
													 select n))
			{
				try
				{
					PCRaiseNotifiersCalled();
					notifierEntry.Notifier.RecordMessage(alertLevel, collectorType, collectorName, oldState, currentState, details);
				}
				catch (Exception ex)
				{
					RaiseRaiseNotifierError(notifierEntry, ex.ToString());
				}
			}
			sw.Stop();
			PCSetNotifiersSendTime(sw.ElapsedMilliseconds);
		}
        private void LogCorrectiveScriptAction(CollectorEntry collector, bool error)
        {
            CollectorMessage details = new CollectorMessage();
            details.PlainText = string.Format("Due to an alert raised on the collector '{0}' the following corrective script was executed: '{1}'",
                collector.Name, error ? collector.CorrectiveScriptOnErrorPath : collector.CorrectiveScriptOnWarningPath);
            //details.LastValue = collector.LastMonitorDetails.LastValue;
            Stopwatch sw = new Stopwatch();
            sw.Start();
            foreach (NotifierEntry notifierEntry in (from n in Notifiers
                                                     where n.Enabled &&
                                                        (
                                                            ((int)n.AlertLevel <= (int)AlertLevel.Warning && !error) ||
                                                            ((int)n.AlertLevel <= (int)AlertLevel.Error && error)
                                                        )&&
                                                        (n.AlertForCollectors.Count == 0 || n.AlertForCollectors.Contains(collector.Name))
                                                     select n))
            {
                try
                {
                    PCRaiseNotifiersCalled();
                    notifierEntry.Notifier.RecordMessage(error ? AlertLevel.Error : AlertLevel.Warning, collector.CollectorRegistrationName, collector.Name, collector.LastMonitorState, collector.CurrentState, details);
                }
                catch (Exception ex)
                {
                    RaiseRaiseNotifierError(notifierEntry, ex.ToString());
                }
            }
            sw.Stop();
            PCSetNotifiersSendTime(sw.ElapsedMilliseconds);
        }
		#endregion

		#region Async refreshing 
		public void StartPolling()
		{
			IsPolling = true;
			ThreadPool.QueueUserWorkItem(new WaitCallback(BackgroundPolling));
		}
		private void BackgroundPolling(object o)
		{
			while (IsPolling)
			{
				try
				{
					LastGlobalState = RefreshStates();
				}
				catch(Exception ex)
				{
					RaiseRaiseMonitorPackError(ex.Message);
				}
				BackgroundWaitIsPolling(PollingFreq);
			}
			ClosePerformanceCounters();
		}
		private void BackgroundWaitIsPolling(int nextWaitInterval)
		{
			int waitTimeRemaining;
			int decrementBy = 2000;
			if (IsPolling)
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
						while (IsPolling && (waitTimeRemaining > 0))
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

		#region Loading and saving configuration
		public void ApplyCollectorConfig(CollectorEntry collectorEntry)
		{
			if (collectorEntry == null)
				return;
			AgentRegistration currentCollector = null;
			if (collectorEntry.IsFolder)
			{
				collectorEntry.Collector = null;
			}
			else
			{
				//first clear/release any existing references
				if (collectorEntry.Collector != null)
					collectorEntry.Collector = null;

				if (AgentRegistrations != null)
					currentCollector = (from o in AgentRegistrations
										where o.IsCollector && o.Name == collectorEntry.CollectorRegistrationName
										select o).FirstOrDefault();
				if (currentCollector != null)
				{
					collectorEntry.Collector = CollectorEntry.CreateCollectorEntry(currentCollector.AssemblyPath, currentCollector.ClassName);
					XmlDocument configDoc = new XmlDocument();
					configDoc.LoadXml(collectorEntry.Configuration);
					try
					{
						collectorEntry.Collector.ReadConfiguration(configDoc);
					}
					catch (Exception ex)
					{
						collectorEntry.LastMonitorState = MonitorStates.ConfigurationError;
						collectorEntry.Enabled = false;
						collectorEntry.LastMonitorDetails.PlainText = ex.Message;
					}
				}
				else
				{
					collectorEntry.LastMonitorState = MonitorStates.ConfigurationError;
					collectorEntry.Enabled = false;
					collectorEntry.LastMonitorDetails.PlainText = string.Format("Collector '{0}' cannot be loaded as the type '{1}' is not registered!", collectorEntry.Name, collectorEntry.CollectorRegistrationName);
					RaiseRaiseMonitorPackError(string.Format("Collector '{0}' cannot be loaded as the type '{1}' is not registered!", collectorEntry.Name, collectorEntry.CollectorRegistrationName));
				}
			}
		}
		/// <summary>
		/// Loading QuickMon monitor pack file
		/// </summary>
		/// <param name="configurationFile">Serialzed monitor pack file</param>
		public void Load(string configurationFile)
		{
			XmlDocument configurationXml = new XmlDocument();
			configurationXml.LoadXml(System.IO.File.ReadAllText(configurationFile, Encoding.UTF8));
			XmlElement root = configurationXml.DocumentElement;
			Name = root.Attributes.GetNamedItem("name").Value;
			Enabled = bool.Parse(root.Attributes.GetNamedItem("enabled").Value);
			AgentsAssemblyPath = root.ReadXmlElementAttr("agentRegistrationPath");

            string defaultViewerNotifierName = root.ReadXmlElementAttr("defaultViewerNotifier");
			RunCorrectiveScripts = bool.Parse(root.ReadXmlElementAttr("runCorrectiveScripts", "false"));
			foreach (XmlElement xmlCollectorEntry in root.SelectNodes("collectorEntries/collectorEntry"))
			{
				CollectorEntry newCollectorEntry = CollectorEntry.FromConfig(xmlCollectorEntry);
				ApplyCollectorConfig(newCollectorEntry);        
				Collectors.Add(newCollectorEntry);
			}
			foreach (XmlElement xmlNotifierEntry in root.SelectNodes("notifierEntries/notifierEntry"))
			{
				NotifierEntry newNotifierEntry = NotifierEntry.FromConfig(xmlNotifierEntry);
				AgentRegistration currentNotifier = null;
				if (AgentRegistrations != null)
					currentNotifier = (from o in AgentRegistrations
									   where o.IsNotifier && o.Name == newNotifierEntry.NotifierRegistrationName
									   select o).FirstOrDefault();
				if (currentNotifier != null)
				{
					newNotifierEntry.Notifier = NotifierEntry.CreateNotifierEntry(currentNotifier.AssemblyPath, currentNotifier.ClassName);
					XmlDocument configDoc = new XmlDocument();
					configDoc.LoadXml(newNotifierEntry.Configuration);
					try
					{
						newNotifierEntry.Notifier.ReadConfiguration(configDoc);
					}
					catch // (Exception ex)
					{
						newNotifierEntry.Enabled = false;
					}
				}
				else
				{
					newNotifierEntry.Enabled = false;
				}
				Notifiers.Add(newNotifierEntry);
				if (newNotifierEntry.Name.ToUpper() == defaultViewerNotifierName.ToUpper())
					DefaultViewerNotifier = newNotifierEntry;
			}
			MonitorPackPath = configurationFile;
            RaiseMonitorPackPathChanged(MonitorPackPath);
			if (Properties.Settings.Default.recentMonitorPacks == null)
				Properties.Settings.Default.recentMonitorPacks = new System.Collections.Specialized.StringCollection();
			if (!Properties.Settings.Default.recentMonitorPacks.Contains(configurationFile))
			{
				Properties.Settings.Default.recentMonitorPacks.Add(configurationFile);
				Properties.Settings.Default.Save();
			}
			InitializeGlobalPerformanceCounters();
		}
		public bool Save()
		{
			if (MonitorPackPath != null && MonitorPackPath.Length > 0 && System.IO.Directory.Exists(System.IO.Path.GetDirectoryName(MonitorPackPath)))
			{
				Save(MonitorPackPath);
				return true;
			}
			else
				return false;
		}
		/// <summary>
		/// Saving QuickMon monitor pack file
		/// </summary>
		/// <param name="configurationFile"></param>
		public void Save(string configurationFile)
		{
			string defaultViewerNotifier = "";
			if (DefaultViewerNotifier != null)
				defaultViewerNotifier = DefaultViewerNotifier.Name;
			string outputXml = string.Format(Properties.Resources.MonitorPackXml,
				Name, Enabled, AgentsAssemblyPath, defaultViewerNotifier,
				RunCorrectiveScripts,
				GetConfigForCollectors(),
				GetConfigForNotifiers());
			XmlDocument outputDoc = new XmlDocument();
			outputDoc.LoadXml(outputXml);
			outputDoc.PreserveWhitespace = false;
			outputDoc.Normalize();
			outputDoc.Save(configurationFile);

			MonitorPackPath = configurationFile;
            RaiseMonitorPackPathChanged(MonitorPackPath);
			if (Properties.Settings.Default.recentMonitorPacks == null)
				Properties.Settings.Default.recentMonitorPacks = new System.Collections.Specialized.StringCollection();
			if (!Properties.Settings.Default.recentMonitorPacks.Contains(configurationFile))
			{
				Properties.Settings.Default.recentMonitorPacks.Add(configurationFile);
				Properties.Settings.Default.Save();
			}
		}

		private string GetConfigForNotifiers()
		{
			StringBuilder sb = new StringBuilder();
			foreach (NotifierEntry notifierEntry in Notifiers)
			{
				sb.AppendLine(notifierEntry.ToConfig());
			}
			return sb.ToString();
		}
		private string GetConfigForCollectors()
		{
			StringBuilder sb = new StringBuilder();
			foreach (CollectorEntry collectorEntry in Collectors)
			{
				sb.AppendLine(collectorEntry.ToConfig());
			}
			return sb.ToString();
		} 
		#endregion        

		#region Sorting/Swapping
		internal void SwapCollectorEntries(CollectorEntry c1, CollectorEntry c2)
		{
			int index1 = Collectors.FindIndex(c => c.UniqueId == c1.UniqueId);
			int index2 = Collectors.FindIndex(c => c.UniqueId == c2.UniqueId);

			if (index1 < index2)
			{
				int tmp = index1;
				index1 = index2;
				index2 = tmp;
			}

			if (index1 > -1 && index2 > -1 && index1 != index2)
			{
				
				Collectors.RemoveAt(index1);
				Collectors.RemoveAt(index2);
				Collectors.Insert(index2, c1);
				Collectors.Insert(index1, c2);
			}
		}
		internal void SwapNotifierEntries(NotifierEntry n1, NotifierEntry n2)
		{
			int index1 = Notifiers.FindIndex(c => c.Name == n1.Name);
			int index2 = Notifiers.FindIndex(c => c.Name == n2.Name);

			if (index1 > -1 && index2 > -1 && index1 != index2)
			{
				Notifiers.RemoveAt(index1);
				Notifiers.RemoveAt(index2);
				Notifiers.Insert(index2, n1);
				Notifiers.Insert(index1, n2);
			}
		} 
		#endregion

		#region Dynamically loading QuickMon Agent assemblies
		private void LoadAgentAssemblies()
		{
			StringBuilder sbExceptions = new StringBuilder();
			AgentLoadingErrors = "";
			try
			{
				AgentRegistrations.Clear();
                string agentAssemblyPathLocal = "";
#if DEBUG
                agentAssemblyPathLocal = System.IO.Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location);
#else
			    agentAssemblyPathLocal = agentsAssemblyPath;
#endif
                foreach (string assemblyPath in System.IO.Directory.GetFiles(agentAssemblyPathLocal, "*.dll"))
				{
					try
					{
						foreach (string className in LoadQuickMonClasses(assemblyPath))
						{
							AgentRegistration ar = new AgentRegistration();
							ar.Name = className.Replace("QuickMon.", "");
							ar.AssemblyPath = assemblyPath;
							ar.ClassName = className;
							ar.IsCollector = IsCollectorClass(assemblyPath, className);
							ar.IsNotifier = !ar.IsCollector;

							AgentRegistrations.Add(ar);
						}
					}
					catch (System.Reflection.ReflectionTypeLoadException rex)
					{
						foreach (Exception lex in rex.LoaderExceptions)
						{
							sbExceptions.AppendLine(string.Format("Error in assembly '{0}' - {1}", assemblyPath, lex.Message));
						}
					}
					catch (Exception innerEx)
					{
						throw new Exception(string.Format("Error loading {0}.\r\n{1}", assemblyPath, innerEx.Message));
					}
				}
			}
			catch (Exception ex)
			{
				throw new Exception(string.Format("An error occured loading QuickMon agents from {0}.\r\n{1}", agentsAssemblyPath, ex.Message));
			}
			if (sbExceptions.Length > 0)
			{
				AgentLoadingErrors = string.Format("There were errors loading some/all agent assemblies\r\n{0}", sbExceptions.ToString());
			}
		}
		private bool IsCollectorClass(string assemblyFilePath, string className)
		{
			Assembly quickAsshehe = Assembly.LoadFile(assemblyFilePath);
			Type[] types = quickAsshehe.GetTypes();
			foreach (Type type in types)
			{
				if (type.FullName == className)
				{
					foreach (Type interfaceType in type.GetInterfaces())
					{
						if (interfaceType.FullName == "QuickMon.ICollector")
							return true;
					}
					return false;
				}
			}
			return false;
		}
		private IEnumerable LoadQuickMonClasses(string assemblyFilePath)
		{
			Assembly quickAsshehe = Assembly.LoadFile(assemblyFilePath);
			Type[] types = quickAsshehe.GetTypes();
			foreach (Type type in types)
			{
				if (!type.IsInterface && !type.IsAbstract)
				{
					foreach (Type interfaceType in type.GetInterfaces())
					{
						if (interfaceType.FullName == "QuickMon.IAgent")
							yield return type.FullName;
					}
				}
			}
		} 
		#endregion

        #region GetCollectorLists
        public List<CollectorEntry> GetRootCollectors()
        {
            return (from c in Collectors
                    where c.ParentCollectorId.Length == 0
                    select c).ToList();
        }
        public List<CollectorEntry> GetChildCollectors(CollectorEntry parentCE)
        {
            return (from c in Collectors
                    where c.ParentCollectorId == parentCE.UniqueId
                    select c).ToList();
        }
        public List<CollectorEntry> GetAllChildCollectors(CollectorEntry parentCE)
        {
            List<CollectorEntry> list = new List<CollectorEntry>();
            List<CollectorEntry> listChildren = new List<CollectorEntry>();
            listChildren = GetChildCollectors(parentCE);
            foreach (CollectorEntry child in listChildren)
            {
                list.Add(child);
                list.AddRange(GetAllChildCollectors(child));
            }
            return list;
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
						new CounterCreationData("Notifier alerts send/Sec", "Notifier alerts send per second", PerformanceCounterType.RateOfCountsPerSecond32),
						new CounterCreationData("Collectors queried/Sec", "Number of Collectors queried per second", PerformanceCounterType.RateOfCountsPerSecond32),
						new CounterCreationData("Notifiers called/Sec", "Number of Notifiers called per second", PerformanceCounterType.RateOfCountsPerSecond32),
						new CounterCreationData("Collectors query time", "Collector total query time (ms)", PerformanceCounterType.NumberOfItems32),
						new CounterCreationData("Notifiers send time", "Notifiers total send time (ms)", PerformanceCounterType.NumberOfItems32)
					};

				if (PerformanceCounterCategory.Exists(quickMonPCCategory))
				{
					PerformanceCounterCategory pcC = new PerformanceCounterCategory(quickMonPCCategory);
					if (pcC.GetCounters().Length != quickMonCreationData.Length)
					{
						PerformanceCounterCategory.Delete(quickMonPCCategory);
					}
				}

				if (!PerformanceCounterCategory.Exists(quickMonPCCategory))
				{
					PerformanceCounterCategory.Create(quickMonPCCategory, "QuickMon General Counters", PerformanceCounterCategoryType.SingleInstance, new CounterCreationDataCollection(quickMonCreationData));
				}
				try
				{
					collectorErrorStatePerSec = InitializePerfCounterInstance(quickMonPCCategory, "Collector error states/Sec");
					collectorWarningStatePerSec = InitializePerfCounterInstance(quickMonPCCategory, "Collector warning states/Sec");
					collectorInfoStatePerSec = InitializePerfCounterInstance(quickMonPCCategory, "Collector success states/Sec");
					notifierAlertSendPerSec = InitializePerfCounterInstance(quickMonPCCategory, "Notifier alerts send/Sec");
					collectorsQueriedPerSecond = InitializePerfCounterInstance(quickMonPCCategory, "Collectors queried/Sec");
					notifiersCalledPerSecond = InitializePerfCounterInstance(quickMonPCCategory, "Notifiers called/Sec");
					collectorsQueryTime = InitializePerfCounterInstance(quickMonPCCategory, "Collectors query time");
					notifiersSendTime = InitializePerfCounterInstance(quickMonPCCategory, "Notifiers send time");
				}
				catch (Exception ex)
				{
					RaiseRaiseMonitorPackError(string.Format("Initialize global performance counters error!: {0}", ex.Message));
				}
			}
			catch (Exception ex)
			{
				RaiseRaiseMonitorPackError(string.Format("Create global performance counters category error!: {0}", ex.Message));
			}            
		}
		public void ClosePerformanceCounters()
		{
			PCSetCollectorsQueryTime(0);
		}
		private PerformanceCounter InitializePerfCounterInstance(string categoryName, string counterName)
		{
			PerformanceCounter counter = new PerformanceCounter(categoryName, counterName, false);
			counter.BeginInit();
			counter.RawValue = 0;
			counter.EndInit();
			return counter;
		}
		private PerformanceCounter GetPerfCounterInstance(string categoryName, string counterName)
		{
			PerformanceCounter counter = new PerformanceCounter(categoryName, counterName, false);
			return counter;
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
		private void PCRaiseNotifierAlertSend()
		{
			IncrementCounter(notifierAlertSendPerSec, "Notifier alerts send per second");
		}
		private void PCRaiseCollectorsQueried()
		{
			IncrementCounter(collectorsQueriedPerSecond, "Collectors queried per second");
		}
		private void PCRaiseNotifiersCalled()
		{
			IncrementCounter(notifiersCalledPerSecond, "Notifiers called per second");
		}
		private void PCSetCollectorsQueryTime(long time)
		{
			SetCounterValue(collectorsQueryTime, time, "Collector total query time (ms)");
		}
		private void PCSetNotifiersSendTime(long time)
		{
			SetCounterValue(notifiersSendTime, time, "Notifiers total send time (ms)");
		}
		private void SetCounterValue(PerformanceCounter counter, long value, string description)
		{
			try
			{
				if (counter == null)
				{
					RaiseRaiseMonitorPackError("Performance counter not set up or installed! : " + description);
				}
				else
				{
					counter.RawValue = value;
				}
			}
			catch (Exception ex)
			{
				RaiseRaiseMonitorPackError(string.Format("Increment performance counter error! : {0}\r\n{1}", description, ex.ToString()));
			}
		}
		private void IncrementCounter(PerformanceCounter counter, string description)
		{
			try
			{
				if (counter == null)
				{
					RaiseRaiseMonitorPackError("Performance counter not set up or installed! : " + description);                    
				}
				else
				{
					counter.Increment();
				}
			}
			catch (Exception ex)
			{
				RaiseRaiseMonitorPackError(string.Format("Increment performance counter error! : {0}\r\n{1}", description, ex.ToString()));
			}
		}
		private void DecrementCounter(PerformanceCounter counter, string description)
		{
			try
			{
				if (counter == null)
				{
					RaiseRaiseMonitorPackError("Performance counter not set up or installed! : " + description);
				}
				else
				{
					if (counter.RawValue > 0)
						counter.Decrement();
				}
			}
			catch (Exception ex)
			{
				RaiseRaiseMonitorPackError(string.Format("Increment performance counter error! : {0}\r\n{1}", description, ex.ToString()));
			}
		}
		#endregion
	}
}
