using System;
using System.Collections.Generic;
using System.Text;
using DriveReporter.Reporting;

namespace DriveReporter.Commands
{
    public class TripCommand : ICommand
    {
        public string DriverName { get; }
        public DateTime StartTime { get; }
        public DateTime EndTime { get; }
        public double MilesDriven { get; }

        public TripCommand(string driverName, DateTime startTime, DateTime endTime, double milesDriven)
        {
            DriverName = driverName;
            StartTime = startTime;
            EndTime = endTime;
            MilesDriven = milesDriven;
        }

        /// <summary>
        /// Adds this trip to the given drive report if it is valid and returns
        /// the modifed report object
        /// </summary>
        public IDriveReport Process(IDriveReport driveReport)
        {
            const double minTripSpeed = 5.0;
            const double maxTripSpeed = 100.0;
            var timeDriven = EndTime - StartTime;
            var tripSpeed = MilesDriven / timeDriven.TotalHours;

            if (timeDriven <= TimeSpan.Zero ||
                tripSpeed < minTripSpeed ||
                tripSpeed > maxTripSpeed)
            {
                driveReport.AddInvalidTrip(DriverName, MilesDriven, StartTime, EndTime);

                return driveReport;
            }

            var highwayMiles = tripSpeed >= 60.0
                ? MilesDriven
                : 0.0;

            return driveReport.AddTrip(DriverName, MilesDriven, highwayMiles, timeDriven);
        }
    }
}
