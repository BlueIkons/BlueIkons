<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="ppredirect.aspx.cs" Inherits="BlueIkons.ppredirect" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title></title>
</head>
<body>
    <form id="form1" runat="server">
    <div>
    <asp:HiddenField ID=hdpakey runat=server Value='0' />
    <script language=javascript>
    function getParameterByName(name)
    {
      name = name.replace(/[\[]/, "\\\[").replace(/[\]]/, "\\\]");
      var regexS = "[\\?&]" + name + "=([^&#]*)";
      var regex = new RegExp(regexS);
      var results = regex.exec(window.location.search);
      if(results == null)
        return "";
      else
        return decodeURIComponent(results[1].replace(/\+/g, " "));
    }
    //alert(document.getElementById('hdpakey').value);
    top.location.href = 'https://www.paypal.com/webscr?cmd=_ap-preapproval&preapprovalkey='+ getParameterByName("pakey");
    </script>
    
    </div>
    </form>
</body>
</html>
