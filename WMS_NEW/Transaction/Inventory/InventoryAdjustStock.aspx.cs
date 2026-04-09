using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace WMS_NEW.Transaction.Inventory
{
    public partial class InventoryAdjustStock : PageCustom
    {
        #region ViewState

        public List<Access.Transaction.Inventory.DTO_Adjust> list_adjust
        {
            get
            {
                if (ViewState["list_adjust"] == null)
                    ViewState["list_adjust"] = new List<Access.Transaction.Inventory.DTO_Adjust>();

                return (List<Access.Transaction.Inventory.DTO_Adjust>)ViewState["list_adjust"];
            }
            set
            {
                ViewState["list_adjust"] = value;
            }
        }

        Guid grid_wh_master_id
        {
            get
            {
                if (ViewState["grid_wh_master_id"] == null)
                    ViewState["grid_wh_master_id"] = Guid.Empty;

                return (Guid)ViewState["grid_wh_master_id"];
            }
            set
            {
                ViewState["grid_wh_master_id"] = value;
            }
        }

        string grid_zone
        {
            get
            {
                if (ViewState["grid_zone"] == null)
                    ViewState["grid_zone"] = string.Empty;

                return (string)ViewState["grid_zone"];
            }
            set
            {
                ViewState["grid_zone"] = value;
            }
        }

        #endregion

        protected void Page_Load(object sender, EventArgs e)
        {
            try
            {
                #region Grid Column

                iColWarehouse.DropDownQueryProperty = delegate () { return Access.MasterData.Warehouse.Instance.GetQuery_User(); };
                //iColWarehouse.DropDownAutoPostBack = true;
                //iColWarehouse.DropDownPostValueChanged = delegate (dynamic _value)
                //{
                //    this.grid_wh_master_id = _value ?? Guid.Empty;

                //    GridExt1.GridColumnRefreshFilter(iColZone);
                //};

                iColZone.DropDownQueryProperty = delegate () { return Access.MasterData.Zone.Instance.GetQueryCode_UserWarehouse(); };
                //iColZone.DropDownAutoPostBack = true;
                //iColZone.DropDownPostValueChanged = delegate (dynamic _value)
                //{
                //    this.grid_zone = _value;

                //    GridExt1.GridColumnRefreshFilter(iColLocation);
                //};

                //iColLocation.DropDownQueryProperty = delegate () { return Access.MasterData.Location.Instance.GetQuery_UserWarehouse(); };
                iColLocation.DropDownQueryProperty = delegate () { return Access.MasterData.Location.Instance.GetCodeQueryUserWarehouseRuleLocNotMove(); };

                iColOwner.DropDownQueryProperty = delegate () { return Access.MasterData.Owner.Instance.GetQuery_User(); };

                iColItem.DropDownQueryProperty = delegate () { return Access.MasterData.Item.Instance.GetQuery(); };
                iColCategory.DropDownQueryProperty = delegate () { return Access.MasterData.ItemCategory.Instance.GetQuery(); };
                iColInvStatus.DropDownQueryProperty = delegate () { return Access.Transaction.Inventory.InventoryStatus.Instance.GetQuery(); };

                #endregion

                GridExt2.GridRowAfterDataBound += GridExt2_GridRowAfterDataBound;
                GridExt2.GridRowTextChanged += GridExt2_GridRowTextChanged;

                if (!Page.IsPostBack)
                {
                    ddlAdjustType.MethodQueryProperty = delegate () { return Access.Configuration.ComboBox.Instance.GetQueryCode("adjust_type"); };
                    ddlAdjustType.BindDataSource();

                    ddlAdjustFunction.MethodQueryProperty = delegate () { return Access.Configuration.ComboBox.Instance.GetQueryCode("adjust_function"); };
                    ddlAdjustFunction.BindDataSource();


                    //ddlInvStatus.MethodQueryProperty = delegate () { return Access.Transaction.Inventory.InventoryStatus.Instance.GetQuery(); };
                    //ddlInvStatus.BindDataSource();

                    ddlReasonCode.MethodQueryProperty = delegate () { return Access.MasterData.Reason.Instance.GetQuery("adjustment"); };
                    ddlReasonCode.BindDataSource();
                }
            }
            catch (Exception ex)
            {
                Logging = new Prototype.Providers.Logging(this, ex);
                RaiseLogging();
            }
        }

        private void GridExt2_GridRowAfterDataBound(System.Web.UI.WebControls.GridViewRowEventArgs e)
        {
            try
            {
                var txtQty = (TextBox)e.Row.FindControl("adjust_qty");
                if (txtQty != null)
                {
                    txtQty.Text = "0.0";
                    txtQty.Enabled = true;


                    var isSerial = (string)DataBinder.Eval(e.Row.DataItem, "sn_control");
                    if (isSerial == "FULL")
                    {
                        txtQty.Text = "1";
                        txtQty.Enabled = false;
                    }
                }
            }
            catch (Exception ex)
            {
                Logging = new Prototype.Providers.Logging(this, ex);
                RaiseLogging();
            }
        }

        private string GridExt2_GridRowTextChanged(object _rowKeyValue, string _rowDataField, string _rowTextValue)
        {
            var key_id = _rowKeyValue.ToString();

            try
            {
                var result = this.list_adjust.Where(qry => qry.key_id == key_id).FirstOrDefault();

                if (result != null)
                {
                    double adjust_quantity = Convert.ToDouble(_rowTextValue);
                    result.qty = adjust_quantity;
                }

                return _rowTextValue;

            }
            catch (Exception ex)
            {
                Logging = new Prototype.Providers.Logging(this, ex);
                RaiseLogging();
            }
            return _rowTextValue;
        }

        protected void btnConfirmAdjust_Click(object sender, EventArgs e)
        {
            try
            {
                using (var acc = new Access.Transaction.Inventory.InventoryAdjust())
                {
                    PlugEventResult(acc);
                    if (ddlAdjustType.GetValue() == "+")
                    {
                        //Adjust In
                        acc.Adjust_QuantityPlus(this.list_adjust, ddlAdjustFunction.GetValue(), ddlReasonCode.GetValue());
                        popupAdjust.HideDialog();
                        GridExt1.DataBind();

                    }
                    else
                    {
                        acc.Adjust_QuantityMinus(this.list_adjust, ddlAdjustFunction.GetValue(), ddlReasonCode.GetValue());
                        popupAdjust.HideDialog();
                        GridExt1.Search();
                        //GridExt1.DataBind();
                    }
                }
            }
            catch (Exception ex)
            {
                Logging = new Prototype.Providers.Logging(this, ex);
                RaiseLogging();
            }
        }

        protected void btnAdjustStock_Click(object sender, EventArgs e)
        {
            try
            {
                this.list_adjust = (from rows in GridExt1.GetListKey()
                                    where rows.Active == true
                                    select new WMS_NEW.Access.Transaction.Inventory.DTO_Adjust
                                    {
                                        key_id = rows.KeyId.ToString(),
                                        qty = 0
                                    }).ToList();

                if (list_adjust == null || list_adjust.Count() == 0)
                {
                    Page.MessageWarning("Please select item.");
                    return;
                }

                ddlAdjustFunction.Clear();
                ddlAdjustType.Clear();
                // ddlInvStatus.Clear();
                ddlReasonCode.Clear();
                popupAdjust.ShowDialog();

                using (var _acc = new WMS_NEW.Access.Transaction.Inventory.InventoryAdjust())
                {
                    base.PlugEventResult(_acc);
                    var list_key = this.list_adjust.Select(s => s.key_id).ToList();
                    hidListKey.SetValue(list_key);
                    GridExt2.Search();
                }

                Control_Validate();

            }
            catch (Exception ex)
            {
                Logging = new Prototype.Providers.Logging(this, ex);
                RaiseLogging();
            }
        }

        void Control_Validate()
        {
            ddlAdjustType.Enabled = true;

            using (var model = new Source.WMSEntities())
            {
                var list = this.list_adjust.Select(s => s.key_id).ToList();
                bool isSerial = model.v_wms_inventory_data_by_serial.Any(a => list.Contains(a.key_id) && a.sn_control == "FULL");
                if (isSerial)
                {
                    ddlAdjustType.SetValue("-");
                    ddlAdjustType.Enabled = false;
                }

            }
        }
    }
}