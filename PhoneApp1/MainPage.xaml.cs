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
            LocalUrl = "https://localhost/account/logon";

            browser.NavigationFailed += new System.Windows.Navigation.NavigationFailedEventHandler(browser_NavigationFailed);
            browser.Navigated += new EventHandler<System.Windows.Navigation.NavigationEventArgs>(browser_Navigated);
            browser.Navigating += new EventHandler<NavigatingEventArgs>(browser_Navigating);
            browser.IsScriptEnabled = true;

            browser.Navigate(new Uri(Uri.EscapeUriString("https://JabbR.rpxnow.com//openid/v2/signin?token_url=" + LocalUrl), UriKind.Absolute));
            //browser.Navigate(new Uri(Uri.EscapeUriString("https://JanrainWP7Test.rpxnow.com/openid/embed?token_url=" + "http://localhost/account/logon"), UriKind.Absolute));
        }

        void browser_Navigating(object sender, NavigatingEventArgs e)
        {
            if (e.Uri.ToString() == LocalUrl)
            {
            }
        }

        void browser_Navigated(object sender, System.Windows.Navigation.NavigationEventArgs e)
        {
            if (e.Uri.ToString() == LocalUrl)
            {
            }
        }

        void browser_NavigationFailed(object sender, System.Windows.Navigation.NavigationFailedEventArgs e)
        {
            if (e.Uri.ToString() == LocalUrl)
            {
                var html = browser.SaveToString();
                var tokenStart = "<INPUT id=token name=token value=";

                var token = html.Substring(html.IndexOf(tokenStart) + tokenStart.Length);
                token = token.Substring(0, token.IndexOf(' '));

                object profileData = GetProfileData(token);
            }
        }

        private object GetProfileData(string token)
        {
            HttpWebRequest request = (HttpWebRequest)WebRequest.Create("http://jabbr.net/Auth/Login.ashx");
            request.Method = "POST";
            request.ContentType = "application/x-www-form-urlencoded";
            string data = "token=" + token;

            request.BeginGetRequestStream(getResultCallback =>
                {
                    using (var requestStream = request.EndGetRequestStream(getResultCallback))
                    {
                        using (var requestWriter = new StreamWriter(requestStream))
                        {
                            requestWriter.Write(data);
                            requestWriter.Flush();
                        }
                    }

                    request.BeginGetResponse(getResponseCallback =>
                        {
                            var responseStream = request.EndGetResponse(getResponseCallback);

                            using (var responseReader = new StreamReader(responseStream.GetResponseStream()))
                            {
                                string responseString = responseReader.ReadToEnd();
                            }

                        }, null);
                }, null);

            return null;


            //string apikey = "5859eaa07ba12e0dc2872d1c03be9528c58bfcc1";

            //string data = "token=" + token + "&apiKey=" + apikey;
            //Uri url = new Uri(@"https://rpxnow.com/api/v2/auth_info");

            //HttpWebRequest request = (HttpWebRequest)WebRequest.Create(url);
            //request.Method = "POST";
            //request.ContentType = "application/x-www-form-urlencoded";

            //request.BeginGetRequestStream(getResultCallback =>
            //    {
            //        using (var requestStream = request.EndGetRequestStream(getResultCallback))
            //        {
            //            //using (var requestWriter = new StreamWriter(requestStream))
            //            //{
            //            //    requestWriter.Write(StringToAscii(data));
            //            //}
            //            var bytes = StringToAscii(data);
            //            requestStream.Write(bytes, 0, bytes.Length);
            //        }

            //        request.BeginGetResponse(getResponseCallback =>
            //            {
            //                var responseStream = request.EndGetResponse(getResponseCallback);

            //                using (var responseReader = new StreamReader(responseStream.GetResponseStream()))
            //                {
            //                    string responseString = responseReader.ReadToEnd();
            //                }

            //                }, null);
            //    }, null);

            //return null; 
        }

        public static byte[] StringToAscii(string s)
        {
            byte[] retval = new byte[s.Length];
            for (int ix = 0; ix < s.Length; ++ix)
            {
                char ch = s[ix];
                if (ch <= 0x7f) retval[ix] = (byte)ch;
                else retval[ix] = (byte)'?';
            }
            return retval;
        }

        public string LocalUrl { get; set; }
    }
}