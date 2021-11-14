using System.Collections.Generic;
using NubankAuthorizer.Controllers;
using NubankAuthorizer.Models;
using NUnit.Framework;

namespace AuthorizerTests
{
    public class TransactionControllerTests
    {
        private TransactionController transactionController;
        private AccountController accountController;
        
        private Account testAccount;
        private Account notActiveAccount;
        
        [SetUp]
        public void Setup()
        {
            accountController = new AccountController();
            transactionController = new TransactionController(accountController);
            testAccount = new Account()
            {
                ActiveCard = true,
                AvailableLimit = 10000
            };
            
            notActiveAccount = new Account()
            {
                ActiveCard = false,
                AvailableLimit = 1000
            };
        }

        [Test]
        public void SuccessTransactionTest()
        {
            accountController.Create(testAccount);

            OperationTransaction testTransaction = new TransactionBuilder()
                    .withAmount(20)
                    .withMerchant("Burger King")
                    .Build();

            int finalLimit = testAccount.AvailableLimit - testTransaction.Amount;

            Response expectedResponse = new ResponseBuilder().withAccount(testAccount).Build();
            
            Response response = transactionController.AddTransaction(testTransaction);
            
            Assert.AreEqual(expectedResponse, response);
            Assert.AreEqual(finalLimit, response.Account.AvailableLimit);
        }

        [Test]
        public void AccountNotInitializedTest()
        {
            OperationTransaction testTransaction = new TransactionBuilder()
                .withAmount(20)
                .withMerchant("Burger King")
                .Build();

            Response expectedResponse = new ResponseBuilder().withViolation(Violations.ACCOUNT_NOT_INITIALIZED).Build();

            Response response = transactionController.AddTransaction(testTransaction);
            
            Assert.AreEqual(expectedResponse, response);
        }
        
        [Test]
        public void CardNotActiveTest()
        {
            accountController.Create(notActiveAccount);

            OperationTransaction testTransaction = new TransactionBuilder()
                .withAmount(20)
                .withMerchant("Burger King")
                .Build();
            
            int expectedLimit = notActiveAccount.AvailableLimit;

            Response expectedResponse = new ResponseBuilder()
                .withAccount(notActiveAccount)
                .withViolation(Violations.CARD_NOT_ACTIVE)
                .Build();

            Response response = transactionController.AddTransaction(testTransaction);
            
            Assert.AreEqual(expectedResponse, response);
            Assert.AreEqual(expectedLimit, response.Account.AvailableLimit);
        }
        
        [Test]
        public void InsufficientLimitTest()
        {
            accountController.Create(testAccount);

            OperationTransaction testTransaction =new TransactionBuilder()
                .withAmount(20000)
                .withMerchant("Mercedes")
                .Build();

            int expectedLimit = testAccount.AvailableLimit;

            Response expectedResponse = new ResponseBuilder()
                .withAccount(testAccount)
                .withViolation(Violations.INSUFFICIENT_LIMIT)
                .Build();

            Response response = transactionController.AddTransaction(testTransaction);
            
            Assert.AreEqual(expectedResponse, response);
            Assert.AreEqual(expectedLimit, response.Account.AvailableLimit);
        }
        
        [Test]
        public void HighFrequencySmallIntervalTest()
        {
            accountController.Create(testAccount);

            OperationTransaction testTransaction1 = new TransactionBuilder()
                .withAmount(20)
                .withMerchant("Burger King")
                .withTime("2019-02-13T11:00:00.000Z")
                .Build();

            OperationTransaction testTransaction2 = new TransactionBuilder()
                .withAmount(10)
                .withMerchant("Habbib's") 
                .withTime("2019-02-13T11:00:01.000Z")
                .Build();

            OperationTransaction testTransaction3 = new TransactionBuilder()
                .withAmount(5)
                .withMerchant("McDonald's")
                .withTime("2019-02-13T11:01:01.000Z")
                .Build();
            
            OperationTransaction testTransaction4 = new TransactionBuilder()
                .withAmount(20)
                .withMerchant("Subway")
                .withTime("2019-02-13T11:01:31.000Z")
                .Build();
            
            OperationTransaction testTransaction5 = new TransactionBuilder()
                .withAmount(20)
                .withMerchant("Burger King")
                .withTime("2019-02-13T12:00:00.000Z")
                .Build();
            
            Response expectedResponse = new ResponseBuilder()
                .withAccount(testAccount)
                .withViolation(Violations.HIGH_FREQUENCY_SMALL_INTERVAL)
                .Build();

            Response successResponse = new ResponseBuilder().withAccount(testAccount).Build();

            int expectedLimit = testAccount.AvailableLimit - testTransaction1.Amount - testTransaction2.Amount -
                                       testTransaction3.Amount - testTransaction5.Amount;

            Response response1 = transactionController.AddTransaction(testTransaction1);
            Response response2 = transactionController.AddTransaction(testTransaction2);
            Response response3 = transactionController.AddTransaction(testTransaction3);
            Response response4 = transactionController.AddTransaction(testTransaction4);
            Response response5 = transactionController.AddTransaction(testTransaction5);
            
            Assert.AreEqual(successResponse, response1);
            Assert.AreEqual(successResponse, response2);
            Assert.AreEqual(successResponse, response3);
            Assert.AreEqual(successResponse, response5);
            
            Assert.AreEqual(expectedResponse, response4);
            Assert.AreEqual(expectedLimit, response4.Account.AvailableLimit);
        }
        
