using SkiResort.XamarinApp.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection.Emit;
using System.Text;

using Xamarin.Forms;

namespace SkiResort.XamarinApp.Pages
{
    public class ReportPage : ContentPage
    {
        public ReportPage()
        {
            Title = "Report";
            Content = new StackLayout
            {
                Children = {
                    getReportWebView()
                }
            };
        }

        private WebView getReportWebView()
        {
            return new WebView()
            {
                VerticalOptions = LayoutOptions.FillAndExpand,
                HorizontalOptions = LayoutOptions.FillAndExpand,
                Source = getReportHtmlWebViewSource()
            };
        }

        private HtmlWebViewSource getReportHtmlWebViewSource()
        {
            var htmlSource = new HtmlWebViewSource();
            htmlSource.BaseUrl = DependencyService.Get<IBaseUrl>().Get();

            htmlSource.Html = 
                string.Format(@"<html><head>
                  <script type=""text/javascript"">window.location.href = './Report.html';</script>
                  </head><body></body></html>");

            return htmlSource;
        }
    }
}
