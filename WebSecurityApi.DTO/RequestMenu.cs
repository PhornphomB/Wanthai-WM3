using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace WebSecurityApi.DTO
{
    public class RequestMenu
    {
        public string app_id { get; set; }
        public string locale_id { get; set; }
        public string platform { get; set; }
        public string user_group_id { get; set; }
    }
}