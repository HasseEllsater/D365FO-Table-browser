using D365FO_Table_browser.Classes;
using D365FO_Table_browser.Data;
using D365FO_Table_browser.Views;
using DevopsHelper.ViewModels;
using Newtonsoft.Json;
using System;
using System.Collections.ObjectModel;
using System.Linq;

namespace D365FO_Table_browser.ViewModels
{
    public class BrowserTabItemViewModel : FodyViewModel
    {
        public ObservableCollection<BrowserTabItem> BrowserTabItems { get; set; }
        public BrowserTabItem SelectedTab { get; set; }

        public BrowserTabItemViewModel()
        {
            LoadTabs();
        }
        public void LoadTabs()
        {
            BrowserTabItems = new ObservableCollection<BrowserTabItem>();
            BrowserTabItem tabItem = new BrowserTabItem
            {
                TabName = "Start",
                AllowCloseButton = false,
                Content = new TableControl()
            };

            BrowserTabItems.Add(tabItem);
        }
    }

}
