using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;

using System.Data.Entity;
using System.Data.Entity.SqlServer;

using Prototype.Providers;
using WMS_NEW.Source;

namespace WMS_NEW.Access.Transaction.Count
{
    public class CycleCountTempDto
    {
        public Guid location_id { get; set; }
        public Guid wh_item_master_id { get; set; }
        public string inv_status { get; set; }
        public string parent_lpn { get; set; }
        public string lpn { get; set; }
        public string lot { get; set; }
        public string expiry_date { get; set; }
        public string serial_number { get; set; }
        public double? stock_qty { get; set; }

        public string attribute1 { get; set; }
        public string attribute2 { get; set; }
        public string attribute3 { get; set; }
        public string attribute4 { get; set; }
        public string attribute5 { get; set; }
    }

    public class CycleCountTemp : AGridObjectSourceQuery, IDisposable
    {
        public WMSEntities _Model { get; set; }

        public CycleCountTemp()
        {
            this._Model = new WMSEntities();

            base.GridObjContext = _Model;
        }


        #region ++INSTANCE STATIC++
        public static CycleCountTemp Instance
        {
            get
            {
                using (CycleCountTemp _Instance = new CycleCountTemp())
                {
                    return _Instance;
                }
            }
        }
        #endregion


        #region Access Command Data

        public bool Save(IQueryable<CycleCountTempDto> _query)
        {
            var temp = from rows in this._Model.t_wms_count_temporary
                       where rows.create_by == _SessionVals.UserName
                       select new CycleCountTempDto
                       {
                           location_id = rows.location_id,
                           wh_item_master_id = rows.wh_item_master_id,
                           inv_status = rows.inv_status,
                           parent_lpn = rows.parent_lpn,
                           lpn = rows.lpn,
                           lot = rows.lot,
                           expiry_date = rows.expiry_date,
                           serial_number = rows.serial_number,
                           stock_qty = rows.stock_qty,
                           attribute1 = rows.attribute1,
                           attribute2 = rows.attribute2,
                           attribute3 = rows.attribute3,
                           attribute4 = rows.attribute4,
                           attribute5 = rows.attribute5,
                       };

            var items = _query.Except(temp).ToList();

            if (items.Count() == 0)
            {
                return !this._Model.Existed(this, delegate ()
                {
                    return true;
                }, "!Not found data, Please find again.");
            }

            return this._Model.Save(this, delegate ()
            {
                var dateNow = DateTime.Now;
                t_wms_count_temporary ent;
                foreach (var item in items)
                {
                    ent = new t_wms_count_temporary();
                    ent.CloneObject(item);

                    ent.running_id = Guid.NewGuid();
                    ent.create_by = _SessionVals.UserName;
                    ent.create_date = dateNow;
                    ent.attribute1 = item.attribute1;
                    ent.attribute2 = item.attribute2;
                    ent.attribute3 = item.attribute3;
                    ent.attribute4 = item.attribute4;
                    ent.attribute5 = item.attribute5;

                    this._Model.t_wms_count_temporary.Add(ent);
                }

                return this._Model.SaveChanges();
            });
        }

        public bool AddTempByInventory(FilterList _filters, Guid _wh_master_id, Guid _owner_id)
        {
            return this._Model.GetDataBy(this, delegate ()
            {

                var rules = from rows in this._Model.t_wms_rule
                            where rows.is_active == "YES" && rows.rule_code == "RULE_LOCATION_TYPE_FOR_COUNT"
                            select rows.value;

                var result = from rows in this._Model.v_wms_inventory_data_by_serial
                             where rows.wh_master_id == _wh_master_id && rules.Contains(rows.loc_type)
                             && rows.owner_id == _owner_id
                             select new
                             {
                                 rows.item_number,
                                 rows.category_id,
                                 rows.location,
                                 rows.cost,
                                 rows.price,

                                 rows.location_id,
                                 rows.wh_item_master_id,
                                 rows.inv_status,
                                 rows.parent_lpn,
                                 rows.lpn,
                                 lot = rows.lot_number,
                                 rows.expiry_date,
                                 rows.serial_number,
                                 stock_qty = rows.quantity,
                                 rows.attribute1,
                                 rows.attribute2,
                                 rows.attribute3,
                                 rows.attribute4,
                                 rows.attribute5,

                                 rows.mfg_date,
                             };

                result = DataFilter.GetQuery(result, _filters);

                var _query = from rows in result
                             select new CycleCountTempDto
                             {
                                 location_id = rows.location_id,
                                 wh_item_master_id = rows.wh_item_master_id,
                                 inv_status = rows.inv_status,
                                 parent_lpn = rows.parent_lpn,
                                 lpn = rows.lpn,
                                 lot = rows.lot,
                                 expiry_date = rows.expiry_date,
                                 serial_number = rows.serial_number,
                                 stock_qty = rows.stock_qty,
                                 attribute1 = rows.attribute1,
                                 attribute2 = rows.attribute2,
                                 attribute3 = rows.attribute3,
                                 attribute4 = rows.attribute4,
                                 attribute5 = rows.attribute5,
                             };

                return this.Save(_query);
            });
        }

        public int CountTempByUser(string _userId)
        {
            return this._Model.GetDataBy(this, delegate ()
            {
                return this._Model.t_wms_count_temporary.Count(qry => qry.create_by == _userId);
            });
        }

