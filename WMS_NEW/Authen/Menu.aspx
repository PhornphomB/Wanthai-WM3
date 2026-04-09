<%@ Page Title="" Language="C#" MasterPageFile="~/Layout.Master" AutoEventWireup="true" CodeBehind="Menu.aspx.cs" Inherits="WMS_NEW.Authen.Menu" %>

<%@ Register Src="~/Authen/AscxControls/ucResource.ascx" TagPrefix="ucControls" TagName="ucResource" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">

    <ucControls:PanelPopupEntity ID="popup1" runat="server" ColumnControlFix="3">
        <ControlTemplate>
            <ucControls:PanelControlRow ID="pnData" runat="server">
                <ucControls:InputHidden ID="hidApplicationID" runat="server" DataFieldValue="app_id" />
                <ucControls:InputCheckBox ID="chkActive" runat="server" DataFieldValue="is_active" CheckBoxType="String" ColumnSpan="1" ResourceGroup="general" ResourceName="is_active" />
                <ucControls:InputDropDown ID="ddPlatform" runat="server" DataFieldValue="platform" ResourceGroup="menu" ResourceName="platform" IsPrimary="true" ComboType="String" DisplayDefault="--Select--" LabelText="Platform" AutoPostBack="true" ControlGroup="DDL_GRB_1" ControlSequence="0" ColumnSpan="3" />
                <ucControls:InputDropDownHD ID="ddParentName" runat="server" DataFieldValue="parent_menu_id" ResourceGroup="menu" ResourceName="parent_menu_id" ComboType="String" DisplayDefault="--No Select--" LabelText="Parent Menu Name" AutoPostBack="true" ControlGroup="DDL_GRB_1" ControlSequence="1" ColumnSpan="4" />
                <ucControls:InputTextBox ID="InputTextBox3" runat="server" DataFieldValue="menu_name" ResourceGroup="menu" ResourceName="menu_name" IsPrimary="true" LabelText="Menu Name" ColumnSpan="4" />
                <ucControls:InputTextInteger ID="txtMenuGroupSeq" runat="server" DataFieldValue="menu_group_sequence" ResourceGroup="menu" ResourceName="menu_group_sequence" IsPrimary="true" MaxLength="2" LabelText="Group Sequence" ColumnSpan="3" />
                <ucControls:InputTextInteger ID="txtMenuSeq" runat="server" DataFieldValue="menu_sequence" ResourceGroup="menu" ResourceName="menu_sequence" IsPrimary="true" MaxLength="2" LabelText="Menu Sequence" ColumnSpan="3" />
                <ucControls:InputTextBox ID="InputTextBox4" runat="server" DataFieldValue="process" ResourceGroup="menu" ResourceName="process" IsMultiLine="true" LabelText="Link Url" ColumnSpan="6" />
            </ucControls:PanelControlRow>

            <ucControls:PanelTab ID="PanelTab1" runat="server">
                <ControlTemplate>
                    <ucControls:PanelControlTab ID="PanelControlTab1" runat="server" PanelName="Resource">
                        <ucControls:ucResource runat="server" ID="ucResource" />
                    </ucControls:PanelControlTab>
                </ControlTemplate>
            </ucControls:PanelTab>
        </ControlTemplate>

    </ucControls:PanelPopupEntity>

    <ucControls:GridExt ID="GridExt1" runat="server"
        SourceAssemblyName="SecurityM.Access" SourceClassName="SecurityM.Access.Master.Menu" KeyField="KeyID" KeyType="String" GridAllowRowEdit="true" GridAllowRowDelete="true">
        <CustomSearchTemplate>
            <ucControls:InputHidden ID="hid_is_admin" runat="server" DataFieldValue="_is_admin" />
            <ucControls:InputHidden ID="hid_app_id" runat="server" DataFieldValue="_app_id" />
        </CustomSearchTemplate>
        <CustomColumnTemplate>
            <ucControls:GridColumnExt ID="GridColumnExt3" runat="server" HeaderText="Menu Name" DataField="menu_name" ResourceGroup="menu" ResourceName="menu_name" FilterIndex="3" AllowSort="true" AllowFilter="true" ShowFilterNow="true" />
            <ucControls:GridColumnExt ID="GridColumnExt4" runat="server" HeaderText="Platform" DataField="platform" ResourceGroup="menu" ResourceName="platform" FilterIndex="2" AllowSort="true" AllowFilter="true" ShowFilterNow="true" UseFilterDropDown="true" DropDownDisplayDefault="--All-" />
            <ucControls:GridColumnExt ID="GridColumnExt8" runat="server" HeaderText="Parent Name" DataField="parent_menu_name" ResourceGroup="menu" ResourceName="parent_menu_name" AllowSort="true" AllowFilter="true" ShowFilterNow="true" />
            <ucControls:GridColumnExt ID="GridColumnExt5" runat="server" HeaderText="Menu Group Seq" DataField="menu_group_sequence" ResourceGroup="menu" ResourceName="menu_group_sequence" FormatType="Integer" AllowSort="true" AllowFilter="true" ShowFilterNow="true" />
            <ucControls:GridColumnExt ID="GridColumnExt9" runat="server" HeaderText="Menu Sequence" DataField="menu_sequence" ResourceGroup="menu" ResourceName="menu_sequence" FormatType="Integer" AllowSort="true" AllowFilter="true" ShowFilterNow="true" />
            <ucControls:GridColumnExt ID="GridColumnExt10" runat="server" HeaderText="Process" DataField="process" ResourceGroup="menu" ResourceName="process" />
            <ucControls:GridColumnExt ID="GridColumnExt15" runat="server" HeaderText="Active" DataField="is_active" ResourceGroup="general" ResourceName="is_active" AllowSort="true" />
            <ucControls:GridColumnExt ID="GridColumnExt6" runat="server" HeaderText="Create By" DataField="create_by" ResourceGroup="general" ResourceName="create_by" AllowSort="true" />
            <ucControls:GridColumnExt ID="GridColumnExt7" runat="server" HeaderText="Create Date" DataField="create_date" ResourceGroup="general" ResourceName="create_date" FormatType="DateTime" AllowSort="true" />
        </CustomColumnTemplate>
    </ucControls:GridExt>

</asp:Content>
