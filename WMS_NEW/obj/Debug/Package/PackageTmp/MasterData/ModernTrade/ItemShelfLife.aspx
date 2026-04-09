<%@ Page Title="" Language="C#" MasterPageFile="~/Layout.Master" AutoEventWireup="true" CodeBehind="ItemShelfLife.aspx.cs" Inherits="WMS_NEW.MasterData.ModernTrade.ItemShelfLife" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">

    <ucControls:PanelPopupEntity ID="popupEntity1" runat="server" ColumnControlFix="2">
        <ControlTemplate>
            <ucControls:PanelControlRow runat="server">
                <%--<ucControls:InputDropDown ID="ddlWarehouse" runat="server" ResourceGroup="ModernTrade" ResourceName="wh_master_id" DataFieldValue="wh_master_id" IsPrimary="true" IsKey="true" DisplayDefault="--Select--" LabelText="Warehouse" />
                <ucControls:LabelExt ID="lblDummy1" runat="server" DefaultText="" CssClass="col-sm-6 col-md-4 col-lg-6" />

                <ucControls:InputDropDownHD ID="ddAgency" runat="server" ResourceGroup="ModernTrade" ResourceName="owner_id" DataFieldValue="owner_id" IsPrimary="true" IsKey="true" DisplayDefault="--Select--" AutoPostBack="true" LabelText="Agency" />
                <ucControls:InputDropDownHD ID="ddlCustomer" runat="server" ResourceGroup="ModernTrade" ResourceName="customer_id" DataFieldValue="customer_id" IsPrimary="true" IsKey="true" DisplayDefault="--Select--" AutoPostBack="true" LabelText="Customer Code" /> 

                <ucControls:InputCheckBox ID="chkCategory" runat="server" ResourceGroup="ModernTrade" ResourceName="choose_category" LabelText="Choose Category" CheckBoxType="String" AutoPostBack="true" IsKey="false" />
                <ucControls:InputCheckBox ID="chkItem" runat="server" ResourceGroup="ModernTrade" ResourceName="choose_item" LabelText="Choose Item" CheckBoxType="String" AutoPostBack="true" IsKey="false" />

                <ucControls:InputDropDownHD ID="ddlCategory" runat="server" ResourceGroup="ModernTrade" ResourceName="category_id" DataFieldValue="category_id" IsPrimary="true" />
                <ucControls:InputDropDownHD ID="ddlItem" runat="server" ResourceGroup="ModernTrade" ResourceName="item_master_id" DataFieldValue="item_master_id" IsPrimary="true" />

                <ucControls:InputTextInteger ID="txtDayRemaining" runat="server" ResourceGroup="ModernTrade" ResourceName="shelf_life_day_remaining"  DataFieldValue="shelf_life_day_remaining" LabelText="Day Remaining" IsPrimary="true" />--%>
                <ucControls:InputDropDown ID="ddlWarehouse" runat="server" DataFieldValue="wh_master_id" IsPrimary="true" IsKey="true" DisplayDefault="--Select--" LabelText="Warehouse" />
                <ucControls:LabelExt ID="lblDummy1" runat="server" DefaultText="" CssClass="col-sm-6 col-md-4 col-lg-6" />

                <ucControls:InputDropDownHD ID="ddAgency" runat="server" DataFieldValue="owner_id" IsPrimary="true" IsKey="true" DisplayDefault="--Select--" AutoPostBack="true" LabelText="Agency" />
                <ucControls:InputDropDownHD ID="ddlCustomer" runat="server" DataFieldValue="customer_id" IsPrimary="true" IsKey="true" DisplayDefault="--Select--" AutoPostBack="true" LabelText="Customer Code" /> 

                <ucControls:InputCheckBox ID="chkCategory" runat="server" LabelText="Choose Category" CheckBoxType="String" AutoPostBack="true" IsKey="false" />
                <ucControls:InputCheckBox ID="chkItem" runat="server" LabelText="Choose Item" CheckBoxType="String" AutoPostBack="true" IsKey="false" />

                <ucControls:InputDropDownHD ID="ddlCategory" runat="server" DataFieldValue="category_id" IsPrimary="true" />
                <ucControls:InputDropDownHD ID="ddlItem" runat="server" DataFieldValue="item_master_id" IsPrimary="true" />

                <ucControls:InputTextInteger ID="txtDayRemaining" runat="server" DataFieldValue="shelf_life_day_remaining" LabelText="Day Remaining" IsPrimary="true" />
            </ucControls:PanelControlRow>

        </ControlTemplate>
    </ucControls:PanelPopupEntity>

    <ucControls:GridExt ID="GridExt1" runat="server"
        SourceAssemblyName="WMS_NEW.Access" SourceClassName="WMS_NEW.Access.MasterData.ModernTrade.ItemShelfLife" KeyField="KeyId" KeyType="Guid"
        ColumnFreezeLength="0" GridSortDefault="agency,category_description,item_number" GridAllowRowEdit="true" GridAllowRowDelete="true"
        AutoGenerateColumn="true" ShowAllSort="true" ShowAllFilter="true" AutoGenColumnIndexs_SEARCH="1,2,3,4">
        <CustomColumnTemplate>
            <ucControls:GridColumnExt ID="iColWarehouse" runat="server" DataField="wh_id" DataFieldFilter="wh_master_id" FilterFormatType="Guid" UseFilterDropDown="true" DropDownDisplayDefault="--All--" HeaderText="Warehouse" />
            <ucControls:GridColumnExt ID="iColOwner" runat="server" DataField="agency" DataFieldFilter="owner_id" FilterFormatType="Guid" UseFilterDropDown="true" DropDownDisplayDefault="--All--" />
            <ucControls:GridColumnExt ID="iColCustomer" runat="server" DataField="customer_name" DataFieldFilter="customer_id" FilterFormatType="Guid" UseFilterDropDown="true" DropDownDisplayDefault="--All--" />
            <ucControls:GridColumnExt ID="iColCategory" runat="server" DataField="category_description" DataFieldFilter="category_id" FilterFormatType="Guid" UseFilterDropDown="true" DropDownDisplayDefault="--All--" />
            <ucControls:GridColumnExt ID="iColItem" runat="server" DataField="item_number" DataFieldFilter="item_master_id" FilterFormatType="Guid" UseFilterDropDown="true" DropDownDisplayDefault="--All--" />
        </CustomColumnTemplate>
    </ucControls:GridExt>

</asp:Content>
