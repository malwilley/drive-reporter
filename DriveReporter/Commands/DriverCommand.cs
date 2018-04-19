using System;
using System.Collections.Generic;
using System.Text;
using DriveReporter.Reporting;

namespace DriveReporter.Commands
{
    public class DriverCommand : ICommand
    {
        public string Name { get; }

        public DriverCommand(string name)
        {
            Name = name;
        }

        /// <summary>
        /// Adds this driver to the given drive report and returns the modified
        /// report object
        /// </summary>
        public IDriveReport Process(IDriveReport driveReport)
        {
            return driveReport.AddDriver(Name);
        }
    }
}
