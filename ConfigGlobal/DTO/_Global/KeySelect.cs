using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ConfigGlobal.DTO._Global
{
    [Serializable()]
    public class KeySelect
    {
        public KeySelect()
        {
            Active = false;
        }

        public object KeyId { get; set; }
        public bool Active { get; set; }
    }
}
