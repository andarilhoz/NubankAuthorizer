using System.Collections.Generic;
using NubankAuthorizer.Models;

namespace NubankAuthorizer.Validators.TransactionValidators
{
    class InsufficientLimitDecorator: Decorator
    {
        public InsufficientLimitDecorator(Validator comp) : base(comp)
        {
        }

        public override List<Violations> Validation(OperationTransaction transaction, Account account)
        {
            List<Violations> baseViolations = base.Validation(transaction, account);
            
            if (account.AvailableLimit < transaction.Amount)
                baseViolations.Add(Violations.INSUFFICIENT_LIMIT);

            return baseViolations;
        }
    }
}