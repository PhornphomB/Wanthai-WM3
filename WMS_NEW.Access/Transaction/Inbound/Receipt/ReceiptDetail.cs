using Prototype.Providers;
using System;
using System.Data;
using System.Data.Entity.Core.Objects;
using System.Linq;
using WMS_NEW.Source;


namespace WMS_NEW.Access.Transaction.Inbound.Receipt
{
    [Serializable()]
    public class DTOReceipt : t_wms_receipt_detail
    {

        public Guid location_id { get; set; }
        public string reason_code { get; set; }
        public Guid inv_status_id { get; set; }

        public DateTime? _expiry_date { get; set; }
    }

    public class ReceiptDetail : AEntityFormCommand<DTOReceipt>
    {
        public WMSEntities _Model { get; set; }


        public ReceiptDetail()
        {
            _Model = new WMSEntities();
            _Model.Configuration.EnsureTransactionsForFunctionsAndCommands = false;
            _Model.Database.CommandTimeout = 1200;

            base.GridObjContext = _Model;
            base.DbSetEntities = () => { return _Model.t_wms_receipt_detail; };
        }

        #region ++INSTANCE STATIC++
        public static ReceiptDetail Instance
        {
            get
            {
                using (ReceiptDetail _Instance = new ReceiptDetail())
                {
                    return _Instance;
                }
            }
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

            var result = from rows in this._Model.v_wms_receipt_detail_v2
                         where rows.inbound_order_master_id == inbound_order_master_id
                         select new
                         {
                             KeyID = rows.receipt_detail_id,
                             receive_date = rows.create_date,
                             rows.location_received,
                             rows.line_number,
                             rows.item_number,
                             rows.item_description,
                             rows.lot_number,
                             rows.expiry_date,
                             rows.parent_lpn,
                             rows.lpn,
                             rows.quantity_order,
                             rows.quantity_received,
                             rows.uom,
                             rows.over_receipt_allowed,
                             receive_by = rows.create_by,
                             rows.serial_number,
                             rows.receipt_header_id,
                             rows.attribute1,
                             rows.attribute2,
                             rows.attribute3,
                             rows.attribute4,
                             rows.attribute5,
                             rows.category_id,
                             rows.category_description,
                             rows.pallet_seq,
                         };

            return result;
        }
        public override IQueryable<dynamic> InitialQueryExport()
        {
            return this.InitialQueryView();
        }

        #endregion

        #region Inherit IEntityCommandForm

        public override bool Save()
        {
            var ent = this.Entity;
            string in_vchApplicationID = _SessionVals.AppID;
            string in_vchDeviceID = _SessionVals.DeviceID;
            string in_vchUserID = _SessionVals.UserName;
            //--------------------------------------------
            var entMaster = this._Model.t_wms_inbound_master.Where(w => w.inbound_order_master_id == ent.inbound_order_master_id).FirstOrDefault();
            Nullable<System.Guid> in_vchWhMasterID = entMaster.wh_master_id;
            Nullable<System.Guid> in_vchInboundMasterID = ent.inbound_order_master_id;
            Nullable<System.Guid> in_vchLocationID = ent.location_id;
            Nullable<System.Guid> in_vchWhItemMasterID = ent.wh_item_master_id;
            Nullable<double> in_fltQuantity = ent.quantity_received;
            Nullable<System.Guid> in_vchItemUomID = ent.item_uom_id;
            string in_vchLot = ent.lot_number;
            string in_vchExpiryDate = ent._expiry_date != null ? ent._expiry_date.Value.ToString("yyyyMMdd") : string.Empty;
            string in_vchSerial = ent.serial_number;
            string in_vchParentLPN = ent.parent_lpn;
            string in_vchLPN = ent.lpn;
            string in_vchReasonCode = ent.reason_code;
            Nullable<System.Guid> in_vchReceiptHeaderID = ent.receipt_header_id;
            Nullable<System.Guid> in_vchInvStatusID = ent.inv_status_id;
            string in_vchAttribute1 = ent.attribute1;
            string in_vchAttribute2 = ent.attribute2;
            string in_vchAttribute3 = ent.attribute3;
            string in_vchAttribute4 = ent.attribute4;
            string in_vchAttribute5 = ent.attribute5;
            Nullable<System.DateTime> in_dtReceiveDate = ent.receive_date;
            Nullable<System.DateTime> in_dtMfgDate = ent.mfg_date;

            var errCode = new ObjectParameter("out_ErrorCode", typeof(string));
            var errMsg = new ObjectParameter("out_ErrorMessage", typeof(string));

            //this._Model.usp_inbound_update_receive_qty(in_vchApplicationID, in_vchDeviceID, in_vchUserID, in_vchWhMasterID, in_vchInboundMasterID, in_vchReceiptHeaderID, in_vchLocationID, in_vchInvStatusID, in_vchWhItemMasterID, in_fltQuantity, in_vchItemUomID, in_vchLot, in_vchExpiryDate, in_vchSerial, in_vchParentLPN, in_vchLPN, in_vchReasonCode, in_vchAttribute1, in_vchAttribute2, in_vchAttribute3, in_vchAttribute4, in_vchAttribute5, in_dtReceiveDate, in_dtMfgDate
            //    , errCode, errMsg);

            //if (errCode.Value.ToString() == "0")
            //{
            //    this.MessageSuccess(this, errMsg.Value.ToString());
            return true;
            //}
            //else
            //{
            //    this.MessageWarning(this, errMsg.Value.ToString());
            //    return false;
            //}
        }

