using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace RecordChecker
{
    class PersonRecord : IRecordCheck
    {
        private StandardRecord mWorkStartTime;
        private int formerDayNum = 0;

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
            else if (key > 1 && time.Hour <= 4)
            {
                DayRecords[formerDayNum].AddTime(time);
            }
            else
            {
                DayRecords[key] = new DayRecord(key);
                DayRecords[key].AddTime(time);
                formerDayNum = key;
            }
        }

        public void PickWorkStartTime()
        {
            Dictionary<StandardRecord, List<int>> startTimes = new Dictionary<StandardRecord, List<int>>();
            foreach (var day in DayRecords)
            {
                StandardRecord sr = day.Value.CloseTo();
                List<int> days = new List<int>();
                if (startTimes.TryGetValue(sr, out days))
                {
                    days.Add(day.Key);
                }
                else
                {
                    startTimes[sr] = new List<int>() { day.Key };
                }
            }
            int max = 0;
            foreach (var startTime in startTimes)
            {
                if (max < startTime.Value.Count)
                {
                    max = startTime.Value.Count;
                    mWorkStartTime = startTime.Key;
                }
            }
        }

        public object Check()
        {
            PersonResult result = new PersonResult(this.Name);
            foreach (var item in DayRecords)
            {
                DayRecord day = item.Value;
                day.SetStandardRecord(this.mWorkStartTime);
                result.AddResult(day.Check() as DayResult);
            }
            return result;
        }
    }
}
