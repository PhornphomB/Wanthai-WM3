using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace WMS_NEW.Transaction.Outbound.AscxControls
{
    public partial class PickDetailUnPick : UControlCustom, _UControls.IFormRelation
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            try
            {
                gridPickDetail.GridRowCanSelectValidate += gridPickDetail_GridRowCanSelectValidate;
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

                txtWarehouseId.SetValue(ent.wh_id);
                txtOrderNo.SetValue(ent.outbound_order_number);
                txtCustomer.SetValue(ent.customer_name);
                txtStatus.SetValue(ent.order_status);

                hidPickRefOrderMasterId.SetValue(ent.outbound_order_master_id);
                hidAllowUnpick.SetValue("YES");

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

        bool gridPickDetail_GridRowCanSelectValidate(GridViewRowEventArgs e)
        {
            var is_staging_location = (bool)DataBinder.Eval(e.Row.DataItem, "is_staging_location");

            return is_staging_location;
        }

        protected void btConfirm_Click(object sender, EventArgs e)
        {
            try
            {

                if (gridPickDetail.CountListKey() == 0)
                {
                    Page.MessageWarning("! Please select row for unpick");
                    return;
                }

                var listKey = gridPickDetail.GetListKey().Select(se => Guid.Parse(se.KeyId.ToString())).ToArray();

                using (var _acc = new Access.Transaction.Outbound.OutboundPickDetail())
                {
                    this.PlugEventResult(_acc);

                    var saved = _acc.UpdateUnPick(txtWarehouseId.GetValue(), txtOrderNo.GetValue(), listKey);

                    gridPickDetail.DeleteAllKey();
                    gridPickDetail.DataBind();

                    if (saved)
                    {
                        Page.MessageSuccess("Unpick Success.");

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