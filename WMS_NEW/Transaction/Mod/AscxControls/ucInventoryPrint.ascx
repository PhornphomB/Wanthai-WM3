<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="ucInventoryPrint.ascx.cs" Inherits="WMS_NEW.Transaction.Mod.AscxControls.ucInventoryPrint" %>

<%@ Register Src="~/Report/AscxControls/ucReportViewer.ascx" TagPrefix="ucControls" TagName="ucReportViewer" %>

<ucControls:ucReportViewer runat="server" ID="ucReportViewer" />

<ucControls:PanelPopup ID="popupPrint" runat="server" HeaderText="Print Item" StyleSize="Default">
    <DataTemplate>
        <asp:Panel ID="Panel1" runat="server">
            <div class="row">
                <%--<div class="col-sm-4">
                    <ucControls:InputTextBox runat="server" ResourceGroup="warehouse" ResourceName="wh_id" ID="txtWarehouse" Enabled="false" />
                    <ucControls:InputTextBox runat="server" ID="txtItemNumber" LabelText="Item Number" Enabled="false" ResourceGroup="item" ResourceName="item_number" />
                    <ucControls:InputTextBox runat="server" ID="txtItemDesc" LabelText="Item Description" Enabled="false" IsMultiLine="true" ResourceGroup="item" ResourceName="description" />
                    <ucControls:InputDropDown runat="server" ID="ddlUOM" LabelText="UOM" IsPrimary="true" AutoPostBack="true" ResourceGroup="item_uom" ResourceName="uom" />
                    <ucControls:InputTextNumber runat="server" ID="txtConvFac" LabelText="Pack Size" IsPrimary="true" />
                </div>
                <div class="col-sm-4">
                    <ucControls:InputTextBox runat="server" ID="txtLot" LabelText="Lot" ResourceGroup="inventory" ResourceName="lot_number" />
                    <ucControls:InputTextDate runat="server" ID="txtExpiryDate" LabelText="Expiry Date" ResourceGroup="inventory" ResourceName="expiry_date" TextMode="Date" />
                    <ucControls:InputTextDate runat="server" ID="txtMFGDate" LabelText="MFG Date" ResourceGroup="inventory" ResourceName="mfg_date" TextMode="Date" />
                    <ucControls:InputTextBox runat="server" ID="txtRemark" LabelText="Remark" IsMultiLine="true" />
                    <ucControls:InputTextBox runat="server" ID="txtLPN" LabelText="LPN" ResourceGroup="inventory" ResourceName="lpn" />

                </div>
                <div class="col-sm-4">
                    <ucControls:InputDropDown runat="server" ResourceGroup="PrinterGroup" ResourceName="group_name" ID="ddlGroupPrint" IsPrimary="true" />
                    <ucControls:InputDropDown runat="server" ResourceGroup="Printer" ResourceName="printer_name" ID="ddlPrint" />
                    <ucControls:InputTextInteger runat="server" ResourceGroup="general" ResourceName="number_copy" ID="txtNumberofCopy" IsPrimary="true" LabelText="Copy" />
                    <ucControls:InputTextBox runat="server" ID="txtOptionField" LabelText="Order Number" />
                </div>--%>
                <div class="col-sm-4">
                    <ucControls:InputHidden runat="server" ID="hidQuantity" IsStaticValue="true" />
                    <ucControls:InputHidden runat="server" ID="hidInventoryId" IsStaticValue="true" />
                    <ucControls:InputTextBox runat="server" ResourceGroup="warehouse" ResourceName="wh_id" ID="txtWarehouse" Enabled="false" />
                    <ucControls:InputTextBox runat="server" ID="txtItemNumber" LabelText="Item Number" Enabled="false" ResourceGroup="item" ResourceName="item_number" />
                    <ucControls:InputTextDate runat="server" ID="txtMFGDate" LabelText="MFG Date" ResourceGroup="inventory" ResourceName="mfg_date" TextMode="Date" Enabled="false" />
                    <ucControls:InputDropDown runat="server" ID="ddlUOM" LabelText="UOM" AutoPostBack="true" ResourceGroup="item_uom" ResourceName="uom" IsPrimary="true" />
                    <ucControls:InputTextNumber runat="server" ID="txtOnHand" ResourceGroup="inventory" ResourceName="onhand" LabelText="On Hand" IsPrimary="true" AutoPostBack="true" />

                </div>
                <div class="col-sm-4">
                    <ucControls:InputTextBox runat="server" ResourceGroup="print_label" ResourceName="order_number" ID="txtOrderNumber" Enabled="false" />
                    <ucControls:InputTextBox runat="server" ID="txtItemDesc" LabelText="Description" ResourceGroup="inventory" ResourceName="item_description" Enabled="false"/>
                    <ucControls:InputTextDate runat="server" ID="txtMFGTime" LabelText="MFG Time" ResourceGroup="inventory" ResourceName="mfg_time" TextMode="Time" IsPrimary="true" />
                    <ucControls:InputTextNumber runat="server" ID="txtConvFac" LabelText="Pack Size" Enabled="false"/>
                </div>
                <div class="col-sm-4">
                    <ucControls:InputTextBox runat="server" ID="txtLPN" LabelText="LPN" ResourceGroup="inventory" ResourceName="lpn" Enabled="false"/>
                    <ucControls:InputTextBox runat="server" ID="txtLot" LabelText="Lot" ResourceGroup="inventory" ResourceName="lot_number" Enabled="false"/>
                    <ucControls:InputTextDate runat="server" ID="txtExpiryDate" LabelText="Expiry Date" ResourceGroup="inventory" ResourceName="expiry_date" TextMode="Date" Enabled="false" />
                    <ucControls:InputTextNumber runat="server" ID="txtPackSizePerPallet" LabelText="Pack Size/Pallet"  Enabled="false"/>
                </div>
            </div>
        </asp:Panel>
    </DataTemplate>
    <CommandTemplate>
        <asp:UpdatePanel runat="server" ID="updatePrint" UpdateMode="Conditional">
            <ContentTemplate>
                <ucControls:ButtonExt runat="server" ResourceGroup="general" ResourceName="btn_print" ID="btnConfrimPrint" Text="Print" CssClass="btn btn-sm btn-info" OnClick="btnConfrimPrint_Click" OnClientClick="if (!confirm('Do you want to confirm ?')) return false;" />
            </ContentTemplate>
        </asp:UpdatePanel>
    </CommandTemplate>
</ucControls:PanelPopup>

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
