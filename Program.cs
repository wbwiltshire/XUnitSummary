using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using System.Xml;
using System.Xml.Linq;
using System.Xml.XPath;

namespace XUnitSummary
{
    class Program
    {
        const string Version = "0.9.0";
        public static void Main(string[] args)
        {
            XDocument doc;
            IEnumerable<XElement> testCases;
            string results = String.Empty;
            CommandLineParameters clp = new CommandLineParameters() { Arguments = 0, FileName = String.Empty, Version = false };
            int total = 0, failed = 0, passed = 0, errors = 0, skipped = 0;

            try
            {
                if (ProcessCommandLine(args, clp))
                {
                    Console.WriteLine("XUnit Test Summary");
                    doc = XDocument.Load(clp.FileName);
                    testCases = doc.XPathSelectElements("descendant::test");
                    if (testCases != null)
                    {
                        foreach (XElement testCase in testCases)
                        {
                            Console.Write($"\t{testCase.Attribute("method").Value} : ");

                            switch(testCase.Attribute("result").Value.ToUpper())
                            {
                                case "PASS":
                                    Console.ForegroundColor = ConsoleColor.Green;
                                    passed++;
                                    break;
                                case "FAIL":
                                    Console.ForegroundColor = ConsoleColor.Red;
                                    failed++;
                                    break;
                                case "ERROR":
                                    Console.ForegroundColor = ConsoleColor.Red;
                                    errors++;
                                    break;
                                case "SKIPPED":
                                    Console.ForegroundColor = ConsoleColor.Yellow;
                                    skipped++;
                                    break;
                            }
                            Console.Write($"{testCase.Attribute("result").Value}");
                            Console.ResetColor();
                            Console.WriteLine($", Duration={testCase.Attribute("time").Value}(sec)");
                            total++;
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Exception: {ex.Message}");
            }

            Console.WriteLine();
            Console.WriteLine($"Total: {total}, Passed: {passed}, Failed: {failed}, Errors: {errors}, Skipped: {skipped}");
        }

        private static bool ProcessCommandLine(string[] args, CommandLineParameters clp)
        {
            bool fileExists = false;

            //Capture number of arguments
            clp.Arguments = args.Count();

            foreach (string arg in args)
            {
                if (arg.ToUpper().StartsWith("-V"))
                    clp.Version = true;
                else
                {
                    clp.FileName = arg;
                    if (File.Exists(arg))
                    {
                        fileExists = true;
                    }
                }
            }

            //If no filename was provided, but version was requested then print version
            //Else, If a filename was provided, but doesn't exist then print usage
            //Otherwise (i.e. a valid filename was provided), skip printing usage or version and just process the filename provided
            if (clp.Version && (clp.FileName == String.Empty))
                Console.WriteLine($"XUnitSummary v.{Version}");
            else if (!fileExists)
                Console.WriteLine("Usage: XUnitSummary filename.xml <-v>");

            return fileExists;
        }
    }
}
