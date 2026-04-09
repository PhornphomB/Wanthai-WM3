<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="BOMAgenDetail.ascx.cs" Inherits="WMS_NEW.MasterData.AscxControls.BOMAgenDetail" %>

<ucControls:PanelPopupEntity ID="popup" runat="server"  ColumnControlFix="2">
    <ControlTemplate>
        <ucControls:PanelControlRow runat="server">

            <ucControls:InputTextBox ID="txtBomCode" runat="server" Readonly="true" IsStaticValue="true" ResourceGroup="Bom" ResourceName="BomCode" />
            <ucControls:InputHidden ID="hidMasterId" runat="server" DataFieldValue="bom_id" IsStaticValue="true" />
            <ucControls:InputTextBox ID="txtLineNO" runat="server" Readonly="true" IsStaticValue="true" DataFieldValue="line_no" />
            <ucControls:InputDropDownHD ID="ddItem" runat="server" DataFieldValue="wh_item_master_id" IsPrimary="true" AutoPostBack="true" DisplayDefault="--Select--" />

            <ucControls:InputTextBox ID="txtDesc" runat="server" DataFieldValue="description" Readonly="true" Enabled="false" IsMultiLine="true" />
            <ucControls:InputTextNumber ID="txtQty" runat="server" DataFieldValue="quantity" IsPrimary="true" NumberType="Double" />
            <ucControls:InputDropDown ID="ddUom" runat="server" DataFieldValue="item_uom_id" IsPrimary="true" />

        </ucControls:PanelControlRow>
    </ControlTemplate>
</ucControls:PanelPopupEntity>

<ucControls:GridExt ID="GridExt1" runat="server"
    SourceAssemblyName="WMS_NEW.Access" SourceClassName="WMS_NEW.Access.MasterData.BomDetail" KeyField="KeyID" KeyType="Integer"
    GridAllowRowEdit="true" GridAllowRowDelete="true" DisableExport="true" DisableSearch="true" AutoSize="true" DisableFirstSearch="true">
    <CustomSearchTemplate>
        <ucControls:InputHidden runat="server" ID="hidSearchMasterId" DataFieldValue="bom_id" />
    </CustomSearchTemplate>
    <CustomColumnTemplate>
        <ucControls:GridColumnExt ID="GridColumnExt2" runat="server" HeaderText="Line No" DataField="line_no" />
        <ucControls:GridColumnExt ID="GridColumnExt5" runat="server" HeaderText="Item Number" DataField="item_number" />
        <ucControls:GridColumnExt ID="GridColumnExt9" runat="server" HeaderText="Description" DataField="description" />
        <ucControls:GridColumnExt ID="GridColumnExt1" runat="server" HeaderText="Quantity" DataField="quantity" />
        <ucControls:GridColumnExt ID="GridColumnExt10" runat="server" HeaderText="UOM" DataField="uom" />
        <ucControls:GridColumnExt ID="GridColumnExt11" runat="server" HeaderText="Create By" DataField="create_by" />
        <ucControls:GridColumnExt ID="GridColumnExt12" runat="server" HeaderText="Create Date" DataField="create_date" />
    </CustomColumnTemplate>
</ucControls:GridExt>
