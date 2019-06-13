using System;
using System.Collections.Generic;
using System.Linq;
using Constant;
using Interface;
using PanelBase;
using SetupBase;
using Manager = SetupBase.Manager;

namespace SetupLog
{
    public partial class Setup
    {
        private String SystemFilter
        {
            get
            {
                if (filterSystemLogComboBox.SelectedItem == null) return "";

                return (filterSystemLogComboBox.SelectedIndex == 0)
                    ? ""
                    : filterSystemLogComboBox.SelectedItem.ToString();
            }
        }

        private String SystemDescFilter
        {
            get
            {
                if (filterSystemDescComboBox.SelectedItem == null) return "";

                return (filterSystemDescComboBox.SelectedIndex == 0)
                    ? ""
                    : filterSystemDescComboBox.SelectedItem.ToString();
            }
        }

        private String UserFilter
        {
            get
            {
                if (filterUserLogComboBox.SelectedItem == null) return "";

                return (filterUserLogComboBox.SelectedIndex == 0)
                    ? ""
                    : filterUserLogComboBox.SelectedItem.ToString();
            }
        }

        private String UserDescFilter
        {
            get
            {
                if (filterUserDescComboBox.SelectedItem == null) return "";

                return (filterUserDescComboBox.SelectedIndex == 0)
                    ? ""
                    : filterUserDescComboBox.SelectedItem.ToString();
            }
        }

        private const UInt16 CountPerPage = 20;
        private void FilterLogWithCondition()
        {
            _logs.Clear();
            _logs.AddRange(_logResults);

            String filter = SystemFilter;
            if (filter != "")
            {
                foreach (Log log in _logResults)
                {
                    if (log.Type != LogType.Server) continue;

                    if (log.User != filter)
                        _logs.Remove(log);
                }
            }

            filter = SystemDescFilter;
            if (filter != "")
            {
                foreach (Log log in _logResults)
                {
                    if (log.Type != LogType.Server) continue;

                    switch (filter)
                    {
                        case "Delete Folder":
                        case "Delay: Delete Folder":
                        case "Unable to Delete Folder":
                        case "Delete Record Index Folder":
                        case "Delete Remove List Folder":
                            if (log.Description.IndexOf(filter) != 0)
                                _logs.Remove(log);
                            break;

                        default:
                            if (log.Description != filter)
                                _logs.Remove(log);
                            break;
                    }
                }
            }

            //---------------------------------------------------------------

            filter = UserFilter;
            if (filter != "")
            {
                foreach (Log log in _logResults)
                {
                    if (log.Type != LogType.Action) continue;

                    if (log.User != filter)
                        _logs.Remove(log);
                }
            }

            filter = UserDescFilter;
            if (filter != "")
            {
                foreach (Log log in _logResults)
                {
                    if (log.Type != LogType.Action) continue;

                    switch (filter)
                    {
                        case "Login":
                        case "Logout":
                            if (log.Description.IndexOf(filter) == -1)
                                _logs.Remove(log);
                            break;

                        default:
                            if (!log.Description.Contains(filter))
                                _logs.Remove(log);
                            break;
                    }
                }
            }

            _logIndex = 0;

            UpdateLabelLogFound();

            _page = 1;
            _pageSelector.SelectPage = _page;
            _pageSelector.Pages = Convert.ToInt32(Math.Ceiling(_logs.Count / (CountPerPage * 1.0)));
            _pageSelector.ShowPages();

            if (_logs.Count > 0)
            {
                ClearLog();
                UpdateLog();
                containerPanel.Visible = true;
            }
            else
            {
                containerPanel.Visible = false;
            }

            if (OnSelectionChange != null)
                OnSelectionChange(this, new EventArgs<String>(Manager.SelectionChangedXml(TitleName, TitleName,
                    "", (_logs.Count > 0) ? "SaveAs" : "")));
        }

        private void ClearLog()
        {
            if (logDoubleBufferPanel.Controls.Count == 0) return;

            logDoubleBufferPanel.Controls.Remove(titlePanel);

            foreach (LogPanel panel in logDoubleBufferPanel.Controls)
            {
                if (!StoredLog.Contains(panel))
                    StoredLog.Enqueue(panel);
            }
            logDoubleBufferPanel.Controls.Clear();
        }

        private void UpdateLog()
        {
            if (_logs.Count == 0)
                return;

            var index = (_page - 1) * CountPerPage;
            if (index > _logs.Count)
                return;

            var drawLog = (index + CountPerPage <= _logs.Count)
                              ? _logs.GetRange(index, CountPerPage)
                              : _logs.GetRange(index, _logs.Count - index);

            var logs = new List<LogPanel>();
            foreach (var log in drawLog)
            {
                var logPanel = GetEventLog();
                logPanel.Log = log;
                logPanel.Index = ++index;

                logs.Add(logPanel);
            }
            logs.Reverse();
            logDoubleBufferPanel.Controls.AddRange(logs.ToArray());
            logDoubleBufferPanel.Controls.Add(titlePanel);
            logs.Clear();
        }

        private void UpdateLabelLogFound()
        {
            if (_logs.Count == 0)
                foundLabel.Text = Localization["SetupLog_NoLogFound"];
            else if (_logs.Count == 1)
                foundLabel.Text = Localization["SetupLog_LogFound"].Replace("%1", _logs.Count.ToString());
            else
                foundLabel.Text = Localization["SetupLog_LogsFound"].Replace("%1", _logs.Count.ToString());
        }

