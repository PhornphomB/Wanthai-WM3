<%@ Page Title="" Language="C#" MasterPageFile="~/Layout.Master" AutoEventWireup="true" CodeBehind="DBoardCenter.aspx.cs" Inherits="WMS_NEW.DashBoard.DBoardCenter" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
    <style type="text/css">
        .dark .card-outline-danger {
            border-color: #353535 !important;
        }

        .icons-list li .desc {
            padding-top: 10px !important;
            height: 0px !important;
            border-bottom: 0px !important;
        }

        .icons-list li .value {
            top: 3px !important;
            position: relative !important;
        }

        .font-pie-chart {
            font-size: 0.9rem;
            font-weight: normal !important;
        }

         .bg-info-prog {
            background-color: #36b3e0 !important;
        }

        .bg-success-prog {
            background-color: #089a3e !important;
        }

        .bg-danger-prog {
            background-color: #ff5f5f !important;
        }

        .breadcrumb-item + .breadcrumb-item::before {
            content: unset !important;
        }

        .btn-warehouse {
            border-radius: 0 !important;
        }
    </style>
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="ContentBreadcrumb" runat="server">
    <ul class="breadcrumb">
        <li class="breadcrumb-item text-uppercase">
            <div class="col-lg-12">
                <asp:DropDownList ID="ddlWarehouse" runat="server" AutoPostBack="true" OnSelectedIndexChanged="ddlWarehouse_SelectedIndexChanged" CausesValidation="false" CssClass="btn btn-sm btn-primary btn-warehouse" />
            </div>
        </li>
    </ul>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <asp:Panel ID="panelForm" runat="server" CssClass="animated fadeIn mt-4">

        <div class="row">
            <div class="col-sm-6 col-lg-3">
                <div class="card card-inverse card-info-gar" style="<% Response.Write(this.HAS_INBOUND ? "cursor: pointer": "cursor: default"); %>" onclick="location.href='<% Response.Write(this.HAS_INBOUND ? ResolveUrl("~/Transaction/Inbound/Inbound.aspx?mkey=6612c1c1e07e257ea3fff1ad6fdd5d55") : "#"); %>';">
                    <div class="card-block pb-0">
                        <h4 id="labInbActual" runat="server" class="mb-0">0</h4>
                        <p class="text-uppercase">inbound receive</p>
                    </div>
                    <div class="chart-wrapper px-3" style="height: 50px;">
                        <div class="bars">
                            <div class="progress progress-xs">
                                <div id="progInb" runat="server" class="progress-bar bg-info-prog" role="progressbar" style="width: 0%" aria-valuenow="0" aria-valuemin="0" aria-valuemax="100"></div>
                            </div>
                        </div>
                        <div>
                            <i class="icon-graph"></i>
                            <span class="title text-uppercase">receive plan</span>
                            <span id="labInbPlan" runat="server" class="value float-right">0</span>
                        </div>
                    </div>
                </div>
            </div>
            <!--/.col-->

            <div class="col-sm-6 col-lg-3">
                <div class="card card-inverse card-danger-gar" style="<% Response.Write(this.HAS_OUTBOUND ? "cursor: pointer": "cursor: default"); %>" onclick="location.href='<% Response.Write(this.HAS_OUTBOUND ? ResolveUrl("~/Transaction/Outbound/Outbound.aspx?mkey=c7485eb1243204a306814dc8334a6246") : "#"); %>';">
                    <div class="card-block pb-0">
                        <h4 id="labOutActual" runat="server" class="mb-0">0</h4>
                        <p class="text-uppercase">outbound pick</p>
                    </div>
                    <div class="chart-wrapper px-3" style="height: 50px;">
                        <div class="bars">
                            <div class="progress progress-xs">
                                <div id="progOut" runat="server" class="progress-bar bg-danger-prog" role="progressbar" style="width: 0%" aria-valuenow="0" aria-valuemin="0" aria-valuemax="100"></div>
                            </div>
                        </div>
                        <div>
                            <i class="icon-graph"></i>
                            <span class="title text-uppercase">pick plan</span>
                            <span id="labOutPlan" runat="server" class="value float-right">0</span>
                        </div>
                    </div>
                </div>
            </div>
            <!--/.col-->

            <div class="col-sm-6 col-lg-3">
                <div class="card card-inverse card-success-gar" style="<% Response.Write(this.HAS_INVENTORY ? "cursor: pointer": "cursor: default"); %>" onclick="location.href='<% Response.Write(this.HAS_INVENTORY ? ResolveUrl("~/Transaction/Inventory/InventoryViewer.aspx?mkey=a981f42538de98445b4c3532ed21ad70") : "#"); %>';">
                    <div class="card-block pb-0">
                        <h4 id="labInvActual" runat="server" class="mb-0">0</h4>
                        <p class="text-uppercase">inventory allocate</p>
                    </div>
                    <div class="chart-wrapper px-3" style="height: 50px;">
                        <div class="bars">
                            <div class="progress progress-xs">
                                <div id="progInv" runat="server" class="progress-bar bg-success-prog" role="progressbar" style="width: 0%" aria-valuenow="0" aria-valuemin="0" aria-valuemax="100"></div>
                            </div>
                        </div>
                        <div>
                            <i class="icon-graph"></i>
                            <span class="title text-uppercase">total inventory</span>
                            <span id="labInvPlan" runat="server" class="value float-right">0</span>
                        </div>
                    </div>
                </div>
            </div>
            <!--/.col-->

            <div class="col-sm-6 col-lg-3">
                <div class="card card-inverse card-warning-gar" style="<% Response.Write(this.HAS_LOCATION ? "cursor: pointer": "cursor: default"); %>" onclick="location.href='<% Response.Write(this.HAS_LOCATION ? ResolveUrl("~/MasterData/Location.aspx?mkey=7f718f584ea60a15c3a382a090b22e18") : "#"); %>';">
                    <div class="card-block pb-0">
                        <h4 id="labLocActual" runat="server" class="mb-0">0</h4>
                        <p class="text-uppercase">location used</p>
                    </div>
                    <div class="chart-wrapper px-3" style="height: 50px;">
                        <div class="bars">
                            <div class="progress progress-xs">
                                <div id="progLoc" runat="server" class="progress-bar bg-warning" role="progressbar" style="width: 0%" aria-valuenow="0" aria-valuemin="0" aria-valuemax="100"></div>
                            </div>
                        </div>
                        <div>
                            <i class="icon-graph"></i>
                            <span class="title text-uppercase">total location</span>
                            <span id="labLocPlan" runat="server" class="value float-right">0</span>
                        </div>
                    </div>
                </div>
            </div>
            <!--/.col-->
        </div>

        <div class="row">
            <div class="col-sm-12 col-lg-8">
                <div class="card">
                    <div class="card-header text-uppercase text-bold">Transaction 7 Days Chart</div>
                    <div class="card-block" style="height: 645px;">
                        <asp:UpdatePanel ID="updateChartTempInOut" runat="server" UpdateMode="Conditional" ChildrenAsTriggers="false">
                            <ContentTemplate>
                                <canvas id="chartTempInOut" height="180"></canvas>
                            </ContentTemplate>
                        </asp:UpdatePanel>
                    </div>
                </div>
            </div>
            <div class="col-sm-12 col-lg-4">
                <div class="card">
                    <div class="card-header text-uppercase text-bold">Inventory Status</div>
                    <div class="card-block" style="height: 645px;">

                        <div class="rows col-sm-12 col-md-12 col-lg-12">
                            <canvas id="chartTempInvStatus" height="260"></canvas>
                        </div>

                        <ul class="icons-list">
                            <li>
                                <i class="icon" style="background-color: #089a3e;"></i>
                                <div class="desc font-pie-chart">
                                    <div class="title">Approved</div>
                                </div>
                                <div class="value font-pie-chart">
                                    <span id="labInvStatAval" runat="server">0</span>
                                </div>
                            </li>
                            <li>
                                <i class="icon" style="background-color: #e2283d;"></i>
                                <div class="desc font-pie-chart">
                                    <div class="title">Quarantine</div>
                                </div>
                                <div class="value font-pie-chart">
                                    <span id="labInvStatDam" runat="server">0</span>
                                </div>
                            </li>
                            <%--<li>
                                <i class="icon" style="background-color: #df8a2a;"></i>
                                <div class="desc font-pie-chart">
                                    <div class="title">Hold</div>
                                </div>
                                <div class="value font-pie-chart">
                                    <span id="labInvStatHold" runat="server">0</span>
                                </div>
                            </li>--%>
                        </ul>
                    </div>
                </div>
            </div>
        </div>

          <div class="row">
            <div class="col-sm-12 col-lg-6">
                <div class="card">
                    <div class="card-header text-uppercase text-bold">Top Item Movement In 3 Months</div>
                    <div class="card-block">
                        <asp:UpdatePanel ID="updateChartItemMovement" runat="server" UpdateMode="Conditional" ChildrenAsTriggers="false">
                            <ContentTemplate>
                                <canvas id="chartItemMovement" height="180"></canvas>
                            </ContentTemplate>
                        </asp:UpdatePanel>
                    </div>
                </div>
            </div>
            <div class="col-sm-12 col-lg-6">
               <div class="card">
                    <div class="card-header text-uppercase text-bold">Top Location Movement In 3 Months</div>
                    <div class="card-block">
                        <asp:UpdatePanel ID="updateChartLocationMovement" runat="server" UpdateMode="Conditional" ChildrenAsTriggers="false">
                            <ContentTemplate>
                                <canvas id="chartLocationMovement" height="180"></canvas>
                            </ContentTemplate>
                        </asp:UpdatePanel>
                    </div>
                </div>
            </div>
        </div>

        <div class="row">
            <div class="col-sm-6 col-lg-4">
                <div class="card card-outline-success" style="<% Response.Write(this.HAS_INVENTORY ? "cursor: pointer": "cursor: default"); %>" onclick="location.href='<% Response.Write(this.HAS_INVENTORY ? ResolveUrl("~/Transaction/Inventory/InventoryViewer.aspx?mkey=a981f42538de98445b4c3532ed21ad70&invw_iexp=false") : "#"); %>';">
                    <div class="card-block p-0 clearfix">
                        <i class="fa fa-bell bg-success p-4 font-2xl mr-3 float-left"></i>
                        <div id="labExp60" runat="server" class="h5 mb-0 pt-3">0</div>
                        <div class="text-muted text-uppercase font-weight-bold font-xs">near expire 60 days</div>
                    </div>
                </div>
            </div>
            <div class="col-sm-6 col-lg-4">
                <div class="card card-outline-info" style="<% Response.Write(this.HAS_INVENTORY ? "cursor: pointer": "cursor: default"); %>" onclick="location.href='<% Response.Write(this.HAS_INVENTORY ? ResolveUrl("~/Transaction/Inventory/InventoryViewer.aspx?mkey=a981f42538de98445b4c3532ed21ad70&invw_iexp=false") : "#"); %>';">
                    <div class="card-block p-0 clearfix">
                        <i class="fa fa-bell bg-info p-4 font-2xl mr-3 float-left"></i>
                        <div id="labExp30" runat="server" class="h5 mb-0 pt-3">0</div>
                        <div class="text-muted text-uppercase font-weight-bold font-xs">near expire 30 days</div>
                    </div>
                </div>
            </div>
            <div class="col-sm-6 col-lg-4">
                <div class="card card-outline-danger" style="<% Response.Write(this.HAS_INVENTORY ? "cursor: pointer": "cursor: default"); %>" onclick="location.href='<% Response.Write(this.HAS_INVENTORY ? ResolveUrl("~/Transaction/Inventory/InventoryViewer.aspx?mkey=a981f42538de98445b4c3532ed21ad70&invw_iexp=true") : "#"); %>';">
                    <div class="card-block p-0 clearfix">
                        <i class="fa fa-bell bg-danger p-4 font-2xl mr-3 float-left"></i>
                        <div id="labExp" runat="server" class="h5 mb-0 pt-3">0</div>
                        <div class="text-muted text-uppercase font-weight-bold font-xs">expired</div>
                    </div>
                </div>
            </div>
        </div>

    </asp:Panel>
</asp:Content>
