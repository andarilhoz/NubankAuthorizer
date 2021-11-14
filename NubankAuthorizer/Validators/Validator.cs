using System.Collections.Generic;
using NubankAuthorizer.Models;

namespace NubankAuthorizer.Validators
{
    public abstract class Validator
    {
        public abstract List<Violations> Validation(OperationTransaction transaction, Account account);
    }

    abstract class Decorator : Validator
    {
        protected Validator Validator;

        public Decorator(Validator validator)
        {
            Validator = validator;
        }

        public override List<Violations> Validation(OperationTransaction transaction, Account account)
        {
            if (Validator != null)
            {
                return Validator.Validation(transaction, account);
            }
            
            return new List<Violations>();
        }
    }
}