        public override DTOReceipt GetByKeyID(object Id)
        {
            return this.GridObjContext.GetDataEntityBy(this, delegate ()
            {
                Entity = DbSetEntities().Find(Id);

                return Entity;
            });
        }

        #endregion



        #region Function
        public bool IsDetailStatus(Guid _receipt_header_id, string _receipt_status)
        {

            var result = (from rows in this._Model.t_wms_receipt_detail
                          where rows.receipt_header_id == _receipt_header_id
                          select rows);

            int countAll = result.Count();
            int countStatus = result.Where(qry => qry.receipt_item_status == _receipt_status).Count();

            return countAll == countStatus ? true : false;
        }

        public bool Close_Receipt(Nullable<System.Guid> in_vchWhMasterID, Nullable<System.Guid> in_vchInboundMasterID, Nullable<System.Guid> in_vchReceiptHeaderID)
        {
            string in_vchApplicationID = _SessionVals.AppID;
            string in_vchDeviceID = _SessionVals.DeviceID;
            string in_vchUserID = _SessionVals.UserName;

            var errCode = new ObjectParameter("out_ErrorCode", typeof(string));
            var errMsg = new ObjectParameter("out_ErrorMessage", typeof(string));


            this._Model.usp_close_partial_receive(in_vchApplicationID, in_vchDeviceID, in_vchWhMasterID, in_vchInboundMasterID, in_vchReceiptHeaderID, in_vchUserID, errCode, errMsg);

            if (errCode.Value.ToString() == "0")
            {
                this._Model.MessageSuccess(this, errMsg.Value.ToString());
                return true;
            }
            else
            {
                this._Model.MessageWarning(this, errMsg.Value.ToString());
                return false;
            }
        }

        public void InboundReceiptAll(Nullable<System.Guid> in_vchInboundMasterID, Nullable<System.Guid> _locationId, string _lpn,out string _errCode, out string _errMsg)
        {
            string in_vchApplicationID = _SessionVals.AppID;
            string in_vchDeviceID = _SessionVals.DeviceID;
            string in_vchUserID = _SessionVals.UserName;

            var errCode = new ObjectParameter("out_ErrorCode", typeof(string));
            var errMsg = new ObjectParameter("out_ErrorMessage", typeof(string));


            this._Model.usp_inbound_update_receive_all(in_vchApplicationID, in_vchInboundMasterID, _locationId, _lpn, in_vchDeviceID, in_vchUserID, errCode, errMsg);

            _errCode = errCode.Value.ToString();
            _errMsg = errMsg.Value.ToString();
             
        }

        public string Generate_Receipt_Number(Guid _inbound_order_master_id, Guid _wh_master_id, string _user_id)
        {
            var receipt_number = new ObjectParameter("out_vchReceiptNumber", typeof(string));
            var receipt_header_id = new ObjectParameter("out_vchReceiptHeaderID", typeof(Guid));

            this._Model.usp_generate_receipt_number(_user_id, _wh_master_id, _inbound_order_master_id, receipt_header_id, receipt_number);

            return receipt_number.Value.ToString() != "0" ? receipt_number.Value.ToString() : string.Empty;

        }

