using Prototype.Providers;
using System;
using System.Linq;
using WMS_NEW.Source;
using System.Data.Entity;
using System.Data.Entity.SqlServer;

namespace WMS_NEW.Access.MasterData
{
    public class ItemBom : AEntityFormCommand<t_wms_item_bom>
    {

        #region ++INSTANCE STATIC++

        public static ItemBom Instance
        {
            get
            {
                using (ItemBom _Instance = new ItemBom())
                {
                    return _Instance;
                }
            }
        }

        #endregion


        public WMSEntities _Model { get; set; }

        public ItemBom()
        {
            this._Model = new WMSEntities();

            base.GridObjContext = _Model;
            base.DbSetEntities = () => { return _Model.t_wms_item_uom; };

        }

        public IQueryable<Property> GetPropertyAll(Guid _wh_master_id, Guid _owner_id)
        {             
            return this._Model.GetDataBy(this, delegate ()
            {
                var result = from rows in this._Model.t_wms_item_bom
                             where rows.wh_master_id == _wh_master_id && rows.owner_id == _owner_id && rows.is_active == "YES"

                             let bom_code = rows.is_bom_item == "YES" ? rows.item_number : rows.bom_raw_code

                             orderby bom_code
                             select new Property
                             {
                                 value_member = SqlFunctions.StringConvert((double?)rows.bom_id).Trim(),
                                 display_member = bom_code
                             };

                return result;
            });
        }

        public IQueryable<Property> GetPropertyByItem(Guid _wh_master_id, Guid _owner_id)
        {
            return this._Model.GetDataBy(this, delegate ()
            {
                var result = from rows in this._Model.t_wms_wh_item
                             let item = rows.t_wms_item
                             join bom in _Model.t_wms_item_bom on item.item_master_id equals bom.item_master_id
                             where rows.wh_master_id == _wh_master_id && bom.owner_id == _owner_id && bom.is_active == "YES" && bom.is_bom_item == "YES"
                             orderby item.item_number
                             select new Property
                             {
                                 Code = rows.wh_item_master_id + "|" + SqlFunctions.StringConvert((double?)bom.bom_id).Trim(),
                                 Name = item.item_number
                             };

                return result;
            });
        }

        #region Query Property
        public override IQueryable<dynamic> InitialQueryView()
        {
            throw new NotImplementedException();
        }

        public override IQueryable<dynamic> InitialQueryExport()
        {
            throw new NotImplementedException();
        }

        #endregion
    }
}
