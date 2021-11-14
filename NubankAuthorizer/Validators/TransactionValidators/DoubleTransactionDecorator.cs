using System;
using System.Collections.Generic;
using System.Linq;
using NubankAuthorizer.Controllers;
using NubankAuthorizer.Models;

namespace NubankAuthorizer.Validators.TransactionValidators
{
    internal class DoubleTransactionValidationDecorator: ValidationDecorator
    {
        private const int DoubleTransactionLimitInMinutes = 2;

        private readonly TransactionController transactionController;
        
        public DoubleTransactionValidationDecorator(Validator comp, TransactionController transactionController) : base(comp)
        {
            this.transactionController = transactionController;
        }

        public override List<Violations> Validation(OperationTransaction transaction, Account account)
        {
            List<Violations> baseViolations = base.Validation(transaction, account);
            List<OperationTransaction> lastTransactions = transactionController.GetTransactions();

            IEnumerable<OperationTransaction> doubledTransactions =
                lastTransactions.Where(IdenticalTransactionsInLastPeriod(transaction));

            if (doubledTransactions.Any())
            {
                baseViolations.Add(Violations.DoubleTransaction);
            }
            
            return baseViolations;
        }

        private static Func<OperationTransaction, bool> IdenticalTransactionsInLastPeriod(OperationTransaction transaction)
        {
            return oldTransaction =>
            {
                double minutesSinceTransaction = (transaction.Time - oldTransaction.Time).TotalMinutes;
                bool sameMerchant = oldTransaction.Merchant == transaction.Merchant;
                bool sameValue = oldTransaction.Amount == transaction.Amount;
                return minutesSinceTransaction < DoubleTransactionLimitInMinutes && sameMerchant && sameValue;
            };
        }
    }
}