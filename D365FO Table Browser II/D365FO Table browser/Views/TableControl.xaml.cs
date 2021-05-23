using D365FO_Table_browser.Data;
using Microsoft.Web.WebView2.Core;
using System;
using System.ComponentModel;
using System.Diagnostics;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using Path = System.IO.Path;

namespace D365FO_Table_browser.Views
{
    /// <summary>
    /// Interaction logic for TableControl.xaml
    /// </summary>
    public partial class TableControl : UserControl
    {
        private bool SkipSelect { get; set; }
        public TableControl(bool _skipSelect = false)
        {
            SkipSelect = _skipSelect;
            InitializeComponent();
            if(!DesignerProperties.GetIsInDesignMode(this))
            {
                InitLists();
                InitializeAsync();
            }
        }

        async void InitializeAsync()
        {
            
            string programData = Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData); ;
            programData = Path.Combine(programData, Properties.Settings.Default.AppDataFolder);

            var webView2Environment = await CoreWebView2Environment.CreateAsync(null, programData);

            await webView.EnsureCoreWebView2Async(webView2Environment);

            if(SkipSelect == false)
            {
                string uri = Properties.Settings.Default.LastURL;
                if (!string.IsNullOrEmpty(uri))
                {
                    MainWindow mainWindow = Application.Current.MainWindow as MainWindow;
                    if (mainWindow.TableListView.Tables != null)
                    {
                        if (!string.IsNullOrEmpty(Properties.Settings.Default.LastTable))
                        {
                            Tables.SelectedItem = mainWindow.TableListView.SetSelectedTable(Properties.Settings.Default.LastTable);
                        }
                    }
                    webView.CoreWebView2.Navigate(uri);
                }
            }
        }
        private void InitLists()
        {
            MainWindow mainWindow = Application.Current.MainWindow as MainWindow;

            try
            {
                if (mainWindow.ServerListView.ServerURLS != null && Properties.Settings.Default.DefaultServer != string.Empty)
                {
                    Server.SelectedItem = mainWindow.ServerListView.SetSelectedItem(Properties.Settings.Default.DefaultServer);
                }

                if (mainWindow.CompanyAccountListView.CompanyAccounts != null && Properties.Settings.Default.DefaultCompany != string.Empty)
                {
                    CompanyAccount.SelectedItem = mainWindow.CompanyAccountListView.SetSelectedItem(Properties.Settings.Default.DefaultCompany);
                }
                //this.Tables.DataContext = mainWindow.TableListView;
                //this.Tables.ItemsSource = mainWindow.TableListView.Tables;


            }
            catch (Exception ex)
            {
                using (EventLog eventLog = new EventLog("Application"))
                {
                    eventLog.Source = "Application";
                    eventLog.WriteEntry(ex.ToString(), EventLogEntryType.Error, 100);
                }
            }


            //var editableCollectionTables = (IEditableCollectionView)Tables.Items;
            //if (editableCollectionTables != null)
            //{
            //    editableCollectionTables.NewItemPlaceholderPosition = NewItemPlaceholderPosition.None;
            //}
        }
        private void OpenTable_Click(object sender, RoutedEventArgs e)
        {
            if (webView != null && webView.CoreWebView2 != null)
            {
                if (Server.SelectedItem == null || CompanyAccount.SelectedItem == null || Tables.SelectedItem == null)
                {
                    MessageBox.Show(Properties.Resources.CantOpenTable, Properties.Resources.ConfirmAction, MessageBoxButton.OK, MessageBoxImage.Exclamation);
                    return;
                }

                D365ServerURL serverUrl = Server.SelectedItem as D365ServerURL;
                string server = serverUrl.ServerURL;

                CompanyAccount companyAccount = CompanyAccount.SelectedItem as CompanyAccount;
                string account = companyAccount.Account;

                D365Table d365table = Tables.SelectedItem as D365Table;
                string table = d365table.Name;

                string menuItem = Properties.Settings.Default.DefaultTableBrowser;

                string gotoTable = "{0}/?mi={1}&cmp={2}&prt=initial&limitednav=true&TableName={3}";


                gotoTable = string.Format(gotoTable, server, menuItem, account, table);

                Properties.Settings.Default.LastURL = gotoTable;

                webView.CoreWebView2.Navigate(gotoTable);

 
                MainWindow mainWindow = Application.Current.MainWindow as MainWindow;

      
                if (mainWindow.BrowserTabListView.SelectedTab != null)
                {
                    BrowserTabItem selectedTab = mainWindow.BrowserTabListView.BrowserTabItems.FirstOrDefault(x => x.TabName.Equals(mainWindow.BrowserTabListView.SelectedTab.TabName));
                    selectedTab.TabName = string.Format ("{0} - {1}", account,table);
                }
            }
        }

        private void Tables_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (Tables.SelectedItem != null)
            {
                D365Table d365table = Tables.SelectedItem as D365Table;
                Properties.Settings.Default.LastTable = d365table.Name;
            }
        }

        private void Left_Click(object sender, RoutedEventArgs e)
        {
            if(webView.CoreWebView2.CanGoBack == true)
            {
                webView.GoBack();
            }
        }

        private void Right_Click(object sender, RoutedEventArgs e)
        {
            if(webView.CoreWebView2.CanGoForward == true)
            {
                webView.GoForward();
            }
        }

        private void Refresh_Click(object sender, RoutedEventArgs e)
        {
            webView.CoreWebView2.Reload();
        }

        private void Help_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                string uri = Properties.Settings.Default.HelpPage;
                webView.CoreWebView2.Navigate(uri);

            }
            catch (Exception ex)
            {

                using (EventLog eventLog = new EventLog("Application"))
                {
                    eventLog.Source = "Application";
                    eventLog.WriteEntry(ex.ToString(), EventLogEntryType.Error, 100);
                }
            }
        }

        private void AddTab_Click(object sender, RoutedEventArgs e)
        {
            MainWindow mainWindow = Application.Current.MainWindow as MainWindow;

            BrowserTabItem tabItem = new BrowserTabItem
            {
                TabName = "New Tab",
                AllowCloseButton = true,
                Content = new TableControl(true)
            };

            mainWindow.BrowserTabListView.BrowserTabItems.Add(tabItem);
            mainWindow.BrowserTabListView.SelectedTab = tabItem;
            this.DataContext = mainWindow.BrowserTabListView;
        }

        private async void RemoveTab_Click(object sender, RoutedEventArgs e)
        {
            MainWindow mainWindow = Application.Current.MainWindow as MainWindow;
            mainWindow.closeTab();
        }
    }
}
