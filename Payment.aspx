<%@ Page Title="Payment" Language="C#" MasterPageFile="~/Masterpage.Master" AutoEventWireup="true" CodeBehind="Payment.aspx.cs" Inherits="_240795P_EvanLim.Payment" %>

<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <div class="container mt-5">
        <div class="row justify-content-center">
            <div class="col-md-7">
                <div class="card shadow-sm">
                    <div class="card-header bg-success text-white">
                        <h3 class="mb-0">Checkout</h3>
                    </div>
                    <div class="card-body">
                        
                        <div class="alert alert-info text-center">
                            <h5>Total to Pay: <asp:Label ID="lblTotalAmount" runat="server" Text="$0.00" Font-Bold="true"></asp:Label></h5>
                        </div>

                        <div class="mb-3">
                            <label class="form-label">Full Name</label>
                            <asp:TextBox ID="txtName" runat="server" CssClass="form-control" placeholder="e.g. John Doe"></asp:TextBox>
                            <asp:RequiredFieldValidator ID="rfvName" runat="server" ControlToValidate="txtName" 
                                ErrorMessage="Name is required" CssClass="text-danger fw-bold" Display="Dynamic" />
                        </div>

                        <div class="mb-3">
                            <label class="form-label">Shipping Address</label>
                            <asp:TextBox ID="txtAddress" runat="server" CssClass="form-control" TextMode="MultiLine" Rows="2" placeholder="123 Orchard Road..."></asp:TextBox>
                            <asp:RequiredFieldValidator ID="rfvAddress" runat="server" ControlToValidate="txtAddress" 
                                ErrorMessage="Address is required" CssClass="text-danger fw-bold" Display="Dynamic" />
                        </div>

                        <hr />

                        <div id="paypal-button-container"></div>
                        
                        <asp:Button ID="btnCompletePayment" runat="server" Text="Complete" OnClick="btnCompletePayment_Click" style="display:none;" />
                        
                        <div class="mt-3 text-center">
                            <a href="Orders" class="text-secondary">Back to Cart</a>
                        </div>

                    </div>
                </div>
            </div>
        </div>
    </div>

    <script src="https://www.paypal.com/sdk/js?client-id=YOUR_CLIENT_ID_HERE&currency=USD"></script>

    <script>
        paypal.Buttons({
            // 1. MANUAL VALIDATION (Bypasses the broken ASP.NET check)
            onClick: function (data, actions) {
                // Get the values directly from the text boxes
                var name = document.getElementById('<%= txtName.ClientID %>').value;
                var address = document.getElementById('<%= txtAddress.ClientID %>').value;
            
                // Check if they are empty
                if (name.trim() === "" || address.trim() === "") {
                    alert("Please fill in your Name and Shipping Address before paying.");
                    return actions.reject(); // This STOPS the PayPal popup from opening
                }
            },

            // 2. CREATE ORDER
            createOrder: function (data, actions) {
                return actions.order.create({
                    purchase_units: [{
                        amount: {
                            value: '<%= TotalAmount %>' 
                        }
                    }]
                });
            },

            // 3. ON APPROVE
            onApprove: function (data, actions) {
                return actions.order.capture().then(function (details) {
                    // Click the hidden button to save to database
                    document.getElementById('<%= btnCompletePayment.ClientID %>').click();
                });
            },

            // 4. ON ERROR
            onError: function (err) {
                console.error(err);
                alert('An error occurred. Please check the console for details.');
            }
        }).render('#paypal-button-container');
    </script>
</asp:Content>