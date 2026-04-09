<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="AssignOrderDetail.ascx.cs" Inherits="WMS_NEW.Transaction.Outbound.AscxControls.AssignOrderDetail" %>

<ucControls:GridExt ID="GridExt1" runat="server"
    SourceAssemblyName="WMS_NEW.Access" SourceClassName="WMS_NEW.Access.Transaction.Outbound.AssignOrderDetailSourceStored" KeyField="KeyId" KeyType="String"
    AutoGenerateColumn="false" ShowAllSort="true" ShowAllFilter="true" GridAllowRowEdit="false" GridSortDefault="door" GridAllowSelectBox="true" KeyFieldSelect="chk_is_checked" >
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
        <ucControls:GridColumnExt ID="GridColumnBtnUncheck" runat="server" DataField="truck_manifest_id" ControlType="CommandButton" CommandText="Uncheck All" CommandName="Uncheck All" ResourceGroup="AssignOrder" ResourceName="button_uncheck_all" />
        <%--<ucControls:GridColumnExt runat="server" ID="GridColumnExt8" HeaderText="Select" DataField="is_checked" />--%>
        <ucControls:GridColumnExt runat="server" ID="GridColumnExt1" HeaderText="Door" DataField="door" AllowFilter="true" ShowFilterNow="true" AllowSort="true" ResourceGroup="CarRegister" ResourceName="door" />
        <ucControls:GridColumnExt runat="server" ID="GridColumnExt2" HeaderText="Truck Type" DataField="truck_type" AllowFilter="true" AllowSort="true" ResourceGroup="CarRegister" ResourceName="truck_type" />
        <ucControls:GridColumnExt runat="server" ID="GridColumnExt3" HeaderText="License" DataField="license" AllowFilter="true" AllowSort="true" ResourceGroup="CarRegister" ResourceName="license" />
        <ucControls:GridColumnExt runat="server" ID="GridColumnExt4" HeaderText="Head/Tail" DataField="head_tail" AllowFilter="true" AllowSort="true" ResourceGroup="CarRegister" ResourceName="head_tail" />
        <ucControls:GridColumnExt runat="server" ID="GridColumnExt5" HeaderText="Assigned Date" DataField="assgin_date" AllowFilter="true" AllowSort="true" ResourceGroup="AssignOrder" ResourceName="assgin_date" FormatType="DateTime" />
        <ucControls:GridColumnExt runat="server" ID="GridColumnExt8" HeaderText="Load Status" DataField="load_status" AllowFilter="true" AllowSort="true" ResourceGroup="CarRegister" ResourceName="load_status" />

        <ucControls:GridColumnExt runat="server" ID="GridColumnExt6" HeaderText="location_id" DataField="location_id" Visible="false"  />
        <ucControls:GridColumnExt runat="server" ID="GridColumnExt7" HeaderText="Door" DataField="is_allow_check" Visible="false" />
    </CustomColumnTemplate>
</ucControls:GridExt>
