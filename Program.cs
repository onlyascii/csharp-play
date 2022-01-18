using System;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace Architecture
{
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

    public interface IConfig
    {
        string FileWriterDestination { get; }
        int MaxRandomInt { get; }
        int RandomIntCount { get; }
        int ConstantValue { get; }
    }

    public class Config : IConfig
    {
        public string FileWriterDestination { get; set; }
        public int MaxRandomInt { get; set; }
        public int RandomIntCount { get; set; }
        public int ConstantValue { get; set; }
    }

    class Program
    {
        static void Main(string[] args)
        {
            //using(var writer = new LogWriter("ints.txt"))
            //{
            //IDataProvider<int> dataProvider = new RandomIntProvider(maxValue: 100, count: 10);
            ////IManipulator<int, int> manipulator = new Squarer();
            //IManipulator<int, int> manipulator = new ConstantMultiplier(constantValue: 20);
            //Processor processor = new Processor(dataProvider, manipulator, writer);

            //processor.Process();


            //}
            IServiceProvider serviceProvider = BuildServiceProvider(args: args);
            Processor processor = serviceProvider.GetService<Processor>();
            processor.Process();
        }

        static IServiceProvider BuildServiceProvider(string[] args)
        {
            IServiceCollection collection = new ServiceCollection();

            IConfiguration configuration = new ConfigurationBuilder()
                .AddJsonFile("appSettings.json", optional: false)
                .AddCommandLine(args: args)
                .Build();

            IConfig config = configuration.Get<Config>();

            collection.AddSingleton<IConfig>(config);
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