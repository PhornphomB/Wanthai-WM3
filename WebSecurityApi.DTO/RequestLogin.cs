using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace WebSecurityApi.DTO
{
    public class RequestLogin
    {
        public string username { get; set; }
        public string password { get; set; }
        public string device_or_ip { get; set; }
        public string platform { get; set; }
        public string app_id { get; set; }
    }
}