using System;

namespace NubankAuthorizer.Models
{
    public class TransactionBuilder
    {
        private int amount;
        private string merchant;
        private DateTime time = DateTime.Now;

        public TransactionBuilder withAmount(int amount)
        {
            this.amount = amount;
            return this;
        }

        public TransactionBuilder withMerchant(string merchant)
        {
            this.merchant = merchant;
            return this;
        }

        public TransactionBuilder withTime(string time)
        {
            this.time = DateTime.Parse(time, null, System.Globalization.DateTimeStyles.AdjustToUniversal);
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