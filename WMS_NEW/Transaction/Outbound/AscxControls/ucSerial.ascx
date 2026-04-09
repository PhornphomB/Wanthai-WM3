<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="ucSerial.ascx.cs" Inherits="WMS_NEW.Transaction.Outbound.AscxControls.ucSerial" %>

<ucControls:PanelPopup runat="server" ID="popSerial" StyleSize="Small">
    <CommandTemplate>
        <ucControls:ButtonExt runat="server" ID="btnCancelSerial" ResourceGroup="general_ex" ResourceName="cancel_serial" OnClick="btnCancelSerial_Click" CssClass="btn btn-sm btn-danger" />
    </CommandTemplate>
    <DataTemplate>
        <ucControls:GridExt ID="gridSerial" runat="server"
            SourceAssemblyName="WMS_NEW.Access" SourceClassName="WMS_NEW.Access.Transaction.Outbound.PickSerial" KeyField="serial_number" KeyType="String"
            DisableExport="true" DisableSearch="true" AutoSize="true" DisableFirstSearch="true" GridSortDefault="serial_number">
            <CustomSearchTemplate>
                <ucControls:InputHidden runat="server" ID="hidPickDetail" DataFieldValue="_outbound_pick_detail_id" />
            </CustomSearchTemplate>
            <CustomColumnTemplate>
                <ucControls:GridColumnExt ID="GridColumnExt1" runat="server" DataField="select_serial" ResourceGroup="general_ex" ResourceName="select_serial" ControlType="CommandButton" CommandName="select_serial" CommandText="Select" />
                <ucControls:GridColumnExt ID="GridColumnExt21" runat="server" DataField="serial_number" ResourceGroup="inventory" ResourceName="serial_number" AllowFilter="true" AllowSort="true" ShowFilterNow="true" />
            </CustomColumnTemplate>
        </ucControls:GridExt>
    </DataTemplate>
</ucControls:PanelPopup>


