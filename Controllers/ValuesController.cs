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

        public IHttpActionResult Get([FromBody]string text)
        {
            claim obj;
            //var text = File.ReadAllText(@"C:\Users\Rahul\Desktop\presto\Data.txt");

            if (!getXML<claim>(text))
                throw new HttpResponseException(new HttpResponseMessage(HttpStatusCode.ExpectationFailed));

            obj = DeserializeFromXmlString<claim>(outputXML.ToString());

            return Json(getOutput(obj));
        }

        private message getOutput(claim obj) => new message(obj);


        private bool getXML<T>(string text)
        {
            var _isSuccess = true;

            outputXML.Clear();

            outputXML.AppendLine("<root>");
            Type type = typeof(T);
            foreach (PropertyInfo property in type.GetProperties())
            {
                var pattern = $@"<{property.Name}.*>((.|\n)*?)<\/{property.Name}>";
                var match = Regex.Match(text, pattern);

                if (match.Success)
                    outputXML.AppendLine(match.Value);
                else
                    _isSuccess = false;


            }

            outputXML.AppendLine("</root>");



            return _isSuccess;
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
