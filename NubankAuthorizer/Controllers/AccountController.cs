using System.Collections.Generic;
using NubankAuthorizer.DBInterfaces;
using NubankAuthorizer.Models;
using NubankAuthorizer.Validators;
using NubankAuthorizer.Validators.AccountValidators;

namespace NubankAuthorizer.Controllers
{
    public class AccountController : IOperationController
    {
        private IDatabase<Account> accountDatabase;
        private readonly Validator validators;

        public AccountController(IDatabase<Account> database)
        {
            accountDatabase = database;
            validators = new AccountAlreadyInitializedValidationDecorator(null);
        }

        /// <summary>
        /// Will return the first account it found on the database
        /// </summary>
        /// <returns>Account: the saved account instance</returns>
        public Account GetAccount()
        {
            return accountDatabase.FindOne();
        }
        
        public Response ProcessOperation(Operations operation)
        {
            return Create(operation.Account);
        }
        
        /// <summary>
        /// Saves the account in the database if doesnt exists
        /// </summary>
        /// <param name="accountData">account instance</param>
        /// <returns>Response with or without violations</returns>
        public Response Create(Account accountData)
        {
            Account currentAccount = accountDatabase.FindOne();
            
            List<Violations> violationsList = validators.Validation(null, currentAccount);
            if (violationsList.Count > 0)
            {
                return Response.Generate(currentAccount, violationsList);
            }
            
            accountDatabase.Save(accountData);

            Response response = Response.Generate(accountData, new List<Violations>());
            return response;
        }
    }
}