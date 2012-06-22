using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Windows.Shapes;
using Microsoft.Phone.Controls;
using Microsoft.Phone.Tasks;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using Newtonsoft.Json;

namespace PhoneApp1
{
    public partial class MainPage : PhoneApplicationPage
    {
        // Constructor
        public MainPage()
        {
            InitializeComponent();
        }

        private void PhoneApplicationPage_Loaded(object sender, RoutedEventArgs e)
        {
            LocalUrl = "http://jabbr.net/";

            browser.Navigated += new EventHandler<System.Windows.Navigation.NavigationEventArgs>(browser_Navigated);
            browser.IsScriptEnabled = true;

            //browser.Navigate(new Uri(Uri.EscapeUriString("https://jabbr.rpxnow.com//openid/v2/signin?token_url=" + LocalUrl), UriKind.Absolute));
            //browser.Navigate(new Uri(Uri.EscapeUriString("https://JanrainWP7Test.rpxnow.com/openid/embed?token_url=" + "http://localhost/account/logon"), UriKind.Absolute));

            var html = @"
<!DOCTYPE html>
<html>
<head>
    <meta name=""MobileOptimized"" content=""width"" />
    <meta name=""HandheldFriendly"" content=""true"" />
    <meta name=""MobileOptimized"" content=""320"" />
    <meta name=""Viewport"" content=""width=320; initial-scale=1.0"" />
    <meta charset=""utf-8"" />
</head>
<body>
    <script type=""text/javascript"">
        (function () {
            if (typeof window.janrain !== 'object') window.janrain = {};
            if (typeof window.janrain.settings !== 'object') window.janrain.settings = {};

            janrain.settings.tokenUrl = 'http://jabbr.net/auth/login.ashx';

            function isReady() {
                janrain.ready = true;
            };
            if (document.addEventListener) {
                document.addEventListener(""DOMContentLoaded"", isReady, false);
            } else {
                window.attachEvent('onload', isReady);
            }

            var e = document.createElement('script');
            e.type = 'text/javascript';
            e.id = 'janrainAuthWidget';

            if (document.location.protocol === 'https:') {
                e.src = 'https://rpxnow.com/js/lib/jabbr/engage.js';
            } else {
                e.src = 'http://widget-cdn.rpxnow.com/js/lib/jabbr/engage.js';
            }

            var s = document.getElementsByTagName('script')[0];
            s.parentNode.insertBefore(e, s);
        })();
</script>
    
    <a class=""janrainEngage"" id=""janrainSignIn"" href=""#"" style=""margin-top:-1000px;""></a>

<script type=""text/javascript"">
    function fireClick() {
        var elem = ""janrainSignIn"";
        if(typeof elem == ""string"") elem = document.getElementById(elem);
        if(!elem) return;

        if(document.dispatchEvent) {   // W3C
            var oEvent = document.createEvent( ""MouseEvents"" );
            oEvent.initMouseEvent(""click"", true, true,window, 1, 1, 1, 1, 1, false, false, false, false, 0, elem);
            elem.dispatchEvent( oEvent );
        }
        else if(document.fireEvent) {   // IE
            elem.click();
        }

        var janrain = document.getElementById(""janrainModal"");
        janrain.style.top = ""0px"";
        janrain.style.marginTop = ""10px"";
    }

    setTimeout(""fireClick()"", 1500);
</script>
</body>
</html>
";

            browser.NavigateToString(html);
        }

        void browser_Navigated(object sender, System.Windows.Navigation.NavigationEventArgs e)
        {
            if (e.Uri.ToString() == LocalUrl)
            {
                var jabbrState = JsonConvert.DeserializeObject<JabbrState>(browser.GetCookies()["jabbr.state"].Value);
            }
        }

        public string LocalUrl { get; set; }
    }

    public class JabbrState
    {
        public string userId { get; set; }
    }
}