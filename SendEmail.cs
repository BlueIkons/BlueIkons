using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Net.Mail;
using System.Configuration;

namespace BlueIkons
{
    public class SendEmail
    {
        public void Send_Email(string From, string To, string Subject, string Body)
        {
            string EmailFrom;
            if (From == "")// put the from address here
            {
                EmailFrom = ConfigurationManager.AppSettings.Get("Default_Email_From").ToString();
            }
            else
            {
                EmailFrom = From;
            }
            
            try
            {
                MailMessage mail = new MailMessage(EmailFrom, To);

                //mail.To = To;             // put to address here
                mail.Subject = Subject;        // put subject here	
                mail.Body = Body;           // put body of email here            
                //SmtpMail.SmtpServer = "localhost"; // put smtp server you will use here 
                // and then send the mail
                mail.IsBodyHtml = true;

                SmtpClient smtp = new SmtpClient();
                smtp.Send(mail);
            }
            catch
            {
            }

        }

        public void Send_GiftEmail(int giftkey,int type)
        {
            Site sitetemp = new Site();

            gift giftinfo = sitetemp.GetGiftInfo(giftkey);
            fbuser fbuser = sitetemp.Getfbuser(giftinfo.sender_fbid);


            string thebody="";
            string strsubject="";
            string stremailaddress = "";
            //type = 0 Sender email
            //type = 1 Receiver email
            if (type == 0){
                thebody = System.IO.File.ReadAllText(HttpContext.Current.Server.MapPath("/Emails/SenderEmail1.txt"));
                strsubject = "Your BlueIkons gift has been sent";
                stremailaddress = fbuser.Email;
            }
            else if (type == 1)
            {
                thebody = System.IO.File.ReadAllText(HttpContext.Current.Server.MapPath("/Emails/ReceiverEmail1.txt"));
                strsubject = fbuser.Firstname + " " + fbuser.Lastname + " has sent you a BlueIkons gift of " + decimal.Round(giftinfo.amount,2).ToString();
                stremailaddress = giftinfo.receiver_email;
            }
            string strpicurl = ConfigurationManager.AppSettings.Get("BlueIkons_Pics").ToString() + giftinfo.blueikon.ToString() + ".png";
            string strapprurl = ConfigurationManager.AppSettings.Get("App_URL").ToString() + "getgift.aspx?invite=" + giftkey.ToString();

            thebody = thebody.Replace("FIRSTNAME", fbuser.Firstname);
            thebody = thebody.Replace("LASTNAME", fbuser.Lastname);
            thebody = thebody.Replace("AMOUNT", decimal.Round(giftinfo.amount,2).ToString());
            thebody = thebody.Replace("WITTYMESSAGE", giftinfo.witty_message);
            thebody = thebody.Replace("IMAGE", strpicurl);
            thebody = thebody.Replace("CLAIMURL", strapprurl);
            thebody = thebody.Replace("RECEIVEREMAIL", giftinfo.receiver_email);                       

            Send_Email(fbuser.Email, stremailaddress, strsubject, thebody);
        }
    }
}