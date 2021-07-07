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
        }
        private void webBrowser_IsBrowserInitializedChanged(object sender, DependencyPropertyChangedEventArgs e)
        {
            if (webBrowser.IsBrowserInitialized == true)
            {
                string mainDirectory = Environment.CurrentDirectory;
                /*                String path = string.Format(@"{0}\Resources\Web\index.html", mainDirectory);
                                string html = "";

                                using (StreamReader sr = new StreamReader(path, System.Text.Encoding.UTF8))
                                {
                                    string line;
                                    while ((line = sr.ReadLine()) != null)
                                    {
                                        html += line;
                                    }
                                }
                */
                string path = string.Format(@"file:///{0}/Resources/Web/index.html", mainDirectory.Replace("\\","/"));

                webBrowser.Load(path);
            }
        }
    }
}
