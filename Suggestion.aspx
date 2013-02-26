<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="Suggestion.aspx.cs" Inherits="BlueIkons.Suggestion" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title></title>
</head>
<body>
    <form id="form1" runat="server">
    <div>

     <telerik:RadScriptManager ID="ScriptManager" runat="server" />                
    
        
    <center>
        <table>
            <tr>
                <td>
                   <h4>Suggestion Box</h4>
                </td>
            </tr>
            <tr>
                <td>
                    <telerik:RadTextBox ID=txtname runat=server Label="Name" Width=300></telerik:RadTextBox>
                </td>
            </tr>
            <tr>
                <td>
                    <telerik:RadTextBox ID=txtemail runat=server Label="Email" Width=300></telerik:RadTextBox>
                </td>
            </tr>
            <tr>
                <td>
                    <telerik:RadTextBox ID=txtcountry runat=server Label="Country" Width=300></telerik:RadTextBox>
                </td>
            </tr>
            <tr>
                <td>
                    <telerik:RadTextBox ID=txtmessage runat=server Label="Message" TextMode=MultiLine Height=100 Width=300></telerik:RadTextBox>
                </td>
            </tr>
            <tr>
                <td>
                    <asp:Button runat=server ID=btnsubmit Text="Submit" onclick="btnsubmit_Click" />
                </td>
            </tr>
        </table>
    </center>

    </div>
    </form>
</body>
</html>
