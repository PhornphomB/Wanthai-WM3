using System;
using System.Data;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Reflection;

namespace Prototype.Providers
{
    public static class ConvertObject
    {
        public static DataTable ToDataTable(this IQueryable items)
        {
            return items.ToDataTable(null);
        }
        public static DataTable ToDataTable(this IQueryable items, List<String> _columnsName, string _tableName = "")
        {
            Type type = items.ElementType;

            // Create the result table, and gather all properties of a type        
            DataTable table = new DataTable();

            if (!string.IsNullOrEmpty(_tableName))
                table.TableName = _tableName;

            PropertyInfo[] props = type.GetProperties(BindingFlags.Public | BindingFlags.Instance);

            // Add the properties as columns to the datatable
            foreach (PropertyInfo prop in props)
            {
                Type propType = prop.PropertyType;

                // Is it a nullable type? Get the underlying type 
                if (propType.IsGenericType && propType.GetGenericTypeDefinition() == typeof(Nullable<>))
                {
                    propType = Nullable.GetUnderlyingType(propType);
                }

                table.Columns.Add(prop.Name, propType);
            }

            // Add the property values as rows to the datatable
            foreach (object item in items)
            {
                var values = new object[props.Length];

                if (item != null)
                {
                    for (var i = 0; i < props.Length; i++)
                    {
                        values[i] = props[i].GetValue(item, null);
                    }
                }

                table.Rows.Add(values);
            }

            if (_columnsName != null && _columnsName.Count > 0)
            {
                for (int inx = 0; inx <= (table.Columns.Count - 1); inx++)
                {
                    if (inx <= (_columnsName.Count - 1))
                        table.Columns[inx].ColumnName = _columnsName[inx];
                    else
                        break;
                }
            }

            return table;
        }

        public static DataTable ToDataTable(this IEnumerable items, string _tableName = "")
        {
            // Create the result table, and gather all properties of a type        
            DataTable table = new DataTable();

            if (!string.IsNullOrEmpty(_tableName))
                table.TableName = _tableName;

            PropertyInfo[] props = null;

            // Add the property values as rows to the datatable
            foreach (object item in items)
            {
                if (props == null && item != null)
                {
                    Type type = item.GetType();
                    props = type.GetProperties(BindingFlags.Public | BindingFlags.Instance);

                    // Add the properties as columns to the datatable
                    foreach (PropertyInfo prop in props)
                    {
                        Type propType = prop.PropertyType;

                        // Is it a nullable type? Get the underlying type 
                        if (propType.IsGenericType && propType.GetGenericTypeDefinition() == typeof(Nullable<>))
                        {
                            propType = Nullable.GetUnderlyingType(propType);
                        }

                        table.Columns.Add(prop.Name, propType);
                    }
                }

                // When the column headers are defined, all the rows have
                // their number of columns "fixed" to the right number
                var values = new object[props != null ? props.Length : 0];

                if (item != null)
                {
                    for (var i = 0; i < props.Length; i++)
                    {
                        values[i] = props[i].GetValue(item, null);
                    }
                }

                table.Rows.Add(values);
            }

            return table;
        }
    }

}
