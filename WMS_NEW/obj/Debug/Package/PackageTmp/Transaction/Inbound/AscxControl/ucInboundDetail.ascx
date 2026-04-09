<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="ucInboundDetail.ascx.cs" Inherits="WMS_NEW.Transaction.Inbound.AscxControl.ucInboundDetail" %>
<%@ Register Src="~/Transaction/Inbound/AscxControl/ucInboundReceipt.ascx" TagPrefix="ucControls" TagName="ucInboundReceipt" %>
<%@ Register Src="~/Report/AscxControls/ucReportViewer.ascx" TagPrefix="ucControls" TagName="ucReportViewer" %>
<%@ Register Src="~/Transaction/Inbound/AscxControl/ucInboundPrint144.ascx" TagPrefix="ucControls" TagName="ucInboundPrint144" %>
<%--<%@ Register Src="~/Transaction/Inbound/AscxControl/ucInboundPrint.ascx" TagPrefix="ucControls" TagName="ucInboundPrint" %>--%>

<ucControls:ucInboundReceipt runat="server" ID="ucInboundReceipt" />
<%--<ucControls:ucInboundPrint runat="server" ID="ucInboundPrint" />--%>
<ucControls:ucReportViewer runat="server" ID="ucReportViewer" />
<ucControls:ucInboundPrint144 runat="server" ID="ucInboundPrint144" />

