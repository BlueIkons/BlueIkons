<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="Admin.aspx.cs" Inherits="BlueIkons.Admin" MasterPageFile="~/Site.Master"%>

<asp:Content ID=body ContentPlaceHolderID=Body runat=server>

<table style="text-align:center;">
    <tr>
        <td>
            <h4>BlueIkons Admin Pay it Forward Info</h4>
        </td>
    </tr>
    <tr>
        <td>
           Charity Name : <telerik:RadTextBox id=txtname runat=server EmptyMessage="Charity Name" Width=300 ></telerik:RadTextBox>
        </td>
    </tr>
    <tr valign=top>
        <td>
           Charity Description : <telerik:RadTextBox ID=txtdescription runat=server EmptyMessage="Charity Description" Rows=5 Width=300 Height="100"></telerik:RadTextBox>
        </td>
    </tr>
    <tr>
        <td>
            Charity Email :<telerik:RadTextBox ID=txtemail runat=server EmptyMessage="Charity Email" Width=300></telerik:RadTextBox>
        </td>
    </tr>
    <tr>
        <td>
            <asp:Button ID=btnsave runat=server Text="Save Changes" 
                onclick="btnsave_Click" />
            <br />
            <asp:Label ID=lblerror runat=server ForeColor=Red></asp:Label>
        </td>
    </tr>
</table>

</asp:Content>