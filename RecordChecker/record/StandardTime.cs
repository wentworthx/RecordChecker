using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace RecordChecker
{
    class StandardTime
    {
        public string Name { get; private set; }
        public TimeSpan Start { get; private set; }
        public TimeSpan End { get; private set; }

        public StandardTime() { }

        public StandardTime(TimeSpan start)
        {
            this.Name = start.ToString();
            this.Start = start;
            this.End = start.Add(new TimeSpan(9, 0, 0));
        }
    }
}
