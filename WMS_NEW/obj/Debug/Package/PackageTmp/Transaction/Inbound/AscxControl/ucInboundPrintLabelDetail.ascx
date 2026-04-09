<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="ucInboundPrintLabelDetail.ascx.cs" Inherits="WMS_NEW.Transaction.Inbound.AscxControl.ucInboundPrintLabelDetail" %>

<%@ Register Src="~/Report/AscxControls/ucReportViewer.ascx" TagPrefix="ucControls" TagName="ucReportViewer" %>


<ucControls:ucReportViewer runat="server" ID="ucReportViewer" />

<ucControls:PanelPopup ID="popupPrint" runat="server" StyleSize="Small" HeaderText="Print">
    <DataTemplate>
        <div class="row pt-2">
            <ucControls:InputHidden ID="hidMaxQuantityOrder" runat="server" />
            <ucControls:InputHidden ID="hidPrintLabelId" runat="server" />
            <ucControls:InputTextBox ID="txtOrderNumber" runat="server" LabelText="Order Number" ResourceGroup="inbound_master" ResourceName="inbound_order_number" BaseContentCss="col-sm-6" Enabled="false" />
            <ucControls:InputTextBox ID="txtWarehouse" runat="server" LabelText="Warehouse" ResourceGroup="warehouse" ResourceName="wh_id" BaseContentCss="col-sm-6" Enabled="false" />
            <ucControls:InputTextBox ID="txtItemNumber" runat="server" LabelText="Item Number" ResourceGroup="item" ResourceName="item_number" BaseContentCss="col-sm-6" Enabled="false" />
            <ucControls:InputTextBox ID="txtLPN" runat="server" LabelText="LPN" ResourceGroup="inventory" ResourceName="lpn" BaseContentCss="col-sm-6" Enabled="false" />
            <ucControls:InputTextBox ID="txtDescription" runat="server" LabelText="Description" ResourceGroup="item" ResourceName="description" BaseContentCss="col-sm-6" Enabled="false" />
            <ucControls:InputTextBox ID="txtProductionLine" runat="server" LabelText="Production Line" ResourceGroup="inbound_master" ResourceName="production_line" BaseContentCss="col-sm-6" Enabled="false" />
            <ucControls:InputTextInteger ID="txtPackQuantityPerPallet" runat="server" LabelText="Pack Quantity/Pallet" ResourceGroup="print_label" ResourceName="pack_qty_pallet" BaseContentCss="col-sm-6" IsPrimary="true" />
            <ucControls:InputTextBox ID="txtPackSizePerPallet" runat="server" LabelText="Pack Size/Pallet" ResourceGroup="item_uom" ResourceName="item_uom_id" BaseContentCss="col-sm-6" Enabled="false" />
            <ucControls:InputTextBox ID="txtMfgDate" runat="server" LabelText="Mfg. Date" ResourceGroup="inventory" ResourceName="mfg_date" BaseContentCss="col-sm-6" Enabled="false" />
            <ucControls:InputTextBox ID="txtExpriryDate" runat="server" LabelText="Expriry Date" ResourceGroup="inventory" ResourceName="expiry_date" BaseContentCss="col-sm-6" Enabled="false" />
            <ucControls:InputTextBox ID="txtAttribute1" runat="server" LabelText="Attribute 1" ResourceGroup="inventory" ResourceName="attribute1" BaseContentCss="col-sm-6" Enabled="false" />
            <%--<ucControls:InputTextBox ID="txtPrintGroup" runat="server" LabelText="PrintLabelId" ResourceGroup="general" ResourceName="reason" BaseContentCss="col-sm-6" />
 <ucControls:InputTextBox ID="" runat="server" LabelText="PrintLabelId" ResourceGroup="general" ResourceName="reason" BaseContentCss="col-sm-6" />--%>
        </div>
    </DataTemplate>
    <CommandTemplate>
        <asp:UpdatePanel runat="server">
            <ContentTemplate>
                <ucControls:ButtonExt ID="btnPrintConfirm" runat="server" Text="Print" CssClass="btn btn-info" OnClick="btnPrint_Click" ResourceGroup="general" ResourceName="btn_save" CausesValidation="false" />
            </ContentTemplate>
            <Triggers>
                <asp:PostBackTrigger ControlID="btnPrintConfirm" />
            </Triggers>
        </asp:UpdatePanel>
    </CommandTemplate>

