<%@ Page Title="Verify Login" Language="C#" MasterPageFile="~/Masterpage.Master" AutoEventWireup="true" CodeBehind="MFA.aspx.cs" Inherits="_240795P_EvanLim.MFA" %>

<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <div class="container mt-5">
        <div class="card shadow-sm col-md-6 mx-auto">
            <div class="card-header bg-primary text-white">
                <h4>Security Verification</h4>
            </div>
            <div class="card-body text-center">
                
                <asp:Panel ID="pnlEmail" runat="server">
                    <h5>Step 1: Email Verification</h5>
                    <p>Please enter the code sent to your email.</p>
                    <asp:TextBox ID="txtEmailCode" runat="server" CssClass="form-control text-center mb-3" placeholder="Email Code"></asp:TextBox>
                    <asp:Button ID="btnVerifyEmail" runat="server" Text="Next >" CssClass="btn btn-primary w-100" OnClick="btnVerifyEmail_Click" />
                </asp:Panel>

                <asp:Panel ID="pnlApp" runat="server" Visible="false">
                    <h5>Step 2: App Verification</h5>
                    <p>Great! Now enter the code from your Authenticator App.</p>
                    <asp:TextBox ID="txtAppCode" runat="server" CssClass="form-control text-center mb-3" placeholder="App Code"></asp:TextBox>
                    <asp:Button ID="btnVerifyApp" runat="server" Text="Verify & Login" CssClass="btn btn-success w-100" OnClick="btnVerifyApp_Click" />
                </asp:Panel>

                <asp:Label ID="lblMessage" runat="server" CssClass="d-block mt-3 fw-bold text-danger"></asp:Label>
            </div>
        </div>
    </div>
</asp:Content>