using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace WebSecurityApi.DTO
{
    public class ResultMenu
    {
        public string menu_id { get; set; }
        public string menu_name { get; set; }
        public string parent_menu_id { get; set; }
        public int? menu_group_sequence { get; set; }
        public int menu_sequence { get; set; }
        public string process { get; set; }
    }
}