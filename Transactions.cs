using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace BlueIkons
{
    public class Transactions
    {

        private int _Tx_Key;
        private decimal _Amount;
        private int _Gift_Key;
        private DateTime _Init_date;
        private DateTime _Collected_date;
        private string _txn_id;
        private int _Tx_Status;
        private string _pakey;
        private string _receiver_email;

        public int Tx_Key
        {
            get
            {
                return _Tx_Key;
            }
            set
            {
                _Tx_Key = value;
            }
        }

        public decimal Amount
        {
            get
            {
                return _Amount;
            }
            set
            {
                _Amount = value;
            }
        }

        public int Gift_Key
        {
            get
            {
                return _Gift_Key;
            }
            set
            {
                _Gift_Key = value;
            }
        }

        public DateTime Init_date
        {
            get
            {
                return _Init_date;
            }
            set
            {
                _Init_date = value;
            }
        }

        public DateTime Collected_date
        {
            get
            {
                return _Collected_date;
            }
            set
            {
                _Collected_date = value;
            }
        }

        public string txn_id
        {
            get
            {
                return _txn_id;
            }
            set
            {
                _txn_id = value;
            }
        }

        public int Tx_Status
        {
            get
            {
                return _Tx_Status;
            }
            set
            {
                _Tx_Status = value;
            }
        }

        public string pakey
        {
            get
            {
                return _pakey;
            }
            set
            {
                _pakey = value;
            }
        }

        public string receiver_email
        {
            get
            {
                return _receiver_email;
            }
            set
            {
                _receiver_email = value;
            }
        }        
    }
}