using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;

namespace LicenseManager
{
    public class LicenceHistory
    {
        public static object status { get; set; }
        public static object userName { get; set; }
        public static object date { get; set; }
        public static DateTime startDate { get; set; }
        public static DateTime endDate { get; set; }
    }

    public class LicenceHistoryData
    {
        /// <summary>
        /// To read file
        /// </summary>
        /// <returns></returns>
        public static List<Tuple<string, string, string>> ReadFile()
        {
            string filePath = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.CommonDocuments), "SrinSoft", "CloudSync", "LicenceLog.csv");
            string line;
            List<Tuple<string, string, string>> fileData = new List<Tuple<string, string, string>>();
            if (File.Exists(filePath))
            {
                StreamReader file = null;
                try
                {
                    //var file2 = File.ReadAllLines(filePath);
                    //var file1 =  File.Open(filePath, FileMode.Open, FileAccess.Read);

                    int i = 1;
                    using (FileStream stream = File.Open(filePath, FileMode.Open, FileAccess.Read, FileShare.ReadWrite))
                    {
                        using (StreamReader reader = new StreamReader(stream))
                        {
                            while (/*!reader.EndOfStream && */(line = reader.ReadLine())!= null)
                            {
                                if (i != 1)
                                {
                                    var values = line.Split(',');
                                    fileData.Add(new Tuple<string, string, string>(values[0], values[1], values[2]));
                                }

                                i++;
                            }
                        }
                    }

                    ///if(fileData.Remove(string.Empty))

                    //int i = 1;
                    //file = new StreamReader(filePath);
                    //while ((line = file.ReadLine()) != null)
                    //{
                    //    if (i != 1)
                    //    {
                    //        var values = line.Split(',');
                    //        fileData.Add(new Tuple<string, string, string>(values[0], values[1], values[2]));
                    //    }

                    //    i++;
                    //}
                }
                catch (Exception ex)
                {

                }
                finally
                {
                    ///if (file != null)
                   /// file.Close();
                }
            }
            return fileData;
        }

        public List<Tuple<string, DateTime, string>> DataFilter()
        {
            List<Tuple<string, string, string>> fileData = ReadFile();
            List<Tuple<string, DateTime, string>> fileDatas = new List<Tuple<string, DateTime, string>>();
            if (fileData != null)
                fileDatas = ApplyingFilter(fileData);

            return fileDatas;
        }

        /// <summary>
        /// Applying filter and get filtered results
        /// </summary>
        /// <param name="fileData"></param>
        /// <returns></returns>
        private List<Tuple<string, DateTime, string>> ApplyingFilter(List<Tuple<string, string, string>> fileData)
        {
            List<Tuple<string, DateTime, string>> fileDatas = new List<Tuple<string, DateTime, string>>();

            try
            {
                foreach (var item in fileData)
                {
                    DateTime dateTime;
                    DateTime.TryParseExact(item.Item2, "dd-MM-yyyy HH:mm:ss", CultureInfo.InvariantCulture, DateTimeStyles.None, out dateTime);
                    fileDatas.Add(new Tuple<string, DateTime, string>(item.Item1, dateTime, item.Item3));
                }

                FilterType filterType = FilterType.Date;

                if (LicenceHistory.date == null)
                    LicenceHistory.date = string.Empty;

                if (LicenceHistory.status == null)
                    LicenceHistory.status = string.Empty;

                if (LicenceHistory.userName == null)
                    LicenceHistory.userName = string.Empty;


                List<string> comboBoxs = new List<string> { LicenceHistory.date.ToString(), LicenceHistory.status.ToString(), LicenceHistory.userName.ToString() };

                if (comboBoxs[0] != "None" && !string.IsNullOrEmpty(comboBoxs[0]))
                    fileDatas = DateFilter(fileDatas);

                if (!string.IsNullOrEmpty(comboBoxs[1])&& comboBoxs[1] != "All")
                {
                    filterType = FilterType.Status;
                    fileDatas = FilterMethod(filterType, fileDatas, LicenceHistory.status.ToString(), null);
                }

                if (comboBoxs[2] != "All" && !string.IsNullOrEmpty(comboBoxs[2]))
                {
                    filterType = FilterType.UserName;
                    fileDatas = FilterMethod(filterType, fileDatas, null, comboBoxs[2]);
                }
                return fileDatas;
            }
            catch (Exception ex)
            {
                return fileDatas;
            }
        }

        private List<Tuple<string, DateTime, string>> DateFilter(List<Tuple<string, DateTime, string>> datas)
        {
            List<Tuple<string, DateTime, string>> fileDatas = new List<Tuple<string, DateTime, string>>();

            try
            {
                foreach (var item in datas)
                {
                    DateTime dateTime = item.Item2;
                    DateTime today = DateTime.Today;
                    if (LicenceHistory.date.ToString() == "From Yesterday")
                    {
                        DateTime yesterday = DateTime.Today.AddDays(-1);
                        if (dateTime >= yesterday && dateTime <= DateTime.Today)
                            fileDatas.Add(new Tuple<string, DateTime, string>(item.Item1, dateTime, item.Item3));
                    }

                    else if (LicenceHistory.date.ToString() == "From Last Week")
                    {
                        int days = DateTime.Now.DayOfWeek - DayOfWeek.Sunday;
                        DateTime pastDate = DateTime.Now.AddDays(-(days + 7));
                        if (dateTime >= pastDate && dateTime <= DateTime.Today && dateTime <= today)
                            fileDatas.Add(new Tuple<string, DateTime, string>(item.Item1, dateTime, item.Item3));
                    }

                    else if (LicenceHistory.date.ToString() == "From Last Month")
                    {
                        int days = Convert.ToInt16(DateTime.Now.ToString("dd", CultureInfo.InvariantCulture));
                        DateTime lastMonth = DateTime.Today.AddMonths(-1).AddDays(-(days - 1));
                        if (dateTime.Month >= lastMonth.Month && dateTime.Year == lastMonth.Year && dateTime <= today)
                            fileDatas.Add(new Tuple<string, DateTime, string>(item.Item1, dateTime, item.Item3));
                    }

                    else if (LicenceHistory.date.ToString() == "From Last Year")
                    {
                        int days = Convert.ToInt16(DateTime.Now.ToString("dd", CultureInfo.InvariantCulture));
                        int month = Convert.ToInt16(DateTime.Now.ToString("MM", CultureInfo.InvariantCulture));
                        DateTime lastYear = DateTime.Today.AddMonths(month - (month - 1)).AddDays(days - (days - 1)).AddYears(-1);
                        if (dateTime >= lastYear && dateTime <= today)
                            fileDatas.Add(new Tuple<string, DateTime, string>(item.Item1, dateTime, item.Item3));
                    }

                    else if (LicenceHistory.date.ToString() == "Period")
                    {
                        DateTime startDate = LicenceHistory.startDate;
                        DateTime endDate = LicenceHistory.endDate;
                        if (dateTime >= startDate && dateTime <= endDate)
                            fileDatas.Add(new Tuple<string, DateTime, string>(item.Item1, dateTime, item.Item3));
                    }
                }
                return fileDatas;
            }
            catch (Exception ex)
            {
                return fileDatas;
            }
        }

        private List<Tuple<string, DateTime, string>> FilterMethod(FilterType filter, List<Tuple<string, DateTime, string>> fileDatas, string findWord, string findWord1)
        {
            List<Tuple<string, DateTime, string>> filteredData = new List<Tuple<string, DateTime, string>>();
            switch (filter)
            {
                case FilterType.Date:
                    return fileDatas;

                case FilterType.UserName:
                    return filteredData = fileDatas.Where(d => d.Item1.Equals(findWord1)).ToList();

                case FilterType.Status:
                    return filteredData = fileDatas.Where(d => d.Item3.Equals(findWord)).ToList();

                case FilterType.StatusandUsername:
                    return filteredData = fileDatas.Where(d => d.Item1.Equals(findWord) && d.Item3.Equals(findWord1)).ToList();
            }
            return fileDatas;
        }

        public enum FilterType
        {
            Date,
            UserName,
            Status,
            StatusandUsername,
        }

    }
}
