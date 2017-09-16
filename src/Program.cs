using CsvHelper;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace MarketViolationsCodingInterview
{
    public class MarketData
    {
        public DateTime Time { get; set; }
        public double BestBid { get; set; }
        public double BestAsk { get; set; }
        public double? Bid { get; set; }
        public double? Ask { get; set; }
    }

    class Program
    {
        static readonly DateTime MarketOpen = DateTime.Parse("9:30 AM");
        static readonly DateTime MarketClose = DateTime.Parse("4:00 PM");

        /// <summary>
        /// Function to detect violation. 
        /// 
        /// If the Bid or Ask is missing, then firm is in violation
        /// If the Bid is >10% less than BestBid, then firm is in violation
        /// If the Ask is >10% greater than BestAsk, then firm is in violation
        /// </summary>
        public static bool IsViolation(MarketData md)
        {
            if (md.Bid == null || md.Ask == null)
                return true;
            if (md.Bid < (md.BestBid * .9))         
                return true;
            if (md.Ask > (md.BestAsk * 1.10))
                return true;
            return false;
        }

        /// <summary>
        /// Calculates the total minutes in violation for the specified time range
        /// </summary>
        public static int TotalMinutesViolation(IEnumerable<MarketData> data, DateTime start, DateTime end)
        {
            if (data == null)
                throw new ApplicationException("data is missing");
            if (start.Date != end.Date)
                throw new ApplicationException("Range is spanning across days");

            // Stay within market hours
            if (start < MarketOpen)
                start = MarketOpen;
            if (end > MarketClose)
                end = MarketClose;

            bool violation = false;
            DateTime startViolation = start;
            int totalMinutes = 0;

            foreach(var r in data)
            {
                if (r.Time < start)
                {
                    violation = IsViolation(r);
                    continue;
                }

                if (r.Time > end)
                    break;

                if (IsViolation(r) && !violation)
                {
                    startViolation = r.Time;
                    violation = true;
                }
                else if (!IsViolation(r) && violation)
                {
                    totalMinutes += (int) r.Time.Subtract(startViolation).TotalMinutes;
                    violation = false;
                }
            }

            // Was missing from whiteboard
            // Covers case when data is done, but still in violation
            if (violation)
                totalMinutes += (int) end.Subtract(startViolation).TotalMinutes;

            return totalMinutes;
        }

        /// <summary>
        /// Helper function to load marketdata from csv file
        /// </summary>
        public static IEnumerable<MarketData> LoadDataStream(string file)
        {
            using (var stream = new StreamReader(file))
            using (var csv = new CsvReader(stream))
            {
                csv.Configuration.TrimHeaders = true;
                csv.Configuration.TrimFields = true;
                csv.Configuration.WillThrowOnMissingField = false;
                foreach (var i in csv.GetRecords<MarketData>())
                    yield return i;
            }
            yield break;
        }

        static void Main(string[] args)
        {
            var testTimes = new List<Tuple<string, string>>();
            testTimes.Add(Tuple.Create("9:30", "9:36"));
            testTimes.Add(Tuple.Create("9:35", "9:42"));
            testTimes.Add(Tuple.Create("9:42", "9:46"));
            testTimes.Add(Tuple.Create("9:40", "9:54"));
            testTimes.Add(Tuple.Create("10:12", "10:13"));
            testTimes.Add(Tuple.Create("10:16", "10:20"));
            testTimes.Add(Tuple.Create("10:16", "16:00"));
            testTimes.Add(Tuple.Create("10:16", "17:30"));

            foreach(var t in testTimes)
            {
                var data = LoadDataStream("marketdata.txt");
                var start = DateTime.Parse(t.Item1);
                var end = DateTime.Parse(t.Item2);
                int minutes = TotalMinutesViolation(data, start, end);
                Console.WriteLine("[{0:hh:mm}, {1:hh:mm}] {2} min", start, end, minutes);
            }
        }
    }
}
