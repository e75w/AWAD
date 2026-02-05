<%@ Page Title="" Language="C#" MasterPageFile="~/Masterpage.Master" AutoEventWireup="true" CodeBehind="Products.aspx.cs" Inherits="_240795P_EvanLim.Products" %>

<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <h2>Our Products</h2>

    <div class="row mb-4">
        <div class="col-md-4">
            <asp:Label runat="server" Text="Category:" AssociatedControlID="ddlCategory" CssClass="form-label" />
            <asp:DropDownList ID="ddlCategory" runat="server" CssClass="form-select" 
                AutoPostBack="true" OnSelectedIndexChanged="ddlCategory_SelectedIndexChanged">
                <asp:ListItem Value="All" Text="All Categories" />
                <asp:ListItem Value="Instruments" Text="Instruments" />
                <asp:ListItem Value="Accessories" Text="Accessories" />
                <asp:ListItem Value="Audio Equipment" Text="Audio Equipment" />
            </asp:DropDownList>
        </div>

        <div class="col-md-6">
            <asp:Label runat="server" Text="Search:" AssociatedControlID="txtSearch" CssClass="form-label" />
            <div class="input-group">
                <asp:TextBox ID="txtSearch" runat="server" CssClass="form-control" Placeholder="Search products..." />
                <asp:Button ID="btnSearch" runat="server" Text="Search" CssClass="btn btn-primary" OnClick="btnSearch_Click" />
                <asp:Button ID="btnReset" runat="server" Text="Reset" CssClass="btn btn-secondary" OnClick="btnReset_Click" />
            </div>
        </div>
    </div>

    <asp:Panel ID="pnlNoResults" runat="server" Visible="false" CssClass="alert alert-warning text-center">
        <h4>No results found for your search.</h4>
        <p>Try checking your spelling or use different keywords.</p>
    </asp:Panel>

    <div class="row">
        <asp:Repeater ID="rptProducts" runat="server" OnItemCommand="rptProducts_ItemCommand">
            <ItemTemplate>
                <div class="col-md-4 mb-4">
                    <div class="card h-100">
                        <img src='<%# Eval("ImageUrl") %>' class="card-img-top" alt="Product Image" style="height:200px; object-fit:contain;">
                        <div class="card-body">
                            <h5 class="card-title"><%# Eval("Name") %></h5>
                            <p class="card-text">$<%# Eval("Price") %></p>
                            <p class="text-muted"><%# Eval("Category") %></p>
                            
                            <asp:Button ID="btnBuy" runat="server" Text="Add to Cart" 
                                CommandName="AddToCart" CommandArgument='<%# Eval("Id") %>' 
                                CssClass="btn btn-success" />
                        </div>
                    </div>
                </div>
            </ItemTemplate>
        </asp:Repeater>
    </div>
</asp:Content>