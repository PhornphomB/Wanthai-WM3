using Prototype.Providers;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Data.Entity.SqlServer;
using System.Linq;
using WMS_NEW.Source;


namespace WMS_NEW.Access.Transaction.Inventory.InventoryViewer
{
    [Serializable()]
    public class InventoryDataKey
    {
        public Guid key_inventory_id { get; set; }
        public Guid key_attribute_group_id { get; set; }
    }

    [Serializable()]
    public class InventoryDataDto : InventoryDataKey
    {
        public string wh_id { get; set; }
        public string owner_code { get; set; }
        public string zone { get; set; }
        public string location { get; set; }
        public string item_number { get; set; }
        public string description { get; set; }
        public string lpn { get; set; }
        public string inv_status { get; set; }
        public string lot_number { get; set; }
        public DateTime? mfg_date { get; set; }
        public string expiry_date { get; set; }
        public double? quantity { get; set; }
        public double? adjust_quantity { get; set; }
        public string quantity_allocated { get; set; }
        public string uom { get; set; }
        public string parent_lpn { get; set; }
        public string dg_code { get; set; }
        public string serial_number { get; set; }
        public string grade { get; set; }
        public string item_category { get; set; }
        public double? price { get; set; }
        public DateTime? receive_date { get; set; }
        public Guid wh_master_id { get; set; }
        public Guid item_master_id { get; set; }
        public Guid category_id { get; set; }
        public Guid owner_id { get; set; }
        public string alternate_item_number { get; set; }

    }


    public class InventoryItem : AGridObjectSourceQuery, IDisposable
    {
        public WMSEntities _Model { get; set; }

        public InventoryItem()
        {
            this._Model = new WMSEntities();
            this._Model.Database.CommandTimeout = 180;

            //AGridObjectSourceQuery Map Model
            base.GridObjContext = _Model;
        }


        #region ++INSTANCE STATIC++
        public static InventoryItem Instance
        {
            get
            {
                using (InventoryItem _Instance = new InventoryItem())
                {
                    return _Instance;
                }
            }
        }
        #endregion


        #region ++ QUERY DATA

        public v_wms_inventory_data Get_InventoryByKeyID(InventoryDataKey KeyId)
        {
            return this._Model.GetDataBy(this, delegate ()
            {
                var result = (from tb in this._Model.v_wms_inventory_data
                              where KeyId.key_inventory_id == tb.inventory_id && KeyId.key_attribute_group_id == tb.attribute_group_id
                              select tb).FirstOrDefault();

                return result;
            });
        }

        public List<string> Get_LocationTypeByKeyID(IEnumerable<InventoryDataKey> listKeyId)
        {
            var result = from tb in _Model.v_wms_inventory_data
                         where listKeyId.Any(x => x.key_inventory_id == tb.inventory_id && x.key_attribute_group_id == tb.attribute_group_id)
                         select tb.loc_type;

            return result.ToList();

        }

        public List<InventoryDataDto> Get_InventoryByKeyID(IEnumerable<InventoryDataKey> listKeyId)
        {
            var result = (from rows in _Model.v_wms_inventory_data
                          where listKeyId.Any(x => x.key_inventory_id == rows.inventory_id && x.key_attribute_group_id == rows.attribute_group_id)
                          select new InventoryDataDto
                          {
                              key_inventory_id = rows.inventory_id,
                              key_attribute_group_id = rows.attribute_group_id,
                              wh_id = rows.wh_id,
                              owner_code = rows.owner_code,
                              zone = rows.zone,
                              location = rows.location,
                              item_number = rows.item_number,
                              description = rows.description,
                              lpn = rows.lpn,
                              inv_status = rows.inv_status,
                              lot_number = rows.lot_number,
                              mfg_date = rows.mfg_date,
                              expiry_date = rows.expiry_date,
                              quantity = rows.quantity,
                              adjust_quantity = 0,
                              uom = rows.uom,
                              parent_lpn = rows.parent_lpn,
                              dg_code = rows.dg_code,
                              serial_number = rows.serial_number,
                              grade = rows.grade,
                              item_category = rows.item_category,
                              price = rows.price,
                              receive_date = rows.receive_date,

                              wh_master_id = rows.wh_master_id,
                              item_master_id = rows.item_master_id,
                              category_id = rows.category_id,
                              owner_id = rows.owner_id,

                              alternate_item_number = rows.alternate_item_number
                          });

            return result.ToList(); ;
        }

