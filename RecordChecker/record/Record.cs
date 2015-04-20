using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace RecordChecker
{
    class Record
    {
        public string Name { get; set; }
        public DateTime Time { get; set; }

        public Record() { }

        public Record(string name, DateTime time)
        {
            this.Name = name;
            this.Time = time;
        }
    }
}
