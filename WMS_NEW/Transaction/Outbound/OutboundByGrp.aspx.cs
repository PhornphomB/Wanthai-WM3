using System;
using System.Collections.Generic;
using System.Data.Entity.Core.Objects;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using WMS_NEW.Source;

namespace WMS_NEW.Transaction.Outbound
{
    public partial class OutboundByGrp : PageCustom
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            try
            {
                GridExt1.GridSearching += GridExt1_GridSearching;
                GridExt1.GridRowEdit += GridExt1_GridRowEdit;
                GridExt1.GridRowCommandClick += GridExt1_GridRowCommandClick;
                GridExt1.GridRowAfterDataBound += GridExt1_GridRowAfterDataBound;

                gridOutboundMaster.GridRowExtendClick += GridOutboundMaster_GridRowExtendClick;
                gridOutboundMaster.GridRowDelete += GridOutboundMaster_GridRowDelete;
                gridOutboundMaster.GridRowCanDeleteValidate += GridOutboundMaster_GridRowCanDeleteValidate;

                chkGenOrderNo.PostValueChanged += chkGenOrderNo_PostValueChanged;

                popupLoadData.CloseClick += PopupLoadData_CloseClick;
                popupConfirmRelease.CloseClick += popupConfirmRelease_CloseClick;

                popupPickDetail.CloseClick += PopupPickDetail_CloseClick;
                #region Initial Peoperty Column Grid

                GridColumnExt1.DropDownQueryProperty = delegate () { return Access.MasterData.Warehouse.Instance.GetQueryCode(_SessionVals.UserName); };
                GridColumnExt3.DropDownQueryProperty = delegate () { return Access.MasterData.Owner.Instance.GetQuery(_SessionVals.UserName.Trim()); };
                GridColumnExt5.DropDownQueryProperty = delegate () { return Access.MasterData.ComboBoxItem.Instance.GetQueryProperty_Display("outbound_order_load_status"); };

                #endregion

                #region Initial Input Data

                //Save
                ddlWarehouse.MethodQueryProperty = delegate () { return Access.MasterData.Warehouse.Instance.GetQueryCode(_SessionVals.UserName); };
                ddlOwner.MethodQueryProperty = delegate () { return Access.MasterData.Owner.Instance.GetQueryCode(_SessionVals.UserName); };
                ddlOrder.MethodQueryProperty = delegate () { return Access.Transaction.Outbound.OutboundLoad.Instance.GetQuery(ddlWarehouse.GetValue(), ddlOwner.GetValue()); };

                #endregion

                if (!Page.IsPostBack)
                {
                    //Search
                    hidSessionUser.SetValue(_SessionVals.UserName);
                    hidIsFirstLoad.SetValue("YES");

                    PICK_TYPE = !string.IsNullOrEmpty(Request.QueryString["dir_pick"]) ? PICK_TYPE_USR : PICK_TYPE_SYS;
                    hidPickType.SetValue(PICK_TYPE);

                    ddlWarehouse.BindDataSource();
                    ddlOwner.BindDataSource();
                }
            }
            catch (Exception ex)
            {
                Logging = new Prototype.Providers.Logging(this, ex);
                RaiseLogging();
            }
        }

        private bool GridOutboundMaster_GridRowCanDeleteValidate(GridViewRowEventArgs e)
        {
            return (bool)DataBinder.Eval(e.Row.DataItem, "allow_update");
        }

        private void PopupPickDetail_CloseClick(object sender, EventArgs e)
        {
            try
            {
                gridPickDetail.DataUnBind();
            }
            catch (Exception ex)
            {
                Logging = new Prototype.Providers.Logging(this, ex);
                RaiseLogging();
            }
        }

        void chkGenOrderNo_PostValueChanged(dynamic _value)
        {
            try
            {
                if (_value == true)
                {
                    var _no = Access.FunctionGenerate.Instance.GetGenerateRunning("OUT_BY_GROUP");

                    txtLoadId.SetValue(_no);
                    txtLoadId.Enabled = false;
                }
                else
                {
                    txtLoadId.Clear();
                    txtLoadId.Enabled = true;
                }

                txtLoadId.Update();
            }
            catch (Exception ex)
            {
                Logging = new Prototype.Providers.Logging(this, ex);
                RaiseLogging();
            }
        }


        private void PopupLoadData_CloseClick(object sender, EventArgs e)
        {
            var priority = txtPriority.GetValue();
            if (priority.ToString() == string.Empty)
            {
                popupLoadData.validateOnCloseClick = true;
                Page.MessageWarning("Please input priority value.");
                return;
            }
            else
            {
                var loadId = txtLoadId.GetValue();
                using (var _Model = new WMSEntities())
                {
                    var master = _Model.t_wms_outbound_master.Where(x => x.load_id == loadId).ToList();
                    foreach (var item in master)
                    {
                        item.priority = priority;
                        item.ship_date_actual = txtShipDate.GetValue() == null ? null : (DateTime?)txtShipDate.GetValue();
                    }
                    _Model.SaveChanges();
                }
                popupLoadData.validateOnCloseClick = false;
            }

            GridExt1.DataBind();
            gridOutboundMaster.DataUnBind();
        }

        void GridExt1_GridSearching(object sender, EventArgs e)
        {
            if (hidIsFirstLoad.GetValue() == "YES")
                hidIsFirstLoad.SetValue(string.Empty);
        }

        private void GridExt1_GridRowEdit(object _rowKeyValue)
        {
            try
            {
                var prms = _rowKeyValue.ToString().Split('|');
                int? priority = (prms.Length > 5 && !string.IsNullOrEmpty(prms[4])) ? Convert.ToInt32(prms[4]) : (int?)null;
                DateTime? ship_date_actual = (prms.Length > 5 && !string.IsNullOrEmpty(prms[5])) ? Convert.ToDateTime(prms[5]) : (DateTime?)null;

                EditLoad(prms[0], prms[1], prms[2], prms[3], priority, ship_date_actual);
            }
            catch (Exception ex)
            {
                Logging = new Prototype.Providers.Logging(this, ex);
                RaiseLogging();
            }
        }

        void GridExt1_GridRowAfterDataBound(GridViewRowEventArgs e)
        {
            try
            {
                var order_status = (string)DataBinder.Eval(e.Row.DataItem, "load_status");
                var allow_unrel = (bool)DataBinder.Eval(e.Row.DataItem, "allow_unrel");
                var allow_release_sys = (bool)DataBinder.Eval(e.Row.DataItem, "allow_release");
                var load_id = (string)DataBinder.Eval(e.Row.DataItem, "load_id");
                var wh_id = (string)DataBinder.Eval(e.Row.DataItem, "wh_id");
                var owner_id = (Guid)DataBinder.Eval(e.Row.DataItem, "owner_id");

                var btRel = (Button)e.Row.FindControl("RELEASE");
                btRel.CssClass = "btn btn-sm btn-success btn-ingrid";

                var btUnrel = (Button)e.Row.FindControl("UNREL");
                btUnrel.CssClass = "btn btn-sm btn-warning btn-ingrid";
                btUnrel.Enabled = allow_unrel;

                if ((PICK_TYPE == PICK_TYPE_SYS && !allow_release_sys) || (PICK_TYPE == PICK_TYPE_USR && order_status != "OPEN"))
                    btRel.Enabled = false;
                else
                {
                    using (var _Model = new WMSEntities())
                    {
                        string[] statusChecker = _Model.t_wms_rule
                            .Where(r => r.is_active == "YES" && r.rule_code == "RULE_CHECKER_ORDER_TYPE")
                            .Select(r => r.value)
                            .ToArray();

                        var outboundMasters = _Model.t_wms_outbound_master
                            .Where(x => x.wh_id == wh_id && x.owner_id == owner_id && x.load_id == load_id)
                            .Select(x => new { x.order_type, x.outbound_order_master_id })
                            .ToList();

                        // If any outboundMaster is a checker type, all must have a mapping
                        if (outboundMasters.Any(x => statusChecker.Contains(x.order_type)))
                        {
                            bool allMapped = outboundMasters.All(item =>
                                _Model.t_tms_manifest_order_mapping.Any(m => m.outbound_order_master_id == item.outbound_order_master_id)
                            );
                            btRel.Enabled = allMapped;
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                Logging = new Prototype.Providers.Logging(this, ex);
                RaiseLogging();
            }
        }

        void GridExt1_GridRowCommandClick(GridViewCommandEventArgs e)
        {
            try
            {
                var ids = e.CommandArgument.ToString();

                var arrs = ids.Split('|');

                var wh_id = arrs[0];
                var owner_code = arrs[1];
                var load_id = arrs[2];

                if (e.CommandName == "RELEASE")
                {
                    if (PICK_TYPE == PICK_TYPE_SYS)
                    {
                        ReleaseKeyActive = ids;
                        CheckRelease();
                    }
                    else
                    {
                        using (var _acc = new Access.Transaction.Outbound.OutboundRelByLoad())
                        {
                            this.PlugEventResult(_acc);

                            if (_acc.ReleaseLoadByUser(wh_id, owner_code, load_id, _SessionVals.UserName))
                            {
                                GridExt1.DataBind();
                                Page.MessageSuccess("Release Load Order Success.");
                            }
                        }
                    }
                }
                else if (e.CommandName == "UNREL")
                {
                    if (PICK_TYPE == PICK_TYPE_SYS)
                    {
                        Unrelease(ids);
                    }
                    else
                    {
                        using (var _acc = new Access.Transaction.Outbound.OutboundRelByLoad())
                        {
                            this.PlugEventResult(_acc);

                            if (_acc.UnreleaseLoadByUser(wh_id, owner_code, load_id))
                            {
                                GridExt1.DataBind();
                                Page.MessageSuccess("Unrelease Load Order Success.");
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                Logging = new Prototype.Providers.Logging(this, ex);
                RaiseLogging();
            }
        }

        protected void btNewLoad_Click(object sender, EventArgs e)
        {
            try
            {
                ddlWarehouse.Clear();
                ddlOwner.Clear();
                txtLoadId.Clear();
                txtPriority.Clear();

                ddlWarehouse.Enabled = ddlOwner.Enabled = txtLoadId.Enabled = true;
                panelAddOrder.Visible = false;
                panelOrderMaster.Visible = false;
                btSave.Visible = true;

                chkGenOrderNo.Checked = true;
                chkGenOrderNo.Enabled = true;
                updateGenOrderNo.Visible = true;

                chkGenOrderNo_PostValueChanged(true);

                popupLoadData.ShowDialog();
            }
            catch (Exception ex)
            {
                Logging = new Prototype.Providers.Logging(this, ex);
                RaiseLogging();
            }
        }

        protected void btSave_Click(object sender, EventArgs e)
        {
            try
            {
                EditLoad(ddlWarehouse.GetValue(), ddlOwner.GetValue(), txtLoadId.GetValue(), "OPEN", null, null);
            }
            catch (Exception ex)
            {
                Logging = new Prototype.Providers.Logging(this, ex);
                RaiseLogging();
            }
        }


        protected void btAddOrder_Click(object sender, EventArgs e)
        {
            try
            {
                if (UpdateLoad((Guid)ddlOrder.GetValue(), true))
                {
                    ddlOrder.BindDataSource();
                }
            }
            catch (Exception ex)
            {
                Logging = new Prototype.Providers.Logging(this, ex);
                RaiseLogging();
            }
        }

        private void GridOutboundMaster_GridRowDelete(object _rowKeyValue)
        {
            try
            {
                UpdateLoad(Guid.Parse(_rowKeyValue.ToString()), false);
            }
            catch (Exception ex)
            {
                Logging = new Prototype.Providers.Logging(this, ex);
                RaiseLogging();
            }
        }

        private void GridOutboundMaster_GridRowExtendClick(object _rowKeyValue, _UControls.GridIncFieldValue[] _values)
        {
            try
            {
                var order_no = _values.First(x => x.DataField == GridColumnExt8.DataField).Value.ToString();

                txtWarehouse.SetValue(ddlWarehouse.GetText());
                txtOwner.SetValue(ddlOwner.GetText());
                txtOrderNo.SetValue(order_no);

                hidPickRefOrderMasterId.SetValue(Guid.Parse(_rowKeyValue.ToString()));
                gridPickDetail.Search();

                popupPickDetail.ShowDialog();
            }
            catch (Exception ex)
            {
                Logging = new Prototype.Providers.Logging(this, ex);
                RaiseLogging();
            }
        }


        bool UpdateLoad(Guid _order_master_id, bool _is_add)
        {
            using (var _acc = new Access.Transaction.Outbound.OutboundLoad())
            {
                this.PlugEventResult(_acc);

                if (_acc.UpdateOrderLoad(ddlWarehouse.GetValue(), ddlOwner.GetValue(), txtLoadId.GetValue(), _order_master_id, _is_add))
                {
                    gridOutboundMaster.DataBind();
                    return true;
                }
            }

            return false;
        }

        void EditLoad(string _wh_id, string _owner_id, string _load_id, string _load_status, int? _priority, DateTime? ship_date_actual)
        {
            using (var _acc = new Access.Transaction.Outbound.OutboundLoad())
            {
                this.PlugEventResult(_acc);

                var last_status = _acc.GetLoadStatus(_wh_id, _owner_id, _load_id);

                if (!string.IsNullOrEmpty(last_status))
                    _load_status = last_status;

                var list_status = _acc.GetOrderStatusList(_wh_id, _owner_id, _load_id);
                var rule_status = Access.MasterData.Rule.Instance.GetListByRuleCode("GROUP_ORDER_STATUS_ENABLE_SHIP_BUTTON");
                btnConfirmShip.Enabled = !(list_status.Any(status => !rule_status.Contains(status)));
            }


            //btnConfirmShipPart.Enabled = Access.MasterData.Rule.Instance.Any_Rule("RULE_OUTBOUND_ORDER_STATUS_FOR_SHIP", _load_status);
            
            //GROUP_ORDER_STATUS_ENABLE_SHIP_BUTTON



            var allow_update = (_load_status == "OPEN");

            ddlWarehouse.SetValue(_wh_id);
            ddlOwner.SetValue(_owner_id);
            txtLoadId.SetValue(_load_id);
            txtStatus.SetValue(_load_status);
            txtPriority.SetValue(_priority);
            txtShipDate.SetValue(ship_date_actual);
            ddlOrder.BindDataSource();

            hidWarehouseID.SetValue(!string.IsNullOrEmpty(_wh_id) ? _wh_id : "");
            hidOwnerID.SetValue(!string.IsNullOrEmpty(_owner_id) ? _owner_id : "");
            hidLoadID.SetValue(!string.IsNullOrEmpty(_load_id) ? _load_id : "");

            //gridOutboundMaster.GridAllowRowDelete = allow_update;
            gridOutboundMaster.Search();
            gridOutboundMaster.UpdateCommand();

            ddlWarehouse.Enabled = ddlOwner.Enabled = txtLoadId.Enabled = false;
            panelAddOrder.Visible = allow_update;
            panelOrderMaster.Visible = true;
            btSave.Visible = false;

            chkGenOrderNo.Checked = false;
            updateGenOrderNo.Visible = false;

            popupLoadData.UpdateContent();
            popupLoadData.UpdateCommand();

            popupLoadData.ShowDialog();
        }

        void popupConfirmRelease_CloseClick(object sender, EventArgs e)
        {
            try
            {
                gvConfirmRelease.DataSource = null;
                gvConfirmRelease.DataBind();

                ReleaseKeyActive = string.Empty;
            }
            catch (Exception ex)
            {
                Logging = new Prototype.Providers.Logging(this, ex);
                RaiseLogging();
            }
        }

        protected void btComfirm_Click(object sender, EventArgs e)
        {
            try
            {
                var is_success = SaveRelease();
                if (is_success)
                {
                    gvConfirmRelease.DataSource = null;
                    gvConfirmRelease.DataBind();

                    popupConfirmRelease.HideDialog();
                }
            }
            catch (Exception ex)
            {
                Logging = new Prototype.Providers.Logging(this, ex);
                RaiseLogging();
            }
        }


        #region Function Release

        const string PICK_TYPE_SYS = "SYSTEM_GROUP";
        const string PICK_TYPE_USR = "USER_GROUP";

        protected string PICK_TYPE
        {
            get
            {
                if (ViewState["PICK_TYPE"] == null)
                    ViewState["PICK_TYPE"] = string.Empty;

                return ViewState["PICK_TYPE"].ToString();
            }
            set
            {
                ViewState["PICK_TYPE"] = value;
            }
        }

        protected string ReleaseKeyActive
        {
            get
            {
                if (ViewState["ReleaseKeyActive"] == null)
                    ViewState["ReleaseKeyActive"] = string.Empty;

                return (string)ViewState["ReleaseKeyActive"];
            }
            set
            {
                ViewState["ReleaseKeyActive"] = value;
            }
        }

        void CheckRelease()
        {
            if (PICK_TYPE == PICK_TYPE_SYS)
            {
                using (var _accRelease = new Access.Transaction.Outbound.OutboundRelByLoad())
                {
                    this.PlugEventResult(_accRelease);

                    var prms = ReleaseKeyActive.ToString().Split('|');

                    var result = _accRelease.CheckItemQtyPreRelease(prms[0], prms[2]);
                    if (result.Count() > 0)
                    {
                        gvConfirmRelease.DataSource = result;
                        gvConfirmRelease.DataBind();

                        popupConfirmRelease.HeaderText = "Confirm Release Order";
                        popupConfirmRelease.ShowDialog();
                        return;
                    }
                    else
                    {
                        SaveRelease();
                    }
                }
            }
            else
            {
                SaveRelease();
            }
        }

        bool SaveRelease()
        {
            var is_success = false;

            using (var _accRelease = new Access.Transaction.Outbound.OutboundRelByLoad())
            {
                this.PlugEventResult(_accRelease);

                var prms = ReleaseKeyActive.ToString().Split('|');

                if (PICK_TYPE == PICK_TYPE_SYS)
                {
                    is_success = _accRelease.ReleaseLoadBySystem(prms[0], prms[2]);
                }
                //else
                //{
                //    is_success = _accRelease.ReleaseOrderByUser(ReleaseKeyActive);
                //}
            }

            if (is_success)
            {
                GridExt1.DataBind();
                ReleaseKeyActive = string.Empty;

                Page.MessageSuccess("Release Order Success.");
            }

            return is_success;
        }

        private void Unrelease(string _ids)
        {
            using (var _accRelease = new Access.Transaction.Outbound.OutboundRelByLoad())
            {
                this.PlugEventResult(_accRelease);

                var prms = _ids.Split('|');

                if (_accRelease.UnreleaseLoadBySystem(prms[0], prms[2]))
                {
                    GridExt1.DataBind();

                    Page.MessageSuccess("Unrelease Order Success.");
                }
            }
        }

        #endregion

        protected void btnConfirmShipPart_Click(object sender, EventArgs e)
        {
            try
            {
                var errCode = new ObjectParameter("out_ErrorCode", typeof(string));
                var errMsg = new ObjectParameter("out_ErrorMessage", typeof(string));

                var wh_id = (string)hidWarehouseID.GetValue();
                var owner_code = (string)hidOwnerID.GetValue();
                var load_id = (string)hidLoadID.GetValue();
                var status = txtStatus.GetValue();
                var priority = txtPriority.GetValue();
                var ship_date_actual = txtShipDate.GetValue();

                using (var _model = new Source.WMSEntities())
                {
                    _model.Configuration.EnsureTransactionsForFunctionsAndCommands = false;

                    var wh_master_id = _model.t_wms_wh.Select(se => new { se.wh_master_id, se.wh_id }).First(x => x.wh_id == wh_id).wh_master_id;

                    _model.usp_outbound_partial_ship_confirm_by_load_id(_SessionVals.AppID, wh_master_id, _SessionVals.DeviceID, _SessionVals.UserName, load_id, errCode, errMsg);
                }

                if (errCode.Value.ToString() == "0")
                {
                    EditLoad(wh_id, owner_code, load_id, status, priority, ship_date_actual);

                    Page.MessageSuccess("Partial Ship Success.");
                }
                else
                {
                    Page.MessageWarning(errMsg.Value.ToString());
                }
            }
            catch (Exception ex)
            {
                Logging = new Prototype.Providers.Logging(this, ex);
                RaiseLogging();
            }
        }

        protected void btnConfirmShip_Click(object sender, EventArgs e)
        {
            try
            {
                var errCode = new ObjectParameter("out_ErrorCode", typeof(string));
                var errMsg = new ObjectParameter("out_ErrorMessage", typeof(string));

                var wh_id = (string)hidWarehouseID.GetValue();
                var owner_code = (string)hidOwnerID.GetValue();
                var load_id = (string)hidLoadID.GetValue();
                var status = txtStatus.GetValue();
                var priority = txtPriority.GetValue();
                var ship_date_actual = txtShipDate.GetValue();

                if(ship_date_actual == null)
                {
                    Page.MessageWarning("Please input Ship Date Actual.");
                    return;
                }

                using (var _model = new Source.WMSEntities())
                {
                    _model.Configuration.EnsureTransactionsForFunctionsAndCommands = false;

                    var wh_master_id = _model.t_wms_wh.Select(se => new { se.wh_master_id, se.wh_id }).First(x => x.wh_id == wh_id).wh_master_id;

                    _model.usp_outbound_ship_confirm_by_load_id(_SessionVals.AppID, wh_master_id, _SessionVals.DeviceID, _SessionVals.UserName, load_id
                        , null, null, null, null, null, null, null, ship_date_actual, errCode, errMsg);
                }

                if (errCode.Value.ToString() == "0")
                {
                    EditLoad(wh_id, owner_code, load_id, status, priority, ship_date_actual);

                    Page.MessageSuccess("Ship Success.");
                }
                else
                {
                    Page.MessageWarning(errMsg.Value.ToString());
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