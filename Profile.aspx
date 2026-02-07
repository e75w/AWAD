<%@ Page Title="My Profile" Language="C#" MasterPageFile="~/Masterpage.Master" AutoEventWireup="true" CodeBehind="Profile.aspx.cs" Inherits="_240795P_EvanLim.Profile" %>

<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <div class="container mt-5">
        <div class="row">
            <div class="col-md-4">
                <div class="card shadow-sm mb-4">
                    <div class="card-header bg-primary text-white">
                        <h5 class="mb-0">My Account</h5>
                    </div>
                    <div class="card-body">
                        <div class="mb-3">
                            <label class="fw-bold">Email Address</label>
                            <asp:TextBox ID="txtEmail" runat="server" CssClass="form-control" ReadOnly="true"></asp:TextBox>
                        </div>
                        <div class="mb-3">
                            <label class="fw-bold">Password</label>
                            <div class="input-group">
                                <asp:TextBox ID="txtPassword" runat="server" CssClass="form-control" TextMode="Password" ReadOnly="true" value="********"></asp:TextBox>
                                <button class="btn btn-outline-secondary" type="button" onclick="alert('Password change feature coming soon!');">Change</button>
                            </div>
                        </div>
                    </div>
                </div>

                <div class="card shadow-sm border-warning">
                    <div class="card-header bg-warning">
                        <h5 class="mb-0">Security</h5>
                    </div>
                    <div class="card-body text-center">
                        <asp:Label ID="lblMFAStatus" runat="server" CssClass="d-block fw-bold mb-2"></asp:Label>
                        
                        <asp:Button ID="btnEnableMFA" runat="server" Text="Enable Two-Factor Auth" 
                            CssClass="btn btn-dark w-100" OnClick="btnEnableMFA_Click" Visible="false" />
                    </div>
                </div>
            </div>

            <div class="col-md-8">
                <div class="card shadow-sm">
                    <div class="card-header bg-success text-white">
                        <h5 class="mb-0">Order History</h5>
                    </div>
                    <div class="card-body">
                        <asp:GridView ID="gvOrders" runat="server" CssClass="table table-striped table-hover" AutoGenerateColumns="False" EmptyDataText="No orders found.">
                            <Columns>
                                <asp:BoundField DataField="Id" HeaderText="Order ID" ItemStyle-Width="200px" />
                                <asp:BoundField DataField="OrderDate" HeaderText="Date" DataFormatString="{0:MMM dd, yyyy}" />
                                <asp:BoundField DataField="TotalAmount" HeaderText="Total" DataFormatString="{0:C}" />
                                <asp:TemplateField HeaderText="Status">
                                    <ItemTemplate>
                                        <span class="badge bg-success">Completed</span>
                                    </ItemTemplate>
                                </asp:TemplateField>
                            </Columns>
                        </asp:GridView>
                    </div>
                </div>
            </div>
        </div>
    </div>
</asp:Content>