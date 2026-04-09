using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using WMS_NEW.Administrator.AscxControls.InterfaceMonitor;

namespace WMS_NEW.Administrator
{
    public partial class InterfaceMonitor : PageCustom
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            try
            {
                #region Event Control


                GridExtInbound.GridRowClick += GridExtInbound_GridRowClick;
                GridExtOutbound.GridRowClick += GridExtOutbound_GridRowClick;
                GridExtReceipt.GridRowClick += GridExtReceipt_GridRowClick;
                GridExtShip.GridRowClick += GridExtShip_GridRowClick;
                GridExtM3Receipt.GridRowClick += GridExtM3Receipt_GridRowClick;
                GridExtM3ShipNotPO.GridRowClick += GridExtM3ShipNotPO_GridRowClick;
                GridExtM3ShipPO.GridRowClick += GridExtM3ShipPO_GridRowClick;
                PanelTab1.TabIndexChanged += PanelTab1_TabIndexChanged;

                #endregion

                #region Initial Data

                ddlProcessingStatus.MethodQueryProperty = delegate () { return Access.MasterData.ComboBoxItem.Instance.GetQueryProperty("processing_status"); };
                ddlWarehouse.MethodQueryProperty = delegate () { return Access.MasterData.Warehouse.Instance.GetQueryCode(_SessionVals.UserName); };
                ddlOwner.MethodQueryProperty = delegate () { return Access.MasterData.Owner.Instance.GetQueryCode(_SessionVals.UserName); };
                ddlOrderType.MethodQueryProperty = delegate () { return Access.MasterData.ComboBoxItem.Instance.GetQueryProperty("inbound_order_type"); };
                ddlOrderStatus.MethodQueryProperty = delegate () { return Access.MasterData.ComboBoxItem.Instance.GetQueryProperty("inbound_order_status"); };
                ddlSupplier.MethodQueryProperty = delegate () { return Access.MasterData.Supplier.Instance.GetQueryCode(); };
                ddlCustomer.MethodQueryProperty = delegate () { return Access.MasterData.Customer.Instance.GetQueryCode(); };

                #endregion

                if (!Page.IsPostBack)
                {
                    #region Bind Data Source

                    ddlProcessingStatus.BindDataSource();
                    ddlWarehouse.BindDataSource();
                    ddlOwner.BindDataSource();
                    ddlOrderType.BindDataSource();
                    ddlOrderStatus.BindDataSource();
                    ddlSupplier.BindDataSource();
                    ddlCustomer.BindDataSource();

                    #endregion

                    SearchByTabActive(false);

                }
            }
            catch (Exception ex)
            {
                Logging = new Prototype.Providers.Logging(this, ex);
                RaiseLogging();
            }
        }

        private void GridExtM3ShipNotPO_GridRowClick(object _rowKeyValue)
        {
            try
            {
                popShipNotPODetail.ShowDialog();
                ucShipNotPODetail.BindParameterPage((string)_rowKeyValue);
            }
            catch (Exception ex)
            {
                Logging = new Prototype.Providers.Logging(this, ex);
                RaiseLogging();
            }
        }

        private void GridExtM3ShipPO_GridRowClick(object _rowKeyValue)
        {
            try
            {
                popShipPODetail.ShowDialog();
                ucShipPODetail.BindParameterPage((string)_rowKeyValue);
            }
            catch (Exception ex)
            {
                Logging = new Prototype.Providers.Logging(this, ex);
                RaiseLogging();
            }
        }

        private void GridExtM3Receipt_GridRowClick(object _rowKeyValue)
        {
            try
            {
                popM3ReceiptDetail.ShowDialog();
                ucM3ReceiptDetail.BindParameterPage((string)_rowKeyValue);
            }
            catch (Exception ex)
            {
                Logging = new Prototype.Providers.Logging(this, ex);
                RaiseLogging();
            }
        }

        void GridExtShip_GridRowClick(object _rowKeyValue)
        {
            try
            {
                popShipDetail.ShowDialog();
                ucShipDetail.BindParameterPage((string)_rowKeyValue);
            }
            catch (Exception ex)
            {
                Logging = new Prototype.Providers.Logging(this, ex);
                RaiseLogging();
            }
        }

        void GridExtReceipt_GridRowClick(object _rowKeyValue)
        {
            try
            {
                popReceiptDetail.ShowDialog();
                ucReceiptDetail.BindParameterPage((string)_rowKeyValue);
            }
            catch (Exception ex)
            {
                Logging = new Prototype.Providers.Logging(this, ex);
                RaiseLogging();
            }
        }

        void GridExtOutbound_GridRowClick(object _rowKeyValue)
        {
            try
            {
                popOutboundDetail.ShowDialog();
                ucOutboundDetail.BindParameterPage((string)_rowKeyValue);
            }
            catch (Exception ex)
            {
                Logging = new Prototype.Providers.Logging(this, ex);
                RaiseLogging();
            }
        }

        void GridExtInbound_GridRowClick(object _rowKeyValue)
        {
            try
            {
                popInboundDetail.ShowDialog();
                ucInboundDetail.BindParameterPage((string)_rowKeyValue);
            }
            catch (Exception ex)
            {
                Logging = new Prototype.Providers.Logging(this, ex);
                RaiseLogging();
            }
        }

        void PanelTab1_TabIndexChanged(int _index)
        {
            try
            {
                switch (_index)
                {
                    case 1:
                        ddlOrderType.MethodQueryProperty = delegate () { return Access.MasterData.ComboBoxItem.Instance.GetQueryProperty("inbound_order_type"); };
                        ddlOrderType.BindDataSource();

                        ddlOrderStatus.MethodQueryProperty = delegate () { return Access.MasterData.ComboBoxItem.Instance.GetQueryProperty("inbound_order_status"); };
                        ddlOrderStatus.BindDataSource();

                        break;
                    case 2:
                        ddlOrderType.MethodQueryProperty = delegate () { return Access.MasterData.ComboBoxItem.Instance.GetQueryProperty("outbound_order_type"); };
                        ddlOrderType.BindDataSource();

                        ddlOrderStatus.MethodQueryProperty = delegate () { return Access.MasterData.ComboBoxItem.Instance.GetQueryProperty("outbound_order_status"); };
                        ddlOrderStatus.BindDataSource();
                        break;

                    case 3:
                        ddlOrderType.MethodQueryProperty = delegate () { return Access.MasterData.ComboBoxItem.Instance.GetQueryProperty("inbound_order_type"); };
                        ddlOrderType.BindDataSource();

                        ddlOrderStatus.MethodQueryProperty = delegate () { return Access.MasterData.ComboBoxItem.Instance.GetQueryProperty("inbound_order_status"); };
                        ddlOrderStatus.BindDataSource();
                        break;

                    case 4:
                        ddlOrderType.MethodQueryProperty = delegate () { return Access.MasterData.ComboBoxItem.Instance.GetQueryProperty("outbound_order_type"); };
                        ddlOrderType.BindDataSource();

                        ddlOrderStatus.MethodQueryProperty = delegate () { return Access.MasterData.ComboBoxItem.Instance.GetQueryProperty("outbound_order_status"); };
                        ddlOrderStatus.BindDataSource();
                        break;

                    default:
                        break;
                }
                SearchByTabActive(false);
            }
            catch (Exception ex)
            {
                Logging = new Prototype.Providers.Logging(this, ex);
                RaiseLogging();
            }
        }

        void Set_Grid(_UControls.GridViewExt gridEx)
        {
            gridEx.AddFilterInputInclude(ddlProcessingStatus);
            gridEx.AddFilterInputInclude(ddlWarehouse);
            gridEx.AddFilterInputInclude(ddlOwner);
            gridEx.AddFilterInputInclude(ddlOrderType);
            gridEx.AddFilterInputInclude(txtOrder);
            gridEx.AddFilterInputInclude(ddlOrderStatus);
            gridEx.AddFilterInputInclude(txtExpectDeliveryDate);
            gridEx.AddFilterInputInclude(ddlSupplier);
            gridEx.AddFilterInputInclude(ddlCustomer);
            gridEx.AddFilterInputInclude(txtCreateDate);
            gridEx.AddFilterInputInclude(txtCreateBy);

        }

        protected void btSearch_Click(object sender, EventArgs e)
        {
            try
            {
                SearchByTabActive(true);
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

                pnFilter.Visible = !pnFilter.Visible;
                updateFilter.Update();
            }
            catch (Exception ex)
            {
                Logging = new Prototype.Providers.Logging(this, ex);
                RaiseLogging();
            }
        }

        protected bool InitLoadTab1
        {
            get
            {
                if (ViewState["InitLoadData1"] == null)
                    ViewState["InitLoadData1"] = false;

                return (bool)ViewState["InitLoadData1"];
            }
            set
            {
                ViewState["InitLoadData1"] = value;
            }
        }
        protected bool InitLoadTab2
        {
            get
            {
                if (ViewState["InitLoadTab2"] == null)
                    ViewState["InitLoadTab2"] = false;

                return (bool)ViewState["InitLoadTab2"];
            }
            set
            {
                ViewState["InitLoadTab2"] = value;
            }
        }
        protected bool InitLoadTab3
        {
            get
            {
                if (ViewState["InitLoadTab3"] == null)
                    ViewState["InitLoadTab3"] = false;

                return (bool)ViewState["InitLoadTab3"];
            }
            set
            {
                ViewState["InitLoadTab3"] = value;
            }
        }
        protected bool InitLoadTab4
        {
            get
            {
                if (ViewState["InitLoadTab4"] == null)
                    ViewState["InitLoadTab4"] = false;

                return (bool)ViewState["InitLoadTab4"];
            }
            set
            {
                ViewState["InitLoadTab4"] = value;
            }
        }
        protected bool InitLoadTab5
        {
            get
            {
                if (ViewState["InitLoadTab5"] == null)
                    ViewState["InitLoadTab5"] = false;

                return (bool)ViewState["InitLoadTab5"];
            }
            set
            {
                ViewState["InitLoadTab5"] = value;
            }
        }
        protected bool InitLoadTab6
        {
            get
            {
                if (ViewState["InitLoadTab6"] == null)
                    ViewState["InitLoadTab6"] = false;

                return (bool)ViewState["InitLoadTab6"];
            }
            set
            {
                ViewState["InitLoadTab6"] = value;
            }
        }
        protected bool InitLoadTab7
        {
            get
            {
                if (ViewState["InitLoadTab7"] == null)
                    ViewState["InitLoadTab7"] = false;

                return (bool)ViewState["InitLoadTab7"];
            }
            set
            {
                ViewState["InitLoadTab7"] = value;
            }
        }
        protected bool InitLoadTab8
        {
            get
            {
                if (ViewState["InitLoadTab8"] == null)
                    ViewState["InitLoadTab8"] = false;

                return (bool)ViewState["InitLoadTab8"];
            }
            set
            {
                ViewState["InitLoadTab8"] = value;
            }
        }
        protected bool InitLoadTab9
        {
            get
            {
                if (ViewState["InitLoadTab9"] == null)
                    ViewState["InitLoadTab9"] = false;

                return (bool)ViewState["InitLoadTab9"];
            }
            set
            {
                ViewState["InitLoadTab9"] = value;
            }
        }
        void SearchByTabActive(bool _isManualSearch)
        {
            if (PanelTab1.TabIndexActive == 1 && ((_isManualSearch) || (!InitLoadTab1)))
            {

                InitLoadTab1 = true;

                Set_Grid(GridExtInbound);
                GridExtInbound.AddFilterCustomInputInclude(hid_active_load);
                GridExtInbound.Search();
            }
            else if (PanelTab1.TabIndexActive == 2 && ((_isManualSearch) || (!InitLoadTab2)))
            {
                InitLoadTab2 = true;

                Set_Grid(GridExtOutbound);
                GridExtOutbound.AddFilterCustomInputInclude(hid_active_load);
                GridExtOutbound.Search();
            }
            else if (PanelTab1.TabIndexActive == 3 && ((_isManualSearch) || (!InitLoadTab3)))
            {
                InitLoadTab3 = true;

                Set_Grid(GridExtReceipt);
                GridExtReceipt.AddFilterCustomInputInclude(hid_active_load);
                GridExtReceipt.Search();
            }
            else if (PanelTab1.TabIndexActive == 4 && ((_isManualSearch) || (!InitLoadTab4)))
            {
                InitLoadTab4 = true;

                Set_Grid(GridExtShip);
                GridExtShip.AddFilterCustomInputInclude(hid_active_load);
                GridExtShip.Search();
            }
            else if (PanelTab1.TabIndexActive == 5 && ((_isManualSearch) || (!InitLoadTab5)))
            {
                InitLoadTab5 = true;

                //Set_Grid(GridExtItemAndUOM);
                GridExtItemAndUOM.AddFilterCustomInputInclude(hid_active_load);
                GridExtItemAndUOM.Search();
            }
            else if (PanelTab1.TabIndexActive == 6 && ((_isManualSearch) || (!InitLoadTab6)))
            {
                InitLoadTab6 = true;

                //Set_Grid(GridExtCustomer);
                GridExtCustomer.AddFilterCustomInputInclude(hid_active_load);
                GridExtCustomer.Search();
            }
            else if (PanelTab1.TabIndexActive == 7 && ((_isManualSearch) || (!InitLoadTab7)))
            {
                InitLoadTab7 = true;

                //Set_Grid(GridExtM3Receipt);
                GridExtM3Receipt.AddFilterCustomInputInclude(hid_active_load);
                GridExtM3Receipt.Search();
            }
            else if (PanelTab1.TabIndexActive == 8 && ((_isManualSearch) || (!InitLoadTab8)))
            {
                InitLoadTab8 = true;

                //Set_Grid(GridExtM3ShipNotPO);
                GridExtM3ShipNotPO.AddFilterCustomInputInclude(hid_active_load);
                GridExtM3ShipNotPO.Search();
            }
            else if (PanelTab1.TabIndexActive == 9 && ((_isManualSearch) || (!InitLoadTab9)))
            {
                InitLoadTab9 = true;

                //Set_Grid(GridExtShip);
                GridExtM3ShipPO.AddFilterCustomInputInclude(hid_active_load);
                GridExtM3ShipPO.Search();
            }
        }
    }
}