using D365FO_Table_browser.Classes;
using D365FO_Table_browser.Data;
using D365FO_Table_browser.ViewModels;
using MahApps.Metro.Controls;
using MahApps.Metro.Controls.Dialogs;
using Microsoft.Web.WebView2.Core;
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
    /// Interaction logic for MainPage.xaml
    /// </summary>
    public partial class MainPage : Page
    {
        public MainPage()
        {
            InitializeComponent();
            InitTabControl();
        }

        private void InitTabControl()
        {
            MainWindow mainWindow = Application.Current.MainWindow as MainWindow;
            TableTabControl.DataContext = mainWindow.BrowserTabListView;
            TableTabControl.ItemsSource = mainWindow.BrowserTabListView.BrowserTabItems;
        }

        private void TableTabControl_TabItemClosingEvent(object sender, MahApps.Metro.Controls.BaseMetroTabControl.TabItemClosingEventArgs e)
        {

        }

        private void TableTabControl_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (TableTabControl.SelectedItem is BrowserTabItem browserTabItem)
            {
                MainWindow mainWindow = Application.Current.MainWindow as MainWindow;
                mainWindow.BrowserTabListView.SelectedTab = browserTabItem;
            }
        }




    }
}
