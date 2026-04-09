using Prototype.Providers;
using System;
using System.Collections.Generic;
using System.Data.Entity.Core.Objects;
using System.Linq;
using System.Text;
using WMS_NEW.Source;

namespace WMS_NEW.Access.Transaction.Inventory
{
    public class InventoryChangeSerial : AGridObjectSourceQuery, IDisposable
    {
        public WMSEntities _Model { get; set; }

        public InventoryChangeSerial()
        {
            this._Model = new WMSEntities();
            this._Model.Configuration.EnsureTransactionsForFunctionsAndCommands = false;

        }


        #region ++INSTANCE STATIC++
        public static InventoryChangeSerial Instance
        {
            get
            {
                using (InventoryChangeSerial _Instance = new InventoryChangeSerial())
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

            var result = from rows in this._Model.v_wms_inventory_data_by_serial
                         where listWarehouse.Contains(rows.wh_master_id)
                         && listOwner.Contains(rows.owner_id)
                         && rows.sn_control.ToUpper() == "FULL"
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
                             rows.lot_control,
                             rows.expiry_date_control
                         };

            if (this.FilterCustom != null)
            {
                var entLotExp = this.FilterCustom.Where(w => w.DataFieldValue == "_control_lot_exp").FirstOrDefault();
                if (entLotExp != null && entLotExp.Value != null)
                {
                    if ((bool)entLotExp.Value)
                    {
                        result = result.Where(w =>  w.lot_control.ToUpper() == "FULL" || w.expiry_date_control.ToUpper() == "FULL");
                    }
                }
            }

            return result;
        }

        public override IQueryable<dynamic> InitialQueryExport()
        {
            return InitialQueryView();
        }

        public bool Serial_Change(Nullable<System.Guid> in_vchInventoryID, string in_vchOldSerialNumber, string in_vchNewSerialNumber, Nullable<System.Guid> in_vchReasonID, string _remark)
        {
            try
            {
                string in_vchApplicationID = _SessionVals.AppID;
                string in_vchDeviceID = _SessionVals.DeviceID;
                string in_vchUserID = _SessionVals.UserName;

                var errCode = new ObjectParameter("out_ErrorCode", typeof(string));
                var errMsg = new ObjectParameter("out_ErrorMessage", typeof(string));
                //Reference Number
                var refID = new ObjectParameter("v_vchReferenceID", typeof(string));
                this._Model.usp_get_ReferenceID(refID);
                string in_vchReferenceNumber = refID.Value.ToString();

                this._Model.usp_inventory_change_serial(in_vchApplicationID, in_vchInventoryID, in_vchOldSerialNumber, in_vchNewSerialNumber, in_vchReasonID, _remark, in_vchReferenceNumber, in_vchDeviceID, in_vchUserID, errCode, errMsg);

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
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public bool Serial_Change_Location(List<string> _listKey, Nullable<System.Guid> in_vchToWhMasterID, Nullable<System.Guid> in_vchToLocationID, string in_vchToParentLPN, string in_vchToLPN, Nullable<System.Guid> in_vchReasonID, string in_vchRemark)
        {
            try
            {
                int countSuccess = 0;
                int countError = 0;
                string success_msg = string.Empty;
                StringBuilder error_msg = new StringBuilder();
                StringBuilder error_success = new StringBuilder();


                string in_vchApplicationID = _SessionVals.AppID;
                string in_vchDeviceID = _SessionVals.DeviceID;
                string in_vchUserID = _SessionVals.UserName;

                var errCode = new ObjectParameter("out_ErrorCode", typeof(string));
                var errMsg = new ObjectParameter("out_ErrorMessage", typeof(string));
                //Reference Number
                var refID = new ObjectParameter("v_vchReferenceID", typeof(string));
                this._Model.usp_get_ReferenceID(refID);
                string in_vchReferenceNumber = refID.Value.ToString();

                Nullable<double> in_fltQty = 1;

                foreach (string item in _listKey)
                {
                    string in_vchSerialNumber = item.Split('|').Last().ToString();
                    Guid in_vchInventoryID = Guid.Parse(item.Split('|').First().ToString());

                    this._Model.usp_inventory_change_location_by_serial(in_vchApplicationID, in_vchInventoryID, in_vchToWhMasterID, in_vchToLocationID, in_vchToParentLPN, in_vchToLPN, in_vchSerialNumber, in_fltQty, in_vchReasonID, in_vchRemark, in_vchReferenceNumber, in_vchDeviceID, in_vchUserID, errCode, errMsg);


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

        public bool Serial_Change_Status(List<string> _listKey, Nullable<System.Guid> in_vchToInvStatusID, Nullable<double> in_fltQty, Nullable<System.Guid> in_vchReasonID, string _remark)
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
                foreach (string item in _listKey)
                {
                    string in_vchSerialNumber = item.Split('|').Last().ToString();
                    Guid in_vchInventoryID = Guid.Parse(item.Split('|').First().ToString());

                    var errCode = new ObjectParameter("out_ErrorCode", typeof(string));
                    var errMsg = new ObjectParameter("out_ErrorMessage", typeof(string));

                    this._Model.usp_inventory_change_status_by_serial(in_vchApplicationID, in_vchInventoryID, in_vchSerialNumber, in_vchToInvStatusID, in_fltQty, in_vchReasonID, _remark, ref_id, in_vchDeviceID, in_vchUserID, errCode, errMsg);

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

        public bool Serial_Change_Lot_Exp(List<string> _listKey, Nullable<System.DateTime> in_dtToMfgDate, string in_vchToExpiryDate, string in_vchToLotNumber, Nullable<double> in_fltQty, Nullable<System.Guid> in_vchReasonID, string in_vchRemark)
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
                string in_vchReferenceNumber = refID.Value.ToString();
                string success_msg = string.Empty;

                foreach (var item in _listKey)
                {
                    var errCode = new ObjectParameter("out_ErrorCode", typeof(string));
                    var errMsg = new ObjectParameter("out_ErrorMessage", typeof(string));

                    Nullable<System.Guid> in_vchInventoryID = Guid.Parse(item.Split('|').First().Trim());
                    string in_vchSerialNumber = item.Split('|').Last().ToString().Trim();

                    this._Model.usp_inventory_change_lot_exp_by_serial(in_vchApplicationID, in_vchInventoryID, in_vchSerialNumber, in_dtToMfgDate, in_vchToExpiryDate, in_vchToLotNumber, in_fltQty, in_vchReasonID, in_vchRemark, in_vchReferenceNumber, in_vchDeviceID, in_vchUserID, errCode, errMsg);

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

        public bool ValidateControlLotExpDate(List<string> _listKey, out v_wms_inventory_data_by_serial ent)
        {

            var result = this._Model.v_wms_inventory_data_by_serial.Where(w => _listKey.Contains(w.key_id));

            ent = result.FirstOrDefault();
            if (ent != null)
            {
                string _is_lot = ent.lot_control.ToUpper();
                string _is_exp = ent.expiry_date_control.ToUpper();
                return !result.Any(a => a.lot_control != _is_lot || a.expiry_date_control != _is_exp);
            }

            return false;
        }
    }

}
