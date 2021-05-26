using D365FO_Table_browser.Classes;
using D365FO_Table_browser.Data;
using MahApps.Metro.Controls.Dialogs;
using System;
using System.Collections.Generic;
using System.ComponentModel;
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
    /// Interaction logic for TablesSettings.xaml
    /// </summary>
    public partial class TablesSettings : Page
    {
        public TablesSettings()
        {
            InitializeComponent();
            InitList();
        }
      
        private void InitList()
        {
            MainWindow mainWindow = Application.Current.MainWindow as MainWindow;
            this.DataContext = mainWindow.TableListView;

        }

        private void TableGrid_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (TableGrid.SelectedItem != null)
            {
                D365Table selectedSet = TableGrid.SelectedItem as D365Table;
                if (selectedSet != null)
                {
                    MainWindow mainWindow = Application.Current.MainWindow as MainWindow;
                    mainWindow.TableListView.SelectedTable = selectedSet;
                    mainWindow.TableListView.SaveChanges();
                }
            }
        }
        private async void Remove_Click(object sender, RoutedEventArgs e)
        {
            if (TableGrid.SelectedItem != null)
            {
                D365Table selectedSet = TableGrid.SelectedItem as D365Table;
                if (selectedSet != null)
                {
                    MainWindow mainWindow = Application.Current.MainWindow as MainWindow;

                    string message;
                    if (TableGrid.SelectedItems.Count > 1)
                    {
                        message = string.Format(Properties.Resources.ConfirmRemovalOfTables, TableGrid.SelectedItems.Count);
                    }
                    else
                    {
                        message = string.Format(Properties.Resources.ConfirmRemovalOfTable, selectedSet.Name);
                    }
                    var res = await InfoBox.ShowMessageAsync(Properties.Resources.ConfirmAction, message, MessageDialogStyle.AffirmativeAndNegative);

                    if (res == MessageDialogResult.Affirmative)
                    {
                        List<D365Table> removeList = new List<D365Table>();

                        foreach (D365Table item in TableGrid.SelectedItems)
                        {
                            removeList.Add(item);
                        }
                        foreach (D365Table removeSet in removeList)
                        {
                            mainWindow.TableListView.Tables.Remove(removeSet);
                            mainWindow.TableListView.SelectedTable = null;
                        }
                        mainWindow.TableListView.SaveChanges();
                    }
                }
            }
        }

        private void TableGrid_CellEditEnding(object sender, DataGridCellEditEndingEventArgs e)
        {
            MainWindow mainWindow = Application.Current.MainWindow as MainWindow;
            mainWindow.TableListView.SaveChanges();
        }
    }
}
