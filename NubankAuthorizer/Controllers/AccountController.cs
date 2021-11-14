using System;
using System.Collections.Generic;
using NubankAuthorizer.Models;
using NubankAuthorizer.Validators;
using NubankAuthorizer.Validators.AccountValidators;

namespace NubankAuthorizer.Controllers
{
    public class AccountController
    {
        private Account currentAccount;
        private Validator validators;

        public AccountController()
        {
            validators = new AccountAlreadyInitializedDecorator(null);
        }

        public Account GetAccount()
        {
            return currentAccount;
        }
        
        public Response Create(Account accountData)
        {
            List<Violations> violationsList = validators.Validation(null, currentAccount);
            if (violationsList.Count > 0)
            {
                return Response.Generate(currentAccount, violationsList);
            }

            currentAccount = accountData;

            Response response = Response.Generate(accountData, new List<Violations>());
            return response;
        }
    }
}