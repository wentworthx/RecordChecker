using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;

namespace RecordChecker
{
    class RecordReader : IDisposable
    {
        private string mFilePath;
        private StreamReader mReader;

        public RecordReader(string filePath)
        {
            this.mFilePath = filePath;
            this.mReader = new StreamReader(this.mFilePath);
        }

        public void Load(string filePath)
        {
            this.mFilePath = filePath;
            this.mReader = new StreamReader(this.mFilePath);
        }

        public LineRecord Read()
        {
            LineRecord record = null;

            string name = string.Empty;
            DateTime time = new DateTime(0);
            while (true)
            {
                string line = this.mReader.ReadLine();
                if (line != null)
                {
                    GetRecord(line, out name, out time);
                    if (time.Ticks == 1)
                        continue;
                    else
                        record = new LineRecord(name, time);
                    break;
                }
                break;
            }

            return record;
        }

        private void GetRecord(string line, out string outName, out DateTime outTime)
        {
            if (line.Contains("上班签到"))
            {
                string[] parts = line.Split('\t');
                outName = parts[2];
                outTime = Convert.ToDateTime(parts[3]);
            }
            else
            {
                outName = string.Empty;
                outTime = new DateTime(1);
            }
        }

        public void Dispose()
        {
            this.mFilePath = string.Empty;
            this.mReader.Dispose();
        }
    }
}
