<%@ Page Title="" Language="C#" MasterPageFile="~/Layout.Master" AutoEventWireup="true" CodeBehind="UserLogon.aspx.cs" Inherits="WMS_NEW.Authen.UserLogon" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">

    <ucControls:GridExt ID="GridExt1" runat="server"
        SourceAssemblyName="SecurityM.Access" SourceClassName="SecurityM.Access.Config.UserLogon" KeyField="KeyID" KeyType="String"
        GridSortDefault="is_logon desc,last_login_date desc" GridAllowSelectBox="true">
        <CustomCommandTemplate>
             <ucControls:ButtonExt ID="btClear" runat="server" Text="Clear Logon" CssClass="btn btn-sm btn-success" CausesValidation="false" OnClick="btClear_Click" />
        </CustomCommandTemplate>
        <CustomSearchTemplate>
            <ucControls:InputHidden ID="hidSessionAppID" runat="server" DataFieldValue="_app_id" />
        </CustomSearchTemplate>
        <CustomColumnTemplate>
            <ucControls:GridColumnExt ID="GridColumnExt1" runat="server" HeaderText="Username" DataField="user_id" AllowFilter="true" ShowFilterNow="true" AllowSort="true" ResourceGroup="user" ResourceName="user_id" />
            <ucControls:GridColumnExt ID="GridColumnExt2" runat="server" HeaderText="Device/IP" DataField="device" AllowFilter="true" ShowFilterNow="true" AllowSort="true" ResourceGroup="user" ResourceName="device"/>
            <ucControls:GridColumnExt ID="GridColumnExt5" runat="server" HeaderText="Is Logon" DataField="is_logon" ShowFilterNow="true" AllowFilter="true" AllowSort="true" UseFilterDropDown="true" DropDownDisplayDefault="--All--" ResourceGroup="user" ResourceName="is_logon" />
            <ucControls:GridColumnExt ID="GridColumnExt3" runat="server" HeaderText="Full Name" DataField="full_name" AllowFilter="true" AllowSort="true" ResourceGroup="user" ResourceName="full_name"/>
            <ucControls:GridColumnExt ID="GridColumnExt7" runat="server" HeaderText="Last Login Date" DataField="last_login_date" FormatType="DateTime" FilterFormatType="Date" AllowFilter="true" AllowSort="true" ResourceGroup="user" ResourceName="last_login_date" />
        </CustomColumnTemplate>
    </ucControls:GridExt>

</asp:Content>
