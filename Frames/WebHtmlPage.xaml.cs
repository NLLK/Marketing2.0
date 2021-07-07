using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using CefSharp;

namespace MegaMarketing2Reborn.Frames
{
    /// <summary>
    /// Логика взаимодействия для WebHtmlPage.xaml
    /// </summary>
    public partial class WebHtmlPage : Page
    {

        public WebHtmlPage()
        {
            InitializeComponent();
            webBrowser = App.chromeBrowser;
            string mainDirectory = Environment.CurrentDirectory;
            String path = string.Format(@"{0}\Resources\Web\index.html", mainDirectory);

            //string path = @"C:\SomeDir\hta.txt";

            string html = "";

            using (StreamReader sr = new StreamReader(path, System.Text.Encoding.UTF8))
            {
                string line;
                while ((line = sr.ReadLine()) != null)
                {
                    html += line;
                }
            }

            webBrowser.LoadHtml(html);

            //Uri uri = new Uri("/index.html", UriKind.Relative);
            //Stream source = Application.GetContentStream(uri).Stream;
            //Stream source = File.OpenRead();

            /*Uri uri = new Uri(String.Format("file:///{0}/{1}", mainDirectory, "Resources/Web/index.html"));
            webBrowser.Source = uri;*/
        }

    }
}
