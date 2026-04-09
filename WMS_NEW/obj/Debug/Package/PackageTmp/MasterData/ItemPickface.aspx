<%@ Page Title="" Language="C#" MasterPageFile="~/Layout.Master" AutoEventWireup="true" CodeBehind="ItemPickface.aspx.cs" Inherits="WMS_NEW.MasterData.ItemPickface" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <asp:UpdatePanel ID="UpdatePanel2" runat="server" UpdateMode="Conditional">
        <ContentTemplate>
            <div class="background-base row">
                <div class="col-lg-3">
                    <ucControls:InputDropDown ID="ddlWarehouse" runat="server" IsPrimary="true" AutoPostBack="true" LabelText="Warehouse ID" ControlGroup="WH_GRB" ControlSequence="1" ResourceGroup="warehouse" ResourceName="wh_id" ValidationGroup="ValidateGroup" />
                </div>
                <div class="col-lg-4">
                    <ucControls:InputDropDownHD ID="ddlItemNumber" runat="server" IsPrimary="true" LabelText="Item ID" ResourceGroup="item" ResourceName="item_number" ValidationGroup="ValidateGroup" DisplayDefault="--SELECT--" AutoPostBack="true" />
                </div>
                <div class="w-100"></div>
                <div class="col-lg-1">
                    <ucControls:ButtonExt ID="btnSearch" runat="server" Text="Search" CssClass="btn btn-success" OnClick="btnSearch_Click" ResourceGroup="pickface" ResourceName="btn_search" ValidationGroup="ValidateGroup" />
                </div>
                <div class="col-lg-1">
                    <ucControls:ButtonExt ID="btnReplenishment" runat="server" Text="Replenishment" CssClass="btn btn-warning" OnClick="btnReplenishment_Click" OnClientClick="if (!confirm('Do you want to confirm ?')) return false;" ResourceGroup="pickface" ResourceName="btn_replenishment" CausesValidation="false" />
                </div>
                <div class="w-100"></div>
                <br />
                <div class="col-lg-6">
                    <div class="card">
                        <div style="padding: 20px 10px;">
                            <span class="text-uppercase text-danger text-bold text-center mb-2" style="padding: 2px 10px;">
                                <ucControls:LabelExt ID="lblDummy1" runat="server" DefaultText="Location Pickface" CssClass="text-uppercase text-danger text-bold text-center" ResourceGroup="item" ResourceName="panel_location_pickface" />
                            </span>
                        </div>

                        <ucControls:GridExt ID="gridLocationPickface" runat="server"
                            SourceAssemblyName="WMS_NEW.Access" SourceClassName="WMS_NEW.Access.MasterData.ItemPickface" GridAllowSelectBox="true"
                            KeyField="KeyID" KeyType="Guid" DisableButtonSearch="true" AutoSize="true" DisableExport="true" DisableFirstSearch="true" AutoGenColumnIndexs_SORT="loc_type_name">
                            <CustomCommandTemplate>
                                <ucControls:ButtonExt ID="btnAddLocation" runat="server" Text="ADD" CssClass="btn btn-sm btn-success" CausesValidation="false" ResourceGroup="General" ResourceName="ADD" OnClick="btnAddLocation_Click" />
                            </CustomCommandTemplate>
                            <CustomSearchTemplate>
                                <ucControls:InputHidden ID="hidSessionUserItOrder" runat="server" DataFieldValue="_userID" />
                                <ucControls:InputHidden ID="hidWhMasterId" runat="server" DataFieldValue="_wh_master_id" />
                                <ucControls:InputHidden ID="hidWhItemMasterId" runat="server" DataFieldValue="_wh_item_master_id" />
                            </CustomSearchTemplate>
                            <CustomColumnTemplate>
                                <ucControls:GridColumnExt runat="server" ResourceGroup="warehouse" ResourceName="wh_id" ID="iColWarehouse" DataField="wh_id" DataFieldFilter="wh_master_id" FilterFormatType="Guid" UseFilterDropDown="false" DropDownDisplayDefault="--All--" />
                                <ucControls:GridColumnExt runat="server" ResourceGroup="location" ResourceName="loc_type" ID="iColLocType" DataField="loc_type_name" DataFieldFilter="loc_type" FilterFormatType="Text" UseFilterDropDown="false" DropDownDisplayDefault="--All--" />
                                <ucControls:GridColumnExt ID="GridColumnExt1" runat="server" HeaderText="location" DataField="location" />
                            </CustomColumnTemplate>
                        </ucControls:GridExt>
                    </div>
                </div>
                <div class="col-lg-6">
                    <div class="card">
                        <div style="padding: 20px 10px;">
                            <span class="text-uppercase text-danger text-bold text-center mb-2" style="padding: 2px 10px;">
                                <ucControls:LabelExt ID="LabelExt1" runat="server" DefaultText="Item Pickface" CssClass="text-uppercase text-danger text-bold text-center" ResourceGroup="item" ResourceName="panel_item_pickface" />
                            </span>
                        </div>
                        <ucControls:GridExt ID="gridLocationPickfaceItem" runat="server"
                            SourceAssemblyName="WMS_NEW.Access" SourceClassName="WMS_NEW.Access.MasterData.LocationPickfaceItem" GridAllowSelectBox="true"
                            KeyField="KeyID" KeyType="String" DisableButtonSearch="true" DisableExport="true" AutoSize="true" DisableFirstSearch="true" OnGridRowTextChanged="gridLocationPickfaceItem_GridRowTextChanged">
                            <CustomCommandTemplate>
                                <ucControls:ButtonExt ID="btnDeleteLocationItem" runat="server" Text="DELETE" CssClass="btn btn-sm btn-danger" CausesValidation="false" ResourceGroup="General" ResourceName="DELETE" OnClick="btnDeleteLocationItem_Click" />
                            </CustomCommandTemplate>
                            <CustomSearchTemplate>
                                <ucControls:InputHidden ID="hidSessionUserItInvent" runat="server" DataFieldValue="_userID" />
                                <ucControls:InputHidden ID="hidWhMasterIdItem" runat="server" DataFieldValue="_wh_master_id" />
                                <ucControls:InputHidden ID="hidItemMasterLocPickItem" runat="server" DataFieldValue="_wh_item_master_id" />
                            </CustomSearchTemplate>
                            <CustomColumnTemplate>
                                <ucControls:GridColumnExt runat="server" ResourceGroup="warehouse" ResourceName="wh_id" ID="GridColumnExt2" DataField="wh_id" DataFieldFilter="wh_master_id" FilterFormatType="Guid" UseFilterDropDown="false" DropDownDisplayDefault="--All--" />
                                <%--    <ucControls:GridColumnExt runat="server" ResourceGroup="location" ResourceName="loc_type" ID="GridColumnExt3" DataField="loc_type_name" DataFieldFilter="loc_type" FilterFormatType="Text" UseFilterDropDown="false" DropDownDisplayDefault="--All--" />--%>
                                <ucControls:GridColumnExt ID="GridColumnExt4" runat="server" HeaderText="location" DataField="location" />
                                <ucControls:GridColumnExt ID="GridColumnExt25" runat="server" HeaderText="Max" DataField="max" Width="90" ControlType="Text" InputTextAutoPostBack="true" FormatType="Number" ResourceGroup="item" ResourceName="max" />
                                <ucControls:GridColumnExt ID="GridColumnExt3" runat="server" HeaderText="Min" DataField="min" Width="90" ControlType="Text" InputTextAutoPostBack="true" FormatType="Number" ResourceGroup="item" ResourceName="min" />
                            </CustomColumnTemplate>
                        </ucControls:GridExt>
                    </div>
                </div>
            </div>
        </ContentTemplate>
    </asp:UpdatePanel>
</asp:Content>
