using System;
using System.Collections.Generic;
using System.Linq;
using NubankAuthorizer.Controllers;
using NubankAuthorizer.Models;

namespace NubankAuthorizer.Validators.TransactionValidators
{
    internal class HighFrequencySmallIntervalValidationDecorator: ValidationDecorator
    {
        private const int HighFrequencyLimitInMinutes = 2;
        private const int HighFrequencyLimit = 3;

        private readonly TransactionController transactionController;
        
        public HighFrequencySmallIntervalValidationDecorator(Validator comp, TransactionController transactionController) : base(comp)
        {
            this.transactionController = transactionController;
        }

        public override List<Violations> Validation(OperationTransaction transaction, Account account)
        {
            List<Violations> baseViolations = base.Validation(transaction, account);
            List<OperationTransaction> lastTransactions = transactionController.GetTransactions();
            
            IEnumerable<OperationTransaction> transactionsInLastPeriod =
                lastTransactions.Where(TransactionsInLastPeriod(transaction));

            if (transactionsInLastPeriod.Count() >= HighFrequencyLimit)
            {
                baseViolations.Add(Violations.HighFrequencySmallInterval);
            }
            
            return baseViolations;
        }

        private static Func<OperationTransaction, bool> TransactionsInLastPeriod(OperationTransaction transaction)
        {
            return oldTransaction =>
            {
                double minutesSinceTransaction = (transaction.Time - oldTransaction.Time).TotalMinutes;
                return minutesSinceTransaction < HighFrequencyLimitInMinutes;
            };
        }
    }
}