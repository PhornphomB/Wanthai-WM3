using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace WMS_NEW.Transaction.Inventory
{
    public partial class InventoryChangeLocationAllLPN : PageCustom
    {
        #region ViewState
        private Guid wh_master_id
        {
            get
            {
                if (ViewState["wh_master_id"] == null)
                    ViewState["wh_master_id"] = Guid.Empty;

                return (Guid)ViewState["wh_master_id"];
            }
            set
            {
                ViewState["wh_master_id"] = value;
            }
        }

        private bool control_serial
        {
            get
            {
                if (ViewState["control_serial"] == null)
                    ViewState["control_serial"] = false;

                return (bool)ViewState["control_serial"];
            }
            set
            {
                ViewState["control_serial"] = value;
            }
        }

        private bool control_lpn
        {
            get
            {
                if (ViewState["control_lpn"] == null)
                    ViewState["control_lpn"] = false;

                return (bool)ViewState["control_lpn"];
            }
            set
            {
                ViewState["control_lpn"] = value;
            }
        }

        private bool control_parent_lpn
        {
            get
            {
                if (ViewState["control_parent_lpn"] == null)
                    ViewState["control_parent_lpn"] = false;

                return (bool)ViewState["control_parent_lpn"];
            }
            set
            {
                ViewState["control_parent_lpn"] = value;
            }
        }

        private bool is_multiple_item
        {
            get
            {
                if (ViewState["is_multiple_item"] == null)
                    ViewState["is_multiple_item"] = false;

                return (bool)ViewState["is_multiple_item"];
            }
            set
            {
                ViewState["is_multiple_item"] = value;
            }
        }

        private Guid filter_wh_master_id
        {
            get
            {
                if (ViewState["filter_wh_master_id"] == null)
                    ViewState["filter_wh_master_id"] = Guid.Empty;

                return (Guid)ViewState["filter_wh_master_id"];
            }
            set
            {
                ViewState["filter_wh_master_id"] = value;
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

                //iColZone.DropDownAutoPostBack = true;
                //iColZone.DropDownPostValueChanged = delegate (dynamic _value)
                //{
                //    this.grid_zone = _value;

                //    GridExt1.GridColumnRefreshFilter(iColLocation);
                //};

                //iColLocation.DropDownQueryProperty = delegate () { return Access.MasterData.Location.Instance.GetQuery_UserWarehouse(); };
                iColLocation.DropDownQueryProperty = delegate () { return Access.MasterData.Location.Instance.GetCodeQueryUserWarehouseRuleLocNotMove(); };

                iColOwner.DropDownQueryProperty = delegate () { return Access.MasterData.Owner.Instance.GetQuery_User(); };
                #endregion

                ddlLocation.MethodQueryProperty = delegate () { return Access.MasterData.Location.Instance.GetQuery_Change(this.wh_master_id); };

                if (!Page.IsPostBack)
                {

                    ddlReasonCode.MethodQueryProperty = delegate () { return Access.MasterData.Reason.Instance.GetQuery("ChangeLocation"); };
                    ddlReasonCode.BindDataSource();

                }
            }
            catch (Exception ex)
            {
                Logging = new Prototype.Providers.Logging(this, ex);
                RaiseLogging();
            }
        }

        private void ddlWarehouse_PostValueChanged(dynamic obj)
        {
            try
            {
                if (obj == null)
                {
                    return;
                }
                ddlLocation.Clear();
                this.wh_master_id = obj;

            }
            catch (Exception ex)
            {
                Logging = new Prototype.Providers.Logging(this, ex);
                RaiseLogging();
            }
        }


        protected void btnChangeLocation_Click(object sender, EventArgs e)
        {
            try
            {
                List<string> listKey = GridExt1.GetListKey().Where(w => w.Active == true).Select(s => s.KeyId.ToString()).ToList();
                if (listKey == null || listKey.Count == 0)
                {
                    Page.MessageWarning("Please select item.");
                    return;
                }


                ddlLocation.Clear();
                ddlReasonCode.Clear();
                txtRemark.Clear();

                //=====================

                List<string> listWh = listKey.Select(s => s.Split('|').First()).ToList();
                var countWH = listWh.Distinct().Count();

                if (countWH > 1)
                {
                    Page.MessageWarning("Warehouse is not the same !");
                    return;
                }


                string wh = listWh[0];
                this.wh_master_id = Guid.Parse(listWh[0]);
                string lpn = string.Empty;
                var listLPN = listKey.Select(s => s.Split('|').Last()).ToList();
                foreach (var item in listLPN)
                {
                    lpn += item + "|";
                }

                lpn = lpn.Substring(0, lpn.Length - 1);

                hidLPN.SetValue(lpn);
                hidWh.SetValue(wh);
                GridExt2.Search();
                popupChange.ShowDialog();
                popupChange.UpdateContent();
            }
            catch (Exception ex)
            {
                Logging = new Prototype.Providers.Logging(this, ex);
                RaiseLogging();
            }
        }

        protected void btnConfirmChange_Click(object sender, EventArgs e)
        {
            try
            {
                using (var acc = new Access.Transaction.Inventory.InventoryChangeLocationAllLPN())
                {
                    PlugEventResult(acc);
                    List<string> listKey = GridExt1.GetListKey().Where(w => w.Active == true).Select(s => s.KeyId.ToString()).ToList();
                    if (listKey == null || listKey.Count <= 0)
                    {
                        this.Page.MessageWarning("Please select item again !");
                        popupChange.HideDialog();
                        return;
                    }


                    Guid location_id = ddlLocation.GetValue();
                    Guid reason_id = ddlReasonCode.GetValue();
                    string remark = txtRemark.GetValue();

                    bool isSuccess = acc.Change_LocationAllLPN(listKey, location_id, reason_id, remark);
                    if (isSuccess)
                    {
                        popupChange.HideDialog();
                       // GridExt1.DataBind();
                        GridExt1.Search();
                    }
                }
            }
            catch (Exception ex)
            {
                Logging = new Prototype.Providers.Logging(this, ex);
                RaiseLogging();
            }
        }

    }

}