using System.Collections.Generic;
using System.Linq;
using NubankAuthorizer.Controllers;
using NubankAuthorizer.Models;

namespace NubankAuthorizer.Validators.TransactionValidators
{
    class DoubleTransactionDecorator: Decorator
    {
        private const int DoubleTransactionLimitInMinutes = 2;

        private readonly TransactionController transactionController;
        
        public DoubleTransactionDecorator(Validator comp, TransactionController transactionController) : base(comp)
        {
            this.transactionController = transactionController;
        }

        public override List<Violations> Validation(OperationTransaction transaction, Account account)
        {
            List<Violations> baseViolations = base.Validation(transaction, account);
            
            IEnumerable<OperationTransaction> sameOperationsDoubledTransaction = transactionController.GetTransactions().Where(t =>
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