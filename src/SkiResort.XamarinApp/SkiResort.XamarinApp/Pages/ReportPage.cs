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

            var reportUrl = string.Format(
                "./Report.html?accessToken={0}&reportId={1}",
                Config.POWERBI_ACCESS_TOKEN,
                Config.POWERBI_REPORT_ID);

            htmlSource.Html = 
                string.Format(@"<html><head>
                  <script type=""text/javascript"">window.location.href = '{0}';</script>
                  </head><body></body></html>", reportUrl);

            return htmlSource;
        }
    }
}
