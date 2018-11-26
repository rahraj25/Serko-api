using Serko_api.Utility;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Serko_api.Models
{
    public class message
    {
        public message(claim claimObj)
        {
            this.claim = claimObj;
            this.GST = claim.total.getGST();
            this.actualtotal = Math.Round(this.claim.total - this.GST, 2);

        }
        public claim claim { get; set; }
        public double GST { get; }
        public double actualtotal { get; set; }
    }
}