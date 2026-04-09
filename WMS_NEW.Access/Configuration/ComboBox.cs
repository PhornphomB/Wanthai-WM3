using Prototype.Providers;
using System;
using System.Linq;
using WMS_NEW.Source;

namespace WMS_NEW.Access.Configuration
{
    public class ComboBox : AEntityFormCommand<t_com_combobox_item>
    {
        #region ++INSTANCE STATIC++
        public static ComboBox Instance
        {
            get
            {
                using (ComboBox _Instance = new ComboBox())
                {
                    return _Instance;
                }
            }
        }
        #endregion

        protected WMSEntities _Model { get; set; }

        public ComboBox()
        {
            _Model = new WMSEntities();

            base.GridObjContext = _Model;
            base.DbSetEntities = () => { return _Model.t_com_combobox_item; };
        }

        public override bool ValidateSaveNew(t_com_combobox_item ent, ref string msg_validate)
        {
            if (_Model.t_com_combobox_item.Any(x => x.group_name == ent.group_name && x.value_member == ent.value_member))
            {
                msg_validate = "! Group and value has in system.";
                return false;
            }
            else
                return true;
        }


        public override void SetOptionalSaveNew(t_com_combobox_item ent)
        {
            ent.create_datetime = DateTime.Now;
        }


        #region Inherit AGridObjectSourceQuery

        public override IQueryable<dynamic> InitialQueryView()
        {
            var result = from rows in this._Model.t_com_combobox_item
                         select new
                         {
                             KeyId = rows.combobox_item_id,
                             rows.group_name,
                             rows.value_member,
                             rows.display_member,
                             rows.description,
                             rows.display_sequence,
                             rows.is_active,
                             rows.create_by,
                             rows.create_datetime,
                             rows.update_by,
                             rows.update_date,
                         };


            return result;
        }
        public override IQueryable<dynamic> InitialQueryExport()
        {
            return this.InitialQueryView();
        }

        #endregion


        #region Query Property
        public IQueryable<Property> GetQuery(string _group_name)
        {
            var result = from rows in _Model.t_com_combobox_item
                         where rows.is_active == "YES"
                         && rows.group_name == _group_name
                         orderby rows.display_sequence, rows.group_name, rows.value_member ascending
                         select new Property
                         {
                             guid_member = rows.combobox_item_id,
                             display_member = rows.display_member
                         };

            return result;
        }

        public IQueryable<Property> GetQueryCode(string _group_name)
        {
            var result = from rows in _Model.t_com_combobox_item
                         where rows.is_active == "YES"
                         && rows.group_name == _group_name
                         orderby rows.display_sequence, rows.group_name, rows.value_member ascending
                         select new Property
                         {
                             value_member = rows.value_member,
                             display_member = rows.display_member
                         };

            return result;
        }

        public IQueryable<Property> GetQuery_Locale()
        {
            var result = from rows in _Model.t_com_locale
                         where rows.is_active == "YES"
                         orderby rows.locale ascending
                         select new Property
                         {
                             value_member = rows.locale_id,
                             display_member = rows.name
                         };

            return result;
        }

        public IQueryable<Property> GetQuery_Print()
        {
            var result = from rows in _Model.t_com_config_printer
                         where rows.is_active == "YES"
                         orderby rows.printer_name ascending
                         select new Property
                         {
                             guid_member = rows.printer_id,
                             display_member = rows.printer_name
                         };

            return result;
        }

        public IQueryable<Property> GetQuery_PrintGroup()
        {
            var result = from rows in _Model.t_com_config_printer_group
                         where rows.is_active == "YES"
                         orderby rows.group_name ascending
                         select new Property
                         {
                             guid_member = rows.group_id,
                             display_member = rows.group_name
                         };

            return result;
        }

        public IQueryable<Property> GetQueryRule(string _rule_code) {
            var result = from rows in _Model.t_wms_rule
                         where rows.is_active == "YES"
                         && rows.rule_code == _rule_code
                         orderby rows.sequence ascending
                         select new Property {
                             value_member = rows.value,
                             display_member = rows.value
                         };

            return result;
        }
        #endregion

    }
}
