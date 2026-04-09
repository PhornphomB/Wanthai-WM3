<%@ Page Title="" Language="C#" MasterPageFile="~/Layout.Master" AutoEventWireup="true" CodeBehind="UserOwner.aspx.cs" Inherits="WMS_NEW.MasterData.Mapping.UserOwner" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <ucControls:GridExt ID="GridExt1" runat="server"
        SourceAssemblyName="WMS_NEW.Access" SourceClassName="WMS_NEW.Access.MasterData.Mapping.UserOwner" KeyField="KeyId" KeyType="String" KeyFieldSelect="is_active"
        ColumnFreezeLength="0" GridSortDefault="owner_code" GridAllowRowEdit="false" GridAllowRowDelete="false"
        AutoGenerateColumn="true" ShowAllSort="true" ShowAllFilter="false" GridAllowSelectBox="true" DisableFirstSearch="true" AutoGenColumnFields_Exclude="is_active">
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
            <ucControls:GridColumnExt runat="server" ResourceGroup="user" ResourceName="user" ID="iColUser" DataField="user_id" AllowFilter="true" ShowFilterNow="true" FilterPrimary="true" FilterFormatType="Text" UseFilterDropDown="true" DropDownFilterType="LazySearch" DropDownDisplayDefault="--Select--" DropDownAutoPostBack="true" />
            <ucControls:GridColumnExt runat="server" ResourceGroup="owner" ResourceName="owner_code" DataField="owner_code" AllowFilter="false" AllowSort="true" ShowFilterNow="false"  />
            <ucControls:GridColumnExt runat="server" ResourceGroup="general" ResourceName="map_status" ID="iColActive" DataField="map_status" AllowFilter="true" ShowFilterNow="true" FilterFormatType="Text" UseFilterDropDown="true" DropDownFilterType="Normal" DropDownDisplayDefault="-- All --" />
        </CustomColumnTemplate>
    </ucControls:GridExt>
</asp:Content>
