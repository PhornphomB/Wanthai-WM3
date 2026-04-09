using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ConfigGlobal.DTO.Authen
{
    public class MenuView
    {
        public int MenuGroupIndex { get; set; }
        public int MenuIndex { get; set; }
        public string MenuKey { get; set; }
        public string MenuParentKey { get; set; }
        public string MenuCode { get; set; }
        public string MenuName { get; set; }
        public string MenuUrl { get; set; }
    }
}
