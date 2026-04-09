using System;
using System.Linq;
using System.Web.UI.WebControls;

namespace WMS_NEW.Transaction.Outbound.AscxControls
{
    public partial class ucSerial : UControlCustom
    {
        public Delegate dg_CallBackSearch;

        protected void Page_Load(object sender, EventArgs e)
        {
            gridSerial.GridRowCommandClick += GridSerial_GridRowCommandClick;
            gridSerial.GridRowAfterDataBound += GridSerial_GridRowAfterDataBound;
        }

        private void GridSerial_GridRowAfterDataBound(GridViewRowEventArgs e)
        {
            try
            {
                Button btn = e.Row.FindControl("select_serial") as Button;
                if (btn != null)
                {
                    btn.CssClass = "btn btn-sm btn-warning btn-ingrid";
                }
            }
            catch (Exception ex)
            {
                Logging = new Prototype.Providers.Logging(this, ex);
                RaiseLogging();
            }
        }

        public void BindParameter(Guid _outbound_pick_detail_id)
        {
            hidPickDetail.SetValue(_outbound_pick_detail_id);
            gridSerial.Search();
            popSerial.ShowDialog();
        }

        private void GridSerial_GridRowCommandClick(GridViewCommandEventArgs e)
        {
            try
            {
                if (e.CommandName == "select_serial")
                {
                    UpdateSerial(e.CommandArgument.ToString(),1);
                }
            }
            catch (Exception ex)
            {
                Logging = new Prototype.Providers.Logging(this, ex);
                RaiseLogging();
            }
        }

        protected void btnCancelSerial_Click(object sender, EventArgs e)
        {
            try
            {
                UpdateSerial(string.Empty,null);
            }
            catch (Exception ex)
            {
                Logging = new Prototype.Providers.Logging(this, ex);
                RaiseLogging();
            }
        }

        void UpdateSerial(string _serial_number,double? _qty )
        {
            Guid outbound_pick_detail_id = hidPickDetail.GetValue();

            using (var model = new Source.WMSEntities())
            {
                var ent = (from rows in model.t_wms_outbound_pick_detail
                           where rows.outbound_pick_detail_id == outbound_pick_detail_id
                           select rows).FirstOrDefault();
                if (ent != null)
                {
                    string serial_number = _serial_number;
                    ent.confirm_serial_number_pc = serial_number;
                    ent.quantity_comfirm_pick_pc = _qty;

                    int rowAffected = model.SaveChanges();
                    popSerial.HideDialog();
                    if (dg_CallBackSearch != null)
                    {
                        dg_CallBackSearch.DynamicInvoke();
                    }
                }

            }
        }
    }
}