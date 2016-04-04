using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace RecordChecker
{
    class LineRecord
    {
        public string Name { get; set; }
        public DateTime Time { get; set; }

        public LineRecord() { }

        public LineRecord(string name, DateTime time)
        {
            this.Name = name;
            this.Time = time;
        }
    }
}
