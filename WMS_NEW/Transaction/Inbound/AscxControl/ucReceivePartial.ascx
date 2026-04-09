<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="ucReceivePartial.ascx.cs" Inherits="WMS_NEW.Transaction.Inbound.AscxControl.ucReceivePartial" %>
<asp:UpdatePanel ID="updateCloseReceipt" runat="server" UpdateMode="Conditional" ChildrenAsTriggers="true">
    <ContentTemplate>

        <div class="form-group row">
            <div class="col-sm-4">
                <ucControls:InputDropDown runat="server" ResourceGroup="receipt_header" ResourceName="receipt_number" ID="ddlReceiptNo" IsPrimary="true" AutoPostBack="true" DisplayDefault="--Select--" />
                <ucControls:InputTextBox runat="server" ResourceGroup="receipt_header" ResourceName="receipt_status" ID="txtStatus" Enabled="false" />
                <ucControls:ButtonExt ID="btnCloseReceipt" runat="server" Text="Close Receipt" CssClass="btn btn-warning" OnClick="btnCloseReceipt_Click" OnClientClick="if (!confirm('
                                    ')) return false;"
                    ResourceGroup="General" ResourceName="CloseReceipt" BaseContentCss="col-sm-6" />
                <ucControls:ButtonExt ID="btnSaveReceipt" runat="server" Text="Save Receipt" CssClass="btn btn-info" OnClick="btnSaveReceipt_Click" OnClientClick="if (!confirm('Do you want to confirm ?')) return false;" ResourceGroup="general" ResourceName="btn_save_receipt" BaseContentCss="col-sm-6" />
            </div>
            <div class="col-sm-4">
                <ucControls:InputTextBox ID="txtCreateBy" runat="server" DataFieldValue="create_by" Enabled="false" ResourceGroup="general" ResourceName="create_by" />
                <ucControls:InputTextDate ID="txtCreateDate" runat="server" DataFieldValue="create_date" Enabled="false" ResourceGroup="general" ResourceName="create_date" TextMode="Date" />
            </div>
            <div class="col-sm-4">
                <ucControls:InputTextBox ID="txtCloseBy" runat="server" DataFieldValue="close_by" Enabled="false" ResourceGroup="general" ResourceName="close_by" />
                <ucControls:InputTextDate ID="txtCloseDate" runat="server" DataFieldValue="close_date" Enabled="false" TextMode="Date" ResourceGroup="general" ResourceName="close_date" />
            </div>

        </div>
    </ContentTemplate>
</asp:UpdatePanel>

<ucControls:PanelPopup ID="popupCloseReceipt" runat="server" HeaderText="Close Receipt" StyleSize="Small">
    <DataTemplate>
        <div class="row">
            <div class="col-sm-12">
                <asp:Label ID="lblValidateCloseOrder" runat="server"></asp:Label>
            </div>
        </div>
    </DataTemplate>
    <CommandTemplate>
        <span style="padding-left: 5px;">
            <ucControls:ButtonExt ID="btComfirm" ResourceGroup="general" ResourceName="btn_confirm" runat="server" Text="Confirm Close Receipt" CssClass="btn btn-warning" OnClick="btComfirmClose_Click" OnClientClick="if (!confirm('Do you want to confirm ?')) return false;" />
        </span>
    </CommandTemplate>
</ucControls:PanelPopup>

