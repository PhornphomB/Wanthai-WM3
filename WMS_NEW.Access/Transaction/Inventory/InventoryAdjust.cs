using Prototype.Providers;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity.Core.Objects;
using System.Linq;
using System.Text;
using WMS_NEW.Source;

namespace WMS_NEW.Access.Transaction.Inventory
{
    [Serializable]
    public class DTO_Adjust
    {
        public string key_id { get; set; }
        public double? qty { get; set; }

    }
    public class InventoryAdjust : AGridObjectSourceQuery, IDisposable
    {
        public WMSEntities _Model { get; set; }

        public InventoryAdjust()
        {
            this._Model = new WMSEntities();
            this._Model.Configuration.EnsureTransactionsForFunctionsAndCommands = false;

        }


        #region ++INSTANCE STATIC++
        public static InventoryChange Instance
        {
            get
            {
                using (InventoryChange _Instance = new InventoryChange())
                {
                    return _Instance;
                }
            }
        }
        #endregion

        #region IDisposable Members
        //public void Dispose()
        //{
        //    GC.SuppressFinalize(this);
        //}

        #endregion


        public override IQueryable<dynamic> InitialQueryView()
        {
            string user_id = _SessionVals.UserName;
            var listWarehouse = this._Model.t_wms_wh_user.Where(w => w.user_id == user_id).Select(s => s.wh_master_id).ToList();
            var listOwner = this._Model.t_wms_owner_user.Where(w => w.user_id == user_id).Select(s => s.owner_id).ToList();
            var listLoc = this._Model.t_wms_rule.Where(w => w.rule_code == "LOCATION_TYPE_NOT_MOVE_ITEM" && w.is_active == "YES").Select(s => s.value).ToList();

            var result = from rows in this._Model.v_wms_inventory_data_by_serial
                         where listWarehouse.Contains(rows.wh_master_id)
                         && listOwner.Contains(rows.owner_id)
                         && !listLoc.Contains(rows.loc_type)
                         select new
                         {
                             KeyId = rows.key_id,
                             rows.inventory_id,
                             rows.attribute_group_id,
                             rows.inv_type,
                             rows.wh_id,
                             rows.wh_master_id,
                             rows.owner_id,
                             rows.owner_code,
                             rows.location_id,
                             rows.parent_lpn,
                             rows.lpn,
                             rows.wh_item_master_id,
                             rows.item_number,
                             rows.description,
                             rows.uom_prompt,
                             rows.uom,
                             rows.serial_number,
                             rows.sn_control,
                             quantity_avalible = rows.quantity - rows.quantity_allocated,
                             rows.quantity,
                             rows.quantity_allocated,
                             rows.quantity_incoming,
                             rows.receive_date,
                             rows.mfg_date,
                             rows.exp_date,
                             rows.inv_status,
                             rows.inventory_status_id,
                             rows.location,
                             rows.loc_type,
                             rows.pick_sequence,
                             rows.pick_area_id,
                             rows.alternate_item_number,
                             rows.lot_number,
                             rows.expiry_date,
                             rows.zone,
                             rows.category_id,
                             rows.item_category,
                             rows.cate_description,
                             rows.receive_date_filter,
                             rows.dg_code,
                             rows.grade,
                             rows.price,
                             rows.Checked,
                             rows.adjust_qty,
                             rows.item_master_id,
                             rows.lpn_controlled,
                             rows.cost,
                             rows.attribute1,
                             rows.attribute2,
                             rows.attribute3,
                             rows.attribute4,
                             rows.attribute5,
                             rows.days_to_expire,
                             rows.create_date,
                             rows.create_by,
                         };

            var entListKey = this.FilterCustom.Where(w => w.DataFieldValue == "_list_key").FirstOrDefault();
            if (entListKey != null && entListKey.Value != null)
            {
                List<string> listKey = (List<string>)entListKey.Value;
                result = result.Where(w => listKey.Contains(w.KeyId));
            }

            return result;
        }

