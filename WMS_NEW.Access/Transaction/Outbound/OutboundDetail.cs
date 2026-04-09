using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;

using System.Data.Entity;
using System.Data.Entity.SqlServer;

using Prototype.Providers;
using WMS_NEW.Source;

namespace WMS_NEW.Access.Transaction.Outbound
{
    public class OutboundDetail : AEntityFormCommand<t_wms_outbound_detail>
    {
        #region ++INSTANCE STATIC++
        public static OutboundDetail Instance
        {
            get
            {
                using (OutboundDetail _Instance = new OutboundDetail())
                {
                    return _Instance;
                }
            }
        }
        #endregion


        public WMSEntities _Model { get; set; }

        public OutboundDetail()
        {
            this._Model = new WMSEntities();

            base.GridObjContext = _Model;
            base.DbSetEntities = () => { return _Model.t_wms_outbound_detail; };
        }

        public override bool Save()
        {
            var msg_validate = string.Empty;
            var is_valid = ValidateSaveNew(Entity, ref msg_validate);

            if (this.GridObjContext.Engaged(this, () => { return !is_valid; }, msg_validate))
                return false;

            return this.GridObjContext.Save(this, delegate ()
            {
                if (Entity.bom_id != null)
                {
                    SetOptionalSaveNew(Entity);
                }
                else
                {
                    var keys = this.GridObjContext.GetEntityKeys(Entity.GetType());
                    foreach (var key in keys)
                    {
                        if (key.TypeName.ToLower() == "guid")
                        {
                            Entity.SetPropertyValue(key.Name, Guid.NewGuid());
                        }
                    }

                    this.GridObjContext.Entry(Entity).State = System.Data.Entity.EntityState.Added;
                }

                return this.GridObjContext.SaveChanges();
            });
        }

        public override bool ValidateSaveNew(t_wms_outbound_detail ent, ref string msg_validate)
        {
            if (ent.bom_id == null)
            {

                var item = DataPrimaryField.Instance.GetMasterItem(ent.wh_item_master_id);
                ent.item_master_id = item.item_master_id;
                ent.item_number = item.item_number;

                ent.line_number = GetLineNumber(ent.outbound_order_master_id);
                ent.item_status = "NEW";

                ent.quantity_load = 0;
                ent.quantity_pick = 0;
                ent.quantity_ship = 0;
            }
            return true;
        }

        public override void SetOptionalSaveNew(t_wms_outbound_detail ent)
        {
            if (ent.bom_id == null)
            {
                var item = DataPrimaryField.Instance.GetMasterItem(ent.wh_item_master_id);
                ent.item_master_id = item.item_master_id;
                ent.item_number = item.item_number;

                ent.line_number = GetLineNumber(ent.outbound_order_master_id);
                ent.item_status = "NEW";

                ent.quantity_load = 0;
                ent.quantity_pick = 0;
                ent.quantity_ship = 0;
            }
            else
            {
                #region New By Bom

                var bom = _Model.t_wms_item_bom
                            .Select(se => new { se.bom_id, se.item_number, se.bom_raw_code, se.is_bom_item, se.wh_item_master_id, se.item_master_id })
                            .First(x => x.bom_id == ent.bom_id);

                //New 188
                var ent_bom = new t_wms_outbound_group_bom();
                ent_bom.outbound_order_bom_id = Guid.NewGuid();
                ent_bom.outbound_order_master_id = ent.outbound_order_master_id;
                ent_bom.create_by = ent.create_by;
                ent_bom.create_date = ent.create_date;
                ent_bom.bom_id = bom.bom_id;
                ent_bom.wh_item_master_id = bom.wh_item_master_id;
                ent_bom.item_master_id = bom.item_master_id;
                ent_bom.item_number = bom.item_number;
                ent_bom.bom_raw_code = bom.bom_raw_code;
                ent_bom.quantity = ent.quantity_order;

                this._Model.t_wms_outbound_group_bom.Add(ent_bom);
                //......

                int line_number = ent.line_number.ToInteger();
                t_wms_outbound_detail detail;

                foreach (var bom_de in _Model.t_wms_item_bom_detail.Where(x => x.bom_id == bom.bom_id))
                {
                    detail = new t_wms_outbound_detail();
                    detail.line_number = line_number.ToString("000000");
                    detail.default_item_status = ent.default_item_status;
                    detail.item_status = "NEW";
                    //detail.item_status = ent.item_status;
                    detail.quantity_ship = 0;
                    detail.quantity_pick = 0;
                    detail.quantity_load = 0;

                    //detail.lot_number = ent.lot_number;
                    //detail.expiry_date = ent.expiry_date;
                    //detail.user_def1 = ent.user_def1;
                    //detail.user_def2 = ent.user_def2;
                    //detail.user_def3 = ent.user_def3;
                    //detail.user_def4 = ent.user_def4;
                    //detail.user_def5 = ent.user_def5;
                    //detail.user_def6 = ent.user_def6;
                    //detail.user_def7 = ent.user_def7;
                    //detail.user_def8 = ent.user_def8;
                    //detail.user_def9 = ent.user_def9;
                    //detail.user_def10 = ent.user_def10;

                    detail.outbound_order_detail_id = Guid.NewGuid();
                    detail.outbound_order_master_id = ent.outbound_order_master_id;
                    detail.create_by = ent.create_by;
                    detail.create_date = ent.create_date;

                    detail.quantity_order = (bom_de.quantity.Value * ent.quantity_order);
                    detail.wh_item_master_id = bom_de.wh_item_master_id;
                    detail.item_master_id = bom_de.item_master_id;
                    detail.item_number = bom_de.item_number;
                    detail.item_uom_id = bom_de.item_uom_id;
                    detail.uom = bom_de.uom;
                    detail.bom_detail_id = bom_de.bom_detail_id;

                    detail.bom_id = bom.bom_id;
                    detail.bom_wh_item_master_id = bom.wh_item_master_id;
                    detail.bom_item_master_id = bom.item_master_id;
                    detail.bom_item_number = bom.is_bom_item == "YES" ? bom.item_number : bom.bom_raw_code;

                    //New 188
                    detail.outbound_order_bom_id = ent_bom.outbound_order_bom_id;
                    detail.bom_detail_quantity = bom_de.quantity;
                    //......

                    this._Model.t_wms_outbound_detail.Add(detail);

                    line_number++;
                }

                #endregion
            }

        }

