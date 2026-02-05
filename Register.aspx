<%@ Page Title="" Language="C#" MasterPageFile="~/Masterpage.Master" AutoEventWireup="true" CodeBehind="Register.aspx.cs" Inherits="_240795P_EvanLim.Register" %>

<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <div class="row justify-content-center">
        <div class="col-md-6">
            <div class="card mt-5">
                <div class="card-header bg-primary text-white">
                    <h3>Create an Account</h3>
                </div>
                <div class="card-body">
                    <asp:Label ID="lblMessage" runat="server" CssClass="text-danger" Visible="false"></asp:Label>

                    <div class="mb-3">
                        <label>Email Address</label>
                        <asp:TextBox ID="txtEmail" runat="server" CssClass="form-control" TextMode="Email"></asp:TextBox>
                    </div>

                    <div class="mb-3">
                        <label>Password</label>
                        <asp:TextBox ID="txtPassword" runat="server" CssClass="form-control" TextMode="Password"></asp:TextBox>
                    </div>

                    <div class="mb-3">
                        <label>Confirm Password</label>
                        <asp:TextBox ID="txtConfirmPass" runat="server" CssClass="form-control" TextMode="Password"></asp:TextBox>
                    </div>

                    <asp:Button ID="btnRegister" runat="server" Text="Register" CssClass="btn btn-success w-100" OnClick="btnRegister_Click" />
                    
                    <hr />
                    <a href="login" class="btn btn-link w-100">Already have an account? Login here.</a>
                </div>
            </div>
        </div>
    </div>
</asp:Content>