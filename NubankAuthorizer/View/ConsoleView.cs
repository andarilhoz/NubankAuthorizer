using System;
using System.Collections.Generic;
using Newtonsoft.Json;
using NubankAuthorizer.Models;

namespace NubankAuthorizer.View
{
    public static class ConsoleView
    {
        public static void PrintOutput(List<Response> responses)
        {
            foreach (Response response in responses)
            {
                string responseString = JsonConvert.SerializeObject(response);
                Console.WriteLine(responseString);
            }
        }
    }
}