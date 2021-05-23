using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace D365FO_Table_browser.Data
{
    public class D365Table : IComparable, INotifyPropertyChanged
    {
        private string name;

        public string Name
        {
            get
            {
                return name;
            }
            set
            {
                name = value;
                this.PropertyChanged?.Invoke(this, new PropertyChangedEventArgs("Name"));
            }
        }
        public string Description { get; set; }

        public event PropertyChangedEventHandler PropertyChanged;

        public int CompareTo(object obj)
        {
            if (obj is null)
            {
                throw new Exception(Properties.Resources.CompareObjIsNull);
            }

            var table = obj as D365Table;
            if (table is null)
            {
                throw new Exception(Properties.Resources.CompareObjIsNull);
            }

            return Name.CompareTo(table.Name);
        }
        public override string ToString()
        {
            return Name;
        }
    }
}
