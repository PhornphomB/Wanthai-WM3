using System;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

using System.Data.Entity;
using System.Data.Entity.SqlServer;
using Prototype.Providers;
using System.Data;
using System.Data.SqlClient;
using WMS_NEW.Source;
using System.Data.Entity.Core.Objects;

namespace WMS_NEW.DashBoard
{
    public partial class DBoardCenter : PageCustom
    {

        public bool HAS_INBOUND
        {
            get
            {
                if (ViewState["HAS_INBOUND"] == null)
                    ViewState["HAS_INBOUND"] = false;

                return (bool)ViewState["HAS_INBOUND"];
            }
            set
            {
                ViewState["HAS_INBOUND"] = value;
            }
        }

        public bool HAS_OUTBOUND
        {
            get
            {
                if (ViewState["HAS_OUTBOUND"] == null)
                    ViewState["HAS_OUTBOUND"] = false;

                return (bool)ViewState["HAS_OUTBOUND"];
            }
            set
            {
                ViewState["HAS_OUTBOUND"] = value;
            }
        }

        public bool HAS_INVENTORY
        {
            get
            {
                if (ViewState["HAS_INVENTORY"] == null)
                    ViewState["HAS_INVENTORY"] = false;

                return (bool)ViewState["HAS_INVENTORY"];
            }
            set
            {
                ViewState["HAS_INVENTORY"] = value;
            }
        }

        public bool HAS_LOCATION
        {
            get
            {
                if (ViewState["HAS_LOCATION"] == null)
                    ViewState["HAS_LOCATION"] = false;

                return (bool)ViewState["HAS_LOCATION"];
            }
            set
            {
                ViewState["HAS_LOCATION"] = value;
            }
        }


        protected void Page_Load(object sender, EventArgs e)
        {
            try
            {
                if (!Page.IsPostBack)
                {
                    //var ms = (Page.Master as Layout);
                    //ThemeCurrent = ms.ThemeLoad();
                    ThemeLoad();

                    var list = Access.MasterData.Warehouse.Instance.GetQueryForDBoard_User(_SessionVals.UserName).ToList();

                    Prototype.Providers.Controls.ControlsList.BindListBox(ref ddlWarehouse, list, list.Count > 0 ? "ALL WAREHOUSE" : "NO WAREHOUSE");

                    ddlWarehouse_SelectedIndexChanged(sender, e);

                    var menus = (List<ConfigGlobal.DTO.Authen.MenuView>)Session["MenuView"];

                    HAS_INBOUND = menus.Any(x => x.MenuCode.ToLower() == "6612c1c1e07e257ea3fff1ad6fdd5d55".ToLower());
                    HAS_OUTBOUND = menus.Any(x => x.MenuCode.ToLower() == "c7485eb1243204a306814dc8334a6246".ToLower());
                    HAS_INVENTORY = menus.Any(x => x.MenuCode.ToLower() == "a981f42538de98445b4c3532ed21ad70".ToLower());
                    HAS_LOCATION = menus.Any(x => x.MenuCode.ToLower() == "7f718f584ea60a15c3a382a090b22e18".ToLower());
                }
            }
            catch (Exception ex)
            {
                Logging = new Prototype.Providers.Logging(this, ex);
                RaiseLogging();
            }
        }

        public string ThemeLoad()
        {
            var name = "";

            var cookInfo = Request.Cookies["WMS_CFG"];
            if (cookInfo != null)
            {
                name = cookInfo["THEME_NAME"].ToString();
                ThemeCurrent = name;
            }

            return name;
        }

        protected void Page_PreRender(object sender, EventArgs e)
        {
            try
            {
                if (Page.IsPostBack)
                {
                    var main_theme = (Page.Master as Layout).ThemeName;
                    if (ThemeCurrent.ToUpper() != main_theme.ToUpper())
                    {
                        ThemeCurrent = main_theme;

                        var wh_master_id = !string.IsNullOrEmpty(ddlWarehouse.SelectedValue) ? Guid.Parse(ddlWarehouse.SelectedValue) : Guid.Empty;

                        BindChartTransac(null, wh_master_id);
                        BindChartItemMovement(null, wh_master_id);
                        BindChartLocationMovement(null, wh_master_id);
                    }
                }
            }
            catch (Exception ex)
            {
                Logging = new Prototype.Providers.Logging(this, ex);
                RaiseLogging();
            }
        }


