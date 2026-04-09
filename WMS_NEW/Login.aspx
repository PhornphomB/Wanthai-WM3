<%@ Page Title="" Language="C#" MasterPageFile="~/Layout.Master" AutoEventWireup="true" CodeBehind="Login.aspx.cs" Inherits="WMS_NEW.Login" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
    <script type="text/javascript">
        function onkeyBoxNumber(event, obj) {
            var keyCode = event.keyCode ? event.keyCode : event.which ? event.which : event.charCode;
            if (keyCode === 13) {
                doPostBackAsync('CHECK_LOGIN', obj.value);
                return false;
            }
            else {
                return true;
            }
        }
    </script>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">

    <ucControls:PanelPopup ID="popupChangePassword" runat="server" HeaderText="Change Password">
        <DataTemplate>           
            <div class="col-sm-12 text-uppercase text-white">
                <label class="text-muted text-alian">New Passwrod</label>                
                <asp:TextBox ID="txtNewPasswords" runat="server" CssClass="form-control form-login" TextMode="Password" MaxLength="100" placeholder="New Passwrod" autocomplete="off"
                                        AutoPostBack="false" onkeypress="return onkeyBoxNumber(event,this);"></asp:TextBox>
            </div>
            <div class="col-sm-12 text-uppercase text-white">
                <label class="text-muted text-alian">Re-New Passwrod</label>                
                <asp:TextBox ID="txtReNewPasswords" runat="server" CssClass="form-control form-login" TextMode="Password" MaxLength="100" placeholder="Re-New Passwrod" autocomplete="off"
                                        AutoPostBack="false" onkeypress="return onkeyBoxNumber(event,this);"></asp:TextBox>
            </div>  
        </DataTemplate>
        <CommandTemplate>
            <span style="padding-left: 5px;">
                <ucControls:ButtonExt ID="btComfirm" runat="server" Text="Update Password" CssClass="btn btn-danger" OnClick="btComfirmChangePass_Click" />
            </span>
        </CommandTemplate>
    </ucControls:PanelPopup>

    <link href="_css/theme-light-effect.css" rel="stylesheet" />
    <link href="_css/theme-dark-effect.css" rel="stylesheet" />

    <style type="text/css">
        body {
            overflow: hidden;
        }


        body, .main, .card-login {
            height: 100%;
            border-width: 0px !important;
        }

            body.dark, .dark .card-message-danger {
                background: radial-gradient(ellipse at bottom, #1b2735 0%, #090a0f 100%) !important;
            }

        .text-alian {
            /*font-family: 'ZELDA';*/
        }


        .form-control {
            height: calc(2.5625rem + 1px) !important;
            padding: .375rem .75rem;
            font-size: 1.2rem;
            line-height: 2;
            background-color: inherit !important;
            background-clip: padding-box;
            color: #3a94d0 !important;
            border: 2px solid #368dc6 !important;
        }

        .light .form-control {
            border: 1px solid #368dc6 !important;
        }

        .form-control:focus {
            box-shadow: 0 0 0 0.09rem #368dc6;
        }

        .dark .btn-thm-dark, .dark .btn:hover, .dark .btn:active {
            background-color: transparent !important;
        }

        .dark .btn-thm-light:hover, .dark .btn-thm-light:active {
            background-color: #fff !important;
        }


        .card-login, .form-login {
            background-color: transparent !important;
        }

        .btn-login {
            font-size: 1.3rem !important;
        }

            .btn-login:hover {
                color: #ed3045 !important;
                background-color: transparent;
                box-shadow: 0 0 0 0.09rem #df2035;
            }

        .text-login-desc {
            color: #4a4a4a;
        }

        .dark .text-login-desc {
            color: #b0bec5;
        }
    </style>

    <% if (Request.Browser.IsMobileDevice)
        { %>
    <style type="text/css">
        body {
            overflow: auto;
        }

        .header-fixed .app-body {
            margin-top: 0px;
        }

        .card-login .card-block {
            margin: 0px;
            padding: 0px;
        }

        .content-logo-sm {
            width: 70% !important;
        }
    </style>
    <% } %>


    <div class="row justify-content-center">
        <div class="col-sm-12 col-md-8 col-lg-6">
            <div class="card-group mb-0">
                <div class="card card-login p-2 pt-3">
                    <div class="card-block col-sm-12">

                        <div class="text-center mb-4">
                            <div class="content-logo content-logo-sm" style="width: 50% !important;"></div>
                            <h4 class="text-uppercase text-info">WAREHOUSE MANAGEMENT SYSTEM</h4>
                            <p class="font-sm text-uppercase text-login-desc" style="font-size: 1rem;">sign in to your account</p>
                        </div>

                        <asp:UpdatePanel ID="updateFrom" runat="server" ChildrenAsTriggers="true" UpdateMode="Conditional" class="row pt-2">
                            <ContentTemplate>
                                <div class="col-sm-2 col-md-3 col-lg-2 pt-2">
                                    <h6 class="text-muted font-weight-normal  text-alian">T H E M E</h6>
                                </div>
                                <div class="col-sm-5 col-md-4 col-lg-5 text-alian">
                                    <asp:Button ID="btThemeDark" runat="server" CssClass="btn btn-thm-dark btn-block px-4" Text="D A R K" OnClientClick="changeTheme('DARK');" OnClick="btThemeDark_Click" />
                                </div>
                                <div class="col-sm-5 col-md-4 col-lg-5 text-alian mb-3">
                                    <asp:Button ID="btThemeLight" runat="server" CssClass="btn btn-thm-light btn-block px-4" Text="L I G H T" OnClientClick="changeTheme('');" OnClick="btThemeLight_Click" />
                                </div>

                                <div class="col-sm-12 text-uppercase text-white">
                                    <label class="text-muted text-alian">Username</label>
                                    <asp:TextBox ID="txtUsername" runat="server" CssClass="form-control form-login" MaxLength="50" placeholder="U S E R N A M E" autocomplete="off"></asp:TextBox>
                                </div>
                                <div class="col-sm-12 text-uppercase text-white mt-2">
                                    <label class="text-muted text-alian">Password</label>
                                    <asp:TextBox ID="txtPassword" runat="server" CssClass="form-control form-login" TextMode="Password" MaxLength="100" placeholder="P A S S W O R D" autocomplete="off"
                                        AutoPostBack="false" onkeypress="return onkeyBoxNumber(event,this);"></asp:TextBox>
                                </div>
                                <div class="col-sm-12 mt-3">
                                    <asp:Button ID="btLogin" runat="server" CssClass="btn btn-danger btn-login btn-block text-uppercase px-4 text-alian" Text="L o g i n" OnClick="btLogin_Click" />
                                </div>
                            </ContentTemplate>
                        </asp:UpdatePanel>

                    </div>
                </div>
            </div>
        </div>
    </div>

    <div class="app-footer">
        <span class="float-right version">
            <span>web version </span>
            <asp:Label runat="server" ID="lblVersion"></asp:Label>
        </span>
    </div>


</asp:Content>
