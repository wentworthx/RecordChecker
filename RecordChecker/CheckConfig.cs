using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Xml;

namespace RecordChecker
{
    class CheckConfig
    {
        private static CheckConfig mConfig = null;
        private static object locker = new object();

        private XmlDocument mXmlDoc = null;
        private string configFile = string.Empty;

        private Dictionary<string, StandardTime> mStandardTimes;
        private Dictionary<string, HashSet<string>> mDepartmentTimes;
        private Dictionary<string, HashSet<string>> mPersonTimes;
        private HashSet<string> mUnpaidLeaveTimes;

        public Dictionary<string, StandardTime> StandardTimes { get { return this.mStandardTimes; } }
        public Dictionary<string, HashSet<string>> DepartmentTimes { get { return this.mDepartmentTimes; } }
        public Dictionary<string, HashSet<string>> PersonTimes { get { return this.mPersonTimes; } }
        public HashSet<string> UnpaidLeaveTimes { get { return this.mUnpaidLeaveTimes; } }

        public static CheckConfig GetInstance()
        {
            lock (locker)
            {
                if (mConfig == null)
                {
                    mConfig = new CheckConfig();
                }
                return mConfig;
            }
        }

        private CheckConfig()
        {
            string currentDirectory = System.AppDomain.CurrentDomain.SetupInformation.ApplicationBase;
            this.configFile = Path.Combine(currentDirectory, "CheckerConfig.xml");
            if (File.Exists(this.configFile))
            {
                this.mXmlDoc = new XmlDocument();
                this.mXmlDoc.Load(this.configFile);

                GetStandardTimes();
                GetDepartmentTimes();
                GetPersonTimes();
                GetUnpaidLeaveTime();
            }
            else
            {
                throw new Exception("CheckerConfig.xml does not exist.");
            }
        }

        void GetStandardTimes()
        {
            this.mStandardTimes = new Dictionary<string, StandardTime>();

            XmlNodeList timeToWorkList = this.mXmlDoc.DocumentElement.SelectNodes("/recordchecker/timetowork/time");
            string name = string.Empty;
            int hour = 0;
            int minute = 0;
            int second = 0;
            foreach (var node in timeToWorkList)
            {
                XmlElement ele = node as XmlElement;
                name = ele.GetAttribute("name");
                hour = Convert.ToInt32(ele.GetAttribute("hour"));
                minute = Convert.ToInt32(ele.GetAttribute("minute"));
                second = Convert.ToInt32(ele.GetAttribute("second"));
                StandardTimes[name] = new StandardTime(new TimeSpan(hour, minute, second));
            }
        }

        void GetDepartmentTimes()
        {
            this.mDepartmentTimes = new Dictionary<string, HashSet<string>>();

            XmlNodeList departmentTimes = this.mXmlDoc.DocumentElement.SelectNodes("/recordchecker/departments/department");
            string name = string.Empty;
            string departmentName = string.Empty;
            foreach (var node in departmentTimes)
            {
                XmlElement ele = node as XmlElement;
                name = ele.GetAttribute("name");
                departmentName = ele.InnerText;
                if (!string.IsNullOrEmpty(name) && !string.IsNullOrEmpty(departmentName))
                {
                    if (this.mDepartmentTimes.ContainsKey(departmentName))
                    {
                        this.mDepartmentTimes[departmentName].Add(name);
                    }
                    else
                    {
                        HashSet<string> timeNames = new HashSet<string>();
                        timeNames.Add(name);
                        this.mDepartmentTimes[departmentName] = timeNames;
                    }
                }
            }
        }

        void GetPersonTimes()
        {
            this.mPersonTimes = new Dictionary<string, HashSet<string>>();

            XmlNodeList personTimes = this.mXmlDoc.DocumentElement.SelectNodes("/recordchecker/persons/person");
            string name = string.Empty;
            string personName = string.Empty;
            foreach (var node in personTimes)
            {
                XmlElement ele = node as XmlElement;
                name = ele.GetAttribute("name");
                personName = ele.InnerText;
                if (!string.IsNullOrEmpty(name) && !string.IsNullOrEmpty(personName))
                {
                    if (this.mPersonTimes.ContainsKey(personName))
                    {
                        this.mPersonTimes[personName].Add(name);
                    }
                    else
                    {
                        HashSet<string> timeNames = new HashSet<string>();
                        timeNames.Add(name);
                        this.mPersonTimes[personName] = timeNames;
                    }
                }
            }
        }

        void GetUnpaidLeaveTime()
        {
            this.mUnpaidLeaveTimes = new HashSet<string>();

            XmlNode unpaidLeaveTime = this.mXmlDoc.DocumentElement.SelectSingleNode("/recordchecker/unpaidleavetime");
            XmlElement ele = unpaidLeaveTime as XmlElement;
            string name = ele.GetAttribute("name");
            if (!string.IsNullOrEmpty(name))
            {
                this.mUnpaidLeaveTimes.Add(name);
            }
        }
    }
}
