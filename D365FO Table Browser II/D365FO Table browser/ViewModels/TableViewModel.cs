using D365FO_Table_browser.Classes;
using D365FO_Table_browser.Data;
using DevopsHelper.ViewModels;
using Newtonsoft.Json;
using System;
using System.Collections.ObjectModel;
using System.Linq;

namespace D365FO_Table_browser.ViewModels
{
    public class TableViewModel : FodyViewModel
    {
        public ObservableCollection<D365Table> Tables { get; set; }
        public D365Table SelectedTable { get; set; }
        public TableViewModel()
        {
            LoadTables();
        }
        internal void LoadTables()
        {
            Tables = JsonConvert.DeserializeObject<ObservableCollection<D365Table>>(Properties.Settings.Default.Tables) ?? new ObservableCollection<D365Table>();
            Tables.BubbleSort();
        }
        public D365Table SetSelectedTable(string _table)
        {
            return Tables.FirstOrDefault(x => x.Name.Equals(_table));
        }
        internal void SaveChanges()
        {
            Properties.Settings.Default.Tables= JsonConvert.SerializeObject(Tables, new JsonSerializerSettings { TypeNameHandling = TypeNameHandling.Auto });
            Properties.Settings.Default.Save();
        }
    }
}
