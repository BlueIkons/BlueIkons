<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="getgift.aspx.cs" Inherits="BlueIkons.getgift" MasterPageFile="~/Site.Master"%>
<%@ Register Assembly="Telerik.Web.UI" Namespace="Telerik.Web.UI" TagPrefix="telerik" %>

<asp:Content ID=body ContentPlaceHolderID=Body runat=server>
    
<div id="div1">
</div>

<center>
<div class="transbox">
<table>
    <tr>
        <td>
            <table width=100%>
                <tr>
                    <td>
                        BlueIkons GiftID : <asp:Label ID=lblgiftid runat=server/> 
                    </td>
                    <td style="text-align:right;">
                        Amount : <asp:Label ID=lblgiftamount runat=server></asp:Label>
                    </td>
                </tr>
            </table>
            
        </td>
    </tr>
    <tr>
        <td>
            <asp:Panel ID=pnltotal runat=server>
    <div id=divtotal>
<table>
    <tr>
        <td colspan=2 style="text-align:center;">
        <h4>You lucky duck!<br /> How would you like to collect your BlueIkons Cash Gift?</h4>
        </td>     
    </tr>
    <tr>
        <td style="text-align:center;">
            <input type=button value="Send to PayPal" onclick="gocollect()" />
            
            <asp:Button ID=btnSendtoPayPal runat=server Text="Send to PayPal" 
                OnClientClick="gocollect()" Visible=false />
            <br />
            Collect your gift into your PayPal Account
        </td>
        <td style="text-align:center;">
            <input type=button value="Pay it Forward" onclick="gopayforward()" />

            <asp:Button runat=server ID=btnPayForward Text="Pay it Forward" 
                 OnClientClick="gopayforward()"  Visible=false/>
            <br />
            Donate your Gift to Charity
        </td>
    </tr>
</table>    
</div>
</asp:Panel>
</td>
</tr>
<tr>
<td>

<asp:Panel ID=pnlcollect runat=server >
<div id="divcollect" style="display:none;">
<table style="text-align:center;">
    <tr>
        <td>
            <h4>Please enter your PayPal Email Address</h4>
        </td>
    </tr>    
    <tr>
        <td>
            <telerik:RadTextBox ID=txtppemail runat=server Width=300px EmptyMessage="PayPal email address"></telerik:RadTextBox>
        </td>
    </tr>
    <tr>
        <td>
            <telerik:RadTextBox ID=txtppemail2 runat=server Width=300px EmptyMessage="Confirm PayPal email address"></telerik:RadTextBox>
        </td>
    </tr>
    <tr>
        <td>
            <asp:Button runat=server text="Continue" id="btnCollect" OnClick="btnCollect_Click" />
        </td>
    </tr>
    <tr>
        <td>
            Having Trouble?<br />
            1: Ummmm, I forgot my PayPal email address?<br />
            2: Huh? What? I don't have a PayPal email address?<br />
            3: What day is it anyway?
            <br />
            No worries - you can still collect your Cash Gift,
            <br />just visit <a href="http://www.PayPal.com" target=_blank>PayPal.com</a> 
        </td>
    </tr>
</table>

</div>
</asp:Panel>
</td>
</tr>
<tr>
<td>

<asp:Panel ID=pnlpayforward runat=server >
<div id=divpayforward style="display:none;">

<table style="text-align:center;">
    <tr>
        <td>
            <h4>Pay it Forward to:            <br />
            <asp:Label ID=lblcharityname runat=server></asp:Label>
            </h4>            
            <span style=" font-size:small;"><asp:Label ID=lblcharitydescription runat=server></asp:Label>
            </span>
        </td>
    </tr>        
    <tr>
        <td>
            <b>Your gift of <asp:Label ID=lblamount runat=server></asp:Label> is being forwarded. </b>
            <br />
         <span style="font-size:small;">(sorry no receipts for your taxes, just know you’re doing the right thing)</span>
        </td>
    </tr>
    <tr>
        <td>
            <asp:Button runat=server text="Pay it Forward" id="btnPayForward2" OnClick="btnPayForward2_Click"/>
            <asp:HiddenField ID=hdcharityemail runat=server value=""/>
        </td>
    </tr>    
</table>
</div>
</asp:Panel>
</td>
</tr>
<tr>
<td>

<asp:Panel ID=pnltxcompleted runat=server Visible=false>
<div id=divtxcompleted >

<table>
    <tr>
        <td>Congratulations, you are loved and adored!  
        <br />
        Generous Human ID <asp:Label ID=lblhumanid runat=server></asp:Label>
        </td>
    </tr>
    <tr>
        <td>
            That was fun! Now what?
        <br />
        <a href="default.aspx">Send More BlueIkons</a>
        <br />
        <a href="http://www.facebook.com/sharer.php
?u=http%3A%2F%2Fapps.facebook.com%2Fblueikons%2Fdefault.aspx
&t=BlueIkons" target=_blank>Share BlueIkons with friends</a>
        </td>
    </tr>
</table>
</div>
</asp:Panel>
<asp:Label runat=server ForeColor=Red ID=lblerror Visible=false>An error occured and your Transaction did not complete</asp:Label>

</td>
    </tr>
</table>
</div>
</center>

<asp:Label ID=lblinfo runat=server Visible=false></asp:Label>

<script language=javascript>

    
        var body = document.getElementsByTagName('body')[0];
        body.style.backgroundImage = "url('images/BlueIkonsbackground.jpg')";
    
    
    /*
    divtotal
    divcollect
    divpayforward
    divtxcompleted
    */



function gocollect() {
    document.getElementById("divtotal").style.display = "none";
    document.getElementById("divcollect").style.display = "block";
}

function gopayforward() {
    document.getElementById("divtotal").style.display = "none";
    document.getElementById("divpayforward").style.display = "block";
}

function btnpf() {
    __doPostBack('btnpf', '');
}

function btncollect() {
    __doPostBack('btncollect', '');
}
</script>

</asp:Content>