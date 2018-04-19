using DriveReporter.Commands;
using DriveReporter.Inputs;
using DriveReporter.Reporting;
using FluentAssertions;
using Moq;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using Xunit;

namespace UnitTests
{
    public class DriveReporterServiceTests
    {
        [Fact]
        public void GeneratesReport()
        {
            var mockCommand = new Mock<ICommand>();
            mockCommand
                .Setup(c => c.Process(It.IsAny<IDriveReport>()))
                .Returns(new DriveReport(new[]
                {
                    new DriverData("Bob", 10.6, 2, TimeSpan.FromMinutes(30), new [] { new InvalidTrip(10, DateTime.Parse("1:00"), DateTime.Parse("1:01")) }),
                    new DriverData("Alice", 0.0, 0.0, TimeSpan.Zero, new InvalidTrip[] { }),
                    new DriverData("Sally", 0.2, 0.0, TimeSpan.FromMinutes(1), new InvalidTrip[] { })
                }));

            var mockInputReader = new Mock<IInputFileReader>();
            mockInputReader
                .Setup(i => i.ReadCommands(It.IsAny<FileInfo>()))
                .Returns(new[]
                {
                    mockCommand.Object
                });

            var driveReporter = new DriveReporterService(mockInputReader.Object);

            var expectedReportOutput = string.Join('\n', new[]
            {
                "Bob: 11 miles @ 21 mph, 19% highway",
                "Invalid Bob trips",
                "  10 miles 1:00 1:01",
                "Sally: 0 miles",
                "Alice: 0 miles"
            });

            driveReporter.GenerateReport(new FileInfo("test"))
                .Should().Be(expectedReportOutput);
        }
    }
}
