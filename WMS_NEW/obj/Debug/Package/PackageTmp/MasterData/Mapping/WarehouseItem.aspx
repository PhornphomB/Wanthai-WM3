<%@ Page Title="" Language="C#" MasterPageFile="~/Layout.Master" AutoEventWireup="true" CodeBehind="WarehouseItem.aspx.cs" Inherits="WMS_NEW.MasterData.Mapping.WarehouseItem" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <ucControls:GridExt ID="GridExt1" runat="server"
        SourceAssemblyName="WMS_NEW.Access" SourceClassName="WMS_NEW.Access.MasterData.Mapping.WarehouseItem" KeyField="KeyId" KeyType="String" KeyFieldSelect="is_active"
        ColumnFreezeLength="0" GridSortDefault="item_number" GridAllowRowEdit="false" GridAllowRowDelete="false"
        AutoGenerateColumn="true" ShowAllSort="true" ShowAllFilter="false" GridAllowSelectBox="true" DisableFirstSearch="true" AutoGenColumnFields_Exclude="is_active" AutoGenColumnFields_SEARCH="item_number">
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
        <CustomColumnTemplate>
            <ucControls:GridColumnExt runat="server" ResourceGroup="warehouse" ResourceName="wh_id" ID="iColWarehouse" DataField="wh_id" DataFieldFilter="wh_master_id" AllowFilter="true" ShowFilterNow="true" FilterPrimary="true" FilterFormatType="Guid" UseFilterDropDown="true" DropDownFilterType="Normal" DropDownDisplayDefault="--Select--" DropDownAutoPostBack="true" />
            <ucControls:GridColumnExt ID="iColOwner" runat="server" HeaderText="Owner" DataField="owner_code" DataFieldFilter="owner_id" FilterFormatType="Guid" ShowFilterNow="true" AllowFilter="true" AllowSort="true" UseFilterDropDown="true" DropDownDisplayDefault="--All--" ResourceGroup="owner" ResourceName="Code" />
            <ucControls:GridColumnExt runat="server" ResourceGroup="item" ResourceName="item_number" DataField="item_number" AllowFilter="true" AllowSort="true" ShowFilterNow="true" />
            <ucControls:GridColumnExt runat="server" ResourceGroup="general" ResourceName="map_status" ID="iColActive" DataField="map_status" AllowFilter="true" ShowFilterNow="true" FilterFormatType="Text" UseFilterDropDown="true" DropDownFilterType="Normal" DropDownDisplayDefault="-- All --" />
        </CustomColumnTemplate>
    </ucControls:GridExt>
</asp:Content>
