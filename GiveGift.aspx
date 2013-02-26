<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="GiveGift.aspx.cs" Inherits="BlueIkons.GiveGift" MasterPageFile="~/Site.Master"%>
<%@ Register Assembly="Telerik.Web.UI" Namespace="Telerik.Web.UI" TagPrefix="telerik" %>

<asp:Content ID=body ContentPlaceHolderID=Body runat=server>
<asp:HiddenField ID=hdfbid runat=server Value="0" />

    <telerik:RadCodeBlock ID="RadCodeBlock1" runat="server">   
<script type="text/javascript">
    
    function OnClientValueChanged(sender, args) {
        var giftamount = args.get_newValue();
        if (giftamount < 5) {
            giftamount = 5;
        }
        else if (giftamount > 200) {
            giftamount = 200;
        }
        updateamounts(giftamount);

        //alert(sender.get_id() + ".OnClientValueChanged: Value is changed to " + args.get_newValue());
    }

    function updateamounts(giftamount) {
        var blueikonfee = giftamount / 10;        
        var paypalfee = (giftamount * .029) + (blueikonfee * .029) + 0.6;
        var totalamount = giftamount + paypalfee + blueikonfee;
        if (giftamount == 0) {
            paypalfee = 0;
            totalamount = 0;
        }
        document.getElementById('friendamount').innerHTML = giftamount.toFixed(2);
        document.getElementById('blueikonfee').innerHTML = blueikonfee.toFixed(2);
        document.getElementById('paypalfee').innerHTML = paypalfee.toFixed(2);
        document.getElementById('totalamount').innerHTML = totalamount.toFixed(2);
    }

    function updateamountsfrombutton() {
        var giftamount = parseFloat(document.getElementById('ctl00_Body_txtamount_text').value.replace('$', ''));                
        updateamounts(giftamount);
    }
</script>   
</telerik:RadCodeBlock>   
   
    <telerik:RadAjaxLoadingPanel ID="RadAjaxLoadingPanel1" runat="server">  
</telerik:RadAjaxLoadingPanel> 

<center>

<div id="ppnotification" class="transbox2" style="display:none;">
<h4>
Wait for it.
<br />
You are being Transfered to PayPal
</h4>
</div>
</center>
<script language=javascript>
    function displaypaypal() {
        document.getElementById('ppnotification').style.display = "block";
    }

    
    if (document.getElementById('ctl00_hdpaypal').value != '0') {
        alert('here');
        gotourl(document.getElementById('ctl00_hdpaypal').value);
        displaypaypal();
    }
</script>



    <telerik:RadAjaxPanel ID="RadAjaxPanel1" runat="server" LoadingPanelID="RadAjaxLoadingPanel1">
    
