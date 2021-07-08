using CefSharp;
using MegaMarketing2Reborn.Source;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

namespace MegaMarketing2Reborn.Frames
{
    /// <summary>
    /// Логика взаимодействия для WebHtmlWindow.xaml
    /// </summary>
    public partial class WebHtmlWindow : Window
    {
        private string QuestionnaireName = "";
        private string RecordId = "";
        public WebHtmlWindow(string _questionnaireName, string _recordId)
        {
            QuestionnaireName = _questionnaireName;
            RecordId = _recordId;
            InitializeComponent();
        }
        private void webBrowser_IsBrowserInitializedChanged(object sender, DependencyPropertyChangedEventArgs e)
        {
            if (webBrowser.IsBrowserInitialized == true)
            {
                string mainDirectory = Environment.CurrentDirectory;
                string html = string.Format(@"file:///{0}/Resources/Web/index.html", mainDirectory.Replace("\\", "/"));
                webBrowser.Load(html);
            }
        }

        private void SaveButton_Click(object sender, RoutedEventArgs e)
        {
            string mainDirectory = Environment.CurrentDirectory;
            //string html = string.Format(@"file:///{0}/Resources/Web/{1}", mainDirectory.Replace("\\", "/"), "index.html");
            //JsScriptBuilder scriptBuilder = new JsScriptBuilder(html);
        }
        private async void webBrowser_FrameLoadEnd(object sender, FrameLoadEndEventArgs e)
        {
            //JavascriptResponse result = await webBrowser.EvaluateScriptAsync("alert(\"AAAAAAAA\")");
        }

        private void webBrowser_JavascriptMessageReceived(object sender, JavascriptMessageReceivedEventArgs e)
        {
            if (e.Message.ToString().Equals("windowLoaded"))
            {
                responseFromJS_windowLoaded();
            }
        }
        private async void responseFromJS_windowLoaded()
        {
            string mainDirectory = Environment.CurrentDirectory;
            string path = string.Format(@"{0}\Resources\Web\{1}", mainDirectory, "setFieldsScript.js");
            JsScriptBuilder scriptBuilder = new JsScriptBuilder(path);
            scriptBuilder.ReplaceValue("questionnaireName", QuestionnaireName);
            scriptBuilder.ReplaceValue("recordId", RecordId);

            JavascriptResponse result = await webBrowser.EvaluateScriptAsync(scriptBuilder.getScript());
        }
    }
}
