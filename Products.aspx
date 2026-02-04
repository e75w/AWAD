<%@ Page Title="" Language="C#" MasterPageFile="~/Masterpage.Master" AutoEventWireup="true" CodeBehind="Products.aspx.cs" Inherits="_240795P_EvanLim.WebForm2" %>

<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <h2>Our Products</h2>

    <div class="row mb-4">
        <div class="col-md-4">
            <asp:TextBox ID="txtSearch" runat="server" CssClass="form-control" placeholder="Search..."></asp:TextBox>
        </div>
        <div class="col-md-3">
            <asp:DropDownList ID="ddlCategory" runat="server" CssClass="form-select">
                <asp:ListItem Text="All Categories" Value="" />
                <asp:ListItem Text="Instruments" Value="Instruments" />
                <asp:ListItem Text="Accessories" Value="Accessories" />
            </asp:DropDownList>
        </div>
        <div class="col-md-2">
            <asp:Button ID="btnFilter" runat="server" Text="Filter" CssClass="btn btn-primary" OnClick="btnFilter_Click" />
        </div>
    </div>

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