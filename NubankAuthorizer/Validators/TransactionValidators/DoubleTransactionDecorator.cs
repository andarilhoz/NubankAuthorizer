using System.Collections.Generic;
using System.Linq;
using NubankAuthorizer.Models;

namespace NubankAuthorizer.Validators.TransactionValidators
{
    class DoubleTransactionDecorator: Decorator
    {
        private const int DoubleTransactionLimitInMinutes = 2;

        private readonly List<OperationTransaction> transactions;
        
        public DoubleTransactionDecorator(Validator comp, List<OperationTransaction> transactions) : base(comp)
        {
            this.transactions = transactions;
        }

        public override List<Violations> Validation(OperationTransaction transaction, Account account)
        {
            List<Violations> baseViolations = base.Validation(transaction, account);
            
            IEnumerable<OperationTransaction> sameOperationsDoubledTransaction = transactions.Where(t =>
                (transaction.Time - t.Time).TotalMinutes < DoubleTransactionLimitInMinutes && transaction.Amount == t.Amount &&
                t.Merchant == transaction.Merchant);
            
            if (sameOperationsDoubledTransaction.Any())
            {
                baseViolations.Add(Violations.DOUBLE_TRANSACTION);
            }
            
            return baseViolations;
        }
    }
}