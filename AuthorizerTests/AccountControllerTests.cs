using System;
using System.Collections.Generic;
using NubankAuthorizer.Controllers;
using NubankAuthorizer.Models;
using NUnit.Framework;

namespace AuthorizerTests
{
    public class AccountControllerTests
    {
        private AccountController accountController;
        private Account testAccount;
        
        [SetUp]
        public void Setup()
        {
            accountController = new AccountController();
            testAccount = new Account()
            {
                ActiveCard = true,
                AvailableLimit = 100
            };
        }

        [Test]
        public void TestCreateAccount()
        {
            Response expectedResponse = new ResponseBuilder().withAccount(testAccount).Build();
            
            Response response = accountController.Create(testAccount);
            Assert.AreEqual(expectedResponse, response);
        }
        
        [Test]
        public void TestDoubleCreateAccount()
        {
            Response expectedResponse = new ResponseBuilder()
                .withAccount(testAccount)
                .withViolation(Violations.ACCOUNT_ALREADY_INITIALIZED).Build();

            accountController.Create(testAccount);
            
            Response secondResponse = accountController.Create(testAccount);
            Assert.AreEqual(expectedResponse, secondResponse);
        }
    }
}