using System.Collections.Generic;
using NubankAuthorizer.DBInterfaces;
using NubankAuthorizer.Models;
using NubankAuthorizer.Validators;
using NubankAuthorizer.Validators.AccountValidators;
using NubankAuthorizer.Validators.TransactionValidators;

namespace NubankAuthorizer.Controllers
{
    public class TransactionController : IOperationController
    {
        private readonly AccountController accountController;
        private readonly Validator validators;

        private readonly IDatabase<OperationTransaction> transactionsDatabase;

        public TransactionController(AccountController accountController, IDatabase<OperationTransaction> transactionsDatabase)
        {
            this.transactionsDatabase = transactionsDatabase;
            this.accountController = accountController;
            
            validators = new CardNotActiveValidationDecorator(null);
            validators = new InsufficientLimitValidationDecorator(validators);
            validators = new HighFrequencySmallIntervalValidationDecorator(validators, this);
            validators = new DoubleTransactionValidationDecorator(validators, this);
            validators = new AccountNotInitializedValidationDecorator(validators);
        }

        /// <summary>
        /// Return all transactions from database
        /// </summary>
        /// <returns>List with OperationTransactions</returns>
        public List<OperationTransaction> GetTransactions()
        {
            return transactionsDatabase.GetAll();
        }

        public Response ProcessOperation(Operations operation)
        {
            return AddTransaction(operation.Transaction);
        }
        
        /// <summary>
        /// Receive a transaction operation and validates it, if valid, add to the database
        /// </summary>
        /// <param name="transaction"></param>
        /// <returns>Response with or without violations</returns>
        public Response AddTransaction(OperationTransaction transaction)
        {
            Account account = accountController.GetAccount();
            
            List<Violations> violationsList = validators.Validation(transaction, account);
            
            if (violationsList.Count > 0)
            {
                return Response.Generate(account, violationsList);
            }

            account.AvailableLimit -= transaction.Amount;
            transactionsDatabase.Save(transaction);
            
            return Response.Generate(account, new List<Violations>());
        }
    }
}