        private void UpdateLabelLogFound(double timeSpan)
        {
            UpdateLabelLogFound();

            //dont display time
            //foundLabel.Text += @" " + Localization["Common_UsedSeconds"].Replace("%1", timeSpan.ToString("0.00"));
        }

        private void FilterSystemLogComboBoxIndexChanged(Object sender, EventArgs e)
        {
            filterSystemDescComboBox.SelectedIndexChanged -= FilterComboBoxIndexChanged;
            filterSystemDescComboBox.Items.Clear();

            FilterComboBoxIndexChanged(sender, e);

            var serverDescfilters = GetServerDescfilters();
            //----------------------------------------------------------
            if (serverDescfilters.Count > 0)
            {
                filterSystemDescComboBox.Items.Add(Localization["SetupLog_AllLog"]);
                filterSystemDescComboBox.Enabled = true;
                foreach (String filter in serverDescfilters)
                {
                    filterSystemDescComboBox.Items.Add(filter);
                }
            }
            else
            {
                filterSystemDescComboBox.Items.Add("");
                filterSystemDescComboBox.Enabled = false;
            }

            filterSystemDescComboBox.SelectedIndex = 0;
            filterSystemDescComboBox.SelectedIndexChanged += FilterComboBoxIndexChanged;
        }
        private void FilterUserLogComboBoxIndexChanged(Object sender, EventArgs e)
        {
            filterUserDescComboBox.SelectedIndexChanged -= FilterComboBoxIndexChanged;
            filterUserDescComboBox.Items.Clear();

            FilterComboBoxIndexChanged(sender, e);

            var userDescfilters = GetUserDescfilters();
            //----------------------------------------------------------
            if (userDescfilters.Count > 0)
            {
                filterUserDescComboBox.Items.Add(Localization["SetupLog_AllLog"]);
                filterUserDescComboBox.Enabled = true;
                foreach (String filter in userDescfilters)
                {
                    filterUserDescComboBox.Items.Add(filter);
                }
            }
            else
            {
                filterUserDescComboBox.Items.Add("");
                filterUserDescComboBox.Enabled = false;
            }

            filterUserDescComboBox.SelectedIndex = 0;
            filterUserDescComboBox.SelectedIndexChanged += FilterComboBoxIndexChanged;
        }

        private List<String> GetServerfilters()
        {
            var serverfilters = new List<String>();

            foreach (Log log in _logs)
            {
                if (log.User == "") continue;

                if (log.Type == LogType.Server)
                {
                    if (!serverfilters.Contains(log.User))
                        serverfilters.Add(log.User);
                }
            }

            return serverfilters;
        }

        private List<String> GetServerDescfilters()
        {
            var serverDescfilters = new List<String>();

            foreach (Log log in _logs)
            {
                if (log.User == "") continue;

                if (log.Type == LogType.Server)
                {
                    if (log.Description.IndexOf("Delete Folder") == 0)
                    {
                        if (!serverDescfilters.Contains("Delete Folder"))
                            serverDescfilters.Add("Delete Folder");
                    }
                    else if (log.Description.IndexOf("Delay: Delete Folder") == 0)
                    {
                        if (!serverDescfilters.Contains("Delay: Delete Folder"))
                            serverDescfilters.Add("Delay: Delete Folder");
                    }
                    else if (log.Description.IndexOf("Unable to Delete Folder") == 0)
                    {
                        if (!serverDescfilters.Contains("Unable to Delete Folder"))
                            serverDescfilters.Add("Unable to Delete Folder");
                    }
                    else if (log.Description.IndexOf("Delete Record Index Folder") == 0)
                    {
                        if (!serverDescfilters.Contains("Delete Record Index Folder"))
                            serverDescfilters.Add("Delete Record Index Folder");
                    }
                    else if (log.Description.IndexOf("Delete Remove List Folder") == 0)
                    {
                        if (!serverDescfilters.Contains("Delete Remove List Folder"))
                            serverDescfilters.Add("Delete Remove List Folder");
                    }
                    else
                    {
                        if (!serverDescfilters.Contains(log.Description))
                            serverDescfilters.Add(log.Description);
                    }
                }
            }

            return serverDescfilters;
        }

        private List<String> GetUserfilters()
        {
            var userfilters = new List<String>();

            foreach (Log log in _logs)
            {
                if (log.User == "") continue;

                if (log.Type == LogType.Action)
                {
                    if (!userfilters.Contains(log.User))
                        userfilters.Add(log.User);
                }
            }

            return userfilters;
        }

        protected List<string> UserDescFilters = new List<string>()
        {
            "Login", "Logout"
        };

        private List<String> GetUserDescfilters()
        {
            var userDescfilters = new List<String>();

            foreach (Log log in _logs.Where(l => !string.IsNullOrEmpty(l.User) && l.Type == LogType.Action))
            {
                if (userDescfilters.All(u => !log.Description.Contains(u)))
                {
                    var fileter = UserDescFilters.FirstOrDefault(f => log.Description.Contains(f));
                    userDescfilters.Add(fileter ?? log.Description);
                }
            }

            return userDescfilters;
        }

        private void FilterComboBoxIndexChanged(Object sender, EventArgs e)
        {
            FilterLogWithCondition();

            if (OnSelectionChange != null)
                OnSelectionChange(this, new EventArgs<String>(Manager.SelectionChangedXml(TitleName, TitleName,
                    "", (_logs.Count > 0) ? "SaveAs" : "")));
        }
    }
}
