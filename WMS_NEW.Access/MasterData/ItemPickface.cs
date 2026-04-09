using Prototype.Providers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WMS_NEW.Source;

namespace WMS_NEW.Access.MasterData
{
    public class ItemPickface : AEntityFormCommand<t_wms_item_pickface>
    {
        #region ++INSTANCE STATIC++
        public static ItemPickface Instance
        {
            get
            {
                using (ItemPickface _Instance = new ItemPickface())
                {
                    return _Instance;
                }
            }
        }
        #endregion


        public WMSEntities _Model { get; set; }

        public ItemPickface()
        {
            _Model = new WMSEntities();

            base.GridObjContext = _Model;
            base.DbSetEntities = () => { return _Model.t_wms_item_pickface; };
        }
        #region Inherit AGridObjectSourceQuery

        public override IQueryable<dynamic> InitialQueryView()
        {
            string user_id = string.Empty;
            if (this.FilterCustom != null)
            {
                var entU = this.FilterCustom.FirstOrDefault(qry => qry.DataFieldValue == "_userID");
                if (entU?.Value != null)
                {
                    user_id = entU.Value.ToString().ToUpper().Trim();
                }
            }

            Guid wh_master_id = Guid.Empty;
            if (this.FilterCustom != null)
            {
                var entU = this.FilterCustom.FirstOrDefault(qry => qry.DataFieldValue == "_wh_master_id");
                if (entU?.Value != null)
                {
                    wh_master_id = Guid.Parse(entU.Value.ToString().ToUpper().Trim());
                }
            }

            Guid wh_item_master_id = Guid.Empty;
            if (this.FilterCustom != null)
            {
                var entU = this.FilterCustom.FirstOrDefault(qry => qry.DataFieldValue == "_wh_item_master_id");
                if (entU?.Value != null)
                {
                    wh_item_master_id = Guid.Parse(entU.Value.ToString().ToUpper().Trim());
                }
            }
            var pickface = from rows in _Model.t_wms_item_pickface
                           join whItem in _Model.t_wms_wh_item on rows.wh_item_master_id equals whItem.wh_item_master_id
                           where whItem.wh_master_id == wh_master_id && whItem.is_active == "YES"
                           select new { rows.location_id };
            
            var rule_loc_type = _Model.t_wms_rule.Where(x => x.rule_code == "LOC_TYPE_PICKFACE_SEQ" && x.is_active == "YES").Select(s => s.value).ToList();

            var result = from rows in this._Model.t_wms_location

                         join cboitem in this._Model.t_com_combobox_item
                         on rows.loc_type equals cboitem.value_member into CBOITEM
                         from leftCboItem in CBOITEM.DefaultIfEmpty()
                         where leftCboItem.group_name == "mst_location_type" && rule_loc_type.Contains(rows.loc_type) && rows.is_active == "YES"
                         && rows.t_wms_wh.t_wms_wh_user.Any(qry => qry.user_id.ToUpper().Trim() == user_id) && !rows.t_wms_item_pickface.Any(w => w.wh_item_master_id == wh_item_master_id)
                         && rows.wh_master_id == wh_master_id 
                         && !pickface.Any(w => w.location_id == rows.location_id)
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
            if (wh_item_master_id == Guid.Empty)
                return result.Where(x => false);

            return result;
        }
        public override IQueryable<dynamic> InitialQueryExport()
        {
            return this.InitialQueryView();
        }

        #endregion
        public IQueryable<Property> GetQuery_Item(Guid wh_master_id)
        {
            var result = from rows in _Model.t_wms_wh_item
                         join item in _Model.t_wms_item on rows.item_master_id equals item.item_master_id
                         where rows.is_active == "YES"
                         && rows.wh_master_id == wh_master_id
                         orderby rows.t_wms_wh.wh_id ascending
                         select new Property
                         {
                             guid_member = rows.wh_item_master_id,
                             display_member = item.item_number + " : " +  item.description
                         };

            return result;
        }
    }
}
