using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ConfigGlobal.DTO._Global
{
    public class MappingType : List<Prototype.Providers.Property>
    {
        public MappingType()
        {
            Add(new Prototype.Providers.Property() { Code = "0", Name = "Manual" });
            Add(new Prototype.Providers.Property() { Code = "1", Name = "Select All" });
            Add(new Prototype.Providers.Property() { Code = "2", Name = "Unselect All" });
        }
    }
}
