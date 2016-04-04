using Aspose.Cells;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Xml;

namespace RecordChecker
{
    class RecordChecker : IDisposable
    {
        string fileName;
        string currentDirectory = System.AppDomain.CurrentDomain.SetupInformation.ApplicationBase;

        Dictionary<string, PersonRecord> Persons;
        List<PersonResult> CheckResults;

        bool ConsiderElastic = true;

        private CheckConfig mContext = null;

        public RecordChecker(string file)
        {
            this.fileName = file;
            this.Persons = new Dictionary<string, PersonRecord>();
            this.CheckResults = new List<PersonResult>();
        }

        public void Check()
        {
            Initialize();
            GetRecords();
            CheckRecords(Persons);
            OutputResults(CheckResults);
        }

        public void Initialize()
        {
            this.mContext = CheckConfig.GetInstance();
        }

        public void GetRecords()
        {
            using (RecordReader reader = new RecordReader(this.fileName))
            {
                /*
                 * Format record instance, add to collection
                 */
                LineRecord record = null;
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

        public void CheckRecords(Dictionary<string, PersonRecord> persons)
        {
            foreach (var person in persons)
            {
                PersonResult checkResult = new PersonResult();
                PersonRecord personRecord = person.Value;
                personRecord.PickWorkStartTime();
                checkResult = personRecord.Check() as PersonResult;
                CheckResults.Add(checkResult);
            }
        }

        public void OutputResults(List<PersonResult> results)
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

        public void Dispose()
        {
            if (this.Persons != null)
            {
                this.Persons.Clear();
                this.Persons = null;
            }
            if (this.CheckResults != null)
            {
                this.CheckResults.Clear();
                this.CheckResults = null;
            }
        }
    }
}
