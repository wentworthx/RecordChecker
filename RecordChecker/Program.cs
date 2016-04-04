using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Xml;
using Aspose.Cells;

namespace RecordChecker
{
    class Program
    {
        static string fileName;

        static void Main(string[] args)
        {
            try
            {
                if (args.Length == 1)
                {
                    fileName = args[0];
                    if (!File.Exists(fileName))
                    {
                        Console.WriteLine(string.Format("File '{0}' not found.", fileName));
                        return;
                    }
                    using (RecordChecker checker = new RecordChecker(fileName))
                    {
                        checker.Check();
                    }
                }
                else
                {
                    Console.WriteLine("Parameters are not correct.");
                }
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
            }
        }
    }
}
