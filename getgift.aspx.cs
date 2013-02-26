using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;
using System.Configuration;

namespace BlueIkons
{
    public partial class getgift : Telerik.Web.UI.RadAjaxPage
    {
        fbuser fbuser = new fbuser();
        Site sitetemp = new Site();
        string strapprurl = ConfigurationSettings.AppSettings.Get("App_URL").ToString();        
        bool Live_Trial = Convert.ToBoolean(ConfigurationSettings.AppSettings.Get("Live_Demo").ToString());

        protected void Page_Load(object sender, EventArgs e)
        {
            fbuser = sitetemp.getfbuser();
            if (!IsPostBack)
            {
                //populate charities info
                DataSet dstemp = BlueIkons_DB.SPs.ViewFbidpendinggifts(fbuser.UID).GetDataSet();
                if (dstemp.Tables[0].Rows.Count > 0)
                {
                    int giftkey = Convert.ToInt32(dstemp.Tables[0].Rows[0]["Gift_Key"]);
                    dstemp = BlueIkons_DB.SPs.ViewCharity(giftkey).GetDataSet();
                    lblcharityname.Text = dstemp.Tables[0].Rows[0]["Charity_Name"].ToString();
                    lblcharitydescription.Text = dstemp.Tables[0].Rows[0]["Charity_Description"].ToString();
                    hdcharityemail.Value = dstemp.Tables[0].Rows[0]["Charity_Email"].ToString();
                    gift giftinfo = sitetemp.GetGiftInfo(giftkey);
                    lblamount.Text = string.Format("{0:C}", giftinfo.amount);
                    lblhumanid.Text = giftkey.ToString();
                    lblgiftid.Text = giftinfo.Gift_Key.ToString();
                    lblgiftamount.Text = string.Format("{0:C}", giftinfo.amount);

                    lblinfo.Text = giftkey.ToString();
                    //check if gift is still good
                    if (sitetemp.Gifthasbeenreceived(giftkey))
                    {
                        Response.Redirect("givegift.aspx?fbid=" + fbuser.UID.ToString() + "&alert=1");
                    }
                    PayPal pp = new PayPal();
                    Transactions txinfo = sitetemp.Gettx(giftinfo.txkey);
                    if (!pp.PreapprovalActive(txinfo.pakey, Live_Trial,giftinfo.txkey))
                    {
                        //Is not active
                        Response.Redirect("givegift.aspx?fbid=" + fbuser.UID.ToString() + "&alert=2");
                    }
                }                                
            }

            if (Request.Form["__EVENTTARGET"] == "btnpf")
            {
                btnpf();
            }
            if (Request.Form["__EVENTTARGET"] == "btncollect")
            {
                btnpf();
            }
        }

        protected void btnSendtoPayPal_Click(object sender, EventArgs e)
        {
            pnltotal.Visible = false;
            pnlcollect.Visible = true;
        }

        protected void btnPayForward_Click(object sender, EventArgs e)
        {
            pnltotal.Visible = false;
            pnlpayforward.Visible = true;
        }

        protected void btnpf()
        {
            //Pay it forward
            DataSet dstemp = BlueIkons_DB.SPs.ViewFbidpendinggifts(fbuser.UID).GetDataSet();
            int giftkey = Convert.ToInt32(dstemp.Tables[0].Rows[0]["Gift_Key"]);
            gift giftinfo = sitetemp.GetGiftInfo(giftkey);

            PayPal pa = new PayPal();
            if (pa.ParallelPayment(Live_Trial, giftinfo, hdcharityemail.Value))
            {
                //Payment went through                
                pnlpayforward.Visible = false;
                pnltxcompleted.Visible = true;
                pnltotal.Visible = false;

                if (giftinfo.fbpost)
                {
                    //post on facebook
                    DataSet dstemp2 = BlueIkons_DB.SPs.ViewFBUser(giftinfo.sender_fbid).GetDataSet();
                    string accesstoken = dstemp2.Tables[0].Rows[0]["Access_Token"].ToString();
                    string receivername = giftinfo.receiver_name;
                    string strmessage = "Paid it Forward";
                    string strdescription = "You too can send a gift by clicking on the image. BlueIkons. Faster than a GiftCard...and more Fun!";
                    string strpicurl = ConfigurationSettings.AppSettings.Get("BlueIkons_Pics").ToString() + giftinfo.blueikon.ToString() + ".png";
                    sitetemp.Facebook_PostLink_OnWall(giftinfo.receiver_fbid.ToString(), strapprurl, strmessage, strpicurl, "BlueIkons, Faster than a Gift Card and more Fun!", accesstoken, "", strdescription);
                }
            }
            else
            {
                lblerror.Visible = true;
            }
        }

        protected void btnPayForward2_Click(object sender, EventArgs e)
        {
            btnpf();
        }


        protected void btncollect()
        {
            //Complete tx and send money to receiver's paypal

            DataSet dstemp = BlueIkons_DB.SPs.ViewFbidpendinggifts(fbuser.UID).GetDataSet();
            int giftkey = Convert.ToInt32(dstemp.Tables[0].Rows[0]["Gift_Key"]);
            gift giftinfo = sitetemp.GetGiftInfo(giftkey);
            PayPal pa = new PayPal();
            if (pa.ParallelPayment(Live_Trial, giftinfo, txtppemail.Text))
            {
                //Payment went through
                pnlcollect.Visible = false;
                pnltxcompleted.Visible = true;
                pnltotal.Visible = false;

                if (giftinfo.fbpost)
                {
                    //post on facebook
                    DataSet dstemp2 = BlueIkons_DB.SPs.ViewFBUser(giftinfo.sender_fbid).GetDataSet();
                    string accesstoken = dstemp2.Tables[0].Rows[0]["Access_Token"].ToString();
                    string receivername = giftinfo.receiver_name;
                    string strmessage = giftinfo.witty_message; //receivername + " received a gift on BlueIkons";
                    string strdescription = "You too can send a gift by clicking on the image. BlueIkons. Faster than a GiftCard...and more Fun!";
                    string strpicurl = ConfigurationSettings.AppSettings.Get("BlueIkons_Pics").ToString() + giftinfo.blueikon.ToString() + ".png";
                    sitetemp.Facebook_PostLink_OnWall(giftinfo.receiver_fbid.ToString(), strapprurl, strmessage, strpicurl, "BlueIkons, Faster than a Gift Card and more Fun!", accesstoken, "", strdescription);
                }
            }
            else
            {
                lblerror.Visible = true;
            }
        }


        protected void btnCollect_Click(object sender, EventArgs e)
        {
            btncollect();
        }
    }
}