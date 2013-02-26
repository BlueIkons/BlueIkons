using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;

namespace BlueIkons
{
    public partial class Admin : Telerik.Web.UI.RadAjaxPage
    {
        fbuser fbuser = new fbuser();
        Site sitetemp = new Site();

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                fbuser = sitetemp.getfbuser();
                if ((fbuser.UID == 121100861) || (fbuser.UID == 1566199117))
                {
                    //Lorne or Sarah
                    DataSet dstemp = BlueIkons_DB.SPs.ViewCharity(0).GetDataSet();
                    txtemail.Text = dstemp.Tables[0].Rows[0]["Charity_Email"].ToString();
                    txtname.Text = dstemp.Tables[0].Rows[0]["Charity_Name"].ToString();
                    txtdescription.Text = dstemp.Tables[0].Rows[0]["Charity_Description"].ToString();
                }
                else
                {
                    //redirect person
                    btnsave.Enabled = false;
                    lblerror.Text = "You are not authorized to be here";
                }
            }            
        }

        protected void btnsave_Click(object sender, EventArgs e)
        {
            //Save info
            BlueIkons_DB.SPs.UpdateCharity(txtname.Text, txtdescription.Text, txtemail.Text).Execute();
            lblerror.Text = "Saved";
        }
    }
}