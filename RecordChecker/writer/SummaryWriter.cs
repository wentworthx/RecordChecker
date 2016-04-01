using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Aspose.Cells;

namespace RecordChecker
{
    class SummaryWriter : RecordWriter
    {
        private const string defaultSheetName = "Summary";

        public SummaryWriter()
            : base(defaultSheetName) { }

        protected override void SetSheetTitles(Worksheet sheet)
        {
            Cells cells = sheet.Cells;
            Style boldTitle = mWorkbook.Styles[BOLD_TITLE];
            int maxDataRow = cells.MaxDataRow;
            cells[0, 0].PutValue("Name");
            cells[0, 0].SetStyle(boldTitle);
            for (int i = 1; i < 32; i++)
            {
                cells[0, i].PutValue(i);
                cells[0, i].SetStyle(boldTitle);
                cells.SetColumnWidthPixel(i, 30);
            }
        }

        protected override void FillOnePerson(PersonResult result, Worksheet sheet)
        {
            Cells cells = sheet.Cells;
            int maxDataRow = cells.MaxDataRow + 1;
            cells[maxDataRow, 0].PutValue(result.PersonName);
            foreach (var dayResult in result.DayResults)
            {
                int dayNum = dayResult.DayNum;
                if (dayResult.Status == ResultStatus.Normal)
                {
                    SetCellColor(cells, maxDataRow, dayNum, 1, mWorkbook.Styles[NORMAL_COLOR]);
                }
                else
                {
                    //cells[maxDataRow, dayNum].PutValue(GetStatusString(dayResult.Status));
                    if (IsContain(dayResult.Status, ResultStatus.Late))
                    {
                        SetCellColor(cells, maxDataRow, dayNum, 1, mWorkbook.Styles[LATE_COLOR]);
                    }
                    else if (IsContain(dayResult.Status, ResultStatus.HR))
                    {
                        SetCellColor(cells, maxDataRow, dayNum, 1, mWorkbook.Styles[NEED_HR_COLOR]);
                    }
                }
            }
        }
    }
}
