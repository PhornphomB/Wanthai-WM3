using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace WebSecurityApi.DTO
{
    public class ResultApplication
    {
        public string app_id { get; set; }
        public string application_name { get; set; }
        public string app_version { get; set; }
        public string db_server_name { get; set; }
        public string db_name { get; set; }
        public string db_user { get; set; }
        public string db_password { get; set; }
        public string db_connectionstring { get; set; }
    }
}