using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace RecordChecker
{
    class DayRecord : IRecordCheck
    {
        private const double RATE = 0.5;

        private StandardRecord mWorkStartTime;
        private TimeSpan mStartTime;
        private TimeSpan mEndTime;
        private bool isInitialized = false;
        private bool useElastic = false;

        public int DayNum { get; private set; }
        public DateTime HighDayTime { get; private set; }
        public List<TimeSpan> DayTimes { get; private set; }

        public DayRecord()
        {
            this.mStartTime = new TimeSpan(0);
            this.DayTimes = new List<TimeSpan>();
        }

        public DayRecord(int name)
        {
            this.DayNum = name;
            this.mStartTime = new TimeSpan(0);
            this.DayTimes = new List<TimeSpan>();
        }

        public void AddTime(DateTime time)
        {
            if (!isInitialized)
            {
                HighDayTime = new DateTime(time.Year, time.Month, time.Day);
                isInitialized = true;
            }
            TimeSpan temp = time - HighDayTime;
            DayTimes.Add(temp);
        }

        public StandardRecord CloseTo()
        {
            mStartTime = DayTimes.Min();
            long minTicks = long.MaxValue;
            string closeStandName = string.Empty;
            Dictionary<string, StandardRecord> standRecords = Program.StandardRecords;
            foreach (var stand in standRecords)
            {
                long tempMin = 0;
                TimeSpan standStart = stand.Value.Start;
                if (mStartTime < standStart)
                {
                    tempMin = (long)((standStart - mStartTime).Ticks * RATE);
                }
                else
                {
                    tempMin = (mStartTime - standStart).Ticks;
                }
                if (minTicks > tempMin)
                {
                    minTicks = tempMin;
                    closeStandName = stand.Key;
                }
            }
            return standRecords[closeStandName];
        }

        public void SetStandardRecord(StandardRecord standardRecord)
        {
            this.mWorkStartTime = standardRecord;
            this.useElastic = WhetherUseElastic();
        }

        public object Check()
        {
            DayResult result = new DayResult(this.DayNum);
            result.DayRecord = this;
            bool isLate = true;
            bool isLess = true;
            isLate = IsLate(this.mWorkStartTime.Start);
            isLess = IsLess(this.mWorkStartTime.End);
            if (isLate)
            {
                if (isLess)
                    result.Status = ResultStatus.Late | ResultStatus.LessTime | ResultStatus.HR;
                else if (useElastic)
                    result.Status = ResultStatus.Late | ResultStatus.MaybeUnpaidLeave | ResultStatus.HR;
                else
                    result.Status = ResultStatus.Late | ResultStatus.HR;
            }
            else
            {
                if (isLess)
                    result.Status = ResultStatus.LessTime | ResultStatus.HR;
                else if (DayTimes.Count == 2 | DayTimes.Count == 3)
                    result.Status = ResultStatus.Normal;
                else
                    result.Status = ResultStatus.Normal | ResultStatus.HR;
            }
            return result;
        }

        private bool IsLate(TimeSpan standardStart)
        {
            bool isLate = true;

            if (useElastic)
            {
                isLate = this.mStartTime > standardStart + new TimeSpan(0, 30, 0);
            }
            else
            {
                isLate = this.mStartTime > standardStart;
            }

            return isLate;
        }

        private bool IsLess(TimeSpan standardEnd)
        {
            bool isLess = true;

            this.mEndTime = DayTimes.Max();
            if (useElastic)
            {
                isLess = this.mEndTime - this.mStartTime < new TimeSpan(9, 0, 0);
            }
            else
            {
                isLess = this.mEndTime < standardEnd;
            }

            return isLess;
        }

        //private bool IsLess(TimeSpan standardEnd, bool isLate)
        //{
        //    bool isLess = true;

        //    this.mEndTime = DayTimes.Max();
        //    if (isLate)
        //    {
        //        TimeSpan closestHalf = new TimeSpan(this.mStartTime.Hours, 0, 0);
        //        if (this.mStartTime.Minutes > 30)
        //        {
        //            closestHalf = closestHalf.Add(new TimeSpan(1, 0, 0));
        //        }
        //        else
        //        {
        //            closestHalf = closestHalf.Add(new TimeSpan(0, 30, 0));
        //        }
        //        if (this.mEndTime > closestHalf.Add(new TimeSpan(9, 0, 0)))
        //        {
        //            isLess = false;
        //        }
        //        else
        //        {
        //            isLess = true;
        //        }
        //    }
        //    else
        //    {
        //        isLess = this.mEndTime < standardEnd;
        //    }

        //    return isLess;
        //}

        private bool WhetherUseElastic()
        {
            return Program.ConsiderElastic && this.mWorkStartTime.Name.Equals("08:30:00");
        }
    }
}
