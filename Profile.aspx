<%@ Page Title="My Profile" Language="C#" MasterPageFile="~/Masterpage.Master" AutoEventWireup="true" CodeBehind="Profile.aspx.cs" Inherits="_240795P_EvanLim.Profile" %>

<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <div class="container mt-5">
        <div class="row">
            <div class="col-md-5">
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
                                <asp:TextBox ID="txtPasswordDisplay" runat="server" CssClass="form-control" TextMode="Password" value="DummyPass" ReadOnly="true"></asp:TextBox>
                                <asp:Button ID="btnInitView" runat="server" Text="👁 View" CssClass="btn btn-outline-secondary" OnClick="btnInitView_Click" />
                            </div>
                        </div>

                        <asp:Panel ID="pnlVerification" runat="server" Visible="false" CssClass="alert alert-secondary mt-3">
                            <h6 class="fw-bold">Security Verification</h6>
                            <p class="small mb-2">Please enter your current password to view details.</p>
                            
                            <div class="mb-2">
                                <asp:TextBox ID="txtVerifyPass" runat="server" CssClass="form-control" TextMode="Password" placeholder="Current Password"></asp:TextBox>
                            </div>
                            <asp:Button ID="btnVerifyPass" runat="server" Text="Verify Password" CssClass="btn btn-primary btn-sm w-100" OnClick="btnVerifyPass_Click" />

                            <asp:Panel ID="pnlMFA" runat="server" Visible="false" CssClass="mt-3 pt-3 border-top border-secondary">
                                <p class="small mb-2 text-warning fw-bold">Two-Factor Authentication Required</p>
                                <p class="small">Enter the code from your App or Email.</p>
                                <div class="mb-2">
                                    <asp:TextBox ID="txtMFA" runat="server" CssClass="form-control" placeholder="6-Digit Code" MaxLength="6"></asp:TextBox>
                                </div>
                                <asp:Button ID="btnVerifyMFA" runat="server" Text="Verify Code" CssClass="btn btn-success btn-sm w-100" OnClick="btnVerifyMFA_Click" />
                            </asp:Panel>

                            <asp:Label ID="lblVerifyMsg" runat="server" CssClass="d-block mt-2 small text-danger"></asp:Label>
                        </asp:Panel>

                    </div>
                </div>

                <div class="card shadow-sm border-warning">
                    <div class="card-header bg-warning">
                        <h5 class="mb-0">Security Settings</h5>
                    </div>
                    <div class="card-body text-center">
                        <asp:Label ID="lblMFAStatus" runat="server" CssClass="d-block fw-bold mb-2"></asp:Label>
                        <asp:Button ID="btnEnableMFA" runat="server" Text="Enable 2FA" CssClass="btn btn-dark w-100" OnClick="btnEnableMFA_Click" Visible="false" />
                    </div>
                </div>
            </div>

            <div class="col-md-7">
                <div class="card shadow-sm">
                    <div class="card-header bg-success text-white">
                        <h5 class="mb-0">Order History</h5>
                    </div>
                    <div class="card-body">
                        <asp:GridView ID="gvOrders" runat="server" CssClass="table table-striped table-hover" AutoGenerateColumns="False" EmptyDataText="No orders found.">
                            <Columns>
                                <asp:BoundField DataField="Id" HeaderText="Order ID" ItemStyle-Width="150px" />
                                <asp:BoundField DataField="OrderDate" HeaderText="Date" DataFormatString="{0:MMM dd, yyyy}" />
                                <asp:BoundField DataField="TotalAmount" HeaderText="Total" DataFormatString="{0:C}" />
                                <asp:TemplateField HeaderText="Status">
                                    <ItemTemplate><span class="badge bg-success">Paid</span></ItemTemplate>
                                </asp:TemplateField>
                            </Columns>
                        </asp:GridView>
                    </div>
                </div>
            </div>
        </div>
    </div>
</asp:Content>