using D365FO_Table_browser.Classes;
using D365FO_Table_browser.Data;
using DevopsHelper.ViewModels;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace D365FO_Table_browser.ViewModels
{
    public class ServerListViewModel: FodyViewModel
    {
        public ObservableCollection<D365ServerURL> ServerURLS { get; set; }
        public D365ServerURL SelectedServerURL { get; set; }

        public ServerListViewModel()
        {
            LoadServers();
        }
        public D365ServerURL SetSelectedItem(string _serverURL)
        {
            try
            {
                return ServerURLS.FirstOrDefault(x => x.ServerURL.Equals(_serverURL));
            }
            catch (Exception ex)
            {
                using (EventLog eventLog = new EventLog("Application"))
                {
                    eventLog.Source = "Application";
                    eventLog.WriteEntry(ex.ToString(), EventLogEntryType.Error, 100);
                }
            }
            return null;
        }

        internal void SaveChanges()
        {
            Properties.Settings.Default.ServerURLS = JsonConvert.SerializeObject(ServerURLS, new JsonSerializerSettings { TypeNameHandling = TypeNameHandling.Auto });
            Properties.Settings.Default.Save();
        }
        internal void LoadServers()
        {
            ServerURLS = JsonConvert.DeserializeObject<ObservableCollection<D365ServerURL>>(Properties.Settings.Default.ServerURLS) ?? new ObservableCollection<D365ServerURL>();
            ServerURLS.BubbleSort();
        }
    }
}
