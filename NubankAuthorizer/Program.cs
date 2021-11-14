using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Text;
using NubankAuthorizer.Controllers;
using NubankAuthorizer.Models;
using NubankAuthorizer.View;

namespace NubankAuthorizer
{
    class Program
    {
        static void Main(string[] args)
        {
            InputController inputController = new InputController();
            AccountController accountController = new AccountController();
            TransactionController transactionController = new TransactionController(accountController);
            ConsoleView consoleView = new ConsoleView();
            
            string line;

            List<string> lines = new List<string>();

            while ((line = Console.ReadLine()) != null)
            {
                lines.Add(line);
            }
            
            List<Operations> operations = inputController.ConvertToOperation(lines);
            List<Response> responses = new List<Response>();
            
            foreach (Operations operation in operations)
            {
                if (operation.Account != null)
                {
                    responses.Add(accountController.Create(operation.Account));
                }else if (operation.Transaction != null)
                {
                    responses.Add(transactionController.AddTransaction(operation.Transaction));
                }
            }
            consoleView.PrintOutput(responses);
        }
    }
}