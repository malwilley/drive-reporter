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
    public class InputFileReaderTests
    {
        private FileInfo _testCommandsFile = 
            new FileInfo(Path.Combine("Input Files", "TestCommands.txt"));

        [Fact]
        public void ReadsCommandsFromFile()
        {
            var factoryMock = new Mock<ICommandFactory>();
            factoryMock
                .Setup(f => f.ParseCommand(It.Is<string>(c => c == "Driver Bob")))
                .Returns(new DriverCommand("Bob"));
            factoryMock
                .Setup(f => f.ParseCommand(It.Is<string>(c => c == "Driver Alice")))
                .Returns(new DriverCommand("Alice"));

            var inputReader = new InputFileReader(factoryMock.Object);

            inputReader.ReadCommands(_testCommandsFile).Should()
                .BeEquivalentTo(new[]
                {
                    new DriverCommand("Bob"),
                    new DriverCommand("Alice")
                });
        }
    }
}
