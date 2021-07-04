using MegaMarketing2Reborn.Models;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
namespace WebApp.Controllers
{
    public class WebFormsController : ApiController
    {
        private List<RegisterQuestion> QuestionsList = new List<RegisterQuestion>();
        // POST: api/WebForms
        public string PostQuestions(List<RegisterQuestion> questions)
        {
            QuestionsList = questions;
            return "OK";
        } 
        public string GetQuestions()
        {
            string json = JsonConvert.SerializeObject(QuestionsList);
            return json;
            //QuestionsList = questions;
        }

    }
}