        [Test]
        public void DoubleTransactionTest()
        {
            accountController.Create(testAccount);
            
            OperationTransaction testTransaction1 = new TransactionBuilder()
                .withAmount(20)
                .withMerchant("Burger King")
                .withTime("2019-02-13T11:00:00.000Z")
                .Build();

            OperationTransaction testTransaction2 = new TransactionBuilder()
                .withAmount(10)
                .withMerchant("McDonald's")
                .withTime("2019-02-13T11:00:01.000Z")
                .Build();
            
            OperationTransaction testTransaction3 = new TransactionBuilder()
                .withAmount(20)
                .withMerchant("Burger King")
                .withTime("2019-02-13T11:00:02.000Z")
                .Build();
            
            OperationTransaction testTransaction4 = new TransactionBuilder()
                .withAmount(15)
                .withMerchant("Burger King")
                .withTime("2019-02-13T11:00:03.000Z")
                .Build();
            
            Response expectedResponse = new ResponseBuilder()
                .withAccount(testAccount)
                .withViolation(Violations.DOUBLE_TRANSACTION)
                .Build();

            Response successResponse = new Response()
            {
                Account = testAccount,
                Violations = new List<Violations>()
            };

            int expectedLimit = testAccount.AvailableLimit - testTransaction1.Amount - testTransaction2.Amount -testTransaction4.Amount;
            
            Response response1 = transactionController.AddTransaction(testTransaction1);
            Response response2 = transactionController.AddTransaction(testTransaction2);
            Response response3 = transactionController.AddTransaction(testTransaction3);
            Response response4 = transactionController.AddTransaction(testTransaction4);
            
            Assert.AreEqual(successResponse, response1);
            Assert.AreEqual(successResponse, response2);
            Assert.AreEqual(expectedResponse, response3);
            Assert.AreEqual(successResponse, response4);

            Assert.AreEqual(expectedLimit, response4.Account.AvailableLimit);
        }
        
        [Test]
        public void MultipleViolationsTest()
        {
            Account multipleViolationsAccount = new Account()
            {
                ActiveCard = true,
                AvailableLimit = 100
            };
            
            accountController.Create(multipleViolationsAccount);


            OperationTransaction testTransaction1 = new TransactionBuilder()
                .withAmount(10)
                .withMerchant("McDonald's")
                .withTime("2019-02-13T11:00:01.000Z")
                .Build();
            
            OperationTransaction testTransaction2 = new TransactionBuilder()
                .withAmount(20)
                .withMerchant("Burger King")
                .withTime("2019-02-13T11:00:02.000Z")
                .Build();
            
            OperationTransaction testTransaction3 = new TransactionBuilder()
                .withAmount(5)
                .withMerchant("Burger King")
                .withTime("2019-02-13T11:00:07.000Z")
                .Build();

            OperationTransaction testTransaction4 = new TransactionBuilder()
                .withAmount(5)
                .withMerchant("Burger King")
                .withTime("2019-02-13T11:00:08.000Z")
                .Build();
            
            OperationTransaction testTransaction5 = new TransactionBuilder()
                .withAmount(150)
                .withMerchant("Burger King")
                .withTime("2019-02-13T11:00:18.000Z")
                .Build();
            
            OperationTransaction testTransaction6 = new TransactionBuilder()
                .withAmount(190)
                .withMerchant("Burger King")
                .withTime("2019-02-13T11:00:22.000Z")
                .Build();
            
            OperationTransaction testTransaction7 = new TransactionBuilder()
                .withAmount(15)
                .withMerchant("Burger King")
                .withTime("2019-02-13T12:00:27.000Z")
                .Build();
            
            Response expectedResponse4 = new ResponseBuilder()
                .withAccount(multipleViolationsAccount)
                .withViolation(Violations.HIGH_FREQUENCY_SMALL_INTERVAL)
                .withViolation(Violations.DOUBLE_TRANSACTION)
                .Build();

            Response expectedResponse5And6 = new ResponseBuilder()
                .withAccount(multipleViolationsAccount)
                .withViolation(Violations.INSUFFICIENT_LIMIT)
                .withViolation(Violations.HIGH_FREQUENCY_SMALL_INTERVAL)
                .Build();

            Response successResponse = new ResponseBuilder().withAccount(multipleViolationsAccount).Build();

            int expectedLimit = multipleViolationsAccount.AvailableLimit - testTransaction1.Amount - testTransaction2.Amount -testTransaction4.Amount - testTransaction7.Amount;
            
            Response response1 = transactionController.AddTransaction(testTransaction1);
            Response response2 = transactionController.AddTransaction(testTransaction2);
            Response response3 = transactionController.AddTransaction(testTransaction3);
            Response response4 = transactionController.AddTransaction(testTransaction4);
            Response response5 = transactionController.AddTransaction(testTransaction5);
            Response response6 = transactionController.AddTransaction(testTransaction6);
            Response response7 = transactionController.AddTransaction(testTransaction7);
            
            Assert.AreEqual(successResponse, response1);
            Assert.AreEqual(successResponse, response2);
            Assert.AreEqual(successResponse, response3);
            Assert.AreEqual(expectedResponse4, response4);
            Assert.AreEqual(expectedResponse5And6, response5);
            Assert.AreEqual(expectedResponse5And6, response6);
            Assert.AreEqual(successResponse, response7);

            Assert.AreEqual(expectedLimit, response4.Account.AvailableLimit);
        }
    }
}