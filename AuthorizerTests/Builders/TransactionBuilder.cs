using System;
using System.Numerics;

namespace NubankAuthorizer.Models
{
    public class TransactionBuilder
    {
        private BigInteger amount;
        private string merchant;
        private DateTime time = DateTime.Now;

        public TransactionBuilder withAmount(BigInteger amount)
        {
            this.amount = amount;
            return this;
        }

        public TransactionBuilder withMerchant(string merchant)
        {
            this.merchant = merchant;
            return this;
        }

        public TransactionBuilder withTime(DateTime time)
        {
            this.time = time;
            return this;
        }

        public OperationTransaction Build()
        {
            return new OperationTransaction()
            {
                Amount = amount,
                Merchant = merchant,
                Time = time
            };
        }
    }
}