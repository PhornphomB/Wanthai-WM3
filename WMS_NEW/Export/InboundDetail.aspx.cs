using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using WMS_NEW.Source;

namespace WMS_NEW.Export
{
    public partial class InboundDetail : PageCustom
    {
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
        protected void Page_Load(object sender, EventArgs e)
        {
            #region Initial Peoperty Column Grid

            GridColumnExt00.DropDownQueryProperty = delegate () { return Access.MasterData.Warehouse.Instance.GetQuery_User(_SessionVals.UserName); };
            //GridColumnExt01.DropDownQueryProperty = delegate() { return Access.MasterData.Owner.Instance.GetQueryCode(_SessionVals.UserName); };
            GridColumnExt03.DropDownQueryProperty = delegate () { return Access.MasterData.ComboBoxItem.Instance.GetQueryProperty_Display("inbound_order_type"); };
            //GridColumnExt04.DropDownQueryProperty = delegate () { return Access.MasterData.ComboBoxItem.Instance.GetQueryProperty_Display("inbound_order_status"); };
            //GridColumnExt09.DropDownQueryProperty = delegate () { return  Access.MasterData.ItemCategory.Instance.GetQuery(); };
            GridColumnExt00.DropDownPostValueChanged = GridColumnExt00_DropDownPostValueChanged;
            GridColumnExt20.DropDownQueryProperty = delegate () { return Access.MasterData.ProductionLine.Instance.GetQuery(this.wh_master_id); };
            GridColumnExt24.DropDownQueryProperty = delegate () { return Access.MasterData.Customer.Instance.GetQuery(); };

            if (GridColumnExt00.DropDownQueryProperty().ToList().Count == 1)
            {
                this.wh_master_id = GridColumnExt00.DropDownQueryProperty().Select(w => w.guid_member).FirstOrDefault();
            }

            GridExt1.GridPreExportData += GridExt1_GridPreExportData;
            GridExt1.GridSearched += GridExt1_GridSearched;
            GridExt1.GridRowEdit += GridExt1_GridRowEdit;
            this.LoadComplete += Material_LoadComplete;
            #endregion

            if (!Page.IsPostBack)
            {
                GetWhMasterId();
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
        private void GridExt1_GridRowEdit(object _rowKeyValue)
        {
            hidReceiptDetailId.SetValue(_rowKeyValue);
            using (var _Model = new WMSEntities())
            {
                string keyId = _rowKeyValue.ToString();
                DataTable _dt = GridExt1.DataSource();
                DataRow row = _dt.AsEnumerable()
                    .FirstOrDefault(r => r.Field<string>("sys_id") == keyId);

                string lpn = row != null ? row.Field<string>("lpn") : null;
                string remark = row != null ? row.Field<string>("remark") : null;

                txtLPN.SetValue(lpn);
                txtRemark.SetValue(remark);
                popupChange.ShowDialog();
            }
        }

        private void GridExt1_GridPreExportData(ref System.Data.DataTable dtExport)
        {
            if (dtExport.Rows.Count > 0)
            {
                DataRow row = dtExport.NewRow();
                row["alter_quantity_receive"] = dtExport.Compute("Sum(alter_quantity_receive)", "alter_quantity_receive > 0");
                //row["quantity_received"] = dtExport.Compute("Sum(quantity_received)", "quantity_received > 0");
                dtExport.Rows.Add(row);
            }
        }

        private void GridColumnExt00_DropDownPostValueChanged(dynamic _value)
        {
            if (_value != null)
            {
                this.wh_master_id = _value;
            }
        }
        private void Material_LoadComplete(object sender, EventArgs e)
        {
            GridExt1_GridSearched(sender, e);
        }

        private void GridExt1_GridSearched(object sender, EventArgs e)
        {
            string text_quantity = Access.Configuration.ResourceDetail.GetResource("inbound_detail", "alter_quantity_receive");

            //DataTable _dt = GridExt1.DataSource();
            //double sum_qty = _dt.AsEnumerable().Where(row => row.Field<string>("sys_id") != "0").Sum(row => row.Field<double>("alter_quantity_receive"));
            //double sum_qty = _dt.AsEnumerable()
            //         .Where(row => row.Field<string>("sys_id") != "0")
            //         .Select(row => row.Field<double>("alter_quantity_receive"))
            //         .FirstOrDefault();

            decimal sum = Access.Viewer.InboundDetail.Instance.GetSummary(GridExt1.FilterGridOptions);

            string CssClass = "label-warning";
            string msg = "<span><b class=\"" + CssClass + "\">" + text_quantity + " : " + sum.ToString(Extensions.FormatDecimal) + "</b></span>";

            AltherQty.Text = msg;
            UpdateAltherQty.Update();
        }


        protected void btnSave_Click(object sender, EventArgs e)
        {
            var receipt_detail_id = hidReceiptDetailId.GetValue();
            var remark = txtRemark.GetValue();

            using (var _Model = new WMSEntities())
            {
                Guid keyId = Guid.Parse(receipt_detail_id);
                var receiptDetail = _Model.t_wms_receipt_detail.FirstOrDefault(x => x.receipt_detail_id == keyId);
                if (receiptDetail != null)
                {
                    receiptDetail.remark = remark;
                    _Model.SaveChanges();
                    Page.MessageSuccess("Remark updated successfully.");
                    popupChange.HideDialog();
                }
                else
                {
                    Page.MessageWarning("Receipt detail not found.");
                }
            }
        }
    }
}