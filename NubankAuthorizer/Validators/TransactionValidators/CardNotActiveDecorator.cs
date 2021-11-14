using System.Collections.Generic;
using NubankAuthorizer.Models;

namespace NubankAuthorizer.Validators.TransactionValidators
{
    internal class CardNotActiveValidationDecorator: ValidationDecorator
    {
        public CardNotActiveValidationDecorator(Validator comp) : base(comp)
        {
        }

        public override List<Violations> Validation(OperationTransaction transaction, Account account)
        {
            List<Violations> baseViolations = base.Validation(transaction, account);
            
            if (!account.ActiveCard)
                baseViolations.Add(Violations.CardNotActive);
            
            return baseViolations;
        }
    }
}