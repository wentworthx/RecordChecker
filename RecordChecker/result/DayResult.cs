using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace RecordChecker
{
    class DayResult
    {
        public int DayNum { get; private set; }
        public bool NeedHR { get; set; }
        public List<DateTime> TimesNeedHR { get; private set; }

        public DayResult()
        {
            this.TimesNeedHR = new List<DateTime>();
        }

        public DayResult(int day)
        {
            this.DayNum = day;
            this.TimesNeedHR = new List<DateTime>();
        }
    }
}
