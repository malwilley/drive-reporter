using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace DriveReporter.Reporting
{
    public class DriveReport : IDriveReport
    {
        /// <summary>
        /// Underlying data structure is a dictionary map of driver names to
        /// their associated data
        /// </summary>
        private readonly IDictionary<string, DriverData> _allDriverData;
        public IEnumerable<DriverData> AllDriverData => _allDriverData.Select(kvp => kvp.Value);

        public DriveReport()
        {
            _allDriverData = new Dictionary<string, DriverData>();
        }

        public DriveReport(IEnumerable<DriverData> allDriverData)
        {
            _allDriverData = allDriverData.ToDictionary(d => d.Name, d => d);
        }

        /// <summary>
        /// Registers a new driver for the report. Must not have been
        /// registered previously.
        /// </summary>
        /// <param name="driverName">Name to register</param>
        /// <returns>The modified report class</returns>
        public IDriveReport AddDriver(string driverName)
        {
            if (_allDriverData.ContainsKey(driverName))
            {
                throw new Exception($"Driver {driverName} already registered");
            }

            _allDriverData.Add(driverName, new DriverData(driverName));

            return this;
        }

        /// <summary>
        /// Adds miles and time to the given driver.
        /// </summary>
        /// <param name="driverName">Driver to log this trip for</param>
        /// <param name="milesDriven">Number of miles driven in the trip</param>
        /// <param name="timeDriven">Total time driven in the trip</param>
        /// <returns>The modified report class</returns>
        public IDriveReport AddTrip(string driverName, double milesDriven, double highwayMilesDriven, TimeSpan timeDriven)
        {
            AssertHasDriver(driverName);

            _allDriverData[driverName].AddTrip(
                miles: milesDriven,
                highwayMiles: highwayMilesDriven,
                time: timeDriven
            );

            return this;
        }

        public IDriveReport AddInvalidTrip(string driverName, double milesDriven, DateTime startTime, DateTime endTime)
        {
            AssertHasDriver(driverName);

            _allDriverData[driverName].AddInvalidTrip(
                miles: milesDriven,
                start: startTime,
                end: endTime
            );

            return this;
        }

        private void AssertHasDriver(string driverName)
        {
            if (!_allDriverData.ContainsKey(driverName))
            {
                throw new Exception($"Cannot add trip for unregistered driver {driverName}");
            }
        }
    }
}
