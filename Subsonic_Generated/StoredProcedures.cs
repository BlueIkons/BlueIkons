using System; 
using System.Text; 
using System.Data;
using System.Data.SqlClient;
using System.Data.Common;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Configuration; 
using System.Xml; 
using System.Xml.Serialization;
using SubSonic; 
using SubSonic.Utilities;
// <auto-generated />
namespace BlueIkons_DB{
    public partial class SPs{
        
        /// <summary>
        /// Creates an object wrapper for the Update_Charity Procedure
        /// </summary>
        public static StoredProcedure UpdateCharity(string CharityName, string CharityDescription, string CharityEmail)
        {
            SubSonic.StoredProcedure sp = new SubSonic.StoredProcedure("Update_Charity", DataService.GetInstance("BlueIkons"), "dbo");
        	
            sp.Command.AddParameter("@Charity_Name", CharityName, DbType.String, null, null);
        	
            sp.Command.AddParameter("@Charity_Description", CharityDescription, DbType.String, null, null);
        	
            sp.Command.AddParameter("@Charity_Email", CharityEmail, DbType.String, null, null);
        	
            return sp;
        }
        
        /// <summary>
        /// Creates an object wrapper for the Update_FBUser Procedure
        /// </summary>
        public static StoredProcedure UpdateFBUser(long? FBid, string FirstName, string LastName, string Email, string AccessToken)
        {
            SubSonic.StoredProcedure sp = new SubSonic.StoredProcedure("Update_FBUser", DataService.GetInstance("BlueIkons"), "dbo");
        	
            sp.Command.AddParameter("@FBid", FBid, DbType.Int64, 0, 19);
        	
            sp.Command.AddParameter("@First_Name", FirstName, DbType.AnsiString, null, null);
        	
            sp.Command.AddParameter("@Last_Name", LastName, DbType.AnsiString, null, null);
        	
            sp.Command.AddParameter("@Email", Email, DbType.AnsiString, null, null);
        	
            sp.Command.AddParameter("@Access_Token", AccessToken, DbType.String, null, null);
        	
            return sp;
        }
        
        /// <summary>
        /// Creates an object wrapper for the Update_Gift Procedure
        /// </summary>
        public static StoredProcedure UpdateGift(int? GiftKey, long? senderfbid, long? receiverfbid, string receiveremail, string witty, int? blueikon, int? GiftKeyReturn, bool? fbpost, string receivername)
        {
            SubSonic.StoredProcedure sp = new SubSonic.StoredProcedure("Update_Gift", DataService.GetInstance("BlueIkons"), "dbo");
        	
            sp.Command.AddParameter("@Gift_Key", GiftKey, DbType.Int32, 0, 10);
        	
            sp.Command.AddParameter("@senderfbid", senderfbid, DbType.Int64, 0, 19);
        	
            sp.Command.AddParameter("@receiverfbid", receiverfbid, DbType.Int64, 0, 19);
        	
            sp.Command.AddParameter("@receiveremail", receiveremail, DbType.String, null, null);
        	
            sp.Command.AddParameter("@witty", witty, DbType.String, null, null);
        	
            sp.Command.AddParameter("@blueikon", blueikon, DbType.Int32, 0, 10);
        	
            sp.Command.AddOutputParameter("@Gift_Key_Return", DbType.Int32, 0, 10);
            
            sp.Command.AddParameter("@fbpost", fbpost, DbType.Boolean, null, null);
        	
            sp.Command.AddParameter("@receivername", receivername, DbType.String, null, null);
        	
            return sp;
        }
        
        /// <summary>
        /// Creates an object wrapper for the Update_Gift_Receiver Procedure
        /// </summary>
        public static StoredProcedure UpdateGiftReceiver(int? GiftKey, long? receiverfbid)
        {
            SubSonic.StoredProcedure sp = new SubSonic.StoredProcedure("Update_Gift_Receiver", DataService.GetInstance("BlueIkons"), "dbo");
        	
            sp.Command.AddParameter("@Gift_Key", GiftKey, DbType.Int32, 0, 10);
        	
            sp.Command.AddParameter("@receiverfbid", receiverfbid, DbType.Int64, 0, 19);
        	
            return sp;
        }
        
