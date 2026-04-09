using System;
using System.Linq;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace WMS_NEW.Transaction.Outbound.AscxControls
{
    public partial class PickDetailConfirmPick : UControlCustom, _UControls.IFormRelation
    {
        #region ++ DELEGATE ++
        delegate void dg_Search();
        event dg_Search eSearch;
        #endregion

        protected void Page_Load(object sender, EventArgs e)
        {
            try
            {
                #region Deledate Call Back
                eSearch += new dg_Search(Pick_Callback);
                ucSerial.dg_CallBackSearch = eSearch;
                #endregion

                popupPickDetail.CloseClick += popupPickDetail_CloseClick;

                #region Binding Event

                gridPickDetail.GridRowTextChanged += gridPickDetail_GridRowTextChanged;
                gridPickDetail.GridRowCommandClick += GridPickDetail_GridRowCommandClick;
                gridPickDetail.GridRowAfterDataBound += GridPickDetail_GridRowAfterDataBound;

                #endregion
            }
            catch (Exception ex)
            {
                Logging = new Prototype.Providers.Logging(this, ex);
                RaiseLogging();
            }
        }

        private void Pick_Callback()
        {
            gridPickDetail.Search();
        }

        public Action<dynamic> UpdateParent { get; set; }

        public void InitForm(dynamic _obj)
        {
            try
            {
                //WM.AccessTransaction.Outbound.OutboundPickDetail.Instance.UpdateResetQtyConfirm(_ent.outbound_order_master_id);

                IsChanged = false;
                var ent = (Access.Transaction.Outbound.OutboundMasterDto)_obj;

                ddLocStage.MethodQueryProperty = delegate () { return Access.MasterData.Location.Instance.GetQuery(ent.wh_master_id, "STG"); };
                ddLocStage.BindDataSource();

                if (ddLocStage.MethodQueryProperty().ToList().Count == 1)
                {
                    Guid LocId = Guid.Parse(ddLocStage.MethodQueryProperty().ToList().FirstOrDefault().ToString());
                    ddLocStage.SetValue(LocId);
                }

                txtWarehouseId.SetValue(ent.wh_id);
                txtOrderNo.SetValue(ent.outbound_order_number);
                txtCustomer.SetValue(ent.customer_name);
                txtStatus.SetValue(ent.order_status);

                hidPickRefOrderMasterId.SetValue(ent.outbound_order_master_id);
                hidAllowPick.SetValue("YES");

                gridPickDetail.Search();

                popupPickDetail.ShowDialog();
            }
            catch (Exception ex)
            {
                Logging = new Prototype.Providers.Logging(this, ex);
                RaiseLogging();
            }
        }

        protected bool IsChanged
        {
            get
            {
                if (ViewState["IsChanged"] == null)
                    ViewState["IsChanged"] = false;

                return (bool)ViewState["IsChanged"];
            }
            set
            {
                ViewState["IsChanged"] = value;
            }
        }

        public event EventHandler CloseClick;
        void popupPickDetail_CloseClick(object sender, EventArgs e)
        {
            if (CloseClick != null && IsChanged)
                CloseClick(sender, e);
        }

        #region Grid Event
        private void GridPickDetail_GridRowAfterDataBound(GridViewRowEventArgs e)
        {
            try
            {
                Button btn = e.Row.FindControl("select_serial") as Button;
                if ( btn != null)
                {
                    btn.CssClass = "btn btn-sm btn-warning btn-ingrid";

                    bool sn_control = (bool)DataBinder.Eval(e.Row.DataItem, "sn_control");

                    bool is_serial = (bool)DataBinder.Eval(e.Row.DataItem, "is_serial");
                    if (is_serial)
                    {
                        btn.Enabled = false;
                    }
                    else {
                        btn.Enabled = sn_control;
                    }

                }
            }
            catch (Exception ex)
            {
                Logging = new Prototype.Providers.Logging(this, ex);
                RaiseLogging();
            }
        }

        private void GridPickDetail_GridRowCommandClick(GridViewCommandEventArgs e)
        {
            try
            {
                if (e.CommandName == "select_serial")
                {
                    var outbound_pick_detail_id = Guid.Parse(e.CommandArgument.ToString());
                    ucSerial.BindParameter(outbound_pick_detail_id);
                }
            }
            catch (Exception ex)
            {
                Logging = new Prototype.Providers.Logging(this, ex);
                RaiseLogging();
            }
        }
        string gridPickDetail_GridRowTextChanged(object _rowKeyValue, string _rowDataField, string _rowTextValue)
        {

            if (string.IsNullOrEmpty(_rowTextValue))
                _rowTextValue = "-1";

            var defaultValue = _rowTextValue;

            try
            {
                using (var _acc = new Access.Transaction.Outbound.OutboundPickDetail())
                {
                    this.PlugEventResult(_acc);

                    var result = _acc.UpdatePickQty(Guid.Parse(_rowKeyValue.ToString()), Convert.ToDouble(_rowTextValue));
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

        #endregion


        protected void btConfirm_Click(object sender, EventArgs e)
        {
            try
            {
                using (var _acc = new Access.Transaction.Outbound.OutboundPickDetail())
                {
                    this.PlugEventResult(_acc);

                    var saved = _acc.UpdateConfirmPick((Guid)hidPickRefOrderMasterId.GetValue(), (Guid)ddLocStage.GetValue());
                    if (saved)
                    {
                        //WM.AccessTransaction.Outbound.OutboundPickDetail.Instance.UpdateResetQtyConfirm(hidPickRefOrderMasterId.GetValue());

                        gridPickDetail.DataBind();
                        Page.MessageSuccess("Confirm Pick Success.");

                        IsChanged = true;
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