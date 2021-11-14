using System.Collections.Generic;
using System.Linq;
using Newtonsoft.Json;
using NubankAuthorizer.Models;

namespace NubankAuthorizer.Controllers
{
    public class InputController
    {
        /// <summary>
        /// Receive multiline string and converts to a List of operations
        /// </summary>
        /// <param name="multilineString">multiline string with commands</param>
        /// <returns>Operations List based on data input</returns>
        public List<Operations> ReceiveOperationsFromString(string multilineString)
        {
            List<string> lines = SliceOperations(multilineString);
            return ConvertToOperation(lines);
        }
        
        /// <summary>
        /// Receive multiline string and splits into string list
        /// </summary>
        /// <param name="multilineString">multiline string with commands</param>
        /// <returns>List with spliced lines as each item</returns>
        public static List<string> SliceOperations(string multilineString)
        {
            List<string> splicedLines = multilineString.Replace("\r", "").Split("\n").ToList();
            return splicedLines;
        }

        /// <summary>
        /// Convert list of strings to list of operations
        /// </summary>
        /// <param name="commandLines">List of command lines</param>
        /// <returns>List of operations</returns>
        public static List<Operations> ConvertToOperation(List<string> commandLines)
        {
            List<Operations> operations = new List<Operations>();
            foreach (string line in commandLines)
            {
                operations.Add(ReadSingleOperation(line));
            }
            return operations;
        }
        
        /// <summary>
        /// Convert single line string of operation into Operation
        /// </summary>
        /// <param name="commandLine">string with single line command</param>
        /// <returns>Operation</returns>
        public static Operations ReadSingleOperation(string commandLine)
        {
            return JsonConvert.DeserializeObject<Operations>(commandLine);
        }
    }
}