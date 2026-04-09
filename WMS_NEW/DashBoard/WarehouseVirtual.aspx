<%@ Page Title="" Language="C#" MasterPageFile="~/Layout.Master" AutoEventWireup="true" CodeBehind="WarehouseVirtual.aspx.cs" Inherits="WMS_NEW.DashBoard.WarehouseVirtual" %>

<%@ Register Src="~/DashBoard/AscxControls/ucWarehouseVirtualDetail.ascx" TagPrefix="ucControls" TagName="ucWarehouseVirtualDetail" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">

    <link href="../_css/theme/red-white/warehouse_virtual.css" rel="stylesheet" />

    <ucControls:PanelPopup runat="server" ID="popDetail" HeaderText="View Detail">
        <DataTemplate>
            <ucControls:ucWarehouseVirtualDetail runat="server" ID="ucWarehouseVirtualDetail" />
        </DataTemplate>

    </ucControls:PanelPopup>

    <div class="col-lg-12 pt-3 pb-3 background-base">

        <asp:UpdatePanel ID="updateFilter" runat="server" UpdateMode="Conditional" ChildrenAsTriggers="true" CssClass="col-sm-12">
            <ContentTemplate>
                <ucControls:PanelControlRow ID="pnFilter" runat="server" CssClass="row">
                    <ucControls:InputDropDown ID="ddlWareHouse" runat="server" Filterable="true" DataFieldValue="wh_master_id" DisplayDefault="-- All --" ResourceGroup="warehouse" ResourceName="wh_id" BaseContentCss="col-sm-2" IsPrimary="true"/>
                    <ucControls:InputDropDown ID="ddlLocationLevel" runat="server" Filterable="true" DataFieldValue="location_level" DisplayDefault="-- Select --" ResourceGroup="location" ResourceName="location_level" BaseContentCss="col-sm-2" IsPrimary="true" ComboType="String"/>
                </ucControls:PanelControlRow>
            </ContentTemplate>
        </asp:UpdatePanel>

        <asp:UpdatePanel ID="updateContent" runat="server" UpdateMode="Conditional" ChildrenAsTriggers="true" CssClass="row col-sm-12" style="padding-bottom: 14px;">
            <ContentTemplate>
                <ucControls:ButtonExt ID="btToggle" runat="server" Text="Hide Filter" CssClass="btn btn-sm btn-info" CausesValidation="false" OnClick="btToggle_Click" />
                <ucControls:ButtonExt ID="btSearch" runat="server" Text="Search" CssClass="btn btn-sm btn-success" CausesValidation="true" OnClick="btSearch_Click" ResourceGroup="general" ResourceName="search" />
                <ucControls:ButtonExt ID="btClear" runat="server" Text="Clear" CssClass="btn btn-sm btn-danger" CausesValidation="false" OnClick="btClear_Click" ResourceGroup="general" ResourceName="clear" />
            </ContentTemplate>
        </asp:UpdatePanel>

        <ucControls:PanelTab ID="PanelTab1" runat="server">
            <ControlTemplate>
                <ucControls:PanelControlTab ID="PanelControlTab1" runat="server" PanelName="Warehouse Visual" ResourceGroup="warehouse_virtual" ResourceName="tab_warehouse_virtual">
                    <asp:UpdatePanel ID="updateVirtual" runat="server" UpdateMode="Conditional" ChildrenAsTriggers="true">
                        <ContentTemplate>
                            <div class="warehouse">
                                <asp:Literal runat="server" ID="txtVisual"></asp:Literal>
                            </div>

                        </ContentTemplate>
                    </asp:UpdatePanel>
                </ucControls:PanelControlTab>

                <ucControls:PanelControlTab ID="pnItem" runat="server" PanelName="Warehouse Data" ResourceGroup="warehouse_virtual" ResourceName="tab_warehouse_data">
                    <ucControls:GridExt ID="GridExtItem" runat="server"
                        SourceAssemblyName="WMS_NEW.Access" SourceClassName="WMS_NEW.Access.DashBoard.WarehouseVirtual" KeyField="KeyId" KeyType="String"
                        DisableButtonSearch="true" DisableFirstSearch="true" GridSortDefault="zone, location" AutoSize="true">
                        <CustomCommandTemplate>
                            <%--<ucControls:ButtonExt runat="server" ID="btnNew" OnClick="btnNew_Click" Text="new" />--%>
                        </CustomCommandTemplate>
                        <CustomSearchTemplate>
                            <ucControls:InputHidden runat="server" ID="hdf_wh_master_id" DataFieldValue="_wh_master_id" />
                            <ucControls:InputHidden runat="server" ID="hdf_location_level" DataFieldValue="_location_level" />

                        </CustomSearchTemplate>
                        <CustomColumnTemplate>
                            <ucControls:GridColumnExt ID="GridColumnExt3" runat="server" ResourceGroup="zone" ResourceName="zone" DataField="zone" AllowSort="true" />
                            <ucControls:GridColumnExt ID="GridColumnExt4" runat="server" ResourceGroup="location" ResourceName="location" DataField="location" AllowSort="true" />
                            <ucControls:GridColumnExt ID="GridColumnExt11" runat="server" ResourceGroup="location" ResourceName="capacity_qty" DataField="capacity_qty" FormatType="Number" AllowSort="true" />
                            <ucControls:GridColumnExt ID="GridColumnExt1" runat="server" ResourceGroup="location" ResourceName="current_qty" DataField="current_qty" FormatType="Number" AllowSort="true" />
                            <ucControls:GridColumnExt ID="GridColumnExt2" runat="server" ResourceGroup="location" ResourceName="available_qty" DataField="available_qty" FormatType="Number" AllowSort="true" />
                            <ucControls:GridColumnExt ID="GridColumnExt5" runat="server" ResourceGroup="location" ResourceName="usage_qty" DataField="usage_qty" FormatType="Number" AllowSort="true" />
                        </CustomColumnTemplate>
                    </ucControls:GridExt>
                </ucControls:PanelControlTab>
            </ControlTemplate>
        </ucControls:PanelTab>

    </div>
</asp:Content>
