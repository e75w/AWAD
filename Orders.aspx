<%@ Page Title="" Language="C#" MasterPageFile="~/Masterpage.Master" AutoEventWireup="true" CodeBehind="Orders.aspx.cs" Inherits="_240795P_EvanLim.Orders" %>

<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <div class="container mt-5">
        <h2>Your Shopping Cart</h2>
        
        <asp:GridView ID="gvCart" 
                      runat="server" 
                      AutoGenerateColumns="False" 
                      CssClass="table table-striped table-bordered align-middle" 
                      EmptyDataText="Your cart is empty."
                      DataKeyNames="ProductId"
                      OnRowCommand="gvCart_RowCommand">
    
            <Columns>
                <asp:BoundField DataField="Name" HeaderText="Product Name" />
                <asp:BoundField DataField="Price" HeaderText="Price ($)" DataFormatString="{0:N2}" />
                <asp:BoundField DataField="Quantity" HeaderText="Qty" />
                <asp:BoundField DataField="TotalItemPrice" HeaderText="Subtotal ($)" DataFormatString="{0:N2}" />

                <asp:TemplateField HeaderText="Action">
                    <ItemTemplate>
                        <asp:Button ID="btnRemove" runat="server" Text="Remove" 
                            CommandName="RemoveItem" 
                            CommandArgument='<%# Container.DataItemIndex %>'
                            CssClass="btn btn-danger btn-sm"
                            OnClientClick="return confirm('Are you sure you want to remove this item?');" />
                    </ItemTemplate>
                </asp:TemplateField>

            </Columns>
        </asp:GridView>

        <div class="row mt-3">
            <div class="col text-end">
                <h4>Total Amount: <asp:Label ID="lblGrandTotal" runat="server" Text="$0.00" CssClass="text-primary"></asp:Label></h4>
                
                <a href="products.aspx" class="btn btn-secondary">Continue Shopping</a>
                <asp:Button ID="btnCheckout" runat="server" Text="Proceed to Checkout" CssClass="btn btn-success" OnClick="btnCheckout_Click" />
            </div>
        </div>
    </div>
</asp:Content>
