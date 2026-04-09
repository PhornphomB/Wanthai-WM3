using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Prototype.Providers
{
    [Serializable()]
    public class Property
    {
        public Property()
        {
            guid_member = Guid.Empty;
            value_member = null;
            display_member = string.Empty;
        }

        public const string KeyValue = "value_member";
        public const string KeyDisplay = "display_member";

        public const string KeyCode = "Code";
        public const string KeyName = "Name";

        public Guid guid_member { get; set; }
        public string value_member { get; set; }
        public string display_member { get; set; }

        public string Code
        {
            set
            {
                value_member = value;
            }
            get
            {
                if (guid_member != Guid.Empty)
                    return guid_member.ToString();
                else
                    return value_member;
            }
        }
        public string Name
        {
            set
            {
                display_member = value;
            }
            get
            {
                return display_member;
            }
        }

        public override string ToString()
        {
            return Code;
        }
    }
}

