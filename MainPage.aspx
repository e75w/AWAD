<%@ Page Title="" Language="C#" MasterPageFile="~/Masterpage.Master" AutoEventWireup="true" CodeBehind="MainPage.aspx.cs" Inherits="_240795P_EvanLim.MainPage" %>

<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    
    <div id="homeCarousel" class="carousel slide mb-5" data-bs-ride="carousel">
        <div class="carousel-inner">
            <asp:Repeater ID="rptCarousel" runat="server">
                <ItemTemplate>
                    <div class='carousel-item <%# Container.ItemIndex == 0 ? "active" : "" %>'>
                        <img src='<%# Eval("ImageUrl") %>' class="d-block w-100" style="height:400px; object-fit:cover;">
                        <div class="carousel-caption">
                            <h3 style="background-color: rgba(0, 0, 0, 0.6); padding: 10px 20px; border-radius: 5px; display: inline-block;">
                                <%# Eval("Name") %>
                            </h3>
                        </div>
                    </div>
                </ItemTemplate>
            </asp:Repeater>
        </div>
    </div>

    <h2>Welcome to MusicStore</h2>
    <p>Check out our latest instruments and deals.</p>
    <a href="Products" class="btn btn-primary">View Full Catalogue</a>
</asp:Content>
