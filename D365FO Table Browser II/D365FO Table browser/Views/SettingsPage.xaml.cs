using D365FO_Table_browser.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;

namespace D365FO_Table_browser.Views
{
    /// <summary>
    /// Interaction logic for SettingsPage.xaml
    /// </summary>
    public partial class SettingsPage : Page
    {
        public SettingsPage()
        {
            InitializeComponent();
            InitLists();
        }
        private void InitLists()
        {
            MainWindow mainWindow = Application.Current.MainWindow as MainWindow;

            if (mainWindow.ServerListView.ServerURLS != null)
            {
                ServerList.SelectedItem = mainWindow.ServerListView.SetSelectedItem(Properties.Settings.Default.DefaultServer);
            }

            if (mainWindow.CompanyAccountListView.CompanyAccounts != null)
            {
                CompanyList.SelectedItem = mainWindow.CompanyAccountListView.SetSelectedItem(Properties.Settings.Default.DefaultCompany);
            }

            TableBrowser.Text = Properties.Settings.Default.DefaultTableBrowser;
        }

        private void CompanyList_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if(CompanyList.SelectedItem != null)
            {
                CompanyAccount selected = CompanyList.SelectedItem as CompanyAccount;
                Properties.Settings.Default.DefaultCompany = selected.Account;
                Properties.Settings.Default.Save();

            }
        }

        private void TableBrowser_TextChanged(object sender, TextChangedEventArgs e)
        {
            Properties.Settings.Default.DefaultTableBrowser = TableBrowser.Text;
            Properties.Settings.Default.Save();
        }

        private void ServerList_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if(ServerList.SelectedItem != null)
            {
                D365ServerURL serverURL = ServerList.SelectedItem as D365ServerURL;
                Properties.Settings.Default.DefaultServer = serverURL.ServerURL;
                Properties.Settings.Default.Save();
            }
        }
    }
}
