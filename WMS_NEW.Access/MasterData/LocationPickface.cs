using Prototype.Providers;
using System;
using System.Collections.Generic;
using System.Linq;
using WMS_NEW.Source;

namespace WMS_NEW.Access.MasterData
{
    public class LocationPickface : AGridObjectSourceQuery
    {
        #region ++INSTANCE STATIC++
        public static LocationPickface Instance
        {
            get
            {
                using (LocationPickface _Instance = new LocationPickface())
                {
                    return _Instance;
                }
            }
        }
        #endregion

        protected WMSEntities _Model { get; set; }

        public LocationPickface()
        {
            this._Model = new WMSEntities();
            this._Model.Configuration.EnsureTransactionsForFunctionsAndCommands = false;

        }

        #region Inherit AGridObjectSourceQuery

        public override IQueryable<dynamic> InitialQueryView()
        {
            string user_id = string.Empty;
            if (this.FilterCustom != null)
            {
                var entU = this.FilterCustom.FirstOrDefault(qry => qry.DataFieldValue == "_userID");
                if (entU != null)
                {
                    user_id = entU.Value.ToString().ToUpper().Trim();
                }
            }

            Guid wh_item_master_id = Guid.Empty;
            if (this.FilterCustom != null)
            {
                var entU = this.FilterCustom.FirstOrDefault(qry => qry.DataFieldValue == "_wh_item_master_id");
                if (entU != null)
                {
                    wh_item_master_id = Guid.Parse(entU.Value.ToString().ToUpper().Trim());
                }
            }

            var result = from rows in this._Model.t_wms_location

                         join cboitem in this._Model.t_com_combobox_item
                         on rows.loc_type equals cboitem.value_member into CBOITEM
                         from leftCboItem in CBOITEM.DefaultIfEmpty()
                         where leftCboItem.group_name == "mst_location_type" && rows.loc_type == "PICKFACE" && rows.is_active == "YES"
                         && rows.t_wms_wh.t_wms_wh_user.Any(qry => qry.user_id.ToUpper().Trim() == user_id)  && !rows.t_wms_item_pickface.Any(w=>w.wh_item_master_id == wh_item_master_id)
                         select new
                         {
                             KeyId = rows.location_id,
                             rows.t_wms_wh.wh_id,
                             rows.wh_master_id,
                             rows.location,
                             rows.loc_type,
                             loc_type_name = leftCboItem.display_member,
                             rows.description,
                             rows.capacity_qty,
                             rows.is_active,
                             rows.create_by,
                             rows.create_date,
                             rows.x_cordinate,
                             rows.y_cordinate
                         };

            return result;
        }
        public override IQueryable<dynamic> InitialQueryExport()
        {
            return this.InitialQueryView();
        }

        #endregion

    }

    public class LocationPickfaceItem : AGridObjectSourceQuery
    {
        #region ++INSTANCE STATIC++
        public static LocationPickfaceItem Instance
        {
            get
            {
                using (LocationPickfaceItem _Instance = new LocationPickfaceItem())
                {
                    return _Instance;
                }
            }
        }
        #endregion

        protected WMSEntities _Model { get; set; }

        public LocationPickfaceItem()
        {
            this._Model = new WMSEntities();
            this._Model.Configuration.EnsureTransactionsForFunctionsAndCommands = false;

        }

        #region Inherit AGridObjectSourceQuery

        public override IQueryable<dynamic> InitialQueryView()
        {
            string user_id = string.Empty;
            if (this.FilterCustom != null)
            {
                var entU = this.FilterCustom.FirstOrDefault(qry => qry.DataFieldValue == "_userID");
                if (entU != null)
                {
                    user_id = entU.Value.ToString().ToUpper().Trim();
                }
            }

            Guid wh_item_master_id = Guid.Empty;
            if (this.FilterCustom != null)
            {
                var entU = this.FilterCustom.FirstOrDefault(qry => qry.DataFieldValue == "_wh_item_master_id");
                if (entU != null)
                {
                    wh_item_master_id = Guid.Parse(entU.Value.ToString().ToUpper().Trim());
                }
            }

            var result = from rows in this._Model.t_wms_item_pickface

                             // join cboitem in this._Model.t_com_combobox_item
                             //  on rows.loc_type equals cboitem.value_member into CBOITEM
                             //  from leftCboItem in CBOITEM.DefaultIfEmpty()

                         where rows.t_wms_location.t_wms_wh.t_wms_wh_user.Any(qry => qry.user_id.ToUpper().Trim() == user_id)
                         && rows.wh_item_master_id == wh_item_master_id
                         //leftCboItem.group_name == "mst_location_type" && rows.loc_type == "PICKFACE" && rows.is_active == "YES"   && 
                         // rows.t_wms_wh.t_wms_wh_user.Any(qry => qry.user_id.ToUpper().Trim() == user_id)
                         select new
                         {
                             KeyId = rows.item_pickface_id,
                             rows.location_id,
                             rows.t_wms_location.t_wms_wh.wh_id,
                             rows.t_wms_location.wh_master_id,
                             rows.t_wms_location.location,
                             //rows.loc_type,
                             //loc_type_name = leftCboItem.display_member,
                             //rows.description,
                             //rows.capacity_qty,
                             //rows.is_active,
                             rows.max,
                             rows.min,
                             rows.create_by,
                             rows.create_date,
                             //rows.x_cordinate,
                             //rows.y_cordinate
                         };

            return result;
        }
        public override IQueryable<dynamic> InitialQueryExport()
        {
            return this.InitialQueryView();
        }

        public double UpdateQty(Guid _item_pickface_id, double _qty,string _type)
        {
            var ent = this._Model.t_wms_item_pickface.First(qry => qry.item_pickface_id == _item_pickface_id);

            return this._Model.GetDataBy(this, delegate ()
            {
                if (_type == "Max")
                {
                    ent.max = _qty;
                    this._Model.SaveChanges();
                    return _qty;
                }
                else
                {
                    ent.min = _qty;
                    this._Model.SaveChanges();
                    return _qty;
                }
                 
            });
        }
        #endregion

    }
}
