using Prototype.Providers;
using System;
using System.Collections.Generic;
using System.Data.Entity.Core.Objects;
using System.Linq;
using System.Text;
using WMS_NEW.Source;

namespace WMS_NEW.Access.Transaction.Inventory
{
    public class InventoryChange : AGridObjectSourceQuery, IDisposable
    {
        public WMSEntities _Model { get; set; }

        public InventoryChange()
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
        public void Dispose()
        {
            GC.SuppressFinalize(this);
        }

        #endregion


        public override IQueryable<dynamic> InitialQueryView()
        {
            string user_id = _SessionVals.UserName;
            var listWarehouse = this._Model.t_wms_wh_user.Where(w => w.user_id == user_id).Select(s => s.wh_master_id).ToList();
            var listOwner = this._Model.t_wms_owner_user.Where(w => w.user_id == user_id).Select(s => s.owner_id).ToList();
            var listLoc = this._Model.t_wms_rule.Where(w => w.rule_code == "LOCATION_TYPE_NOT_MOVE_ITEM" && w.is_active == "YES").Select(s => s.value).ToList();

            var result = from rows in this._Model.v_wms_inventory_data
                         where listWarehouse.Contains(rows.wh_master_id)
                         && listOwner.Contains(rows.owner_id)
                         && !listLoc.Contains(rows.loc_type)
                         select new
                         {
                             KeyId = rows.inventory_id,
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
                             rows.quantity,
                             rows.quantity_allocated,
                             rows.quantity_incoming,
                             rows.receive_date,
                             rows.mfg_date,
                             rows.inv_status,
                             rows.inventory_status_id,
                             rows.location,
                             rows.loc_type,
                             rows.pick_sequence,
                             rows.pick_area_id,
                             rows.alternate_item_number,
                             rows.lot_number,
                             rows.expiry_date,
                             rows.exp_date,
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
                             rows.expiry_date_control,
                             rows.lot_control
                         };

            if (this.FilterCustom != null)
            {
                var entLot = this.FilterCustom.Where(w => w.DataFieldValue == "_control_lot").FirstOrDefault();
                if (entLot != null && entLot.Value != null)
                {
                    if ((bool)entLot.Value)
                    {
                        result = result.Where(w => w.lot_control.ToUpper().Trim() == "FULL");
                    }
                }

                var entExp = this.FilterCustom.Where(w => w.DataFieldValue == "_control_exp_date").FirstOrDefault();
                if (entExp != null && entExp.Value != null)
                {
                    if ((bool)entExp.Value)
                    {
                        result = result.Where(w => w.expiry_date_control.ToUpper().Trim() == "FULL");
                    }
                }
            }

            return result;
        }

        public override IQueryable<dynamic> InitialQueryExport()
        {
            return InitialQueryView();
        }