<ucControls:PanelTab ID="PanelTab1" runat="server">
    <ControlTemplate>
        <ucControls:PanelControlTab ID="PanelControlTab1" runat="server" PanelName="Partail Receive" ResourceGroup="partail_receive" ResourceName="tab_partial_receive">
            <ucControls:GridExt ID="GridExtSummary" runat="server"
                SourceAssemblyName="WMS_NEW.Access" SourceClassName="WMS_NEW.Access.Transaction.Inbound.Receipt.PartialReceiptDetailSummary" KeyField="KeyId" KeyType="String"
                GridAllowRowEdit="false" GridAllowRowDelete="false" DisableSearch="true" AutoSize="true" DisableFirstSearch="true" OnGridRefreshClick="btRefreshDetailSummary_Click">
                <CustomCommandTemplate>
                    <ucControls:ButtonExt ID="btRefreshDetailSummary" runat="server" Text="Refresh" CssClass="btn btn-sm btn-info" CausesValidation="false" OnClick="btRefreshDetailSummary_Click" ResourceGroup="General" ResourceName="Refresh" />
                </CustomCommandTemplate>
                <CustomSearchTemplate>
                    <ucControls:InputHidden ID="hdfreceipt_header_id_summary" runat="server" DataFieldValue="_receipt_header_id" />
                </CustomSearchTemplate>
                <CustomColumnTemplate>
                    <ucControls:GridColumnExt ID="GridColumnExt10" runat="server" DataField="line_number" AllowSort="true" ResourceGroup="general" ResourceName="line_number" />
                    <ucControls:GridColumnExt ID="GridColumnExt13" runat="server" DataField="category_description" AllowSort="true" ResourceGroup="category" ResourceName="description" />
                    <ucControls:GridColumnExt ID="GridColumnExt12" runat="server" DataField="description" AllowSort="true" ResourceGroup="item" ResourceName="description" />
                    <ucControls:GridColumnExt ID="GridColumnExt11" runat="server" DataField="item_number" AllowSort="true" ResourceGroup="item" ResourceName="item_number" />
                    <%--<ucControls:GridColumnExt ID="GridColumnExt14" runat="server" DataField="grade" AllowSort="true" ResourceGroup="item" ResourceName="grade" />
                    <ucControls:GridColumnExt ID="GridColumnExt15" runat="server" DataField="price" AllowSort="true" ResourceGroup="item" ResourceName="price" />--%>
                    <ucControls:GridColumnExt ID="GridColumnExt16" runat="server" DataField="quantity_order" AllowSort="true" ResourceGroup="inbound_detail" ResourceName="quantity_order" />
                    <ucControls:GridColumnExt ID="GridColumnExt17" runat="server" DataField="quantity_received" AllowSort="true" ResourceGroup="inbound_detail" ResourceName="quantity_received" />
                    <ucControls:GridColumnExt ID="GridColumnExt18" runat="server" DataField="uom" AllowSort="true" ResourceGroup="item_uom" ResourceName="uom" />
                </CustomColumnTemplate>
            </ucControls:GridExt>
        </ucControls:PanelControlTab>

        <ucControls:PanelControlTab ID="PanelControlTab2" runat="server" PanelName="Partail Receipt Detail" ResourceGroup="partail_receive" ResourceName="tab_partial_receive_detail">
            <ucControls:GridExt ID="GridExtDetail" runat="server"
                SourceAssemblyName="WMS_NEW.Access" SourceClassName="WMS_NEW.Access.Transaction.Inbound.Receipt.PartialReceiptDetail" KeyField="KeyId" KeyType="String"
                DisableExport="false" DisableSearch="true" AutoSize="true" DisableFirstSearch="true" OnGridRefreshClick="btRefreshDetail_Click">
                <CustomCommandTemplate>
                    <ucControls:ButtonExt ID="btRefreshDetail" runat="server" Text="Refresh" CssClass="btn btn-info btn-sm" CausesValidation="false" OnClick="btRefreshDetail_Click" ResourceGroup="General" ResourceName="Refresh" />
                </CustomCommandTemplate>
                <CustomSearchTemplate>
                    <ucControls:InputHidden ID="hdfreceipt_header_id" runat="server" DataFieldValue="_receipt_header_id" />
                </CustomSearchTemplate>
                <CustomColumnTemplate>
                    <ucControls:GridColumnExt ID="GridColumnExt1" runat="server" DataField="line_number" AllowSort="true" ResourceGroup="general" ResourceName="line_number" />
                    <ucControls:GridColumnExt ID="GridColumnExt4" runat="server" DataField="category_description" AllowSort="true" ResourceGroup="category" ResourceName="description" />
                    <ucControls:GridColumnExt ID="GridColumnExt3" runat="server" DataField="description" AllowSort="true" ResourceGroup="item" ResourceName="description" />
                    <ucControls:GridColumnExt ID="GridColumnExt2" runat="server" DataField="item_number" AllowSort="true" ResourceGroup="item" ResourceName="item_number" />
                    <%--<ucControls:GridColumnExt ID="GridColumnExt5" runat="server" DataField="grade" AllowSort="true" ResourceGroup="item" ResourceName="grade" />
                    <ucControls:GridColumnExt ID="GridColumnExt6" runat="server" DataField="price" AllowSort="true" ResourceGroup="item" ResourceName="price" />--%>
                    <ucControls:GridColumnExt ID="GridColumnExt7" runat="server" DataField="receipt_item_status" AllowSort="true" ResourceGroup="receipt_detail" ResourceName="receipt_item_status" />
                    <ucControls:GridColumnExt ID="GridColumnExt8" runat="server" DataField="quantity_received" AllowSort="true" ResourceGroup="inbound_detail" ResourceName="quantity_received" />
                    <ucControls:GridColumnExt ID="GridColumnExt9" runat="server" DataField="uom" AllowSort="true" ResourceGroup="item_uom" ResourceName="uom" />
                </CustomColumnTemplate>
            </ucControls:GridExt>
        </ucControls:PanelControlTab>

        <ucControls:PanelControlTab ID="PanelControlTab3" runat="server" PanelName="User Define" ResourceGroup="partail_receive" ResourceName="tab_user_define">
            <div class="row">
                <div class="col-sm-3">
                    <ucControls:InputTextBox ID="txtUDF1" runat="server" DataFieldValue="user_def1" ResourceGroup="receipt_header" ResourceName="user_def1" />
                    <ucControls:InputTextBox ID="txtUDF2" runat="server" DataFieldValue="user_def2" ResourceGroup="receipt_header" ResourceName="user_def2" />
                    <ucControls:InputTextBox ID="txtUDF3" runat="server" DataFieldValue="user_def3" ResourceGroup="receipt_header" ResourceName="user_def3" />
                </div>
                <div class="col-sm-3">
                    <ucControls:InputTextBox ID="txtUDF4" runat="server" DataFieldValue="user_def4" ResourceGroup="receipt_header" ResourceName="user_def4" />
                    <ucControls:InputTextBox ID="txtUDF5" runat="server" DataFieldValue="user_def5" ResourceGroup="receipt_header" ResourceName="user_def5" />
                    <ucControls:InputTextBox ID="txtUDF6" runat="server" DataFieldValue="user_def6" ResourceGroup="receipt_header" ResourceName="user_def6" />
                </div>
                <div class="col-sm-3">
                    <ucControls:InputTextNumber ID="txtUDF7" runat="server" DataFieldValue="user_def7" ResourceGroup="receipt_header" ResourceName="user_def7" />
                    <ucControls:InputTextNumber ID="txtUDF8" runat="server" DataFieldValue="user_def8" ResourceGroup="receipt_header" ResourceName="user_def8" />
                </div>
                <div class="col-sm-3">
                    <ucControls:InputTextDate ID="txtUDF9" runat="server" DataFieldValue="user_def9" ResourceGroup="receipt_header" ResourceName="user_def9" TextMode="Date" />
                    <ucControls:InputTextDate ID="txtUDF10" runat="server" DataFieldValue="user_def10" ResourceGroup="receipt_header" ResourceName="user_def10" TextMode="Date" />
                </div>
            </div>
            <ucControls:PanelControlRow runat="server" ID="PanelControlRow2">
            </ucControls:PanelControlRow>
        </ucControls:PanelControlTab>
    </ControlTemplate>
</ucControls:PanelTab>

