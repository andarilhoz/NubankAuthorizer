using System;
using System.Collections.Generic;
using System.IO;
using NubankAuthorizer.Models;
using NubankAuthorizer.View;
using NUnit.Framework;

namespace AuthorizerTests
{
    [TestFixture]
    public class ConsoleViewTests
    {
        private ConsoleView consoleView;
        [SetUp]
        public void Setup()
        {
            consoleView = new ConsoleView();
        }

        [Test]
        public void TestConsoleOutput()
        {
            
            Response response = new Response()
            {
                Account = new Account(),
                Violations = new List<Violations>() { Violations.CARD_NOT_ACTIVE }
            };
            
            using (StringWriter sw = new StringWriter())
            {
                Console.SetOut(sw);

                consoleView.PrintOutput(new List<Response>(){ response});

                string expected = "{\"account\":{\"active-card\":false,\"available-limit\":0},\"violations\":[\"card-not-active\"]}\r\n";
                Assert.AreEqual(expected, sw.ToString());
            }
        }
    }
}