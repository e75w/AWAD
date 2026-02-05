<%@ Page Title="" Language="C#" MasterPageFile="~/Masterpage.Master" AutoEventWireup="true" CodeBehind="Admin.aspx.cs" Inherits="_240795P_EvanLim.Admin" %>

<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    
    <h1>Admin Dashboard</h1>
    <hr />

    <div class="row mb-5">
        <div class="col-md-4">
            <div class="card bg-success text-white">
                <div class="card-body">
                    <h3>Total Revenue</h3>
                    <h2><asp:Label ID="lblRevenue" runat="server"></asp:Label></h2>
                </div>
            </div>
        </div>
        <div class="col-md-4">
             <div class="card bg-info text-white">
                <div class="card-body">
                    <h3>Total Orders</h3>
                    <h2><asp:Label ID="lblOrdersCount" runat="server"></asp:Label></h2>
                </div>
            </div>
        </div>
    </div>

    <h3>Manage Products</h3>
    
    <div class="card p-3 mb-4 bg-light">
        <h5>Add New Product</h5>
        <div class="row">
            <div class="col-md-3"><asp:TextBox ID="txtNewName" runat="server" Placeholder="Name" CssClass="form-control"></asp:TextBox></div>
            <div class="col-md-2"><asp:TextBox ID="txtNewPrice" runat="server" Placeholder="Price" CssClass="form-control"></asp:TextBox></div>
            <div class="col-md-3"><asp:TextBox ID="txtNewImg" runat="server" Placeholder="Image URL" CssClass="form-control"></asp:TextBox></div>
             <div class="col-md-2">
                 <asp:DropDownList ID="ddlNewCat" runat="server" CssClass="form-select">
                     <asp:ListItem>Instruments</asp:ListItem>
                     <asp:ListItem>Accessories</asp:ListItem>
                 </asp:DropDownList>
             </div>
            <div class="col-md-2"><asp:Button ID="btnAdd" runat="server" Text="Add" CssClass="btn btn-primary" OnClick="btnAdd_Click" /></div>
        </div>
    </div>

    <asp:GridView ID="gvProducts" runat="server" CssClass="table table-bordered" AutoGenerateColumns="False"
        DataKeyNames="Id" 
        OnRowEditing="gvProducts_RowEditing" 
        OnRowCancelingEdit="gvProducts_RowCancelingEdit" 
        OnRowUpdating="gvProducts_RowUpdating" 
        OnRowDeleting="gvProducts_RowDeleting">
        <Columns>
            <asp:BoundField DataField="Id" HeaderText="ID" ReadOnly="True" />
            <asp:BoundField DataField="Name" HeaderText="Name" />
            <asp:BoundField DataField="Price" HeaderText="Price" />
            <asp:BoundField DataField="Category" HeaderText="Category" />
            
            <asp:CommandField ShowEditButton="True" ShowDeleteButton="True" ControlStyle_CssClass="btn btn-sm btn-secondary" />
        </Columns>
    </asp:GridView>

</asp:Content>