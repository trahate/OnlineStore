using System;
using System.Collections.ObjectModel;
using System.Globalization;
using System.IO;
using System.Web;


namespace TravelDataRecorder.Common
{
    public class DateTimeHandling
    {
        public static DateTime GetDateTime(DateTime dt, string timeZone = "")
        {
            if (!String.IsNullOrEmpty(timeZone))
            {
                TimeZoneInfo timeInfo = TimeZoneInfo.FindSystemTimeZoneById(timeZone);
                dt = TimeZoneInfo.ConvertTimeFromUtc(dt, timeInfo);
            }
            else
            {
                TimeZone localZone = TimeZone.CurrentTimeZone;
                TimeSpan currentOffset = localZone.GetUtcOffset(dt);
                DaylightTime daylight = localZone.GetDaylightChanges(dt.Year);
                dt = dt.Add(currentOffset);
            }

            return dt;
        }

        public static string GetDateTime(string dt, string timeZone = "")
        {
            DateTime convertedDateTime;
            var isValidFormat = DateTime.TryParse(dt, out convertedDateTime);
            if (isValidFormat)
            {
                if (!String.IsNullOrEmpty(timeZone))
                {
                    TimeZoneInfo timeInfo = TimeZoneInfo.FindSystemTimeZoneById(timeZone);
                    dt = TimeZoneInfo.ConvertTimeFromUtc(convertedDateTime, timeInfo).ToString();
                }
                else
                {
                    TimeZone localZone = TimeZone.CurrentTimeZone;
                    TimeSpan currentOffset = localZone.GetUtcOffset(convertedDateTime);
                    DaylightTime daylight = localZone.GetDaylightChanges(convertedDateTime.Year);
                    convertedDateTime = convertedDateTime.Add(currentOffset);
                    //convertedDateTime = convertedDateTime.Add(daylight.Delta); // If required then later we can add
                    return convertedDateTime.ToString();
                }
            }
            return dt;
        }

        public static string GetTimeByDate(string dt, string timeZone = "")
        {
            DateTime convertedDateTime;
            var isValidFormat = DateTime.TryParse(dt, out convertedDateTime);
            if (isValidFormat)
            {
                if (!String.IsNullOrEmpty(timeZone))
                {
                    TimeZoneInfo timeInfo = TimeZoneInfo.FindSystemTimeZoneById(timeZone);
                    dt = TimeZoneInfo.ConvertTimeFromUtc(convertedDateTime, timeInfo).ToString();
                }
                else
                {
                    TimeZone localZone = TimeZone.CurrentTimeZone;
                    TimeSpan currentOffset = localZone.GetUtcOffset(convertedDateTime);
                    DaylightTime daylight = localZone.GetDaylightChanges(convertedDateTime.Year);
                    convertedDateTime = convertedDateTime.Add(currentOffset);
                    //convertedDateTime = convertedDateTime.Add(daylight.Delta); // If required then later we can add
                    return convertedDateTime.ToString("hh:mm tt");
                }
            }
            return dt;
        }

        public static string GetDateWithFormat(string dt, string timeZone = "")
        {
            DateTime convertedDateTime;
            var isValidFormat = DateTime.TryParse(dt, out convertedDateTime);
            if (isValidFormat)
            {
                if (!String.IsNullOrEmpty(timeZone))
                {
                    TimeZoneInfo timeInfo = TimeZoneInfo.FindSystemTimeZoneById(timeZone);
                    dt = TimeZoneInfo.ConvertTimeFromUtc(convertedDateTime, timeInfo).ToString("MM/dd/yyyy");
                }
                else
                {
                    TimeZone localZone = TimeZone.CurrentTimeZone;
                    TimeSpan currentOffset = localZone.GetUtcOffset(convertedDateTime);
                    DaylightTime daylight = localZone.GetDaylightChanges(convertedDateTime.Year);
                    convertedDateTime = convertedDateTime.Add(currentOffset);
                    //convertedDateTime = convertedDateTime.Add(daylight.Delta); // If required then later we can add
                    return convertedDateTime.ToString("MM/dd/yyyy");
                }
            }
            return dt;
        }

        public static DateTime SetDateTime(DateTime dt)
        {
            return TimeZoneInfo.ConvertTimeToUtc(dt);
        }

        public static string SetDateTime(string strdate)
        {
            DateTime convertedDateTime;
            var isValidFormat = DateTime.TryParse(strdate, out convertedDateTime);
            if (isValidFormat)
            {
                //to do:needs to be removed
                return TimeZoneInfo.ConvertTimeToUtc(convertedDateTime).ToString("MM/dd/yyyy HH:mm:ss");
                //to do:needs to be removed
            }
            return strdate;
        }

