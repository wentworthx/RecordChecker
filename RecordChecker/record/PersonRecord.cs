using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace RecordChecker
{
    class PersonRecord : IRecordCheck
    {
        public string Name { get; private set; }
        public Dictionary<int, DayRecord> DayRecords { get; private set; }

        public PersonRecord()
        {
            DayRecords = new Dictionary<int, DayRecord>();
        }

        public PersonRecord(string name)
        {
            this.Name = name;
            DayRecords = new Dictionary<int, DayRecord>();
        }

        public void AddTime(DateTime time)
        {
            int key = time.Day;
            if (DayRecords.ContainsKey(key))
            {
                DayRecords[key].AddTime(time);
            }
            else
            {
                DayRecords[key] = new DayRecord(key);
                DayRecords[key].AddTime(time);
            }
        }

        public object Check()
        {
            PersonResult result = new PersonResult(this.Name);
            foreach (var day in DayRecords)
            {
                result.AddResult(day.Value.Check() as DayResult);
            }
            return result;
        }
    }
}
