using DriveReporter.Commands;
using DriveReporter.Inputs;
using DriveReporter.Reporting;
using SimpleInjector;
using System;
using System.IO;
using System.Linq;

namespace DriveReporter
{
    class Program
    {
        /// <summary>
        /// Entrypoint for the application
        /// </summary>
        /// <param name="args"></param>
        static void Main(string[] args)
        {
            if (args.Length != 1)
            {
                Console.WriteLine("Please call with a single file argument");
                return;
            }

            var filePath = args.First();

            var driveReporter = GetDriveReporter();

            try
            {
                var report = driveReporter.GenerateReport(new FileInfo(filePath));
                Console.WriteLine(report);
            }
            catch (Exception e)
            {
                Console.WriteLine($"There is a problem with your input:\n{e.Message}");
            }
        }

        /// <summary>
        /// Registers all dependencies in a IoC container and ceates a 
        /// DriveReporterService 
        /// </summary>
        static DriveReporterService GetDriveReporter()
        {
            var container = new Container();

            container.Register<ICommandFactory, CommandFactory>();
            container.Register<IInputFileReader, InputFileReader>();
            container.Register<DriveReporterService>();

            container.Verify();

            return container.GetInstance<DriveReporterService>();
        }
    }
}
