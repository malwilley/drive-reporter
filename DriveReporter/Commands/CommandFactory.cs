using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace DriveReporter.Commands
{
    public class CommandFactory : ICommandFactory
    {
        public CommandFactory() { }

        public ICommand ParseCommand(string line)
        {
            var args = line.Split(' ');

            return CreateCommand(args.First(), args.Skip(1).ToList());
        }

        private ICommand CreateCommand(string commandString, IList<string> arguments)
        {
            switch (commandString)
            {
                case "Driver":
                    return CreateDriverCommand(arguments);
                case "Trip":
                    return CreateTripCommand(arguments);
                default:
                    throw new Exception("Command must be 'Driver' or 'Trip', case sensitive");
            }
        }

        private DriverCommand CreateDriverCommand(IList<string> arguments)
        {
            if (arguments.Count() != 1)
            {
                throw new Exception("'Driver' command expects 1 argument");
            }

            return new DriverCommand(arguments.First());
        }

        private TripCommand CreateTripCommand(IList<string> arguments)
        {
            if (arguments.Count() != 4)
            {
                throw new Exception("'Trip' command expects 4 arguments");
            }

            return new TripCommand(
                driverName: arguments[0],
                startTime: DateTime.Parse(arguments[1]),
                endTime: DateTime.Parse(arguments[2]),
                milesDriven: double.Parse(arguments[3])
            );
        }
    }
}
