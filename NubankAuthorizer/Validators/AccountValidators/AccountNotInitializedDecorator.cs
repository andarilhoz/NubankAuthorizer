using System.Collections.Generic;
using NubankAuthorizer.Models;

namespace NubankAuthorizer.Validators.AccountValidators
{
    internal class AccountNotInitializedValidationDecorator : ValidationDecorator
    {
        public AccountNotInitializedValidationDecorator(Validator comp) : base(comp)
        {
        }

        public override List<Violations> Validation(OperationTransaction transaction, Account account)
        {
            List<Violations> baseViolations = new List<Violations>();
            
            if(account == null)
                baseViolations.Add(Violations.AccountNotInitialized);
            else
                return base.Validation(transaction, account);
            
            return baseViolations;
        }
    }
}