        /// <summary>
        /// Creates an object wrapper for the Update_Transaction Procedure
        /// </summary>
        public static StoredProcedure UpdateTransaction(int? TxKey, decimal? Amount, int? TxKeyReturn, int? GiftKey, int? TxStatus, string txnid, string receiveremail)
        {
            SubSonic.StoredProcedure sp = new SubSonic.StoredProcedure("Update_Transaction", DataService.GetInstance("BlueIkons"), "dbo");
        	
            sp.Command.AddParameter("@Tx_Key", TxKey, DbType.Int32, 0, 10);
        	
            sp.Command.AddParameter("@Amount", Amount, DbType.Currency, 4, 19);
        	
            sp.Command.AddOutputParameter("@Tx_Key_Return", DbType.Int32, 0, 10);
            
            sp.Command.AddParameter("@Gift_Key", GiftKey, DbType.Int32, 0, 10);
        	
            sp.Command.AddParameter("@Tx_Status", TxStatus, DbType.Int32, 0, 10);
        	
            sp.Command.AddParameter("@txn_id", txnid, DbType.String, null, null);
        	
            sp.Command.AddParameter("@receiver_email", receiveremail, DbType.String, null, null);
        	
            return sp;
        }
        
        /// <summary>
        /// Creates an object wrapper for the Update_Transaction_expired Procedure
        /// </summary>
        public static StoredProcedure UpdateTransactionExpired(int? TxKey)
        {
            SubSonic.StoredProcedure sp = new SubSonic.StoredProcedure("Update_Transaction_expired", DataService.GetInstance("BlueIkons"), "dbo");
        	
            sp.Command.AddParameter("@Tx_Key", TxKey, DbType.Int32, 0, 10);
        	
            return sp;
        }
        
        /// <summary>
        /// Creates an object wrapper for the Update_Transaction_pakey Procedure
        /// </summary>
        public static StoredProcedure UpdateTransactionPakey(int? TxKey, string pakey)
        {
            SubSonic.StoredProcedure sp = new SubSonic.StoredProcedure("Update_Transaction_pakey", DataService.GetInstance("BlueIkons"), "dbo");
        	
            sp.Command.AddParameter("@Tx_Key", TxKey, DbType.Int32, 0, 10);
        	
            sp.Command.AddParameter("@pakey", pakey, DbType.String, null, null);
        	
            return sp;
        }
        
        /// <summary>
        /// Creates an object wrapper for the View_Charity Procedure
        /// </summary>
        public static StoredProcedure ViewCharity(int? GiftKey)
        {
            SubSonic.StoredProcedure sp = new SubSonic.StoredProcedure("View_Charity", DataService.GetInstance("BlueIkons"), "dbo");
        	
            sp.Command.AddParameter("@Gift_Key", GiftKey, DbType.Int32, 0, 10);
        	
            return sp;
        }
        
        /// <summary>
        /// Creates an object wrapper for the View_fbidpendinggifts Procedure
        /// </summary>
        public static StoredProcedure ViewFbidpendinggifts(long? fbid)
        {
            SubSonic.StoredProcedure sp = new SubSonic.StoredProcedure("View_fbidpendinggifts", DataService.GetInstance("BlueIkons"), "dbo");
        	
            sp.Command.AddParameter("@fbid", fbid, DbType.Int64, 0, 19);
        	
            return sp;
        }
        
        /// <summary>
        /// Creates an object wrapper for the View_FBUser Procedure
        /// </summary>
        public static StoredProcedure ViewFBUser(long? fbid)
        {
            SubSonic.StoredProcedure sp = new SubSonic.StoredProcedure("View_FBUser", DataService.GetInstance("BlueIkons"), "dbo");
        	
            sp.Command.AddParameter("@fbid", fbid, DbType.Int64, 0, 19);
        	
            return sp;
        }
        
        /// <summary>
        /// Creates an object wrapper for the View_Gift Procedure
        /// </summary>
        public static StoredProcedure ViewGift(int? GiftKey)
        {
            SubSonic.StoredProcedure sp = new SubSonic.StoredProcedure("View_Gift", DataService.GetInstance("BlueIkons"), "dbo");
        	
            sp.Command.AddParameter("@Gift_Key", GiftKey, DbType.Int32, 0, 10);
        	
            return sp;
        }
        
        /// <summary>
        /// Creates an object wrapper for the View_Transactions Procedure
        /// </summary>
        public static StoredProcedure ViewTransactions(int? txkey)
        {
            SubSonic.StoredProcedure sp = new SubSonic.StoredProcedure("View_Transactions", DataService.GetInstance("BlueIkons"), "dbo");
        	
            sp.Command.AddParameter("@txkey", txkey, DbType.Int32, 0, 10);
        	
            return sp;
        }
        
        /// <summary>
        /// Creates an object wrapper for the View_Transactions_GiftKey Procedure
        /// </summary>
        public static StoredProcedure ViewTransactionsGiftKey(int? giftkey)
        {
            SubSonic.StoredProcedure sp = new SubSonic.StoredProcedure("View_Transactions_GiftKey", DataService.GetInstance("BlueIkons"), "dbo");
        	
            sp.Command.AddParameter("@giftkey", giftkey, DbType.Int32, 0, 10);
        	
            return sp;
        }
        
    }
    
}