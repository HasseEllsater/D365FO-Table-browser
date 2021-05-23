using D365FO_Table_browser.Classes;
using D365FO_Table_browser.Data;
using D365FO_Table_browser.ViewModels;
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
        private void CheckAboutFile()
        {
            string fileName = "About.html";
            string destfile = Path.GetDirectoryName(System.Reflection.Assembly.GetExecutingAssembly().Location) + "\\" + "About.html";
            Stream file;
            if (!File.Exists(destfile))
            {
                file = this.getFile(fileName);
                this.SaveStreamToFile(destfile, file);
            }
            fileName = "SKutt.png";
            destfile = Path.GetDirectoryName(System.Reflection.Assembly.GetExecutingAssembly().Location) + "\\" + "SKutt.png";
            if (!File.Exists(destfile))
            {
                file = this.getFile(fileName);
                this.SaveStreamToFile(destfile, file);
            }
            Properties.Settings.Default.AboutFile = Path.GetDirectoryName(System.Reflection.Assembly.GetExecutingAssembly().Location) + "\\" + "About.html";
            Properties.Settings.Default.Save();

        }
        private void SaveStreamToFile(string fileFullPath, Stream stream)
        {
            if (stream.Length == 0)
            {
                return;
            }

            // Create a FileStream object to write a stream to a file
            using (FileStream fileStream = System.IO.File.Create(fileFullPath, (int)stream.Length))
            {
                // Fill the bytes[] array with the stream data
                byte[] bytesInStream = new byte[stream.Length];
                stream.Read(bytesInStream, 0, (int)bytesInStream.Length);
                // Use FileStream object to write to the specified file
                fileStream.Write(bytesInStream, 0, bytesInStream.Length);

                //byte[] bytesInStream = ToArray(stream);
                //fileStream.Write(bytesInStream, 0, bytesInStream.Length);


            }
        }
        [System.Security.Permissions.SecurityPermission(System.Security.Permissions.SecurityAction.Demand)]
        private Stream getFile(string Name)
        {
            string asmname = string.Empty;
            try
            {
                // Gets the current assembly.
                Assembly Asm = Assembly.GetExecutingAssembly();
                asmname = Asm.GetName().Name + "." + Name;
                // Resources are named using a fully qualified name.
                Stream strm = Asm.GetManifestResourceStream(asmname);
                return strm; ;
            }
            catch (Exception ex)
            {           
                throw ex;
            }
        }
        public static byte[] ToArray(Stream s)
        {
            if (s == null)
                throw new ArgumentNullException(nameof(s));
            if (!s.CanRead)
                throw new ArgumentException("Stream cannot be read");

            MemoryStream ms = s as MemoryStream;
            if (ms != null)
                return ms.ToArray();

            long pos = s.CanSeek ? s.Position : 0L;
            if (pos != 0L)
                s.Seek(0, SeekOrigin.Begin);

            byte[] result = new byte[s.Length];
            s.Read(result, 0, result.Length);
            if (s.CanSeek)
                s.Seek(pos, SeekOrigin.Begin);
            return result;
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

        internal async void closeTab()
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
