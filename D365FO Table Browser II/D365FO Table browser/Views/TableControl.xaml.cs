using D365FO_Table_browser.Classes;
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
            
            if (SkipSelect == false)
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
                    FullUrl.Text = uri;
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

                InitDataContext(mainWindow);

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

        public void InitDataContext(MainWindow mainWindow)
        {
            Tables.DataContext = mainWindow.TableListView;
            Tables.ItemsSource = mainWindow.TableListView.Tables.ToList<D365Table>();
            CompanyAccount.DataContext = mainWindow.CompanyAccountListView;
            CompanyAccount.ItemsSource = mainWindow.CompanyAccountListView.CompanyAccounts.ToList<CompanyAccount>();
            Server.DataContext = mainWindow.ServerListView;
            Server.ItemsSource = mainWindow.ServerListView.ServerURLS.ToList<D365ServerURL>();
        }

        private void OpenTable_Click(object sender, RoutedEventArgs e)
        {
            if (webView != null && webView.CoreWebView2 != null)
            {
                if (Server.SelectedItem == null || CompanyAccount.SelectedItem == null || string.IsNullOrEmpty(Tables.Text))
                {
                    MessageBox.Show(Properties.Resources.CantOpenTable, Properties.Resources.ConfirmAction, MessageBoxButton.OK, MessageBoxImage.Exclamation);
                    return;
                }
                MainWindow mainWindow = Application.Current.MainWindow as MainWindow;

                if (Tables.SelectedItem == null)
                {
                    D365Table d365tableAdd = new D365Table();
                    d365tableAdd.Name = Tables.Text;
                    mainWindow.TableListView.Tables.Add(d365tableAdd);
                    mainWindow.TableListView.SelectedTable = d365tableAdd;
                    Tables.DataContext = mainWindow.TableListView;
                    Tables.ItemsSource = mainWindow.TableListView.Tables.ToList<D365Table>();
                    Tables.SelectedItem = d365tableAdd;
                }


                D365ServerURL serverUrl = Server.SelectedItem as D365ServerURL;
                string server = serverUrl.ServerURL;

                CompanyAccount companyAccount = CompanyAccount.SelectedItem as CompanyAccount;
                string account = companyAccount.Account;

                D365Table d365table = Tables.SelectedItem as D365Table;
                string table = d365table.Name;

                string menuItem = Properties.Settings.Default.DefaultTableBrowser;

                string gotoTable = "{0}?mi={1}&cmp={2}&prt=initial&limitednav=true&TableName={3}";


                gotoTable = string.Format(gotoTable, server, menuItem, account, table);
                FullUrl.Text = gotoTable;

                Properties.Settings.Default.LastURL = gotoTable;
         
                webView.CoreWebView2.Navigate(gotoTable);
      
                if (mainWindow.BrowserTabListView.SelectedTab != null)
                {
                    string tabName = string.Empty;
                    BrowserTabItem selectedTab = mainWindow.BrowserTabListView.BrowserTabItems.FirstOrDefault(x => x.TabName.Equals(mainWindow.BrowserTabListView.SelectedTab.TabName));
                    if(!string.IsNullOrEmpty(Properties.Settings.Default.First))
                    {
                        if(Properties.Settings.Default.First != "<empty>")
                        {
                            //Switch on Properties.Setting.Default.First
                            switch (Properties.Settings.Default.First)
                            {
                                case "Company":
                                    tabName += account + "-";
                                    break;
                                    case "Server":
                                        tabName += serverUrl.Name + "-";
                                    break;
                                    case "Table":
                                        tabName += table + "-";
                                    break;
                                default:
                                    break;
                            }
                        }
                    }
                    if (!string.IsNullOrEmpty(Properties.Settings.Default.Second))
                    {
                        if (Properties.Settings.Default.First != "<empty>")
                        {
                            //Switch on Properties.Setting.Default.First
                            switch (Properties.Settings.Default.Second)
                            {
                                case "Company":
                                    tabName += account + "-";
                                    break;
                                case "Server":
                                    tabName += serverUrl.Name + "-";
                                    break;
                                case "Table":
                                    tabName += table + "-";
                                    break;
                                default:
                                    break;
                            }
                        }
                    }
                    if (!string.IsNullOrEmpty(Properties.Settings.Default.Last))
                    {
                        if (Properties.Settings.Default.First != "<empty>")
                        {
                            //Switch on Properties.Setting.Default.First
                            switch (Properties.Settings.Default.Last)
                            {
                                case "Company":
                                    tabName += account + "-";
                                    break;
                                case "Server":
                                    tabName += serverUrl.Name + "-";
                                    break;
                                case "Table":
                                    tabName += table + "-";
                                    break;
                                default:
                                    break;
                            }
                        }
                    }
                    if(!string.IsNullOrWhiteSpace(tabName))
                    {
                        //Remove last -
                        tabName = tabName.Substring(0, tabName.Length - 1);
                        selectedTab.TabName = tabName;
                    }
                    else
                    {
                        selectedTab.TabName = string.Format("{0} - {1}", account, table);
                    }
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
            MainWindow mainWindow = Application.Current.MainWindow as MainWindow;
            InitDataContext(mainWindow);

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
            this.DataContext = mainWindow.BrowserTabListView;
        }

        private async void RemoveTab_Click(object sender, RoutedEventArgs e)
        {
            MainWindow mainWindow = Application.Current.MainWindow as MainWindow;
            mainWindow.CloseTab();
        }


    }
}
