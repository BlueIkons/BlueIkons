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
using System.Text;
using System.IO;
using Facebook;
using Facebook.Web;


namespace BlueIkons
{
    public partial class Site : System.Web.UI.MasterPage
    {
        string thereturnpage = ConfigurationManager.AppSettings.Get("App_URL").ToString() + "Default.aspx";
        string[] requiredAppPermissions = { "email", "publish_stream", "offline_access" };
        string strrequiredAppPermissions = "email,publish_stream,offline_access";
        bool Live_Trial = Convert.ToBoolean(ConfigurationSettings.AppSettings.Get("Live_Demo").ToString());

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {                
                fbuser fbuser = new fbuser();
                if ((HttpContext.Current.Request.QueryString["fbid"] != null) && (HttpContext.Current.Request.QueryString["fbid"] != ""))
                {
                    fbuser = Getfbuser(Convert.ToInt64(HttpContext.Current.Request.QueryString["fbid"].ToString()));
                }
                else
                {
                    fbuser = getfbuser();
                }                
                if (fbuser != null)
                {
                    string strcurrentpage = Body.Page.GetType().FullName;                    
                    /*
                    if (((!strcurrentpage.ToLower().Contains("getgift")) && (checkifhavegift(fbuser.UID))))
                    {
                        Response.Redirect("getgift.aspx?fbid="+fbuser.UID.ToString());                        
                        //hdredirect.Value = "getgift.aspx";
                    }
                    else if (strcurrentpage.ToLower().Contains("default"))
                    {
                        Response.Redirect("givegift.aspx?fbid="+fbuser.UID.ToString());                        
                        //hdredirect.Value = "givegift.aspx";
                    }
                     */
                    if (strcurrentpage.ToLower().Contains("default"))
                    {
                        Response.Redirect("givegift.aspx?fbid=" + fbuser.UID.ToString());                        
                    }

                    if (HttpContext.Current.Request.QueryString["alert"] != null){
                        string thealert = HttpContext.Current.Request.QueryString["alert"].ToString();
                        if (hdpopup != null){
                            switch (thealert)
                            {
                                case "1": hdpopup.Value = "You already accepted this gift.";
                                    break;
                                case "2": hdpopup.Value = "Your gift has already expired or cancelled.";
                                    break;
                            }
                            
                        }
                        
                    }
                        
                }
                lbluser.Text = fbuser.Firstname + " " + fbuser.Lastname;
            }            
        }

        public Boolean checkifhavegift(Int64 fbid){
            Boolean havegift = false;
            
            DataSet dstemp = BlueIkons_DB.SPs.ViewFbidpendinggifts(fbid).GetDataSet();
            if (dstemp.Tables[0].Rows.Count > 0)
            {
                havegift = true;
            }

            return havegift;
        }

        public int checkifhavegiftamount(Int64 fbid)
        {
            int havegiftamount = 0;

            DataSet dstemp = BlueIkons_DB.SPs.ViewFbidpendinggifts(fbid).GetDataSet();
            if (dstemp.Tables[0].Rows.Count > 0)
            {
                havegiftamount = dstemp.Tables[0].Rows.Count;
            }

            return havegiftamount;
        }

        

        public Boolean Gifthasreceiver(int giftkey)
        {
            Boolean hasreceiver = false;

            DataSet dstemp = BlueIkons_DB.SPs.ViewGift(giftkey).GetDataSet();
            if (dstemp.Tables[0].Rows.Count > 0)
            {
                if (dstemp.Tables[0].Rows[0]["receiver_fbid"] != DBNull.Value)
                {
                    if (dstemp.Tables[0].Rows[0]["receiver_fbid"].ToString() != "0")
                    {
                        hasreceiver = true;
                    }
                }
            }
            

            return hasreceiver;
        }

        public Boolean Gifthasbeenreceived(int giftkey)
        {
            Boolean hasreceiver = false;

            DataSet dstemp = BlueIkons_DB.SPs.ViewTransactionsGiftKey(giftkey).GetDataSet();
            if (dstemp.Tables[0].Rows.Count > 0)
            {
                if (dstemp.Tables[0].Rows[0]["Tx_Status"] != DBNull.Value)
                {
                    if (dstemp.Tables[0].Rows[0]["Tx_Status"].ToString() == "3")
                    {
                        hasreceiver = true;
                    }
                }
            }


            return hasreceiver;
        }

