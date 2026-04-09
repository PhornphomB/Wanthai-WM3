<%@ Page Title="" Language="C#" MasterPageFile="~/Layout.Master" AutoEventWireup="true" CodeBehind="TransactionLog.aspx.cs" Inherits="WMS_NEW.Administrator.TransactionLog" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">

    <div class="col-lg-12 col-md-12 col-sm-12 pt-3 background-base">

        <ucControls:PanelTab ID="PanelTab1" runat="server">
            <ControlTemplate>
                <ucControls:PanelControlTab ID="pnTransactionLog" runat="server" PanelName="Transaction Log" ResourceGroup="transaction_log" ResourceName="tab_transaction_log">
                    <ucControls:GridExt ID="GridChange" runat="server"
                        SourceAssemblyName="WMS_NEW.Access" SourceClassName="WMS_NEW.Access.Administrator.TransactionLog" KeyField="KeyID" KeyType="Guid"
                        GridAllowRowEdit="false" GridAllowRowDelete="false" AutoSize="true" GridAllowSelectBox="false" DisableFirstSearch="false" GridSortDefault="create_date desc">
                        <CustomColumnTemplate>
                            <ucControls:GridColumnExt ID="GridColumnExt1" runat="server" HeaderText="Transacion Type" DataField="tran_type" AllowSort="true" AllowFilter="true" ShowFilterNow="true" UseFilterDropDown="true" DropDownDisplayDefault="-- All --"
                                ResourceGroup="t_com_tran_log" ResourceName="tran_type" />
                            <ucControls:GridColumnExt ID="GridColumnExt2" runat="server" HeaderText="Sub Transaction type" DataField="sub_tran_type" ShowFilterNow="false" AllowSort="true"   AllowFilter="true" UseFilterDropDown="true" DropDownDisplayDefault="-- All --"
                                ResourceGroup="t_com_tran_log" ResourceName="sub_tran_type" />
                            <ucControls:GridColumnExt ID="GridColumnExt3" runat="server" HeaderText="Application Name" DataField="app_name" AllowSort="true" AllowFilter="true" ShowFilterNow="false"
                                ResourceGroup="t_com_tran_log" ResourceName="app_name" />
                            <ucControls:GridColumnExt ID="GridColumnExt4" runat="server" HeaderText="Start Transaction" DataField="start_tran_datetime" AllowSort="true" AllowFilter="true" ShowFilterNow="true" FormatType="Date"
                                ResourceGroup="t_com_tran_log" ResourceName="start_tran_datetime" />
                            <ucControls:GridColumnExt ID="GridColumnExt5" runat="server" HeaderText="End Transaction" DataField="end_tran_datetime" AllowSort="true" AllowFilter="true" ShowFilterNow="false" FormatType="Date"
                                ResourceGroup="t_com_tran_log" ResourceName="end_tran_datetime" />
                            <ucControls:GridColumnExt ID="GridColumnExt6" runat="server" HeaderText="From Warehouse" DataField="warehouse" AllowSort="true" AllowFilter="true" ShowFilterNow="true" UseFilterDropDown="true" DropDownDisplayDefault="-- All --"
                                ResourceGroup="t_com_tran_log" ResourceName="wh_id" DropDownFilterType="LazySearch" />
                            <ucControls:GridColumnExt ID="GridColumnExt7" runat="server" HeaderText="To Warehouse" DataField="to_warehouse" AllowSort="true" AllowFilter="true" ShowFilterNow="false" UseFilterDropDown="true" DropDownDisplayDefault="-- All --"
                                ResourceGroup="t_com_tran_log" ResourceName="to_warehouse" DropDownFilterType="LazySearch" />
                            <ucControls:GridColumnExt ID="GridColumnExt8" runat="server" HeaderText="From Location" DataField="location" AllowSort="true" AllowFilter="true" ShowFilterNow="true" UseFilterDropDown="true" DropDownFilterType="LazySearch" DropDownDisplayDefault="-- All --"
                                ResourceGroup="t_com_tran_log" ResourceName="location" />
                            <ucControls:GridColumnExt ID="GridColumnExt9" runat="server" HeaderText="To Location" DataField="to_location" AllowSort="true" AllowFilter="true" ShowFilterNow="true" UseFilterDropDown="true" DropDownFilterType="LazySearch" DropDownDisplayDefault="-- All --"
                                ResourceGroup="t_com_tran_log" ResourceName="to_location" />
                            <ucControls:GridColumnExt ID="GridColumnExt12" runat="server" HeaderText="Item" DataField="item_number" AllowSort="true" AllowFilter="true" ShowFilterNow="true" UseFilterDropDown="true" DropDownFilterType="LazySearch" DropDownDisplayDefault="-- All --"
                                ResourceGroup="t_com_tran_log" ResourceName="item_number" />
                            <ucControls:GridColumnExt ID="GridColumnExt13" runat="server" HeaderText="Quantity" DataField="quantity" ShowFilterNow="false"
                                ResourceGroup="t_com_tran_log" ResourceName="quantity" />
                            <ucControls:GridColumnExt ID="GridColumnExt14" runat="server" HeaderText="Quantity UOM" DataField="quantity_uom" ShowFilterNow="false"
                                ResourceGroup="t_com_tran_log" ResourceName="quantity_uom" />
                            <ucControls:GridColumnExt ID="GridColumnExt15" runat="server" HeaderText="After Quantity" DataField="after_quantity" AllowSort="false" AllowFilter="false" ShowFilterNow="false"
                                ResourceGroup="t_com_tran_log" ResourceName="after_quantity" />
                            <ucControls:GridColumnExt ID="GridColumnExt17" runat="server" HeaderText="Batch Number" DataField="lot_number" AllowSort="true" AllowFilter="true" ShowFilterNow="true"
                                ResourceGroup="t_com_tran_log" ResourceName="lot_number" />
                            <ucControls:GridColumnExt ID="GridColumnExt19" runat="server" HeaderText="Expiry Date" DataField="expiry_date" AllowSort="true" AllowFilter="true" ShowFilterNow="true" FormatType="Date"
                                ResourceGroup="t_com_tran_log" ResourceName="expiry_date" />
                            <ucControls:GridColumnExt ID="GridColumnExt10" runat="server" HeaderText="Receive Date" DataField="receive_date" AllowSort="true" AllowFilter="true" ShowFilterNow="true" FormatType="Date"
                                ResourceGroup="t_com_tran_log" ResourceName="receive_date" />


                            <ucControls:GridColumnExt ID="GridColumnExt21" runat="server" HeaderText="Serial Number" DataField="serial_number" AllowSort="true" AllowFilter="true" ShowFilterNow="true"
                                ResourceGroup="t_com_tran_log" ResourceName="serial_number" />
                            <ucControls:GridColumnExt ID="GridColumnExt24" runat="server" HeaderText="Parent LPN" DataField="parent_lpn" AllowSort="true" AllowFilter="true" ShowFilterNow="true"
                                ResourceGroup="t_com_tran_log" ResourceName="parent_lpn" />
                            <ucControls:GridColumnExt ID="GridColumnExt25" runat="server" HeaderText="LPN" DataField="lpn" AllowSort="true" AllowFilter="true" ShowFilterNow="true"
                                ResourceGroup="t_com_tran_log" ResourceName="lpn" />
                            <ucControls:GridColumnExt ID="GridColumnExt26" runat="server" HeaderText="After LPN" DataField="after_lpn" AllowSort="true" AllowFilter="true" ShowFilterNow="false"
                                ResourceGroup="t_com_tran_log" ResourceName="after_lpn" />
                            <ucControls:GridColumnExt ID="GridColumnExt27" runat="server" HeaderText="Before Status" DataField="status" AllowSort="true" AllowFilter="true" ShowFilterNow="false"
                                ResourceGroup="t_com_tran_log" ResourceName="status" />
                            <ucControls:GridColumnExt ID="GridColumnExt28" runat="server" HeaderText="After Status" DataField="after_status" AllowSort="true" AllowFilter="true" ShowFilterNow="false"
                                ResourceGroup="t_com_tran_log" ResourceName="after_status" />
                            <ucControls:GridColumnExt ID="GridColumnExt30" runat="server" HeaderText="Order Number" DataField="order_number" AllowSort="true" AllowFilter="true" ShowFilterNow="true"
                                ResourceGroup="t_com_tran_log" ResourceName="order_number" />
                            <ucControls:GridColumnExt ID="GridColumnExt31" runat="server" HeaderText="Line Number" DataField="line_number" AllowSort="true" AllowFilter="true" ShowFilterNow="false"
                                ResourceGroup="t_com_tran_log" ResourceName="line_number" />
                            <ucControls:GridColumnExt ID="GridColumnExt34" runat="server" HeaderText="Reason" DataField="reason_desc" DataFieldFilter="reason_code" AllowSort="true" AllowFilter="true" ShowFilterNow="true" UseFilterDropDown="true" DropDownFilterType="LazySearch" DropDownDisplayDefault="-- All --"
                                ResourceGroup="t_com_tran_log" ResourceName="reason_code" />
                            <ucControls:GridColumnExt ID="GridColumnExt35" runat="server" HeaderText="Udf 1" DataField="udf_1" AllowSort="true" AllowFilter="true" ShowFilterNow="false"
                                ResourceGroup="t_com_tran_log" ResourceName="udf_1" />
                            <ucControls:GridColumnExt ID="GridColumnExt36" runat="server" HeaderText="Udf 2" DataField="udf_2" AllowSort="true" AllowFilter="true" ShowFilterNow="false"
                                ResourceGroup="t_com_tran_log" ResourceName="udf_2" />
                            <ucControls:GridColumnExt ID="GridColumnExt37" runat="server" HeaderText="Udf 3" DataField="udf_3" AllowSort="true" AllowFilter="true" ShowFilterNow="false"
                                ResourceGroup="t_com_tran_log" ResourceName="udf_3" />
                            <ucControls:GridColumnExt ID="GridColumnExt42" runat="server" HeaderText="Device" DataField="device" AllowSort="true" AllowFilter="true" ShowFilterNow="false"
                                ResourceGroup="t_com_tran_log" ResourceName="device" />
                            <ucControls:GridColumnExt ID="GridColumnExt43" runat="server" HeaderText="Create By" DataField="create_by" AllowSort="true" AllowFilter="true" ShowFilterNow="true"
                                ResourceGroup="t_com_tran_log" ResourceName="create_by" />
                            <ucControls:GridColumnExt ID="GridColumnExt100" runat="server" HeaderText="Create Date" DataField="create_date" AllowSort="true" AllowFilter="true" ShowFilterNow="false" FormatType="DateTime"
                                ResourceGroup="t_com_tran_log" ResourceName="create_date" />
                            <ucControls:GridColumnExt ID="GridColumnExt54" runat="server" HeaderText="Attribute1" DataField="attribute1" AllowSort="true" AllowFilter="true" ShowFilterNow="true"
                                ResourceGroup="t_com_tran_log" ResourceName="attribute1" />
                            <ucControls:GridColumnExt ID="GridColumnExt55" runat="server" HeaderText="After Attribute1" DataField="after_attribute1" AllowSort="true" AllowFilter="true" ShowFilterNow="false"
                                ResourceGroup="t_com_tran_log" ResourceName="attribute1" />
                            <ucControls:GridColumnExt ID="GridColumnExt56" runat="server" HeaderText="Attribute2" DataField="attribute2" AllowSort="true" AllowFilter="true" ShowFilterNow="true"
                                ResourceGroup="t_com_tran_log" ResourceName="attribute2" />
                            <ucControls:GridColumnExt ID="GridColumnExt57" runat="server" HeaderText="After Attribute2" DataField="after_attribute2" AllowSort="true" AllowFilter="true" ShowFilterNow="false"
                                ResourceGroup="t_com_tran_log" ResourceName="attribute2" />
                            <ucControls:GridColumnExt ID="GridColumnExt58" runat="server" HeaderText="Attribute3" DataField="attribute3" AllowSort="true" AllowFilter="true" ShowFilterNow="true"
                                ResourceGroup="t_com_tran_log" ResourceName="attribute3" />
                            <ucControls:GridColumnExt ID="GridColumnExt59" runat="server" HeaderText="After Attribute3" DataField="after_attribute3" AllowSort="true" AllowFilter="true" ShowFilterNow="false"
                                ResourceGroup="t_com_tran_log" ResourceName="attribute3" />
                            <ucControls:GridColumnExt ID="GridColumnExt60" runat="server" HeaderText="Attribute4" DataField="attribute4" AllowSort="true" AllowFilter="true" ShowFilterNow="true"
                                ResourceGroup="t_com_tran_log" ResourceName="attribute4" />
                            <ucControls:GridColumnExt ID="GridColumnExt61" runat="server" HeaderText="After Attribute4" DataField="after_attribute4" AllowSort="true" AllowFilter="true" ShowFilterNow="false"
                                ResourceGroup="t_com_tran_log" ResourceName="attribute4" />
                            <ucControls:GridColumnExt ID="GridColumnExt62" runat="server" HeaderText="Attribute5" DataField="attribute5" AllowSort="true" AllowFilter="true" ShowFilterNow="true"
                                ResourceGroup="t_com_tran_log" ResourceName="attribute5" />
                            <ucControls:GridColumnExt ID="GridColumnExt63" runat="server" HeaderText="After Attribute5" DataField="after_attribute5" AllowSort="true" AllowFilter="true" ShowFilterNow="false"
                                ResourceGroup="t_com_tran_log" ResourceName="attribute5" />
                            <ucControls:GridColumnExt ID="GridColumnExt22" runat="server" HeaderText="Reference Number" DataField="reference_number" AllowSort="true" AllowFilter="false" ShowFilterNow="true"
                                ResourceGroup="t_com_tran_log" ResourceName="reference_number" />

                        </CustomColumnTemplate>
                    </ucControls:GridExt>
                </ucControls:PanelControlTab>

                <ucControls:PanelControlTab ID="pnProcessLog" runat="server" PanelName="Process Log" ResourceGroup="transaction_log" ResourceName="tab_process_log">
                    <ucControls:GridExt ID="GridExt1" runat="server"
                        SourceAssemblyName="WMS_NEW.Access" SourceClassName="WMS_NEW.Access.Administrator.ProcessLog" KeyField="KeyID" KeyType="Guid"
                        GridAllowRowEdit="false" GridAllowRowDelete="false" AutoSize="true">
                        <CustomCommandTemplate>
                        </CustomCommandTemplate>
                        <CustomSearchTemplate>
                        </CustomSearchTemplate>
                        <CustomColumnTemplate>
                            <ucControls:GridColumnExt ID="GridColumnExt44" runat="server" HeaderText="Warehouse" DataField="warehouse" AllowSort="true" AllowFilter="true" ShowFilterNow="true" UseFilterDropDown="true" ResourceGroup="t_com_process_log" ResourceName="wh_id" />
                            <ucControls:GridColumnExt ID="GridColumnExt45" runat="server" HeaderText="Device" DataField="device" AllowSort="true" AllowFilter="true" ShowFilterNow="false" ResourceGroup="t_com_process_log" ResourceName="device" />
                            <ucControls:GridColumnExt ID="GridColumnExt46" runat="server" HeaderText="Process" DataField="process" AllowSort="true" AllowFilter="true" ShowFilterNow="false" ResourceGroup="t_com_process_log" ResourceName="process" />
                            <ucControls:GridColumnExt ID="GridColumnExt47" runat="server" HeaderText="Process Datetime" DataField="process_datetime" AllowSort="true" AllowFilter="true" ShowFilterNow="false" FormatType="DateTime" ResourceGroup="t_com_process_log" ResourceName="process_datetime" />
                            <ucControls:GridColumnExt ID="GridColumnExt48" runat="server" HeaderText="Data 1" DataField="data_1" AllowSort="true" AllowFilter="true" ShowFilterNow="true" ResourceGroup="t_com_process_log" ResourceName="data_1" />
                            <ucControls:GridColumnExt ID="GridColumnExt49" runat="server" HeaderText="Data 2" DataField="data_2" AllowSort="true" AllowFilter="true" ShowFilterNow="true" ResourceGroup="t_com_process_log" ResourceName="data_2" />
                            <ucControls:GridColumnExt ID="GridColumnExt50" runat="server" HeaderText="Data 3" DataField="data_3" AllowSort="true" AllowFilter="true" ShowFilterNow="true" ResourceGroup="t_com_process_log" ResourceName="data_3" />
                            <ucControls:GridColumnExt ID="GridColumnExt51" runat="server" HeaderText="Data 4" DataField="data_4" AllowSort="true" AllowFilter="true" ShowFilterNow="true" ResourceGroup="t_com_process_log" ResourceName="data_4" />
                            <ucControls:GridColumnExt ID="GridColumnExt52" runat="server" HeaderText="Message" DataField="message" AllowSort="true" AllowFilter="true" ShowFilterNow="true" ResourceGroup="t_com_process_log" ResourceName="message" />
                            <ucControls:GridColumnExt ID="GridColumnExt53" runat="server" HeaderText="Create By" DataField="create_by" AllowSort="true" AllowFilter="true" ShowFilterNow="false" ResourceGroup="t_com_process_log" ResourceName="create_by" />

                        </CustomColumnTemplate>
                    </ucControls:GridExt>
                </ucControls:PanelControlTab>

                 <ucControls:PanelControlTab ID="PanelControlTab1" runat="server" PanelName="Interface Log" ResourceGroup="transaction_log" ResourceName="tab_interface_log">
                    <ucControls:GridExt ID="GridExt2" runat="server"
                        SourceAssemblyName="WMS_NEW.Access" SourceClassName="WMS_NEW.Access.Administrator.InterfaceLog" KeyField="KeyID" KeyType="Guid"
                        GridAllowRowEdit="false" GridAllowRowDelete="false" AutoSize="true">
                        <CustomCommandTemplate>
                        </CustomCommandTemplate>
                        <CustomSearchTemplate>
                        </CustomSearchTemplate>
                        <CustomColumnTemplate>
                            <ucControls:GridColumnExt ID="GridColumnExt11" runat="server" HeaderText="Process" DataField="process" AllowSort="true" AllowFilter="true" ShowFilterNow="true" ResourceGroup="t_interface_process_log_execute" ResourceName="process" />
                            <ucControls:GridColumnExt ID="GridColumnExt16" runat="server" HeaderText="Hosr Record ID" DataField="host_record_id" AllowSort="true" AllowFilter="true" ShowFilterNow="false" ResourceGroup="t_interface_process_log_execute" ResourceName="host_record_id" />
                            <ucControls:GridColumnExt ID="GridColumnExt18" runat="server" HeaderText="Data Member" DataField="data_number" AllowSort="true" AllowFilter="true" ShowFilterNow="false" ResourceGroup="t_interface_process_log_execute" ResourceName="data_number" />
                            <ucControls:GridColumnExt ID="GridColumnExt20" runat="server" HeaderText="Error Code" DataField="error_code" AllowSort="true" AllowFilter="true" ShowFilterNow="false"  ResourceGroup="t_interface_process_log_execute" ResourceName="error_code" />
                            <ucControls:GridColumnExt ID="GridColumnExt23" runat="server" HeaderText="Error Message" DataField="error_msg" AllowSort="true" AllowFilter="true" ShowFilterNow="true" ResourceGroup="t_interface_process_log_execute" ResourceName="error_msg" />
                            <ucControls:GridColumnExt ID="GridColumnExt40" runat="server" HeaderText="Data 1" DataField="data_1" AllowSort="true" AllowFilter="true" ShowFilterNow="true" ResourceGroup="t_interface_process_log_execute" ResourceName="data_1" />
                            <ucControls:GridColumnExt ID="GridColumnExt29" runat="server" HeaderText="Data 2" DataField="data_2" AllowSort="true" AllowFilter="true" ShowFilterNow="true" ResourceGroup="t_interface_process_log_execute" ResourceName="data_2" />
                            <ucControls:GridColumnExt ID="GridColumnExt32" runat="server" HeaderText="Data 3" DataField="data_3" AllowSort="true" AllowFilter="true" ShowFilterNow="true" ResourceGroup="t_interface_process_log_execute" ResourceName="data_3" />
                            <ucControls:GridColumnExt ID="GridColumnExt33" runat="server" HeaderText="Data 4" DataField="data_4" AllowSort="true" AllowFilter="true" ShowFilterNow="true" ResourceGroup="t_interface_process_log_execute" ResourceName="data_4" />
                            <ucControls:GridColumnExt ID="GridColumnExt39" runat="server" HeaderText="Create Date" DataField="create_date" AllowSort="true" AllowFilter="true" ShowFilterNow="false" ResourceGroup="t_interface_process_log_execute" ResourceName="create_date" />

                        </CustomColumnTemplate>
                    </ucControls:GridExt>
                </ucControls:PanelControlTab>
            </ControlTemplate>
        </ucControls:PanelTab>

    </div>

</asp:Content>
