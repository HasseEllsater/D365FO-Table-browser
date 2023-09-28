using D365FO_Table_browser.Data;
using D365FO_Table_browser.ViewModels;
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
            InitTabTitle();
        }
        private void InitTabTitle()
        {
            TabTitleViewModel firstTabTitleViewModel = First.DataContext as TabTitleViewModel;
            if(!string.IsNullOrEmpty(Properties.Settings.Default.First))
            {
                First.SelectedItem = firstTabTitleViewModel.SetSelectedTabTitle(Properties.Settings.Default.First);
            }
            TabTitleViewModel secondTabTitleViewModel = Second.DataContext as TabTitleViewModel;
            if (!string.IsNullOrEmpty(Properties.Settings.Default.Second))
            {
                Second.SelectedItem = secondTabTitleViewModel.SetSelectedTabTitle(Properties.Settings.Default.Second);
            }
            TabTitleViewModel lastTabTitleViewModel = Last.DataContext as TabTitleViewModel;
            if (!string.IsNullOrEmpty(Properties.Settings.Default.Last))
            {
                Last.SelectedItem = lastTabTitleViewModel.SetSelectedTabTitle(Properties.Settings.Default.Last);
            }
        }   
        private void InitLists()
        {
            MainWindow mainWindow = Application.Current.MainWindow as MainWindow;

            if (mainWindow.ServerListView.ServerURLS != null)
            {
                D365ServerURL d365ServerURL = mainWindow.ServerListView.SetSelectedItem(Properties.Settings.Default.DefaultServer);
                ServerList.ItemsSource = mainWindow.ServerListView.ServerURLS;
                
                if (d365ServerURL != null)
                {
                    ServerList.SelectedItem = d365ServerURL;
                }
            }

            if (mainWindow.CompanyAccountListView.CompanyAccounts != null)
            {
                CompanyAccount companyAccount = mainWindow.CompanyAccountListView.SetSelectedItem(Properties.Settings.Default.DefaultCompany);
                CompanyList.ItemsSource = mainWindow.CompanyAccountListView.CompanyAccounts;
                if (companyAccount != null)
                {
                    CompanyList.SelectedItem = companyAccount;
                }
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

        private void First_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if(First.SelectedItem != null)
            {
                Properties.Settings.Default.First = First.SelectedItem.ToString();
                Properties.Settings.Default.Save();
            }
        }

        private void Second_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if(Second.SelectedItem != null)
            {
                Properties.Settings.Default.Second = Second.SelectedItem.ToString();
                Properties.Settings.Default.Save();
            }
        }

        private void Last_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if(Last.SelectedItem != null)
            {
                Properties.Settings.Default.Last = Last.SelectedItem.ToString();
                Properties.Settings.Default.Save();
            }
        }
    }
}