        public override IQueryable<dynamic> InitialQueryExport()
        {
            return InitialQueryView();
        }

        #region Adjust Not Stock

        //public DataTable ListAdjustStock(List<Guid> _listKey)
        //{
        //    string user_id = _SessionVals.UserName;
        //    var listWarehouse = this._Model.t_wms_wh_user.Where(w => w.user_id == user_id).Select(s => s.wh_master_id).ToList();
        //    var listOwner = this._Model.t_wms_owner_user.Where(w => w.user_id == user_id).Select(s => s.owner_id).ToList();

        //    var result = from rows in this._Model.v_wms_inventory_data
        //                 where listWarehouse.Contains(rows.wh_master_id)
        //                 && listOwner.Contains(rows.owner_id)
        //                 && _listKey.Contains(rows.inventory_id)
        //                 select new
        //                 {
        //                     KeyId = rows.inventory_id,
        //                     rows.wh_id,
        //                     rows.owner_code,
        //                     rows.parent_lpn,
        //                     rows.lpn,
        //                     rows.item_number,
        //                     rows.description,
        //                     rows.serial_number,
        //                     rows.quantity,
        //                     rows.quantity_allocated,
        //                     rows.quantity_incoming,
        //                     rows.inv_status,
        //                     rows.location,
        //                     rows.lot_number,
        //                     rows.expiry_date,
        //                     rows.zone,
        //                     rows.cate_description,
        //                     rows.adjust_qty,
        //                     rows.days_to_expire,
        //                     rows.create_date,
        //                     rows.create_by,
        //                 };

        //    return result.ToDataTable();
        //}


        public bool Adjust_NotStock(string in_vchSubTranType, Nullable<System.Guid> in_vchWhMasterID, Nullable<System.Guid> in_vchOwnerID, Nullable<System.Guid> in_vchInvStatusID, Nullable<System.Guid> in_vchLocationID, Nullable<System.Guid> in_vchWhItemMasterID, string in_vchParentLPN, string in_vchLPN, string in_vchLotNumber, string in_vchExpiryDate, string in_vchAttribute1, string in_vchAttribute2, string in_vchAttribute3, string in_vchAttribute4, string in_vchAttribute5, string in_vchSerialNumber, Nullable<double> in_fltQty, Nullable<System.Guid> in_vchItemUomID, Nullable<System.Guid> in_vchReasonID, Nullable<System.DateTime> in_dtReceiveDate, Nullable<System.DateTime> in_dtMfgDate)
        {
            var errCode = new ObjectParameter("out_ErrorCode", typeof(string));
            var errMsg = new ObjectParameter("out_ErrorMessage", typeof(string));
            //Reference Number
            var refID = new ObjectParameter("v_vchReferenceID", typeof(string));
            this._Model.usp_get_ReferenceID(refID);
            string ref_id = refID.Value.ToString();


            string in_vchApplicationID = _SessionVals.AppID;
            string in_vchDeviceID = _SessionVals.DeviceID;
            string in_vchUserID = _SessionVals.UserName;

            string in_vchTransactionType = "INV_ADJUST";

            this._Model.usp_inventory_adjust_in(in_vchApplicationID, in_vchDeviceID, in_vchUserID, in_vchTransactionType, in_vchSubTranType, in_vchWhMasterID, in_vchOwnerID, in_vchInvStatusID, in_vchLocationID, in_vchWhItemMasterID, in_vchParentLPN, in_vchLPN, in_vchLotNumber, in_vchExpiryDate, in_vchAttribute1, in_vchAttribute2, in_vchAttribute3, in_vchAttribute4, in_vchAttribute5, in_vchSerialNumber, in_fltQty, in_vchItemUomID, in_vchReasonID, in_dtReceiveDate, in_dtMfgDate, ref_id, errCode, errMsg);
            if (errCode.Value.ToString() == "0")
            {
                this.MessageSuccess(this, errMsg.Value.ToString());
                return true;
            }
            else
            {
                this.MessageWarning(this, errMsg.Value.ToString());
                return false;
            }
        }
        #endregion

