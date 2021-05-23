using D365FO_Table_browser.Classes;
using D365FO_Table_browser.Data;
using D365FO_Table_browser.ViewModels;
using MahApps.Metro.Controls.Dialogs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace D365FO_Table_browser.Views
{
    /// <summary>
    /// Interaction logic for CompanyAccountsPage.xaml
    /// </summary>
    public partial class CompanyAccountsPage : Page
    {
        public CompanyAccountsPage()
        {
            InitializeComponent();
            InitList();
        }

        private void InitList()
        {
            MainWindow mainWindow = Application.Current.MainWindow as MainWindow;
            this.DataContext = mainWindow.CompanyAccountListView;
        }

        private void CompanyGrid_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (CompanyGrid.SelectedItem != null)
            {
                CompanyAccount selectedSet = CompanyGrid.SelectedItem as CompanyAccount;
                if (selectedSet != null)
                {
                    MainWindow mainWindow = Application.Current.MainWindow as MainWindow;
                    mainWindow.CompanyAccountListView.SelectedAccount = selectedSet;
                    mainWindow.CompanyAccountListView.SaveChanges();
                }
            }

        }
        private async void Remove_Click(object sender, RoutedEventArgs e)
        {
            if (CompanyGrid.SelectedItem != null)
            {
                CompanyAccount selectedSet = CompanyGrid.SelectedItem as CompanyAccount;
                if (selectedSet != null)
                {
                    MainWindow mainWindow = Application.Current.MainWindow as MainWindow;

                    string message;
                    if (CompanyGrid.SelectedItems.Count > 1)
                    {
                        message = string.Format(Properties.Resources.ConfirmRemovalOfCompanyAccounts, CompanyGrid.SelectedItems.Count);
                    }
                    else
                    {
                        message = string.Format(Properties.Resources.ConfirmRemovalOfCompanyAccount, selectedSet.Account);
                    }
                    var res = await InfoBox.ShowMessageAsync(Properties.Resources.ConfirmAction, message, MessageDialogStyle.AffirmativeAndNegative);

                    if (res == MessageDialogResult.Affirmative)
                    {
                        List<CompanyAccount> removeList = new List<CompanyAccount>();

                        foreach (CompanyAccount item in CompanyGrid.SelectedItems)
                        {
                            removeList.Add(item);
                        }
                        foreach (CompanyAccount removeSet in removeList)
                        {
                            mainWindow.CompanyAccountListView.CompanyAccounts.Remove(removeSet);
                            mainWindow.CompanyAccountListView.SelectedAccount = null;

                        }

                        mainWindow.CompanyAccountListView.SaveChanges();
                        
                    }
                }
            }
        }
    }
}
