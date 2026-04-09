using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ConfigGlobal.DTO._Global
{
    public class LimitOption : List<Prototype.Providers.Property>
    {
        public LimitOption()
        {
            Add(new Prototype.Providers.Property() { Code = "20", Name = "Limit 20" });
            Add(new Prototype.Providers.Property() { Code = "50", Name = "Limit 50" });
            Add(new Prototype.Providers.Property() { Code = "100", Name = "Limit 100" });
            Add(new Prototype.Providers.Property() { Code = "500", Name = "Limit 500" });
            Add(new Prototype.Providers.Property() { Code = "1000", Name = "Limit 1000" });
        }
    }
}