        /*
        private fbuser Setfbid()
        {
            string oauth = "";
            oauth = HttpContext.Current.Request.QueryString["code"].ToString();
            //oauth = oauth.Substring(0, oauth.IndexOf("|"));
            fbuser fbuser = new fbuser();
            //oauth = oauth.Substring(0, oauth.IndexOf("|"));                

            WebClient wc = new WebClient();
            wc.Encoding = System.Text.Encoding.UTF8; //This is if you have non english characters
            //string result = wc.DownloadString("https://graph.facebook.com/oauth/access_token?response_type=token&client_secret=" + ConfigurationManager.AppSettings.Get("Secret").ToString() + "&client_id=" + ConfigurationManager.AppSettings.Get("fbAppID").ToString() + "&code=" + oauth);
            string strsend = "https://graph.facebook.com/oauth/access_token?client_id=" + ConfigurationManager.AppSettings.Get("fbAppID").ToString() + "&redirect_uri=" + thereturnpage + "&client_secret=" + ConfigurationManager.AppSettings.Get("Secret").ToString() + "&code=" + oauth;
            var url = "https://graph.facebook.com/oauth/authorize?client_id=" + ConfigurationSettings.AppSettings.Get("fbAppID").ToString() + "&redirect_uri=" + ConfigurationSettings.AppSettings.Get("App_URL").ToString() + "default.aspx&scope=" + strrequiredAppPermissions;
            string result = wc.DownloadString(url);
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
                

            }
            catch
            {
            }

            return fbuser;
        }*/

        protected fbuser PopulatefbuserGraph()
        {
            CanvasAuthorizer authorizer;
            fbuser localfbuser = new fbuser();
            FacebookApp fbApp = new FacebookApp();

            
            authorizer = new CanvasAuthorizer();
            authorizer.Permissions = requiredAppPermissions;
            //if ((authorizer.Session != null) || ((HttpContext.Current.Request.QueryString["code"] != null) && (HttpContext.Current.Request.QueryString["code"] != "")))
            if (authorizer.Session != null)
            {
                //ShowFacebookContent();                        
                JsonObject myInfo = (JsonObject)fbApp.Get("me");

                localfbuser.UID = Convert.ToInt64(myInfo["id"].ToString());
                localfbuser.AccessToken = fbApp.AccessToken;
                localfbuser.SessionKey = fbApp.Session.Signature;
                localfbuser.Firstname = myInfo["first_name"].ToString();
                localfbuser.Lastname = myInfo["last_name"].ToString();
                localfbuser.Fullname = localfbuser.Firstname + " " + localfbuser.Lastname;
                localfbuser.Email = getfbappemail(myInfo);

                //HttpContext.Current.Session["fbuser"] = fbuser;
                BlueIkons_DB.SPs.UpdateFBUser(localfbuser.UID, localfbuser.Firstname, localfbuser.Lastname, localfbuser.Email, localfbuser.AccessToken).Execute();

                if ((HttpContext.Current.Session["invite"] != null) || (HttpContext.Current.Request.QueryString["invite"] != null))
                {
                    updateinvite(localfbuser);
                }
                //Eventomatic_DB.SPs.UpdateResource(fbuser.UID, fbuser.Firstname, fbuser.Lastname, "", HttpContext.Current.Request.UserHostAddress, GetCurrentPageName(), 0, 0, fbuser.SessionKey, fbuser.AccessToken, 0).Execute();                
            }
            else if ((HttpContext.Current.Request.QueryString["fbid"] != null) && (HttpContext.Current.Request.QueryString["fbid"] != ""))
            {
                localfbuser = Getfbuser(Convert.ToInt64(HttpContext.Current.Request.QueryString["fbid"].ToString()));
                if (HttpContext.Current.Request.QueryString["invite"] != null)
                {
                    updateinvite(localfbuser);
                }
                //HttpContext.Current.Session["fbuser"] = fbuser;
            }
            else
            {
                if (HttpContext.Current.Request.QueryString["invite"] != null)
                {
                    //remember invitekey
                    HttpContext.Current.Session["invite"] = HttpContext.Current.Request.QueryString["invite"].ToString();
                }
                var pageName = Path.GetFileName(HttpContext.Current.Request.PhysicalPath);
                var urlSB = new StringBuilder();
                urlSB.Append("https://graph.facebook.com/oauth/authorize?client_id=");
                urlSB.Append(ConfigurationManager.AppSettings["fbAppID"]);
                urlSB.Append("&redirect_uri=");
                urlSB.Append(ConfigurationManager.AppSettings["App_URL"]);
                urlSB.Append(pageName);
                urlSB.Append("&scope=");
                urlSB.Append(strrequiredAppPermissions);
                //var url = authorizer.ge auth.GetLoginUrl(new HttpRequestWrapper(Request));
                Uri newuri = new Uri(urlSB.ToString());
                var content = CanvasUrlBuilder.GetCanvasRedirectHtml(newuri);
                HttpContext.Current.Response.ContentType = "text/html";
                HttpContext.Current.Response.Write(content);
                HttpContext.Current.Response.End();                
            }
            return localfbuser;
        }


