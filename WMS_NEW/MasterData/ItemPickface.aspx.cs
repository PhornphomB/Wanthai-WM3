using ICSharpCode.SharpZipLib.Zip;
using System;
using System.Collections.Generic;
using System.Data.Entity.Core.Objects;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using WMS_NEW.Source;

namespace WMS_NEW.MasterData
{
    public partial class ItemPickface : PageCustom
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            ddlWarehouse.MethodQueryProperty = delegate () { return Access.MasterData.Warehouse.Instance.GetQuery_User(_SessionVals.UserName); };
            ddlItemNumber.MethodQueryProperty = delegate () { return Access.MasterData.ItemPickface.Instance.GetQuery_Item(ddlWarehouse.GetValue()); };
            ddlWarehouse.PostValueChanged += ddlWarehouse_PostValueChanged;
            ddlItemNumber.PostValueChanged += ddlItemNumber_PostValueChanged;
            if (!IsPostBack)
            {
                ddlWarehouse.BindDataSource();
            }
        }


        void ddlWarehouse_PostValueChanged(dynamic _value)
        {
            try
            {
                if (_value != null)
                {
                    ddlItemNumber.BindDataSource();
                    Search();
                }
            }
            catch (Exception ex)
            {
                Logging = new Prototype.Providers.Logging(this, ex);
                RaiseLogging();
            }
        }
        void ddlItemNumber_PostValueChanged(dynamic _value)
        {
            try
            {
                if (_value != null)
                {
                    Search();
                }
            }
            catch (Exception ex)
            {
                Logging = new Prototype.Providers.Logging(this, ex);
                RaiseLogging();
            }
        }
        protected void btnAddLocation_Click(object sender, EventArgs e)
        {
            try
            {
                var _listSelect = gridLocationPickface.GetListKey().Select(se => se.KeyId.ToString()).ToArray();
                if (_listSelect.Length == 0)
                {
                    Page.MessageWarning("Please Select Location Pickface!");
                    return;
                }
                using (var _model = new WMSEntities())
                {
                    foreach (var item in _listSelect)
                    {
                        Guid _location_id = Guid.Parse(item);
                        var loc = _model.t_wms_location.Where(w => w.location_id == _location_id).FirstOrDefault();
                        if (loc != null)
                        {
                            t_wms_item_pickface _objItemPickface = new t_wms_item_pickface();
                            _objItemPickface.item_pickface_id = Guid.NewGuid();
                            _objItemPickface.location_id = loc.location_id;
                            _objItemPickface.wh_item_master_id = hidWhItemMasterId.GetValue();
                            _objItemPickface.create_by = _SessionVals.UserName;
                            _objItemPickface.create_date = DateTime.Now;
                            _model.t_wms_item_pickface.Add(_objItemPickface);
                            _model.SaveChanges();
                        }
                    }
                }

                gridLocationPickface.Search();
                gridLocationPickfaceItem.Search();
            }
            catch (Exception ex)
            {
                Logging = new Prototype.Providers.Logging(this, ex);
                RaiseLogging();
            }
        }


        protected void btnDeleteLocationItem_Click(object sender, EventArgs e)
        {
            try
            {
                var _listSelect = gridLocationPickfaceItem.GetListKey().Select(se => Guid.Parse(se.KeyId.ToString())).ToArray();
                if (_listSelect.Length == 0)
                {
                    Page.MessageWarning("Please Select Location Item Pickface!");
                    return;
                }
                using (var _model = new WMSEntities())
                {
                    var _listItemPickface = _model.t_wms_item_pickface.Where(w => _listSelect.Contains(w.item_pickface_id)).ToList();
                    _model.t_wms_item_pickface.RemoveRange(_listItemPickface);
                    _model.SaveChanges();
                }

                gridLocationPickface.Search();
                gridLocationPickfaceItem.Search();
            }
            catch (Exception ex)
            {
                Logging = new Prototype.Providers.Logging(this, ex);
                RaiseLogging();
            }
        }

        protected string gridLocationPickfaceItem_GridRowTextChanged(object _rowKeyValue, string _rowDataField, string _rowTextValue)
        {
            //if (string.IsNullOrEmpty(_rowTextValue))
            //    _rowTextValue = "-1";

            var defaultValue = _rowTextValue;

            try
            {
                using (var _acc = new Access.MasterData.LocationPickfaceItem())
                {
                    this.PlugEventResult(_acc);

                    string _type = "";
                    if (_rowDataField == "x")
                        _type = "Max";
                    else
                        _type = "Min";


                    var result = _acc.UpdateQty(Guid.Parse(_rowKeyValue.ToString()), Convert.ToDouble(_rowTextValue), _type);
                    if (result > 0)
                    {
                        defaultValue = result.ToString();
                    }
                }
            }
            catch (Exception ex)
            {
                Logging = new Prototype.Providers.Logging(this, ex);
                RaiseLogging();
            }

            return defaultValue;
        }

        protected void btnSearch_Click(object sender, EventArgs e)
        {
            if (ddlItemNumber.GetValue() == null || ddlItemNumber.GetValue() == Guid.Empty)
            {
                Page.MessageWarning("Please select Item Number!");
                return;
            }
            Search();
        }

        private void Search()
        {
            hidSessionUserItOrder.SetValue(_SessionVals.User.user_id);
            hidWhMasterId.SetValue(ddlWarehouse.GetValue());
            hidWhItemMasterId.SetValue(ddlItemNumber.GetValue() ?? Guid.Empty);
            hidSessionUserItInvent.SetValue(_SessionVals.User.user_id);
            hidWhMasterIdItem.SetValue(ddlWarehouse.GetValue());
            hidItemMasterLocPickItem.SetValue(ddlItemNumber.GetValue() ?? Guid.Empty);
            gridLocationPickface.Search();
            gridLocationPickfaceItem.Search();
        }

        protected void btnReplenishment_Click(object sender, EventArgs e)
        {
            var errCode = new ObjectParameter("out_ErrorCode", typeof(string));
            var errMsg = new ObjectParameter("out_ErrorMessage", typeof(string));

            using (var _model = new Source.WMSEntities())
            {
                _model.Configuration.EnsureTransactionsForFunctionsAndCommands = false;

                _model.usp_outbound_release_replenishment(_SessionVals.AppID, _SessionVals.DeviceID, _SessionVals.UserName, errCode, errMsg);

                if (errCode.Value.ToString() == "0")
                {

                    Page.MessageSuccess(errMsg.Value.ToString());
                }
                else
                {
                    Page.MessageWarning(errMsg.Value.ToString());
                }
            }
        }
    }
}