        //public object Get_Receipt_Item(Guid _inbound_order_master_id, string _item_number)
        //{
        //    var result = (from rows in this._Model.t_wms_inbound_detail
        //                  let item = rows.t_wms_wh_item.t_wms_item
        //                  where rows.inbound_order_master_id == _inbound_order_master_id
        //                  && (item.item_number == _item_number || item.t_wms_item_crossref.Any(an => an.alternate_item_number == _item_number))
        //                  select new DTOReceipt
        //                  {
        //                      wh_item_master_id = rows.wh_item_master_id,
        //                      item_number = item.item_number,
        //                      lot_control = item.lot_control,
        //                      expiry_date_control = item.expiry_date_control,
        //                      sn_control = item.sn_control,
        //                      over_receipt_allowed = rows.over_receipt_allowed,

        //                      attribute1_control = item.attribute1_control,
        //                      attribute2_control = item.attribute2_control,
        //                      attribute3_control = item.attribute3_control,
        //                      attribute4_control = item.attribute4_control,
        //                      attribute5_control = item.attribute5_control,

        //                  }).FirstOrDefault();

        //    return result;
        //}


        public void Validate_Qty_Over_Receipt(
            string _appID
            , string _localID
            , Guid _wh_master_id
            , Guid _inbound_order_master_id
            , Guid _wh_item_master_id
            , Guid _item_master_id
            , Guid _owner_id
            , double _qty
            , string _uomPrompt
            , string _lot
            , string _expDate
            , string _serial
            , string _lpn
            , ref double _qtyPlan
            , ref double _qtyReceive
            , ref string _errCode
            , ref string _errMsg)
        {
            var qtyPlan = new ObjectParameter("out_fltQuantityPlan", typeof(double));
            var qtyReceive = new ObjectParameter("out_fltQuantityReceive", typeof(double));
            var errCode = new ObjectParameter("out_ErrorCode", typeof(string));
            var errMsg = new ObjectParameter("out_ErrorMessage", typeof(string));

            this._Model.usp_inbound_validate_quantity(
                _appID
                , _localID
                , _wh_master_id
                , _inbound_order_master_id
                , _wh_item_master_id
                , _item_master_id
                , _owner_id
                , _qty
                , _uomPrompt
                , _lot
                , _expDate
                , _serial
                , _lpn
                , qtyPlan
                , qtyReceive
                , errCode
                , errMsg);

            _qtyPlan = qtyPlan.Value == DBNull.Value ? 0 : (double)qtyPlan.Value;
            _qtyReceive = qtyReceive.Value == DBNull.Value ? 0 : (double)qtyReceive.Value;
            _errCode = errCode.Value == DBNull.Value ? string.Empty : errCode.Value.ToString();
            _errMsg = errMsg.Value == DBNull.Value ? string.Empty : errMsg.Value.ToString();
        }

        public string Generate_Lotnumber(Guid _inbound_order_detail_id, string _wh_item_master_id)
        {
            string lastLot = string.Empty;
            string strDateNow = DateTime.Now.ToString("yyyyMMdd");
            var ent = (from rows in this._Model.t_wms_receipt_detail
                       where rows.inbound_order_detail_id == _inbound_order_detail_id
                       orderby rows.lot_number descending
                       select rows).FirstOrDefault();
            if (ent != null)
            {
                int running = 0;
                string lotDate = ent.lot_number.Substring(0, 8);
                if (strDateNow == lotDate)
                {
                    running = ent.lot_number.Substring(8, 2).ToInteger() + 1;
                    lastLot = lotDate + running.ToString("00");
                    return lastLot;
                }
                else
                {
                    lastLot = strDateNow + "01";
                    return lastLot;
                }
            }
            else
            {
                lastLot = strDateNow + "01";
                return lastLot;
            }
        }

        public bool Validate_Lot(Guid _inbound_order_master_id, Guid _wh_item_master_id, string _lot_number)
        {

            var result = (from rows in this._Model.t_wms_receipt_detail
                          where rows.lot_number == _lot_number
                          select rows);
            if (result != null && result.Count() > 0)
            {
                var ent = result.Any(w => w.t_wms_inbound_detail.inbound_order_master_id == _inbound_order_master_id && w.t_wms_inbound_detail.wh_item_master_id == _wh_item_master_id);
                if (ent)
                {
                    return true;
                }
                else
                {
                    return false;
                }
            }
            else
            {
                return true;
            }
        }
        public DateTime? GetMfgDate(Guid receipt_header_id)
        {
            var result = (from rows in this._Model.t_wms_receipt_detail
                          where rows.receipt_header_id == receipt_header_id
                          orderby rows.mfg_date descending
                          select rows.mfg_date).FirstOrDefault();

            return result;
        }
        #endregion
    }
}
