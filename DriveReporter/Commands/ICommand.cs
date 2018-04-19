using DriveReporter.Reporting;
using System;
using System.Collections.Generic;
using System.Text;

namespace DriveReporter.Commands
{
    public interface ICommand
    {
        IDriveReport Process(IDriveReport driveReport);
    }
}
