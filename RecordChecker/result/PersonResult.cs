using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace RecordChecker
{
    class PersonResult
    {
        public string PersonName { get; private set; }
        public List<DayResult> DayResults { get; private set; }

        public PersonResult()
        {
            this.DayResults = new List<DayResult>();
        }

        public PersonResult(string name)
        {
            this.PersonName = name;
            this.DayResults = new List<DayResult>();
        }

        public void AddResult(DayResult result)
        {
            DayResults.Add(result);
        }
    }
}