        private void updateinvite(fbuser localfbuser)
        {
            //set fbid as receiver of gift
            int giftkey = 0;
            if (HttpContext.Current.Request.QueryString["invite"] != null)
            {
                giftkey = Convert.ToInt32(HttpContext.Current.Request.QueryString["invite"]);
            }
            else if (HttpContext.Current.Request.QueryString["invite"] != null)
            {
                giftkey = Convert.ToInt32(HttpContext.Current.Request.QueryString["invite"]);
            }

            //check if gift is still good
            if (Gifthasbeenreceived(giftkey))
            {
                HttpContext.Current.Response.Redirect("givegift.aspx?fbid=" + localfbuser.UID.ToString() + "&alert=1");
            }
            PayPal pp = new PayPal();
            try
            {
                gift giftinfo = GetGiftInfo(giftkey);
                Transactions txinfo = Gettx(giftinfo.txkey);
                if (!pp.PreapprovalActive(txinfo.pakey, Live_Trial, giftinfo.txkey))
                {
                    //Is not active
                    HttpContext.Current.Response.Redirect("givegift.aspx?fbid=" + localfbuser.UID.ToString() + "&alert=2");
                }

                if (!Gifthasreceiver(giftkey))
                {
                    BlueIkons_DB.SPs.UpdateGiftReceiver(giftkey, localfbuser.UID).Execute();
                }

                HttpContext.Current.Session["invite"] = null;
            }
            catch
            {
            }            
        }

        public string getfbappemail(JsonObject myInfo)
        {
            string stremail = "";
            if (myInfo.ContainsKey("email"))
            {
                stremail = myInfo["email"].ToString();
            }
            return stremail;
        }

        public fbuser getfbuser(){
            fbuser fbuser = new fbuser();
            if (HttpContext.Current.Session["fbuser"] == null) //No authorization or fbuid
            {
                fbuser = PopulatefbuserGraph();
                /*
                if ((HttpContext.Current.Request.QueryString["code"] != null) && (HttpContext.Current.Request.QueryString["code"] != ""))
                {
                    fbuser = Setfbid();
                    HttpContext.Current.Session["fbuser"] = fbuser;
                }
                else
                {
                    PopulatefbuserGraph();
                    //string strredirecturl = "http://www.facebook.com/dialog/oauth?client_id=" + ConfigurationManager.AppSettings.Get("fbAppID").ToString() + "&redirect_uri=" + thereturnpage + "&scope=email,publish_stream";
                    //hdredirect.Value = strredirecturl;
                    //Response.Redirect("http://www.facebook.com/dialog/oauth?client_id=" + ConfigurationManager.AppSettings.Get("fbAppID").ToString() + "&redirect_uri=" + thereturnpage + "&scope=email,publish_stream");
                     
                } */

            }            
            //fbuser = (fbuser)HttpContext.Current.Session["fbuser"];            
            return fbuser;
        }

        public DataTable getFriendslist(fbuser fbuser)
        {
            DataTable dtFriendsList = new DataTable("FBFriends");
            DataColumn cltemp;
            DataColumn cltemp2;
            DataRow rtemp;

            cltemp = new DataColumn();
            cltemp.DataType = System.Type.GetType("System.String");
            cltemp.ColumnName = "fbid";
            cltemp.ReadOnly = false;
            cltemp.Unique = true;
            dtFriendsList.Columns.Add(cltemp);
            cltemp2 = new DataColumn();
            cltemp2.DataType = System.Type.GetType("System.String");
            cltemp2.ColumnName = "Name";
            cltemp2.ReadOnly = false;
            cltemp2.Unique = true;
            dtFriendsList.Columns.Add(cltemp2);
            Hashtable fbht = new Hashtable();

            ListItem litemp = new ListItem();

            //System.Collections.Generic.IList<facebook.Schema.user> friends = Master.API.friends.getUserObjects();

            string fbid = fbuser.UID.ToString();
            DataSet dstemp = ExecuteQuery("SELECT uid, name FROM user WHERE  uid IN (SELECT uid2 FROM friend WHERE uid1 ='" + fbid + "')", fbuser);
            //foreach (facebook.Schema.user friend in friends)
            foreach (DataRow r in dstemp.Tables[1].Rows)
            {
                //ListItem li = new ListItem(friend.name, friend.uid.ToString());
                //lstFacebookFriends.Items.Add(li);
                litemp.Value = r["uid"].ToString(); //friend.uid.ToString();
                litemp.Text = r["name"].ToString();//friend.name.ToString();
                if ((!fbht.ContainsValue(r["name"].ToString().ToUpper())))
                {
                    rtemp = dtFriendsList.NewRow();
                    rtemp[0] = r["uid"].ToString();
                    rtemp[1] = r["name"].ToString();
                    dtFriendsList.Rows.Add(rtemp);
                    fbht.Add(r["uid"].ToString(), r["name"].ToString().ToUpper());
                }

            }
            dtFriendsList.DefaultView.Sort = "Name asc";
            return dtFriendsList;
        }

