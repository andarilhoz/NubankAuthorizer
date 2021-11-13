using System;
using System.Numerics;

namespace NubankAuthorizer.Models
{
    public class Operations
    {
        
    }

    public class Account
    {
        public bool ActiveCard;
        public BigInteger AvailableLimit;
    }

    public class OperationTransaction
    {
        public string Merchant;
        public BigInteger Amount;
        public DateTime Time;
    }
}