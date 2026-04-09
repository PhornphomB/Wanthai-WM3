using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ConfigGlobal.DTO._Global
{
    public class ActiveType : List<Prototype.Providers.Property>
    {
        public ActiveType()
        {
            Add(new Prototype.Providers.Property() { Code = "YES", Name = "YES" });
            Add(new Prototype.Providers.Property() { Code = "NO", Name = "NO" });
        }
    }

    public class TypeBarcode : List<Prototype.Providers.Property>
    {
        public TypeBarcode()
        {
            Add(new Prototype.Providers.Property() { Code = "1D", Name = "1D" });
            Add(new Prototype.Providers.Property() { Code = "2D", Name = "2D" });
        }
    }

    public class TypeItemBarcode : List<Prototype.Providers.Property>
    {
        public TypeItemBarcode()
        {
            Add(new Prototype.Providers.Property() { Code = "meter", Name = "มิเตอร์" });
            Add(new Prototype.Providers.Property() { Code = "motor", Name = "หม้อแปลง" });
            Add(new Prototype.Providers.Property() { Code = "ctvt", Name = "ซีที วีที" });
            Add(new Prototype.Providers.Property() { Code = "cable", Name = "สายไฟ" });
            Add(new Prototype.Providers.Property() { Code = "other", Name = "อื่นๆ" });
        }
    }

    public class SizeLabel : List<Prototype.Providers.Property>
    {
        public SizeLabel()
        {
            Add(new Prototype.Providers.Property() { Code = "4*5", Name = "4x5 นิ้ว" });
            Add(new Prototype.Providers.Property() { Code = "2*5", Name = "2x5 ซม." });
        }
    }

    public class Batch : List<Prototype.Providers.Property>
    {
        public Batch()
        {
            Add(new Prototype.Providers.Property() { Code = "N", Name = "N" });
            Add(new Prototype.Providers.Property() { Code = "B", Name = "B" });
            Add(new Prototype.Providers.Property() { Code = "R", Name = "R" });
        }
    }
}
