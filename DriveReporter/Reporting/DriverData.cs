using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace DriveReporter.Reporting
{
    /// <summary>
    /// Simple data class containing milage and time data for a single driver
    /// </summary>
    public class DriverData
    {
        public string Name { get; }
        public double MilesDriven { get; private set; }
        public double HighwayMilesDriven { get; private set; }
        public TimeSpan TimeDriven { get; private set; }
        public IList<InvalidTrip> InvalidTrips { get; private set; }

        public DriverData(string name)
        {
            Name = name;
            MilesDriven = 0;
            TimeDriven = TimeSpan.Zero;
            InvalidTrips = new List<InvalidTrip>();
        }

        public DriverData(string name, double milesDriven, double highWayMilesDriven, TimeSpan timeDriven, IEnumerable<InvalidTrip> invalidTrips)
        {
            Name = name;
            MilesDriven = milesDriven;
            HighwayMilesDriven = highWayMilesDriven;
            TimeDriven = timeDriven;
            InvalidTrips = invalidTrips.ToList();
        }

        public DriverData AddTrip(double miles, double highwayMiles, TimeSpan time)
        {
            MilesDriven += miles;
            TimeDriven += time;
            HighwayMilesDriven += highwayMiles;

            return this;
        }

        public DriverData AddInvalidTrip(double miles, DateTime start, DateTime end)
        {
            InvalidTrips.Add(new InvalidTrip(miles, start, end));

            return this;
        }
    }
}
