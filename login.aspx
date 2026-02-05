<%@ Page Title="" Language="C#" MasterPageFile="~/Masterpage.Master" AutoEventWireup="true" CodeBehind="login.aspx.cs" Inherits="_240795P_EvanLim.login" %>

<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <div class="row justify-content-center">
        <div class="col-md-4">
            <h3>Login</h3>
            <div class="mb-3">
                <label>Email</label>
                <asp:TextBox ID="txtEmail" runat="server" CssClass="form-control"></asp:TextBox>
            </div>
            <div class="mb-3">
                <label>Password</label>
                <asp:TextBox ID="txtPass" runat="server" CssClass="form-control" TextMode="Password"></asp:TextBox>
            </div>
            <asp:Button ID="btnLogin" runat="server" Text="Login" CssClass="btn btn-primary w-100" OnClick="btnLogin_Click" />
            
            <!-- Register -->
            <div class="mt-3 text-center">
                <p>Don't have an account?</p>
                <a href="Register" class="btn btn-outline-secondary w-100">Create New Account</a>
            </div>

            <asp:Label ID="lblError" runat="server" ForeColor="Red" CssClass="mt-2"></asp:Label>
        </div>
    </div>
</asp:Content>
