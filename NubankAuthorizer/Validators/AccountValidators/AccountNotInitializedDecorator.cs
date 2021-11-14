using System.Collections.Generic;
using NubankAuthorizer.Models;

namespace NubankAuthorizer.Validators.AccountValidators
{
    class AccountNotInitializedDecorator : Decorator
    {
        public AccountNotInitializedDecorator(Validator comp) : base(comp)
        {
        }

        public override List<Violations> Validation(OperationTransaction transaction, Account account)
        {
            List<Violations> baseViolations = new List<Violations>();
            
            if(account == null)
                baseViolations.Add(Violations.ACCOUNT_NOT_INITIALIZED);
            else
                return base.Validation(transaction, account);
            
            return baseViolations;
        }
    }
}