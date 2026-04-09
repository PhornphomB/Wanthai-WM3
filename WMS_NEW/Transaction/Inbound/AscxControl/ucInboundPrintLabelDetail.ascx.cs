using _UControls;
using Neodynamic.SDK.Web;
using Prototype.Providers;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using WMS_NEW._ExtensionClass;
using WMS_NEW.Access.Report;
using WMS_NEW.Report.AscxControls;
using WMS_NEW.Source;

namespace WMS_NEW.Transaction.Inbound.AscxControl
{
    public partial class ucInboundPrintLabelDetail : UControlCustom, IFormRelation
    {
        #region ++ DELEGATE ++
        delegate void dg_Search();
        event dg_Search eSearch;

        public Delegate dg_CallBackSearch;

        #endregion
        public string array_print_label_id
        {
            get
            {
                
                return (string)ViewState["array_print_label_id"];
            }
            set
            {
                ViewState["array_print_label_id"] = value;
            }
        }
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
            iColIsPrint.DropDownQueryProperty = delegate () { return new Access.Configuration.ComboBox().GetQueryCode("print_status"); };
            //ddlProductionLine.MethodQueryProperty = delegate () { return Access.Configuration.ComboBox.Instance.GetQueryCode("production_line"); };
            //ddlProductionLine.MethodQueryProperty = delegate () { return Access.MasterData.ProductionLine.Instance.GetQuery(this.wh_master_id); };
            iColProductionLine.DropDownQueryProperty = delegate () { return Access.MasterData.ProductionLine.Instance.GetQuery(this.wh_master_id); };
            //var productionLine = ddlProductionLine.GetValue();
            ucReportViewer.BindingParameter += UcReportViewer_BindingParameter;
            GridExt1.GridRowCommandClick += GridExt1_GridRowCommandClick;
            GridExt1.GridRowAfterDataBound += GridExt1_GridRowAfterDataBound;
        }

        private void GridExt1_GridRowAfterDataBound(GridViewRowEventArgs e)
        {
            try
            {
                var isPrint = (string)DataBinder.Eval(e.Row.DataItem, "is_print");
                if (isPrint == "YES")
                {
                    e.Row.CssClass = "table-success";
                }
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

                var ent = (Source.t_wms_inbound_master)_obj;
                gridInboundOrderMasterId.SetValue(ent.inbound_order_master_id);
                wh_master_id = ent.wh_master_id;
                var ucFirstLoad = new _UControls.InputHidden() { DataFieldValue = "_is_first_load" };
                ucFirstLoad.SetValue(true);
                GridExt1.AddFilterCustomInputInclude(ucFirstLoad);
                GridExt1.ClearFilters();
                GridExt1.Search();
            }
            catch (Exception ex)
            {
                Logging = new Prototype.Providers.Logging(this, ex);
                RaiseLogging();
            }
        }
        private void GridExt1_GridRowCommandClick(GridViewCommandEventArgs e)
        {
            try
            {
                GridExt1.SetCustomFilterForSearch();
                var productionLine = GridExt1.FilterGridOptions.FirstOrDefault(x => x.DataPropertyName == "production_line");
                if (productionLine == null || productionLine.Value == null)
                {
                    Page.MessageWarning(clsResource.GetResource("print_label", "validate_production_line"));
                    return;
                }
                var keyID = Guid.Parse(e.CommandArgument.ToString());
                if (e.CommandName == "PREPRINT")
                {
                    using (WMSEntities Model = new WMSEntities())
                    {
                        var entLabel = Model.t_wms_print_label.FirstOrDefault(x => x.print_label_id == keyID);
                        var entLabelLast = Model.t_wms_print_label.Where(x => x.inbound_order_master_id == entLabel.inbound_order_master_id && x.inbound_order_detail_id == entLabel.inbound_order_detail_id).OrderByDescending(x => x.row_print).FirstOrDefault();
                        array_print_label_id = keyID.ToString();
                        hidPrintLabelId.SetValue(entLabel.print_label_id);
                        txtOrderNumber.SetValue(entLabel.inbound_order_number);
                        txtWarehouse.SetValue(entLabel.wh_id);
                        txtItemNumber.SetValue(entLabel.item_number);
                        txtLPN.SetValue(entLabel.lpn);
                        txtDescription.SetValue(entLabel.item_description);
                        txtProductionLine.SetValue(entLabel.production_line);
                        txtPackQuantityPerPallet.SetValue(Convert.ToInt32(entLabel.pack_size_per_pallet));
                        hidMaxQuantityOrder.SetValue(Convert.ToInt32(entLabel.pack_size_per_pallet));
                        txtPackSizePerPallet.SetValue(entLabel.pack_size_uom);
                        txtMfgDate.SetValue(Convert.ToDateTime(entLabel.mfg_date).ToString("dd/MM/yyyy"));
                        txtExpriryDate.SetValue(Convert.ToDateTime(entLabel.expiry_date).ToString("dd/MM/yyyy"));
                        txtAttribute1.SetValue(entLabel.attribute1);
                        txtPackQuantityPerPallet.Enabled = (entLabel.is_received == "NO" && keyID == entLabelLast.print_label_id);

                        //if (entLabel.is_print == "NO")
                        //{
                        //    btnPrintConfirm.OnClientClick = $"if (!confirm('{clsResource.GetResource("print_label", "confirm_print")}')) return false;";
                        //}
                        //else
                        //{
                        //    btnPrintConfirm.OnClientClick = "";
                        //}
                        
                        popupPrint.ShowDialog();

                    }
                }

            }
            catch (Exception ex)
            {
                Logging = new Prototype.Providers.Logging(this, ex);
                RaiseLogging();
            }
        }


