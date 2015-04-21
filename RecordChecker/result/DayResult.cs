using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace RecordChecker
{
    class DayResult
    {
        public int DayNum { get; private set; }
        //public bool NeedHR { get; set; }
        public ResultStatus Status { get; set; }
        public DayRecord DayRecord { get; set; }

        public DayResult()
        {
            this.Status = ResultStatus.None;
        }

        public DayResult(int day)
        {
            this.DayNum = day;
            this.Status = ResultStatus.None;
        }
    }
}
