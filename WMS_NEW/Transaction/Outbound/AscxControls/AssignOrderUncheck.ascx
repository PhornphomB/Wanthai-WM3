<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="AssignOrderUncheck.ascx.cs" Inherits="WMS_NEW.Transaction.Outbound.AscxControls.AssignOrderUncheck" %>

<ucControls:GridExt ID="GridExt1" runat="server"
    SourceAssemblyName="WMS_NEW.Access" SourceClassName="WMS_NEW.Access.Transaction.Outbound.AssignOrderUncheck" KeyField="KeyId" KeyType="String"
    AutoGenerateColumn="false" ShowAllSort="true" ShowAllFilter="true" GridSortDefault="door" >
    <CustomCommandTemplate>        
        <ucControls:ButtonExt runat="server" ResourceGroup="AssignOrder" ResourceName="uncheck_all" ID="btUncheckAll" Text="Uncheck All" CausesValidation="false" CssClass="btn btn-sm btn-warning" OnClick="btUncheckAll_Click" Visible="false" />        
    </CustomCommandTemplate>
    <CustomSearchTemplate>
        <ucControls:InputHidden ID="hidWhMasterId" runat="server" DataFieldValue="_wh_master_id" />
        <ucControls:InputHidden ID="hidWId" runat="server" DataFieldValue="_wh_id" />
        <ucControls:InputHidden ID="hidOwnerID" runat="server" DataFieldValue="_owner_id" />
        <ucControls:InputHidden ID="hidOwnerCode" runat="server" DataFieldValue="_owner_code" />
        <ucControls:InputHidden ID="hidOrderMasterId" runat="server" DataFieldValue="_outbound_order_master_id" />
        <ucControls:InputHidden ID="hidOrderOrderNumber" runat="server" DataFieldValue="_outbound_order_number" />
        <ucControls:InputHidden ID="hidOrderStatus" runat="server" DataFieldValue="_order_status" />
    </CustomSearchTemplate>
    <CustomColumnTemplate>
        <ucControls:GridColumnExt ID="GridColumnBtnUncheck" runat="server" DataField="outbound_pick_detail_id" ControlType="CommandButton" CommandText="Uncheck" CommandName="Uncheck" ResourceGroup="AssignOrder" ResourceName="ButtonUncheck" />   
        
        <ucControls:GridColumnExt runat="server" ID="GridColumnExt1" HeaderText="Door" DataField="door" AllowFilter="true" ShowFilterNow="true" AllowSort="true" ResourceGroup="CarRegister" ResourceName="door" />
        <ucControls:GridColumnExt runat="server" ID="GridColumnExt8" HeaderText="Pallet No." DataField="lpn" AllowFilter="true" ShowFilterNow="true" AllowSort="true" ResourceGroup="AssignOrder" ResourceName="lpn" />
        <ucControls:GridColumnExt runat="server" ID="GridColumnExt5" HeaderText="Check Date" DataField="check_date" AllowFilter="true" AllowSort="true" ResourceGroup="AssignOrder" ResourceName="check_date" FormatType="DateTime" />
        <ucControls:GridColumnExt runat="server" ID="GridColumnExt3" HeaderText="License" DataField="license" AllowFilter="true" AllowSort="true" ResourceGroup="CarRegister" ResourceName="license" />
        <ucControls:GridColumnExt runat="server" ID="GridColumnExt2" HeaderText="Truck Type" DataField="truck_type" AllowFilter="true" AllowSort="true" ResourceGroup="CarRegister" ResourceName="truck_type" />        
        <ucControls:GridColumnExt runat="server" ID="GridColumnExt4" HeaderText="Head/Tail" DataField="head_tail" AllowFilter="true" AllowSort="true" ResourceGroup="CarRegister" ResourceName="head_tail" />      

    </CustomColumnTemplate>
</ucControls:GridExt>