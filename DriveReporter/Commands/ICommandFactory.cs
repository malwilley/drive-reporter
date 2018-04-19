using System;
using System.Collections.Generic;
using System.Text;

namespace DriveReporter.Commands
{
    public interface ICommandFactory
    {
        ICommand ParseCommand(string line);
    }
}
