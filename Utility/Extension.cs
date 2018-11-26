using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Serko_api.Utility
{
    public static class Extension
    {
        public static double getGST(this double amt) => Math.Round(amt * 0.15, 2);
    }
}