        public static DateTime SetDateTimeFormat(string strdate)
        {
            DateTime convertedDateTime;
            var isValidFormat = DateTime.TryParse(strdate, out convertedDateTime);
            if (isValidFormat)
            {
                //to do:needs to be removed
                return TimeZoneInfo.ConvertTimeToUtc(convertedDateTime);
                //to do:needs to be removed
            }
            return convertedDateTime;
        }

        public static DateTime GetDateTimeFormat(string dt, string timeZone = "")
        {
            DateTime convertedDateTime;
            var isValidFormat = DateTime.TryParse(dt, out convertedDateTime);
            if (isValidFormat)
            {
                if (!String.IsNullOrEmpty(timeZone))
                {
                    TimeZoneInfo timeInfo = TimeZoneInfo.FindSystemTimeZoneById(timeZone);
                    convertedDateTime = TimeZoneInfo.ConvertTimeFromUtc(convertedDateTime, timeInfo);
                }
                else
                {
                    TimeZone localZone = TimeZone.CurrentTimeZone;
                    TimeSpan currentOffset = localZone.GetUtcOffset(convertedDateTime);
                    DaylightTime daylight = localZone.GetDaylightChanges(convertedDateTime.Year);
                    convertedDateTime = convertedDateTime.Add(currentOffset);
                    //convertedDateTime = convertedDateTime.Add(daylight.Delta); // If required then later we can add
                    return convertedDateTime;
                }
            }
            return convertedDateTime;
        }

        public static DateTime GetClientTime(string date, string clientTimeZoneoffset)
        {
            if (clientTimeZoneoffset != null)
            {
                string Temp = clientTimeZoneoffset.ToString().Trim();
                if (!Temp.Contains("+") && !Temp.Contains("-"))
                {
                    Temp = Temp.Insert(0, "+");
                }
                //Retrieve all system time zones available into a collection
                ReadOnlyCollection<TimeZoneInfo> timeZones = TimeZoneInfo.GetSystemTimeZones();
                //WriteErrorLog(Directory.GetCurrentDirectory(), "DateBeforeConversion", date.ToString());
                DateTime startTime = DateTime.Parse(date);
                //WriteErrorLog(Directory.GetCurrentDirectory(), "DateAfterConversion", date.ToString());
                DateTime _now = DateTime.Parse(date);
                foreach (TimeZoneInfo timeZoneInfo in timeZones)
                {
                    if (timeZoneInfo.ToString().Contains(Temp))
                    {
                        TimeZoneInfo tst = TimeZoneInfo.FindSystemTimeZoneById(timeZoneInfo.Id);
                        try
                        {
                            _now = TimeZoneInfo.ConvertTime(startTime, TimeZoneInfo.Utc, tst);
                            // WriteErrorLog(Directory.GetCurrentDirectory(), "DateAfterTimeZoneConversion", date.ToString());
                        }
                        catch (Exception)
                        {
                            // Hack for handling Old datetime
                            _now = TimeZoneInfo.ConvertTime(startTime, TimeZoneInfo.Local, tst);
                        }
                        break;
                    }
                }
                return _now;
            }
            else
                return DateTime.Parse(date);
        }

        public static void WriteErrorLog(string folderPath, string fileName, string ex)
        {
            try
            {
                Random RndNum = new Random();
                string rootDirectoryPath = HttpContext.Current.Server.MapPath("~\\" + "ErrorLogs");
                if (!Directory.Exists(rootDirectoryPath))
                {
                    Directory.CreateDirectory(rootDirectoryPath);
                }
                string folderPathName = rootDirectoryPath + "\\" + "ErrorLogs";
                if (!Directory.Exists(folderPath))
                {
                    Directory.CreateDirectory(folderPath);
                }

                FileStream fs = new FileStream(rootDirectoryPath + "/" + fileName + RndNum.Next(1, 1000) + ".txt", FileMode.OpenOrCreate);
                StreamWriter str = new StreamWriter(fs);
                str.BaseStream.Seek(0, SeekOrigin.End);
                str.WriteLine("Message=> " + ex);
                str.WriteLine("---------------------------------");
                str.WriteLine(System.Environment.NewLine);
                str.Flush();
                str.Close();
                fs.Close();
            }
            catch (Exception)
            {
            }

        }
    }
}