        public DataSet ExecuteQuery(string queryCommand, fbuser fbuser)
        {
            string errorInfo = String.Empty;
            DataSet queryResults = null;

            try
            {                

                WebClient wc = new WebClient();
                wc.Encoding = System.Text.Encoding.UTF8; //This is if you have non english characters
                //string result = wc.DownloadString("http://api.facebook.com/restserver.php?method=facebook.fql.query&query=SELECT%20fan_count%20FROM%20page%20WHERE%20page_id=36922302396");
                string result = wc.DownloadString("https://api.facebook.com/method/fql.query?query=" + queryCommand + "&access_token=" + fbuser.AccessToken);

                /*string xmlDataReturned = API.fql.query(queryCommand);*/

                queryResults = new DataSet();
                System.IO.StringReader xmlReader = new System.IO.StringReader(result);
                queryResults.ReadXml(xmlReader);

                return queryResults;
            }

            catch (Exception ex_exec_query)
            {
                errorInfo = "Failed to exec [" + queryCommand + "]:" + ex_exec_query.Message;
                return queryResults;
            }
        }

        public void Facebook_PostLink_OnWall(string fbid, string linkurl, string message, string picurl, string name, string Access_Token, string caption, string description)
        {

            try
            {
                //doGraphcall(thecall);
                StringBuilder requestString = new StringBuilder();
                requestString.Append("access_token=" + Access_Token + "&message=" + message + "&link=" + linkurl + "&picture=" + picurl + "&name=" + name + "&description=" + description);
                HttpWebResponse webResponse;
                HttpWebRequest webRequest = WebRequest.Create("https://graph.facebook.com/" + fbid + "/feed") as HttpWebRequest;
                webRequest.Method = "POST";
                webRequest.ContentType = "application/x-www-form-urlencoded";
                string request = requestString.ToString();
                webRequest.ContentLength = request.Length;

                StreamWriter writer = new StreamWriter(webRequest.GetRequestStream());
                writer.Write(request);
                writer.Close();

                webResponse = webRequest.GetResponse() as HttpWebResponse;
            }
            catch
            {
            }

        }

        public Transactions Gettx(int txkey)
        {
            Transactions txinfo = new Transactions();

            txinfo.Tx_Key = txkey;
            DataSet dstemp = BlueIkons_DB.SPs.ViewTransactions(txkey).GetDataSet();

            if (dstemp.Tables[0].Rows[0]["Amount"] != DBNull.Value)
            {
                txinfo.Amount = Convert.ToDecimal(dstemp.Tables[0].Rows[0]["Amount"]);
            }

            if (dstemp.Tables[0].Rows[0]["Gift_Key"] != DBNull.Value)
            {
                txinfo.Gift_Key = Convert.ToInt32(dstemp.Tables[0].Rows[0]["Gift_Key"]);
            }

            if (dstemp.Tables[0].Rows[0]["Init_date"] != DBNull.Value)
            {
                txinfo.Init_date = Convert.ToDateTime(dstemp.Tables[0].Rows[0]["Init_date"]);
            }

            if (dstemp.Tables[0].Rows[0]["Collected_date"] != DBNull.Value)
            {
                txinfo.Collected_date = Convert.ToDateTime(dstemp.Tables[0].Rows[0]["Collected_date"]);
            }

            if (dstemp.Tables[0].Rows[0]["txn_id"] != DBNull.Value)
            {
                txinfo.txn_id = Convert.ToString(dstemp.Tables[0].Rows[0]["txn_id"]);
            }

            if (dstemp.Tables[0].Rows[0]["Tx_Status"] != DBNull.Value)
            {
                txinfo.Tx_Status = Convert.ToInt32(dstemp.Tables[0].Rows[0]["Tx_Status"]);
            }

            if (dstemp.Tables[0].Rows[0]["pakey"] != DBNull.Value)
            {
                txinfo.pakey = Convert.ToString(dstemp.Tables[0].Rows[0]["pakey"]);
            }

            if (dstemp.Tables[0].Rows[0]["receiver_email"] != DBNull.Value)
            {
                txinfo.receiver_email = Convert.ToString(dstemp.Tables[0].Rows[0]["receiver_email"]);
            }

            return txinfo;
        }

