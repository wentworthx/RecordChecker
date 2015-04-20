using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace RecordChecker
{
    class DayRecord : IRecordCheck
    {
        public int DayNum { get; private set; }
        public DateTime HighDayeTime { get; private set; }
        public List<DateTime> DayTimes { get; private set; }

        public DayRecord()
        {
            this.DayTimes = new List<DateTime>();
        }

        public DayRecord(int name)
        {
            this.DayNum = name;
            this.DayTimes = new List<DateTime>();
        }

        public void AddTime(DateTime time)
        {
            DayTimes.Add(time);
        }

        public object Check()
        {
            DayResult result = new DayResult(this.DayNum);

            return result;
        }
    }
}
