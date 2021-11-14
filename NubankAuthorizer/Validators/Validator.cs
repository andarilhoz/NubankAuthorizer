using System.Collections.Generic;
using NubankAuthorizer.Models;

namespace NubankAuthorizer.Validators
{
    public abstract class Validator
    {
        public abstract List<Violations> Validation(OperationTransaction transaction, Account account);
    }

    internal abstract class ValidationDecorator : Validator
    {
        private readonly Validator validator;

        protected ValidationDecorator(Validator validator)
        {
            this.validator = validator;
        }

        public override List<Violations> Validation(OperationTransaction transaction, Account account)
        {
            if (validator != null)
            {
                return validator.Validation(transaction, account);
            }
            
            return new List<Violations>();
        }
    }
}