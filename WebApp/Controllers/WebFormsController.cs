using MegaMarketing2Reborn.Models;
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

        // GET: api/WebForms
        public IEnumerable<string> Get()
        {
            return new string[] { "value1", "value2" };
        }

        // GET: api/WebForms/5
        public string Get(int id)
        {
            return "value";
        }

        // POST: api/WebForms
        public void Post(List<RegisterQuestion> questions)
        {

        }

        // PUT: api/WebForms/5
        public void Put(int id, [FromBody]string value)
        {
        }

        // DELETE: api/WebForms/5
        public void Delete(int id)
        {
        }
    }
}
