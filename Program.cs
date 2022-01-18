using System;
using CommandLine;
using Microsoft.Extensions.DependencyInjection;

namespace Architecture
{
    public interface IOptions 
    {
        public string FileWriterDestination { get; }
        public int MaxRandomInt { get; }
        public int RandomIntCount { get; }
        public int ConstantValue { get; }


    }
    public class Options : IOptions
    {
        [Option('f', "filename", Required = false, Default = "file.txt", HelpText ="filename to write to")]
        public string FileWriterDestination { get; set; }
        [Option('r', "max", Required = false, Default = 5000, HelpText ="max random number")]
        public int MaxRandomInt { get; set; }
        [Option('c', "count", Required = false, Default = 20, HelpText ="numbers to process")]
        public int RandomIntCount { get; set; }
        [Option('v', "constant", Required = false, Default = 100, HelpText ="value to multiply by")]
        public int ConstantValue { get; set; }

    }
    class Processor
    {
        private readonly IDataProvider<int> _dataProvider;
        private readonly IManipulator<int, int> _manipulator;
        private readonly IWriter<int> _writer;
        private readonly ILogger _logger;

        public Processor(IDataProvider<int> dataProvider, IManipulator<int, int> manipulator, IWriter<int> writer, ILogger logger)
        {
            _dataProvider = dataProvider ?? throw new ArgumentNullException(nameof(dataProvider));
            _manipulator = manipulator ?? throw new ArgumentNullException(nameof(manipulator));
            _writer = writer ?? throw new ArgumentNullException(nameof(writer));
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
        }

        public void Process()
        {
            foreach(int i in _dataProvider.GetData())
            {
                _logger.Log($"in: {i}");
                int result = _manipulator.Manipulate(i);
                _logger.Log($"out: {result}");

                _writer.Write(result);
            }
        }
    }

    public interface ILogger
    {
        void Log(string message);
    }

    public class Logger : ILogger
    {
        public void Log(string message)
        {
            Console.WriteLine($"{DateTime.Now.ToShortTimeString()} --> {message}");
        }
    }
    class Program
    {
        static void Main(string[] args)
        {
            Parser.Default.ParseArguments<IOptions>(args).WithParsed<IOptions>(o => {
                IServiceProvider serviceProvider = BuildServiceProvider(options: o);
                Processor processor = serviceProvider.GetService<Processor>();
                processor.Process();
            });
        }

        static IServiceProvider BuildServiceProvider(IOptions options)
        {
            IServiceCollection collection = new ServiceCollection();

            collection.AddSingleton<IOptions>(options);
            collection.AddTransient<ILogger, Logger>();
            collection.AddSingleton<IDataProvider<int>, RandomIntProvider>();
            collection.AddTransient<IManipulator<int, int>, ConstantMultiplier>();
            //collection.AddTransient<IManipulator<int, int>, Squarer>();
            collection.AddSingleton<IWriter<int>, FileWriter>();
            collection.AddSingleton<Processor>();

            return collection.BuildServiceProvider();
        }
    }
}