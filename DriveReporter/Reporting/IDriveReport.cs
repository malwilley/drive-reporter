using System;
using System.Collections.Generic;
using System.Text;

namespace DriveReporter.Reporting
{
    public interface IDriveReport
    {
        IEnumerable<DriverData> AllDriverData { get; }

        IDriveReport AddDriver(string driverName);

        IDriveReport AddTrip(string driverName, double milesDriven, double highwayMilesDriven, TimeSpan timeDriven);

        IDriveReport AddInvalidTrip(string driverName, double milesDriven, DateTime startTime, DateTime endTime);
    }
}
