using DriveReporter.Commands;
using DriveReporter.Inputs;
using DriveReporter.Reporting;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;

namespace DriveReporter.Reporting
{
    public class DriveReporterService
    {
        private readonly IInputFileReader _inputFileReader;

        public DriveReporterService(IInputFileReader inputFileReader)
        {
            _inputFileReader = inputFileReader;
        }

        /// <summary>
        /// Reads commands from the given input file and generates a report
        /// </summary>
        public string GenerateReport(FileInfo inputCommandFile)
        {
            var commands = _inputFileReader.ReadCommands(inputCommandFile);

            return GenerateReport(commands);
        }

        /// <summary>
        /// Processes the given commands and generates a report from the 
        /// resulting driver data
        /// </summary>
        private string GenerateReport(IEnumerable<ICommand> commands)
        {
            var finalReport = commands
                .Aggregate<ICommand, IDriveReport>(new DriveReport(), (report, nextCommand) => nextCommand.Process(report));

            return GenerateReport(finalReport);
        }

        /// <summary>
        /// Generates summary lines for the given report and orders by most
        /// miles driven
        /// </summary>
        private string GenerateReport(IDriveReport driveReport)
        {
            var reportLines = driveReport.AllDriverData
                .OrderByDescending(d => d.MilesDriven)
                .Select(d => GenerateReportLine(d));

            return string.Join('\n', reportLines);
        }

        /// <summary>
        /// Summarizes the given driver data
        /// </summary>
        private string GenerateReportLine(DriverData driverData)
        {
            var roundedMiles = Math.Round(driverData.MilesDriven, 0);
            var driverMiles = $"{driverData.Name}: {roundedMiles} miles";

            var line = driverMiles + (roundedMiles > 0
                ? $" @ {GetRoundedSpeedMph(driverData.MilesDriven, driverData.TimeDriven)} mph"
                : "");

            line = line + (roundedMiles > 0
                ? $", {GetPercentHighway(driverData.MilesDriven, driverData.HighwayMilesDriven)}% highway"
                : "");

            if (driverData.InvalidTrips.Any())
            {
                line += '\n';
                line += GenerateDriverInvalidTrips(driverData);
            }

            return line;
        }

        private string GenerateDriverInvalidTrips(DriverData driverData)
        {
            var header = $"Invalid {driverData.Name} trips";
            var lines = new[]
            {
                new[] { header },
                driverData.InvalidTrips.Select(t => GenerateDRiverInvalidTripLine(t))
            }.SelectMany(l => l);

            return string.Join('\n', lines);
        }

        private string GenerateDRiverInvalidTripLine(InvalidTrip trip)
        {
            return $"  {trip.MilesDriven} miles {TimeString(trip.TimeStart)} {TimeString(trip.TimeEnd)}";
        }

        private string TimeString(DateTime time)
        {
            var minutes = time.Minute >= 10
                ? time.Minute.ToString()
                : "0" + time.Minute;

            return $"{time.Hour}:{minutes}";
        }

        private double GetRoundedSpeedMph(double milesDriven, TimeSpan timeDriven)
        {
            return Math.Round(milesDriven / timeDriven.TotalHours, 0);
        }

        private double GetPercentHighway(double totalMiles, double highwayMiles)
        {
            return Math.Round(100 * (highwayMiles / totalMiles), 0);
        }
    }
}
