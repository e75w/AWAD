<%@ Page Title="" Language="C#" MasterPageFile="~/Masterpage.Master" AutoEventWireup="true" CodeBehind="Payment.aspx.cs" Inherits="_240795P_EvanLim.Payment" %>

<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <div class="container mt-5">
        <div class="row justify-content-center">
            <div class="col-md-6">
                <div class="card">
                    <div class="card-header bg-success text-white">
                        <h3>Secure Payment</h3>
                    </div>
                    <div class="card-body">
                        <div class="alert alert-info text-center">
                            <h5>Total to Pay: <asp:Label ID="lblTotalAmount" runat="server" Text="$0.00" Font-Bold="true"></asp:Label></h5>
                        </div>

                        <div class="mb-3">
                            <label>Name on Card</label>
                            <asp:TextBox ID="txtName" runat="server" CssClass="form-control" placeholder="e.g. John Doe"></asp:TextBox>
                            <asp:RequiredFieldValidator ID="rfvName" runat="server" ControlToValidate="txtName" ErrorMessage="Name is required" CssClass="text-danger" Display="Dynamic" />
                        </div>

                        <div class="mb-3">
                            <label>Card Number</label>
                            <asp:TextBox ID="txtCardNum" runat="server" CssClass="form-control" placeholder="0000 0000 0000 0000" MaxLength="16"></asp:TextBox>
                            <asp:RequiredFieldValidator ID="rfvCard" runat="server" ControlToValidate="txtCardNum" ErrorMessage="Card number is required" CssClass="text-danger" Display="Dynamic" />
                        </div>

                        <div class="row">
                            <div class="col-md-6 mb-3">
                                <label>Expiry Date (MM/YY)</label>
                                <asp:TextBox ID="txtExpiry" runat="server" CssClass="form-control" placeholder="MM/YY" MaxLength="5"></asp:TextBox>
                            </div>
                            <div class="col-md-6 mb-3">
                                <label>CVV</label>
                                <asp:TextBox ID="txtCVV" runat="server" CssClass="form-control" placeholder="123" MaxLength="3" TextMode="Password"></asp:TextBox>
                            </div>
                        </div>

                        <div class="mb-3">
                            <label>Billing Address</label>
                            <asp:TextBox ID="txtAddress" runat="server" CssClass="form-control" TextMode="MultiLine" Rows="2"></asp:TextBox>
                            <asp:RequiredFieldValidator ID="rfvAddress" runat="server" ControlToValidate="txtAddress" ErrorMessage="Address is required" CssClass="text-danger" Display="Dynamic" />
                        </div>

                        <asp:Button ID="btnPay" runat="server" Text="Confirm Payment" CssClass="btn btn-success w-100 btn-lg" OnClick="btnPay_Click" />
                        
                        <div class="mt-3 text-center">
                            <a href="orders.aspx" class="text-secondary">Back to Cart</a>
                        </div>
                    </div>
                </div>
            </div>
        </div>
    </div>
</asp:Content>