using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace WMS_NEW.Transaction.Outbound.AscxControls
{
    public partial class PickDetailConfirmShip : UControlCustom, _UControls.IFormRelation
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            try
            {
                popupPickDetail.CloseClick += popupPickDetail_CloseClick;
            }
            catch (Exception ex)
            {
                Logging = new Prototype.Providers.Logging(this, ex);
                RaiseLogging();
            }
        }

        public Action<dynamic> UpdateParent { get; set; }

        public void InitForm(dynamic _obj)
        {
            try
            {
                IsChanged = false;
                var ent = (Access.Transaction.Outbound.OutboundMasterDto)_obj;

                ddCarrier.MethodQueryProperty = delegate () { return Access.MasterData.Carrier.Instance.GetQuery(); };
                ddCarrier.BindDataSource();

                ddTruckType.MethodQueryProperty = delegate () { return Access.MasterData.Transportation.TruckType.Instance.GetQuery(); };
                ddTruckType.BindDataSource();

                hidWarehouseMasterId.SetValue(ent.wh_master_id);
                txtWarehouseId.SetValue(ent.wh_id);
                txtOrderNo.SetValue(ent.outbound_order_number);
                txtPostDate.SetValue(DateTime.Now);
                txtShipDate.SetValue(DateTime.Now);

                ddCarrier.Clear();
                ddCarrier.SetValue(ent.carrier_id);

                ddTruckType.Clear();

                txtQtyPlan.SetValue(ent.sum_qty_plan);
                txtQtyStaging.SetValue(ent.sum_qty_stage);
                txtQtyShip.SetValue(ent.sum_qty_ship);

                txtDriLicense.Clear();
                //txtLocationTo.Clear();
                txtContainerNo.Clear();

                hidPickRefOrderMasterId.SetValue(ent.outbound_order_master_id);
                gridPickDetail.Search();

                btConfirm.Enabled = !(ent.order_status == "SHIP");
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

        protected void btConfirm_Click(object sender, EventArgs e)
        {
            try
            {
                using (var _acc = new Access.Transaction.Outbound.OutboundShipDetail())
                {
                    this.PlugEventResult(_acc);

                    var saved = _acc.UpdateConfirmShip((Guid)hidWarehouseMasterId.GetValue(), (Guid)hidPickRefOrderMasterId.GetValue(), (Guid?)ddCarrier.GetValue(),
                                                ddTruckType.GetText(), txtDriLicense.GetValue(), txtContainerNo.GetValue(), txtPostDate.GetValue(), txtShipDate.GetValue());

                    if (saved)
                    {
                        gridPickDetail.DataBind();
                        Page.MessageSuccess("Confirm Ship Success.");

                        btConfirm.Enabled = false;
                        popupPickDetail.UpdateCommand();

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