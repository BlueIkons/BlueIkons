using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using PayPal.Platform.SDK;
using PayPal.Services.Private.AP;
using System.Configuration;
using System.Net;
using Newtonsoft.Json.Linq;
using System.Data;
using System.Collections;

namespace BlueIkons
{
    public class PayPal
    {

        public Boolean PreapprovalActive(string pakey, bool Live_Trial, int txkey)
        {
            Boolean isactive = false;

            PreapprovalDetailsRequest preapprovalRequest = null;
            BaseAPIProfile profile2 = new BaseAPIProfile();

            profile2.APIProfileType = ProfileType.ThreeToken;

            if (Live_Trial)//true = Live , false = trial
            {
                profile2.Environment = System.Configuration.ConfigurationSettings.AppSettings.Get("Live_Environment").ToString();
                profile2.ApplicationID = System.Configuration.ConfigurationSettings.AppSettings.Get("AppID_Live").ToString();
                profile2.APIUsername = System.Configuration.ConfigurationSettings.AppSettings.Get("APIUsername_Live").ToString();
                profile2.APIPassword = System.Configuration.ConfigurationSettings.AppSettings.Get("APIPassword_Live").ToString();
                profile2.APISignature = System.Configuration.ConfigurationSettings.AppSettings.Get("APISignature_Live").ToString();
            }
            else
            {
                profile2.Environment = System.Configuration.ConfigurationSettings.AppSettings.Get("Trial_Environment").ToString();
                profile2.ApplicationID = System.Configuration.ConfigurationSettings.AppSettings.Get("AppID").ToString();
                profile2.APIUsername = System.Configuration.ConfigurationSettings.AppSettings.Get("APIUsername").ToString();
                profile2.APIPassword = System.Configuration.ConfigurationSettings.AppSettings.Get("APIPassword").ToString();
                profile2.APISignature = System.Configuration.ConfigurationSettings.AppSettings.Get("APISignature").ToString();
            }
            profile2.RequestDataformat = "SOAP11";
            profile2.ResponseDataformat = "SOAP11";

            profile2.IsTrustAllCertificates = Convert.ToBoolean(ConfigurationManager.AppSettings["TrustAll"]);

            try
            {

                preapprovalRequest = new PreapprovalDetailsRequest();
                
                //preapprovalRequest.senderEmail = senderEmail.Value;
                preapprovalRequest.requestEnvelope = new RequestEnvelope();
                preapprovalRequest.requestEnvelope.errorLanguage = "en-US";
                preapprovalRequest.preapprovalKey = pakey;
                

                AdapativePayments ap = new AdapativePayments();
                ap.APIProfile = profile2;

                PreapprovalDetailsResponse PResponse = ap.preapprovalDetails(preapprovalRequest);

                if (ap.isSuccess.ToUpper() == "FAILURE")
                {
                    //HttpContext.Current.Session[Constants.SessionConstants.FAULT] = ap.LastError;
                    //HttpContext.Current.Response.Redirect("APIError.aspx", false);
                }
                else
                {
                    if (PResponse.status == "ACTIVE")
                    {
                        isactive = true;
                    }
                    else if ((PResponse.status == "CANCELED") || (PResponse.status == "DEACTIVED"))
                    {
                        BlueIkons_DB.SPs.UpdateTransactionExpired(txkey).Execute();
                    }
                }


            }
            catch (FATALException FATALEx)
            {
                //   Session[Constants.SessionConstants.FATALEXCEPTION] = FATALEx;
                // this.Response.Redirect(Constants.ASPXPages.APIERROR + "?" + Constants.QueryStringConstants.TYPE + "=FATAL", false);

            }
            catch (Exception ex)
            {

                //FATALException FATALEx = new FATALException("Error occurred in PreApproval Page.", ex);
                //Session[Constants.SessionConstants.FATALEXCEPTION] = FATALEx;
                //this.Response.Redirect("APIError.aspx?type=FATAL", false);

            }


            return isactive;
        }

