using Serko_api.Utility;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Xml.Serialization;

namespace Serko_api.Models
{
    [Serializable, XmlRoot("root")]
    public class claim
    {
        public string vendor { get; set; }
        public string description { get; set; }
        public string date { get; set; }
        public string cost_centre { get; set; } = "UNKNOWN";

        [Validate]
        public double total { get; set; }
        public string payment_method { get; set; }
    }
}