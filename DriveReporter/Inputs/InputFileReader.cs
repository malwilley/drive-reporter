using DriveReporter.Commands;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;

namespace DriveReporter.Inputs
{
    public class InputFileReader : IInputFileReader
    {
        private readonly ICommandFactory _commandFactory;

        public InputFileReader(ICommandFactory commandFactory)
        {
            _commandFactory = commandFactory;
        }

        /// <summary>
        /// Reads the given file an parses each line into a command
        /// </summary>
        public IEnumerable<ICommand> ReadCommands(FileInfo file)
        {
            if (!file.Exists)
            {
                throw new FileNotFoundException($"No file found: {file.FullName}");
            }

            var lines = File.ReadAllLines(file.FullName);

            return lines.Select(_commandFactory.ParseCommand);
        }
    }
}
