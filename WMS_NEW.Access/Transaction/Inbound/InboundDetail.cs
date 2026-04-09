using Prototype.Providers;
using System;
using System.Data;
using System.Data.Entity.Core.Objects;
using System.Linq;
using WMS_NEW.Source;

namespace WMS_NEW.Access.Transaction.Inbound
{
    [Serializable()]
    public class DTOInboundDetailSummary
    {
        public double PlanQuantity { get; set; }
        public double ReceiveQuantity { get; set; }
        public double PrintQuantity { get; set; }


    }

    public class InboundDetail : AEntityFormCommand<t_wms_inbound_detail>
    {
        public WMSEntities _Model { get; set; }

        public InboundDetail()
        {
            _Model = new WMSEntities();

            base.GridObjContext = _Model;
            base.DbSetEntities = () => { return _Model.t_wms_inbound_detail; };
        }


        #region ++INSTANCE STATIC++
        public static InboundDetail Instance
        {
            get
            {
                using (InboundDetail _Instance = new InboundDetail())
                {
                    return _Instance;
                }
            }
        }
        #endregion

        #region Inherit IEntityCommandForm

        public override bool ValidateSaveNew(t_wms_inbound_detail ent, ref string msg_validate)
        {
            return true;
        }

        public override void SetOptionalSaveNew(t_wms_inbound_detail ent)
        {
            if (ent.bom_id != null)
            {
                var bom = _Model.t_wms_item_bom
                          .Select(se => new { se.bom_id, se.item_number, se.bom_raw_code, se.is_bom_item, se.wh_item_master_id, se.item_master_id })
                          .First(x => x.bom_id == ent.bom_id);

                var ent_bom = new t_wms_inbound_group_bom();
                ent_bom.inbound_order_bom_id = Guid.NewGuid();
                ent_bom.inbound_order_master_id = ent.inbound_order_master_id;
                ent_bom.create_by = ent.create_by;
                ent_bom.create_date = ent.create_date;
                ent_bom.bom_id = bom.bom_id;
                ent_bom.wh_item_master_id = bom.wh_item_master_id;
                ent_bom.item_master_id = bom.item_master_id;
                ent_bom.item_number = bom.item_number;
                ent_bom.bom_raw_code = bom.bom_raw_code;
                ent_bom.quantity = ent.quantity_order;

                this._Model.t_wms_inbound_group_bom.Add(ent_bom);


                int line_number = ent.line_number.ToInteger();
                t_wms_inbound_detail detail;

                foreach (var bom_de in _Model.t_wms_item_bom_detail.Where(x => x.bom_id == bom.bom_id))
                {
                    detail = new t_wms_inbound_detail();
                    detail.line_number = line_number.ToString("000000");
                    detail.default_item_status = ent.default_item_status;
                    detail.item_status = ent.item_status;
                    detail.over_receipt_allowed = ent.over_receipt_allowed;
                    detail.quantity_receive = 0;

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

                    detail.inbound_order_detail_id = Guid.NewGuid();
                    detail.inbound_order_master_id = ent.inbound_order_master_id;
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

                    detail.inbound_order_bom_id = ent_bom.inbound_order_bom_id;
                    detail.bom_detail_quantity = bom_de.quantity;

                    this._Model.t_wms_inbound_detail.Add(detail);

                    line_number++;
                }
            }
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

        #endregion


        #region Inherit AGridObjectSourceQuery

        public override IQueryable<dynamic> InitialQueryView()
        {
            Guid? inbound_order_master_id = null;
            if (this.FilterCustom != null)
            {
                var entOrder = this.FilterCustom.FirstOrDefault(qry => qry.DataFieldValue == "_inbound_order_master_id");
                if (entOrder != null && entOrder.Value != null)
                {
                    inbound_order_master_id = (Guid)entOrder.Value;
                }
            }

            var result = from rows in this._Model.v_wms_inbound_detail_v2
                         join mas in this._Model.t_wms_inbound_master on rows.inbound_order_master_id equals mas.inbound_order_master_id
                         join detail in this._Model.t_wms_inbound_detail on rows.inbound_order_detail_id equals detail.inbound_order_detail_id into leftLabel
                         from detail in leftLabel.DefaultIfEmpty()
                         where rows.inbound_order_master_id == inbound_order_master_id
                         select new
                         {
                             Print = "",
                             EditBOM = "",
                             mas.inbound_order_number,
                             KeyId = rows.inbound_order_detail_id,
                             rows.line_number,
                             rows.default_item_status,
                             rows.item_number,
                             rows.item_description,
                             rows.lpn,
                             rows.mfg_date,
                             rows.expiry_date,
                             rows.serial_number,
                             rows.lot_number,
                             rows.quantity_order,
                             rows.quantity_receive,
                             quantity_remain = rows.quantity_order - rows.quantity_receive,
                             rows.uom,
                             bom_master = rows.bom_item_number,
                             rows.bom_id,
                             rows.over_receipt_allowed,
                             rows.over_receipt_percentage,
                             rows.price,
                             rows.category_id,
                             rows.item_category,
                             rows.category_description,
                             rows.grade,
                             rows.create_by,
                             rows.create_date,
                             rows.attribute1,
                             rows.attribute2,
                             rows.attribute3,
                             rows.attribute4,
                             rows.attribute5,
                             rows.ref_lpn,
                             alter_quantity_order = rows.quantity_order / (detail.pack_size_conversion_factor ?? 1),
                             alter_quantity_receive = rows.quantity_receive / (detail.pack_size_conversion_factor ?? 1),
                             alter_quantity_remain = (rows.quantity_order - rows.quantity_receive) / (detail.pack_size_conversion_factor ?? 1),
                             alter_uom = detail.pack_size_uom ?? rows.uom
                         };

            return result;


        }
        public override IQueryable<dynamic> InitialQueryExport()
        {
            return this.InitialQueryView();
        }

        #endregion


        #region Function

        public DTOInboundDetailSummary Get_InboundDetailSummary(Guid _inbound_order_master_id)
        {
            //INBOUND ORDER
            var result = from rows in this._Model.t_wms_inbound_detail
                         let item = rows.t_wms_wh_item.t_wms_item

                         join uom in this._Model.t_wms_item_uom
                         on rows.item_uom_id equals uom.item_uom_id

                         where rows.inbound_order_master_id == _inbound_order_master_id

                         group rows
                         by rows.inbound_order_master_id into grp
                         select new DTOInboundDetailSummary
                         {
                             PlanQuantity = grp.Sum(su => su.quantity_order / (su.pack_size_conversion_factor ?? 1)),
                             ReceiveQuantity = grp.Sum(su => su.quantity_receive / (su.pack_size_conversion_factor ?? 1)),
                         };

            return result.FirstOrDefault();


        }
        public DTOInboundDetailSummary Get_OrderSummary(Guid _inbound_order_master_id)
        {
            //INBOUND ORDER
            var result = from rows in this._Model.t_wms_print_label
                         where rows.inbound_order_master_id == _inbound_order_master_id
                         group rows
                         by rows.inbound_order_master_id into grp
                         select new DTOInboundDetailSummary
                         {
                             PlanQuantity = grp.Sum(su => (su.pack_size_per_pallet ?? 0)),
                         };
            if(result.Count() > 0)
            {
                return result.FirstOrDefault();
            }
            else
            {
                return Get_InboundDetailSummary(_inbound_order_master_id);
            }
        }
        public DTOInboundDetailSummary Get_ReceiveSummary(Guid _inbound_order_master_id)
        {
            //INBOUND ORDER
            var result = from rows in this._Model.t_wms_print_label
                         where rows.inbound_order_master_id == _inbound_order_master_id
                         && rows.is_received == "YES"
                         group rows
                         by rows.inbound_order_master_id into grp
                         select new DTOInboundDetailSummary
                         {
                             ReceiveQuantity = grp.Sum(su => (su.pack_size_per_pallet ?? 0)),
                         };

            if (result.Count() > 0)
            {
                return result.FirstOrDefault();
            }
            else
            {
                return Get_InboundDetailSummary(_inbound_order_master_id);
            }
        }
        public DTOInboundDetailSummary Get_PrintSummary(Guid _inbound_order_master_id)
        {
            //INBOUND ORDER
            var result = from rows in this._Model.t_wms_print_label
                         where rows.inbound_order_master_id == _inbound_order_master_id
                         && rows.is_print == "YES" && (rows.is_cancelled == "NO" || rows.is_cancelled != "YES")
                         group rows
                         by rows.inbound_order_master_id into grp
                         select new DTOInboundDetailSummary
                         {
                             PrintQuantity = grp.Sum(su => (su.pack_size_per_pallet ?? 0)),
                         };

            return result.FirstOrDefault();
        }

        public string GetLineNumber(Guid _inbound_order_master_id)
        {

            return this._Model.GetDataBy(this, delegate ()
            {
                var result = (from rows in this._Model.t_wms_inbound_detail
                              where rows.inbound_order_master_id == _inbound_order_master_id
                              select new
                              {
                                  rows.line_number
                              }).ToList();

                int count = 0;
                if (result.Count > 0)
                {
                    count = result.Max(ss => Convert.ToInt32(ss.line_number));
                }

                string _lineNumber = (count + 1).ToString().PadLeft(6, '0');
                return _lineNumber;

            });

        }

        public object GetInboundDetail_Uom(Guid _inbound_order_master_id, Guid _wh_item_master_id)
        {

            return this._Model.GetDataEntityBy<t_wms_inbound_detail>(this, delegate ()
            {
                var result = (from rows in this._Model.t_wms_inbound_detail
                              where rows.inbound_order_master_id == _inbound_order_master_id
                              && rows.wh_item_master_id == _wh_item_master_id
                              select rows).FirstOrDefault();

                return result;
            });

        }

        public object GetInboundDetail(Guid inbound_order_master_id, Guid wh_item_master_id, string line_number)
        {
            return this._Model.GetDataBy(this, delegate ()
            {

                var result = (from rows in this._Model.t_wms_inbound_detail
                              where rows.inbound_order_master_id == inbound_order_master_id
                              && rows.wh_item_master_id == wh_item_master_id
                              && rows.line_number == line_number
                              select rows).FirstOrDefault();

                return result;


            });
        }

        public object IsOrderQTY(string reference_number, string line_number, Guid wh_item_master_id, double quantity_input)
        {
            return IsOrderQTY(reference_number, line_number, wh_item_master_id, quantity_input, null);
        }

        public object IsOrderQTY(string reference_number, string line_number, Guid wh_item_master_id, double quantity_input, Guid? inbound_order_detail_id)
        {
            return this._Model.GetDataBy(this, delegate ()
            {
                var result = (from rows in this._Model.t_wms_inbound_detail

                              where rows.t_wms_inbound_master.reference_number == reference_number
                              && rows.line_number == line_number
                              && rows.wh_item_master_id == wh_item_master_id
                              select rows);

                if (inbound_order_detail_id == null)
                {
                    result = result.Where(wh => wh.inbound_order_detail_id != inbound_order_detail_id);
                }

                var total = result.Count() > 0 ? result.Sum(su => su.quantity_order) : 0;



                var orderPO = (from rows in this._Model.t_wms_inbound_po_detail
                               where rows.t_wms_inbound_po_master.inbound_order_number == reference_number
                               && rows.line_number == line_number
                               && rows.wh_item_master_id == wh_item_master_id
                               select rows.quantity_order).Sum();

                double remainQTY = orderPO - (total + quantity_input);

                return remainQTY;


            });
        }

        public bool IsReceiveItem(Guid inbound_order_master_id, Guid wh_item_master_id, string lot_number, bool isEdit, Guid inbound_order_detail_id)
        {
            return this._Model.GetDataBy(this, delegate ()
            {
                var result = (from rows in this._Model.t_wms_inbound_detail
                              let lot = rows.lot_number == null ? string.Empty : rows.lot_number
                              where rows.inbound_order_master_id == inbound_order_master_id
                              && rows.wh_item_master_id == wh_item_master_id
                              && lot == lot_number
                              select rows);

                if (isEdit)
                {
                    result = result.Where(w => w.inbound_order_detail_id != inbound_order_detail_id);
                }

                return result.FirstOrDefault() != null ? true : false;

            });
        }

        public void GetLPN(
        string appID
        , string warehouseCode
        , string deviceID
        , string userID
        , string prefix
        , string suffix
        , int number_of_serial
        , string generateType
        , ref string _LPN
        , ref string _errCode
        , ref string _errMsg)
        {
            var LPN = new ObjectParameter("in_vchLPN", typeof(string));
            var errCode = new ObjectParameter("out_ErrorCode", typeof(string));
            var errMsg = new ObjectParameter("out_ErrorMessage", typeof(string));

            this._Model.usp_generate_running_lpn(
                appID,
                warehouseCode,
                deviceID,
                userID,
                prefix,
                suffix,
                number_of_serial,
                generateType,
                LPN,
                errCode,
                errMsg);

            _LPN = LPN.Value.ToString();
            _errCode = errCode.Value.ToString();
            _errMsg = errMsg.Value.ToString();
        }

        public void GetRunningLPN(
            string appID
            , string warehouseCode
            , string deviceID
            , string userID
            , string prefix
            , string suffix
            , int number_of_serial
            , string generateType
            , ref string _LPN
            , ref string _errCode
            , ref string _errMsg)
        {
            var LPN = new ObjectParameter("in_vchLPN", typeof(string));
            var errCode = new ObjectParameter("out_ErrorCode", typeof(string));
            var errMsg = new ObjectParameter("out_ErrorMessage", typeof(string));

            this._Model.usp_generate_running_lpn(
                appID,
                warehouseCode,
                deviceID,
                userID,
                prefix,
                suffix,
                number_of_serial,
                generateType,
                LPN,
                errCode,
                errMsg);

            _LPN = LPN.Value.ToString();
            _errCode = errCode.Value.ToString();
            _errMsg = errMsg.Value.ToString();
        }

        public void GetRunningSerial(
            string userID
            , string prefix
            , string suffix
            , int number_of_serial
            , string generateType
            , ref string _serial
            , ref string _serial_int
            , ref string _errCode
            , ref string _errMsg)
        {

            var serial = new ObjectParameter("in_vchSerial", typeof(string));
            var serial_int = new ObjectParameter("in_vchSerial_INT", typeof(string));
            var errCode = new ObjectParameter("out_ErrorCode", typeof(string));
            var errMsg = new ObjectParameter("out_ErrorMessage", typeof(string));

            this._Model.usp_generate_running_serial(userID
                , prefix
                , suffix
                , number_of_serial
                , generateType
                , serial_int
                , serial
                , errCode
                , errMsg);

            _serial = serial.Value.ToString();
            _serial_int = serial_int.Value.ToString();
            _errCode = errCode.Value.ToString();
            _errMsg = errMsg.Value.ToString();

        }
        public override bool DeleteById(object _Id)
        {
            return this._Model.Delete(this, delegate ()
            {
                Guid inbound_order_detail_id = (Guid)_Id;

                var entDet = this._Model.t_wms_inbound_detail.Where(w => w.inbound_order_detail_id == inbound_order_detail_id);
                this._Model.t_wms_inbound_detail.RemoveRange(entDet);

                var entPrint = this._Model.t_wms_print_label.Where(w => w.inbound_order_detail_id == inbound_order_detail_id);
                this._Model.t_wms_print_label.RemoveRange(entPrint);
                return this._Model.SaveChanges();
            });
        }

        #endregion

    }
}
