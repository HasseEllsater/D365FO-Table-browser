using D365FO_Table_browser.Classes;
using D365FO_Table_browser.Data;
using DevopsHelper.ViewModels;
using Newtonsoft.Json;
using System.Collections.ObjectModel;
using System.Linq;

namespace D365FO_Table_browser.ViewModels
{
    public class CompanyAccountViewModel : FodyViewModel
    {
        public ObservableCollection<CompanyAccount> CompanyAccounts { get; set; }
        public CompanyAccount SelectedAccount { get; set; }

        public CompanyAccountViewModel()
        {
            LoadAccounts();
        }

        private void LoadAccounts()
        {
            CompanyAccounts = JsonConvert.DeserializeObject<ObservableCollection<CompanyAccount>>(Properties.Settings.Default.CompanyAccounts) ?? new ObservableCollection<CompanyAccount>();
            CompanyAccounts.BubbleSort();

        }
        internal void SaveChanges()
        {
            Properties.Settings.Default.CompanyAccounts = JsonConvert.SerializeObject(CompanyAccounts, new JsonSerializerSettings { TypeNameHandling = TypeNameHandling.Auto });
            Properties.Settings.Default.Save();
        }
        public CompanyAccount SetSelectedItem(string _companyAccount)
        {
            return CompanyAccounts.FirstOrDefault(x => x.Account.Equals(_companyAccount));
        }
    }
}