<asp:HiddenField runat=server Value="0" ID=hdpaypal />

    <table width=100%>
        <tr valign=top>
            <td>
                <center>

                <table cellpadding=5px>
                <tr>
                    <td style="text-align:center;">
                        <asp:HyperLink ID=hyppendinggift runat=server ForeColor=Red Font-Size=Larger Font-Underline=true></asp:HyperLink>
                    </td>
                </tr>
        <tr>
            <td>
                1. Give Cash with Snazzy BlueIkons
                <br />
                <table width=300 style="text-align:center;">
                    <tr>
                        <td>
                            <img src="/Images/1.png" width=50 />
                            <br />
                            Cool Cash             
                            <br />                                        
                            <asp:RadioButton ID=rdicon1 runat=server GroupName=Icons Checked=true />   
                        </td>
                        <td>
                            <img src="/Images/2.png" width=50 />
                            <br />
                            Cheers
                            <br />
                            <asp:RadioButton ID=rdicon2 runat=server GroupName=Icons />
                            
                        </td>
                        <td>
                            <img src="/Images/3.png" width=50 />
                            <br />
                            Surprise
                            <br />
                            <asp:RadioButton ID=rdicon3 runat=server GroupName=Icons />
                            
                        </td>
                        <td>
                            <img src="/Images/4.png" width=50 />
                            <br />
                            Good Day
                            <br />
                            <asp:RadioButton ID=rdicon4 runat=server GroupName=Icons />
                            
                        </td>
                    </tr>
                </table>
            </td>
        </tr>
        <tr>
            <td>
                <table>
                    <tr valign=top>
                        <td>
                            2. Gift Value ($5 - $200)
                            <br /> 
                            <telerik:RadNumericTextBox ID=txtamount runat=server type=Currency EmptyMessage="$0.00" Font-Size=Medium Width=300 MinValue=5 MaxValue=200>                                
                                <ClientEvents OnValueChanging=OnClientValueChanged />                                
                            </telerik:RadNumericTextBox>
                        </td>
                        <td  style="margin-left:20px; text-align:left; width:200px;">
                            <table>
                                <tr>
                                    <td colspan=2>
                                        <b>Gift amount Breakdown:</b>
                                    </td>
                                </tr>
                                <tr>
                                    <td>
                                        Friend receives: 
                                    </td>
                                    <td>$<span id=friendamount>0.00</span></td>
                                </tr>
                                <tr>
                                    <td>
                                        BlueIkons KTLO<br />(Keep the lights on) Fee: 
                                    </td>
                                    <td>
                                    $<span id=blueikonfee>0.00</span>
                                    </td>
                                </tr>
                                <tr>
                                    <td>
                                        PayPal Service Fee: 
                                    </td>
                                    <td>
                                    $<span id=paypalfee>0.00</span>
                                    </td>
                                </tr>
                                <tr>
                                    <td>
                                        Total Amount: 
                                    </td>
                                    <td>
                                        $<span id=totalamount>0.00</span>
                                    </td>
                                </tr>
                            </table>
                        </td>            
                    </tr>
                </table>                
            </td>
        </tr>
        <tr>
            <td>
                3.  Who’s the Lucky Facebook Friend? <span style="font-size:smaller; color:Blue;"> 
                <br />
                No Facebook Friends? No problem – just give us an email below.</span>
                <br />
                <telerik:RadComboBox ID=ddlfbfriend runat=server MarkFirstMatch=true Width=300></telerik:RadComboBox>                
                <asp:Label ID=lblerrorfriend runat=server Visible=false ForeColor=Red><br />Unfortunately we are unable to read your friends list</asp:Label>
            </td>
        </tr>
        <tr>
            <td>
                4.  Email Address for Gift Delivery, please?
                <br />
                <telerik:RadTextBox ID=txtemail runat=server Width=300/>
            </td>
        </tr>        
        <tr>
            <td>
                5.  Add Witty Message:
                <br />
                <telerik:RadTextBox ID=txtwitty runat=server Width=300 MaxLength=300 Rows=3 Height=80 TextMode=MultiLine EmptyMessage="BlueIkons, faster than a Gift Card - and more fun!"/>
            </td>
        </tr>                
        <tr>
            <td>
                <asp:CheckBox ID=chksharefb Checked=true runat=server />It’s a party!
                <br />(BlueIkons will broadcast with your friends on Facebook)
            </td>
        </tr>
        <tr>
            <td>
                <asp:CheckBox ID=chkterms Checked=false runat=server />Click to accept <a href="http://blueikons.com/web/facebook/terms/2" target=_blank>Legal Mumbo Jumbo</a>
            </td>
        </tr>
        <tr>
            <td>
                <telerik:RadButton ID=btnPayPal runat=server Text="Send Gift via PayPal" 
                    Width=300 BackColor=Yellow onclick="btnPayPal_Click" OnClientClicked="updateamountsfrombutton()">                    
                    </telerik:RadButton>
                <asp:Label ID=lblerror ForeColor=Red runat=server></asp:Label>
            </td>
        </tr>
        <tr>
            <td style="font-size:smaller;" >
                Fine Print:
     <br />Cash Value NOT displayed on Friend’s Wall, sssshhhhh!
     <br />Gift values between $5 - $200, boom-done.
     <br />No Take-backs; all gifts are final.
     <br />Gifts not accepted (awkward!), will automatically cancel in 30 days.
     <br />All transactions in USD
    <br />Suggestion Box: we’re new, and love good ideas,<a href="Suggestion.aspx">click here</a> to give us  your feedback

            </td>
        </tr>
    </table>
                
                </center>
            </td>
            
        </tr>
    </table>

    

    </telerik:RadAjaxPanel>

</asp:Content>