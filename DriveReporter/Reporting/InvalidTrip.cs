using System;
using System.Collections.Generic;
using System.Text;

namespace DriveReporter.Reporting
{
    public class InvalidTrip
    {
        public double MilesDriven { get; set; }
        public DateTime TimeStart { get; set; }
        public DateTime TimeEnd { get; set; }

        public InvalidTrip(double miles, DateTime start, DateTime end)
        {
            MilesDriven = miles;
            TimeStart = start;
            TimeEnd = end;
        }
    }
}
