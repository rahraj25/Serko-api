using Serko_api.Models;
using Serko_api.Utility;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Reflection;
using System.Text;
using System.Text.RegularExpressions;
using System.Web.Hosting;
using System.Web.Http;
using System.Web.Http.Results;
using System.Xml.Linq;
using System.Xml.Serialization;

namespace Serko_api.Controllers
{

    public class ValuesController : ApiController
    {

        readonly StringBuilder outputXML;

        public ValuesController()
        {
            outputXML = new StringBuilder();
        }

        // /api/values/
        [HttpGet]
        public IHttpActionResult Get()//[FromBody]string text
        {
            claim obj;
            var text = File.ReadAllText(HostingEnvironment.MapPath("~/App_Data/Data.txt"));

            var message = string.Empty;
            switch (getXML<claim>(text))
            {
                case responseEnum.MissingTag: message = "Missing end tag"; break;
                case responseEnum.Invalid: message = "No [Total] attribute found"; break;

            }

            if (!string.IsNullOrEmpty(message))
                throw new HttpResponseException(Request.CreateErrorResponse(HttpStatusCode.PreconditionFailed,
                      message));

            obj = DeserializeFromXmlString<claim>(outputXML.ToString());
            return Json(getOutput(obj));
        }

        private message getOutput(claim obj) => new message(obj);


        private responseEnum getXML<T>(string text)
        {
            responseEnum _response = responseEnum.Success;

            outputXML.Clear();

            outputXML.AppendLine("<root>");
            Type type = typeof(T);
            foreach (PropertyInfo property in type.GetProperties())
            {
                var startpattern = $@"<{property.Name}.*>((.|\n)*?)";
                var endpattern = $@"<\/{property.Name}>";

                var validation = property.GetCustomAttributes<Validate>(true).Any();

                var openTagmatch = Regex.Match(text, startpattern);
                var actualTagmatch = Regex.Match(text, string.Concat(startpattern, endpattern));


                if (actualTagmatch.Success)
                    outputXML.AppendLine(actualTagmatch.Value);
                else if (openTagmatch.Success)
                {
                    _response = responseEnum.MissingTag;
                    break;
                }
                else if (validation)
                {
                    _response = responseEnum.Invalid;
                    break;
                }



            }

            outputXML.AppendLine("</root>");



            return _response;
        }




        private T DeserializeFromXmlString<T>(string xmlString)
        {
            var serializer = new XmlSerializer(typeof(T));
            using (var reader = new StringReader(xmlString))
            {
                return (T)serializer.Deserialize(reader);
            }
        }



    }
}
