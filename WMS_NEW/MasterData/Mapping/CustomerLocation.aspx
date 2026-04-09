<%@ Page Title="" Language="C#" MasterPageFile="~/Layout.Master" AutoEventWireup="true" CodeBehind="CustomerLocation.aspx.cs" Inherits="WMS_NEW.MasterData.Mapping.CustomerLocation" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <ucControls:GridExt ID="GridExt1" runat="server"
        SourceAssemblyName="WMS_NEW.Access" SourceClassName="WMS_NEW.Access.MasterData.Mapping.CustomerLocation" KeyField="key_id" KeyType="String" KeyFieldSelect="is_active" AutoGenerateColumn="false"
        ColumnFreezeLength="0" GridSortDefault="wh_id,location" GridAllowRowEdit="false" GridAllowRowDelete="false" ShowAllSort="true" ShowAllFilter="false" GridAllowSelectBox="true" DisableFirstSearch="true">
        <CustomCommandTemplate>
            <div class="input-group" style="margin-left: 5px;">
                <ucControls:ButtonExt ID="btMapping" runat="server" Text="Save" CssClass="btn btn-sm btn-primary" CausesValidation="false"
                    OnClientClick="if (!confirm('Do you want to confirm ?')) return false;" OnClick="btMapping_Click" ResourceGroup="General" ResourceName="Save" />

                <span class="input-group-prepend">
                    <ucControls:InputDropDown ID="comboMapType" runat="server" AutoPostBack="true"
                        DisableSearchData="true" VisibleLabel="true" ComboType="String" />
                </span>
            </div>
        </CustomCommandTemplate>
        <CustomSearchTemplate>
            <ucControls:InputDropDownHD runat="server" ID="ddlWarehouse" ResourceGroup="warehouse" ResourceName="wh_id" DataFieldValue="_wh_master_id" IsPrimary="true" AutoPostBack="true" DisplayDefault="--Select--" Filterable="true" FixFilter="true" DefaultFilter="Equal" />
            <ucControls:InputDropDownHD runat="server" ID="ddlCustomer" ResourceGroup="customer" ResourceName="customer" DataFieldValue="_customer_id" IsPrimary="true" AutoPostBack="true" DisplayDefault="--Select--" Filterable="true" FixFilter="true" DefaultFilter="Equal" />
        </CustomSearchTemplate>
        <CustomColumnTemplate>
            <ucControls:GridColumnExt runat="server" ResourceGroup="warehouse" ResourceName="wh_id" ID="iColWh" DataField="wh_id" AllowFilter="false" ShowFilterNow="false" />
<%--            <ucControls:GridColumnExt runat="server" ResourceGroup="warehouse" ResourceName="wh_id" ID="GridColumnExt1" DataField="wh_id" DataFieldFilter="wh_master_id" AllowFilter="true" ShowFilterNow="true" FilterFormatType="Guid" UseFilterDropDown="true" DropDownFilterType="LazySearch" DropDownDisplayDefault="--All--" DropDownAutoPostBack="true" FilterPrimary="true" />--%>
            <ucControls:GridColumnExt runat="server" ResourceGroup="location" ResourceName="location" ID="iColLocation" DataField="location" DataFieldFilter="location_id" AllowFilter="true" ShowFilterNow="true" FilterFormatType="Guid" UseFilterDropDown="true" DropDownFilterType="LazySearch" DropDownDisplayDefault="--All--" />
            <ucControls:GridColumnExt runat="server" ResourceGroup="general" ResourceName="map_status" ID="iColActive" DataField="map_status" AllowFilter="true" ShowFilterNow="true" FilterFormatType="Text" UseFilterDropDown="true" DropDownFilterType="Normal" DropDownDisplayDefault="-- All --" />

        </CustomColumnTemplate>
    </ucControls:GridExt>
</asp:Content>
