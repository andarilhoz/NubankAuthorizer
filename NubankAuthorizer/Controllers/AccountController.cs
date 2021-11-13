using System;
using System.Collections.Generic;
using NubankAuthorizer.Models;

namespace NubankAuthorizer.Controllers
{
    public class AccountController
    {
        private Account currentAccount;

        public Account GetAccount()
        {
            return currentAccount;
        }
        
        public Response Create(Account accountData)
        {
            if (currentAccount != null)
            {
                return Response.Generate(currentAccount, Violations.ACCOUNT_ALREADY_INITIALIZED);
            }

            currentAccount = accountData;

            Response response = Response.Generate(accountData, new List<Violations>());
            return response;
        }
    }
}