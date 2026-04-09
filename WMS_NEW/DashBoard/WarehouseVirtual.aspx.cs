using System;
using System.Linq;
using System.Text;
using System.Web.UI;

namespace WMS_NEW.DashBoard
{
    public partial class WarehouseVirtual : PageCustom
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            try
            {

                if (!Page.IsPostBack)
                {

                    ddlWareHouse.MethodQueryProperty = delegate () { return Access.MasterData.Warehouse.Instance.GetQuery_User(); };
                    ddlWareHouse.BindDataSource();

                    ddlLocationLevel.MethodQueryProperty = delegate () { return Access.MasterData.Location.Instance.GetQueryPropertyLocationLevel(); };
                    ddlLocationLevel.BindDataSource();

                }
            }
            catch (Exception ex)
            {
                Logging = new Prototype.Providers.Logging(this, ex);
                RaiseLogging();
            }



        }

       

        protected void Page_PreRender(object sender, EventArgs e)
        {
            try
            {
                var eventControl = Request.Params.Get("__EVENTTARGET");
                var eventArgument = Request.Params.Get("__EVENTARGUMENT");
                if (eventControl == "view_capacity" && eventArgument != string.Empty)
                {
                    //Show Detail capacity
                    popDetail.ShowDialog();
                    string loc_id = eventArgument.ToString();
                    Guid location_id = Guid.Parse(loc_id);
                    ucWarehouseVirtualDetail.InitForm(location_id);
                }
            }
            catch (Exception ex)
            {
                Logging = new Prototype.Providers.Logging(this, ex);
                RaiseLogging();
            }
        }

        #region Event Control
        protected void btSearch_Click(object sender, EventArgs e)
        {
            try
            {
                hdf_wh_master_id.SetValue(ddlWareHouse.GetValue());
                hdf_location_level.SetValue(ddlLocationLevel.GetValue());
                GridExtItem.Search();
                Visual(ddlWareHouse.GetValue(), ddlLocationLevel.GetValue());
            }
            catch (Exception ex)
            {
                Logging = new Prototype.Providers.Logging(this, ex);
                RaiseLogging();
            }
        }


        protected void btClear_Click(object sender, EventArgs e)
        {
            try
            {
                var listCus = pnFilter.Controls.OfType<_UControls._IInputControl>();
                foreach (var item in listCus)
                {
                    item.Clear();
                    item.Update();
                }
            }
            catch (Exception ex)
            {
                Logging = new Prototype.Providers.Logging(this, ex);
                RaiseLogging();
            }
        }


        protected void btToggle_Click(object sender, EventArgs e)
        {
            try
            {
                if (btToggle.Text.StartsWith("Hide"))
                    btToggle.Text = btToggle.Text.Replace("Hide", "Show");
                else
                    btToggle.Text = btToggle.Text.Replace("Show", "Hide");

                btSearch.Visible = !btSearch.Visible;
                btClear.Visible = !btClear.Visible;

                pnFilter.Visible = !pnFilter.Visible;
                updateFilter.Update();
            }
            catch (Exception ex)
            {
                Logging = new Prototype.Providers.Logging(this, ex);
                RaiseLogging();
            }
        }
        #endregion

        #region Function
        void Set_Grid(_UControls.GridViewExt gridEx)
        {
            gridEx.AddFilterInputInclude(ddlWareHouse);
            gridEx.AddFilterInputInclude(ddlLocationLevel);
        }

        //void Visual(Guid _wh_master_id, string _location_level)
        //{
        //    using (var acc = new Access.MasterData.Warehouse())
        //    {
        //        var ent = acc.GetByKeyID(_wh_master_id);
        //        if (ent != null)
        //        {
        //            var listlocation = acc._Model.v_wms_dashboard_warehouse_virtual.Where(w => w.wh_master_id == _wh_master_id && w.location_level == _location_level);

        //            StringBuilder sb = new StringBuilder();

        //            for (int y = 0; y < ent.y_max; y++)
        //            {
        //                double width = 6.5;
        //                double width_max = width * ent.x_max ?? 0;
        //                sb.AppendLine("<div class=\"row\" style=\"width:" + width_max.ToString() + "rem\">");
        //                for (int x = 0; x < ent.x_max; x++)
        //                {
        //                    string class_hightlight = string.Empty;
        //                    string class_space = string.Empty;
        //                    string location = (y + 1).ToString() + "-" + (x + 1).ToString();
        //                    var entLocation = listlocation.Where(w => w.y_cordinate == (y + 1) && w.x_cordinate == (x + 1)).FirstOrDefault();
        //                    if (entLocation != null)
        //                    {
        //                        //string param = "WarehouseVirtual.aspx?location_id=" + entLocation.location_id;
        //                        string param = entLocation.location_id.ToString();
        //                        string capacity_qty = entLocation.capacity_qty == null ? string.Empty : entLocation.capacity_qty.ToString();
        //                        string current_qty = entLocation.current_qty == null ? "0" : entLocation.current_qty.ToString();
        //                        location = "<a class='title_tip' href=\"javascript:doPostBackAsync('view_capacity','" + param + "');\" title=\"" + entLocation.location + " (" + capacity_qty + ") \">" + entLocation.location + "</a><div class=\"text-value-lg\">" + current_qty + "</div>";
        //                        class_hightlight = " highlight";
        //                    }


        //                    if (x > 0 && x % 2 == 0)
        //                        class_space = " space";


        //                    sb.AppendLine("<div class=\"location" + class_space + class_hightlight + "\" style=\"width:" + width.ToString() + "rem\">" + location + "</div>");

        //                }
        //                sb.AppendLine("</div>");
        //            }

        //            txtVisual.Text = sb.ToString();
        //            updateVirtual.Update();
        //        }
        //    }

        //}

        void Visual(Guid _wh_master_id, string _location_level)
        {
            using (var acc = new Access.MasterData.Warehouse())
            {
                var ent = acc.GetByKeyID(_wh_master_id);
                if (ent != null)
                {
                    var listlocation = acc._Model.v_wms_dashboard_warehouse_virtual.Where(w => w.wh_master_id == _wh_master_id && w.location_level == _location_level);

                    StringBuilder sb = new StringBuilder();

                    for (int y = 0; y < ent.y_max; y++)
                    {
                        double width = 6.5;
                        double width_max = width * ent.x_max ?? 0;
                        sb.AppendLine("<div class=\"row\" style=\"width:" + width_max.ToString() + "rem\">");
                        for (int x = 0; x < ent.x_max; x++)
                        {
                            string class_hightlight = string.Empty;
                            string class_space = string.Empty;
                            string location = (y + 1).ToString() + "-" + (x + 1).ToString();
                            var entLocation = listlocation.Where(w => w.y_cordinate == (y + 1) && w.x_cordinate == (x + 1)).FirstOrDefault();
                            string param = string.Empty;
                            string current_qty = string.Empty;
                            string capacity_qty = string.Empty;
                            string locationName = string.Empty;

                            if (entLocation != null)
                            {
                                locationName = entLocation.location;
                                //string param = "WarehouseVirtual.aspx?location_id=" + entLocation.location_id;
                                param = entLocation.location_id.ToString();
                                capacity_qty = entLocation.capacity_qty == null ? string.Empty : entLocation.capacity_qty.ToString();
                                current_qty = entLocation.current_qty == null ? "0" : entLocation.current_qty.ToString();
                                if (entLocation.current_qty > 0)
                                    class_hightlight = " highlight2";
                            }


                            if (x > 0 && x % 2 == 0)
                                class_space = " space";

                            //sb.AppendLine("<a class=\"location" + class_space + class_hightlight + "\" style=\"width:" + width.ToString() + "rem\" href=\"javascript:doPostBackAsync('view_capacity','" + param + "');>" + location + "<div class=\"text-value-lg\">" + current_qty + "</div></a>");
                            location = "<span class='title_tip' \" title=\"" + locationName + " (" + capacity_qty + ") \">" + locationName + "<div class=\"text-value-lg\">" + current_qty + "</div></span>";
                            if (!string.IsNullOrEmpty(class_hightlight))
                            {
                                sb.AppendLine("<div onclick=\"javascript:doPostBackAsync('view_capacity','" + param + "');\" class=\"location" + class_space + class_hightlight + "\" style=\"width:" + width.ToString() + "rem\">" + location + "</div>");
                            }
                            else
                            {
                                sb.AppendLine("<div class=\"location" + class_space + class_hightlight + "\" style=\"width:" + width.ToString() + "rem\">" + location + "</div>");
                            }

                        }
                        sb.AppendLine("</div>");
                    }

                    txtVisual.Text = sb.ToString();
                    updateVirtual.Update();
                }
            }

        }

        #endregion

        protected void btnNew_Click(object sender, EventArgs e)
        {

            //popDetail.ShowDialog();
            //Guid location_id = new Guid("4DB8E359-C273-4E15-AD7B-358A11A4474F");
            //ucWarehouseVirtualDetail.InitForm(location_id);

        }
    }
}