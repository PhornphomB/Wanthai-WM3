using _UControls;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Reflection.Emit;
using System.Text;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using WMS_NEW.Report.AscxControls;
using WMS_NEW.Source;

namespace WMS_NEW.Transaction.Mod.AscxControls
{
    public partial class ucInventoryPrint : UControlCustom
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            try
            {
                ddlUOM.PostValueChanged += ddlUOM_PostValueChanged;
                ucReportViewer.BindingParameter += UcReportViewer_BindingParameter;
                txtOnHand.TextEnterChanged += txtOnHand_TextEnterChanged;
                if (!Page.IsPostBack)
                {
                    //ddlPrint.MethodQueryProperty = delegate () { return Access.Configuration.ComboBox.Instance.GetQuery_Print(); };
                    //ddlPrint.BindDataSource();

                    //ddlGroupPrint.MethodQueryProperty = delegate () { return Access.Configuration.ComboBox.Instance.GetQuery_PrintGroup(); };
                    //ddlGroupPrint.BindDataSource();
                }
            }
            catch (Exception ex)
            {
                Logging = new Prototype.Providers.Logging(this, ex);
                RaiseLogging();
            }
        }

        public void InitialForm(Guid _inventory_id)
        {
            using (var acc = new Access.Transaction.Inventory.InventoryViewer.InventoryItem())
            {
                hidInventoryId.SetValue(_inventory_id);
                var ent = acc.Get_InventoryByKeyID(_inventory_id);
                if (ent != null)
                {
                    txtWarehouse.SetValue(ent.wh_id);
                    txtItemNumber.SetValue(ent.item_number);
                    txtItemDesc.SetValue(ent.description);
                    txtOrderNumber.SetValue(ent.attribute1);

                    txtLot.SetValue(ent.lot_number);
                    //txtLot.IsPrimary = !string.IsNullOrEmpty(ent.lot_number);

                    txtExpiryDate.SetValue(ent.exp_date);
                    //txtExpiryDate.IsPrimary = ent.exp_date != null;

                    txtMFGDate.SetValue(ent.mfg_date);
                    //txtMFGDate.IsPrimary = ent.mfg_date != null;

                    txtLPN.SetValue(ent.lpn);
                    //txtLPN.IsPrimary = !string.IsNullOrEmpty(ent.lpn);

                    hidQuantity.SetValue(ent.quantity);
                    txtOnHand.SetValue(ent.quantity);
                    //txtNumberofCopy.SetValue(1);

                    ddlUOM.MethodQueryProperty = delegate () { return Access.MasterData.ItemUom.Instance.GetQuery_WhItem(ent.wh_item_master_id); };
                    ddlUOM.BindDataSource();
                    ddlUOM.SetValue(ent.item_uom_id);

                    t_wms_print_label _Label = new t_wms_print_label();
                    string format = "yyyyMMdd";

                    // ParseExact method throws an exception if the parsing fails
                    try
                    {
                        DateTime result = DateTime.ParseExact(ent.expiry_date, format, CultureInfo.InvariantCulture);
                        _Label = acc._Model.t_wms_print_label.FirstOrDefault(x => x.wh_item_master_id == ent.wh_item_master_id && x.lpn == ent.lpn
                        && x.lot_number == ent.lot_number && x.mfg_date == ent.mfg_date && (x.expiry_date == null || x.expiry_date == result));
                    }
                    catch (FormatException ex)
                    {
                        Console.WriteLine("Error parsing DateTime: " + ex.Message);
                    }

                    if (_Label != null)
                    {
                        
                        txtMFGTime.SetValue(_Label.print_date);
                        var uom = acc._Model.t_wms_item_uom.FirstOrDefault(x => x.item_master_id == ent.item_master_id && x.uom == _Label.pack_size_uom && x.conversion_factor == _Label.pack_size_conversion_factor);
                        if(uom != null)
                        {
                            ddlUOM.SetValue(uom.item_uom_id);
                        }
                    }
                    else
                    {
                        txtMFGTime.Clear();
                    }

                    txtMFGTime.Update();
                    ddlUOM.ValueChange();
                }
            }

            //txtOptionField.Clear();

            popupPrint.ShowDialog();
        }


        void ddlUOM_PostValueChanged(dynamic _value)
        {
            try
            {
                txtConvFac.SetValue(0);

                var Quantity = (Int32)txtOnHand.GetValue();
                if (_value != null)
                {
                    var dto_uom = Access.MasterData.ItemUom.Instance.GetEditKeyID((Guid)_value);
                    txtConvFac.SetValue(dto_uom.conversion_factor);
                    txtPackSizePerPallet.SetValue(Quantity / (Int32)dto_uom.conversion_factor);
                }
                txtConvFac.Update();
                txtPackSizePerPallet.Update();
            }
            catch (Exception ex)
            {
                Logging = new Prototype.Providers.Logging(this, ex);
                RaiseLogging();
            }
        }
        void txtOnHand_TextEnterChanged(_IInputText obj)
        {
            var Quantity = (Int32)txtOnHand.GetValue();
            var con = (Int32)txtConvFac.GetValue();
            txtPackSizePerPallet.SetValue(Quantity / con);
            txtPackSizePerPallet.Update();
        }
        protected void btnConfrimPrint_Click(object sender, EventArgs e)
        {
            try
            {
                popupPrint.HideDialog();
                ClientPrint();
            }
            catch (Exception ex)
            {
                Logging = new Prototype.Providers.Logging(this, ex);
                RaiseLogging();
            }
        }
        
        //void Print_Barcode()
        //{
        //    try
        //    {
        //        //Get Parth
        //        string path = string.Empty;
        //        string path_trigger = string.Empty;

        //        using (var acc = new Access.Transaction.Inbound.InboundDetail())
        //        {
        //            Guid printer_id = ddlPrint.GetValue();
        //            Guid group_id = ddlGroupPrint.GetValue();

        //            var ent = acc._Model.t_com_config_printer_group_mapping.Where(w => w.group_id == group_id && w.printer_id == printer_id).FirstOrDefault();
        //            if (ent != null)
        //            {
        //                path = ent.bartender_data_filepath;
        //                path_trigger = ent.bartender_trigger_filepath;
        //            }
        //            else
        //            {
        //                this.Page.MessageWarning("Please set printer and group printer !");
        //                return;
        //            }

        //            //Exist Folder
        //            string[] arr = path.Split('\\');
        //            string folder = path.Substring(0, path.Length - (arr.Last().Length + 1));
        //            if (!Directory.Exists(folder))
        //            {
        //                Directory.CreateDirectory(folder);
        //            }

        //            using (var file = new StreamWriter(path, false, System.Text.UnicodeEncoding.Unicode))
        //            {
        //                string txtData = string.Empty;
        //                string txtSplit = "|";

        //                var sb = new StringBuilder();

        //                //Header
        //                sb.Append("warehouse|");
        //                sb.Append("item_number|");
        //                sb.Append("item_description|");
        //                sb.Append("uom|");
        //                sb.Append("pack_size|");
        //                sb.Append("lot|");
        //                sb.Append("expiry_date|");
        //                sb.Append("mfg_date|");
        //                sb.Append("lpn|");
        //                sb.Append("remark|");
        //                sb.Append("copy_no|");
        //                sb.Append("order_number");

        //                file.WriteLine(sb.ToString());


        //                //Detail
        //                for (var i = 0; i < txtNumberofCopy.GetValue().Value; i++)
        //                {
        //                    sb.Clear();

        //                    sb.Append(txtWarehouse.GetValue() + txtSplit);
        //                    sb.Append(txtItemNumber.GetValue() + txtSplit);
        //                    sb.Append(txtItemDesc.GetValue() + txtSplit);
        //                    sb.Append(ddlUOM.GetText() + txtSplit);
        //                    sb.Append(txtConvFac.GetValue() + txtSplit);
        //                    sb.Append(txtLot.GetValue() + txtSplit);

        //                    if (txtExpiryDate.GetValue() != null)
        //                        sb.Append(txtExpiryDate.GetValue().Value.ToString("yyyyMMdd", FieldsStatic.CultureInfo) + txtSplit);
        //                    else
        //                        sb.Append(txtSplit);

        //                    if (txtMFGDate.GetValue() != null)
        //                        sb.Append(txtMFGDate.GetValue().Value.ToString("yyyyMMdd", FieldsStatic.CultureInfo) + txtSplit);
        //                    else
        //                        sb.Append(txtSplit);

        //                    sb.Append(txtLPN.GetValue() + txtSplit);
        //                    sb.Append(txtRemark.GetValue() + txtSplit);
        //                    sb.Append(txtNumberofCopy.GetValue() + txtSplit);
        //                    sb.Append(txtOptionField.GetValue());

        //                    file.WriteLine(sb.ToString());
        //                }

        //                var fileTrigger = new System.IO.StreamWriter(path_trigger, false, System.Text.UnicodeEncoding.Unicode);
        //                fileTrigger.WriteLine("Write data complete." + DateTime.Now.ToString());
        //                fileTrigger.Close();
        //                this.Page.MessageSuccess("Print success.");

        //                popupPrint.HideDialog();
        //            }
        //        }
        //    }
        //    catch (Exception ex)
        //    {

        //        throw ex;
        //    }
        //}
        private List<ReportParameter> UcReportViewer_BindingParameter(string _report_id)
        {
            try
            {
                //string PackQuantity = Convert.ToString(Math.Floor(txtOnHand.GetValue() / txtConvFac.GetValue()));
                var prms = new List<ReportParameter>();
                prms.Add(new ReportParameter { Name = "@in_vchInventoryId", Value = hidInventoryId.GetValue().ToString() });
                prms.Add(new ReportParameter { Name = "@in_vchPackQuantity", Value = txtPackSizePerPallet.GetValue().ToString() });
                //prms.Add(new ReportParameter { Name = "@in_vchPackQuantity", Value = PackQuantity });
                prms.Add(new ReportParameter { Name = "@in_vchUOMId", Value = ddlUOM.GetValue().ToString() });
                prms.Add(new ReportParameter { Name = "@in_datePrintDate", Value = txtMFGTime.GetValue().ToString() });
                prms.Add(new ReportParameter { Name = "@in_vchUpdateBy", Value = _SessionVals.UserName });

                return prms;
            }
            catch (Exception ex)
            {

                throw ex;
            }
        }
        void ClientPrint()
        {
            string rp_name = "rptRePrintLabel.rdl";
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
                              window.location.href= window.location;

                              //$('#printOptionModal').modal('show');
                              //$('.modal-backdrop').remove();

                              //$('#printOptionModal').modal('hide');
                              //$('.modal-backdrop').remove();
                              //window.location.href= window.location;

                              history.go(0);
                        }, 2000);
                    }

                    $('#printOptionModal').on('show.bs.modal', function () {
                        $.fn.printReport('" + rp_name + @"','" + process_log_id + @"');
                    });

                    $('#printOptionModal').modal('show');     
                    $('.modal-backdrop').remove();

               });";

            ClientScriptExt.RegisterStartupScript(this.Page, js);
        }
        private DataTable GetSPResult()
        {
            string InventoryId = hidInventoryId.GetValue().ToString();
            string PackSizePerPallet = txtPackSizePerPallet.GetValue().ToString();
            string UOM = ddlUOM.GetValue().ToString();
            string MFGTime = txtMFGTime.GetValue().ToString();
           

            DataTable dt = new DataTable();
            using (var model = new Source.WMSEntities())
            {
                var cmd = model.Database.Connection.CreateCommand();
                try
                {

                    cmd.CommandText = "usp_wms_report_re_print_label";
                    cmd.CommandType = CommandType.StoredProcedure;

                    cmd.Parameters.Add(new SqlParameter { ParameterName = "@in_vchInventoryId", Value = (object)InventoryId ?? DBNull.Value });
                    cmd.Parameters.Add(new SqlParameter { ParameterName = "@in_vchPackQuantity", Value = (object)PackSizePerPallet ?? DBNull.Value });
                    cmd.Parameters.Add(new SqlParameter { ParameterName = "@in_vchUOMId", Value = (object)UOM ?? DBNull.Value });
                    cmd.Parameters.Add(new SqlParameter { ParameterName = "@in_datePrintDate", Value = (object)MFGTime ?? DBNull.Value });
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