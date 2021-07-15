using CefSharp;
using MegaMarketing2Reborn.Models;
using MegaMarketing2Reborn.Source;
using Newtonsoft.Json;
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
        private string MainDirectory = Environment.CurrentDirectory;
        private string RecordId = "";
        private Excel excel;
        private RegisterQuestionnaire Questionnaire;
        public WebHtmlWindow(string _recordId, RegisterQuestionnaire _questionnaire, Excel _excel)
        {
            RecordId = _recordId;
            Questionnaire = _questionnaire;
            excel = _excel;
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
            //string html = string.Format(@"file:///{0}/Resources/Web/{1}", MainDirectory.Replace("\\", "/"), "index.html");
            //JsScriptBuilder scriptBuilder = new JsScriptBuilder(html);
        }
        private async void webBrowser_FrameLoadEnd(object sender, FrameLoadEndEventArgs e)
        {
            string path = string.Format(@"{0}\Resources\Web\{1}", MainDirectory, "formsScript.js");
            JsScriptBuilder scriptBuilder = new JsScriptBuilder(path);
            scriptBuilder.ReplaceValueWithObject("questionnaire", JsonConvert.SerializeObject(Questionnaire.QuestionsList));

            JavascriptResponse result = await webBrowser.EvaluateScriptAsync(scriptBuilder.getScript());
        }

        private void webBrowser_JavascriptMessageReceived(object sender, JavascriptMessageReceivedEventArgs e)
        {
            if (e.Message.ToString().Equals("windowLoaded"))
            {
                responseFromJS_windowLoaded();
            }
            else responseFromJS_answers(e.Message.ToString());
        }
        private async void responseFromJS_windowLoaded()
        {
            string path = string.Format(@"{0}\Resources\Web\{1}", MainDirectory, "setFieldsScript.js");
            JsScriptBuilder scriptBuilder = new JsScriptBuilder(path);
            scriptBuilder.ReplaceValue("questionnaireName", Questionnaire.QuestionnaireName);
            scriptBuilder.ReplaceValue("recordId", excel.lastRowNumber.ToString());

            JavascriptResponse result = await webBrowser.EvaluateScriptAsync(scriptBuilder.getScript());
        }
        private async void responseFromJS_answers(string jsonAnswers)
        {
            //TODO:добавить вложенность
            List<RegisterQuestion> answersFromJson = JsonConvert.DeserializeObject<List<RegisterQuestion>>(jsonAnswers);
            excel.WriteRow(answersFromJson);
        }
    }
}
