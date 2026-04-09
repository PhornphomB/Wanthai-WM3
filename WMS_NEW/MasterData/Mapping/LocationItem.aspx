<%@ Page Title="" Language="C#" MasterPageFile="~/Layout.Master" AutoEventWireup="true" CodeBehind="LocationItem.aspx.cs" Inherits="WMS_NEW.MasterData.Mapping.LocationItem" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <ucControls:GridExt ID="GridExt1" runat="server"
        SourceAssemblyName="WMS_NEW.Access" SourceClassName="WMS_NEW.Access.MasterData.Mapping.LocationItem" KeyField="key_id" KeyType="String" KeyFieldSelect="is_active" AutoGenerateColumn="false"
        ColumnFreezeLength="0" GridSortDefault="location ,item_number" GridAllowRowEdit="false" GridAllowRowDelete="false" ShowAllSort="true" ShowAllFilter="false" GridAllowSelectBox="true" DisableFirstSearch="true">
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
            <ucControls:InputHidden runat="server" ID="hidLocationID" DataFieldValue="_location_id" />

<%--            <ucControls:InputTextBox runat="server" ID="txtFilterItem" DataFieldValue="_item_number" DefaultFilter="Contains" FixFilter="true" />--%>
        </CustomSearchTemplate>
        <CustomColumnTemplate>
            <ucControls:GridColumnExt runat="server" ResourceGroup="warehouse" ResourceName="wh_id" ID="iColWh" DataField="wh_id" DataFieldFilter="wh_master_id" AllowFilter="true" ShowFilterNow="true" FilterPrimary="true" FilterFormatType="Guid" UseFilterDropDown="true" DropDownFilterType="Normal" DropDownDisplayDefault="--Select--" DropDownAutoPostBack="true" />
            <ucControls:GridColumnExt runat="server" ResourceGroup="location" ResourceName="location" ID="iColLocation" DataField="location" DataFieldFilter="location_id" AllowFilter="true" ShowFilterNow="true" FilterPrimary="false" FilterFormatType="Guid" UseFilterDropDown="true" DropDownFilterType="LazySearch" DropDownDisplayDefault="--Select--" DropDownAutoPostBack="true" FixFilter="true" DefaultFilter="Equal" />
            <ucControls:GridColumnExt runat="server" ResourceGroup="item" ResourceName="item_number" ID="GridColumnExt2" DataField="item_number" AllowFilter="true" ShowFilterNow="true" FixFilter="true" DefaultFilter="Contains" />
            <ucControls:GridColumnExt runat="server" ResourceGroup="item" ResourceName="description" ID="GridColumnExt1" DataField="description" AllowFilter="true" ShowFilterNow="true" FixFilter="true" DefaultFilter="Contains" />
            <ucControls:GridColumnExt runat="server" ResourceGroup="general" ResourceName="map_status" ID="iColActive" DataField="map_status" AllowFilter="true" ShowFilterNow="true" FilterFormatType="Text" UseFilterDropDown="true" DropDownFilterType="Normal" DropDownDisplayDefault="-- All --" />
        </CustomColumnTemplate>
    </ucControls:GridExt>
</asp:Content>
