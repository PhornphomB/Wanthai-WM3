using System;
using System.Collections.Generic;
using System.Data.Entity.Validation;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using WMS_NEW.Source;

namespace WMS_NEW.Transaction.Inventory
{
    public partial class CancelReplenishment : PageCustom
    {
        public Guid wh_master_id
        {
            get
            {
                return (Guid)ViewState["wh_master_id"];
            }
            set
            {
                ViewState["wh_master_id"] = value;
            }
        }
        protected void Page_Load(object sender, EventArgs e)
        {
            iColWarehouse.DropDownPostValueChanged += iColWarehouse_DropDownPostValueChanged;

            iColWarehouse.DropDownQueryProperty = delegate () { return new Access.MasterData.Warehouse().GetQuery_User(_SessionVals.UserName); };
            iColOwner.DropDownQueryProperty = delegate () { return Access.MasterData.Owner.Instance.GetQuery(_SessionVals.UserName); };
            iColLocation.DropDownQueryProperty = delegate () { return Access.MasterData.Location.Instance.GetQuery(this.wh_master_id); };
            iColDesLocation.DropDownQueryProperty = delegate () { return Access.MasterData.Location.Instance.GetQuery(this.wh_master_id); };
            iColItem.DropDownQueryProperty = delegate () { return Access.MasterData.Item.Instance.GetQuery(); };
            iColReplenish.DropDownQueryProperty = delegate () { return Access.MasterData.Item.Instance.GetQuery(); };
            iColReplenish.DropDownQueryProperty = delegate () { return new Access.Configuration.ComboBox().GetQueryCode("confirm_yesno"); };
            GridExt1.GridRowAfterDataBound += GridExt1_GridRowAfterDataBound;
            if (!IsPostBack)
            {
                var ucFirstLoad = new _UControls.InputHidden() { DataFieldValue = "_is_first_load" };
                ucFirstLoad.SetValue(true);
                GridExt1.AddFilterCustomInputInclude(ucFirstLoad);
                GetWhMasterId();
            }
        }
        private void iColWarehouse_DropDownPostValueChanged(dynamic obj)
        {
            try
            {
                GridExt1.ClearFilter(iColLocation);
                GridExt1.ClearFilter(iColDesLocation);

                if (obj == null)
                {
                    this.wh_master_id = Guid.Empty;
                    return;
                }

                this.wh_master_id = (Guid)obj;
            }
            catch (Exception ex)
            {
                Logging = new Prototype.Providers.Logging(this, ex);
                RaiseLogging();
            }
        }
        private void GetWhMasterId()
        {
            try
            {
                using (var _Model = new WMSEntities())
                {
                    var mapping = _Model.v_wms_mapping_user_warehouse.Where(x => x.user_id == _SessionVals.UserName && x.is_active == true);
                    if (mapping.Count() == 1)
                    {
                        this.wh_master_id = mapping.FirstOrDefault().wh_master_id;
                    }
                }
            }
            catch (Exception ex)
            {
                Logging = new Prototype.Providers.Logging(this, ex);
                RaiseLogging();
            }
        }
        private void GridExt1_GridRowAfterDataBound(GridViewRowEventArgs e)
        {
            try
            {
                var cmdSelect = (CheckBox)e.Row.FindControl("cmdSelect");
                var is_replenish = (string)DataBinder.Eval(e.Row.DataItem, "is_replenish");
                var stg_location_id = (Guid?)DataBinder.Eval(e.Row.DataItem, "stg_location_id");
                //ปิดปุ่ม UncheckAll
                if (is_replenish == "YES" || stg_location_id != null)
                {
                    cmdSelect.Enabled = false;
                }
            }
            catch (Exception ex)
            {
                Logging = new Prototype.Providers.Logging(this, ex);
                RaiseLogging();
            }
        }

