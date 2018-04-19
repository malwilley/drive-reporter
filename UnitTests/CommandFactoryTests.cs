using DriveReporter.Commands;
using FluentAssertions;
using System;
using System.Collections.Generic;
using System.Text;
using Xunit;

namespace UnitTests
{
    public class CommandFactoryTests
    {
        [Fact]
        public void ThrowsOnInvalidCommandName()
        {
            var factory = new CommandFactory();

            factory
                .Invoking(f => f.ParseCommand("Driverr Bob"))
                .Should().Throw<Exception>()
                .WithMessage("Command must be 'Driver' or 'Trip', case sensitive");
        }

        [Fact]
        public void ParsesValidDriverCommand()
        {
            var factory = new CommandFactory();

            var command = factory.ParseCommand("Driver Bob");

            command.Should()
                .BeOfType<DriverCommand>().Which.Name.Should()
                .Be("Bob");
        }

        [Fact]
        public void ThrowsOnInvalidDriverArguments()
        {
            var factory = new CommandFactory();

            factory
                .Invoking(f => f.ParseCommand("Driver Bob 4"))
                .Should().Throw<Exception>();
        }

        [Fact]
        public void ParsesValidTripCommand()
        {
            var factory = new CommandFactory();

            var command = factory.ParseCommand("Trip Bob 6:30 14:30 400.5");

            command.Should()
                .BeOfType<TripCommand>().Which.Should()
                .BeEquivalentTo(new TripCommand(
                    "Bob",
                    DateTime.Parse("6:30"),
                    DateTime.Parse("14:30"),
                    400.5
                ));
        }

        [Fact]
        public void ThrowsOnInvalidTripArguements()
        {
            var factory = new CommandFactory();

            factory
                .Invoking(f => f.ParseCommand("Trip Bob 6:30 14:30"))
                .Should().Throw<Exception>()
                .WithMessage("'Trip' command expects 4 arguments");
        }

        [Fact]
        public void ThrowsOnInvalidTripDates()
        {
            var factory = new CommandFactory();

            factory
                .Invoking(f => f.ParseCommand("Trip Bob 6:30 sdfs 400"))
                .Should().Throw<Exception>();
        }

        [Fact]
        public void ThrowsOnInvalidTripLength()
        {
            var factory = new CommandFactory();

            factory
                .Invoking(f => f.ParseCommand("Trip Bob 6:30 14:30 400.q"))
                .Should().Throw<Exception>();
        }
    }
}
