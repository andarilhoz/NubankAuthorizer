using System.Collections.Generic;
using NubankAuthorizer.Models;

namespace NubankAuthorizer.Validators.TransactionValidators
{
    class CardNotActiveDecorator: Decorator
    {
        public CardNotActiveDecorator(Validator comp) : base(comp)
        {
        }

        public override List<Violations> Validation(OperationTransaction transaction, Account account)
        {
            List<Violations> baseViolations = base.Validation(transaction, account);
            
            if (!account.ActiveCard)
                baseViolations.Add(Violations.CARD_NOT_ACTIVE);
            
            return baseViolations;
        }
    }
}