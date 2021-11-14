using AuthorizerTests.Builders;
using NubankAuthorizer.Controllers;
using NubankAuthorizer.DBInterfaces;
using NubankAuthorizer.Models;
using NUnit.Framework;

namespace AuthorizerTests
{
    public class AccountControllerTests
    {
        private AccountController accountController;
        private MemoryDatabase<Account> memoryAccountDatabase;
        private Account testAccount;
        
        [SetUp]
        public void Setup()
        {
            memoryAccountDatabase = new MemoryDatabase<Account>();
            
            accountController = new AccountController(memoryAccountDatabase);
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
                .withViolation(Violations.AccountAlreadyInitialized).Build();

            accountController.Create(testAccount);
            
            Response secondResponse = accountController.Create(testAccount);
            Assert.AreEqual(expectedResponse, secondResponse);
        }
    }
}