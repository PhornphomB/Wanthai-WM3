using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using WMS_NEW.Report.AscxControls;
using WMS_NEW.Source;

namespace WMS_NEW.Administrator {
    public partial class InterfaceMonitor2 : PageCustom {
        protected void Page_Load(object sender, EventArgs e) {
            try {
                #region Event Control

                //Wanthai ไม่ใช้งาน
                //GridExtInbound.GridRowClick += GridExtInbound_GridRowClick;
                //GridExtOutbound.GridRowClick += GridExtOutbound_GridRowClick;

                GridExtReceipt.GridRowClick += GridExtReceipt_GridRowClick;
                GridExtShip.GridRowClick += GridExtShip_GridRowClick;
                GridExtM3Receipt.GridRowClick += GridExtM3Receipt_GridRowClick;
                GridExtM3ShipNotPO.GridRowClick += GridExtM3ShipNotPO_GridRowClick;
                GridExtM3ShipPO.GridRowClick += GridExtM3ShipPO_GridRowClick;
                GridExtReceipt.GridRowAfterDataBound += GridExtReceipt_GridRowAfterDataBound;
                GridExtShip.GridRowAfterDataBound += GridExtShip_GridRowAfterDataBound;
                GridExtReceipt.GridRowCommandClick += GridExtReceipt_GridRowCommandClick;
                GridExtShip.GridRowCommandClick += GridExtShip_GridRowCommandClick;
                #endregion

                #region Initial Data

                //ddlProcessingStatus.MethodQueryProperty = delegate () { return Access.MasterData.ComboBoxItem.Instance.GetQueryProperty("processing_status"); };
                //ddlWarehouse.MethodQueryProperty = delegate () { return Access.MasterData.Warehouse.Instance.GetQueryCode(_SessionVals.UserName); };
                //ddlOwner.MethodQueryProperty = delegate () { return Access.MasterData.Owner.Instance.GetQueryCode(_SessionVals.UserName); };
                //ddlOrderType.MethodQueryProperty = delegate () { return Access.MasterData.ComboBoxItem.Instance.GetQueryProperty("inbound_order_type"); };
                //ddlOrderStatus.MethodQueryProperty = delegate () { return Access.MasterData.ComboBoxItem.Instance.GetQueryProperty("inbound_order_status"); };
                //ddlSupplier.MethodQueryProperty = delegate () { return Access.MasterData.Supplier.Instance.GetQueryCode(); };
                //ddlCustomer.MethodQueryProperty = delegate () { return Access.MasterData.Customer.Instance.GetQueryCode(); };

                #endregion

                if (!Page.IsPostBack) {
                    #region Bind Data Source

                    //ddlProcessingStatus.BindDataSource();
                    //ddlWarehouse.BindDataSource();
                    //ddlOwner.BindDataSource();
                    //ddlOrderType.BindDataSource();
                    //ddlOrderStatus.BindDataSource();
                    //ddlSupplier.BindDataSource();
                    //ddlCustomer.BindDataSource();

                    #endregion

                    //SearchByTabActive(false);

                    var ucFirstLoad = new _UControls.InputHidden() { DataFieldValue = "_is_first_load" };
                    ucFirstLoad.SetValue(true);
                    GridExtReceipt.AddFilterCustomInputInclude(ucFirstLoad);
                    GridExtShip.AddFilterCustomInputInclude(ucFirstLoad);
                    GridExtM3Receipt.AddFilterCustomInputInclude(ucFirstLoad);
                    GridExtM3ShipPO.AddFilterCustomInputInclude(ucFirstLoad);
                    GridExtM3ShipNotPO.AddFilterCustomInputInclude(ucFirstLoad);

                }
            } catch (Exception ex) {
                Logging = new Prototype.Providers.Logging(this, ex);
                RaiseLogging();
            }
        }

        private void GridExtM3ShipNotPO_GridRowClick(object _rowKeyValue) {
            try {
                popShipNotPODetail.ShowDialog();
                ucShipNotPODetail.BindParameterPage((string)_rowKeyValue);
            } catch (Exception ex) {
                Logging = new Prototype.Providers.Logging(this, ex);
                RaiseLogging();
            }
        }

        private void GridExtM3ShipPO_GridRowClick(object _rowKeyValue) {
            try {
                popShipPODetail.ShowDialog();
                ucShipPODetail.BindParameterPage((string)_rowKeyValue);
            } catch (Exception ex) {
                Logging = new Prototype.Providers.Logging(this, ex);
                RaiseLogging();
            }
        }

        private void GridExtM3Receipt_GridRowClick(object _rowKeyValue) {
            try {
                popM3ReceiptDetail.ShowDialog();
                ucM3ReceiptDetail.BindParameterPage((string)_rowKeyValue);
            } catch (Exception ex) {
                Logging = new Prototype.Providers.Logging(this, ex);
                RaiseLogging();
            }
        }

        void GridExtShip_GridRowClick(object _rowKeyValue) {
            try {
                popShipDetail.ShowDialog();
                ucShipDetail.BindParameterPage((string)_rowKeyValue);
            } catch (Exception ex) {
                Logging = new Prototype.Providers.Logging(this, ex);
                RaiseLogging();
            }
        }

