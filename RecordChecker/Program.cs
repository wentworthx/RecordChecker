using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace RecordChecker
{
    class Program
    {
        static string fileName = @"C:\Users\fxiao\Desktop\2015.3\2015.3\9F\Record.txt";

        static Dictionary<string, PersonRecord> Persons = new Dictionary<string, PersonRecord>();
        static List<PersonResult> CheckResults = new List<PersonResult>();

        public static bool ConsiderElastic = true;
        public static Dictionary<string, StandardRecord> StandardRecords = new Dictionary<string, StandardRecord>();

        static void Main(string[] args)
        {
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
            StandardRecords["830"] = new StandardRecord(new TimeSpan(8, 30, 0));
            StandardRecords["700"] = new StandardRecord(new TimeSpan(7, 0, 0));
            StandardRecords["1400"] = new StandardRecord(new TimeSpan(14, 0, 0));
        }

        static void OutputResults(List<PersonResult> CheckResults)
        {

        }
    }
}
