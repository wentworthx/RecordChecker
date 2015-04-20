using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace RecordChecker
{
    class StandardRecord
    {
        public DateTime Start { get; private set; }
        public DateTime End { get; private set; }

        public StandardRecord(DateTime start)
        {
            this.Start = start;
            this.End = start.AddHours(9);
        }
    }
}
