using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace D365FO_Table_browser.Data
{

    public class CompanyAccount : IComparable, INotifyPropertyChanged
    {
        private string account;
        public string Account 
        { 
            get
            {
                return account;
            }
            set
            {
                account = value;
                this.PropertyChanged?.Invoke(this, new PropertyChangedEventArgs("Account"));
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

            var companyAccount = obj as CompanyAccount;
            if (companyAccount is null)
            {
                throw new Exception(Properties.Resources.CompareObjIsNull);
            }

            return Account.CompareTo(companyAccount.Account);
        }

        public override string ToString()
        {
            return Account;
        }
    }
}
