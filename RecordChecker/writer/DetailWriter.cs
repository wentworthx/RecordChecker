using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Aspose.Cells;

namespace RecordChecker
{
    class DetailWriter : RecordWriter
    {
        private const string defaultSheetName = "Detail";

        public DetailWriter()
            : base(defaultSheetName) { }

        protected override void SetSheetTitles(Worksheet sheet)
        {
            Cells cells = sheet.Cells;
            Style boldTitle = mWorkbook.Styles[BOLD_TITLE];
            int maxDataRow = cells.MaxDataRow;
            cells[0, 0].PutValue("Name");
            cells[0, 0].SetStyle(boldTitle);
            cells[0, 1].PutValue("Time");
            cells[0, 1].SetStyle(boldTitle);
            cells[0, 2].PutValue("Status");
            cells[0, 2].SetStyle(boldTitle);
            cells.SetColumnWidthPixel(1, 140);
            cells.SetColumnWidthPixel(2, 180);
        }

        protected override void FillOnePerson(PersonResult result, Worksheet sheet)
        {
            Cells cells = sheet.Cells;
            int maxDataRow = cells.MaxDataRow;
            int index = 1;
            foreach (var dayResult in result.DayResults)
            {
                DayRecord dayRecord = dayResult.DayRecord;
                foreach (var dayTime in dayRecord.DayTimes)
                {
                    cells[maxDataRow + index, 0].PutValue(result.PersonName);
                    cells[maxDataRow + index, 1].PutValue((dayRecord.HighDayTime + dayTime).ToString());
                    cells[maxDataRow + index, 2].PutValue(GetStatusString(dayResult.Status));
                    if (IsContain(dayResult.Status, ResultStatus.Late))
                    {
                        SetCellColor(cells, maxDataRow + index, 0, 3, mWorkbook.Styles[LATE_COLOR]);
                    }
                    else if (IsContain(dayResult.Status, ResultStatus.HR))
                    {
                        SetCellColor(cells, maxDataRow + index, 0, 3, mWorkbook.Styles[NEED_HR_COLOR]);
                    }
                    index++;
                }
            }
        }
    }
}
