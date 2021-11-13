using System;
using System.Collections.Generic;
using System.Linq;
using NubankAuthorizer.Models;

namespace NubankAuthorizer.Controllers
{
    public class TransactionController
    {
        private AccountController accountController;
        private List<OperationTransaction> transactions = new List<OperationTransaction>();

        private const int HighFrequencyLimitInMinutes = 2;
        private const int HighFrequencyLimit = 3;

        private const int DoubleTransactionLimitInMinutes = 2;
        public TransactionController(AccountController accountController)
        {
            this.accountController = accountController;
        }
        
        public Response AddTransaction(OperationTransaction transaction)
        {
            Account account = accountController.GetAccount();
            
            List<Violations> violationsList = CheckForViolations(transaction, account);
            if (violationsList.Count > 0)
            {
                return Response.Generate(account, violationsList);
            }

            account.AvailableLimit -= transaction.Amount;
            transactions.Add(transaction);
            
            return Response.Generate(account, new List<Violations>());
        }

        private List<Violations> CheckForViolations(OperationTransaction transaction, Account account)
        {
            List<Violations> violationsList = new List<Violations>();
            List<Violations> accountViolations = AccountValidation(transaction, account);
            List<Violations> transactionViolations = TransactionValidation(transaction);

            violationsList.AddRange(accountViolations);
            violationsList.AddRange(transactionViolations);
            return violationsList;
        }
        
        private static List<Violations> AccountValidation(OperationTransaction transaction, Account account)
        {
            List<Violations> violationsList = new List<Violations>();
            
            if (account == null)
            { 
                violationsList.Add(Violations.ACCOUNT_NOT_INITIALIZED);
                return violationsList;
            }
            
            if (!account.ActiveCard)
            {
                violationsList.Add(Violations.CARD_NOT_ACTIVE);
            }

            if (account.AvailableLimit < transaction.Amount)
            {
                violationsList.Add(Violations.INSUFFICIENT_LIMIT);
            }

            return violationsList;
        }

        private List<Violations> TransactionValidation(OperationTransaction transaction)
        {
            List<Violations> violationsList = new List<Violations>();
            
            IEnumerable<OperationTransaction> lastOperationsHighFrequency =
                transactions.Where(t => (transaction.Time - t.Time).TotalMinutes < HighFrequencyLimitInMinutes);

            if (lastOperationsHighFrequency.Count() >= HighFrequencyLimit)
            {
                violationsList.Add(Violations.HIGH_FREQUENCY_SMALL_INTERVAL);
            }

            IEnumerable<OperationTransaction> sameOperationsDoubledTransaction = transactions.Where(t =>
                (transaction.Time - t.Time).TotalMinutes < DoubleTransactionLimitInMinutes && transaction.Amount == t.Amount &&
                t.Merchant == transaction.Merchant);
            
            if (sameOperationsDoubledTransaction.Any())
            {
                violationsList.Add(Violations.DOUBLE_TRANSACTION);
            }

            return violationsList;
        }
    }
}