        public string SetPreapproval(fbuser fbuser, bool Live_Trial, int Tx_Key, decimal amount)
        {
            string strReturn = "";
            PreapprovalRequest preapprovalRequest = null;
            BaseAPIProfile profile2 = new BaseAPIProfile();

            profile2.APIProfileType = ProfileType.ThreeToken;

            if (Live_Trial)//true = Live , false = trial
            {
                profile2.Environment = System.Configuration.ConfigurationSettings.AppSettings.Get("Live_Environment").ToString();
                profile2.ApplicationID = System.Configuration.ConfigurationSettings.AppSettings.Get("AppID_Live").ToString();
                profile2.APIUsername = System.Configuration.ConfigurationSettings.AppSettings.Get("APIUsername_Live").ToString();
                profile2.APIPassword = System.Configuration.ConfigurationSettings.AppSettings.Get("APIPassword_Live").ToString();
                profile2.APISignature = System.Configuration.ConfigurationSettings.AppSettings.Get("APISignature_Live").ToString();
            }
            else
            {
                profile2.Environment = System.Configuration.ConfigurationSettings.AppSettings.Get("Trial_Environment").ToString();
                profile2.ApplicationID = System.Configuration.ConfigurationSettings.AppSettings.Get("AppID").ToString();
                profile2.APIUsername = System.Configuration.ConfigurationSettings.AppSettings.Get("APIUsername").ToString();
                profile2.APIPassword = System.Configuration.ConfigurationSettings.AppSettings.Get("APIPassword").ToString();
                profile2.APISignature = System.Configuration.ConfigurationSettings.AppSettings.Get("APISignature").ToString();
            }
            profile2.RequestDataformat = "SOAP11";
            profile2.ResponseDataformat = "SOAP11";
                    
            profile2.IsTrustAllCertificates = Convert.ToBoolean(ConfigurationManager.AppSettings["TrustAll"]);
            
            try
            {
                decimal amountfee1 = (amount * Convert.ToDecimal(0.029)) + Convert.ToDecimal(0.3);
                decimal amountblueikon = (amount * Convert.ToDecimal(0.1));
                decimal amountfee2 = (amountblueikon * Convert.ToDecimal(0.029)) + Convert.ToDecimal(0.3);
                amount = amount + amountblueikon + amountfee1 + amountfee2;
                amount = decimal.Round(amount, 2);

                string url = System.Configuration.ConfigurationSettings.AppSettings.Get("App_URL").ToString() ;
                string returnURL = "http://www.blueikons.com/Order_Confirmation.aspx?Tx_key=" + Tx_Key.ToString();
                string cancelURL = url;
                preapprovalRequest = new PreapprovalRequest();
                preapprovalRequest.cancelUrl = cancelURL;
                preapprovalRequest.returnUrl = returnURL;
                //preapprovalRequest.senderEmail = senderEmail.Value;
                preapprovalRequest.requestEnvelope = new RequestEnvelope();
                preapprovalRequest.requestEnvelope.errorLanguage = "en-US";
                preapprovalRequest.maxNumberOfPayments = 2;
                preapprovalRequest.maxTotalAmountOfAllPayments = amount;
                preapprovalRequest.maxTotalAmountOfAllPaymentsSpecified = true;
                preapprovalRequest.currencyCode = "USD";
                preapprovalRequest.startingDate = DateTime.Today;
                preapprovalRequest.endingDate = DateTime.Today.AddMonths(1);
                preapprovalRequest.endingDateSpecified = true;
                preapprovalRequest.clientDetails = new ClientDetailsType();                
                //preapprovalRequest.clientDetails = ClientInfoUtil.getMyAppDetails();
                preapprovalRequest.memo = "BlueIkons";
                preapprovalRequest.maxNumberOfPayments = 2;
                preapprovalRequest.displayMaxTotalAmount = true;
                preapprovalRequest.displayMaxTotalAmountSpecified = true;
                preapprovalRequest.feesPayer = "SENDER";

                AdapativePayments ap = new AdapativePayments();
                ap.APIProfile = profile2;

                PreapprovalResponse PResponse = ap.preapproval(preapprovalRequest);

                if (ap.isSuccess.ToUpper() == "FAILURE")
                {
                    //HttpContext.Current.Session[Constants.SessionConstants.FAULT] = ap.LastError;
                    //HttpContext.Current.Response.Redirect("APIError.aspx", false);
                }
                else
                {

                   // Session[Constants.SessionConstants.PREAPPROVALKEY] = PResponse.preapprovalKey;
                    //this.Response.Redirect(ConfigurationManager.AppSettings["PAYPAL_REDIRECT_URL"] + "_ap-preapproval&preapprovalkey=" + PResponse.preapprovalKey, false);
                    BlueIkons_DB.SPs.UpdateTransactionPakey(Tx_Key, PResponse.preapprovalKey).Execute();
                    if (Live_Trial)//true = Live , false = trial
                    {
                        //HttpContext.Current.Response.Redirect("https://paypal.com/webapps/adaptivepayment/flow/pay?paykey=" + PResponse.preapprovalKey, false);
                    }
                    else{
                        //HttpContext.Current.Response.Redirect("https://www.sandbox.paypal.com/webscr?cmd=" + "_ap-payment&paykey=" + PResponse.preapprovalKey, false);                                                
                    }
                    strReturn = PResponse.preapprovalKey;
                }


            }
            catch (FATALException FATALEx)
            {
             //   Session[Constants.SessionConstants.FATALEXCEPTION] = FATALEx;
               // this.Response.Redirect(Constants.ASPXPages.APIERROR + "?" + Constants.QueryStringConstants.TYPE + "=FATAL", false);
              
            }
            catch (Exception ex)
            {
                
                //FATALException FATALEx = new FATALException("Error occurred in PreApproval Page.", ex);
                //Session[Constants.SessionConstants.FATALEXCEPTION] = FATALEx;
                //this.Response.Redirect("APIError.aspx?type=FATAL", false);
                 
            }
            return strReturn;
        }

