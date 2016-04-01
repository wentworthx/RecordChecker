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
        static string currentDirectory = System.AppDomain.CurrentDomain.SetupInformation.ApplicationBase;

        static Dictionary<string, PersonRecord> Persons = new Dictionary<string, PersonRecord>();
        static List<PersonResult> CheckResults = new List<PersonResult>();

        public static bool ConsiderElastic = true;
        public static Dictionary<string, StandardRecord> StandardRecords = new Dictionary<string, StandardRecord>();
        public static string UnpaidLeaveTime;

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
                }

                /*
                 * Load File
                 */
                GetRecords(fileName);

                /*
                 * Check correct
                 */
                CheckRecords(Persons);

                /*
                 * Output
                 */
                OutputResults(CheckResults);
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
            }
        }

        static void GetRecords(string fileName)
        {
            using (RecordReader reader = new RecordReader(fileName))
            {
                /*
                 * Format instance, add to collection
                 */
                Record record = null;
                while ((record = reader.Read()) != null)
                {
                    if (Persons.ContainsKey(record.Name))
                    {
                        Persons[record.Name].AddTime(record.Time);
                    }
                    else
                    {
                        PersonRecord person = new PersonRecord(record.Name);
                        person.AddTime(record.Time);
                        Persons[record.Name] = person;
                    }
                }
            }
        }

        static void CheckRecords(Dictionary<string, PersonRecord> persons)
        {
            InitStandardRecords();
            foreach (var person in persons)
            {
                PersonResult checkResult = new PersonResult();
                PersonRecord personRecord = person.Value;
                personRecord.PickWorkStartTime();
                checkResult = personRecord.Check() as PersonResult;
                CheckResults.Add(checkResult);
            }
        }

        static void InitStandardRecords()
        {
            #region Test
            //StandardRecords["830"] = new StandardRecord(new TimeSpan(8, 30, 0));
            //StandardRecords["700"] = new StandardRecord(new TimeSpan(7, 0, 0));
            //StandardRecords["1400"] = new StandardRecord(new TimeSpan(14, 0, 0));
            #endregion
            XmlDocument xmlDoc = new XmlDocument();
            xmlDoc.Load(Path.Combine(currentDirectory, "CheckerConfig.xml"));
            XmlNodeList timeToWorkList = xmlDoc.DocumentElement.SelectNodes("/RecordChecker/TimeToWork/Time");
            int hour = 0;
            int minute = 0;
            int second = 0;
            foreach (var node in timeToWorkList)
            {
                XmlElement ele = node as XmlElement;
                hour = Convert.ToInt32(ele.GetAttribute("hour"));
                minute = Convert.ToInt32(ele.GetAttribute("minute"));
                second = Convert.ToInt32(ele.GetAttribute("second"));
                StandardRecords[hour.ToString() + minute.ToString() + second.ToString()] = new StandardRecord(new TimeSpan(hour, minute, second));
            }
            XmlElement element = xmlDoc.DocumentElement.SelectSingleNode("/RecordChecker/UnpaidLeaveTime") as XmlElement;
            hour = Convert.ToInt32(element.GetAttribute("hour"));
            minute = Convert.ToInt32(element.GetAttribute("minute"));
            second = Convert.ToInt32(element.GetAttribute("second"));
            UnpaidLeaveTime = (new TimeSpan(hour, minute, second)).ToString();
        }

        static void OutputResults(List<PersonResult> CheckResults)
        {
            string outputFileName = Path.GetFileNameWithoutExtension(fileName);
            string outputFilePrefix = Path.Combine(currentDirectory, outputFileName);
            using (DetailWriter writer = new DetailWriter())
            {
                writer.FillRange(CheckResults);
                writer.Save(outputFilePrefix + "_Detail.xlsx", SaveFormat.Xlsx, true);
            }
            using (SummaryWriter writer = new SummaryWriter())
            {
                writer.FillRange(CheckResults);
                writer.Save(outputFilePrefix + "_Summary.xlsx", SaveFormat.Xlsx, true);
            }
            Console.ForegroundColor = ConsoleColor.White;
            Console.WriteLine("Report output.");
            Console.ForegroundColor = ConsoleColor.Green;
            Console.WriteLine("  " + Path.GetFullPath(outputFilePrefix + "_Detail.xlsx"));
            Console.WriteLine("  " + Path.GetFullPath(outputFilePrefix + "_Summary.xlsx"));
            Console.ForegroundColor = ConsoleColor.White;
        }
    }
}
