<%@ Page Title="" Language="C#" MasterPageFile="~/Layout.Master" AutoEventWireup="true" CodeBehind="ChangePass.aspx.cs" Inherits="WMS_NEW.ChangePass" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <div class="row justify-content-center" style="margin-top: 2%">
        <div class="col-sm-12 col-md10 col-lg-8">
            <div class="card card-accent-info">
                <div class="card-header bg-info">
                    <span class="text-uppercase text-white text-bold text-xl-center">CHANGE PASSWORD</span>
                </div>
                <div class="card-block col-sm-12 pt-4 p-4">
                    <asp:UpdatePanel ID="updateFrom" runat="server" ChildrenAsTriggers="true" UpdateMode="Conditional" class="row">
                        <ContentTemplate>
                            <div class="col-sm-12 text-muted">
                                <label class="text-uppercase">New Password</label>
                                <asp:TextBox ID="txtNewPass" runat="server" CssClass="form-control" TextMode="Password" placeholder="N E W   P A S S W O R D" autocomplete="off"></asp:TextBox>
                            </div>
                            <div class="col-sm-12 text-muted mt-2">
                                <label class="text-uppercase">Confirm Password</label>
                                <asp:TextBox ID="txtNewPassConf" runat="server" CssClass="form-control" TextMode="Password" placeholder="C O N F I R M   P A S S W O R D" autocomplete="off"></asp:TextBox>
                            </div>
                            <div class="col-sm-12 mt-4">
                                <asp:Button ID="btChange" runat="server" Text="Confirm Change" CssClass="btn btn-danger btn-block text-uppercase px-4" OnClick="btChange_Click" />
                            </div>
                        </ContentTemplate>
                    </asp:UpdatePanel>

                </div>
            </div>
        </div>
    </div>
</asp:Content>
