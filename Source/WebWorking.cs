using MegaMarketing2Reborn.Models;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;
namespace MegaMarketing2Reborn
{
    public class WebWorking 
    {
        private static string APP_PATH = "http://localhost:60327";
        static HttpClient client;
        //TODO: rename
        public WebWorking()
        {
            client = new HttpClient();

            client.BaseAddress = new Uri(APP_PATH);
            client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
        }

        public async Task<string> PostQuestionListToAPIAsync(List<RegisterQuestion> questions)
        {
            HttpResponseMessage response = await client.PostAsJsonAsync("/api/WebForms/PostQuestions", questions);
            return "Posted";
        }
    }
}
