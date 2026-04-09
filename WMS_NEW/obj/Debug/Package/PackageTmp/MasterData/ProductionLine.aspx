<%@ Page Title="" Language="C#" MasterPageFile="~/Layout.Master" AutoEventWireup="true" CodeBehind="ProductionLine.aspx.cs" Inherits="WMS_NEW.MasterData.ProductionLine" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">

    <%--<ucControls:PanelPopupEntity ID="popupEntity1" runat="server" />--%>
   
    <ucControls:PanelPopupEntity ID="popupEntity1" runat="server" ColumnControlFix="1">
    <ControlTemplate>
        <div class="row">
            <ucControls:PanelControlRow ID="PanelControlRow0" runat="server">
                <ucControls:InputDropDown runat="server" ResourceGroup="warehouse" ResourceName="wh_id" ID="ddlWarehouse" DataFieldValue="wh_master_id" IsPrimary="true" IsKey="true" UseDefaultDisplay="false" />
                <ucControls:InputTextBox ID="txtProductionLine" runat="server" ResourceGroup="inbound_master" ResourceName="production_line" DataFieldValue="production_line" IsPrimary="true" />
                <ucControls:InputTextBox ID="txtErpProductionLine" runat="server" ResourceGroup="production_line" ResourceName="erp_production_line" DataFieldValue="erp_production_line" IsPrimary="true"/>
                <ucControls:InputTextBox ID="txtDescription" runat="server"  ResourceGroup="production_line" ResourceName="description" DataFieldValue="description" />
                <ucControls:InputCheckBox ID="chkActive" runat="server" DataFieldValue="is_active" CheckBoxType="String" ColumnSpan="2" LabelText="Active" ResourceGroup="General" ResourceName="Active" />
            </ucControls:PanelControlRow>
        </div>

    </ControlTemplate>
</ucControls:PanelPopupEntity>

    <ucControls:GridExt ID="GridExt1" runat="server"
        SourceAssemblyName="WMS_NEW.Access" SourceClassName="WMS_NEW.Access.MasterData.ProductionLine" KeyField="KeyId" KeyType="Guid"
        ColumnFreezeLength="0" GridSortDefault="wh_id" GridAllowRowEdit="true" GridAllowRowDelete="true"
        AutoGenerateColumn="true" ShowAllSort="true" ShowAllFilter="true" AutoGenColumnIndexs_SEARCH="3,4,5" AutoGenColumnFields_Exclude="wh_master_id">
        <%--<CustomSearchTemplate>
            <ucControls:InputDropDown ID="iColWarehouse" runat="server" ResourceGroup="warehouse" ResourceName="wh_id" DataFieldValue="_wh_master_id" ComboType="Guid" DisplayDefault="--All--" DefaultFilter="Equal" />
        </CustomSearchTemplate>--%>
        <CustomColumnTemplate>
            <ucControls:GridColumnExt ID="GridColumnExt1" runat="server" HeaderText="Warehouse" DataField="wh_id" DataFieldFilter="wh_master_id" AllowSort="true" ResourceGroup="warehouse" ResourceName="wh_id" FilterFormatType="Guid" ShowFilterNow="true" DropDownDisplayDefault="--All--" UseFilterDropDown="true" />
            <ucControls:GridColumnExt ID="GridColumnExt2" runat="server" HeaderText="Warehouse" DataField="wh_name" AllowSort="true" ResourceGroup="warehouse" ResourceName="wh_name" />
            <ucControls:GridColumnExt ID="GridColumnExt3" runat="server" HeaderText="Warehouse" DataField="production_line" AllowSort="true" ResourceGroup="inbound_master" ResourceName="production_line" ShowFilterNow="true" />
            <ucControls:GridColumnExt ID="GridColumnExt5" runat="server" HeaderText="Erp Production Line" DataField="erp_production_line" AllowSort="true" ResourceGroup="production_line" ResourceName="erp_production_line" ShowFilterNow="true" />
            <ucControls:GridColumnExt ID="GridColumnExt4" runat="server" HeaderText="Warehouse" DataField="description" AllowSort="true" ResourceGroup="production_line" ResourceName="description" />
        </CustomColumnTemplate>
    </ucControls:GridExt>

</asp:Content>
