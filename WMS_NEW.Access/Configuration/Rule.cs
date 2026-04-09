using Prototype.Providers;
using System;
using System.Linq;
using WMS_NEW.Source;

namespace WMS_NEW.Access.Configuration
{
    public class Rule : AEntityFormCommand<t_wms_rule>
    {
        protected WMSEntities _Model { get; set; }

        public Rule()
        {
            _Model = new WMSEntities();

            base.GridObjContext = _Model;
            base.DbSetEntities = () => { return _Model.t_wms_rule; };
        }

        public override bool ValidateSaveNew(t_wms_rule ent, ref string msg_validate)
        {
            if (_Model.t_wms_rule.Any(x => x.rule_code == ent.rule_code && x.value == ent.value))
            {
                msg_validate = "! Rule code and value has in system.";
                return false;
            }
            else
                return true;
        }

        public override void SetOptionalSaveNew(t_wms_rule ent)
        {
            ent.create_date = DateTime.Now;
        }


        #region Inherit AGridObjectSourceQuery

        public override IQueryable<dynamic> InitialQueryView()
        {
            var result = from rows in this._Model.t_wms_rule
                         select new
                         {
                             KeyId = rows.rule_id,
                             rows.rule_code,
                             rows.type,
                             rows.name,
                             rows.condition,
                             _operator = rows.@operator,
                             rows.value,
                             rows.sequence,
                             rows.process_name,
                             rows.tran_type,
                             rows.is_active,
                             rows.create_by
                         };


            return result;
        }
        public override IQueryable<dynamic> InitialQueryExport()
        {
            return this.InitialQueryView();
        }

        #endregion

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

        #region Query Property
        public IQueryable<Property> GetQueryCode(string _rule_code)
        {
            var result = from rows in _Model.t_wms_rule
                         where rows.is_active == "YES"
                         && rows.rule_code == _rule_code
                         orderby rows.sequence ascending
                         select new Property
                         {
                             value_member = rows.value,
                             display_member = rows.value 
                         };

            return result;
        }
        #endregion

    }
}
