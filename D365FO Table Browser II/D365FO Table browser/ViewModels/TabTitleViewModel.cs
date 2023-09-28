using DevopsHelper.ViewModels;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace D365FO_Table_browser.ViewModels
{
    internal class TabTitleViewModel : FodyViewModel
    {
        public ObservableCollection<TabTitle> TabTitles { get; set; } = new ObservableCollection<TabTitle>();
        public TabTitle SelectedTabTitle { get; set; }
        public TabTitleViewModel()
        {
            TableViewModel tableViewModel = new TableViewModel();
            TabTitles.Add(new TabTitle { Title = "<empty>" });
            TabTitles.Add(new TabTitle { Title = "Company" });
            TabTitles.Add(new TabTitle { Title = "Server" });
            TabTitles.Add(new TabTitle { Title = "Table" });
        }
        public TabTitle SetSelectedTabTitle(string title)
        {
            TabTitle tabTitle = TabTitles.FirstOrDefault(x => x.Title == title);
            SelectedTabTitle = tabTitle;
            return tabTitle;
        }
    }
    public class TabTitle
    {
        public string Title { get; set; }
        public override string ToString()
        {
            return Title; 
        }
    }
}
