using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace RecordChecker
{
    enum ResultStatus
    {
        None = 0,
        Normal = 1,
        Late = 2,
        LessTime = 4,
        MaybeUnpaidLeave = 8,
        HR = 16,
    }
}
