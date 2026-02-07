<%@ Page Title="" Language="C#" MasterPageFile="~/Masterpage.Master" AutoEventWireup="true" CodeBehind="Admin.aspx.cs" Inherits="_240795P_EvanLim.Admin" %>

<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <div class="container-fluid mt-4">
        
        <!-- Analytics -->
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

        <!-- Charts n stuff -->
        <div class="row mb-4">
            <div class="col-lg-6">
                <div class="card shadow-sm">
                    <div class="card-header bg-primary text-white">
                        <h5 class="mb-0">Stock Levels (Hover for Details)</h5>
                    </div>
                    <div class="card-body">
                        <div style="height: 400px; overflow-y: auto;">
                            <div style="height: 1000px;"> <canvas id="stockChart"></canvas>
                            </div>
                        </div>
                    </div>
                </div>
            </div>

            <div class="col-lg-6">
                <div class="card shadow-sm">
                    <div class="card-header bg-success text-white">
                        <h5 class="mb-0">Revenue Trend</h5>
                    </div>
                    <div class="card-body">
                        <canvas id="revenueChart" height="200"></canvas>
                    </div>
                </div>
            </div>
        </div>

        <asp:HiddenField ID="hfStockLabels" runat="server" />
        <asp:HiddenField ID="hfStockData" runat="server" />
        <asp:HiddenField ID="hfRevLabels" runat="server" />
        <asp:HiddenField ID="hfRevData" runat="server" />

        <script src="https://cdn.jsdelivr.net/npm/chart.js"></script>
        <script>
            document.addEventListener("DOMContentLoaded", function () {
                // --- 1. STOCK CHART (Horizontal) ---
                var stockCtx = document.getElementById('stockChart').getContext('2d');
                var stockLabelsRaw = document.getElementById('<%= hfStockLabels.ClientID %>').value;
                var stockDataRaw = document.getElementById('<%= hfStockData.ClientID %>').value;

                // Parse CSV strings into Arrays (Handle empty data safely)
                var stockLabels = stockLabelsRaw ? stockLabelsRaw.split(',') : [];
                var stockData = stockDataRaw ? stockDataRaw.split(',').map(Number) : [];

                new Chart(stockCtx, {
                    type: 'bar',
                    data: {
                        labels: stockLabels,
                        datasets: [{
                            label: 'Units in Stock',
                            data: stockData,
                            backgroundColor: 'rgba(54, 162, 235, 0.7)',
                            borderColor: 'rgba(54, 162, 235, 1)',
                            borderWidth: 1
                        }]
                    },
                    options: {
                        indexAxis: 'y', // Makes it Horizontal
                        maintainAspectRatio: false, // Allows it to stretch inside our scrollable div
                        plugins: {
                            tooltip: {
                                enabled: true, // ENABLES HOVER
                                callbacks: {
                                    label: function (context) {
                                        return context.raw + ' items left'; // Custom text on hover
                                    }
                                }
                            }
                        },
                        scales: {
                            x: { beginAtZero: true }
                        }
                    }
                });

                // --- 2. REVENUE CHART (Line) ---
                var revCtx = document.getElementById('revenueChart').getContext('2d');
                var revLabelsRaw = document.getElementById('<%= hfRevLabels.ClientID %>').value;
                var revDataRaw = document.getElementById('<%= hfRevData.ClientID %>').value;

                var revLabels = revLabelsRaw ? revLabelsRaw.split(',') : [];
                var revData = revDataRaw ? revDataRaw.split(',').map(Number) : [];

                new Chart(revCtx, {
                    type: 'line',
                    data: {
                        labels: revLabels,
                        datasets: [{
                            label: 'Revenue ($)',
                            data: revData,
                            borderColor: 'rgba(75, 192, 192, 1)',
                            backgroundColor: 'rgba(75, 192, 192, 0.2)',
                            fill: true,
                            tension: 0.3 // Makes line curvy
                        }]
                    },
                    options: {
                        responsive: true,
                        plugins: {
                            tooltip: {
                                callbacks: {
                                    label: function (context) {
                                        return '$' + context.raw; // Shows $ on hover
                                    }
                                }
                            }
                        }
                    }
                });
            });
        </script>

        <!-- Inventory -->
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
                            <label>Stock Quantity</label>
                            <asp:TextBox ID="txtStock" runat="server" CssClass="form-control" TextMode="Number"></asp:TextBox>
                        </div>

                        <div class="mb-3">
                            <label>Description</label>
                            <asp:TextBox ID="txtDesc" runat="server" CssClass="form-control" TextMode="MultiLine" Rows="2"></asp:TextBox>
                        </div>

                        <asp:Button ID="btnAdd" runat="server" Text="Add Product" CssClass="btn btn-success w-100" OnClick="btnAdd_Click" ValidationGroup="AddGroup" />
                        <asp:HiddenField ID="hfProductId" runat="server" /> 
                        <asp:Button ID="btnUpdate" runat="server" Text="Save Changes" CssClass="btn btn-primary w-100 mt-2" 
                            OnClick="btnUpdate_Click" Visible="false" ValidationGroup="AddGroup" />

                        <asp:Button ID="btnCancel" runat="server" Text="Cancel Editing" CssClass="btn btn-secondary w-100 mt-2" 
                            OnClick="btnCancel_Click" Visible="false" CausesValidation="false" />
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
                                            <asp:Button ID="btnEdit" runat="server" Text="Edit" 
                                                            CommandName="EditProduct" 
                                                            CommandArgument='<%# Container.DataItemIndex %>' 
                                                            CssClass="btn btn-warning btn-sm me-2" />
            
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