<%@ Page Title="" Language="C#" MasterPageFile="~/Masterpage.Master" AutoEventWireup="true" CodeBehind="Admin.aspx.cs" Inherits="_240795P_EvanLim.Admin" %>

<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <div class="container-fluid mt-4">
        
        <div class="row mb-4">
            <div class="col-12">
                <h2 class="text-primary border-bottom pb-2">Admin Analytics Dashboard</h2>
            </div>
        </div>

        <div class="row mb-4">
            <div class="col-md-4">
                <div class="card text-white bg-success mb-3 shadow-sm">
                    <div class="card-header">Total Revenue</div>
                    <div class="card-body">
                        <h3 class="card-title"><asp:Label ID="lblTotalRevenue" runat="server" Text="$0.00"></asp:Label></h3>
                        <p class="card-text">Lifetime earnings from all orders.</p>
                    </div>
                </div>
            </div>
            <div class="col-md-4">
                <div class="card text-white bg-info mb-3 shadow-sm">
                    <div class="card-header">Total Orders</div>
                    <div class="card-body">
                        <h3 class="card-title"><asp:Label ID="lblTotalOrders" runat="server" Text="0"></asp:Label></h3>
                        <p class="card-text">Total number of checkout transactions.</p>
                    </div>
                </div>
            </div>
            <div class="col-md-4">
                <div class="card text-white bg-warning mb-3 shadow-sm">
                    <div class="card-header">Top Selling Product</div>
                    <div class="card-body">
                        <h3 class="card-title"><asp:Label ID="lblTopProduct" runat="server" Text="-"></asp:Label></h3>
                        <p class="card-text">Most popular item by quantity sold.</p>
                    </div>
                </div>
            </div>
        </div>

        <div class="row">
            <div class="col-12">
                <h3 class="text-secondary border-bottom pb-2 mt-4">Inventory Management (CRUD)</h3>
            </div>
        </div>

        <div class="row mt-3">
            <div class="col-lg-4 mb-4">
                <div class="card shadow-sm">
                    <div class="card-header bg-dark text-white">
                        <h5 class="mb-0">Add New Product</h5>
                    </div>
                    <div class="card-body">
                        <asp:Label ID="lblMessage" runat="server" Visible="false"></asp:Label>

                        <div class="mb-2">
                            <label>Product Name</label>
                            <asp:TextBox ID="txtName" runat="server" CssClass="form-control" placeholder="e.g. Fender Stratocaster"></asp:TextBox>
                            <asp:RequiredFieldValidator ID="rfvName" runat="server" ControlToValidate="txtName" ErrorMessage="*" ForeColor="Red" ValidationGroup="AddGroup"/>
                        </div>

                        <div class="mb-2">
                            <label>Price ($)</label>
                            <asp:TextBox ID="txtPrice" runat="server" CssClass="form-control" TextMode="Number" step="0.01"></asp:TextBox>
                            <asp:RequiredFieldValidator ID="rfvPrice" runat="server" ControlToValidate="txtPrice" ErrorMessage="*" ForeColor="Red" ValidationGroup="AddGroup"/>
                        </div>

                        <div class="mb-2">
                            <label>Category</label>
                            <asp:DropDownList ID="ddlCategory" runat="server" CssClass="form-select">
                                <asp:ListItem Text="Instruments" Value="Instruments" />
                                <asp:ListItem Text="Audio Equipment" Value="Audio Equipment" />
                                <asp:ListItem Text="Accessories" Value="Accessories" />
                            </asp:DropDownList>
                        </div>

                        <div class="mb-2">
                            <label>Image URL</label>
                            <asp:TextBox ID="txtImage" runat="server" CssClass="form-control" placeholder="/images/guitar.jpg"></asp:TextBox>
                        </div>

                        <div class="mb-3">
                            <label>Description</label>
                            <asp:TextBox ID="txtDesc" runat="server" CssClass="form-control" TextMode="MultiLine" Rows="2"></asp:TextBox>
                        </div>

                        <asp:Button ID="btnAdd" runat="server" Text="Add Product" CssClass="btn btn-success w-100" OnClick="btnAdd_Click" ValidationGroup="AddGroup" />
                    </div>
                </div>
            </div>

            <div class="col-lg-8">
                <div class="card shadow-sm">
                    <div class="card-header bg-secondary text-white">
                        <h5 class="mb-0">Current Stock</h5>
                    </div>
                    <div class="card-body">
                        <div class="table-responsive">
                            <asp:GridView ID="gvAdminProducts" runat="server" AutoGenerateColumns="False" 
                                CssClass="table table-hover align-middle"
                                DataKeyNames="Id"
                                OnRowCommand="gvAdminProducts_RowCommand"
                                EmptyDataText="No inventory found.">
                                <Columns>
                                    <asp:TemplateField HeaderText="Image">
                                        <ItemTemplate>
                                            <img src='<%# Eval("ImageUrl") %>' width="40" height="40" style="object-fit:cover; border-radius:4px;" />
                                        </ItemTemplate>
                                    </asp:TemplateField>

                                    <asp:BoundField DataField="Name" HeaderText="Name" />
                                    <asp:BoundField DataField="Category" HeaderText="Category" />
                                    <asp:BoundField DataField="Price" HeaderText="Price" DataFormatString="{0:C}" />

                                    <asp:TemplateField HeaderText="Actions">
                                        <ItemTemplate>
                                            <asp:Button ID="btnDelete" runat="server" Text="Delete" 
                                                CommandName="DeleteProduct" 
                                                CommandArgument='<%# Container.DataItemIndex %>'
                                                CssClass="btn btn-danger btn-sm"
                                                OnClientClick="return confirm('WARNING: Deleting this product will remove it from the shop.');" />
                                        </ItemTemplate>
                                    </asp:TemplateField>
                                </Columns>
                            </asp:GridView>
                        </div>
                    </div>
                </div>
            </div>
        </div>
    </div>
</asp:Content>