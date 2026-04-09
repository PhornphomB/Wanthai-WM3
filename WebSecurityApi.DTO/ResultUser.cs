using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace WebSecurityApi.DTO
{
    public class ResultUser
    {
        public string user_id { get; set; }
        public string username { get; set; }
        public string user_group_id { get; set; }
        public string user_group_name { get; set; }
        public string locale_id { get; set; }


        public string first_name { get; set; }
        public string last_name { get; set; }
        public string email { get; set; }
        public string telephone { get; set; }
        public string department { get; set; }
    }
}