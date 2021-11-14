using System.Collections.Generic;
using System.IO;
using System.Reflection;
using NubankAuthorizer.Controllers;
using NubankAuthorizer.Models;
using NUnit.Framework;

namespace AuthorizerTests
{
    public class InputControllerTests
    {
        private InputController inputController;
        
        [SetUp]
        public void Setup()
        {
            inputController = new InputController();
        }

        [Test]
        public void ReadSingleOperationTest()
        {
            string fakeData = "{\"account\": {\"active-card\": true, \"available-limit\": 1000}}";
            
            Operations operation = inputController.ReadSingleOperation(fakeData);

            Account account = new Account()
            {
                ActiveCard = true,
                AvailableLimit = 1000
            };
            
            Assert.AreEqual(account, operation.Account);
            Assert.AreSame(null, operation.Transaction);
        }
        
        
        [Test]
        public void SliceLengthOperationsTest()
        {
            string fakeData = GetFileContents("operations");

            List<string> operations = inputController.SliceOperations(fakeData);

            Assert.AreEqual(4, operations.Count);
        }
        
        [Test]
        public void GetOperationsTest()
        {
            string fakeData = GetFileContents("operations");

            List<string> operationsString = inputController.SliceOperations(fakeData);
            List<Operations> operations = inputController.ConvertToOperation(operationsString);

            Account account = new Account()
            {
                ActiveCard = true,
                AvailableLimit = 100
            };

            OperationTransaction firstOperation = new TransactionBuilder()
                .withAmount(20)
                .withMerchant("Burger King")
                .withTime("2019-02-13T10:00:00.000Z")
                .Build();

            Assert.AreEqual(account, operations[0].Account);
            Assert.AreSame(null, operations[0].Transaction);
            Assert.AreEqual(firstOperation, operations[1].Transaction);
        }
        
        
        [Test]
        public void ReceiveOperationsFromStringTest()
        {
            string fakeData = GetFileContents("operations");
            List<Operations> operations = inputController.ReceiveOperationsFromString(fakeData);

            Account account = new Account()
            {
                ActiveCard = true,
                AvailableLimit = 100
            };

            OperationTransaction firstOperation = new TransactionBuilder()
                .withAmount(20)
                .withMerchant("Burger King")
                .withTime("2019-02-13T10:00:00.000Z")
                .Build();

            Assert.AreEqual(account, operations[0].Account);
            Assert.AreSame(null, operations[0].Transaction);
            Assert.AreEqual(firstOperation, operations[1].Transaction);
        }
        
        
        
        private string GetFileContents(string sampleFile)
        {
            var asm = Assembly.GetExecutingAssembly();
            var resource = string.Format("AuthorizerTests.Resources.{0}", sampleFile);
            string[] names = asm.GetManifestResourceNames();
            using (var stream = asm.GetManifestResourceStream(resource))
            {
                if (stream != null)
                {
                    var reader = new StreamReader(stream);
                    return reader.ReadToEnd();
                }
            }
            return string.Empty;
        }

    }
}