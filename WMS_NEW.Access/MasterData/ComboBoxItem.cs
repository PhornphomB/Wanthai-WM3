using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Prototype.Providers;
using WMS_NEW.Source;

namespace WMS_NEW.Access.MasterData
{
    public class ComboBoxItem : IDisposable
    {

        public WMSEntities _Model { get; set; }

        public ComboBoxItem()
        {
            this._Model = new WMSEntities();
        }

        #region ++INSTANCE STATIC++
        public static ComboBoxItem Instance
        {
            get
            {
                using (ComboBoxItem _Instance = new ComboBoxItem())
                {
                    return _Instance;
                }
            }
        }
        #endregion

        public IQueryable<Property> GetQueryProperty(string _groupName)
        {
            return this._Model.GetDataBy(this, delegate()
            {
                var invStatus = from rows in this._Model.t_wms_rule
                                where rows.is_active == "YES" && rows.rule_code == "BLIND_PICK_ORDER_TYPE"
                                orderby rows.value
                                select rows.value;
                //Delegate Statemant ---
                var result = from rows in this._Model.t_com_combobox_item
                             where rows.is_active == "YES" && rows.group_name == _groupName && !invStatus.Contains(rows.value_member)
                             orderby rows.display_sequence
                             select new Property
                             {
                                 Code = rows.value_member,
                                 Name = rows.display_member
                             };

                return result;
            });
        }
        public IQueryable<Property> GetQueryPropertyAll(string _groupName)
        {
            return this._Model.GetDataBy(this, delegate ()
            {
                //Delegate Statemant ---
                var result = from rows in this._Model.t_com_combobox_item
                             where rows.is_active == "YES" && rows.group_name == _groupName
                             orderby rows.display_sequence
                             select new Property
                             {
                                 Code = rows.value_member,
                                 Name = rows.display_member
                             };

                return result;
            });
        }

        public IQueryable<Property> GetQueryProperty_Display(string _groupName)
        {
            return this._Model.GetDataBy(this, delegate()
            {
                //Delegate Statemant ---
                var result = from rows in this._Model.t_com_combobox_item
                             where rows.is_active == "YES" && rows.group_name == _groupName
                             orderby rows.display_sequence
                             select new Property
                             {
                                 Code = rows.display_member,
                                 Name = rows.display_member
                             };

                return result;
            });
        }

        public IQueryable<Property> GetQueryPropertyForSearchGroup()
        {
            return this._Model.GetDataBy(this, delegate()
            {
                //Delegate Statemant ---
                var result = from rows in this._Model.t_com_combobox_item
                             group rows by rows.group_name into grb
                             orderby grb.Key
                             select new Property
                             {
                                 Code = grb.Key,
                                 Name = grb.Key
                             };

                return result;
            });
        }

        #region IDisposable Members

        public void Dispose()
        {
            GC.SuppressFinalize(this);
        }

        #endregion
    }
}
