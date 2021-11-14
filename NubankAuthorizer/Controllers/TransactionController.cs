using System.Collections.Generic;
using NubankAuthorizer.Models;
using NubankAuthorizer.Validators;
using NubankAuthorizer.Validators.AccountValidators;
using NubankAuthorizer.Validators.TransactionValidators;

namespace NubankAuthorizer.Controllers
{
    public class TransactionController
    {
        private AccountController accountController;
        private List<OperationTransaction> transactions = new List<OperationTransaction>();
        
        private Validator validators;
        
        public TransactionController(AccountController accountController)
        {
            this.accountController = accountController;
            
            validators = new CardNotActiveDecorator(null);
            validators = new InsufficientLimitDecorator(validators);
            validators = new HighFrequencySmallIntervalDecorator(validators, this);
            validators = new DoubleTransactionDecorator(validators, this);
            validators = new AccountNotInitializedDecorator(validators);
        }

        public List<OperationTransaction> GetTransactions()
        {
            return transactions;
        }
        
        public Response AddTransaction(OperationTransaction transaction)
        {
            Account account = accountController.GetAccount();
            
            List<Violations> violationsList = validators.Validation(transaction, account);
            
            if (violationsList.Count > 0)
            {
                return Response.Generate(account, violationsList);
            }

            account.AvailableLimit -= transaction.Amount;
            transactions.Add(transaction);
            
            return Response.Generate(account, new List<Violations>());
        }
    }
}