using D365FO_Table_browser.Classes;
using D365FO_Table_browser.Data;
using D365FO_Table_browser.ViewModels;
using D365FO_Table_browser.Views;
using MahApps.Metro.Controls;
using MahApps.Metro.Controls.Dialogs;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Windows;
using System.Windows.Navigation;
using MenuItem = D365FO_Table_browser.ViewModels.MenuItem;


namespace D365FO_Table_browser
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : MetroWindow
    {
        private readonly Navigation.NavigationServiceEx navigationServiceEx;
        public ServerListViewModel ServerListView { get; set; }
        public CompanyAccountViewModel CompanyAccountListView { get; set; }
        public TableViewModel TableListView { get; set; }
        public BrowserTabItemViewModel BrowserTabListView { get; set; }

        public MainWindow()
        {
            try
            {
                InitializeComponent();
                this.navigationServiceEx = new Navigation.NavigationServiceEx();
                this.navigationServiceEx.Navigated += this.NavigationServiceEx_OnNavigated;
                this.HamburgerMenuControl.Content = this.navigationServiceEx.Frame;

                // Navigate to the home page.
                this.Loaded += (sender, args) => this.navigationServiceEx.Navigate(new Uri("Views/MainPage.xaml", UriKind.RelativeOrAbsolute));
                //CheckAboutFile();
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
        #region FrameWork
        private void HamburgerMenuControl_OnItemInvoked(object sender, HamburgerMenuItemInvokedEventArgs e)
        {
            if (e.InvokedItem is MenuItem menuItem && menuItem.IsNavigation)
            {
                this.navigationServiceEx.Navigate(menuItem.NavigationDestination);
            }
        }

        private void NavigationServiceEx_OnNavigated(object sender, NavigationEventArgs e)
        {
            // select the menu item
            this.HamburgerMenuControl.SelectedItem = this.HamburgerMenuControl
                                                         .Items
                                                         .OfType<MenuItem>()
                                                         .FirstOrDefault(x => x.NavigationDestination == e.Uri);
            this.HamburgerMenuControl.SelectedOptionsItem = this.HamburgerMenuControl
                                                                .OptionsItems
                                                                .OfType<MenuItem>()
                                                                .FirstOrDefault(x => x.NavigationDestination == e.Uri);

            // or when using the NavigationType on menu item
            // this.HamburgerMenuControl.SelectedItem = this.HamburgerMenuControl
            //                                              .Items
            //                                              .OfType<MenuItem>()
            //                                              .FirstOrDefault(x => x.NavigationType == e.Content?.GetType());
            // this.HamburgerMenuControl.SelectedOptionsItem = this.HamburgerMenuControl
            //                                                     .OptionsItems
            //                                                     .OfType<MenuItem>()
            //                                                     .FirstOrDefault(x => x.NavigationType == e.Content?.GetType());

            // update back button
            this.GoBackButton.Visibility = this.navigationServiceEx.CanGoBack ? Visibility.Visible : Visibility.Collapsed;
        }

        private void GoBack_OnClick(object sender, RoutedEventArgs e)
        {
            this.navigationServiceEx.GoBack();
        }
        #endregion

        private void MetroWindow_Loaded(object sender, RoutedEventArgs e)
        {
            ServerListView = new ServerListViewModel();
            CompanyAccountListView = new CompanyAccountViewModel();
            TableListView = new TableViewModel();
            BrowserTabListView = new BrowserTabItemViewModel();
        }
        public void RefreshAllTabs()
        {
            foreach(BrowserTabItem tab in BrowserTabListView.BrowserTabItems)
            {
                TableControl tableControl = tab.Content as TableControl;
                tableControl.InitDataContext(this);

            }


        }
        internal async void CloseTab()
        {
            if(this.BrowserTabListView.BrowserTabItems.Count == 1)
            {
                MessageBox.Show(Properties.Resources.LastTab, Properties.Resources.LastTabWarning, MessageBoxButton.OK, MessageBoxImage.Information);
                return;
            }

            if (this.BrowserTabListView.SelectedTab != null)
            {
                BrowserTabItem tabItem = this.BrowserTabListView.SelectedTab;
                string message = string.Format(Properties.Resources.ConfirmCloseTab, tabItem.TabName);
                if(MessageBox.Show(message, Properties.Resources.ConfirmAction,MessageBoxButton.YesNo,MessageBoxImage.Question) == MessageBoxResult.Yes)
                {
                    this.BrowserTabListView.BrowserTabItems.Remove(tabItem);
                }

                //var res = await InfoBox.ShowMessageAsync(Properties.Resources.ConfirmAction, message, MessageDialogStyle.AffirmativeAndNegative);
                //if (res == MessageDialogResult.Affirmative)
                //{
                //    this.BrowserTabListView.BrowserTabItems.Remove(tabItem);
                //}

            }
        }

        private void MetroWindow_Closing(object sender, System.ComponentModel.CancelEventArgs e)
        {
            Properties.Settings.Default.LastTable = string.Empty;
            Properties.Settings.Default.LastURL = string.Empty;
            Properties.Settings.Default.Save();
        }
    }
}
