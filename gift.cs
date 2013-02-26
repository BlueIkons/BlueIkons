using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace BlueIkons
{
    public class gift
    {
        private int _Gift_Key;
        private Int64 _sender_fbid;
        private Int64 _receiver_fbid;
        private string _receiver_email;
        private string _witty_message;
        private DateTime _created_date;
        private int _blueikon;
        private Boolean _fbpost;
        private string _receiver_name;
        private decimal _amount;
        private int _txkey;

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

        public Int64 sender_fbid
        {
            get
            {
                return _sender_fbid;
            }
            set
            {
                _sender_fbid = value;
            }
        }

        public Int64 receiver_fbid
        {
            get
            {
                return _receiver_fbid;
            }
            set
            {
                _receiver_fbid = value;
            }
        }

        public string witty_message
        {
            get
            {
                return _witty_message;
            }
            set
            {
                _witty_message = value;
            }
        }

        public DateTime created_date
        {
            get
            {
                return _created_date;
            }
            set
            {
                _created_date = value;
            }
        }

        public int blueikon
        {
            get
            {
                return _blueikon;
            }
            set
            {
                _blueikon = value;
            }
        }

        public Boolean fbpost
        {
            get
            {
                return _fbpost;
            }
            set
            {
                _fbpost = value;
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

        public string receiver_name
        {
            get
            {
                return _receiver_name;
            }
            set
            {
                _receiver_name = value;
            }
        }

        public decimal amount
        {
            get
            {
                return _amount;
            }
            set
            {
                _amount = value;
            }
        }

        public int txkey
        {
            get
            {
                return _txkey;
            }
            set
            {
                _txkey = value;
            }
        }
    }
}