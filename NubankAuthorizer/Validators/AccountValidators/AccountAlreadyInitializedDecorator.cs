using System.Collections.Generic;
using NubankAuthorizer.Models;

namespace NubankAuthorizer.Validators.AccountValidators
{
    internal class AccountAlreadyInitializedValidationDecorator: ValidationDecorator
    {
        public AccountAlreadyInitializedValidationDecorator(Validator comp) : base(comp)
        {
        }
        
        public override List<Violations> Validation(OperationTransaction transaction, Account account)
        {
            List<Violations> baseViolations = base.Validation(transaction, account);
            
            if(account != null)
                baseViolations.Add(Violations.AccountAlreadyInitialized);
            
            return baseViolations;
        }
    }
}