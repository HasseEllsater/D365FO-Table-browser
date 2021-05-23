using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace D365FO_Table_browser.Data
{
    public class D365ServerURL : IComparable, INotifyPropertyChanged
    {
        private string serverurl;
        public string ServerURL
        {
            get
            {
                return serverurl;
            }
            set
            {
                serverurl = value;
                this.PropertyChanged?.Invoke(this, new PropertyChangedEventArgs("ServerURL"));
            }
        }
        public string Name { get; set; }

        public event PropertyChangedEventHandler PropertyChanged;

        public int CompareTo(object obj)
        {
            if (obj is null)
            {
                throw new Exception(Properties.Resources.CompareObjIsNull);
            }

            var server = obj as D365ServerURL;
            if (server is null)
            {
                throw new Exception(Properties.Resources.CompareObjIsNull);
            }

            return Name.CompareTo(server.Name);
        }
        public override string ToString()
        {
            return Name;
        }
    }
}