        public v_wms_inventory_data Get_InventoryByKeyID(Guid _inventory_id)
        {
            return this._Model.GetDataBy(this, delegate ()
            {
                var result = (from tb in this._Model.v_wms_inventory_data
                              where tb.inventory_id == _inventory_id
                              select tb).FirstOrDefault();

                return result;
            });
        }

        #endregion


        #region Inherit AGridObjectSourceQuery

        public override IQueryable<dynamic> InitialQueryView()
        {
            int iBeforeExp = 0;
            var entRule = this._Model.t_wms_rule.Where(w => w.rule_code.ToUpper() == "COLOR_TAB_BEFORE_EXPIRE" && w.is_active == "YES").FirstOrDefault();
            if (entRule != null && entRule.value != null)
            {
                iBeforeExp = Convert.ToInt32(entRule.value);
            }

            var _active_load = this.FilterCustom.FirstOrDefault(wh => wh.DataFieldValue == "_active_load");

            DateTime min_date = new DateTime(1753, 1, 7);

            var listWh = this._Model.t_wms_wh_user.Where(w => w.user_id == _SessionVals.UserName).Select(s => s.wh_master_id);
            var listOwn = this._Model.t_wms_owner_user.Where(w => w.user_id == _SessionVals.UserName).Select(s => s.owner_id);

            DateTime currentDate = DateTime.Now;

            var query = from rows in this._Model.v_wms_inventory_data
                         where listWh.Contains(rows.wh_master_id)
                         && listOwn.Contains(rows.owner_id)

                         let _minDate = DbFunctions.CreateDateTime(0001, 1, 1, 0, 0, 0)
                         let _year = SqlFunctions.DatePart("year", (DbFunctions.Left(rows.expiry_date, 4).Trim()))
                         let _month = SqlFunctions.DatePart("year", "00" + rows.expiry_date.Substring(4, 2).Trim())
                         let _day = SqlFunctions.DatePart("year", "00" + rows.expiry_date.Substring(6, 2).Trim())
                         let expiry_date = !string.IsNullOrEmpty(rows.expiry_date)
                         ? SqlFunctions.DateAdd("day", _day - 1,
                               SqlFunctions.DateAdd("month", _month - 1,
                                   SqlFunctions.DateAdd("year", _year - 1, _minDate)))
                         : min_date
                        let daysUntilExpiry = SqlFunctions.DateDiff("day", currentDate, rows.expiry_date)
                        select new
                         {
                             KeyId = rows.inventory_id,
                             cmd_reprint = "",
                             rows.wh_master_id,
                             rows.wh_id,
                             rows.owner_code,
                             rows.zone,
                             rows.location,
                             rows.item_number,
                             rows.description,
                             rows.lpn,
                             rows.parent_lpn,
                             rows.inv_status,
                             rows.lot_number,
                             expiry_date = expiry_date == min_date ? null : expiry_date,
                             rows.quantity,
                             rows.quantity_allocated,
                             quantity_avalible = rows.quantity - rows.quantity_allocated,
                             rows.uom,
                             rows.dg_code,
                             rows.serial_number,
                             rows.grade,
                             rows.item_category,
                             rows.price,
                             rows.receive_date,
                             rows.item_master_id,
                             rows.category_id,
                             rows.cate_description,
                             rows.owner_id,
                             rows.days_to_expire,
                             //#ทำเป็น Temp หลอก เพื่อให้ทุกหน้าจอ ค้นหาได้
                             delivery_date = min_date,
                             delivery_number = "",
                             receipt_date = DbFunctions.TruncateTime(rows.receive_date),
                             //#Custom
                             rows.alternate_item_number,
                             rows.mfg_date,
                            //isBeforeExp = SqlFunctions.DateDiff("dd", DateTime.Now, rows.exp_date) <= iBeforeExp ? true : false,
                            ////isExp = SqlFunctions.DateDiff("dd", rows.exp_date, current) >= 0 ? true : false
                            isBeforeExp = (daysUntilExpiry > 0 && daysUntilExpiry <= 30) ? true : false,
                            isExp = daysUntilExpiry <= 0 ? true : false,
                            rows.attribute1
                            //Color = daysUntilExpiry >= 30 ? ConsoleColor.Red :
                            //        daysUntilExpiry >= 0 ? ConsoleColor.Yellow :
                            //        ConsoleColor.Gray
                        };

            

            if (_active_load == null)
            {
                query = query.Where(wh => false);
            }
            return query;
        }
        public override IQueryable<dynamic> InitialQueryExport()
        {
            return this.InitialQueryView();
        }

        #endregion
    }
}
