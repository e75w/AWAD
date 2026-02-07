<%@ Page Title="Verify Login" Language="C#" MasterPageFile="~/Masterpage.Master" AutoEventWireup="true" CodeBehind="MFA.aspx.cs" Inherits="_240795P_EvanLim.MFA" %>

<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <div class="container mt-5">
        <div class="row justify-content-center">
            <div class="col-md-5">
                <div class="card shadow-sm">
                    <div class="card-header bg-primary text-white text-center">
                        <h4 class="mb-0">Two-Factor Authentication</h4>
                    </div>
                    <div class="card-body text-center p-4">
                        <p class="mb-3">We have sent a verification code to your email.<br />Please enter it below to continue.</p>
                        
                        <div class="mb-3">
                            <asp:TextBox ID="txtOTP" runat="server" CssClass="form-control text-center fs-4" placeholder="000000" MaxLength="6" autocomplete="off"></asp:TextBox>
                        </div>

                        <asp:Label ID="lblMessage" runat="server" CssClass="d-block mb-3 text-danger fw-bold"></asp:Label>
                        
                        <asp:Button ID="btnVerify" runat="server" Text="Verify Login" CssClass="btn btn-success w-100 btn-lg" OnClick="btnVerify_Click" />
                        
                        <div class="mt-3">
                            <asp:LinkButton ID="btnResend" runat="server" Text="Resend Code" CssClass="text-decoration-none small" OnClick="btnResend_Click"></asp:LinkButton>
                        </div>
                    </div>
                </div>
            </div>
        </div>
    </div>
</asp:Content>