        public void DeleteTempByUser(string _userId)
        {
            this._Model.usp_com_delete_count_temp_byuser(_userId);
        }
        public bool DeleteByDefault(Guid location_id, Guid wh_item_master_id)
        {
            return this._Model.Update(this, delegate ()
            {
                var result = this._Model.t_wms_count_temporary.Where(qry => qry.location_id == location_id && qry.wh_item_master_id == wh_item_master_id);
                foreach (var item in result)
                {
                    this._Model.t_wms_count_temporary.Remove(item);
                }

                return this._Model.SaveChanges();
            });
        }
        public bool DeleteByNormal(Guid location_id, Guid wh_item_master_id, string parent_lpn, string lpn)
        {
            if (!string.IsNullOrEmpty(parent_lpn) && !string.IsNullOrEmpty(lpn))
            {
                return this._Model.Update(this, delegate ()
                {
                    var result = this._Model.t_wms_count_temporary.Where(qry => qry.location_id == location_id && qry.wh_item_master_id == wh_item_master_id
                        && qry.parent_lpn == parent_lpn && qry.lpn == lpn);
                    foreach (var item in result)
                    {
                        this._Model.t_wms_count_temporary.Remove(item);
                    }

                    return this._Model.SaveChanges();
                });
            }
            else if (!string.IsNullOrEmpty(parent_lpn))
            {
                return this.DeleteByParentLPN(location_id, wh_item_master_id, parent_lpn);
            }
            else if (!string.IsNullOrEmpty(lpn))
            {
                return this.DeleteByLPN(location_id, wh_item_master_id, lpn);
            }
            else
            {
                return this.DeleteByDefault(location_id, wh_item_master_id);
            }
        }

        public bool DeleteByParentLPN(Guid location_id, Guid wh_item_master_id, string parent_lpn)
        {
            if (!string.IsNullOrEmpty(parent_lpn))
            {
                return this._Model.Update(this, delegate ()
                {
                    var result = this._Model.t_wms_count_temporary.Where(qry => qry.location_id == location_id && qry.wh_item_master_id == wh_item_master_id
                        && qry.parent_lpn == parent_lpn);
                    foreach (var item in result)
                    {
                        this._Model.t_wms_count_temporary.Remove(item);
                    }

                    return this._Model.SaveChanges();
                });
            }
            else
            {
                return this.DeleteByDefault(location_id, wh_item_master_id);
            }
        }
        public bool DeleteByLPN(Guid location_id, Guid wh_item_master_id, string lpn)
        {
            if (!string.IsNullOrEmpty(lpn))
            {
                return this._Model.Update(this, delegate ()
                {
                    var result = this._Model.t_wms_count_temporary.Where(qry => qry.location_id == location_id && qry.wh_item_master_id == wh_item_master_id
                        && qry.lpn == lpn);
                    foreach (var item in result)
                    {
                        this._Model.t_wms_count_temporary.Remove(item);
                    }

                    return this._Model.SaveChanges();
                });
            }
            else
            {
                return this.DeleteByDefault(location_id, wh_item_master_id);
            }
        }

        public dynamic GetDeleteDtoById(Guid _id)
        {
            return this._Model.GetDataBy(this, delegate ()
            {
                var result = (from rows in this._Model.t_wms_count_temporary
                              where rows.running_id == _id
                              select new
                              {
                                  rows.location_id,
                                  rows.wh_item_master_id,
                                  rows.parent_lpn,
                                  rows.lpn
                              }).First();

                dynamic data = new System.Dynamic.ExpandoObject();
                data.location_id = result.location_id;
                data.wh_item_master_id = result.wh_item_master_id;
                data.parent_lpn = result.parent_lpn;
                data.lpn = result.lpn;

                return data;
            });
        }

        #endregion


        #region Inherit AGridObjectSourceQuery

        public override IQueryable<dynamic> InitialQueryView()
        {
            var _userID = (string)this.FilterCustom.FirstOrDefault(qry => qry.DataFieldValue == "_userID").Value;

            DateTime min_date = new DateTime(1753, 1, 7);

            var result = from rows in this._Model.t_wms_count_temporary
                         where rows.create_by == _userID
                         join wh_item in this._Model.t_wms_wh_item on rows.wh_item_master_id equals wh_item.wh_item_master_id
                         join location in this._Model.t_wms_location on rows.location_id equals location.location_id

                         let item = wh_item.t_wms_item

                         let _minDate = DbFunctions.CreateDateTime(0001, 1, 1, 0, 0, 0)
                         let _year = SqlFunctions.DatePart("year", (DbFunctions.Left(rows.expiry_date, 4).Trim()))
                         let _month = SqlFunctions.DatePart("year", "00" + rows.expiry_date.Substring(4, 2).Trim())
                         let _day = SqlFunctions.DatePart("year", "00" + rows.expiry_date.Substring(6, 2).Trim())
                         let expiry_date = !string.IsNullOrEmpty(rows.expiry_date)
                         ? SqlFunctions.DateAdd("day", _day - 1,
                               SqlFunctions.DateAdd("month", _month - 1,
                                   SqlFunctions.DateAdd("year", _year - 1, _minDate)))
                         : min_date

                         select new
                         {
                             KeyId = rows.running_id,
                             location.location,
                             item.item_number,
                             item.description,
                             rows.parent_lpn,
                             rows.lpn,
                             rows.lot,
                             rows.serial_number,
                             expiry_date = expiry_date == min_date ? null : expiry_date,
                             rows.stock_qty
                         };
            return result;
        }
        public override IQueryable<dynamic> InitialQueryExport()
        {
            return this.InitialQueryView();
        }

        #endregion


    }
}

