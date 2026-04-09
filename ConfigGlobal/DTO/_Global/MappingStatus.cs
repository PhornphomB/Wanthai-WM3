using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ConfigGlobal.DTO._Global
{
    public class MappingStatus : List<Prototype.Providers.Property>
    {
        public MappingStatus()
        {
            Add(new Prototype.Providers.Property() { Code = "", Name = "--All--" });
            Add(new Prototype.Providers.Property() { Code = "YES", Name = "YES" });
            Add(new Prototype.Providers.Property() { Code = "NO", Name = "NO" });
        }
    }
}
