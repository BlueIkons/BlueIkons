using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Configuration;
using System.Net;
using Newtonsoft.Json.Linq;
using System.Data;
using System.Collections;
using SubSonic;

namespace BlueIkons
{
    public partial class GiveGift : Telerik.Web.UI.RadAjaxPage
    {
        string thereturnpage = ConfigurationManager.AppSettings.Get("App_URL").ToString() + "GiveGift.aspx";
        fbuser fbuser = new fbuser();
        Site sitetemp = new Site();
        bool Live_Trial = Convert.ToBoolean(ConfigurationManager.AppSettings.Get("Live_Demo").ToString());

        protected void Page_Load(object sender, EventArgs e)
        {
            if (Request.Form["__EVENTTARGET"] == "btnpaypal")
            {
                btnpaypal();
            }
            if ((Request.QueryString["fbid"] != null) && (Request.QueryString["fbid"] != ""))
            {
                fbuser = sitetemp.Getfbuser(Convert.ToInt64(Request.QueryString["fbid"].ToString()));
            }
            else if (hdfbid.Value != "0")
            {
                fbuser = sitetemp.Getfbuser(Convert.ToInt64(hdfbid.Value));
            }
            else
            {
                fbuser = sitetemp.getfbuser();
            }            
            hdfbid.Value = fbuser.UID.ToString();
            if (!IsPostBack)
            {                
                //Page.ClientScript.RegisterClientScriptBlock(this.GetType(), "alert('standardone');", "");                
                //RadAjaxPanel1.ResponseScripts.Add("alert('hello');");
                updatefriendslist();
                int pendinggiftamount = sitetemp.checkifhavegiftamount(fbuser.UID);
                if (pendinggiftamount > 0)
                {
                    hyppendinggift.Text = "You have " + pendinggiftamount.ToString() + " pending gifts, click here to redeem";
                    hyppendinggift.NavigateUrl = "getgift.aspx?fbid=" + fbuser.UID.ToString();                        
                }
            }            
            /*            
                Setfbid();
                
            }
            else
            {
                Response.Redirect("http://www.facebook.com/dialog/oauth?client_id=" + ConfigurationSettings.AppSettings.Get("fbAppID").ToString() + "&redirect_uri=" + thereturnpage + "&scope=email,publish_stream");
            } */
        }

        protected void updatefriendslist()
        {
            DataTable dttemp = new DataTable();
            try
            {
                dttemp = sitetemp.getFriendslist(fbuser);
            }
            catch
            {
                lblerrorfriend.Visible = true;
            }
            
            ddlfbfriend.DataSource = dttemp;
            ddlfbfriend.DataTextField = "Name";
            ddlfbfriend.DataValueField = "fbid";
            ddlfbfriend.DataBind();

            Telerik.Web.UI.RadComboBoxItem comb = new Telerik.Web.UI.RadComboBoxItem();
            comb.Text="None";
            comb.Value = "0";
            ddlfbfriend.Items.Insert(0,comb);
        }

        protected void Setfbid()
        {
            string oauth = "";
            oauth = HttpContext.Current.Request.QueryString["code"].ToString();
            //oauth = oauth.Substring(0, oauth.IndexOf("|"));

            //oauth = oauth.Substring(0, oauth.IndexOf("|"));                

            WebClient wc = new WebClient();
            wc.Encoding = System.Text.Encoding.UTF8; //This is if you have non english characters
            //string result = wc.DownloadString("https://graph.facebook.com/oauth/access_token?response_type=token&client_secret=" + ConfigurationManager.AppSettings.Get("Secret").ToString() + "&client_id=" + ConfigurationManager.AppSettings.Get("fbAppID").ToString() + "&code=" + oauth);
            string strsend = "https://graph.facebook.com/oauth/access_token?client_id=" + ConfigurationManager.AppSettings.Get("fbAppID").ToString() + "&redirect_uri=" + thereturnpage + "&client_secret=" + ConfigurationManager.AppSettings.Get("Secret").ToString() + "&code=" + oauth;
            string result = wc.DownloadString(strsend);
            string accesstoken = result.Replace("access_token=", "");
            int endofaccesstoken = accesstoken.IndexOf("&expire");
            accesstoken = accesstoken.Substring(0, endofaccesstoken);

            //Get user id
             wc.Encoding = System.Text.Encoding.UTF8; //This is if you have non english characters
            string result2 = wc.DownloadString("https://graph.facebook.com/me?access_token=" + accesstoken);

            try
            {
                JObject o = JObject.Parse(result2);
                string fbid = (string)o["id"];                
                string email = "";
                string firstname = "";
                string lastname = "";
                if (o["email"] != null)
                {
                    email = (string)o["email"];
                }
                if (o["first_name"] != null)
                {
                    firstname = (string)o["first_name"];
                }
                if (o["last_name"] != null)
                {
                    lastname = (string)o["last_name"];
                }
                
                bool isnewuser = false;

                BlueIkons_DB.SPs.UpdateFBUser(Convert.ToInt64(fbid), firstname, lastname, email, accesstoken).Execute();
                fbuser.UID = Convert.ToInt64(fbid);
                fbuser.Email = email;
                fbuser.Firstname = firstname;
                fbuser.Lastname = lastname;
                fbuser.AccessToken = accesstoken;

                Site sitetemp = new Site();                
                
            }
            catch
            {
            }

        }

