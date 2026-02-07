<%@ Page Title="" Language="C#" MasterPageFile="~/Masterpage.Master" AutoEventWireup="true" CodeBehind="ProductDetails.aspx.cs" Inherits="_240795P_EvanLim.ProductDetails" %>

<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <div class="container mt-5">
        
        <a href="Products" class="btn btn-outline-secondary mb-4">&larr; Back to Shop</a>

        <div class="row">
            <div class="col-md-6 text-center">
                <asp:Image ID="imgProduct" runat="server" CssClass="img-fluid border rounded shadow-sm" style="max-height: 400px;" />
            </div>

            <div class="col-md-6">
                <h2 class="mb-3"><asp:Label ID="lblName" runat="server"></asp:Label></h2>
                <h4 class="text-primary mb-3">Price: $<asp:Label ID="lblPrice" runat="server"></asp:Label></h4>
                
                <p class="text-muted"><strong>Category:</strong> <asp:Label ID="lblCategory" runat="server"></asp:Label></p>
                
                <hr />
                
                <p><strong>Description:</strong></p>
                <p><asp:Label ID="lblDescription" runat="server"></asp:Label></p>

                <p class="text-muted">
                    Availability: <asp:Label ID="lblStock" runat="server" Text="Checking..." Font-Bold="true"></asp:Label>
                </p>

                <div class="d-flex mt-3">
                    <asp:TextBox ID="txtQty" runat="server" TextMode="Number" Text="1" CssClass="form-control w-25 me-2"></asp:TextBox>
                    <asp:Button ID="btnAddToCart" runat="server" Text="Add to Cart" CssClass="btn btn-primary" OnClick="btnAddToCart_Click" />
                </div>
                <asp:Label ID="lblError" runat="server" ForeColor="Red" Visible="false"></asp:Label>
            </div>
        </div>
    </div>
</asp:Content>