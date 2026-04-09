using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;
using System.Dynamic;

namespace Prototype.Providers
{
    public static class DataTableConverter
    {
        public static List<dynamic> ToDynamicList(this DataTable dt)
        {
            var dns = new List<dynamic>();

            foreach (var item in dt.AsEnumerable())
            {
                IDictionary<string, object> dn = new ExpandoObject();

                foreach (var column in dt.Columns.Cast<DataColumn>())
                {
                    dn[column.ColumnName] = item[column];
                }

                dns.Add(dn);
            }

            return dns;
        }

        //public static IEnumerable<dynamic> AsDynamicEnumerable(this DataTable dt)
        //{
        //    foreach (var item in dt.AsEnumerable())
        //    {
        //        IDictionary<string, object> dn = new ExpandoObject();

        //        foreach (var column in dt.Columns.Cast<DataColumn>())
        //        {
        //            dn[column.ColumnName] = item[column];
        //        }

        //        yield return dn;
        //    }
        //}

        public static IEnumerable<dynamic> AsDynamicEnumerable(this DataTable table)
        {
            // Validate argument here..
            return table.AsEnumerable().Select(row => new DynamicRow(row));
        }

        private sealed class DynamicRow : DynamicObject
        {
            private readonly DataRow _row;
            internal DynamicRow(DataRow row) { _row = row; }

            // Interprets a member-access as an indexer-access on the 
            // contained DataRow.
            public override bool TryGetMember(GetMemberBinder binder, out object result)
            {
                var retVal = _row.Table.Columns.Contains(binder.Name);
                result = retVal ? _row[binder.Name] : null;
                return retVal;
            }
        }

    }
}
