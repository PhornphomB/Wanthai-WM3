using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace WMS_NEW._ExtensionClass
{
    public static class InboundOrderStatus
    {
        public static string OPEN { get { return "OPEN"; } }
        public static string CLOSE { get { return "CLOSE"; } }
        public static string SHIP { get { return "SHIP"; } }

    }

    public static class ReceiptOrderStatus
    {
        public static string OPEN { get { return "OPEN"; } }
        public static string CLOSE { get { return "CLOSE"; } }
        public static string SHIP { get { return "SHIP"; } }

    }
}