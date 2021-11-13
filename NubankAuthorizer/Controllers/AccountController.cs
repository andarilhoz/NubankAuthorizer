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
                return new Response()
                {
                    Account = currentAccount,
                    Violations = new List<Violations>() { Violations.ACCOUNT_ALREADY_INITIALIZED }
                };
            }

            currentAccount = accountData;
            
            Response response = new Response()
            {
                Account = accountData,
                Violations = new List<Violations>()
            };
            
            return response;
        }
    }
}