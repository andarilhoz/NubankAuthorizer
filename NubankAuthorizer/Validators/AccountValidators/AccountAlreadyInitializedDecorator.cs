using System.Collections.Generic;
using NubankAuthorizer.Models;

namespace NubankAuthorizer.Validators.AccountValidators
{
    class AccountAlreadyInitializedDecorator: Decorator
    {
        public AccountAlreadyInitializedDecorator(Validator comp) : base(comp)
        {
        }

        public override List<Violations> Validation(OperationTransaction transaction, Models.Account account)
        {
            List<Violations> baseViolations = base.Validation(transaction, account);
            
            if(account != null)
                baseViolations.Add(Violations.ACCOUNT_ALREADY_INITIALIZED);
            
            return baseViolations;
        }
    }
}