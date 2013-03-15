<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="Order_Confirmation.aspx.cs" Inherits="BlueIkons.Order_Confirmation" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title></title>
    <link href="BlueIkons.css" media="screen" rel="stylesheet" type="text/css" />  
</head>
<body style="background-image:url('images/BlueIkonsbackground.jpg');background-repeat:no-repeat;
background-attachment:fixed;
background-position:center; ">
    <form id="form1" runat="server">
    <script language=javascript>
        function gotourl(theurl) {
            top.location.href = theurl;
        }    
    </script>    
    <div>
    <center>
    <div class="transbox">
        <asp:Panel ID=pnlworked runat=server>
        Your BlueIkons gift has been sent to your friend's email.  An email was also sent to you confirming your gift.
        <br /><br />
        You will receive an email when the gift has been received.<br />
        Generous Human ID: <asp:Label ID=lblid runat=server></asp:Label>

        <br /><br /><br />
        That was fun! Now what?
        <br />
        <a href="default.aspx">Send More BlueIkons</a>
        <br />
        <a href="http://www.facebook.com/sharer.php
?u=http%3A%2F%2Fapps.facebook.com%2Fblueikons%2Fdefault.aspx
&t=BlueIkons" target=_blank>Share BlueIkons with friends</a>
        </asp:Panel>
        
        <asp:Panel ID=pnlerror Visible=false runat=server>  
        Uh-oh, uno problemo.  We couldn’t deliver your Cash Gift and haven’t charged you.
        <br /><br />
        Now What?<br />
        <a href="http://www.PayPal.com">Check my PayPal Account</a><br />
        <a href="default.aspx">Send More BlueIkons</a><br />
        <a href="">Share BlueIkons with friends</a>


        </asp:Panel>
    </div>
    </center>
    </div>
    </form>
</body>
</html>