</ucControls:PanelPopup>

<%--<ucControls:PanelPopup ID="popClose" runat="server" StyleSize="Small" StyleColor="Danger" HeaderText="Close Order" ResourceGroup="outbound_master" ResourceName="pop_cloase_order">
    <DataTemplate>
        <ucControls:PanelControlRow ID="PanelControlRow7" runat="server">
           
        </ucControls:PanelControlRow>
    </DataTemplate>
    <CommandTemplate>
        <ucControls:ButtonExt ID="btPrintConfirm" runat="server" Text="Confirm Close Plan" CssClass="btn btn-sm btn-danger" ValidateGroup="OUT_CLOSE_ORDER" OnClick="btPrintConfirm_Click" OnClientClick="if (!confirm('Do you want to confirm ?')) return false;" ResourceGroup="general" ResourceName="btn_close" />
    </CommandTemplate>
</ucControls:PanelPopup>--%>

<ucControls:GridExt ID="GridExt1" runat="server"
    SourceAssemblyName="WMS_NEW.Access" SourceClassName="WMS_NEW.Access.Transaction.Inbound.InboundPrintLabelDetail" KeyField="KeyId" KeyType="Guid"
    GridAllowRowEdit="false" GridAllowRowDelete="false" ShowAllSort="true" AutoSize="true" DisableFirstSearch="true"
    NewVisible="false" GridAllowSelectBox="true" GridAllowShowSelectBoxAll="true" GridSortDefault="line_number asc,row_print asc" DisableRowNo="true" ShowAllFilter="true">
    <CustomSearchTemplate>
        <ucControls:InputHidden ID="gridInboundOrderMasterId" runat="server" DataFieldValue="_inbound_order_master_id" />
        <%--<ucControls:InputDropDownHD ID="ddlProductionLine" runat="server" ResourceGroup="inventory" ResourceName="production_line" DataFieldValue="_production_line" ComboType="String" DisplayDefault="--All--" DefaultFilter="Equal" />--%>
        <%--<ucControls:InputDropDown ID="ddlPrintStatus" runat="server" ResourceGroup="inventory" ResourceName="is_print" DataFieldValue="_is_print" ComboType="String" DisplayDefault="--All--" DefaultFilter="Equal" IsDefaultValue="true" />--%>
    </CustomSearchTemplate>
    <CustomCommandTemplate>
        <asp:UpdatePanel runat="server">
            <ContentTemplate>
                <ucControls:ButtonExt runat="server" ResourceGroup="general" ResourceName="btn_print" ID="btnPrePrint" Text="Print" CssClass="btn btn-sm btn-default" OnClick="btnPrePrint_Click" CausesValidation="false" Visible="true" />
            </ContentTemplate>
            <Triggers>
                <asp:PostBackTrigger ControlID="btnPrePrint" />
            </Triggers>
        </asp:UpdatePanel>
    </CustomCommandTemplate>
    <CustomColumnTemplate>
        <ucControls:GridColumnExt runat="server" HeaderText="Action" ID="gcPrint" DataField="print_label_id" CommandName="PREPRINT" IsConfirm="false" ControlType="CommandButton" CommandText="PRINT" AllowFilter="false" AllowSort="false" ShowFilterNow="false" />
        <%--<ucControls:GridColumnExt runat="server" ResourceGroup="item_uom" HeaderText="Production Line" ResourceName="production_line" DataField="production_line" AllowFilter="false" AllowSort="true" />--%>
        <ucControls:GridColumnExt runat="server" ResourceGroup="inbound_master" HeaderText="Production Line" ResourceName="production_line" ID="iColProductionLine" DataField="production_line" AllowFilter="true" AllowSort="true" ShowFilterNow="true" UseFilterDropDown="true" DropDownDisplayDefault="--All--" DropDownFilterType="LazySearch" />
        <ucControls:GridColumnExt runat="server" ResourceGroup="inventory" HeaderText="LPN" ResourceName="lpn" ID="iColLPN" DataField="lpn" AllowFilter="true" AllowSort="true" ShowFilterNow="true" />
        <ucControls:GridColumnExt runat="server" ResourceGroup="inventory" HeaderText="Is Print" ID="iColIsPrint" ResourceName="is_print" DataField="is_print" AllowFilter="true" AllowSort="true" ShowFilterNow="true" UseFilterDropDown="true" DropDownDisplayDefault="--All--" />
        <ucControls:GridColumnExt runat="server" ResourceGroup="inventory" HeaderText="Is Receipt" ResourceName="is_received" DataField="is_received" AllowFilter="true" AllowSort="true" UseFilterDropDown="true" />
        <%--<ucControls:GridColumnExt runat="server" ResourceGroup="inventory" HeaderText="Is Interface Hana" ResourceName="is_interface_hana" DataField="is_interface_hana" AllowFilter="true" AllowSort="true" UseFilterDropDown="true" />--%>
        <ucControls:GridColumnExt runat="server" ResourceGroup="item" HeaderText="Item Number" ResourceName="item_number" ID="iColItemNumber" DataField="item_number" AllowFilter="true" AllowSort="true" ShowFilterNow="true" />
        <ucControls:GridColumnExt runat="server" ResourceGroup="item" HeaderText="Description" ResourceName="description" DataField="item_description" AllowFilter="true" AllowSort="true" />
        <ucControls:GridColumnExt runat="server" ResourceGroup="print_label" HeaderText="print_date" ResourceName="print_date" DataField="print_date" AllowFilter="true" AllowSort="true" FormatType="Time" />
        <ucControls:GridColumnExt runat="server" ResourceGroup="inventory" HeaderText="Lot Number" ResourceName="lot_number" DataField="lot_number" AllowFilter="true" AllowSort="true" />
        <ucControls:GridColumnExt runat="server" ResourceGroup="inventory" HeaderText="MFG Date" ResourceName="mfg_date" DataField="mfg_date" FormatType="Date" AllowFilter="true" AllowSort="true" />
        <ucControls:GridColumnExt runat="server" ResourceGroup="inventory" HeaderText="Expiry Date" ResourceName="expiry_date" DataField="expiry_date" FormatType="Date" AllowFilter="true" AllowSort="true" />
        <ucControls:GridColumnExt runat="server" ResourceGroup="inventory" HeaderText="Attribute 1" ResourceName="attribute1" DataField="attribute1" AllowFilter="true" AllowSort="true" />
        <ucControls:GridColumnExt runat="server" ResourceGroup="print_label" HeaderText="Pack Size/Pallet" ResourceName="pack_qty_pallet" DataField="pack_size_per_pallet" FormatType="Number" AllowFilter="true" AllowSort="true" />
        <ucControls:GridColumnExt runat="server" ResourceGroup="item_uom" HeaderText="Pack Size Conversion" ResourceName="pack_size_conversion_factor" DataField="pack_size_conversion_factor" FormatType="Number" AllowSort="true" />
        <ucControls:GridColumnExt runat="server" ResourceGroup="item_uom" HeaderText="Pack Size UOM" ResourceName="item_uom_id" DataField="pack_size_uom" AllowFilter="true" AllowSort="true" />
        <ucControls:GridColumnExt runat="server" ResourceGroup="item_uom" HeaderText="Pallet Size Conversion" ResourceName="pallet_size_conversion_factor" DataField="pallet_size_conversion_factor" FormatType="Number" AllowSort="true" />
        <ucControls:GridColumnExt runat="server" ResourceGroup="item_uom" HeaderText="Pallet Size UOM" ResourceName="pallet_uom" DataField="pallet_size_uom" AllowFilter="true" AllowSort="true" />

    </CustomColumnTemplate>