        #region Parallel Payments        
        public Boolean ParallelPayment(bool Live_Trial, gift giftinfo, string receiveremail)
        {

            Boolean txcompleted = false;
            BaseAPIProfile profile2 = new BaseAPIProfile();
            int Tx_Key = giftinfo.txkey;
            Site sitetemp = new Site();
            Transactions txinfo = sitetemp.Gettx(giftinfo.txkey);


            ////Three token 
             
            profile2.APIProfileType = ProfileType.ThreeToken;
            string blueikonemail = "";
            
            if (Live_Trial)//true = Live , false = trial
            {
                profile2.Environment = System.Configuration.ConfigurationSettings.AppSettings.Get("Live_Environment").ToString();
                profile2.ApplicationID = System.Configuration.ConfigurationSettings.AppSettings.Get("AppID_Live").ToString();
                profile2.APIUsername = System.Configuration.ConfigurationSettings.AppSettings.Get("APIUsername_Live").ToString();
                profile2.APIPassword = System.Configuration.ConfigurationSettings.AppSettings.Get("APIPassword_Live").ToString();
                profile2.APISignature = System.Configuration.ConfigurationSettings.AppSettings.Get("APISignature_Live").ToString();
                blueikonemail = ConfigurationSettings.AppSettings.Get("My_Email_Live").ToString();
            }
            else
            {
                profile2.Environment = System.Configuration.ConfigurationSettings.AppSettings.Get("Trial_Environment").ToString();
                profile2.ApplicationID = System.Configuration.ConfigurationSettings.AppSettings.Get("AppID").ToString();
                profile2.APIUsername = System.Configuration.ConfigurationSettings.AppSettings.Get("APIUsername").ToString();
                profile2.APIPassword = System.Configuration.ConfigurationSettings.AppSettings.Get("APIPassword").ToString();
                profile2.APISignature = System.Configuration.ConfigurationSettings.AppSettings.Get("APISignature").ToString();                
                blueikonemail = ConfigurationSettings.AppSettings.Get("My_Email_Trial").ToString();
            }
            /*profile2.RequestDataformat = "SOAP11";
            profile2.ResponseDataformat = "SOAP11";
            */            


            profile2.IsTrustAllCertificates = Convert.ToBoolean(ConfigurationManager.AppSettings["TrustAll"]);

            string url = System.Configuration.ConfigurationSettings.AppSettings.Get("Callback").ToString() + "/";
            string returnURL = url + "Order_Confirmation.aspx?Tx_key=" + Tx_Key.ToString();
            string cancelURL = url + "Order_Confirmation.aspx?Tx_key=0";

            PayRequest payRequest = null;
            payRequest = new PayRequest();
            payRequest.cancelUrl = cancelURL;
            payRequest.returnUrl = returnURL;            
            payRequest.reverseAllParallelPaymentsOnError = true;
            

            //payRequest.senderEmail = email.Value;
            //payRequest.clientDetails = new ClientDetailsType();
            //payRequest.clientDetails = ClientInfoUtil.getMyAppDetails();            

            payRequest.feesPayer = "SENDER";//feesPayer.Value;
            payRequest.memo = "BlueIkons";// memo.Value;
            payRequest.actionType = "PAY";
            payRequest.currencyCode = "USD"; //currencyCode.Items[currencyCode.SelectedIndex].Value;            
            payRequest.requestEnvelope = new RequestEnvelope();
            payRequest.requestEnvelope.errorLanguage = "en_US";//ClientInfoUtil.getMyAppRequestEnvelope();             
            payRequest.preapprovalKey = txinfo.pakey;

            payRequest.receiverList = new Receiver[2];
            payRequest.receiverList[0] = new Receiver();
            payRequest.receiverList[0].amount = decimal.Round(giftinfo.amount,2);//amount_0.Value);
            payRequest.receiverList[0].email = receiveremail;//receiveremail_0.Value;                        

            decimal blueikonamount = (giftinfo.amount * Convert.ToDecimal(.1));
            if (receiveremail != blueikonemail)
            {                
                payRequest.receiverList[1] = new Receiver();
                payRequest.receiverList[1].amount = decimal.Round(blueikonamount, 2);
                payRequest.receiverList[1].email = blueikonemail;
            }
            else
            {
                payRequest.receiverList[0].amount += decimal.Round(blueikonamount, 2);                
            }
            //Eventomatic_DB.SPs.UpdateTransactionTicketAmountEmail(Tx_Key, strEmail1).Execute();

            profile2.ResponseDataformat = "SOAP11";
            profile2.RequestDataformat = "SOAP11";
                        

            AdapativePayments ap = new AdapativePayments();
            
            ap.APIProfile = profile2;                        

            PayResponse PResponse = ap.pay(payRequest);            

            if (ap.isSuccess.ToUpper() == "FAILURE")
            {
                //HttpContext.Current.Session[Constants.SessionConstants.FAULT] = ap.LastError;                
                for (int i = 0; i <= ap.LastError.ErrorDetails.Length - 1; i++)
                {
                    FaultDetailFaultMessageError ETtemp = (FaultDetailFaultMessageError)ap.LastError.ErrorDetails.GetValue(i);
                    //decimal OverallTotal = decimal.Round(dcAmount1, 2) + decimal.Round(dcAmount2, 2);
                    //Eventomatic_DB.SPs.UpdateCCErrors(ETtemp.message.ToString(), Tx_Key, 2, OverallTotal.ToString()).Execute();
                }
                //HttpContext.Current.Response.Redirect("APIError.aspx", false);
            }
            else
            {
                //Payment went through
                BlueIkons_DB.SPs.UpdateTransaction(Tx_Key, 0, 0, 0, 3, PResponse.responseEnvelope.correlationId, receiveremail).Execute();
                txcompleted = true;
            }
            return txcompleted;
        }
        #endregion

    }
}