<ucControls:PanelPopupEntity ID="popInboundDetail" runat="server" AutoClear="false">
    <ControlTemplate>
        <ucControls:PanelTab ID="PanelTab1" runat="server">
            <ControlTemplate>
                <ucControls:PanelControlTab ID="PanelControlTab1" runat="server" PanelName="General" ResourceGroup="inbound_detail" ResourceName="tab_general">
                    <ucControls:PanelControlRow ID="PanelControlRow1" runat="server">
                        <ucControls:InputTextBox ID="txtOwnerCode" runat="server" ResourceGroup="owner" ResourceName="owner_code" Enabled="false" />


                        <ucControls:InputTextBox ID="txtLineNumber" runat="server" ResourceGroup="general" ResourceName="line_number" DataFieldValue="line_number" Enabled="false" />

                        <%-- <asp:Panel ID="panelWhItem" runat="server">--%>
                        <ucControls:InputDropDownHD ID="ddlWhItem" runat="server" ResourceGroup="item" ResourceName="item_number" DataFieldValue="wh_item_master_id" AutoPostBack="true" IsPrimary="true" IsKey="true" DisplayDefault="--Select--" ColumnSpan="2" />
                        <%--</asp:Panel>--%>
                        <ucControls:InputDropDownHD ID="ddlItemBom" runat="server" ResourceGroup="item" ResourceName="item_bom" DataFieldValue="bom_id" IsPrimary="true" DisplayDefault="--Select--" ComboType="Integer" ColumnSpan="2" />
                        <ucControls:InputCheckBox ID="chkItemBom" runat="server" ResourceGroup="inbound_detail" ResourceName="create_by_bom" LabelText="Create By Bom" CheckBoxType="Boolean" AutoPostBack="true" ColumnSpan="1" />


                        <ucControls:InputTextBox ID="txtItemCrossRef" runat="server" ResourceGroup="item_crossref" ResourceName="alternate_item_number" Enabled="false" />
                        <ucControls:InputTextBox ID="txtItemDesc" runat="server" ResourceGroup="item" ResourceName="description" Enabled="false" IsMultiLine="true" />

                        <ucControls:InputTextBox ID="txtlot" runat="server" ResourceGroup="inventory" ResourceName="lot_number" DataFieldValue="lot_number" />
                        <ucControls:InputTextDate ID="txtMFGDate" runat="server" ResourceGroup="inventory" ResourceName="mfg_date" DataFieldValue="mfg_date" TextMode="Date" AutoPostBack="true" IsPrimary="true" ColumnSpan="2" />
                        <ucControls:InputTextInteger ID="txtMonthToExp" runat="server" ResourceGroup="inbound_detail" ResourceName="month_to_expire" DataFieldValue="month_to_expire" ColumnSpan="1" Enabled="false" AutoPostBack="true" IsFocusOut="true" />
                        <ucControls:InputTextDate ID="txtExpDate" runat="server" ResourceGroup="inventory" ResourceName="expiry_date" DataFieldValue="expiry_date" TextMode="Date" />
                        <ucControls:InputTextBox ID="txtSerial" runat="server" ResourceGroup="inventory" ResourceName="serial_number" DataFieldValue="serial_number" KeyEnterName="serial" />
                        <ucControls:InputTextBox ID="txtAttribute1" runat="server" ResourceGroup="inventory" ResourceName="attribute1" DataFieldValue="attribute1" />
                        <ucControls:InputTextBox ID="txtAttribute2" runat="server" ResourceGroup="inventory" ResourceName="attribute2" DataFieldValue="attribute2" />
                        <ucControls:InputTextBox ID="txtAttribute3" runat="server" ResourceGroup="inventory" ResourceName="attribute3" DataFieldValue="attribute3" />
                        <ucControls:InputTextBox ID="txtAttribute4" runat="server" ResourceGroup="inventory" ResourceName="attribute4" DataFieldValue="attribute4" />
                        <ucControls:InputTextBox ID="txtAttribute5" runat="server" ResourceGroup="inventory" ResourceName="attribute5" DataFieldValue="attribute5" />
                        <ucControls:InputDropDown ID="ddlDefaultItemStatus" runat="server" ResourceGroup="inbound_detail" ResourceName="default_item_status" DataFieldValue="default_item_status" ComboType="String" UseDefaultDisplay="false" />
                        <ucControls:InputTextNumber ID="txtPrice" runat="server" ResourceGroup="item" ResourceName="price" DataFieldValue="price" />
                        <ucControls:InputTextNumber ID="txtOrderQTY" runat="server" ResourceGroup="inbound_detail" ResourceName="quantity_order" DataFieldValue="quantity_order" IsPrimary="true" MaxLength="15" />
                        <ucControls:InputDropDown ID="ddlUOM" runat="server" ResourceGroup="item_uom" ResourceName="item_uom_id" DataFieldValue="item_uom_id" IsPrimary="true" DisplayDefault="-- Select --" AutoPostBack="true" />
                        <ucControls:InputHidden ID="hidPackSizeUOMId" runat="server" DataFieldValue="pack_size_uom_id" />
                        <ucControls:InputHidden ID="hidPackSizeUOM" runat="server" DataFieldValue="pack_size_uom" />
                        <ucControls:InputTextNumber ID="txtPackSizeQty" runat="server" ResourceGroup="item_uom" ResourceName="pack_size_conversion_factor" DataFieldValue="pack_size_conversion_factor" IsPrimary="false" Enabled="false" />
                        <ucControls:InputTextBox ID="txtBaseUnit" runat="server" ResourceGroup="item_uom" ResourceName="base_unit" DataFieldValue="base_unit" Enabled="false" />
                        <ucControls:InputDropDown ID="ddlPalletUOM" runat="server" ResourceGroup="item_uom" ResourceName="pallet_uom" DataFieldValue="pallet_size_uom_id" IsPrimary="true" DisplayDefault="-- Select --" AutoPostBack="true" />
                        <ucControls:InputHidden ID="txtPalletSizeUom" runat="server" DataFieldValue="pallet_size_uom" />
                        <ucControls:InputTextNumber ID="txtPalletSizeQty" runat="server" ResourceGroup="item_uom" ResourceName="pallet_size_conversion_factor" DataFieldValue="pallet_size_conversion_factor" IsPrimary="false" Enabled="false" />
                        <ucControls:InputDropDownHD ID="ddlProductionLine" runat="server" ResourceGroup="inbound_master" ResourceName="production_line" DataFieldValue="production_line" ComboType="String" ControlSequence="2" DisplayDefault="--Select--" IsPrimary="true" />
                        <ucControls:InputTextBox ID="txtRefOutboundOrderNumber" runat="server" ResourceGroup="inbound_master" ResourceName="ref_outbound_order_number" DataFieldValue="ref_outbound_order_number" />
                        <ucControls:InputDropDownHD ID="ddRefItemNumber" runat="server" ResourceGroup="inbound_detail" ResourceName="ref_item_number" DataFieldValue="ref_item_number" DisplayDefault="-- Select --" ComboType="String" />
                        <ucControls:InputTextNumber ID="txtOrderSequence" runat="server" ResourceGroup="inbound_detail" ResourceName="order_sequence" DataFieldValue="order_sequence" Enabled="false" />
                        <ucControls:InputDropDown ID="ddlOverReceipt" runat="server" ResourceGroup="inbound_detail" ResourceName="over_receipt_allowed" DataFieldValue="over_receipt_allowed" AutoPostBack="true" ComboType="String" />
                        <ucControls:InputTextNumber ID="txtOverPercent" runat="server" ResourceGroup="inbound_detail" ResourceName="over_receipt_percentage" DataFieldValue="over_receipt_percentage" NumberDegit="2" />
                        <ucControls:InputTextBox ID="txtRefLpn" runat="server" ResourceGroup="inbound_master" ResourceName="ref_lpn" DataFieldValue="ref_lpn" Enabled="false" />

                    </ucControls:PanelControlRow>
                </ucControls:PanelControlTab>

                <ucControls:PanelControlTab ID="PanelControlTab2" runat="server" ResourceGroup="inbound_detail" ResourceName="tab_user_define" PanelName="User Define">
                    <ucControls:PanelControlRow ID="PanelControlRow2" runat="server">
                        <ucControls:InputTextBox ID="InputTextBox1" runat="server" ResourceGroup="inbound_detail" ResourceName="user_def1" DataFieldValue="user_def1" />
                        <ucControls:InputTextBox ID="InputTextBox2" runat="server" ResourceGroup="inbound_detail" ResourceName="user_def2" DataFieldValue="user_def2" />
                        <ucControls:InputTextBox ID="InputTextBox6" runat="server" ResourceGroup="inbound_detail" ResourceName="user_def3" DataFieldValue="user_def3" />
                        <ucControls:InputTextBox ID="InputTextBox7" runat="server" ResourceGroup="inbound_detail" ResourceName="user_def4" DataFieldValue="user_def4" />
                        <ucControls:InputTextBox ID="InputTextBox8" runat="server" ResourceGroup="inbound_detail" ResourceName="user_def5" DataFieldValue="user_def5" />
                        <ucControls:InputTextBox ID="InputTextBox9" runat="server" ResourceGroup="inbound_detail" ResourceName="user_def6" DataFieldValue="user_def6" />
                        <ucControls:InputTextNumber ID="InputTextBox20" runat="server" ResourceGroup="inbound_detail" ResourceName="user_def7" DataFieldValue="user_def7" />
                        <ucControls:InputTextNumber ID="InputTextNumber1" runat="server" ResourceGroup="inbound_detail" ResourceName="user_def8" DataFieldValue="user_def8" />
                        <ucControls:InputTextDate ID="InputTextBox22" runat="server" ResourceGroup="inbound_detail" ResourceName="user_def9" DataFieldValue="user_def9" TextMode="Date" />
                        <ucControls:InputTextDate ID="InputTextDate1" runat="server" ResourceGroup="inbound_detail" ResourceName="user_def10" DataFieldValue="user_def10" TextMode="Date" />
                    </ucControls:PanelControlRow>
                </ucControls:PanelControlTab>

            </ControlTemplate>
        </ucControls:PanelTab>
    </ControlTemplate>
