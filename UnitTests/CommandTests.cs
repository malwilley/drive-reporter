using DriveReporter.Commands;
using DriveReporter.Reporting;
using Moq;
using System;
using Xunit;

namespace UnitTests
{
    public class CommandTests
    {
        [Fact]
        public void ProcessesAddDriverCommand()
        {
            var mockReport = new Mock<IDriveReport>();

            var command = new DriverCommand("Bob");
            command.Process(mockReport.Object);

            mockReport.Verify(r => r.AddDriver(It.Is<string>(name => name == "Bob")), Times.Once);
        }

        [Fact]
        public void ProcessesValidAddTripCommand()
        {
            var mockReport = new Mock<IDriveReport>();

            var command = new TripCommand("Bob", DateTime.Now, DateTime.Now + TimeSpan.FromHours(2), 100);
            command.Process(mockReport.Object);

            mockReport.Verify(
                r => r.AddTrip(
                    It.Is<string>(name => name == command.DriverName),
                    It.Is<double>(miles => miles == command.MilesDriven),
                    It.Is<double>(highwayMiles => highwayMiles == 0.0),
                    It.Is<TimeSpan>(time => time == command.EndTime - command.StartTime)
                ), Times.Once);
        }

        [Fact]
        public void ProcessesValidAddTripCommandHighway()
        {
            var mockReport = new Mock<IDriveReport>();

            var command = new TripCommand("Bob", DateTime.Now, DateTime.Now + TimeSpan.FromHours(2), 150);
            command.Process(mockReport.Object);

            mockReport.Verify(
                r => r.AddTrip(
                    It.Is<string>(name => name == command.DriverName),
                    It.Is<double>(miles => miles == command.MilesDriven),
                    It.Is<double>(highwayMiles => highwayMiles == 150),
                    It.Is<TimeSpan>(time => time == command.EndTime - command.StartTime)
                ), Times.Once);
        }

        private bool IsHighwayMileage(double miles, TimeSpan span)
        {
            return miles / span.Hours >= 60;
        }

        [Fact]
        public void DoesNotProcessInvalidAddTripCommand()
        {
            var mockReport = new Mock<IDriveReport>();

            var start = DateTime.Now;

            var tooSlowCommand = new TripCommand("1", start, start + TimeSpan.FromHours(2), 5);
            var tooFastCommand = new TripCommand("2", start, start + TimeSpan.FromHours(2), 1000);
            var negativeDistanceCommand = new TripCommand("3", start, start + TimeSpan.FromHours(2), -50);
            var negativeTimespanCommand = new TripCommand("4", start, start - TimeSpan.FromHours(2), 1000);
            var bothNegativeCommand = new TripCommand("5", start, start - TimeSpan.FromHours(2), 1000);

            tooSlowCommand.Process(mockReport.Object);
            tooFastCommand.Process(mockReport.Object);
            negativeDistanceCommand.Process(mockReport.Object);
            negativeTimespanCommand.Process(mockReport.Object);
            bothNegativeCommand.Process(mockReport.Object);

            mockReport.Verify(
                r => r.AddTrip(
                    It.IsAny<string>(),
                    It.IsAny<double>(),
                    It.IsAny<double>(),
                    It.IsAny<TimeSpan>()
                ), Times.Never);

            mockReport.Verify(
                r => r.AddInvalidTrip(
                    It.Is<string>(n => n == "1"),
                    It.Is<double>(m => m == 5),
                    It.Is<DateTime>(t => t == start),
                    It.Is<DateTime>(t => t.Equals(start + TimeSpan.FromHours(2)))
                ), Times.Once
            );
        }
    }
}