        protected void btnPrePrint_Click(object sender, EventArgs e)
        {
            GridExt1.SetCustomFilterForSearch();
            //var productionLine = GridExt1.FilterCtrlOptions.FirstOrDefault(x => x.DataFieldValue == "_production_line");
            //if (productionLine == null || productionLine.Value == null)
            //{
            //    Page.MessageWarning(clsResource.GetResource("print_label", "validate_production_line"));
            //    return;
            //}
            var Keys = GridExt1.GetListKey();
            List<string> strings = new List<string>();
            for (int i = 0; i < Keys.Count; i++)
            {
                strings.Add(Keys[i].KeyId.ToString());
            }

            if (strings.Count > 0)
            {
                array_print_label_id = string.Join(",", strings);
                hidPrintLabelId.SetValue(Guid.Empty);
                UpdateIsPrint();
                ClientPrint();
                //popClose.ShowDialog();

                //ucReportViewer.ViewReportNow("4D5E899F-90A4-4084-A984-64E91F082850");

                var ucFirstLoad = new _UControls.InputHidden() { DataFieldValue = "_is_first_load" };
                ucFirstLoad.SetValue(true);
                GridExt1.AddFilterCustomInputInclude(ucFirstLoad);

                GridExt1.Search();
            }

            //List<Guid> guids = new List<Guid>();
            //for (int i = 0; i < Keys.Count; i++)
            //{
            //    guids.Add(Guid.Parse(Keys[i].KeyId.ToString()));
            //}


        }
        protected void btnPrint_Click(object sender, EventArgs e)
        {
            try
            {
                var quantityPerPallet = txtPackQuantityPerPallet.GetValue();
                var maxQuantityOrder = hidMaxQuantityOrder.GetValue();
                if (quantityPerPallet > maxQuantityOrder)
                {
                    //Page.MessageWarning("ห้ามแก้ไขเกินจำนวน Pallet");
                    Page.MessageWarning(clsResource.GetResource("print_label", "not_edit_quantities_beyond_the_quantity_per_pallet"));
                    return;
                }
                else
                {
                    UpdateIsPrint();
                    ClientPrint();
                    //ucReportViewer.ViewReportNow("4D5E899F-90A4-4084-A984-64E91F082850");
                    popupPrint.HideDialog();

                    var ucFirstLoad = new _UControls.InputHidden() { DataFieldValue = "_is_first_load" };
                    ucFirstLoad.SetValue(true);
                    GridExt1.AddFilterCustomInputInclude(ucFirstLoad);

                    GridExt1.Search();
                }
                
                //if (GridExt1.CountListKey() == 0)
                //{
                //    Page.MessageWarning(Access.Configuration.ResourceDetail.GetResource("Approve", "msg_PleaseSelectCheckList"));
                //    return;
                //}

                //using (var _acc = new Access.Transaction.Approve.Approve())
                //{
                //    this.PlugEventResult(_acc);

                //    var chkList = GridExt1.GetListKey().Select(se => se.KeyId.ToString()).ToArray();
                //    string reason = txtReason.GetValue();
                //    _acc.CallReject(chkList, reason);
                //    popupReson.HideDialog();
                //    GridExt1.Search();
                //}
            }
            catch (Exception ex)
            {
                this.Logging = new Prototype.Providers.Logging(this, ex);
                this.RaiseLogging();
            }
        }
        //protected void btPrintConfirm_Click(object sender, EventArgs e)
        //{
        //    popClose.HideDialog();
        //    ClientPrint();
        //}
        private List<ReportParameter> UcReportViewer_BindingParameter(string _report_id)
        {
            try
            {
                var prms = new List<ReportParameter>();
                if(hidPrintLabelId.GetValue() != Guid.Empty)
                {
                    prms.Add(new ReportParameter { Name = "@in_vchPrintLabelId", Value = hidPrintLabelId.GetValue().ToString() });
                    prms.Add(new ReportParameter { Name = "@in_vchPackQuantity", Value = txtPackQuantityPerPallet.GetValue().Value.ToString() });
                    prms.Add(new ReportParameter { Name = "@in_vchArrayPrintLabelId", Value = "" });
                    prms.Add(new ReportParameter { Name = "@in_vchReportType", Value = "1" });
                    prms.Add(new ReportParameter { Name = "@in_vchUpdateBy", Value = _SessionVals.UserName });
                }
                else
                {
                    prms.Add(new ReportParameter { Name = "@in_vchPrintLabelId", Value = "" });
                    prms.Add(new ReportParameter { Name = "@in_vchPackQuantity", Value = "" });
                    prms.Add(new ReportParameter { Name = "@in_vchArrayPrintLabelId", Value = array_print_label_id });
                    prms.Add(new ReportParameter { Name = "@in_vchReportType", Value = "2" });
                    prms.Add(new ReportParameter { Name = "@in_vchUpdateBy", Value = _SessionVals.UserName });
                }
                

                return prms;
            }
            catch (Exception ex)
            {

                throw ex;
            }
        }
        void UpdateIsPrint()
        {
            string[] _print_label_id = array_print_label_id.Split(',');
            using (var _model = new WMSEntities())
            {
                foreach (string p in _print_label_id)
                {
                    var KeyId = Guid.Parse(p);
                    var entPrintLabel = _model.t_wms_print_label.FirstOrDefault(x => x.print_label_id == KeyId && x.is_print == "NO");
                    if (entPrintLabel != null)
                    {
                        entPrintLabel.is_print = "YES";
                        entPrintLabel.print_date = DateTime.Now;
                        entPrintLabel.update_by = _SessionVals.UserName;
                        entPrintLabel.update_date = DateTime.Now;
                    }

                }
                _model.SaveChanges();
                //_model.usp_interface_hana_inbound_print();
            }
        }
        void ClientPrint()
        {
            string rp_name = "rptPrintLabel.rdl";
            DataTable dt = GetSPResult();
            string process_log_id = "";
            foreach (DataRow dr in dt.Rows)
            {
                process_log_id += "[" + string.Join(",", dr.ItemArray) + "]";
            }

            // PRINT Client
            string js = @"$(document).ready(function() {
                        $.fn.printReport = function(name, id){ 
                            jsWebClientPrint.print('useDefaultPrinter=checked' + '&printerName=null'
                                        + '&reportName=" + rp_name + @"'
                                        + '&plid=" + process_log_id + @"'
                                        + '&qrtype=ok'
                                        + '&reportParam=undefined');
                         
                            //alert(name);
                            setTimeout(function () {
                                  //window.location.href= window.location;

                                  ////$('#printOptionModal').modal('show');
                                  ////$('.modal-backdrop').remove();

                                  ////$('#printOptionModal').modal('hide');
                                  ////$('.modal-backdrop').remove();
                                  ////window.location.href= window.location;

                                  //history.go(-1);

                                   $('#printOptionModal').remove();  
                                        
                             }, 1000);
                        }

                        $('#printOptionModal').on('show.bs.modal', function () {
                            $.fn.printReport('" + rp_name + @"','" + process_log_id + @"');
                        });

                        $('#printOptionModal').modal('show');     
                        $('.modal-backdrop').remove();

                   });";


            ClientScriptExt.RegisterStartupScript(this.Page, js);

            if (dg_CallBackSearch != null)
            {
                dg_CallBackSearch.DynamicInvoke();
            }
        }
        private DataTable GetSPResult()
        {
            string PrintLabelId = string.Empty;
            string PackQuantity = string.Empty;
            string ArrayPrintLabelId = string.Empty;
            string ReportType = string.Empty;
            if (hidPrintLabelId.GetValue() != Guid.Empty)
            {
                PrintLabelId = hidPrintLabelId.GetValue().ToString();
                PackQuantity = txtPackQuantityPerPallet.GetValue().Value.ToString();
                ReportType = "1";
            }
            else
            {
                ArrayPrintLabelId = array_print_label_id;
                ReportType = "2";
            }

            DataTable dt = new DataTable();
            using (var model = new Source.WMSEntities())
            {
                var cmd = model.Database.Connection.CreateCommand();
                try
                {
                    
                    cmd.CommandText = "usp_wms_report_print_label";
                    cmd.CommandType = CommandType.StoredProcedure;

                    cmd.Parameters.Add(new SqlParameter { ParameterName = "@in_vchPrintLabelId", Value = (object)PrintLabelId ?? DBNull.Value });
                    cmd.Parameters.Add(new SqlParameter { ParameterName = "@in_vchPackQuantity", Value = (object)PackQuantity ?? DBNull.Value });
                    cmd.Parameters.Add(new SqlParameter { ParameterName = "@in_vchArrayPrintLabelId", Value = (object)ArrayPrintLabelId ?? DBNull.Value });
                    cmd.Parameters.Add(new SqlParameter { ParameterName = "@in_vchReportType", Value = (object)ReportType ?? DBNull.Value });
                    cmd.Parameters.Add(new SqlParameter { ParameterName = "@in_vchUpdateBy", Value = _SessionVals.UserName });
                    cmd.Connection.Open();



                    dt.Load(cmd.ExecuteReader());
                }
                catch (Exception ex)
                {
                    Response.Write(ex.ToString());
                }
                finally
                {
                    if (cmd.Connection != null)
                    {
                        cmd.Connection.Close();
                    }
                }
            }

            return dt;
        }
    }
}