</ucControls:GridExt>



<!-- Modal -->
<div id="printOptionModal" class="modal fade" tabindex="-1" role="dialog" aria-labelledby="printOptionModalLabel" aria-hidden="true" style="height: 1px !important; width: 1px !important;">
    <div class="modal-dialog modal-dialog-centered" role="document">

        <div class="modal-content" style="height: 1px !important; width: 1px !important;">
            <div class="modal-header">
                <h4 class="modal-title" id="printOptionModalLabel">Printing</h4>
                <button type="button" class="close" data-dismiss="modal" aria-label="Close">
                    <span aria-hidden="true">&times;</span>
                </button>
            </div>
            <div class="modal-body">
                <%--   <div>
                                <img src="../../_images/printer_animation.gif" />
                            </div>--%>
            </div>
            <div class="modal-footer">
                <%--<ucControls:InputHidden runat="server" ID="hid_process_log_id" />--%>
                <ucControls:InputTextBox runat="server" ID="hid_process_log_id" Visible="false" />
                <button class="btn" data-dismiss="modal" aria-hidden="true" style="display: none;">Close</button>
                <input type="button" class="btn btn-primary" onclick="printReport()" value="Print" style="display: none;" />

                <script type="text/javascript">
                    var wcppGetPrintersTimeout_ms = 10000; //10 sec
                    var wcppGetPrintersTimeoutStep_ms = 500; //0.5 sec
                    var wcppPingTimeout_ms = 10000; //10 sec
                    var wcppPingTimeoutStep_ms = 500; //0.5 sec

                    function wcppDetectOnSuccess() {
                        // WCPP utility is installed at the client side
                        // redirect to WebClientPrint sample page

                        // get WCPP version
                        var wcppVer = arguments[0];
                        if (wcppVer.substring(0, 1) == "4") {
                            //window.location.href = 'PrintRPT.aspx';
                            console.log("Print Client Detected");
                            //$('#msgInProgress').hide();
                            //$('#msgInstallWCPP').hide();
                            $('#printValidateModal').modal('hide');
                        }
                        else //force to install WCPP v4.0
                            wcppDetectOnFailure();
                    }

                    function wcppDetectOnFailure() {
                        // It seems WCPP is not installed at the client side
                        // ask the user to install it
                        $('#msgInProgress').hide();
                        $('#msgInstallWCPP').show();
                    }

                    function wcpGetPrintersOnSuccess() {
                        // Display client installed printers
                        if (arguments[0].length > 0) {
                            var p = arguments[0].split("|");
                            var options = '';
                            for (var i = 0; i < p.length; i++) {
                                options += '<option>' + p[i] + '</option>';
                            }
                            $('#installedPrinters').css('visibility', 'visible');
                            $('#installedPrinterName').html(options);
                            $('#installedPrinterName').focus();
                            $('#loadPrinters').hide();
                        } else {
                            alert("No printers are installed in your system. ");
                        }
                    }
                    function wcpGetPrintersOnFailure() {
                        // Do something if printers cannot be got from the client
                        alert("No printers are installed in your system.");
                    }

                                <%--$('#printOptionModal').on('show.bs.modal', function () {
                                    //jsWebClientPrint.getPrinters();
                                    printReport();
                                    UIkit.modal('#<%=PanelPopupProduction.ClientID%>_popup_main').hide();
                                    setTimeout(function () {
                                        //$('#printOptionModal').click();
                                        //alert("Printed");
                                        $('#printOptionModal').modal('hide');
                                    }, 3000);
                                });

                               function printReport() {
                                    var plid = $('#<%=hid_process_log_id.ClientID%>').val();
                                    jsWebClientPrint.print('useDefaultPrinter=checked' + '&printerName=null'
                                        + '&reportName=<%=Session["sesReportName"].ToString()%>'
                                        + '&plid=' + plid
                                        + '&reportParam=undefined');
                                }--%>


                    //$(document).ready(function() {
                    //    $.fn.printReport = function(name, id){
                    //        jsWebClientPrint.print('useDefaultPrinter=checked' + '&printerName=null'
                    //                    + '&reportName=' + name
                    //                    + '&plid=' + id
                    //                    + '&qrtype=ok'
                    //                    + '&reportParam=undefined');
                    //        //UIkit.modal('#" + PanelPopupProduction.ClientID + @"_popup_main').hide();
                    //        //alert(name);
                    //        setTimeout(function () {
                    //              $('#printOptionModal').modal('hide');
                    //              $('.modal-backdrop').remove();
                    //              //window.location.href= window.location;
                    //        }, 3000);
                    //    }
                    //});                                 

                </script>

                <div id="scriptPrintLayout" runat="server"></div>

                <%=Neodynamic.SDK.Web.WebClientPrint.CreateScript(
  HttpContext.Current.Request.Url.Scheme + "://" + HttpContext.Current.Request.Url.Authority + HttpContext.Current.Request.ApplicationPath + "/WebPrint" + "/WebClientPrintAPI.ashx"
, HttpContext.Current.Request.Url.Scheme + "://" + HttpContext.Current.Request.Url.Authority + HttpContext.Current.Request.ApplicationPath + "/WebPrint" + "/PrintRPTHandler.ashx"
, HttpContext.Current.Session.SessionID)    
                %>
            </div>
        </div>
    </div>
</div>
