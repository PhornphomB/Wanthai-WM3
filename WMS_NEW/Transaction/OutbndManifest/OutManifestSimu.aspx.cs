using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

using Prototype.Providers;
using WMS_NEW.Report.AscxControls;

namespace WMS_NEW.Transaction.OutbndManifest
{
    public partial class OutManifestSimu : PageCustom
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            try
            {
                #region Binding Event

                GridExt1.GridRowCommandClick += GridExt1_GridRowCommandClick;
                GridExt1.GridRowAfterDataBound += GridExt1_GridRowAfterDataBound;

                gridAddItems.GridRowCanSelectValidate += GridAddItems_GridRowCanSelectValidate;

                gridTspItem.GridRowAfterDataBound += GridTspItem_GridRowAfterDataBound;
                gridTspItem.GridRowCommandClick += GridTspItem_GridRowCommandClick;

                gridTruckSim.RowCommand += GridTruckSim_RowCommand;
                gridTruck.RowDataBound += GridTruck_RowDataBound;

                gridTspAssign.GridRowClick += GridTspAssign_GridRowClick;
                gridTspAssign.GridRowCommandClick += GridTspAssign_GridRowCommandClick;

                ReportViewer1.BindingParameter += ReportViewer1_BindingParameter;

                #endregion

                #region Initial Peoperty Column Grid

                GridColumnExt1.DropDownQueryProperty = delegate () { return Access.MasterData.Warehouse.Instance.GetQueryCode(_SessionVals.UserName); };
                GridColumnExt2.DropDownQueryProperty = delegate () { return Access.MasterData.Owner.Instance.GetQuery(_SessionVals.UserName); };
                GridColumnExt3.DropDownQueryProperty = delegate () { return Access.MasterData.ComboBoxItem.Instance.GetQueryProperty_Display("outbound_order_type"); };
                GridColumnExt5.DropDownQueryProperty = delegate () { return Access.MasterData.ComboBoxItem.Instance.GetQueryProperty_Display("outbound_order_status"); };
                GridColumnExt9.DropDownQueryProperty = delegate () { return Access.MasterData.Customer.Instance.GetQuery(); };

                #endregion

                ddManifestId.MethodQueryProperty = delegate () { return Access.Transaction.OutbndManifest.OutbndManifestExt.Instance.GetQuery(); };

                if (!Page.IsPostBack)
                {
                    hidSessionUser.SetValue(_SessionVals.UserName);
                    hidSessionId.SetValue(Session.SessionID);

                    gridTspItem.DataBind();
                    gridTspAssign.DataBind();

                    ddManifestId.BindDataSource();
                    txtManifestId.Visible = false;

                    btCalTruckVolume_Click(sender, e);

                    panelCmdTspSave.Visible = false;
                }
                else
                {
                    var eventControl = Request.Params.Get("__EVENTTARGET");
                    var eventArgument = Request.Params.Get("__EVENTARGUMENT");

                    if (eventControl == "_TRIG_CHOOSE_TRUCK")
                    {
                        CalChooseTruck();
                    }
                }
            }
            catch (Exception ex)
            {
                Logging = new Prototype.Providers.Logging(this, ex);
                RaiseLogging();
            }
        }

        private List<ReportParameter> ReportViewer1_BindingParameter(string _report_id)
        {
            var prms = new List<ReportParameter>();

            if (_report_id.ToUpper() == "411E4B12-E73C-486E-8E05-30D0A430EA5A".ToUpper())
            {
                prms.Add(new ReportParameter { Name = "@tsp_manifest_id", Value = rpt_tsp_manifest_id.ToString() });
                prms.Add(new ReportParameter { Name = "@truck_id", Value = rpt_truck_id.ToString() });
            }
            else if (_report_id.ToUpper() == "f6239355-fbcb-4fa6-8ed3-1b852bd48b18".ToUpper())
            {
                prms.Add(new ReportParameter { Name = "@manifest_code", Value = rpt_tsp_manifest_code });
                prms.Add(new ReportParameter { Name = "@print_by", Value = _SessionVals.UserName });
            }

            return prms;
        }


        string rpt_tsp_manifest_code = string.Empty;
        Guid rpt_tsp_manifest_id = Guid.Empty;
        Guid rpt_truck_id = Guid.Empty;

        private void GridTspAssign_GridRowCommandClick(GridViewCommandEventArgs e)
        {
            try
            {
                var tsp_truck_id = Guid.Parse(e.CommandArgument.ToString());

                if (e.CommandName == "REPORT")
                {
                    using (var _model = new Source.WMSEntities())
                    {
                        var dto = (from rows in _model.t_wms_outbound_manifest_tsp_truck
                                   where rows.tsp_truck_id == tsp_truck_id
                                   select new
                                   {
                                       rows.manifest_code,
                                       rows.tsp_manifest_id,
                                       rows.truck_id
                                   }).FirstOrDefault();

                        rpt_tsp_manifest_id = dto.tsp_manifest_id.Value;
                        rpt_tsp_manifest_code = dto.manifest_code;
                        rpt_truck_id = dto.truck_id;
                    }

                    ReportViewer1.ViewReportNow("f6239355-fbcb-4fa6-8ed3-1b852bd48b18");
                }
            }
            catch (Exception ex)
            {
                Logging = new Prototype.Providers.Logging(this, ex);
                RaiseLogging();
            }
        }

        private void GridTspAssign_GridRowClick(object _rowKeyValue)
        {
            try
            {
                var dto = Access.Transaction.OutbndManifest.ManifestTspAssign.Instance.GetEntityDto(Guid.Parse(_rowKeyValue.ToString()));

                txtViewManifestId.SetValue(dto.manifest_code);
                txtViewTruckLicen.SetValue(dto.license_plate);
                txtViewTruckName.SetValue(dto.truck_name);

                hidTspTruckId.SetValue(_rowKeyValue);

                gridTspAssDetail.Search();

                popupManifestDet.ShowDialog();
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
                var EDIT_ITEM = (Button)e.Row.FindControl("ADD_ITEM");
                EDIT_ITEM.CssClass = "btn btn-sm btn-success btn-ingrid";
                EDIT_ITEM.OnClientClick = "";
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
                if (e.CommandName == "ADD_ITEM")
                {
                    var id = Guid.Parse(e.CommandArgument.ToString());

                    using (var _model = new Source.WMSEntities())
                    {
                        var dto = (from rows in _model.t_wms_outbound_master
                                   where rows.outbound_order_master_id == id
                                   select new
                                   {
                                       rows.wh_id,
                                       rows.owner_code,
                                       rows.outbound_order_number,
                                       rows.order_status,
                                       customer_full = rows.customer_code + " : " + rows.customer_name,
                                       rows.t_wms_customer.province
                                   }).SingleOrDefault();

                        txtWarehouse.SetValue(dto.wh_id);
                        txtOwner.SetValue(dto.owner_code);
                        txtOutboundNo.SetValue(dto.outbound_order_number);

                        txtOrderStatus.SetValue(dto.order_status);
                        txtCustomer.SetValue(dto.customer_full);
                        txtCusProvince.SetValue(dto.province);
                    }

                    hidOrderMasterId.SetValue(id);
                    gridAddItems.Search();

                    popupAddItem.ShowDialog();
                }
            }
            catch (Exception ex)
            {
                Logging = new Prototype.Providers.Logging(this, ex);
                RaiseLogging();
            }
        }


        private bool GridAddItems_GridRowCanSelectValidate(GridViewRowEventArgs e)
        {
            var quantity_avalible = (double)DataBinder.Eval(e.Row.DataItem, "quantity_avalible");
            return (quantity_avalible > 0);
        }

        protected void btAddItemTsp_Click(object sender, EventArgs e)
        {
            try
            {
                if (gridAddItems.CountListKey() == 0)
                {
                    Page.MessageWarning("!Please select item.");
                    return;
                }

                int saved_result = 0;

                var out_ms_id = (Guid)hidOrderMasterId.GetValue();
                var keys = gridAddItems.GetListKey().Select(se => Guid.Parse(se.KeyId.ToString())).ToArray();

                var create_by = _SessionVals.UserName;
                var create_date = DateTime.Now;

                using (var _model = new Source.WMSEntities())
                {
                    var dto = (from rows in _model.t_wms_outbound_master
                               where rows.outbound_order_master_id == out_ms_id
                               select new
                               {
                                   rows.wh_master_id,
                                   rows.wh_id,
                                   rows.owner_id,
                                   rows.owner_code,
                                   rows.outbound_order_master_id,
                                   rows.outbound_order_number,
                                   rows.t_wms_customer.province
                               }).SingleOrDefault();

                    var detail = (from rows in _model.t_wms_outbound_detail
                                  where keys.Contains(rows.outbound_order_detail_id)
                                  join uom in _model.t_wms_item_uom on rows.item_uom_id equals uom.item_uom_id

                                  let quantity_tsp = _model.t_wms_outbound_manifest_tsp_order_de.Where(x => x.outbound_order_detail_id == rows.outbound_order_detail_id).Sum(sm => sm.item_quantity) ?? 0

                                  select new
                                  {
                                      rows.outbound_order_detail_id,
                                      rows.line_number,
                                      rows.wh_item_master_id,
                                      rows.item_master_id,
                                      rows.item_number,
                                      quantity_order = (rows.quantity_order - quantity_tsp),
                                      rows.item_uom_id,
                                      rows.uom,
                                      uom.primary_uom,
                                      uom.length,
                                      uom.weight,
                                      uom.height,
                                      uom.width,
                                      uom.conversion_factor
                                  }).ToArray();


                    Source.t_wms_outbound_manifest_tsp_order_de ent_det;

                    foreach (var item in detail)
                    {
                        ent_det = new Source.t_wms_outbound_manifest_tsp_order_de();
                        ent_det.tsp_order_detail_id = Guid.NewGuid();
                        ent_det.session_id = Session.SessionID;
                        ent_det.create_by = create_by;
                        ent_det.create_date = create_date;
                        ent_det.wh_master_id = dto.wh_master_id;
                        ent_det.wh_id = dto.wh_id;
                        ent_det.owner_id = dto.owner_id;
                        ent_det.owner_code = dto.owner_code;
                        ent_det.cus_province = dto.province;
                        ent_det.outbound_order_master_id = dto.outbound_order_master_id;
                        ent_det.outbound_order_number = dto.outbound_order_number;
                        ent_det.outbound_order_detail_id = item.outbound_order_detail_id;
                        ent_det.line_number = item.line_number;
                        ent_det.wh_item_master_id = item.wh_item_master_id;
                        ent_det.item_master_id = item.item_master_id;
                        ent_det.item_number = item.item_number;

                        _model.t_wms_outbound_manifest_tsp_order_de.Add(ent_det);

                        if (item.primary_uom.ToUpper() == "YES")
                        {
                            ent_det.item_uom_id = item.item_uom_id;
                            ent_det.uom = item.uom.Trim();
                            ent_det.uom_length = item.length;
                            ent_det.uom_weight = item.weight;
                            ent_det.uom_width = item.width;
                            ent_det.uom_height = item.height;
                            ent_det.item_quantity = item.quantity_order;
                            ent_det.volume_per = (item.width * item.length * item.height);
                            ent_det.volume_total = ent_det.volume_per * ent_det.item_quantity;
                        }
                        else
                        {
                            var uom_pri = _model.t_wms_item_uom
                                            .Select(se =>
                                            new
                                            {
                                                se.item_master_id,
                                                se.item_uom_id,
                                                se.uom,
                                                se.primary_uom,
                                                se.length,
                                                se.weight,
                                                se.height,
                                                se.width
                                            })
                                            .FirstOrDefault(x => x.item_master_id == item.item_master_id && x.primary_uom == "YES");

                            if (uom_pri != null)
                            {
                                ent_det.item_uom_id = uom_pri.item_uom_id;
                                ent_det.uom = uom_pri.uom.Trim();
                                ent_det.uom_length = uom_pri.length;
                                ent_det.uom_weight = uom_pri.weight;
                                ent_det.uom_width = uom_pri.width;
                                ent_det.uom_height = uom_pri.height;
                                ent_det.item_quantity = (item.quantity_order * item.conversion_factor);
                                ent_det.volume_per = (uom_pri.width * uom_pri.length * uom_pri.height);
                                ent_det.volume_total = ent_det.volume_per * ent_det.item_quantity;
                            }
                        }
                    }


                    saved_result = _model.SaveChanges();
                }

                if (saved_result > 0)
                {
                    gridTspItem.DataBind();
                    btCalTruckVolume_Click(sender, e);

                    popupAddItem.HideDialog();

                    Page.MessageSuccess("Add Items to Transport Success.");
                }
            }
            catch (Exception ex)
            {
                Logging = new Prototype.Providers.Logging(this, ex);
                RaiseLogging();
            }
        }


        Guid tsp_order_detail_id
        {
            get
            {
                return (Guid)ViewState["tsp_order_detail_id"];
            }
            set
            {
                ViewState["tsp_order_detail_id"] = value;
            }
        }

        private void GridTspItem_GridRowAfterDataBound(GridViewRowEventArgs e)
        {
            var ITEM_SPLIT = (Button)e.Row.FindControl("ITEM_SPLIT");
            ITEM_SPLIT.CssClass = "btn btn-sm btn-info btn-ingrid";
            ITEM_SPLIT.OnClientClick = "";

            var item_quantity = (double?)DataBinder.Eval(e.Row.DataItem, "item_quantity");
            ITEM_SPLIT.Enabled = (item_quantity > 1);
        }

        private void GridTspItem_GridRowCommandClick(GridViewCommandEventArgs e)
        {
            if (e.CommandName == "ITEM_SPLIT")
            {
                var tsp_id = Guid.Parse(e.CommandArgument.ToString());
                tsp_order_detail_id = tsp_id;

                using (var _model = new Source.WMSEntities())
                {
                    var dto = (from rows in _model.t_wms_outbound_manifest_tsp_order_de
                               where rows.tsp_order_detail_id == tsp_id
                               select new
                               {
                                   rows.line_number,
                                   rows.item_number,
                                   rows.item_quantity
                               }).SingleOrDefault();


                    txtSplitLineNumber.SetValue(dto.line_number);
                    txtSplitItemNumber.SetValue(dto.item_number);
                    txtSplitQtyCurrent.SetValue(dto.item_quantity);
                    txtSplitQty.Clear();
                }

                popupItemSplit.ShowDialog();
            }
        }

        protected void btSplitSave_Click(object sender, EventArgs e)
        {
            try
            {
                if (txtSplitQty.GetValue() > txtSplitQtyCurrent.GetValue())
                {
                    Page.MessageWarning("! [Split Qty] more than [Current Qty]");
                    return;
                }

                Guid tsp_id = tsp_order_detail_id;
                var create_by = _SessionVals.UserName;
                var create_date = DateTime.Now;

                using (var _model = new Source.WMSEntities())
                {
                    var ent = _model.t_wms_outbound_manifest_tsp_order_de.SingleOrDefault(x => x.tsp_order_detail_id == tsp_id);
                    if (ent != null)
                    {
                        var ent_split = new Source.t_wms_outbound_manifest_tsp_order_de();
                        ent_split.CloneObject(ent);

                        ent_split.tsp_order_detail_id = Guid.NewGuid();
                        ent_split.item_quantity = (double)txtSplitQty.GetValue();
                        ent_split.volume_total = ent_split.volume_per * ent_split.item_quantity;
                        ent_split.create_by = create_by;
                        ent_split.create_date = create_date;

                        _model.t_wms_outbound_manifest_tsp_order_de.Add(ent_split);

                        ent.item_quantity = (ent.item_quantity - ent_split.item_quantity);
                        ent.volume_total = ent.volume_per * ent.item_quantity;
                        ent.update_by = create_by;
                        ent.update_date = create_date;

                        _model.SaveChanges();

                        Page.MessageSuccess("Split Item Quantity Success.");

                        gridTspItem.DataBind();
                        popupItemSplit.HideDialog();
                    }
                }

            }
            catch (Exception ex)
            {
                Logging = new Prototype.Providers.Logging(this, ex);
                RaiseLogging();
            }
        }


        protected void btTspItemDel_Click(object sender, EventArgs e)
        {
            try
            {
                if (gridTspItem.CountListKey() == 0)
                {
                    Page.MessageWarning("!Please select item.");
                    return;
                }

                var keys = gridTspItem.GetListKey().Select(se => Guid.Parse(se.KeyId.ToString())).ToArray();

                using (var _model = new Source.WMSEntities())
                {
                    var dels = _model.t_wms_outbound_manifest_tsp_order_de.Where(x => keys.Contains(x.tsp_order_detail_id));

                    _model.t_wms_outbound_manifest_tsp_order_de.RemoveRange(dels);
                    _model.SaveChanges();
                }

                gridTspItem.DataBind();
                btCalTruckVolume_Click(sender, e);
            }
            catch (Exception ex)
            {
                Logging = new Prototype.Providers.Logging(this, ex);
                RaiseLogging();
            }
        }

        double GetItemsVolumeTotal()
        {
            var keys = gridTspItem.GetListKey().Select(se => Guid.Parse(se.KeyId.ToString())).ToArray();

            string session_id = Session.SessionID;
            double? volume_total = 0;

            using (var _model = new Source.WMSEntities())
            {
                if (keys.Count() > 0)
                    volume_total = _model.t_wms_outbound_manifest_tsp_order_de.Where(x => x.session_id == session_id && keys.Contains(x.tsp_order_detail_id)).Sum(sm => sm.volume_total);
                else
                    volume_total = _model.t_wms_outbound_manifest_tsp_order_de.Where(x => x.session_id == session_id).Sum(sm => sm.volume_total);

                if (volume_total == null) volume_total = 0;

                return volume_total.Value;
            }
        }

        protected void btCalTruckVolume_Click(object sender, EventArgs e)
        {
            try
            {
                var volume_total = GetItemsVolumeTotal();

                using (var _model = new Source.WMSEntities())
                {
                    var result_trucks = (from rows in _model.t_wms_truck_type
                                         select new
                                         {
                                             rows.truck_type_id,
                                             rows.truck_type,
                                             volume_per = (rows.width * rows.length * rows.height),
                                             truck_amt = (volume_total / (rows.width * rows.length * rows.height))
                                         }).ToArray();


                    gridTruckSim.DataSource = result_trucks;
                    gridTruckSim.DataBind();
                }

                var keys_count = gridTspItem.CountListKey();

                labTspItemDesc.InnerText = "Order Items : " + (keys_count > 0 ? keys_count + " รายการ" : "ทุกรายการ");
                labTspItemVol.InnerText = "ปริมาตรที่ใช้: " + volume_total.ToString("0.##");

                updateTspCalTruck.Update();
            }
            catch (Exception ex)
            {
                Logging = new Prototype.Providers.Logging(this, ex);
                RaiseLogging();
            }
        }


        private void GridTruckSim_RowCommand(object sender, GridViewCommandEventArgs e)
        {
            if (e.CommandName == "CHOOSE_TYPE")
            {
                TruckTypeId = Guid.Parse(e.CommandArgument.ToString());
                ViewTrucks();
            }
        }

        double TruckTypeVolumePer
        {
            get
            {
                if (ViewState["TruckTypeVolumePer"] == null)
                    ViewState["TruckTypeVolumePer"] = 0;

                return (double)ViewState["TruckTypeVolumePer"];
            }
            set
            {
                ViewState["TruckTypeVolumePer"] = value;
            }
        }

        double ItemsVolumeTotalNow
        {
            get
            {
                if (ViewState["ItemsVolumeTotalNow"] == null)
                    ViewState["ItemsVolumeTotalNow"] = 0;

                return (double)ViewState["ItemsVolumeTotalNow"];
            }
            set
            {
                ViewState["ItemsVolumeTotalNow"] = value;
            }
        }

        Guid TruckTypeId
        {
            get
            {
                if (ViewState["TruckTypeId"] == null)
                    ViewState["TruckTypeId"] = Guid.Empty;

                return (Guid)ViewState["TruckTypeId"];
            }
            set
            {
                ViewState["TruckTypeId"] = value;
            }
        }

        Guid TruckId
        {
            get
            {
                if (ViewState["TruckId"] == null)
                    ViewState["TruckId"] = Guid.Empty;

                return (Guid)ViewState["TruckId"];
            }
            set
            {
                ViewState["TruckId"] = value;
            }
        }

        void ViewTrucks()
        {
            try
            {
                double volume_per = 0;
                TruckTypeVolumePer = 0;

                ItemsVolumeTotalNow = GetItemsVolumeTotal();
                var keys_count = gridTspItem.CountListKey();

                labTspItemDesc.InnerText = "Order Items : " + (keys_count > 0 ? keys_count + " รายการ" : "ทุกรายการ");
                labTspItemVol.InnerText = "ปริมาตรที่ใช้ : " + ItemsVolumeTotalNow.ToString("0.##");


                foreach (GridViewRow gr in gridTruckSim.Rows)
                {
                    if (TruckTypeId == Guid.Parse(gridTruckSim.DataKeys[gr.RowIndex]["truck_type_id"].ToString()))
                    {
                        Button btn = (Button)gr.Cells[0].FindControl("btChooseType");
                        HiddenField hid = (HiddenField)gr.Cells[0].FindControl("hidVolumePer");

                        var text_volume_per = gr.Cells[1].Text;
                        var text_truck_amt = gr.Cells[2].Text;

                        volume_per = Convert.ToDouble(hid.Value);
                        TruckTypeVolumePer = volume_per;

                        labTruckTypeDesc.InnerText = "ประเภทรถ : " + btn.Text;
                        labTruckVolDesc.InnerText = "ปริมาตร/คัน : " + text_volume_per;

                        break;
                    }
                }

                using (var _model = new Source.WMSEntities())
                {
                    var result_trucks = (from rows in _model.t_wms_truck
                                         where rows.truck_type_id == TruckTypeId
                                         let volume_used = _model.t_wms_outbound_manifest_tsp_truck.Where(x => x.truck_id == rows.truck_id).Sum(x => x.volume_total) ?? 0
                                         orderby rows.truck_name
                                         select new
                                         {
                                             rows.truck_id,
                                             rows.license_plate,
                                             rows.truck_name,
                                             volume_used = volume_used,
                                             volume_remain = (volume_per - volume_used)
                                         }).ToArray();

                    gridTruck.DataSource = result_trucks;
                    gridTruck.DataBind();
                }


                panelCmdSimulate.Visible = false;
                panelCmdTspSave.Visible = true;
                btTspSave.Enabled = false;

                updateTspCalTruck.Update();
            }
            catch (Exception ex)
            {
                Logging = new Prototype.Providers.Logging(this, ex);
                RaiseLogging();
            }
        }

        protected void btTspBack_Click(object sender, EventArgs e)
        {
            try
            {
                gridTruck.DataSource = null;
                gridTruck.DataBind();

                panelCmdSimulate.Visible = true;
                panelCmdTspSave.Visible = false;

                updateTspCalTruck.Update();
            }
            catch (Exception ex)
            {
                Logging = new Prototype.Providers.Logging(this, ex);
                RaiseLogging();
            }
        }

        private void GridTruck_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            if (e.Row.RowType == DataControlRowType.DataRow)
            {
                var volume_remain = (double)DataBinder.Eval(e.Row.DataItem, "volume_remain");

                if ((volume_remain - ItemsVolumeTotalNow) >= 0)
                {
                    e.Row.Cells[4].Attributes.Add("style", "background-color:#13b34e;color:#fff;");
                }
                else
                {
                    e.Row.Cells[4].Attributes.Add("style", "background-color:#df2035;color:#fff;");
                }
            }
        }

        private void CalChooseTruck()
        {
            try
            {
                //bool has_choosed = false;

                for (int i = 0; i < gridTruck.Rows.Count; i++)
                {
                    var rb = (RadioButton)gridTruck.Rows[i].Cells[0].FindControl("rdbChooseTruck");
                    if (rb != null && rb.Checked)
                    {
                        var hf = (HiddenField)gridTruck.Rows[i].Cells[0].FindControl("hidChooseTruck");
                        if (hf != null)
                        {
                            TruckId = Guid.Parse(hf.Value);
                        }

                        //var volume_total = GetItemsVolumeTotal();
                        //var keys_count = gridTspItem.CountListKey();
                        //var volume_remain = _volume_remain_now == null ? gridTruck.Rows[i].Cells[4].Text : _volume_remain_now.Value.ToString("0.##");

                        //labCalTruckVolResult.InnerHtml = "<p>Order Items : " + (keys_count > 0 ? keys_count + " รายการ" : "ทุกรายการ") + "</p>";
                        //labCalTruckVolResult.InnerHtml += "<p>ปริมาตรที่ใช้ : " + volume_total.ToString("0.##") + "</p>";
                        //labCalTruckVolResult.InnerHtml += "<p>ปริมาตรที่ว่างในรถ : " + volume_remain + "</p>";

                        //var volume_cal = Convert.ToDouble(volume_remain) - volume_total;

                        //labCalTruckVolResult.Attributes["class"] = "label label-" + (volume_cal >= 0 ? "success" : "important");

                        //has_choosed = true;

                        break;
                    }
                }

                btTspSave.Enabled = true;
                updateSaveManifest.Update();
            }
            catch (Exception ex)
            {
                Logging = new Prototype.Providers.Logging(this, ex);
                RaiseLogging();
            }
        }

        protected void btReCalTruckVolume_Click(object sender, EventArgs e)
        {
            ViewTrucks();
        }


        bool IsNewManifest
        {
            get
            {
                if (ViewState["IsNewManifest"] == null)
                    ViewState["IsNewManifest"] = false;

                return (bool)ViewState["IsNewManifest"];
            }
            set
            {
                ViewState["IsNewManifest"] = value;
            }
        }

        protected void btTspNew_Click(object sender, EventArgs e)
        {
            try
            {
                IsNewManifest = !IsNewManifest;

                btTspNew.Text = btTspNew.Text != "Select" ? "Select" : "New";

                ddManifestId.Visible = !ddManifestId.Visible;
                txtManifestId.Visible = !txtManifestId.Visible;

                updateSaveManifest.Update();
            }
            catch (Exception ex)
            {
                Logging = new Prototype.Providers.Logging(this, ex);
                RaiseLogging();
            }
        }

        protected void btTspSave_Click(object sender, EventArgs e)
        {
            try
            {
                if (gridTspItem.CountListKey() == 0)
                {
                    Page.MessageWarning("! Please Select Order Items");
                    return;
                }
                if (!IsNewManifest && string.IsNullOrEmpty(ddManifestId.GetValue()))
                {
                    Page.MessageWarning("! Please Select Manifest Id");
                    return;
                }
                else if (IsNewManifest && string.IsNullOrEmpty(txtManifestId.GetValue()))
                {
                    Page.MessageWarning("! Please Input Manifest Id");
                    return;
                }


                var item_volume_total = GetItemsVolumeTotal();

                using (var _model = new WMS_NEW.Source.WMSEntities())
                {
                    var truck_volume_used = _model.t_wms_outbound_manifest_tsp_truck.Where(x => x.truck_id == TruckId).Sum(x => x.volume_total) ?? 0;

                    if ((TruckTypeVolumePer - truck_volume_used) < item_volume_total)
                    {
                        Page.MessageWarning("! Cannot Assign Truck, Because due to the volume of space in the car Less than the volume of the selected item.");
                        return;
                    }

                    var create_by = _SessionVals.UserName;
                    var create_date = DateTime.Now;
                    var manifest_code = !IsNewManifest ? (string)ddManifestId.GetValue() : txtManifestId.GetValue();

                    var ent_man = _model.t_wms_outbound_manifest_tsp.FirstOrDefault(x => x.manifest_code == manifest_code);
                    if (ent_man == null)
                    {
                        ent_man = new Source.t_wms_outbound_manifest_tsp();
                        ent_man.tsp_manifest_id = Guid.NewGuid();
                        ent_man.manifest_code = manifest_code;
                        ent_man.create_by = create_by;
                        ent_man.create_date = create_date;

                        _model.t_wms_outbound_manifest_tsp.Add(ent_man);
                    }

                    var ent_tck = _model.t_wms_outbound_manifest_tsp_truck.FirstOrDefault(x => x.tsp_manifest_id == ent_man.tsp_manifest_id && x.truck_id == TruckId);
                    if (ent_tck == null)
                    {
                        ent_tck = new Source.t_wms_outbound_manifest_tsp_truck();
                        ent_tck.tsp_truck_id = Guid.NewGuid();
                        ent_tck.tsp_manifest_id = ent_man.tsp_manifest_id;
                        ent_tck.manifest_code = ent_man.manifest_code;
                        ent_tck.truck_id = TruckId;
                        ent_tck.volume_total = 0;
                        ent_tck.create_by = create_by;
                        ent_tck.create_date = create_date;

                        ent_man.t_wms_outbound_manifest_tsp_truck.Add(ent_tck);
                    }
                    else
                    {
                        ent_tck.update_by = create_by;
                        ent_tck.update_date = create_date;
                    }


                    var entities_item = from rows in _model.t_wms_outbound_manifest_tsp_order_de
                                        where rows.session_id == Session.SessionID
                                        select rows;

                    var keys = gridTspItem.GetListKey().Select(se => Guid.Parse(se.KeyId.ToString())).ToArray();
                    if (keys.Count() > 0)
                    {
                        entities_item = entities_item.Where(x => keys.Contains(x.tsp_order_detail_id));
                    }

                    foreach (var item in entities_item)
                    {
                        ent_tck.volume_total += item.volume_total;

                        item.tsp_truck_id = ent_tck.tsp_truck_id;
                        item.tsp_manifest_id = ent_man.tsp_manifest_id;
                        item.manifest_code = ent_man.manifest_code;
                        item.session_id = string.Empty;
                        item.update_by = create_by;
                        item.update_date = create_date;
                    }

                    _model.SaveChanges();

                    if (IsNewManifest)
                    {
                        txtManifestId.Clear();
                        ddManifestId.SetValue(manifest_code);
                        btTspNew_Click(sender, e);
                    }

                    ViewTrucks();

                    gridTspItem.DataBind();
                    gridTspAssign.DataBind();

                    Page.MessageSuccess("Assign Manifest Success.");
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