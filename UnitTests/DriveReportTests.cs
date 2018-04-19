using DriveReporter.Reporting;
using FluentAssertions;
using System;
using System.Collections.Generic;
using System.Text;
using Xunit;

namespace UnitTests
{
    public class DriveReportTests
    {
        [Fact]
        public void AddsUnregisteredDrivers()
        {
            var driveReport = new DriveReport();

            driveReport.AddDriver("1");
            driveReport.AddDriver("2");
            driveReport.AddDriver("3");

            driveReport.AllDriverData.Should().BeEquivalentTo(new[]
            {
                new DriverData("1"),
                new DriverData("2"),
                new DriverData("3")
            });
        }

        [Fact]
        public void ThrowsOnDuplicateDriver()
        {
            var driveReport = new DriveReport();

            driveReport.AddDriver("1");
            driveReport.AddDriver("2");
            driveReport.AddDriver("3");

            driveReport
                .Invoking(d => d.AddDriver("2"))
                .Should().Throw<Exception>()
                .WithMessage("Driver 2 already registered");
        }

        [Fact]
        public void ThrowsOnTripWithNoDriver()
        {
            var driveReport = new DriveReport();
            
            driveReport
                .Invoking(d => d.AddTrip("1", 10, 10, TimeSpan.FromMinutes(20)))
                .Should().Throw<Exception>()
                .WithMessage("Cannot add trip for unregistered driver 1");
        }

        [Fact]
        public void AddsValidTrips()
        {
            var driveReport = new DriveReport();

            driveReport.AddDriver("1");
            driveReport.AddTrip("1", 10, 0.0, TimeSpan.FromMinutes(20));

            driveReport.AddDriver("2");
            driveReport.AddTrip("1", 50, 50.0, TimeSpan.FromMinutes(60));
            driveReport.AddTrip("2", 20, 0.0, TimeSpan.FromMinutes(30));

            driveReport.AddDriver("3");

            driveReport.AllDriverData.Should().BeEquivalentTo(new[]
            {
                new DriverData("1", 60, 50.0, TimeSpan.FromMinutes(80), new InvalidTrip[]{ }),
                new DriverData("2", 20, 0.0, TimeSpan.FromMinutes(30), new InvalidTrip[]{ }),
                new DriverData("3", 0, 0, TimeSpan.Zero, new InvalidTrip[]{ })
            });
        }

        [Fact]
        public void AddsInvalidTrips()
        {
            var driveReport = new DriveReport();

            driveReport.AddDriver("1");
            driveReport.AddInvalidTrip("1", 10, DateTime.Parse("1:00"), DateTime.Parse("1:01"));

            driveReport.AllDriverData.Should().BeEquivalentTo(new[]
            {
                new DriverData("1", 0, 0, TimeSpan.Zero, new[] { new InvalidTrip(10, DateTime.Parse("1:00"), DateTime.Parse("1:01")) })
            });
        }
    }
}
