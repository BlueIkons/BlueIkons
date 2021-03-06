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
namespace BlueIkons_DB
{
    /// <summary>
    /// Controller class for Transactions
    /// </summary>
    [System.ComponentModel.DataObject]
    public partial class TransactionController
    {
        // Preload our schema..
        Transaction thisSchemaLoad = new Transaction();
        private string userName = String.Empty;
        protected string UserName
        {
            get
            {
				if (userName.Length == 0) 
				{
    				if (System.Web.HttpContext.Current != null)
    				{
						userName=System.Web.HttpContext.Current.User.Identity.Name;
					}
					else
					{
						userName=System.Threading.Thread.CurrentPrincipal.Identity.Name;
					}
				}
				return userName;
            }
        }
        [DataObjectMethod(DataObjectMethodType.Select, true)]
        public TransactionCollection FetchAll()
        {
            TransactionCollection coll = new TransactionCollection();
            Query qry = new Query(Transaction.Schema);
            coll.LoadAndCloseReader(qry.ExecuteReader());
            return coll;
        }
        [DataObjectMethod(DataObjectMethodType.Select, false)]
        public TransactionCollection FetchByID(object TxKey)
        {
            TransactionCollection coll = new TransactionCollection().Where("Tx_Key", TxKey).Load();
            return coll;
        }
		
		[DataObjectMethod(DataObjectMethodType.Select, false)]
        public TransactionCollection FetchByQuery(Query qry)
        {
            TransactionCollection coll = new TransactionCollection();
            coll.LoadAndCloseReader(qry.ExecuteReader()); 
            return coll;
        }
        [DataObjectMethod(DataObjectMethodType.Delete, true)]
        public bool Delete(object TxKey)
        {
            return (Transaction.Delete(TxKey) == 1);
        }
        [DataObjectMethod(DataObjectMethodType.Delete, false)]
        public bool Destroy(object TxKey)
        {
            return (Transaction.Destroy(TxKey) == 1);
        }
        
        
    	
	    /// <summary>
	    /// Inserts a record, can be used with the Object Data Source
	    /// </summary>
        [DataObjectMethod(DataObjectMethodType.Insert, true)]
	    public void Insert(decimal? Amount,int? GiftKey,DateTime? InitDate,DateTime? CollectedDate,string TxnId,int? TxStatus,string Pakey,string ReceiverEmail)
	    {
		    Transaction item = new Transaction();
		    
            item.Amount = Amount;
            
            item.GiftKey = GiftKey;
            
            item.InitDate = InitDate;
            
            item.CollectedDate = CollectedDate;
            
            item.TxnId = TxnId;
            
            item.TxStatus = TxStatus;
            
            item.Pakey = Pakey;
            
            item.ReceiverEmail = ReceiverEmail;
            
	    
		    item.Save(UserName);
	    }
    	
	    /// <summary>
	    /// Updates a record, can be used with the Object Data Source
	    /// </summary>
        [DataObjectMethod(DataObjectMethodType.Update, true)]
	    public void Update(int TxKey,decimal? Amount,int? GiftKey,DateTime? InitDate,DateTime? CollectedDate,string TxnId,int? TxStatus,string Pakey,string ReceiverEmail)
	    {
		    Transaction item = new Transaction();
	        item.MarkOld();
	        item.IsLoaded = true;
		    
			item.TxKey = TxKey;
				
			item.Amount = Amount;
				
			item.GiftKey = GiftKey;
				
			item.InitDate = InitDate;
				
			item.CollectedDate = CollectedDate;
				
			item.TxnId = TxnId;
				
			item.TxStatus = TxStatus;
				
			item.Pakey = Pakey;
				
			item.ReceiverEmail = ReceiverEmail;
				
	        item.Save(UserName);
	    }
    }
}
