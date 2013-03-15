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
    public partial class Order_Confirmation : Telerik.Web.UI.RadAjaxPage
    {
        int Tx_Key = 0;
        protected void Page_Load(object sender, EventArgs e)
        {
            if ((Request.QueryString["Tx_Key"] != null) && (Request.QueryString["Tx_Key"] != ""))
            {
                Tx_Key = Convert.ToInt32(Request.QueryString["Tx_Key"].ToString());

                if (Tx_Key == 0)
                {
                    //they cancelled
                    pnlerror.Visible = true;
                    pnlworked.Visible = false;
                }
                else
                {
                    //it worked
                    lblid.Text = Tx_Key.ToString();
                    BlueIkons_DB.SPs.UpdateTransaction(Tx_Key, 0, 0, 0, 2, "", "").Execute();
                    Site sitetemp = new Site();

                    gift giftinfo = sitetemp.GetGiftInfo(Tx_Key);

                    string strapprurl = ConfigurationManager.AppSettings.Get("App_URL").ToString();
                    string strpicurl = ConfigurationManager.AppSettings.Get("BlueIkons_Pics").ToString() + giftinfo.blueikon.ToString() + ".png";
                    if (giftinfo.fbpost)
                    {
                        DataSet dstemp2 = BlueIkons_DB.SPs.ViewFBUser(giftinfo.sender_fbid).GetDataSet();
                        string accesstoken = dstemp2.Tables[0].Rows[0]["Access_Token"].ToString();
                        string receivername = giftinfo.receiver_name;
                        if (receivername.ToLower() == "none")
                        {
                            receivername = "";
                        }
                        string strmessage = "Just sent " + receivername +" a gift using BlueIkons";                                                 
                        string strdescription = receivername + ", you can claim your gift by clicking on the BlueIkons image";
                        sitetemp.Facebook_PostLink_OnWall(giftinfo.sender_fbid.ToString(), strapprurl, strmessage, strpicurl, "BlueIkons, Faster than a Gift Card and more Fun!", accesstoken, "", strdescription);
                    }

                    SendEmail se = new SendEmail();
                    se.Send_GiftEmail(Tx_Key, 0);
                    se.Send_GiftEmail(Tx_Key, 1);
                }                
            }

        }
    }
}