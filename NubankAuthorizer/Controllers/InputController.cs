using System.Collections.Generic;
using System.Linq;
using Newtonsoft.Json;
using NubankAuthorizer.Models;

namespace NubankAuthorizer.Controllers
{
    public class InputController
    {
        public List<Operations> ReceiveOperationsFromString(string data)
        {
            List<string> lines = SliceOperations(data);
            return ConvertToOperation(lines);
        }
        
        public List<string> SliceOperations(string data)
        {
            List<string> splicedLines = data.Replace("\r", "").Split("\n").ToList();
            return splicedLines;
        }

        public List<Operations> ConvertToOperation(List<string> lines)
        {
            List<Operations> operations = new List<Operations>();
            foreach (string line in lines)
            {
                operations.Add(ReadSingleOperation(line));
            }
            return operations;
        }
        
        public Operations ReadSingleOperation(string data)
        {
            return JsonConvert.DeserializeObject<Operations>(data);
        }
    }
}