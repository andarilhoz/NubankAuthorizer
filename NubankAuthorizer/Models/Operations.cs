using System;
using Newtonsoft.Json;

namespace NubankAuthorizer.Models
{
    public class Operations
    {
        [JsonProperty("account")]
        public Account Account { get; set; }
        
        [JsonProperty("transaction")]
        public OperationTransaction Transaction { get; set; }
    }

    public class Account: IEquatable<Account>
    {
        [JsonProperty("active-card")]
        public bool ActiveCard { get; set; }
        
        [JsonProperty("available-limit")]
        public int AvailableLimit { get; set; }
        
        public bool Equals(Account other)
        {
            return ActiveCard == other.ActiveCard && AvailableLimit == other.AvailableLimit;
        }

        public override bool Equals(object obj)
        {
            Account other = obj as Account;
            if( other == null ) 
                return false;
            return Equals (other);    
        }
    }

    public class OperationTransaction: IEquatable<OperationTransaction>
    {
        [JsonProperty("merchant")]
        public string Merchant { get; set; }
        
        [JsonProperty("amount")]
        public int Amount { get; set; }
        
        [JsonProperty("time")]
        public DateTime Time { get; set; }

        public bool Equals(OperationTransaction other)
        {
            if (ReferenceEquals(null, other)) return false;
            if (ReferenceEquals(this, other)) return true;
            return Merchant == other.Merchant && Amount == other.Amount && Time.Equals(other.Time);
        }

        public override bool Equals(object obj)
        {
            OperationTransaction other = obj as OperationTransaction;
            if( other == null ) 
                return false;
            return Equals (other);    
        }
    }
}