        protected void btnCancel_Click(object sender, EventArgs e)
        {
            var Keys = GridExt1.GetListKey();
            List<string> strings = new List<string>();
            for (int i = 0; i < Keys.Count; i++)
            {
                strings.Add(Keys[i].KeyId.ToString());
            }
            if (strings.Count > 0)
            {
                CancelReplenishmentList(strings);
            }
            else
            {
                Page.MessageWarning("Please select at least one record to cancel.");
            }
        }
        public void CancelReplenishmentList(List<string> replenishmentTaskIds)
        {
            using(var _Model = new WMSEntities())
            {
                foreach (var taskId in replenishmentTaskIds)
                {
                    var keyId = Guid.Parse(taskId);
                    var task = _Model.t_wms_replenishment_task.FirstOrDefault(t => t.replenishment_task_id == keyId);
                    if (task != null)
                    {
                        var inventory = _Model.t_wms_inventory.FirstOrDefault(i => i.inventory_id == task.inventory_id);
                        if (inventory != null)
                        {
                            inventory.quantity_allocated = 0;
                            _Model.Entry(inventory).State = System.Data.Entity.EntityState.Modified;
                        }
                        InsertLog(_Model,task);
                        _Model.t_wms_replenishment_task.Remove(task);
                    }

                }
                try
                {
                    _Model.SaveChanges();
                    Page.MessageSuccess("Replenishment tasks cancelled successfully.");
                    GridExt1.Search();
                }
                catch (DbEntityValidationException ex)
                {
                    foreach (var validationErrors in ex.EntityValidationErrors)
                    {
                        foreach (var validationError in validationErrors.ValidationErrors)
                        {
                            Page.MessageWarning($"Validation error: {validationError.ErrorMessage}");
                            Logging = new Prototype.Providers.Logging(this, new Exception(validationError.ErrorMessage));
                            RaiseLogging();
                        }
                    }
                }
                catch (Exception ex)
                {
                    Page.MessageWarning($"Error cancelling replenishment tasks: {ex.Message}");
                    Logging = new Prototype.Providers.Logging(this, ex);
                    RaiseLogging();
                }
            }
        }
        private void InsertLog(WMSEntities _Model, t_wms_replenishment_task task)
        {
            var now = DateTime.Now;
            var expiryDate = (DateTime?)null;
            if (!string.IsNullOrWhiteSpace(task.expiry_date))
            {
                DateTime parsedExpiryDate;
                if (DateTime.TryParseExact(
                        task.expiry_date,
                        "yyyyMMdd",
                        System.Globalization.CultureInfo.InvariantCulture,
                        System.Globalization.DateTimeStyles.None,
                        out parsedExpiryDate))
                {
                    expiryDate = parsedExpiryDate;
                }
            }

            t_com_tran_log log = new t_com_tran_log
            {
                tran_id = Guid.NewGuid(),
                app_id = _SessionVals.AppID,
                tran_type = "REPLENISHMENT",
                sub_tran_type = "CANCEL_REPLENISHMENT",
                app_name = "WM3",
                description = "Cancelled replenishment task.",
                start_tran_datetime = now,
                end_tran_datetime = now,
                warehouse = task.wh_id,
                to_warehouse = task.wh_id,
                location = task.source_location,
                to_location = task.destination_location,
                owner = task.owner_code,
                to_owner = task.owner_code,
                item_number = task.item_number,
                quantity = task.quantity,
                quantity_uom = task.uom,
                after_quantity = task.quantity,
                after_quantity_uom = task.uom,
                lot_number = task.lot_number,
                after_lot_number = task.lot_number,
                expiry_date = expiryDate,
                after_expiry_date = expiryDate,
                attribute1 = task.attribute1,
                after_attribute1 = task.attribute1,
                attribute2 = task.attribute2,
                after_attribute2 = task.attribute2,
                attribute3 = task.attribute3,
                after_attribute3 = task.attribute3,
                attribute4 = task.attribute4,
                after_attribute4 = task.attribute4,
                attribute5 = task.attribute5,
                after_attribute5 = task.attribute5,
                user_id = task.create_by,
                lpn = task.lpn,
                after_lpn = task.lpn,
                receive_date = task.receive_date,
                mfg_date = task.mfg_date,
                status = task.inventory_status,
                after_status = task.inventory_status,
                zone = task.source_zone,
                to_zone = task.destination_zone,
                device = _SessionVals.DeviceID,
                create_by = _SessionVals.UserName,
                create_date = DateTime.Now
            };
            _Model.t_com_tran_log.Add(log);
        }
    }
}