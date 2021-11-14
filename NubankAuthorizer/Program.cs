using System;
using System.Collections.Generic;
using NubankAuthorizer.Controllers;
using NubankAuthorizer.DBInterfaces;
using NubankAuthorizer.Models;
using NubankAuthorizer.View;

namespace NubankAuthorizer
{
    class Program
    {
        static void Main(string[] args)
        {
            MemoryDatabase<Account> memoryAccountDatabase = new MemoryDatabase<Account>();
            MemoryDatabase<OperationTransaction> memoryTransactionsDatabase = new MemoryDatabase<OperationTransaction>();
            
            InputController inputController = new InputController();
            AccountController accountController = new AccountController(memoryAccountDatabase);
            TransactionController transactionController = new TransactionController(accountController, memoryTransactionsDatabase);

            List<string> lines = new List<string>();
            
            string line;
            while ((line = Console.ReadLine()) != null)
            {
                lines.Add(line);
            }
            
            List<Operations> operations = InputController.ConvertToOperation(lines);
            List<Response> responses = new List<Response>();
            
            foreach (Operations operation in operations)
            {
                if (operation.Account != null)
                {
                    responses.Add(accountController.ProcessOperation(operation));
                }else if (operation.Transaction != null)
                {
                    responses.Add(transactionController.ProcessOperation(operation));
                }
            }
            
            ConsoleView.PrintOutput(responses);
        }
    }
}