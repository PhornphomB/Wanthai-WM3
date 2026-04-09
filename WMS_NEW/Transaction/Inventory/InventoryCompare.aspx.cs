using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace WMS_NEW.Transaction.Inventory
{
    public partial class InventoryCompare : PageCustom
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            ddlWareHouse.MethodQueryProperty = delegate () { return Access.MasterData.Warehouse.Instance.GetQuery_User(_SessionVals.UserName); };
            ddlOwner.MethodQueryProperty = delegate () { return Access.MasterData.Owner.Instance.GetQuery(_SessionVals.UserName); };
            ddlItemCategory.MethodQueryProperty = delegate () { return Access.MasterData.ItemCategory.Instance.GetQuery(); };      
            ddlItem.MethodQueryProperty = delegate () { return Access.MasterData.Item.Instance.GetQuery(); };

            gridItemOrder.GridRowClick += gridItemOrder_GridRowClick;
            gridItemInvent.GridRowClick += gridItemInvent_GridRowClick;
            gridItemInvent.GridRowAfterDataBound += GridItemInvent_GridRowAfterDataBound;

            if (!Page.IsPostBack)
            {
                hidSessionUserItOrder.SetValue(_SessionVals.UserName);
                hidSessionUserItInvent.SetValue(_SessionVals.UserName);

                ddlWareHouse.BindDataSource();
                ddlItemCategory.BindDataSource();           
                ddlOwner.BindDataSource();
                ddlItem.BindDataSource();
     
            }
        }

        void popupItemOrderDetail_CloseClick(object sender, EventArgs e)
        {
            try
            {
                hid_ord_wh_item_master_id.SetValue(null);
                gridItemOrderDetail.Search();
            }
            catch (Exception ex)
            {
                Logging = new Prototype.Providers.Logging(this, ex);
                RaiseLogging();
            }
        }
        void popupItemInventDetail_CloseClick(object sender, EventArgs e)
        {
            try
            {
                hid_inv_wh_item_master_id.SetValue(null);
                gridItemInventDetail.Search();
            }
            catch (Exception ex)
            {
                Logging = new Prototype.Providers.Logging(this, ex);
                RaiseLogging();
            }
        }

        void gridItemOrder_GridRowClick(object _rowKeyValue)
        {
            try
            {
                hid_ord_wh_item_master_id.SetValue(_rowKeyValue.ToString());
                gridItemOrderDetail.Search();

                popupItemOrderDetail.ShowDialog();
            }
            catch (Exception ex)
            {
                Logging = new Prototype.Providers.Logging(this, ex);
                RaiseLogging();
            }
        }

        void gridItemInvent_GridRowClick(object _rowKeyValue)
        {
            try
            {
                hid_inv_wh_item_master_id.SetValue(_rowKeyValue.ToString());
                gridItemInventDetail.Search();

                popupItemInventDetail.ShowDialog();
            }
            catch (Exception ex)
            {
                Logging = new Prototype.Providers.Logging(this, ex);
                RaiseLogging();
            }
        }

        private void GridItemInvent_GridRowAfterDataBound(GridViewRowEventArgs e)
        {
            try
            {
                if (e.Row.RowType == DataControlRowType.DataRow)
                {
                    var sumInv = (double?)DataBinder.Eval(e.Row.DataItem, "sumInv");
                    if (sumInv <= 0 )
                    {
                        e.Row.CssClass = "btn-danger";
                    }
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

        protected void btSearch_Click(object sender, EventArgs e)
        {
            try
            {
                gridItemOrder.AddFilterInputInclude(ddlWareHouse);
                gridItemOrder.AddFilterInputInclude(ddlItem);
                gridItemOrder.AddFilterInputInclude(ddlItemCategory);
                gridItemOrder.AddFilterInputInclude(ddlOwner);
                gridItemOrder.AddFilterInputInclude(txtDelPlanDate);
                gridItemOrder.Search();

                gridItemInvent.AddFilterInputInclude(ddlWareHouse);
                gridItemInvent.AddFilterInputInclude(ddlItem);
                gridItemInvent.AddFilterInputInclude(ddlItemCategory);
                gridItemInvent.AddFilterInputInclude(ddlOwner);
                gridItemInvent.Search();
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
    }
}