</ucControls:PanelPopupEntity>

<ucControls:PanelPopup ID="popupBomMaster" runat="server" HeaderText="Bom Master">
    <DataTemplate>
        <ucControls:PanelControlRow ID="PanelControlRow3" runat="server">
            <%--            <ucControls:InputTextBox ID="InputTextBox3" runat="server" ResourceGroup="owner" ResourceName="owner_code" Enabled="false" />--%>

            <ucControls:InputTextBox ID="txtBomMsCode" runat="server" Text="Bom Code" Readonly="true" IsKey="true" />
            <ucControls:InputTextNumber ID="txtBomMsQty" runat="server" Text="Quantity" NumberType="Double" IsPrimary="true" ValidateGroup="BomMaster" />
        </ucControls:PanelControlRow>
        <%--        <asp:Panel ID="Panel3" runat="server">
            <table border="0" class="two_column" style="margin-top: 0px">
                <tr>
                    <td class="tb_label">
                        <ucControls:LabelExt ID="LabelExt54" runat="server" Text="Bom Code" />
                    </td>
                    <td class="tb_control"></td>
                </tr>
                <tr>
                    <td class="tb_label">
                        <ucControls:LabelExt ID="LabelExt55" runat="server" Text="Quantity" />
                    </td>
                    <td class="tb_control"></td>
                </tr>
            </table>
        </asp:Panel>--%>
        <ucControls:PanelControlRow ID="PanelControlRow4" runat="server">
            <fieldset>
                <legend>
                    <ucControls:LabelExt runat="server" Text="Items Set" />
                </legend>
                <ucControls:GridExt ID="gridBomDetail" runat="server"
                    SourceAssemblyName="WMS_NEW.Access" SourceClassName="WMS_NEW.Access.Transaction.Inbound.Bom.BomDetail" KeyField="KeyId" KeyType="Guid" DisableFirstSearch="true"
                    AutoSize="true" GridAlign="Center" PageAlign="Right">
                    <CustomSearchTemplate>
                        <ucControls:InputHidden runat="server" ID="hdf_bom_inbound_order_master_id" DataFieldValue="_inbound_order_master_id" />
                    </CustomSearchTemplate>
                    <CustomColumnTemplate>
                        <ucControls:GridColumnExt ID="GridColumnExt26" runat="server" HeaderText="Item Number" DataField="item_number" />
                        <ucControls:GridColumnExt ID="GridColumnExt28" runat="server" HeaderText="UOM" DataField="uom" />
                        <ucControls:GridColumnExt ID="GridColumnExt29" runat="server" HeaderText="Bom Qty" DataField="bom_detail_quantity" />
                    </CustomColumnTemplate>
                </ucControls:GridExt>
            </fieldset>
        </ucControls:PanelControlRow>

        <%-- <asp:Panel ID="Panel4" runat="server">
            
        </asp:Panel>--%>
    </DataTemplate>
    <CommandTemplate>
        <ucControls:ButtonExt ID="btBomMsQtyDelete" runat="server" Text="Delete Bom Set" CssClass="btn btn-danger" OnClick="btBomMsQtyDelete_Click" ValidateGroup="BomMaster" OnClientClick="if (!confirm('Do you want delete bom set ?')) return false;" />
        <ucControls:ButtonExt ID="btBomMsQtyUpdate" runat="server" Text="Update Qty" CssClass="btn btn-info" OnClick="btBomMsQtyUpdate_Click" ValidateGroup="BomMaster" OnClientClick="if (!confirm('Do you want update quantity ?')) return false;" />
    </CommandTemplate>
