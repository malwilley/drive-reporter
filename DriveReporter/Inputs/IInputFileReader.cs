using DriveReporter.Commands;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace DriveReporter.Inputs
{
    public interface IInputFileReader
    {
        IEnumerable<ICommand> ReadCommands(FileInfo file);
    }
}
