using D365FO_Table_browser.Classes;
using D365FO_Table_browser.Data;
using MahApps.Metro.Controls.Dialogs;
using System.Collections.Generic;
using System.Windows;
using System.Windows.Controls;


namespace D365FO_Table_browser.Views
{
    /// <summary>
    /// Interaction logic for ServerPage.xaml
    /// </summary>
    public partial class ServerPage : Page
    {
        public ServerPage()
        {
            InitializeComponent();
            InitList();
        }

        private void InitList()
        {
            MainWindow mainWindow = Application.Current.MainWindow as MainWindow;
            this.DataContext = mainWindow.ServerListView;
        }


        private void ServerGrid_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (ServerGrid.SelectedItem != null)
            {
                D365ServerURL selectedSet = ServerGrid.SelectedItem as D365ServerURL;
                if (selectedSet != null)
                {
                    MainWindow mainWindow = Application.Current.MainWindow as MainWindow;
                    mainWindow.ServerListView.SelectedServerURL = selectedSet;
                    mainWindow.ServerListView.SaveChanges();
                }
            }
        }

        private async void Remove_Click(object sender, RoutedEventArgs e)
        {
            if (ServerGrid.SelectedItem != null)
            {
                D365ServerURL selectedSet = ServerGrid.SelectedItem as D365ServerURL;
                if (selectedSet != null)
                {
                    MainWindow mainWindow = Application.Current.MainWindow as MainWindow;

                    string message;
                    if (ServerGrid.SelectedItems.Count > 1)
                    {
                        message = string.Format(Properties.Resources.ConfirmRemovalOfServers, ServerGrid.SelectedItems.Count);
                    }
                    else
                    {
                        message = string.Format(Properties.Resources.ConfirmRemovalOfServer, selectedSet.Name);
                    }
                    var res = await InfoBox.ShowMessageAsync(Properties.Resources.ConfirmAction, message, MessageDialogStyle.AffirmativeAndNegative);

                    if (res == MessageDialogResult.Affirmative)
                    {
                        List<D365ServerURL> removeList = new List<D365ServerURL>();

                        foreach (D365ServerURL item in ServerGrid.SelectedItems)
                        {
                            removeList.Add(item);
                        }
                        foreach (D365ServerURL removeSet in removeList)
                        {
                            mainWindow.ServerListView.ServerURLS.Remove(removeSet);
                            mainWindow.ServerListView.SelectedServerURL = null;

                        }

                        mainWindow.ServerListView.SaveChanges();
                    }
                }
            }
        }
    }
}