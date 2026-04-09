<%@ Page Title="" Language="C#" MasterPageFile="~/Layout.Master" AutoEventWireup="true" CodeBehind="OutManifestSimu.aspx.cs" Inherits="WMS_NEW.Transaction.OutbndManifest.OutManifestSimu" %>

<%@ Register Src="~/Report/AscxControls/ucReportViewer.ascx" TagPrefix="ucControls" TagName="ReportViewer" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
    <script type="text/javascript">

        function RadioCheck(rb) {
            var gv = document.getElementById("<%=gridTruck.ClientID%>");

            var rbs = gv.getElementsByTagName("input");
            var row = rb.parentNode.parentNode;

            $('#<%=gridTruck.ClientID%> tr').removeClass("highlight");
            $(row).addClass("highlight");

            for (var i = 0; i < rbs.length; i++) {

                if (rbs[i].type == "radio") {

                    if (rbs[i].checked && rbs[i] != rb) {
                        rbs[i].checked = false;
                        break;
                    }
                }
            }

            doPostBackAsync('_TRIG_CHOOSE_TRUCK', '');
        }

    </script>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">

    <ucControls:ReportViewer runat="server" ID="ReportViewer1" />

    <ucControls:PanelPopup ID="popupAddItem" runat="server" HeaderText="Select Items" StyleSize="Default">
        <DataTemplate>
            <ucControls:PanelControlRow ID="panelAddItem" runat="server" CssClass="row col-sm-12 col-md-12 col-lg-12">
                <ucControls:InputTextBox ID="txtWarehouse" runat="server" Enabled="false" LabelText="Warehouse ID" ResourceGroup="warehouse" ResourceName="wh_id" BaseContentCss="col-sm-12 col-md-6 col-lg-4" />
                <ucControls:InputTextBox ID="txtOwner" runat="server" Enabled="false" LabelText="Owner Code" ResourceGroup="owner" ResourceName="owner_code" BaseContentCss="col-sm-12 col-md-6 col-lg-4" />
                <ucControls:InputTextBox ID="txtOutboundNo" runat="server" Enabled="false" LabelText="Outbound Order No" ResourceGroup="outbound_master" ResourceName="outbound_order_number" BaseContentCss="col-sm-12 col-md-6 col-lg-4" />

                <ucControls:InputTextBox ID="txtOrderStatus" runat="server" Enabled="false" LabelText="Order Status" ResourceGroup="outbound_master" ResourceName="order_status" BaseContentCss="col-sm-12 col-md-6 col-lg-4" />
                <ucControls:InputTextBox ID="txtCusProvince" runat="server" Enabled="false" LabelText="Province" ResourceGroup="customer" ResourceName="province" BaseContentCss="col-sm-12 col-md-6 col-lg-4" />
                <ucControls:InputTextBox ID="txtCustomer" runat="server" Enabled="false" IsMultiLine="true" LabelText="Customer" BaseContentCss="col-sm-12 col-md-6 col-lg-4" ResourceGroup="customer" ResourceName="customer_code" />
            </ucControls:PanelControlRow>

            <ucControls:PanelControlRow ID="panelChooseItem" runat="server" CssClass="mt-1">
                <ucControls:GridExt ID="gridAddItems" runat="server"
                    SourceAssemblyName="WMS_NEW.Access" SourceClassName="WMS_NEW.Access.Transaction.OutbndManifest.ManifestTspOrderItemSrc" KeyField="KeyID" KeyType="Guid"
                    GridAllowSelectBox="true" AutoSize="true" DisableFirstSearch="true" DisableExport="true" GridSortDefault="line_number_int asc">
                    <CustomSearchTemplate>
                        <ucControls:InputHidden ID="hidOrderMasterId" runat="server" DataFieldValue="_outbound_order_master_id" />
                    </CustomSearchTemplate>
                    <CustomColumnTemplate>
                        <ucControls:GridColumnExt ID="GridColumnExt22" runat="server" HeaderText="Line Number" DataField="line_number" ResourceGroup="outbound_detail" ResourceName="line_number" />
                        <ucControls:GridColumnExt ID="GridColumnExt23" runat="server" HeaderText="Item Category" DataField="category_description" ResourceGroup="category" ResourceName="description" />
                        <ucControls:GridColumnExt ID="GridColumnExt25" runat="server" HeaderText="Item Description" DataField="item_description" ResourceGroup="item" ResourceName="description" />
                        <ucControls:GridColumnExt ID="GridColumnExt27" runat="server" HeaderText="Item No" DataField="item_number" ResourceGroup="item" ResourceName="item_number" />
                        <ucControls:GridColumnExt ID="GridColumnExt29" runat="server" HeaderText="Batch" DataField="lot_number" ResourceGroup="inventory" ResourceName="lot_number" />
                        <ucControls:GridColumnExt ID="GridColumnExt30" runat="server" HeaderText="LPN" DataField="lpn" ResourceGroup="inventory" ResourceName="lpn" />
                        <ucControls:GridColumnExt ID="GridColumnExt32" runat="server" HeaderText="Expiry Date" DataField="expiry_date" FormatType="Date" ResourceGroup="inventory" ResourceName="expiry_date" />
                        <ucControls:GridColumnExt ID="GridColumnExt35" runat="server" HeaderText="Order Qty" DataField="quantity_order" FormatType="Number" ResourceGroup="outbound_detail" ResourceName="quantity_order" />
                        <ucControls:GridColumnExt ID="GridColumnExt36" runat="server" HeaderText="Used Qty" DataField="quantity_tsp" FormatType="Number" ResourceGroup="outbound_detail" ResourceName="quantity_tsp" />
                        <ucControls:GridColumnExt ID="GridColumnExt37" runat="server" HeaderText="Avalible Qty" DataField="quantity_avalible" FormatType="Number" ResourceGroup="outbound_detail" ResourceName="quantity_tsp" />
                        <ucControls:GridColumnExt ID="GridColumnExt39" runat="server" HeaderText="UOM" DataField="uom" ResourceGroup="item_uom" ResourceName="uom" />

                        <ucControls:GridColumnExt ID="GridColumnExt45" runat="server" HeaderText="Create By" DataField="create_by" ResourceGroup="general" ResourceName="create_by" />
                        <ucControls:GridColumnExt ID="GridColumnExt46" runat="server" HeaderText="Create Date" DataField="create_date" Width="150" FormatType="DateTime" ResourceGroup="general" ResourceName="create_date" />
                    </CustomColumnTemplate>
                </ucControls:GridExt>
            </ucControls:PanelControlRow>
        </DataTemplate>
        <CommandTemplate>
            <ucControls:ButtonExt ID="btAddItemTsp" runat="server" Text="Add to Transport" CssClass="btn btn-success" OnClick="btAddItemTsp_Click" />
        </CommandTemplate>
    </ucControls:PanelPopup>

    <ucControls:PanelPopup ID="popupItemSplit" runat="server" HeaderText="Split Item Quantity" StyleSize="Small">
        <DataTemplate>
            <ucControls:PanelControlRow ID="panel1" runat="server" CssClass="row col-sm-12 col-md-12 col-lg-12">
                <ucControls:InputTextBox ID="txtSplitLineNumber" runat="server" Enabled="false" LabelText="Line Number" BaseContentCss="col-sm-12 col-md-12 col-lg-12" ResourceGroup="general" ResourceName="line_number" />
                <ucControls:InputTextBox ID="txtSplitItemNumber" runat="server" Enabled="false" LabelText="Item Number" BaseContentCss="col-sm-12 col-md-12 col-lg-12" ResourceGroup="item" ResourceName="item_number" />
                <ucControls:InputTextNumber ID="txtSplitQtyCurrent" runat="server" Enabled="false" NumberType="Double" LabelText="Current Qty" BaseContentCss="col-sm-12 col-md-12 col-lg-12" ResourceGroup="outbound_detail" ResourceName="quantity_current" />
                <ucControls:InputTextNumber ID="txtSplitQty" runat="server" IsPrimary="true" NumberType="Double" LabelText="Split Qty" BaseContentCss="col-sm-12 col-md-12 col-lg-12" ResourceGroup="general" ResourceName="quantity_split" />
            </ucControls:PanelControlRow>
        </DataTemplate>
        <CommandTemplate>
            <ucControls:ButtonExt ID="btSplitSave" runat="server" Text="Split" CssClass="btn btn-success" CausesValidation="true" OnClick="btSplitSave_Click" ResourceGroup="general" ResourceName="btn_split" />
        </CommandTemplate>
    </ucControls:PanelPopup>

    <ucControls:PanelPopup ID="popupManifestDet" runat="server" HeaderText="Manifest Detail">
        <DataTemplate>

            <ucControls:PanelControlRow ID="panel2" runat="server" CssClass="row col-sm-12 col-md-12 col-lg-12">
                <ucControls:InputTextBox ID="txtViewManifestId" runat="server" Enabled="false" LabelText="Manifest ID" BaseContentCss="col-sm-12 col-md-4 col-lg-4" ResourceGroup="outbound_manifest" ResourceName="manifest_code" />
                <ucControls:InputTextBox ID="txtViewTruckLicen" runat="server" Enabled="false" LabelText="Truck License" BaseContentCss="col-sm-12 col-md-4 col-lg-4" ResourceGroup="outbound_master" ResourceName="truck_no" />
                <ucControls:InputTextBox ID="txtViewTruckName" runat="server" Enabled="false" LabelText="Truck Name" BaseContentCss="col-sm-12 col-md-4 col-lg-4" ResourceGroup="outbound_master" ResourceName="truck_name" />
            </ucControls:PanelControlRow>

            <ucControls:PanelControlRow ID="panel3" runat="server" CssClass="mt-1">
                <ucControls:GridExt ID="gridTspAssDetail" runat="server"
                    SourceAssemblyName="WMS_NEW.Access" SourceClassName="WMS_NEW.Access.Transaction.OutbndManifest.ManifestTspAssignDetail" KeyField="KeyID" KeyType="Guid"
                    AutoSize="true" DisableFirstSearch="true" DisableExport="true" GridSortDefault="create_date desc">
                    <CustomSearchTemplate>
                        <ucControls:InputHidden ID="hidTspTruckId" runat="server" DataFieldValue="_tspTruckID" />
                    </CustomSearchTemplate>
                    <CustomColumnTemplate>
                        <ucControls:GridColumnExt ID="GridColumnExt8" runat="server" HeaderText="Province" DataField="province" AllowFilter="true" AllowSort="true" ShowFilterNow="true" ResourceGroup="customer" ResourceName="province" />
                        <ucControls:GridColumnExt ID="GridColumnExt49" runat="server" HeaderText="Order No" DataField="outbound_order_number" AllowFilter="true" AllowSort="true" ShowFilterNow="true" ResourceGroup="outbound_master" ResourceName="outbound_order_number" />
                        <ucControls:GridColumnExt ID="GridColumnExt50" runat="server" HeaderText="Line Number" DataField="line_number" AllowFilter="true" AllowSort="true" ResourceGroup="outbound_detail" ResourceName="line_number" />
                        <ucControls:GridColumnExt ID="GridColumnExt51" runat="server" HeaderText="Item No" DataField="item_number" AllowFilter="true" AllowSort="true" ShowFilterNow="true" ResourceGroup="item" ResourceName="item_number" />
                        <ucControls:GridColumnExt ID="GridColumnExt52" runat="server" HeaderText="Qty" DataField="item_quantity" FormatType="Number" AllowSort="true" ResourceGroup="outbound_detail" ResourceName="quantity_order" />
                        <ucControls:GridColumnExt ID="GridColumnExt53" runat="server" HeaderText="Volume" DataField="volume_per" FormatType="Number" AllowSort="true" ResourceGroup="outbound_manifest" ResourceName="volume_per" />
                        <ucControls:GridColumnExt ID="GridColumnExt54" runat="server" HeaderText="Volume Total" DataField="volume_total" FormatType="Number" AllowSort="true" ResourceGroup="outbound_manifest" ResourceName="volume_total" />
                        <ucControls:GridColumnExt ID="GridColumnExt55" runat="server" HeaderText="UOM" DataField="uom" AllowFilter="true" AllowSort="true" ResourceGroup="item_uom" ResourceName="uom" />
                        <ucControls:GridColumnExt ID="GridColumnExt56" runat="server" HeaderText="Create By" DataField="create_by" Width="130" ResourceGroup="general" ResourceName="create_by" />
                        <ucControls:GridColumnExt ID="GridColumnExt57" runat="server" HeaderText="Create Date" DataField="create_date" Width="130" FormatType="DateTime" ResourceGroup="general" ResourceName="create_date" />
                    </CustomColumnTemplate>
                </ucControls:GridExt>
            </ucControls:PanelControlRow>
        </DataTemplate>
    </ucControls:PanelPopup>

    <div class="col-lg-12 col-md-12 col-sm-12 pt-3 background-base">

        <ucControls:PanelTab ID="PanelTab1" runat="server">
            <ControlTemplate>

                <ucControls:PanelControlTab ID="panelOrders" runat="server" PanelName="Outbound Orders" ResourceGroup="outbound_manifest" ResourceName="tab_outbound_order">
                    <ucControls:GridExt ID="GridExt1" runat="server"
                        SourceAssemblyName="WMS_NEW.Access" SourceClassName="WMS_NEW.Access.Transaction.OutbndManifest.ManifestTspOrderSrc" KeyField="KeyID" KeyType="Guid"
                        GridAllowRowEdit="false" GridAllowRowDelete="false" AutoSize="true" GridSortDefault="create_date desc,outbound_order_number">
                        <CustomSearchTemplate>
                            <ucControls:InputHidden ID="hidSessionUser" runat="server" DataFieldValue="_userID" />
                        </CustomSearchTemplate>
                        <CustomColumnTemplate>
                            <ucControls:GridColumnExt ID="GridColumnExt24" runat="server" HeaderText="" DataField="cmd_item" ControlType="CommandButton" CommandText="Select" CommandName="ADD_ITEM" IsConfirm="false" />
                            <ucControls:GridColumnExt ID="GridColumnExt1" runat="server" HeaderText="Warehouse" DataField="wh_id" AllowFilter="true" AllowSort="true" Width="150" UseFilterDropDown="true" DropDownDisplayDefault="--All--" ResourceGroup="warehouse" ResourceName="wh_id" />
                            <ucControls:GridColumnExt ID="GridColumnExt2" runat="server" HeaderText="Owner" DataField="owner_code" DataFieldFilter="owner_id" AllowFilter="true" AllowSort="true" UseFilterDropDown="true" DropDownDisplayDefault="--All--" ResourceGroup="owner" ResourceName="owner_code" />
                            <ucControls:GridColumnExt ID="GridColumnExt19" runat="server" HeaderText="Province" DataField="province" AllowFilter="true" AllowSort="true" ShowFilterNow="true" ResourceGroup="customer" ResourceName="province" />
                            <ucControls:GridColumnExt ID="GridColumnExt47" runat="server" HeaderText="Route Code" DataField="route_code" AllowFilter="true" AllowSort="true" ShowFilterNow="true" ResourceGroup="customer" ResourceName="route_code" />
                            <ucControls:GridColumnExt ID="GridColumnExt4" runat="server" HeaderText="Order No" DataField="outbound_order_number" AllowFilter="true" AllowSort="true" ShowFilterNow="true" ResourceGroup="outbound_master" ResourceName="outbound_order_number" />
                            <ucControls:GridColumnExt ID="GridColumnExt6" runat="server" HeaderText="Total Item" DataField="item_count" FormatType="Integer" AllowFilter="true" AllowSort="true" ShowFilterNow="false" ResourceGroup="outbound_manifest" ResourceName="item_count" />
                            <ucControls:GridColumnExt ID="GridColumnExt17" runat="server" HeaderText="Total Qty" DataField="total_quantity" FormatType="Number" AllowFilter="true" AllowSort="true" ShowFilterNow="false" ResourceGroup="outbound_manifest" ResourceName="quantity_total" />
                            <ucControls:GridColumnExt ID="GridColumnExt3" runat="server" HeaderText="Order Type (Mvt)" DataField="order_type" AllowFilter="true" AllowSort="true" ShowFilterNow="true" UseFilterDropDown="true" DropDownDisplayDefault="--All--" ResourceGroup="outbound_master" ResourceName="order_type" />
                            <ucControls:GridColumnExt ID="GridColumnExt20" runat="server" HeaderText="Invoice No" DataField="customer_order_number" AllowFilter="true" AllowSort="true" ResourceGroup="outbound_master" ResourceName="customer_order_number" />
                            <ucControls:GridColumnExt ID="GridColumnExt5" runat="server" HeaderText="Status" DataField="order_status" AllowFilter="true" AllowSort="true" ShowFilterNow="true" UseFilterDropDown="true" DropDownDisplayDefault="--All--" ResourceGroup="outbound_master" ResourceName="order_status" />
                            <ucControls:GridColumnExt ID="GridColumnExt16" runat="server" HeaderText="Customer Code" DataField="customer_code" AllowFilter="true" AllowSort="true" ShowFilterNow="false" ResourceGroup="customer" ResourceName="customer_code" />
                            <ucControls:GridColumnExt ID="GridColumnExt9" runat="server" HeaderText="Customer Name" DataField="customer_name" DataFieldFilter="customer_id" Width="250" AllowFilter="true" AllowSort="true" ShowFilterNow="true" UseFilterDropDown="true" DropDownFilterType="LazySearch" DropDownDisplayDefault="--All--" ResourceGroup="customer" ResourceName="customer_name" />
                            <ucControls:GridColumnExt ID="GridColumnExt18" runat="server" HeaderText="Customer PO" DataField="customer_purchase_order" AllowFilter="true" AllowSort="true" ResourceGroup="outbound_master" ResourceName="customer_purchase_order" />
                            <ucControls:GridColumnExt ID="GridColumnExt7" runat="server" HeaderText="Order Date" DataField="order_date" Width="150" FormatType="DateTime" AllowFilter="true" AllowSort="true" ShowFilterNow="true" FilterFormatType="Date" ResourceGroup="outbound_master" ResourceName="order_date" />
                            <ucControls:GridColumnExt ID="GridColumnExt10" runat="server" HeaderText="Create By" DataField="create_by" Width="130" ResourceGroup="general" ResourceName="create_by" />
                            <ucControls:GridColumnExt ID="GridColumnExt11" runat="server" HeaderText="Create Date" DataField="create_date" Width="130" FormatType="DateTime" ResourceGroup="general" ResourceName="create_date" />
                        </CustomColumnTemplate>
                    </ucControls:GridExt>
                </ucControls:PanelControlTab>

                <ucControls:PanelControlTab ID="panelTspItems" runat="server" PanelName="Transport Data" ResourceGroup="outbound_manifest" ResourceName="tab_transport">
                    >

                <div class="row col-sm-12 ml-0 mr-0 pl-0 pr-0">

                    <asp:UpdatePanel ID="updateTspCalTruck" runat="server" UpdateMode="Conditional" ChildrenAsTriggers="true" class="col-lg-4 col-md-12 col-sm-12">
                        <ContentTemplate>

                            <div class="card card-accent-info">
                                <div class="card-header">
                                    <strong>Calculate Truck</strong>
                                </div>
                                <div class="card-block row">

                                    <div class="col-12 mb-2">
                                        <div style="float: left;">
                                            <span id="labTspItemDesc" runat="server" class="badge badge-success">Order Items : </span>
                                        </div>
                                        <div style="float: left; margin-left: 5px;">
                                            <span id="labTspItemVol" runat="server" class="badge badge-success">ปริมาตรที่ใช้ : </span>
                                        </div>
                                        <div style="clear: both;"></div>
                                    </div>

                                    <asp:Panel ID="panelCmdSimulate" runat="server">

                                        <asp:GridView ID="gridTruckSim" runat="server" AutoGenerateColumns="false" ShowHeaderWhenEmpty="true" DataKeyNames="truck_type_id"
                                            CssClass="table table-responsive-sm table-hover mb-0 gridview-custom" GridLines="None">
                                            <Columns>
                                                <asp:TemplateField HeaderText="เลือกรถ" HeaderStyle-HorizontalAlign="Center">
                                                    <ItemTemplate>
                                                        <div style="margin-top: 2px;">
                                                            <asp:Button ID="btChooseType" runat="server" CssClass="btn btn-success btn-block btn-ingrid" Text='<%#Eval("truck_type") %>'
                                                                CommandName="CHOOSE_TYPE" CommandArgument='<%#Eval("truck_type_id") %>' />
                                                            <asp:HiddenField ID="hidVolumePer" runat="server" Value='<%#Eval("volume_per") %>' />
                                                        </div>
                                                    </ItemTemplate>
                                                </asp:TemplateField>
                                                <asp:BoundField HeaderText="ปริมาตร/คัน" DataField="volume_per" DataFormatString="{0:0.##}" ItemStyle-HorizontalAlign="Center" />
                                                <asp:BoundField HeaderText="จำนวนรถที่ต้องใช้ (คัน)" DataField="truck_amt" DataFormatString="{0:0.#}" ItemStyle-HorizontalAlign="Center" />
                                            </Columns>
                                        </asp:GridView>

                                        <div style="margin-top: 10px;">
                                            <ucControls:ButtonExt ID="btCalTruckVolume" runat="server" Text="Simulate Volume" CssClass="btn btn-info btn-block"
                                                CausesValidation="false" OnClick="btCalTruckVolume_Click" ResourceGroup="general" ResourceName="btn_cal_truck_volume" />
                                        </div>

                                    </asp:Panel>

                                    <asp:Panel ID="panelCmdTspSave" runat="server">

                                        <div class="col-12 mb-2">
                                            <div style="float: left;">
                                                <span id="labTruckTypeDesc" runat="server" class="badge badge-info">ประเภทรถ : </span>
                                            </div>
                                            <div style="float: left; margin-left: 5px;">
                                                <span id="labTruckVolDesc" runat="server" class="badge badge-info">ปริมาตร : </span>
                                            </div>
                                            <div style="clear: both;"></div>
                                        </div>

                                        <asp:GridView ID="gridTruck" runat="server" AutoGenerateColumns="false" ShowHeaderWhenEmpty="true" DataKeyNames="truck_id"
                                            CssClass="table table-responsive-sm table-hover mb-0 gridview-custom" GridLines="None">
                                            <Columns>
                                                <asp:TemplateField HeaderText="เลือก" ItemStyle-Width="30px" ItemStyle-HorizontalAlign="Center">
                                                    <ItemTemplate>
                                                        <asp:RadioButton ID="rdbChooseTruck" runat="server" onclick="RadioCheck(this);" />
                                                        <asp:HiddenField ID="hidChooseTruck" runat="server" Value='<%#Eval("truck_id") %>' />
                                                    </ItemTemplate>
                                                </asp:TemplateField>
                                                <asp:BoundField HeaderText="ทะเบียน" DataField="license_plate" ItemStyle-HorizontalAlign="Center" />
                                                <asp:BoundField HeaderText="ชื่อรถ" DataField="truck_name" />
                                                <asp:BoundField HeaderText="ปริมาตรใช้แล้ว" DataField="volume_used" DataFormatString="{0:0.##}" ItemStyle-HorizontalAlign="Center" />
                                                <asp:BoundField HeaderText="ปริมาตรที่ว่าง" DataField="volume_remain" DataFormatString="{0:0.##}" ItemStyle-HorizontalAlign="Center" />
                                            </Columns>
                                        </asp:GridView>

                                        <div class="row col-sm-12 col-md-12 col-lg-12 mt-3 mb-1 ml-2">
                                            <ucControls:ButtonExt ID="btTspBack" runat="server" Text="Back Truck Type" CssClass="btn btn-sm btn-danger col-sm-5 col-md-5 col-lg-5" CausesValidation="false" OnClick="btTspBack_Click" ResourceGroup="general" ResourceName="btTspBack" />
                                            <ucControls:ButtonExt ID="btReCalTruckVolume" runat="server" Text="Calculate Volume" CssClass="btn btn-sm btn-info col-sm-6 col-md-6 col-lg-6" CausesValidation="false" OnClick="btReCalTruckVolume_Click" ResourceGroup="general" ResourceName="btReCalTruckVolume" />

                                        </div>

                                        <div style="margin-top: 10px;">
                                            <asp:Panel ID="panelSaveManifest" runat="server">
                                                <%--<div id="labCalTruckVolResult" runat="server" class="label label-success" style="width: 98%; padding-top: 12px;"></div>--%>

                                                <asp:UpdatePanel ID="updateSaveManifest" runat="server" UpdateMode="Conditional" ChildrenAsTriggers="true">
                                                    <ContentTemplate>
                                                        <div class="mt-2 ml-4">
                                                            <div style="float: left;">
                                                                <ucControls:InputDropDownHD ID="ddManifestId" runat="server" DisplayDefault="--Select--" ComboType="String" LabelText="Manifest ID" ResourceGroup="outbound_manifest" ResourceName="manifest_code" />
                                                                <ucControls:InputTextBox ID="txtManifestId" runat="server" LabelText="Manifest ID" ResourceGroup="outbound_manifest" ResourceName="manifest_code" />
                                                            </div>
                                                            <div style="float: left; margin-top: 20px; margin-left: 2px;">
                                                                <ucControls:ButtonExt ID="btTspNew" runat="server" Text="New" CssClass="btn btn-info" CausesValidation="false" OnClick="btTspNew_Click" ResourceGroup="general" ResourceName="btn_new" />
                                                            </div>
                                                            <div style="float: left; margin-top: 20px;" class="ml-2">
                                                                <ucControls:ButtonExt ID="btTspSave" runat="server" Text="Assign" CssClass="btn btn-success" CausesValidation="false"
                                                                    OnClick="btTspSave_Click" Width="80" ResourceGroup="general" ResourceName="btn_save" />
                                                            </div>
                                                            <div style="clear: both;"></div>
                                                        </div>

                                                    </ContentTemplate>
                                                </asp:UpdatePanel>
                                            </asp:Panel>

                                        </div>

                                    </asp:Panel>

                                </div>
                            </div>

                        </ContentTemplate>
                    </asp:UpdatePanel>

                    <div class="col-lg-8 col-md-12 col-sm-12">

                        <div class="card card-accent-success">
                            <div class="card-header">
                                <strong>Order Items List</strong>
                            </div>
                            <div class="card-block">

                                <ucControls:GridExt ID="gridTspItem" runat="server"
                                    SourceAssemblyName="WMS_NEW.Access" SourceClassName="WMS_NEW.Access.Transaction.OutbndManifest.ManifestTspItem" KeyField="KeyID" KeyType="Guid"
                                    GridAllowRowEdit="false" GridAllowRowDelete="false" GridAllowSelectBox="true" DisableExport="true" AutoSize="true" GridSortDefault="create_date desc">
                                    <CustomCommandTemplate>
                                        <ucControls:ButtonExt ID="btTspItemDel" runat="server" Text="Delete Item" CssClass="btn btn-sm btn-danger" CausesValidation="false" OnClick="btTspItemDel_Click" OnClientClick="if (!confirm('Delete selected items ?')) return false;" ResourceGroup="general" ResourceName="btn_delete" />
                                    </CustomCommandTemplate>
                                    <CustomSearchTemplate>
                                        <%-- <ucControls:InputTextBox ID="InputTextBox1" runat="server" DataFieldValue="_dummy" Visible="false" />--%>
                                        <ucControls:InputHidden ID="hidSessionId" runat="server" DataFieldValue="_sessionID" />
                                    </CustomSearchTemplate>
                                    <CustomColumnTemplate>
                                        <ucControls:GridColumnExt ID="GridColumnExt33" runat="server" HeaderText="" DataField="cmd_split" ControlType="CommandButton" CommandText="Split" CommandName="ITEM_SPLIT" IsConfirm="false" />
                                        <%--<ucControls:GridColumnExt ID="GridColumnExt8" runat="server" HeaderText="Warehouse" DataField="wh_id" AllowFilter="true" AllowSort="true" Width="150" ResourceGroup="warehouse" ResourceName="wh_id" />
                                    <ucControls:GridColumnExt ID="GridColumnExt12" runat="server" HeaderText="Owner" DataField="owner_code" AllowFilter="true" AllowSort="true" ResourceGroup="owner" ResourceName="Code" />--%>
                                        <ucControls:GridColumnExt ID="GridColumnExt14" runat="server" HeaderText="Province" DataField="province" AllowFilter="true" AllowSort="true" ShowFilterNow="true" ResourceGroup="customer" ResourceName="province" />
                                        <ucControls:GridColumnExt ID="GridColumnExt15" runat="server" HeaderText="Order No" DataField="outbound_order_number" AllowFilter="true" AllowSort="true" ShowFilterNow="true" ResourceGroup="outbound_master" ResourceName="outbound_order_number" />
                                        <ucControls:GridColumnExt ID="GridColumnExt44" runat="server" HeaderText="Line Number" DataField="line_number" AllowFilter="true" AllowSort="true" ResourceGroup="outbound_detail" ResourceName="line_number" />
                                        <ucControls:GridColumnExt ID="GridColumnExt13" runat="server" HeaderText="Item No" DataField="item_number" AllowFilter="true" AllowSort="true" ShowFilterNow="true" ResourceGroup="item" ResourceName="item_number" />
                                        <ucControls:GridColumnExt ID="GridColumnExt26" runat="server" HeaderText="Qty" DataField="item_quantity" FormatType="Number" AllowSort="true" ResourceGroup="outbound_detail" ResourceName="quantity_order" />
                                        <ucControls:GridColumnExt ID="GridColumnExt28" runat="server" HeaderText="Volume" DataField="volume_per" FormatType="Number" AllowSort="true" ResourceGroup="outbound_manifest" ResourceName="volume_per" />
                                        <ucControls:GridColumnExt ID="GridColumnExt21" runat="server" HeaderText="Volume Total" DataField="volume_total" FormatType="Number" AllowSort="true" ResourceGroup="outbound_manifest" ResourceName="volume_total" />
                                        <ucControls:GridColumnExt ID="GridColumnExt31" runat="server" HeaderText="UOM" DataField="uom" AllowFilter="true" AllowSort="true" ResourceGroup="item_uom" ResourceName="uom" />
                                        <ucControls:GridColumnExt ID="GridColumnExt34" runat="server" HeaderText="Create By" DataField="create_by" Width="130" ResourceGroup="general" ResourceName="create_by" />
                                        <ucControls:GridColumnExt ID="GridColumnExt38" runat="server" HeaderText="Create Date" DataField="create_date" Width="130" FormatType="DateTime" ResourceGroup="general" ResourceName="create_date" />
                                    </CustomColumnTemplate>
                                </ucControls:GridExt>

                            </div>
                        </div>

                    </div>

                </div>

                </ucControls:PanelControlTab>

                <ucControls:PanelControlTab ID="panelTspAssign" runat="server" PanelName="Manifest Order" ResourceGroup="outbound_manifest" ResourceName="tab_manifest_order">

                <ucControls:GridExt ID="gridTspAssign" runat="server"
                    SourceAssemblyName="WMS_NEW.Access" SourceClassName="WMS_NEW.Access.Transaction.OutbndManifest.ManifestTspAssign" KeyField="KeyID" KeyType="Guid"
                    GridAllowRowClick="true" AutoSize="true" GridSortDefault="manifest_code desc">
                    <CustomColumnTemplate>
                        <ucControls:GridColumnExt ID="GridColumnExt12" runat="server" HeaderText="Report" DataField="cmd_report" ControlType="CommandButton" CommandText="Report" CommandName="REPORT" IsConfirm="false" />
                        <ucControls:GridColumnExt ID="GridColumnExt40" runat="server" HeaderText="Manifest ID" DataField="manifest_code" AllowFilter="true" AllowSort="true" ShowFilterNow="true" ResourceGroup="outbound_manifest" ResourceName="manifest_code" />
                        <ucControls:GridColumnExt ID="GridColumnExt41" runat="server" HeaderText="Truck License" DataField="license_plate" AllowFilter="true" AllowSort="true" ShowFilterNow="true" ResourceGroup="outbound_master" ResourceName="license_plate" />
                        <ucControls:GridColumnExt ID="GridColumnExt42" runat="server" HeaderText="Truck Name" DataField="truck_name" AllowFilter="true" AllowSort="true" ShowFilterNow="true" ResourceGroup="outbound_master" ResourceName="truck_name" />
                        <ucControls:GridColumnExt ID="GridColumnExt43" runat="server" HeaderText="Volume Total" DataField="volume_total" FormatType="Number" AllowSort="true" ResourceGroup="outbound_manifest" ResourceName="volume_total" />
                    </CustomColumnTemplate>
                </ucControls:GridExt>

                </ucControls:PanelControlTab>

            </ControlTemplate>
        </ucControls:PanelTab>

    </div>

</asp:Content>