        void GridExtReceipt_GridRowClick(object _rowKeyValue) {
            try {
                popReceiptDetail.ShowDialog();
                ucReceiptDetail.BindParameterPage((string)_rowKeyValue);
            } catch (Exception ex) {
                Logging = new Prototype.Providers.Logging(this, ex);
                RaiseLogging();
            }
        }

        //Wanthai ไม่ใช้งาน
        //void GridExtOutbound_GridRowClick(object _rowKeyValue) {
        //    try {
        //        popOutboundDetail.ShowDialog();
        //        ucOutboundDetail.BindParameterPage((string)_rowKeyValue);
        //    } catch (Exception ex) {
        //        Logging = new Prototype.Providers.Logging(this, ex);
        //        RaiseLogging();
        //    }
        //}

        //void GridExtInbound_GridRowClick(object _rowKeyValue) {
        //    try {
        //        popInboundDetail.ShowDialog();
        //        ucInboundDetail.BindParameterPage((string)_rowKeyValue);
        //    } catch (Exception ex) {
        //        Logging = new Prototype.Providers.Logging(this, ex);
        //        RaiseLogging();
        //    }
        //}
        protected void btnM3Sync_click(object sender, EventArgs e)
        {
            try
            {
                using(var _model = new WMSEntities())
                {
                    _model.usp_host_interface_m3_receipt_export();
                    GridExtReceipt.Search();
                }
            }
            catch (Exception ex)
            {
                Logging = new Prototype.Providers.Logging(this, ex);
                RaiseLogging();
            }
        }
        protected void btnPoSync_click(object sender, EventArgs e)
        {
            try
            {
                using (var _model = new WMSEntities())
                {
                    _model.usp_host_interface_m3_ship_po_export();
                    GridExtShip.Search();
                }
            }
            catch (Exception ex)
            {
                Logging = new Prototype.Providers.Logging(this, ex);
                RaiseLogging();
            }
        }
        protected void btnNotPoSync_click(object sender, EventArgs e)
        {
            try
            {
                using (var _model = new WMSEntities())
                {
                    _model.usp_host_interface_m3_ship_not_po_export();
                    GridExtShip.Search();
                }
            }
            catch (Exception ex)
            {
                Logging = new Prototype.Providers.Logging(this, ex);
                RaiseLogging();
            }
        }
        private void GridExtReceipt_GridRowAfterDataBound(GridViewRowEventArgs e)
        {
            try
            {
                var Status = (string)DataBinder.Eval(e.Row.DataItem, "processing_status");
                var control = e.Row.FindControl("RESEND") as Button;
                if (control != null)
                {
                    control.Enabled = Status.ToUpper() == "ERP SENT" ? true : false;
                }
            }
            catch (Exception ex)
            {
                Logging = new Prototype.Providers.Logging(this, ex);
                RaiseLogging();
            }
        }
        private void GridExtShip_GridRowAfterDataBound(GridViewRowEventArgs e)
        {
            try
            {
                var Status = (string)DataBinder.Eval(e.Row.DataItem, "processing_status");
                var control = e.Row.FindControl("RESEND") as Button;
                if (control != null)
                {
                    control.Enabled = Status.ToUpper() == "ERP SENT" ? true : false;
                }
            }
            catch (Exception ex)
            {
                Logging = new Prototype.Providers.Logging(this, ex);
                RaiseLogging();
            }
        }
        private void GridExtReceipt_GridRowCommandClick(GridViewCommandEventArgs e)
        {
            try
            {
                var keyID = e.CommandArgument.ToString();
                if (e.CommandName == "RESEND")
                {
                    using (var _model = new WMSEntities())
                    {
                        var headerExists = _model.t_host_wms_receipt_conf_header_export.Any(x => x.receipt_id == keyID);
                        if (headerExists)
                        {
                            var header = _model.t_host_wms_receipt_conf_header_export.First(x => x.receipt_id == keyID);
                            header.processing_status = "PENDING";
                            header.resend_date = DateTime.Now;
                            header.resend_by = _SessionVals.UserName;

                            var details = _model.t_host_wms_receipt_conf_detail_export.Where(x => x.receipt_id == keyID).ToList();
                            foreach (var item in details)
                            {
                                item.processing_status = "PENDING";
                            }

                            _model.SaveChanges();
                            GridExtReceipt.Search();
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
        private void GridExtShip_GridRowCommandClick(GridViewCommandEventArgs e)
        {
            try
            {
                var keyID = e.CommandArgument.ToString();
                if (e.CommandName == "RESEND")
                {
                    using (var _model = new WMSEntities())
                    {
                        var headerExists = _model.t_host_wms_ship_conf_header_export.Any(x => x.host_shipment_id == keyID);
                        if (headerExists)
                        {
                            var header = _model.t_host_wms_ship_conf_header_export.First(x => x.host_shipment_id == keyID);
                            header.processing_status = "PENDING";
                            header.resend_date = DateTime.Now;
                            header.resend_by = _SessionVals.UserName;

                            var details = _model.t_host_wms_ship_conf_detail_export.Where(x => x.host_shipment_id == keyID).ToList();
                            foreach (var item in details)
                            {
                                item.processing_status = "PENDING";
                            }

                            _model.SaveChanges();
                            GridExtShip.Search();
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
    }
}