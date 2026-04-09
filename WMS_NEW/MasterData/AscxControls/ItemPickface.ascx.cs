using _UControls;
using System;
using System.Linq;
using System.Web.UI;
using WMS_NEW.Source;

namespace WMS_NEW.MasterData.AscxControls
{
    public partial class ItemPickface : UControlCustom
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            try
            {

                if (!Page.IsPostBack)
                {
                    hidSessionUserItOrder.SetValue(_SessionVals.UserName);
                    hidSessionUserItInvent.SetValue(_SessionVals.UserName);
                }
            }
            catch (Exception ex)
            {
                Logging = new Prototype.Providers.Logging(this, ex);
                RaiseLogging();
            }
        }


        public Action<dynamic> UpdateParent { get; set; }

        public void InitForm(Guid wh_item_master_id)
        {
            try
            {
                //var ent = (Source.t_wms_item)_obj;

                //txtItemNumber.SetValue(ent.item_number);
                //hidItemMasterId.SetValue(ent.item_master_id);

                //gridItemMasterId.SetValue(ent.item_master_id);
                hidItemMasterLocPick.SetValue(wh_item_master_id);
                hidItemMasterLocPickItem.SetValue(wh_item_master_id);

                hidSessionUserItOrder.SetValue(_SessionVals.UserName);
                hidSessionUserItInvent.SetValue(_SessionVals.UserName);
                
                gridLocationPickface.Search();
                gridLocationPickfaceItem.Search();
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
                            _objItemPickface.wh_item_master_id = hidItemMasterLocPick.GetValue();
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
    }
}