        public bool Change_Status(List<Guid> _listKey, Nullable<System.Guid> in_vchToInvStatusID, Nullable<double> in_fltQty, Nullable<System.Guid> in_vchReasonID, string _remark)
        {
            try
            {
                int countSuccess = 0;
                int countError = 0;
                string in_vchApplicationID = _SessionVals.AppID;
                string in_vchDeviceID = _SessionVals.DeviceID;
                string in_vchUserID = _SessionVals.UserName;

                StringBuilder error_msg = new StringBuilder();
                StringBuilder error_success = new StringBuilder();
                //Reference Number
                var refID = new ObjectParameter("v_vchReferenceID", typeof(string));
                this._Model.usp_get_ReferenceID(refID);
                string ref_id = refID.Value.ToString();

                string success_msg = string.Empty;
                foreach (var inv_id in _listKey)
                {
                    var errCode = new ObjectParameter("out_ErrorCode", typeof(string));
                    var errMsg = new ObjectParameter("out_ErrorMessage", typeof(string));

                    this._Model.usp_inventory_change_status(in_vchApplicationID, inv_id, in_vchToInvStatusID, in_fltQty, in_vchReasonID, _remark, ref_id, in_vchDeviceID, in_vchUserID, errCode, errMsg);

                    if (errCode.Value.ToString() == "0")
                    {
                        countSuccess++;
                        success_msg = errMsg.Value.ToString();
                    }
                    else
                    {
                        countError++;
                        error_msg.AppendLine(countError.ToString() + ". " + errMsg.Value.ToString());
                    }
                }

                if (countError == 0)
                {
                    this.MessageSuccess(this, success_msg);
                    return true;
                }
                else
                {
                    if (_listKey.Count == 1)
                    {
                        this.MessageWarning(this, error_msg.ToString());
                    }
                    else
                    {
                        error_success.AppendLine("Save success[" + countSuccess.ToString() + "]");
                        error_success.AppendLine("Error[" + countError.ToString() + "]");
                        error_success.AppendLine(error_msg.ToString());
                        this.MessageWarning(this, error_success.ToString());
                    }
                    return false;
                }
            }
            catch (Exception ex)
            {

                throw ex;
            }
        }
        public bool Change_Location(List<Guid> _listKey, Nullable<System.Guid> in_vchToWhMasterID, Nullable<System.Guid> in_vchToLocationID, string in_vchToParentLPN, string in_vchToLPN, Nullable<double> in_fltQty, Nullable<System.Guid> in_vchReasonID, string _remark, bool stay_ParentLPN, bool stay_LPN)
        {
            int countSuccess = 0;
            int countError = 0;
            string in_vchApplicationID = _SessionVals.AppID;
            string in_vchDeviceID = _SessionVals.DeviceID;
            string in_vchUserID = _SessionVals.UserName;


            StringBuilder error_msg = new StringBuilder();
            StringBuilder error_success = new StringBuilder();
            //Reference Number
            var refID = new ObjectParameter("v_vchReferenceID", typeof(string));
            this._Model.usp_get_ReferenceID(refID);
            string ref_id = refID.Value.ToString();
            string success_msg = string.Empty;

            foreach (var inv_id in _listKey)
            {
                var errCode = new ObjectParameter("out_ErrorCode", typeof(string));
                var errMsg = new ObjectParameter("out_ErrorMessage", typeof(string));

                if (stay_ParentLPN)
                {
                    var lpn = _Model.t_wms_inventory.Where(x => x.inventory_id == inv_id).FirstOrDefault();
                    in_vchToParentLPN = lpn.parent_lpn;
                }
                if (stay_LPN)
                {
                    var lpn = _Model.t_wms_inventory.Where(x => x.inventory_id == inv_id).FirstOrDefault();
                    in_vchToLPN = lpn.lpn;
                    //in_vchToParentLPN = lpn.lpn;
                }

                this._Model.usp_inventory_change_location(in_vchApplicationID, inv_id, in_vchToWhMasterID, in_vchToLocationID, in_vchToParentLPN, in_vchToLPN, in_fltQty, in_vchReasonID, _remark, ref_id, in_vchDeviceID, in_vchUserID, errCode, errMsg);

                if (errCode.Value.ToString() == "0")
                {
                    countSuccess++;
                    success_msg = errMsg.Value.ToString();
                }
                else
                {
                    countError++;
                    error_msg.AppendLine(countError.ToString() + ". " + errMsg.Value.ToString());
                }
            }

            if (countError == 0)
            {
                this.MessageSuccess(this, success_msg);
                return true;
            }
            else
            {
                if (_listKey.Count == 1)
                {
                    this.MessageWarning(this, error_msg.ToString());
                }
                else
                {
                    error_success.AppendLine("Save success[" + countSuccess.ToString() + "]");
                    error_success.AppendLine("Error[" + countError.ToString() + "]");
                    error_success.AppendLine(error_msg.ToString());
                    this.MessageWarning(this, error_success.ToString());
                }
                return false;
            }

        }
        public bool Change_Lot(List<Guid> _listKey, string in_vchToLotNumber, Nullable<double> in_fltQty, Nullable<System.Guid> in_vchReasonID, string _remark)
        {
            try
            {
                int countSuccess = 0;
                int countError = 0;
                string in_vchApplicationID = _SessionVals.AppID;
                string in_vchDeviceID = _SessionVals.DeviceID;
                string in_vchUserID = _SessionVals.UserName;


                StringBuilder error_msg = new StringBuilder();
                StringBuilder error_success = new StringBuilder();
                //Reference Number
                var refID = new ObjectParameter("v_vchReferenceID", typeof(string));
                this._Model.usp_get_ReferenceID(refID);
                string ref_id = refID.Value.ToString();
                string success_msg = string.Empty;

                foreach (var inv_id in _listKey)
                {
                    var errCode = new ObjectParameter("out_ErrorCode", typeof(string));
                    var errMsg = new ObjectParameter("out_ErrorMessage", typeof(string));

                    this._Model.usp_inventory_change_lot(in_vchApplicationID, inv_id, in_vchToLotNumber, in_fltQty, in_vchReasonID, _remark, ref_id, in_vchDeviceID, in_vchUserID, errCode, errMsg);

                    if (errCode.Value.ToString() == "0")
                    {
                        countSuccess++;
                        success_msg = errMsg.Value.ToString();
                    }
                    else
                    {
                        countError++;
                        error_msg.AppendLine(countError.ToString() + ". " + errMsg.Value.ToString());
                    }
                }

                if (countError == 0)
                {
                    this.MessageSuccess(this, success_msg);
                    return true;
                }
                else
                {
                    if (_listKey.Count == 1)
                    {
                        this.MessageWarning(this, error_msg.ToString());
                    }
                    else
                    {
                        error_success.AppendLine("Save success[" + countSuccess.ToString() + "]");
                        error_success.AppendLine("Error[" + countError.ToString() + "]");
                        error_success.AppendLine(error_msg.ToString());
                        this.MessageWarning(this, error_success.ToString());
                    }
                    return false;
                }
            }
            catch (Exception ex)
            {

                throw ex;
            }
        }
        public bool Change_ExpiryDate(List<Guid> _listKey, Nullable<System.DateTime> in_dtToMfgDate, string in_vchToExpiryDate, Nullable<double> in_fltQty, Nullable<System.Guid> in_vchReasonID, string _remark)
        {
            try
            {
                int countSuccess = 0;
                int countError = 0;
                string in_vchApplicationID = _SessionVals.AppID;
                string in_vchDeviceID = _SessionVals.DeviceID;
                string in_vchUserID = _SessionVals.UserName;


                StringBuilder error_msg = new StringBuilder();
                StringBuilder error_success = new StringBuilder();
                //Reference Number
                var refID = new ObjectParameter("v_vchReferenceID", typeof(string));
                this._Model.usp_get_ReferenceID(refID);
                string ref_id = refID.Value.ToString();
                string success_msg = string.Empty;

                foreach (var inv_id in _listKey)
                {
                    var errCode = new ObjectParameter("out_ErrorCode", typeof(string));
                    var errMsg = new ObjectParameter("out_ErrorMessage", typeof(string));

                    this._Model.usp_inventory_change_expiry_date(in_vchApplicationID, inv_id, in_dtToMfgDate, in_vchToExpiryDate, in_fltQty, in_vchReasonID, _remark, ref_id, in_vchDeviceID, in_vchUserID, errCode, errMsg);

                    if (errCode.Value.ToString() == "0")
                    {
                        countSuccess++;
                        success_msg = errMsg.Value.ToString();
                    }
                    else
                    {
                        countError++;
                        error_msg.AppendLine(countError.ToString() + ". " + errMsg.Value.ToString());
                    }
                }

                if (countError == 0)
                {
                    this.MessageSuccess(this, success_msg);
                    return true;
                }
                else
                {
                    if (_listKey.Count == 1)
                    {
                        this.MessageWarning(this, error_msg.ToString());
                    }
                    else
                    {
                        error_success.AppendLine("Save success[" + countSuccess.ToString() + "]");
                        error_success.AppendLine("Error[" + countError.ToString() + "]");
                        error_success.AppendLine(error_msg.ToString());
                        this.MessageWarning(this, error_success.ToString());
                    }
                    return false;
                }
            }
            catch (Exception ex)
            {

                throw ex;
            }
        }


    }



}
