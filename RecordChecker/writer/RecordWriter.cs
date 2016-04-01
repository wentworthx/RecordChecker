using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using Aspose.Cells;

namespace RecordChecker
{
    class RecordWriter : IDisposable
    {
        protected const string BOLD_TITLE = "BoldTitle";
        protected const string NEED_HR_COLOR = "NeedHR_HotPink";
        protected const string LATE_COLOR = "Late_OrangeRed";
        protected const string NORMAL_COLOR = "Normal_LightGreen";

        private string defaultSheetName;

        protected Workbook mWorkbook = null;

        protected const string AsposeLicence = @"";

        public RecordWriter(string sheetName)
        {
            //using (MemoryStream stream = new MemoryStream(Encoding.UTF8.GetBytes(AsposeLicence)))
            //{
            //    License excelLicense = new License();
            //    excelLicense.SetLicense(stream);
            //}
            this.defaultSheetName = sheetName;

            mWorkbook = new Workbook();
            mWorkbook.Worksheets.Clear();
            Style style = mWorkbook.Styles[mWorkbook.Styles.Add()];
            style.Name = BOLD_TITLE;
            style.Font.IsBold = true;
            style = mWorkbook.Styles[mWorkbook.Styles.Add()];
            style.Name = NEED_HR_COLOR;
            style.Pattern = BackgroundType.Solid;
            style.ForegroundColor = Color.HotPink;
            style = mWorkbook.Styles[mWorkbook.Styles.Add()];
            style.Name = LATE_COLOR;
            style.Pattern = BackgroundType.Solid;
            style.ForegroundColor = Color.Red;
            style = mWorkbook.Styles[mWorkbook.Styles.Add()];
            style.Name = NORMAL_COLOR;
            style.Pattern = BackgroundType.Solid;
            style.ForegroundColor = Color.LightGreen;

            CreateSheet(this.defaultSheetName);
        }

        public void FillRange(List<PersonResult> resultList)
        {
            Worksheet sheet = mWorkbook.Worksheets[this.defaultSheetName];
            foreach (var result in resultList)
            {
                FillOnePerson(result, sheet);
            }
        }

        protected void CreateSheet(string name)
        {
            Worksheet sheet = mWorkbook.Worksheets.Add(name);
            SetSheetTitles(sheet);
        }

        protected virtual void SetSheetTitles(Worksheet sheet) { }

        protected virtual void FillOnePerson(PersonResult result, Worksheet sheet) { }

        public void Save(string filePath, SaveFormat saveFormat, bool overwrite)
        {
            if (mWorkbook != null)
            {
                if (overwrite || !File.Exists(filePath))
                {
                    mWorkbook.Save(filePath, saveFormat);
                }
            }
        }

        protected bool IsContain(ResultStatus status, ResultStatus toCheck)
        {
            return (status & toCheck) != 0;
        }

        protected void SetCellColor(Cells cells, int line, int column, int columnCount, Style style)
        {
            while (columnCount > 0)
            {
                columnCount--;
                cells[line, column + columnCount].SetStyle(style);
            }
        }

        protected string GetStatusString(ResultStatus status)
        {
            StringBuilder str = new StringBuilder();
            if (IsContain(status, ResultStatus.Normal))
                str.Append("Normal | ");
            if (IsContain(status, ResultStatus.Late))
                str.Append("Late | ");
            if (IsContain(status, ResultStatus.LessTime))
                str.Append("Less | ");
            if (IsContain(status, ResultStatus.MaybeUnpaidLeave))
                str.Append("MaybeUnpaidLeave | ");
            if (IsContain(status, ResultStatus.HR))
                str.Append("HR |");
            if (str.Length > 0)
                str.Remove(str.Length - 2, 2);
            return str.ToString();
        }

        public void Dispose()
        {
            this.mWorkbook = null;
        }
    }
}
