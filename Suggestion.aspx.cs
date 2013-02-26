using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace BlueIkons
{
    public partial class Suggestion : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {

        }

        protected void btnsubmit_Click(object sender, EventArgs e)
        {
            //send email
            string thebody = System.IO.File.ReadAllText(HttpContext.Current.Server.MapPath("/Emails/SuggestionEmail.txt"));

            thebody = thebody.Replace("THENAME",txtname.Text);
            thebody = thebody.Replace("THEEMAIL",txtemail.Text);
            thebody = thebody.Replace("THECOUNTRY",txtcountry.Text);
            thebody = thebody.Replace("THEMESSAGE",txtmessage.Text);

            SendEmail se = new SendEmail();
            se.Send_Email("suggestion@blueikons.com", "info@blueikons.com", "Suggestion Box Entry", thebody);
            Page.ClientScript.RegisterClientScriptBlock(this.GetType(), "redirect", "<script language=javascript>alert('Thank you for the suggestion.');location.href='default.aspx';</script>");
            //Response.Redirect("default.aspx");
        }
    }
}