        #region Adjust Stock
        public bool Adjust_QuantityPlus(List<DTO_Adjust> _listKey, string in_vchSubTranType, Nullable<System.Guid> in_vchReasonID)
        {
            int countSuccess = 0;
            int countError = 0;
            string in_vchApplicationID = _SessionVals.AppID;
            string in_vchDeviceID = _SessionVals.DeviceID;
            string in_vchUserID = _SessionVals.UserName;
            string in_vchTransactionType = "INV_ADJUST";

            StringBuilder error_msg = new StringBuilder();
            StringBuilder error_success = new StringBuilder();
            //Reference Number
            var refID = new ObjectParameter("v_vchReferenceID", typeof(string));
            this._Model.usp_get_ReferenceID(refID);
            string ref_id = refID.Value.ToString();

            foreach (var inv in _listKey)
            {
                var errCode = new ObjectParameter("out_ErrorCode", typeof(string));
                var errMsg = new ObjectParameter("out_ErrorMessage", typeof(string));

                //Get Inventory
                var ent = this._Model.v_wms_inventory_data_by_serial.Where(w => w.key_id == inv.key_id).FirstOrDefault();
                if (ent != null)
                {
                    Nullable<double> in_fltQty = ent.sn_control.ToUpper().Trim() == "FULL" ? 1 : inv.qty;
                    Nullable<System.Guid> in_vchWhMasterID = ent.wh_master_id;
                    Nullable<System.Guid> in_vchOwnerID = ent.owner_id;
                    Nullable<System.Guid> in_vchInvStatusID = ent.inventory_status_id;
                    Nullable<System.Guid> in_vchLocationID = ent.location_id;
                    Nullable<System.Guid> in_vchWhItemMasterID = ent.wh_item_master_id;
                    string in_vchParentLPN = ent.parent_lpn;
                    string in_vchLPN = ent.lpn;
                    string in_vchLotNumber = ent.lot_number;
                    string in_vchExpiryDate = ent.expiry_date;
                    string in_vchAttribute1 = ent.attribute1;
                    string in_vchAttribute2 = ent.attribute2;
                    string in_vchAttribute3 = ent.attribute3;
                    string in_vchAttribute4 = ent.attribute4;
                    string in_vchAttribute5 = ent.attribute5;
                    string in_vchSerialNumber = ent.serial_number;
                    Nullable<System.Guid> in_vchItemUomID = ent.item_uom_id;
                    Nullable<System.DateTime> in_dtReceiveDate = ent.receive_date;
                    Nullable<System.DateTime> in_dtMfgDate = ent.mfg_date;


                    this._Model.usp_inventory_adjust_in(in_vchApplicationID, in_vchDeviceID, in_vchUserID, in_vchTransactionType, in_vchSubTranType, in_vchWhMasterID, in_vchOwnerID, in_vchInvStatusID, in_vchLocationID, in_vchWhItemMasterID, in_vchParentLPN, in_vchLPN, in_vchLotNumber, in_vchExpiryDate, in_vchAttribute1, in_vchAttribute2, in_vchAttribute3, in_vchAttribute4, in_vchAttribute5, in_vchSerialNumber, in_fltQty, in_vchItemUomID, in_vchReasonID, in_dtReceiveDate, in_dtMfgDate, ref_id, errCode, errMsg);
                }

                if (errCode.Value.ToString() == "0")
                {
                    countSuccess++;
                }
                else
                {
                    countError++;
                    error_msg.AppendLine(countError.ToString() + ". " + errMsg.Value.ToString());
                }
            }

            if (countError == 0)
            {
                this.MessageSuccess(this, "Save success.");
                return true;
            }
            else
            {
                error_success.AppendLine("Save success[" + countSuccess.ToString() + "]");
                error_success.AppendLine("Error[" + countError.ToString() + "]");
                error_success.AppendLine(error_msg.ToString());
                this.MessageWarning(this, error_success.ToString());
                return false;
            }

        }
        public bool Adjust_QuantityMinus(List<DTO_Adjust> _listKey, string in_vchSubTranType, Nullable<System.Guid> in_vchReasonID)
        {
            int countSuccess = 0;
            int countError = 0;
            string in_vchApplicationID = _SessionVals.AppID;
            string in_vchDeviceID = _SessionVals.DeviceID;
            string in_vchUserID = _SessionVals.UserName;
            string in_vchTransactionType = "INV_ADJUST";

            StringBuilder error_msg = new StringBuilder();
            StringBuilder error_success = new StringBuilder();
            //Reference Number
            var refID = new ObjectParameter("v_vchReferenceID", typeof(string));
            this._Model.usp_get_ReferenceID(refID);
            string ref_id = refID.Value.ToString();

            foreach (var inv in _listKey)
            {
                var errCode = new ObjectParameter("out_ErrorCode", typeof(string));
                var errMsg = new ObjectParameter("out_ErrorMessage", typeof(string));

                //Get Inventory
                var ent = this._Model.v_wms_inventory_data_by_serial.Where(w => w.key_id == inv.key_id).FirstOrDefault();
                if (ent != null)
                {
                    Nullable<double> in_fltQty = ent.sn_control.ToUpper().Trim() == "FULL" ? -1 : inv.qty * -1;
                    Nullable<System.Guid> in_vchWhMasterID = ent.wh_master_id;
                    Nullable<System.Guid> in_vchOwnerID = ent.owner_id;
                    Nullable<System.Guid> in_vchInvStatusID = ent.inventory_status_id;
                    Nullable<System.Guid> in_vchLocationID = ent.location_id;
                    Nullable<System.Guid> in_vchWhItemMasterID = ent.wh_item_master_id;
                    string in_vchParentLPN = ent.parent_lpn;
                    string in_vchLPN = ent.lpn;
                    string in_vchLotNumber = ent.lot_number;
                    string in_vchExpiryDate = ent.expiry_date;
                    string in_vchAttribute1 = ent.attribute1;
                    string in_vchAttribute2 = ent.attribute2;
                    string in_vchAttribute3 = ent.attribute3;
                    string in_vchAttribute4 = ent.attribute4;
                    string in_vchAttribute5 = ent.attribute5;
                    string in_vchSerialNumber = ent.serial_number;
                    Nullable<System.Guid> in_vchItemUomID = ent.item_uom_id;
                    Nullable<System.DateTime> in_dtReceiveDate = ent.receive_date;
                    Nullable<System.DateTime> in_dtMfgDate = ent.mfg_date;


                    this._Model.usp_inventory_adjust_out(in_vchApplicationID, in_vchDeviceID, in_vchUserID, in_vchTransactionType, in_vchSubTranType, in_vchWhMasterID, in_vchOwnerID, in_vchInvStatusID, in_vchLocationID, in_vchWhItemMasterID, in_vchParentLPN, in_vchLPN, in_vchLotNumber, in_vchExpiryDate, in_vchAttribute1, in_vchAttribute2, in_vchAttribute3, in_vchAttribute4, in_vchAttribute5, in_vchSerialNumber, in_fltQty, in_vchItemUomID, in_vchReasonID, in_dtReceiveDate, ref_id, errCode, errMsg);

                    if (errCode.Value.ToString() == "0")
                    {
                        countSuccess++;
                    }
                    else
                    {
                        countError++;
                        error_msg.AppendLine(countError.ToString() + ". " + errMsg.Value.ToString());
                    }
                }
            }

            if (countError == 0)
            {
                this.MessageSuccess(this, "Save success.");
                return true;
            }
            else
            {
                error_success.AppendLine("Save success[" + countSuccess.ToString() + "]");
                error_success.AppendLine("Error[" + countError.ToString() + "]");
                error_success.AppendLine(error_msg.ToString());
                this.MessageWarning(this, error_success.ToString());
                return false;
            }

        }
        #endregion

    }
}
