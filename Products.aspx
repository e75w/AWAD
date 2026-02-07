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
                    <div class="card h-100 shadow-sm">
                
                        <a href="ProductDetails.aspx?id=<%# Eval("Id") %>">
                            <img src='<%# Eval("ImageUrl") %>' class="card-img-top" alt="Product Image" style="height: 200px; object-fit: contain; padding: 10px;">
                        </a>

                        <div class="card-body">
                            <h5 class="card-title">
                                <a href="ProductDetails?id=<%# Eval("Id") %>" style="text-decoration:none; color:inherit;">
                                    <%# Eval("Name") %>
                                </a>
                            </h5>
    
                            <h6 class="card-subtitle mb-2 text-muted">Price: $<%# Eval("Price", "{0:N2}") %></h6>
    
                            <h6 class="card-subtitle mb-2" style='<%# Convert.ToInt32(Eval("Stock")) > 0 ? "color:green;" : "color:red;" %>'>
                                <%# Convert.ToInt32(Eval("Stock")) > 0 ? "Stock: " + Eval("Stock") + " left" : "Out of Stock" %>
                            </h6>

                            <p class="card-text text-truncate"><%# Eval("Description") %></p>
                        </div>

                        <div class="card-footer bg-white border-top-0">
                            <div class="row g-2 align-items-center">
                                <div class="col-auto" runat="server" Visible='<%# Convert.ToInt32(Eval("Stock")) > 0 %>'>
                                    <label class="col-form-label">Qty:</label>
                                </div>
                                <div class="col-auto" runat="server" Visible='<%# Convert.ToInt32(Eval("Stock")) > 0 %>'>
                                    <asp:TextBox ID="txtQuantity" runat="server" TextMode="Number" Text="1" min="1" max='<%# Eval("Stock") %>' CssClass="form-control form-control-sm" Width="60px"></asp:TextBox>
                                </div>
        
                                <div class="col">
                                    <asp:Button ID="btnAddToCart" runat="server" Text="Add to Cart" 
                                        CommandName="AddToCart" 
                                        CommandArgument='<%# Eval("Id") %>' 
                                        CssClass="btn btn-primary btn-sm w-100"
                                        Visible='<%# Convert.ToInt32(Eval("Stock")) > 0 %>' />

                                    <asp:Label ID="lblSoldOut" runat="server" Text="Sold Out" 
                                        CssClass="btn btn-secondary btn-sm w-100 disabled"
                                        Visible='<%# Convert.ToInt32(Eval("Stock")) <= 0 %>' />
                                </div>
                            </div>
                        </div>

                    </div>
                </div>

            </ItemTemplate>
        </asp:Repeater>
    </div>
</asp:Content>