        public override void SetOptionalSaveUpdate(t_wms_outbound_detail ent)
        {
            var item = DataPrimaryField.Instance.GetMasterItem(ent.wh_item_master_id);
            ent.item_master_id = item.item_master_id;
            ent.item_number = item.item_number;
            //ent.item_uom_id = item.ite
        }

        public string GetLineNumber(Guid _outbound_order_master_id)
        {
            var count = this._Model.GetDataBy(this, delegate ()
            {
                var result = this._Model.t_wms_outbound_detail.Where(wh => wh.outbound_order_master_id == _outbound_order_master_id);
                if (result.Any())
                {
                    return Convert.ToInt32(result.Max(max => max.line_number));
                }
                else
                {
                    return 0;
                }
            });

            var line_num = (count + 1).ToString().PadLeft(6, '0');

            return line_num;
        }


        #region Inherit AGridObjectSourceQuery

        public override IQueryable<dynamic> InitialQueryView()
        {
            DateTime min_date = new DateTime(1753, 1, 7);

            Guid? outbound_order_master_id = null;
            if (this.FilterCustom != null)
            {
                var _outbound_order_master_id = this.FilterCustom.FirstOrDefault(qry => qry.DataFieldValue == "_outbound_order_master_id");
                if (_outbound_order_master_id != null)
                {
                    outbound_order_master_id = (Guid)_outbound_order_master_id.Value;
                }
            }
            //if (this.FilterCustom == null)
            //    return result.Where(x => false);

            //var _outbound_order_master_id = this.FilterCustom.FirstOrDefault(qry => qry.DataFieldValue == "_outbound_order_master_id");
            //if (_outbound_order_master_id != null)
            //{
            //    Guid id = (Guid)_outbound_order_master_id.Value;
            //    result = result.Where(x => x.outbound_order_master_id == id);
            //}

            var result = from rows in this._Model.v_wms_outbound_detail_linenumber

                         let _minDate = DbFunctions.CreateDateTime(0001, 1, 1, 0, 0, 0)
                         let _year = SqlFunctions.DatePart("year", (DbFunctions.Left(rows.expiry_date, 4).Trim()))
                         let _month = SqlFunctions.DatePart("year", "00" + rows.expiry_date.Substring(4, 2).Trim())
                         let _day = SqlFunctions.DatePart("year", "00" + rows.expiry_date.Substring(6, 2).Trim())
                         let expiry_date = !string.IsNullOrEmpty(rows.expiry_date)
                         ? SqlFunctions.DateAdd("day", _day - 1,
                               SqlFunctions.DateAdd("month", _month - 1,
                                   SqlFunctions.DateAdd("year", _year - 1, _minDate)))
                         : min_date
                         join master in _Model.t_wms_outbound_master on rows.outbound_order_master_id equals master.outbound_order_master_id
                         join detail in _Model.t_wms_outbound_detail on rows.outbound_order_detail_id equals detail.outbound_order_detail_id into detail_join
                         from detail in detail_join.DefaultIfEmpty()
                         where rows.outbound_order_master_id == outbound_order_master_id
                         select new
                         {
                             EditBOM = "",
                             cmd_print = "",
                             KeyId = rows.outbound_order_detail_id,
                             rows.outbound_order_master_id,
                             rows.line_number,
                             rows.line_number_int,
                             rows.default_item_status,
                             rows.item_number,
                             rows.item_description,
                             rows.bom_id,
                             rows.bom_master,
                             rows.item_category,
                             rows.category_description,
                             rows.lot_number,
                             rows.lpn,
                             expiry_date = expiry_date == min_date ? null : expiry_date,
                             rows.grade,
                             rows.price,
                             rows.quantity_order,
                             rows.quantity_pick,
                             rows.quantity_load,
                             rows.quantity_ship,
                             rows.uom,
                             rows.create_by,
                             rows.create_date,
                             rows.attribute1,
                             rows.attribute2,
                             rows.attribute3,
                             rows.attribute4,
                             rows.attribute5,
                             alter_quantity_order = rows.quantity_order/ (detail.pack_size_conversion_factor ?? 1),
                             alter_quantity_pick = rows.quantity_pick / (detail.pack_size_conversion_factor ?? 1),
                             alter_quantity_load = rows.quantity_load / (detail.pack_size_conversion_factor ?? 1),
                             alter_quantity_ship = rows.quantity_ship / (detail.pack_size_conversion_factor ?? 1),
                             alter_uom = detail.pack_size_uom ?? rows.uom,
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
