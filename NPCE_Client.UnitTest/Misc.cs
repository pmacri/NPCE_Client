using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.IO;
using System.Text;
using System.Text.RegularExpressions;

namespace NPCE_Client.UnitTest
{
    [TestClass]
    public class Misc
    {
        [TestMethod]
        public void LoggingFile()
        {
            string fileName = @"\\10.60.19.20\c$\\NPCE V6\Logging\NPCEROL.log";

            var fs = new FileStream(fileName, FileMode.Open, FileAccess.Read);
            var sr = new StreamReader(fs, Encoding.UTF8);
            
            string[] lines = File.ReadAllLines(@"\\10.60.19.20\c$\\NPCE V6\Logging\NPCEROL.log");

            string line = String.Empty;

            string lastLineFound = string.Empty;

            while ((line = sr.ReadLine()) != null)
            {

                if (Regex.IsMatch(line, $@"\bARCHIV\b"))
                {
                    lastLineFound = line;
                }

                //<Parameter ParamName="ARCHIV" ParamValue="00020024" />
            }

            var paramValue = lastLineFound.Substring(lastLineFound.LastIndexOf("ParamValue") + 12, 8);

            var numeroFogli = int.Parse(paramValue.Substring(0, 4));
            var mesiArchiviazione = int.Parse(paramValue.Substring(4, 4));
            Console.WriteLine(lastLineFound);
            sr.Dispose();
            fs.Dispose();
        }
    }
}
