<%@ Page Title="Setup App 2FA" Language="C#" MasterPageFile="~/Masterpage.Master" AutoEventWireup="true" CodeBehind="Setup2FA.aspx.cs" Inherits="_240795P_EvanLim.Setup2FA" %>

<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <div class="container mt-5">
        <div class="card shadow-sm">
            <div class="card-header bg-primary text-white">
                <h4>Setup Microsoft/Google Authenticator</h4>
            </div>
            <div class="card-body text-center">
                <p>1. Open your Authenticator App.</p>
                <p>2. Scan the QR code below.</p>
                
                <div class="my-3">
                    <asp:Image ID="imgQRCode" runat="server" CssClass="img-fluid border" />
                </div>
                
                <p>3. Enter the 6-digit code from the app to confirm.</p>
                <div class="row justify-content-center">
                    <div class="col-md-4">
                        <asp:TextBox ID="txtCode" runat="server" CssClass="form-control text-center fs-4" MaxLength="6" placeholder="000000"></asp:TextBox>
                    </div>
                </div>

                <div class="mt-3">
                    <asp:Button ID="btnConfirm" runat="server" Text="Enable App 2FA" CssClass="btn btn-success" OnClick="btnConfirm_Click" />
                </div>
                <asp:Label ID="lblMessage" runat="server" CssClass="mt-3 d-block fw-bold"></asp:Label>
            </div>
        </div>
    </div>
</asp:Content>