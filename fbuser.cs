using System;
using System.Data;
using System.Configuration;
using System.Linq;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using System.Xml.Linq;

namespace BlueIkons
{
    public class fbuser
    {
            private Int64 uid;
            private string firstname;
            private string lastname;
            private string fullname;
            private string accesstoken;
            private string sessionkey;
            private string email;
            private string _paypalemail;
            private string approoturl;


            public Int64 UID
            {
                get
                {
                    return uid;
                }
                set
                {
                    uid = value;
                }
            }
            public string Firstname
            {
                get
                {
                    return firstname;
                }
                set
                {
                    firstname = value;
                }
            }

            public string Lastname
            {
                get
                {
                    return lastname;
                }
                set
                {
                    lastname = value;
                }
            }

            public string Fullname
            {
                get
                {
                    return fullname;
                }
                set
                {
                    fullname = value;
                }
            }

            public string AccessToken
            {
                get
                {
                    return accesstoken;
                }
                set
                {
                    accesstoken = value;
                }
            }

            public string SessionKey
            {
                get
                {
                    return sessionkey;
                }
                set
                {
                    sessionkey = value;
                }
            }
            public string Email
            {
                get
                {
                    return email;
                }
                set
                {
                    email = value;
                }
            }
            public string paypalemail
            {
                get
                {
                    return _paypalemail;
                }
                set
                {
                    _paypalemail = value;
                }
            }
            public string AppRootUrl
            {
                get
                {
                    return approoturl;
                }
                set
                {
                    approoturl = value;
                }
            }
    }
}
