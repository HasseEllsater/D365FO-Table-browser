using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace D365FO_Table_browser.Data
{
    public class BrowserTabItem : IComparable, INotifyPropertyChanged
    {
        private string tabname;
        public string TabName 
        {
            get
            {
                return tabname;
            }
            set
            {
                tabname = value;
                this.PropertyChanged?.Invoke(this, new PropertyChangedEventArgs("TabName"));
            }
        }
        public object Content { get; set; }

        public bool AllowCloseButton { get; set; }

        public event PropertyChangedEventHandler PropertyChanged;

        public int CompareTo(object obj)
        {
            if (obj is null)
            {
                throw new Exception(Properties.Resources.CompareObjIsNull);
            }

            var currentTab = obj as BrowserTabItem;
            if (currentTab is null)
            {
                throw new Exception(Properties.Resources.CompareObjIsNull);
            }

            return TabName.CompareTo(currentTab.TabName);
        }
    }
}