        public fbuser Getfbuser(Int64 intfbuser)
        {
            fbuser fbuser = new fbuser();
            fbuser.UID = intfbuser;
            DataSet dstemp = BlueIkons_DB.SPs.ViewFBUser(intfbuser).GetDataSet();
            if (dstemp.Tables[0].Rows[0]["first_name"] != DBNull.Value)
            {
                fbuser.Firstname = dstemp.Tables[0].Rows[0]["first_name"].ToString();
            }

            if (dstemp.Tables[0].Rows[0]["last_name"] != DBNull.Value)
            {
                fbuser.Lastname = dstemp.Tables[0].Rows[0]["last_name"].ToString();
            }

            if (dstemp.Tables[0].Rows[0]["fbemail"] != DBNull.Value)
            {
                fbuser.Email = dstemp.Tables[0].Rows[0]["fbemail"].ToString();
            }

            if (dstemp.Tables[0].Rows[0]["paypalemail"] != DBNull.Value)
            {
                fbuser.paypalemail = dstemp.Tables[0].Rows[0]["paypalemail"].ToString();
            }

            if (dstemp.Tables[0].Rows[0]["Access_Token"] != DBNull.Value)
            {
                fbuser.AccessToken = dstemp.Tables[0].Rows[0]["Access_Token"].ToString();
            }

            return fbuser;
        }

        public gift GetGiftInfo(int Gift_Key)
        {
            gift tempgift = new gift();

            DataSet dstemp = BlueIkons_DB.SPs.ViewGift(Gift_Key).GetDataSet();
            tempgift.Gift_Key = Gift_Key;
            if (dstemp.Tables[0].Rows[0]["sender_fbid"] != DBNull.Value)
            {
                tempgift.sender_fbid = Convert.ToInt64(dstemp.Tables[0].Rows[0]["sender_fbid"]);
            }
            if (dstemp.Tables[0].Rows[0]["receiver_fbid"] != DBNull.Value)
            {
                tempgift.receiver_fbid = Convert.ToInt64(dstemp.Tables[0].Rows[0]["receiver_fbid"]);
            }

            if (dstemp.Tables[0].Rows[0]["receiver_email"] != DBNull.Value)
            {
                tempgift.receiver_email = dstemp.Tables[0].Rows[0]["receiver_email"].ToString();
            }

            if (dstemp.Tables[0].Rows[0]["witty_message"] != DBNull.Value)
            {
                tempgift.witty_message = dstemp.Tables[0].Rows[0]["witty_message"].ToString();
            }

            if (dstemp.Tables[0].Rows[0]["created_date"] != DBNull.Value)
            {
                tempgift.created_date = Convert.ToDateTime(dstemp.Tables[0].Rows[0]["created_date"]);
            }

            if (dstemp.Tables[0].Rows[0]["blueikon"] != DBNull.Value)
            {
                tempgift.blueikon = Convert.ToInt32(dstemp.Tables[0].Rows[0]["blueikon"]);
            }

            if (dstemp.Tables[0].Rows[0]["fbpost"] != DBNull.Value)
            {
                tempgift.fbpost = Convert.ToBoolean(dstemp.Tables[0].Rows[0]["fbpost"]);
            }

            if (dstemp.Tables[0].Rows[0]["receiver_name"] != DBNull.Value)
            {
                tempgift.receiver_name = dstemp.Tables[0].Rows[0]["receiver_name"].ToString();
            }

            dstemp = BlueIkons_DB.SPs.ViewTransactionsGiftKey(Gift_Key).GetDataSet();
            if (dstemp.Tables[0].Rows[0]["Amount"] != DBNull.Value)
            {
                tempgift.amount = Convert.ToDecimal(dstemp.Tables[0].Rows[0]["Amount"]);
            }
            if (dstemp.Tables[0].Rows[0]["Tx_Key"] != DBNull.Value)
            {
                tempgift.txkey = Convert.ToInt32(dstemp.Tables[0].Rows[0]["Tx_Key"]);
            }

            return tempgift;
        }
    }
}