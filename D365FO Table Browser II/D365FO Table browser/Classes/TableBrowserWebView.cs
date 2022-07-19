using Microsoft.Web.WebView2.Core;
using Microsoft.Web.WebView2.Wpf;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace D365FO_Table_browser.Classes
{
    public class TableBrowserWebView : WebView2
    {
        public CoreWebView2Deferral Deferral { get; set; }
        public CoreWebView2NewWindowRequestedEventArgs EventArgs { get; set; }
        public TableBrowserWebView()
        {
            if (!DesignerProperties.GetIsInDesignMode(this))
            {
                this.CoreWebView2InitializationCompleted += TableBrowserWebView_CoreWebView2InitializationCompleted;
            }
        }
        private void CoreWebView2_NewWindowRequested(object sender, CoreWebView2NewWindowRequestedEventArgs e)
        {
            TableBrowserWebView browser = new TableBrowserWebView();
            browser.Deferral = e.GetDeferral();
            browser.EventArgs = e;
        }

        private void TableBrowserWebView_CoreWebView2InitializationCompleted(object sender, CoreWebView2InitializationCompletedEventArgs e)
        {
            if (e.IsSuccess)
            {
                this.CoreWebView2.NewWindowRequested += CoreWebView2_NewWindowRequested;
                // Was deferral set in previous instance?
                if (Deferral != null)
                {
                    // Set this instance, complete NewWindowRequested and clear deferral
                    EventArgs.NewWindow = this.CoreWebView2;
                    Deferral.Complete();
                    Deferral = null;
                    EventArgs = null;
                }
            }
        }
    }
}
