using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel;
using System.Reflection;

namespace Prototype.Providers
{

    public static class EnumExtensions
    {
        public static List<T> GetEnumValues<T>() where T : new()
        {
            T valueType = new T();
            return typeof(T).GetFields().Select(fieldInfo => (T)fieldInfo.GetValue(valueType)).Distinct().ToList();
        }

        public static List<String> GetEnumNames<T>()
        {
            return typeof(T).GetFields().Select(fieldInfo => fieldInfo.Name).Where(qry => !qry.Contains("value__")).Distinct().ToList();
        }

    }

    public static class EnumAttribute
    {
        public static AttributeEntry GetAttrEntry(this Enum _enum)
        {
            return _enum.GetAttribute<AttributeEntry>();
        }

        public static T GetAttribute<T>(this Enum _enum)
        {
            var _fieldInfo = _enum.GetType().GetField(_enum.ToString());
            var _customatr = _fieldInfo.GetCustomAttributes(typeof(T), false)[0];
            var _attribute = (T)Convert.ChangeType(_customatr, typeof(T));
            return _attribute;

        }

        public static T GetEnumAttrEntry<T>(this string _name)
        {
            string[] enumNames = Enum.GetNames(typeof(T));
            foreach (string name in enumNames)
            {
                if (((Enum)Enum.Parse(typeof(T), name)).GetAttribute<AttributeEntry>().Name.ToUpper() == _name.ToUpper())
                    return (T)Enum.Parse(typeof(T), name);
            }
            throw new ArgumentException("The string is not a description or value of the specified enum.");
        }
    }

    public class AttributeEntry : Attribute
    {
        public AttributeEntry(string _name) : this(0, _name, string.Empty) { }
        public AttributeEntry(string _name, string _description) : this(0, _name, _description) { }
        public AttributeEntry(int _id, string _name, string _description)
        {
            ID = _id;
            Name = _name;
            Description = _description;
        }

        public int ID { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
    }
}
