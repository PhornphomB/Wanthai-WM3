using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace WebSecurityApi.DTO
{
    public class ResultLogin
    {
        public bool is_success { get; set; }
        public string result_code { get; set; }
        public string result_msg { get; set; }

        public ResultUser user { get; set; }
    }
}