        protected void btnpaypal()
        {
            //Validation
            if (txtamount.Text == "")
            {
                txtamount.Text = "0";
            }
            if (decimal.Parse(txtamount.Text) <= 0)
            {
                lblerror.Text = "Enter amount greater than 0";
                txtamount.Text = "";
            }
            else if (txtemail.Text.Trim() == "")
            {
                lblerror.Text = "Enter receiver email";
            }
            else if (!chkterms.Checked)
            {
                lblerror.Text = "Agree to our Terms of Service (aka Legal Mumbo Jumbo)";
            }
            else
            {
                //validation ok
                if (txtwitty.Text == "")
                {
                    txtwitty.Text = "BlueIkons, faster than a Gift Card - and more fun!";
                }

                Int64 fbfriend = 0;
                if (!lblerrorfriend.Visible) //could choose from fbfriend list
                {
                    fbfriend = Convert.ToInt64(ddlfbfriend.SelectedValue);
                }

                StoredProcedure sp_UpdateGift = BlueIkons_DB.SPs.UpdateGift(0, fbuser.UID, fbfriend, txtemail.Text, txtwitty.Text, getselectedrdicon(), 0, chksharefb.Checked, ddlfbfriend.SelectedItem.Text);
                sp_UpdateGift.Execute();
                string tempGift_Key = sp_UpdateGift.Command.Parameters[6].ParameterValue.ToString();

                StoredProcedure sp_UpdateTransaction = BlueIkons_DB.SPs.UpdateTransaction(0, decimal.Parse(txtamount.Text), 0, Convert.ToInt32(tempGift_Key), 1, "", txtemail.Text);
                sp_UpdateTransaction.Execute();
                string tempTx_Key = sp_UpdateTransaction.Command.Parameters[2].ParameterValue.ToString();

                PayPal pp = new PayPal();
                string pakey = pp.SetPreapproval(fbuser, Live_Trial, Convert.ToInt32(tempTx_Key), decimal.Parse(txtamount.Text));
                //Page.ClientScript.RegisterClientScriptBlock(this.GetType(), "redirect", "");
                string ppredirect = "https://www.sandbox.paypal.com/webscr?cmd=_ap-preapproval&preapprovalkey=" + pakey;

                //lblscript.Text = "<script language=javascript>alert('here');</script>";
                //hdpaypal.Value = ppredirect;
                //Page.ClientScript.RegisterClientScriptBlock(this.GetType(), "alert('standardone');","");

                HttpContext.Current.Response.Redirect("ppredirect.aspx?pakey="+pakey);

                //RadAjaxManager1.ResponseScripts.Add("gotourl('" + ppredirect  + "');");
                //RadAjaxPanel1.ResponseScripts.Add("displaypaypal();");
                //Response.Redirect(ppredirect);
                //pnlgotopaypal.Visible = true;
            }
        }

        protected void btnPayPal_Click(object sender, EventArgs e)
        {
            btnpaypal();
        }

        public int getselectedrdicon()
        {
            int selectedicon = 1;

            if (rdicon1.Checked)
            {
                selectedicon = 1;
            }
            else if (rdicon2.Checked)
            {
                selectedicon = 2;
            }
            else if (rdicon3.Checked)
            {
                selectedicon = 3;
            }
            else if (rdicon4.Checked)
            {
                selectedicon = 4;
            }

            return selectedicon;
        }
    }
}