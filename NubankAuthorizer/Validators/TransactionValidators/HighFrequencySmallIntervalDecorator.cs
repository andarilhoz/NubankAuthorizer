using System.Collections.Generic;
using System.Linq;
using NubankAuthorizer.Controllers;
using NubankAuthorizer.Models;

namespace NubankAuthorizer.Validators.TransactionValidators
{
    class HighFrequencySmallIntervalDecorator: Decorator
    {
        private const int HighFrequencyLimitInMinutes = 2;
        private const int HighFrequencyLimit = 3;

        private readonly TransactionController transactionController;
        
        public HighFrequencySmallIntervalDecorator(Validator comp, TransactionController transactionController) : base(comp)
        {
            this.transactionController = transactionController;
        }

        public override List<Violations> Validation(OperationTransaction transaction, Account account)
        {
            List<Violations> baseViolations = base.Validation(transaction, account);
            
            IEnumerable<OperationTransaction> lastOperationsHighFrequency =
                transactionController.GetTransactions().Where(t => (transaction.Time - t.Time).TotalMinutes < HighFrequencyLimitInMinutes);

            if (lastOperationsHighFrequency.Count() >= HighFrequencyLimit)
            {
                baseViolations.Add(Violations.HIGH_FREQUENCY_SMALL_INTERVAL);
            }
            
            return baseViolations;
        }
    }
}