        protected void ddlWarehouse_SelectedIndexChanged(object sender, EventArgs e)
        {
            try
            {
                ViewNow();
            }
            catch (Exception ex)
            {
                Logging = new Prototype.Providers.Logging(this, ex);
                RaiseLogging();
            }
        }


        public string ThemeCurrent
        {
            get
            {
                if (ViewState["ThemeCurrent"] == null)
                    ViewState["ThemeCurrent"] = "LIGHT";

                return ViewState["ThemeCurrent"].ToString();
            }
            set
            {
                ViewState["ThemeCurrent"] = value;
            }
        }

        void ViewNow()
        {
            var _model = new Source.WMSEntities();

            try
            {
                var wh_master_id = !string.IsNullOrEmpty(ddlWarehouse.SelectedValue) ? Guid.Parse(ddlWarehouse.SelectedValue) : Guid.Empty;

                var format_comma = Extensions.FormatDecimal;  //"#,##0";
                var dateNow = DateTime.Now.Date;


                #region Card Group 1

                var bar_inbound_plan = _model.v_wms_dashb_center_card_inbound.AsNoTracking().Where(x => (wh_master_id != Guid.Empty ? x.wh_master_id == wh_master_id : _model.t_wms_wh_user.Any(us => us.user_id == _SessionVals.UserName && us.wh_master_id == x.wh_master_id))).FirstOrDefault();
                var bar_inbound_actual = _model.v_wms_dashb_center_period_inbound.AsNoTracking().Where(x => (wh_master_id != Guid.Empty ? x.wh_master_id == wh_master_id : _model.t_wms_wh_user.Any(us => us.user_id == _SessionVals.UserName && us.wh_master_id == x.wh_master_id)) && DbFunctions.TruncateTime(x.trans_date) == dateNow).ToArray();

                var bar_outbound_plan = _model.v_wms_dashb_center_card_outbound.AsNoTracking().Where(x => (wh_master_id != Guid.Empty ? x.wh_master_id == wh_master_id : _model.t_wms_wh_user.Any(us => us.user_id == _SessionVals.UserName && us.wh_master_id == x.wh_master_id))).FirstOrDefault();
                var bar_outbound_actual = _model.v_wms_dashb_center_period_outbound.AsNoTracking().Where(x => (wh_master_id != Guid.Empty ? x.wh_master_id == wh_master_id : _model.t_wms_wh_user.Any(us => us.user_id == _SessionVals.UserName && us.wh_master_id == x.wh_master_id)) && DbFunctions.TruncateTime(x.trans_date) == dateNow).ToArray();

                var bar_inventory = (from rows in _model.t_wms_inventory
                                     let stat = rows.t_wms_inventory_status
                                     where stat.inv_status == "Available" && rows.t_wms_location.loc_type != "STG"
                                     && (wh_master_id != Guid.Empty ? rows.wh_master_id == wh_master_id : _model.t_wms_wh_user.Any(us => us.user_id == _SessionVals.UserName && us.wh_master_id == rows.wh_master_id))
                                     group rows by new
                                     {
                                         stat.inv_status
                                     } into grb
                                     select new
                                     {
                                         qty_order = grb.Sum(sm => sm.quantity),
                                         qty_actual = grb.Sum(sm => sm.quantity_allocated)
                                     }).FirstOrDefault();

                var bar_loc_inv = (from rows in _model.t_wms_inventory
                                   where rows.t_wms_location.is_active == "YES" && rows.t_wms_location.loc_type != "STG" && rows.t_wms_inventory_status.inv_status == "Available"
                                   && (wh_master_id != Guid.Empty ? rows.wh_master_id == wh_master_id : _model.t_wms_wh_user.Any(us => us.user_id == _SessionVals.UserName && us.wh_master_id == rows.wh_master_id))
                                   group rows.location_id by new
                                   {
                                       rows.t_wms_inventory_status.inv_status
                                   } into grb
                                   select new
                                   {
                                       qty_order = grb.Distinct().Count()
                                   }).FirstOrDefault();

                var bar_location = (from rows in _model.t_wms_location
                                    where rows.is_active == "YES" && rows.loc_type != "STG"
                                    && (wh_master_id != Guid.Empty ? rows.wh_master_id == wh_master_id : _model.t_wms_wh_user.Any(us => us.user_id == _SessionVals.UserName && us.wh_master_id == rows.wh_master_id))
                                    group rows by new
                                    {
                                        rows.is_active
                                    } into grb
                                    select new
                                    {
                                        qty_order = grb.Count()
                                    }).FirstOrDefault();


                if (bar_inbound_plan != null)
                {
                    var qty_actual = 0.0;

                    if (bar_inbound_actual.Any())
                        qty_actual = bar_inbound_actual.Sum(sm => sm.quantity) ?? 0;

                    labInbActual.InnerText = qty_actual.ToString(format_comma);
                    labInbPlan.InnerText = bar_inbound_plan.quantity_order.ToString(format_comma);

                    var per = (100 * qty_actual) / bar_inbound_plan.quantity_order;

                    progInb.Attributes["style"] = "width:" + Convert.ToInt32(per) + "%";
                    progInb.Attributes["aria-valuenow"] = per.ToString();
                }
                else
                {
                    labInbActual.InnerText = "0";
                    labInbPlan.InnerText = "0";

                    progInb.Attributes["style"] = "width:0%";
                    progInb.Attributes["aria-valuenow"] = "0";
                }

                if (bar_outbound_plan != null)
                {
                    var qty_actual = 0.0;

                    if (bar_outbound_actual.Any())
                        qty_actual = bar_outbound_actual.Sum(sm => sm.quantity) ?? 0;

                    labOutActual.InnerText = qty_actual.ToString(format_comma);
                    labOutPlan.InnerText = bar_outbound_plan.quantity_order.ToString(format_comma);

                    var per = (100 * qty_actual) / bar_outbound_plan.quantity_order;

                    progOut.Attributes["style"] = "width:" + Convert.ToInt32(per) + "%";
                    progOut.Attributes["aria-valuenow"] = per.ToString();
                }
                else
                {
                    labOutActual.InnerText = "0";
                    labOutPlan.InnerText = "0";

                    progOut.Attributes["style"] = "width:0%";
                    progOut.Attributes["aria-valuenow"] = "0";
                }

                if (bar_inventory != null)
                {
                    labInvActual.InnerText = bar_inventory.qty_actual.ToString(format_comma);
                    labInvPlan.InnerText = bar_inventory.qty_order.ToString(format_comma);

                    var per = (100 * bar_inventory.qty_actual) / bar_inventory.qty_order;

                    progInv.Attributes["style"] = "width:" + Convert.ToInt32(per) + "%";
                    progInv.Attributes["aria-valuenow"] = per.ToString();
                }
                else
                {
                    labInvActual.InnerText = "0";
                    labInvPlan.InnerText = "0";

                    progInv.Attributes["style"] = "width:0%";
                    progInv.Attributes["aria-valuenow"] = "0";
                }

                if (bar_location != null)
                {
                    if (bar_loc_inv != null)
                        labLocActual.InnerText = bar_loc_inv.qty_order.ToString(format_comma);

                    labLocPlan.InnerText = bar_location.qty_order.ToString(format_comma);

                    var per = (100 * (bar_loc_inv != null ? bar_loc_inv.qty_order : 0)) / bar_location.qty_order;

                    progLoc.Attributes["style"] = "width:" + Convert.ToInt32(per) + "%";
                    progLoc.Attributes["aria-valuenow"] = per.ToString();
                }
                else
                {
                    labLocActual.InnerText = "0";
                    labLocPlan.InnerText = "0";

                    progLoc.Attributes["style"] = "width:0%";
                    progLoc.Attributes["aria-valuenow"] = "0";
                }

                #endregion

                #region Card Group 2

                #region Chart 1

                BindChartTransac(_model, wh_master_id);

                #endregion

                #region Chart 2

                var listColor = ChartInvStatusColor();

                var query = (from rows in _model.t_wms_inventory
                             let stat = rows.t_wms_inventory_status
                             where rows.t_wms_location.loc_type != "STG" && (wh_master_id != Guid.Empty ? rows.wh_master_id == wh_master_id : _model.t_wms_wh_user.Any(us => us.user_id == _SessionVals.UserName && us.wh_master_id == rows.wh_master_id))
                             group rows by new
                             {
                                 stat.inventory_status_id,
                                 stat.inv_status,
                                 stat.sequence
                             } into grb
                             select new
                             {
                                 grb.Key.inventory_status_id,
                                 grb.Key.inv_status,
                                 grb.Key.sequence,
                                 total = grb.Sum(sm => sm.quantity)
                             }).AsEnumerable();

                var result = (from color in listColor
                              join rows in query on color.sequence equals rows.sequence
                              orderby rows.sequence, color.sequence
                              select new
                              {
                                  color.status,
                                  color.background_color,
                                  color.border_color,
                                  color.sequence,
                                  rows.total,
                              }).ToArray();


                string data_pie = string.Empty;
                string label_pie = string.Empty;
                string color_pie = string.Empty;
                double sumInv = result.Sum(s => s.total);


                labInvStatAval.InnerText = "0";
                labInvStatDam.InnerText = "0";
                //labInvStatHold.InnerText = "0";

                foreach (var item in result)
                {
                    double percent = sumInv == 0 ? 0 : ((item.total / sumInv) * 100);
                    data_pie += item.total.ToString() + ",";
                    label_pie += "'" + item.status.ToString() + "',";
                    color_pie += "'" + item.border_color.ToString() + "',";

                    if (item.sequence == 7)
                        labInvStatAval.InnerText = item.total.ToString(format_comma);
                    else if (item.sequence == 6)
                        labInvStatDam.InnerText = item.total.ToString(format_comma);
                }

                string html = @"
                    var ctx = document.getElementById('chartTempInvStatus').getContext('2d');
                    var myChart = new Chart(ctx, {
                        type: 'pie',
                        data: {
                            labels:[" + label_pie + @"],
                            datasets: [";
                html = html + "{backgroundColor: [" + color_pie + "],";
                html = html + "data: [" + data_pie + "]}";
                html = html + @"]
                            },
    options: {
        legend: {
                display: false,
            },
        plugins: {
            labels: {
                // render 'label', 'value', 'percentage', 'image' or custom function, default is 'percentage'
                render: 'percentage',

                // precision for percentage, default is 0
                precision: 2,

                // identifies whether or not labels of value 0 are displayed, default is false
                showZero: true,

                // font size, default is defaultFontSize
                fontSize: 12,

                // font color, can be color array for each data or function for dynamic color, default is defaultFontColor
                fontColor: '#fff',

                // font style, default is defaultFontStyle
                fontStyle: 'normal',


                // draw text shadows under labels, default is false
                textShadow: false,

                // text shadow intensity, default is 6
                shadowBlur: 10,

                // text shadow X offset, default is 3
                shadowOffsetX: -5,

                // text shadow Y offset, default is 3
                shadowOffsetY: 5,

                // text shadow color, default is 'rgba(0,0,0,0.3)'
                shadowColor: 'rgba(255,0,0,0.75)',

                // draw label in arc, default is false
                // bar chart ignores this
                //arc: true,

                // position to draw label, available value is 'default', 'border' and 'outside'
                // bar chart ignores this
                // default is 'default'
                position: 'default',

                // draw label even it's overlap, default is true
                // bar chart ignores this
                //overlap: true,

                // show the real calculated percentages from the values and don't apply the additional logic to fit the percentages to 100 in total, default is false
                showActualPercentages: true
            }
        },
        tooltips: {
			callbacks: {
				label: function(tooltipItem, data) {
					var allData = data.datasets[tooltipItem.datasetIndex].data;
					var tooltipLabel = data.labels[tooltipItem.index];
					var tooltipData = allData[tooltipItem.index];
					var total = 0;
					for (var i in allData) {
						total += allData[i];
					}
					var tooltipPercentage = (tooltipData / total) * 100 ;
					return tooltipLabel + ': ' + tooltipData + ' (' + tooltipPercentage.toFixed(2) + '%)';
				}
			}
		}
	}
                        });";

                Page.ScriptPageRegister("$('document').ready(function () { " + html + " });", "chartTempInvStatus_pieChartInvent");

                //chartInvStatus.Text = html;

                #endregion


                BindChartItemMovement(_model, wh_master_id);
                BindChartLocationMovement(_model, wh_master_id);
                #endregion

                #region Card Group 3

                //  พี่หนุ่มให้เอา stg คิดด้วย 20/06/2022
                var query_exp = (from rows in _model.v_wms_inventory_data_for_dashboard_exp
                             where rows.quantity > 0
                                   && rows.inv_status == "Approved"
                                   && (wh_master_id != Guid.Empty
                                       ? rows.wh_master_id == wh_master_id
                                       : _model.t_wms_wh_user.Any(us =>
                                             us.user_id == _SessionVals.UserName &&
                                             us.wh_master_id == rows.wh_master_id))
                             select new
                             {
                                 rows.quantity,
                                 rows.exp_date
                             }).ToList(); // ดึงมาเป็น List เพื่อทำงานใน Memory

                // คำนวณ DiffDays บน Memory
                var expGroups = query_exp.Select(x => new
                {
                    Days = (x.exp_date.HasValue ? (x.exp_date.Value.Date - dateNow.Date).Days : int.MaxValue),
                    x.quantity
                });

                // รวมยอดตามช่วงวัน
                var expNear60 = expGroups.Where(x => x.Days > 30 && x.Days <= 60).Sum(x => (decimal?)x.quantity) ?? 0;
                var expNear30 = expGroups.Where(x => x.Days > 0 && x.Days <= 30).Sum(x => (decimal?)x.quantity) ?? 0;
                var exp = expGroups.Where(x => x.Days <= 0).Sum(x => (decimal?)x.quantity) ?? 0;

                // แสดงผล
                labExp60.InnerText = expNear60.ToString(format_comma);
                labExp30.InnerText = expNear30.ToString(format_comma);
                labExp.InnerText = exp.ToString(format_comma);

                #endregion
            }
            catch (Exception ex)
            {
                Logging = new Prototype.Providers.Logging(this, ex);
                RaiseLogging();
            }
            finally
            {
                _model.Dispose();
            }
        }


        void BindChartTransac(Source.WMSEntities _model, Guid wh_master_id)
        {
            try
            {
                if (_model == null)
                    _model = new Source.WMSEntities();

                var dateNow = DateTime.Now.Date;
                var dateBefore = dateNow.AddDays(-6);


                var result_inb = (from rows in _model.v_wms_dashb_center_period_inbound
                                  let create_date = DbFunctions.TruncateTime(rows.trans_date)
                                  where create_date >= dateBefore && (wh_master_id != Guid.Empty ? rows.wh_master_id == wh_master_id : _model.t_wms_wh_user.Any(us => us.user_id == _SessionVals.UserName && us.wh_master_id == rows.wh_master_id))
                                  group rows by new
                                  {
                                      create_date = create_date
                                  } into grb
                                  select new ChartTransac
                                  {
                                      date = grb.Key.create_date,
                                      qty = grb.Sum(sm => sm.quantity) ?? 0
                                  }).ToList();


                var result_outb = (from rows in _model.v_wms_dashb_center_period_outbound
                                   let create_date = DbFunctions.TruncateTime(rows.trans_date)
                                   where create_date >= dateBefore && (wh_master_id != Guid.Empty ? rows.wh_master_id == wh_master_id : _model.t_wms_wh_user.Any(us => us.user_id == _SessionVals.UserName && us.wh_master_id == rows.wh_master_id))
                                   group rows by new
                                   {
                                       create_date = create_date
                                   } into grb
                                   select new ChartTransac
                                   {
                                       date = grb.Key.create_date,
                                       qty = grb.Sum(sm => sm.quantity) ?? 0
                                   }).ToList();


                var display_days = "";
                var display_val_inb = "";
                var display_val_outb = "";

                while (dateBefore <= dateNow)
                {
                    var dto_outb = result_outb.FirstOrDefault(x => x.date == dateBefore);
                    if (dto_outb == null)
                    {
                        dto_outb = new ChartTransac();
                        dto_outb.date = dateBefore;
                        dto_outb.qty = 0;

                        result_outb.Add(dto_outb);
                    }

                    var dto_inb = result_inb.FirstOrDefault(x => x.date == dateBefore);
                    if (dto_inb == null)
                    {
                        dto_inb = new ChartTransac();
                        dto_inb.date = dateBefore;
                        dto_inb.qty = 0;

                        result_inb.Add(dto_inb);
                    }

                    display_val_inb += dto_inb.qty.ToString() + ",";
                    display_val_outb += dto_outb.qty.ToString() + ",";

                    display_days += "'" + dateBefore.ToStringExt() + "',";

                    dateBefore = dateBefore.AddDays(1);
                }

                display_val_inb = display_val_inb.TrimEnd(',');
                display_val_outb = display_val_outb.TrimEnd(',');

                display_days = display_days.TrimEnd(',');

                var html = @"
                                var ctx2 = document.getElementById('chartTempInOut').getContext('2d');
                                  var myChart = new Chart(ctx2, {
			                                type: 'line',
			                                data: {
				                                labels: [CHART_FORMAT_DAYS],
				                                datasets: [{
					                                label: 'INBOUND RECEIVE',
					                                backgroundColor: '#2883c0',
					                                borderColor: '#2883c0',
					                                fill: false,
					                                data: [CHART_FORMAT_VALUE_INB],
				                                }, {
					                                label: 'OUTBOUND PICK',
					                                backgroundColor: '#e2283d',
					                                borderColor: '#e2283d',
					                                fill: false,
					                                data: [CHART_FORMAT_VALUE_OUTB],
				                                }]
			                                },
			                                options: {
				                                responsive: true,
				                                title: {
					                                display: false,
					                                text: 'Sample Chart'
				                                },
				                                tooltips: {
					                                mode: 'index',
					                                intersect: false,
				                                },
				                                hover: {
					                                mode: 'nearest',
					                                intersect: true
				                                },
                                                plugins: {
                                                        labels: {
                                                            render: function (args) {
              return isNaN(args.percentage) ? '0 %' : args.percentage + '%';
            },
                                                   },
                                                },
				                                scales: {
					                                xAxes: [{
						                                display: true,
						                                scaleLabel: {
							                                display: false,
							                                labelString: 'DAYS'
						                                },
                                                        ticks: {
                                                                 beginAtZero: true,
                                                                 fontColor: 'CHART_TEXT_COL', 
                                                               },
					                                }],
					                                yAxes: [{
						                                display: true,
						                                scaleLabel: {
							                                display: false,
							                                labelString: 'VALUE'
						                                },
                                                        ticks: {
                                                                 beginAtZero: true,
                                                                 fontColor: 'CHART_TEXT_COL', 
                                                               },
					                                }]
				                                },               
                                                legend: {
                                                            labels: {
                                                                // This more specific font property overrides the global property
                                                                fontColor: 'CHART_TEXT_COL'
                                                            }
                                                },
			                                },
		                                });";

                var html_render = html.Replace("CHART_FORMAT_DAYS", display_days)
                                      .Replace("CHART_FORMAT_VALUE_INB", display_val_inb)
                                      .Replace("CHART_FORMAT_VALUE_OUTB", display_val_outb)
                                      .Replace("CHART_TEXT_COL", ThemeCurrent.ToUpper() == "LIGHT" ? "#353535" : "#FFF");

                Page.ScriptPageRegister("$('document').ready(function () { " + html_render + " });", "chartTempInOut_lineChartInOutbound");


                if (Page.IsPostBack)
                    updateChartTempInOut.Update();

            }
            catch (Exception ex)
            {
                Logging = new Prototype.Providers.Logging(this, ex);
                RaiseLogging();
            }
        }


        void BindChartItemMovement(Source.WMSEntities _model, Guid wh_master_id)
        {
            try
            {
                if (_model == null)
                    _model = new Source.WMSEntities();

                var ListResult = new List<usp_wms_dashboard_item_movement_viewer_Result>();

                if (wh_master_id == Guid.Empty)
                    ListResult = _model.usp_wms_dashboard_item_movement_viewer(null).ToList();
                else
                    ListResult = _model.usp_wms_dashboard_item_movement_viewer(wh_master_id).ToList();

                var display_val = "";
                var display_item2 = "";
                var display_item1 = "";

                List<string> _listNameItem = new List<string>();

                foreach (var item in ListResult.OrderByDescending(o=>o.c_count).OrderBy(o=>o.movement_type))
                {
                    if (!_listNameItem.Contains(item.item_number))
                    {
                        _listNameItem.Add(item.item_number);
                        display_val += "'" + item.item_number + "',";

                        if (item.movement_type == "INBOUND")
                        {
                            display_item1 += "'" + item.c_count.ToString("##,##0.00") + "',";
                            var GetRultItemDup = ListResult.Where(w => w.item_number == item.item_number && w.movement_type == "OUTBOUND").FirstOrDefault();
                            if (GetRultItemDup != null)
                                display_item2 += "'" + GetRultItemDup.c_count.ToString("##,##0.00") + "',";
                            else
                                display_item2 += "'" + 0.ToString("##,##0.00") + "',";
                        }
                        else
                        {
                            display_item2 += "'" + item.c_count.ToString("##,##0.00") + "',";

                            var GetRultItemDup = ListResult.Where(w => w.item_number == item.item_number && w.movement_type == "INBOUND").FirstOrDefault();
                            if (GetRultItemDup != null)
                                display_item1 += "'" + GetRultItemDup.c_count.ToString("##,##0.00") + "',";
                            else
                                display_item1 += "'" + 0.ToString("##,##0.00") + "',";
                        }
                    }
                }
                //display_item1 += "'0',";

                display_val = display_val.TrimEnd(',');
                display_item1 = display_item1.TrimEnd(',');
                display_item2 = display_item2.TrimEnd(',');

                var html = @"
                                var ctx2 = document.getElementById('chartItemMovement').getContext('2d');
                                  var myChart = new Chart(ctx2, {
			                                type: 'line',
			                                data: {
				                                labels: [CHART_FORMAT_DAYS],
				                                datasets: [{
					                                label: 'INBOUND ITEM',
					                                backgroundColor: '#2883c0',
					                                borderColor: '#2883c0',
					                                fill: false,
					                                data: [CHART_FORMAT_VALUE_INB],
				                                }, {
					                                label: 'OUTBOUND ITEM',
					                                backgroundColor: '#e2283d',
					                                borderColor: '#e2283d',
					                                fill: false,
					                                data: [CHART_FORMAT_VALUE_OUTB],
				                                }]
			                                },
			                                options: {
				                                responsive: true,
				                                title: {
					                                display: false,
					                                text: 'Sample Chart'
				                                },
				                                tooltips: {
					                                mode: 'index',
					                                intersect: false,
				                                },
				                                hover: {
					                                mode: 'nearest',
					                                intersect: true
				                                },
            //                                    plugins: {
            //                                            labels: {
            //                                                render: function (args) {
            //  return isNaN(args.percentage) ? '0 %' : args.percentage + '%';
            //},
            //                                       },
            //                                    },
				                                scales: {
					                                xAxes: [{
						                                display: true,
						                                scaleLabel: {
							                                display: false,
							                                labelString: 'Item'
						                                },
                                                        ticks: {
                                                                 beginAtZero: true,
                                                                 fontColor: 'CHART_TEXT_COL', 
                                                               },
					                                }],
					                                yAxes: [{
						                                display: true,
						                                scaleLabel: {
							                                display: false,
							                                labelString: 'VALUE'
						                                },
                                                        ticks: {
                                                                 beginAtZero: true,
                                                                 fontColor: 'CHART_TEXT_COL', 
                                                               },
					                                }]
				                                },               
                                                legend: {
                                                            labels: {
                                                                // This more specific font property overrides the global property
                                                                fontColor: 'CHART_TEXT_COL'
                                                            }
                                                },
			                                },
		                                });";

                var html_render = html.Replace("CHART_FORMAT_DAYS", display_val)
                                      .Replace("CHART_FORMAT_VALUE_INB", display_item1)
                                      .Replace("CHART_FORMAT_VALUE_OUTB", display_item2)
                                      .Replace("CHART_TEXT_COL", ThemeCurrent.ToUpper() == "LIGHT" ? "#353535" : "#FFF");

                Page.ScriptPageRegister("$('document').ready(function () { " + html_render + " });", "chartTempItemMovement");


                if (Page.IsPostBack)
                    updateChartItemMovement.Update();

            }
            catch (Exception ex)
            {
                Logging = new Prototype.Providers.Logging(this, ex);
                RaiseLogging();
            }
        }

        void BindChartLocationMovement(Source.WMSEntities _model, Guid wh_master_id)
        {
            try
            {
                if (_model == null)
                    _model = new Source.WMSEntities();

                var ListResult = new List<usp_wms_dashboard_location_movement_viewer_Result>();

                if (wh_master_id == Guid.Empty)
                    ListResult = _model.usp_wms_dashboard_location_movement_viewer(null).ToList();
                else
                    ListResult = _model.usp_wms_dashboard_location_movement_viewer(wh_master_id).ToList();

                var display_val = "";
                var display_item1 = "";

                foreach (var item in ListResult.OrderByDescending(o => o.c_count))
                {
                    display_val += "'" + item.location + "',";
                    display_item1 += "'" + (item.c_count ?? 0).ToString("##,##0.00") + "',";
                }

                display_val = display_val.TrimEnd(',');
                display_item1 = display_item1.TrimEnd(','); 

                var html = @"
                                var ctx2 = document.getElementById('chartLocationMovement').getContext('2d');
                                  var myChart = new Chart(ctx2, {
			                                type: 'line',
			                                data: {
				                                labels: [CHART_FORMAT_DAYS],
				                                datasets: [{
					                                label: 'Location',
					                                backgroundColor: '#13b34e',
					                                borderColor: '#13b34e',
					                                fill: false,
					                                data: [CHART_FORMAT_VALUE_INB],
				                                } ]
			                                },
			                                options: {
				                                responsive: true,
				                                title: {
					                                display: false,
					                                text: 'Sample Chart'
				                                },
				                                tooltips: {
					                                mode: 'index',
					                                intersect: false,
				                                },
				                                hover: {
					                                mode: 'nearest',
					                                intersect: true
				                                },
            //                                    plugins: {
            //                                            labels: {
            //                                                render: function (args) {
            //  return isNaN(args.percentage) ? '0 %' : args.percentage + '%';
            //},
            //                                       },
            //                                    },
				                                scales: {
					                                xAxes: [{
						                                display: true,
						                                scaleLabel: {
							                                display: false,
							                                labelString: 'Item'
						                                },
                                                        ticks: {
                                                                 beginAtZero: true,
                                                                 fontColor: 'CHART_TEXT_COL', 
                                                               },
					                                }],
					                                yAxes: [{
						                                display: true,
						                                scaleLabel: {
							                                display: false,
							                                labelString: 'VALUE'
						                                },
                                                        ticks: {
                                                                 beginAtZero: true,
                                                                 fontColor: 'CHART_TEXT_COL', 
                                                               },
					                                }]
				                                },               
                                                legend: {
                                                            labels: {
                                                                // This more specific font property overrides the global property
                                                                fontColor: 'CHART_TEXT_COL'
                                                            }
                                                },
			                                },
		                                });";

                var html_render = html.Replace("CHART_FORMAT_DAYS", display_val)
                                      .Replace("CHART_FORMAT_VALUE_INB", display_item1)
                                     // .Replace("CHART_FORMAT_VALUE_OUTB", display_item2)
                                      .Replace("CHART_TEXT_COL", ThemeCurrent.ToUpper() == "LIGHT" ? "#353535" : "#FFF");

                Page.ScriptPageRegister("$('document').ready(function () { " + html_render + " });", "chartTempLocationMovement");


                if (Page.IsPostBack)
                    updateChartLocationMovement.Update();

            }
            catch (Exception ex)
            {
                Logging = new Prototype.Providers.Logging(this, ex);
                RaiseLogging();
            }
        }

        public List<DTOChartColor> ChartInvStatusColor()
        {
            var _list = new List<DTOChartColor>();
            _list.Add(new DTOChartColor() { sequence = 1, status = "Available", border_color = "#2883c0", background_color = "#2883c0" });
            _list.Add(new DTOChartColor() { sequence = 2, status = "Damage", border_color = "#e2283d", background_color = "#e2283d" });
            _list.Add(new DTOChartColor() { sequence = 3, status = "Hold", border_color = "#df8a2a", background_color = "#df8a2a" });
            _list.Add(new DTOChartColor() { sequence = 4, status = "Other", border_color = "#13b34e", background_color = "#13b34e" });
            _list.Add(new DTOChartColor() { sequence = 5, status = "Other 2", border_color = "#1DAFA7", background_color = "#1DAFA7" });
            _list.Add(new DTOChartColor() { sequence = 6, status = "Quarantine", border_color = "#e2283d", background_color = "#e2283d" });
            _list.Add(new DTOChartColor() { sequence = 7, status = "Approved", border_color = "#089a3e", background_color = "#089a3e" });

            return _list;
        }


        public class ChartTransac
        {
            public double qty { get; set; }
            public DateTime? date { get; set; }
        }

        public class DTOChartColor
        {
            public int sequence { get; set; }
            public string status { get; set; }
            public string background_color { get; set; }
            public string border_color { get; set; }
        }
    }
}