</ucControls:PanelPopup>


<ucControls:PanelPopup ID="pnlPopReceiveAll" runat="server" HeaderText="Receive All">
    <DataTemplate>
        <ucControls:PanelControlRow ID="PanelControlRow5" runat="server">
            <ucControls:InputDropDownHD runat="server" ResourceGroup="location" ResourceName="location" ID="ddlLocation" ValidateGroup="ReceiveAll" AutoPostBack="true" DisplayDefault="-- Please Select --" IsPrimary="true" ColumnSpan="6" />
            <ucControls:InputTextBox runat="server" ResourceGroup="inventory" ResourceName="lpn" ID="txtLPN" ValidateGroup="ReceiveAll" ColumnSpan="6" />
        </ucControls:PanelControlRow>
    </DataTemplate>
    <CommandTemplate>
        <ucControls:ButtonExt ID="btConfReceiveAll" runat="server" Text="Save" CssClass="btn btn-success" OnClick="btConfReceiveAll_Click" ValidateGroup="ReceiveAll" OnClientClick="if (!confirm('Do you want confirm receive all ?')) return false;" CausesValidation="true" />
    </CommandTemplate>
</ucControls:PanelPopup>

<ucControls:GridExt ID="GridExt1" runat="server"
    SourceAssemblyName="WMS_NEW.Access" SourceClassName="WMS_NEW.Access.Transaction.Inbound.InboundDetail" KeyField="KeyId" KeyType="Guid"
    GridAllowRowEdit="true" GridAllowRowDelete="true" DisableExport="false" DisableSearch="true" AutoSize="true" DisableFirstSearch="true" ShowAllSort="true" GridSortDefault="line_number asc">
    <CustomCommandTemplate>
        <ucControls:ButtonExt runat="server" ResourceGroup="general" ResourceName="btn_receipt" ID="btnReceipt" Text="Receipt" CssClass="btn btn-info btn-sm" CausesValidation="false" OnClick="btnReceipt_Click" Visible="true" />
        <ucControls:ButtonExt runat="server" ResourceGroup="general" ResourceName="btn_report" ID="btReport" Text="Report" CausesValidation="false" CssClass="btn btn-sm" OnClick="btReport_Click" />
        <ucControls:ButtonExt runat="server" ResourceGroup="general" ResourceName="btn_receiveall" ID="btReceiveAll" Text="Receive All" CausesValidation="false" CssClass="btn btn-sm btn-warning" OnClick="btReceiveAll_Click" />
        <%--<ucControls:ButtonExt runat="server" ResourceGroup="general" ResourceName="btn_print_all" ID="btPrintAll" Text="Print All" CausesValidation="false" CssClass="btn btn-sm" OnClick="btPrintAll_Click" />--%>
    </CustomCommandTemplate>
    <CustomSearchTemplate>
        <ucControls:InputHidden runat="server" ID="hdf_inbound_order_master_id" DataFieldValue="_inbound_order_master_id" />
    </CustomSearchTemplate>
    <CustomColumnTemplate>
        <%--<ucControls:GridColumnExt runat="server" ResourceGroup="general" ResourceName="btn_print" ID="GridColumnExt18" HeaderText="Print" DataField="Print" ControlType="CommandButton" CommandText="Print" CommandName="PRINT" IsConfirm="false" />--%>
        <%--<ucControls:GridColumnExt runat="server" ResourceGroup="inbound_master" ResourceName="inbound_order_number" DataField="inbound_order_number" />--%>
        <ucControls:GridColumnExt runat="server" ResourceGroup="general" ResourceName="line_number" DataField="line_number" FormatType="Integer" />
        <ucControls:GridColumnExt runat="server" ResourceGroup="inventory" HeaderText="Attribute 1" ResourceName="attribute1" DataField="attribute1" />
        <ucControls:GridColumnExt runat="server" ResourceGroup="category" ResourceName="description" DataField="category_description" />
        <ucControls:GridColumnExt runat="server" ResourceGroup="item" ResourceName="item_number" DataField="item_number" />
        <ucControls:GridColumnExt runat="server" ResourceGroup="item" ResourceName="description" DataField="item_description" />
        <ucControls:GridColumnExt runat="server" ResourceGroup="inventory" ResourceName="lpn" DataField="lpn" />
        <ucControls:GridColumnExt runat="server" HeaderText="Bom Master" DataField="EditBOM" ControlType="CommandButton" CommandText="Bom Master" CommandName="EDITBOM" IsConfirm="false" ResourceGroup="" ResourceName="" />

        <ucControls:GridColumnExt runat="server" ResourceGroup="inventory" ResourceName="lot_number" DataField="lot_number" />
        <ucControls:GridColumnExt runat="server" ResourceGroup="inventory" ResourceName="mfg_date" DataField="mfg_date" FormatType="Date" />
        <ucControls:GridColumnExt runat="server" ResourceGroup="inventory" ResourceName="expiry_date" DataField="expiry_date" FormatType="Date" />
        <%--<ucControls:GridColumnExt runat="server" ResourceGroup="inventory" ResourceName="serial_number" DataField="serial_number" />--%>
        <%--<ucControls:GridColumnExt runat="server" ResourceGroup="inventory" ResourceName="attribute2" DataField="attribute2" />
        <ucControls:GridColumnExt runat="server" ResourceGroup="inventory" ResourceName="attribute3" DataField="attribute3" />
        <ucControls:GridColumnExt runat="server" ResourceGroup="inventory" ResourceName="attribute4" DataField="attribute4" />
        <ucControls:GridColumnExt runat="server" ResourceGroup="inventory" ResourceName="attribute5" DataField="attribute5" />--%>
        <ucControls:GridColumnExt runat="server" ResourceGroup="inbound_detail" ResourceName="alter_quantity_order" DataField="alter_quantity_order" FormatType="Number" />
        <ucControls:GridColumnExt runat="server" ResourceGroup="inbound_detail" ResourceName="alter_quantity_receive" DataField="alter_quantity_receive" FormatType="Number" />
        <ucControls:GridColumnExt runat="server" ResourceGroup="inbound_detail" ResourceName="alter_quantity_remain" DataField="alter_quantity_remain" FormatType="Number" />
        <ucControls:GridColumnExt runat="server" ResourceGroup="inbound_detail" ResourceName="alter_uom" DataField="alter_uom" />
        <ucControls:GridColumnExt runat="server" ResourceGroup="inbound_detail" ResourceName="quantity_order" DataField="quantity_order" FormatType="Number" />
        <ucControls:GridColumnExt runat="server" ResourceGroup="inbound_detail" ResourceName="quantity_receive" DataField="quantity_receive" FormatType="Number" />
        <ucControls:GridColumnExt runat="server" ResourceGroup="inbound_detail" ResourceName="quantity_remain" DataField="quantity_remain" FormatType="Number" />
        <ucControls:GridColumnExt runat="server" ResourceGroup="item_uom" ResourceName="uom" DataField="uom" />
        <%--<ucControls:GridColumnExt runat="server" ResourceGroup="inbound_detail" ResourceName="over_receipt_allowed" DataField="over_receipt_allowed" />
        <ucControls:GridColumnExt runat="server" ResourceGroup="inbound_detail" ResourceName="over_receipt_percentage" DataField="over_receipt_percentage" FormatType="Number" />--%>
        <%--<ucControls:GridColumnExt runat="server" ResourceGroup="item" ResourceName="price" DataField="price" />
        <ucControls:GridColumnExt runat="server" ResourceGroup="item" ResourceName="grade" DataField="grade" />--%>
        <ucControls:GridColumnExt runat="server" ResourceGroup="inbound_detail" ResourceName="default_item_status" DataField="default_item_status" />
        <ucControls:GridColumnExt runat="server" ResourceGroup="general" ResourceName="create_by" DataField="create_by" />
        <ucControls:GridColumnExt runat="server" ResourceGroup="general" ResourceName="create_date" DataField="create_date" FormatType="DateTime" />
    </CustomColumnTemplate>
</ucControls:GridExt>
