using System;
using System.Linq;

using Prototype.Providers;
using WMS_NEW.Source;
using System.Collections.Generic;

namespace WMS_NEW.Access.MasterData
{
    public class Rule : IDisposable
    {
        #region ++INSTANCE STATIC++
        public static Rule Instance
        {
            get
            {
                using (Rule _Instance = new Rule())
                {
                    return _Instance;
                }
            }
        }
        #endregion


        public WMSEntities _Model { get; set; }

        public Rule()
        {
            this._Model = new WMSEntities();

            //AGridObjectSourceQuery Map Model
            //GC.SuppressFinalize(this);

        }

        public void Dispose()
        {
            GC.SuppressFinalize(this);
        }


        public bool Is_Rule(string _rule_code, string _value)
        {
            var result = (from rows in this._Model.t_wms_rule
                          where rows.rule_code == _rule_code
                          select rows).FirstOrDefault();

            if (result != null)
            {
                return result.value.ToUpper() == _value.ToUpper() ? true : false;
            }
            else
            {
                return false;
            }
            //return this._Model.t_wms_rule.Any(an => an.rule_code.ToUpper() == _rule_code.ToUpper() 
            //    && an.value.ToUpper() == _value.ToUpper());
        }

        public bool Any_Rule(string _rule_code, string _value)
        {
            return this._Model.t_wms_rule.Any(wh => wh.rule_code == _rule_code && wh.value == _value);
        }

        public string GetRuleValue(string _rule_code)
        {
            return this._Model.t_wms_rule.Where(wh => wh.rule_code == _rule_code).Select(se => se.value).FirstOrDefault();
        }

        public List<string> GetListByRuleCode(string _rule_code)
        {
            return this._Model.GetDataEntityListBy<string>(this, delegate ()
            {
                //Delegate Statemant ---
                var result = from rows in this._Model.t_wms_rule
                             where rows.is_active == "YES" && rows.rule_code == _rule_code
                             orderby rows.sequence
                             select rows.value;

                return result.ToList();
            });
        }


        public IQueryable<Property> GetQueryProperty(string _type)
        {
            return this._Model.GetDataBy(this, delegate ()
            {
                //Delegate Statemant ---
                var result = from rows in this._Model.t_wms_rule
                             where rows.is_active == "YES" && rows.type == _type
                             orderby rows.sequence
                             select new Property
                             {
                                 guid_member = rows.rule_id,
                                 display_member = rows.name
                             };

                return result;
            });
        }

        public IQueryable<Property> GetQueryUpperTextId(string _type)
        {
            var result = GetQueryProperty(_type);
            var result_cv = result.AsEnumerable().Select(se => new Property { guid_member = Guid.Empty, value_member = se.guid_member.ToString().ToUpper(), display_member = se.display_member }).AsQueryable();

            return result_cv;
        }

        public IQueryable<Property> GetQueryPropertyForSearchCode()
        {
            return this._Model.GetDataBy(this, delegate ()
            {
                //Delegate Statemant ---
                var result = from rows in this._Model.t_wms_rule
                             group rows by rows.rule_code into grb
                             orderby grb.Key
                             select new Property
                             {
                                 Code = grb.Key,
                                 Name = grb.Key
                             };

                return result;
            });
        }

        public IQueryable<Property> GetQueryPropertyForSearchType()
        {
            return this._Model.GetDataBy(this, delegate ()
            {
                //Delegate Statemant ---
                var result = from rows in this._Model.t_wms_rule
                             group rows by rows.type into grb
                             orderby grb.Key
                             select new Property
                             {
                                 Code = grb.Key,
                                 Name = grb.Key
                             };

                return result;
            });
        }


    }
}
