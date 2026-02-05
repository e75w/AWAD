<%@ Page Title="" Language="C#" MasterPageFile="~/Masterpage.Master" AutoEventWireup="true" CodeBehind="Orders.aspx.cs" Inherits="_240795P_EvanLim.Orders" %>

<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <h2>Checkout</h2>
    
    <div class="row">
        <div class="col-md-6">
            <h4>Your Cart</h4>
            <asp:GridView ID="gvCart" runat="server" CssClass="table table-striped" AutoGenerateColumns="False">
                <Columns>
                    <asp:BoundField DataField="Name" HeaderText="Product" />
                    <asp:BoundField DataField="Price" HeaderText="Price" DataFormatString="{0:C}" />
                </Columns>
            </asp:GridView>
            <h4>Total: <asp:Label ID="lblTotal" runat="server" Text="$0.00"></asp:Label></h4>
        </div>

        <div class="col-md-6">
            <h4>Payment Details</h4>
            <div class="mb-3">
                <label>Card Number</label>
                <asp:TextBox ID="txtCard" runat="server" CssClass="form-control"></asp:TextBox>
            </div>
            <asp:Button ID="btnCheckout" runat="server" Text="Pay Now" CssClass="btn btn-success w-100" OnClick="btnCheckout_Click" />
            <asp:Label ID="lblMessage" runat="server" ForeColor="Red"></asp:Label>
        </div>
    </div>
</asp:Content>
