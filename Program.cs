namespace Architecture
{
    public interface IDataProvider<T>
    {
        IEnumerable<T> GetData();
    }

    public class RandomIntProvider : IDataProvider<int>
    {
        private readonly int _maxValue;
        private readonly int _count;
        private readonly Random _random;

        public RandomIntProvider(int maxValue, int count)
        {
            _maxValue = maxValue;
            _count = count;
            _random = new Random();
        }

        public IEnumerable<int> GetData()
        {
            for(int i=0; i<_count; i++ )
            {
                yield return _random.Next(_maxValue);
            }
        }
    }

    public interface IManipulator<T, V>
    {
        V Manipulate(T data);
    }

    public class Squarer : IManipulator<int, int>
    {

        public int Manipulate(int data)
        {
            return data * data;
        }
    }

    public class ConstantMultiplier : IManipulator<int, int>
    {
        private readonly int _constantValue;

        public ConstantMultiplier(int constantValue)
        {
            _constantValue = constantValue;
        }

        public int Manipulate(int data)
        {
            return data * _constantValue;
        }
    }

    public interface IWriter<T>
    {
        void Write(T data);
    }

    public class LogWriter : IWriter<int>, IDisposable
    {
        private readonly string _fileName;
        private readonly Stream _fileStream;
        private readonly StreamWriter _streamWriter;

        public LogWriter(string fileName)
        {
            _fileName = $"logs/{fileName}";
            Directory.CreateDirectory("logs");
            _fileStream = new FileStream(_fileName, FileMode.OpenOrCreate);
            _streamWriter = new StreamWriter(_fileStream);
        }

        public void Dispose()
        {
            _streamWriter.Flush();
            _streamWriter.Dispose();
            _fileStream.Dispose();
        }

        public void Write(int data)
        {
            _streamWriter.WriteLine(data);
        }
    }

    class Processor
    {
        private readonly IDataProvider<int> _dataProvider;
        private readonly IManipulator<int, int> _manipulator;
        private readonly IWriter<int> _writer;

        public Processor(IDataProvider<int> dataProvider, IManipulator<int, int> manipulator, IWriter<int> writer)
        {
            _dataProvider = dataProvider ?? throw new ArgumentNullException(nameof(dataProvider));
            _manipulator = manipulator ?? throw new ArgumentNullException(nameof(manipulator));
            _writer = writer ?? throw new ArgumentNullException(nameof(writer));
        }

        public void Process()
        {
            foreach(int i in _dataProvider.GetData())
            {
                int result = _manipulator.Manipulate(i);
                _writer.Write(result);
            }
        }
    }


    class Program
    {
        static void Main(string[] args)
        {
            using(var writer = new LogWriter("ints.txt"))
            {
                IDataProvider<int> dataProvider = new RandomIntProvider(maxValue: 100, count: 10);
                //IManipulator<int, int> manipulator = new Squarer();
                IManipulator<int, int> manipulator = new ConstantMultiplier(constantValue: 20);
                Processor processor = new Processor(dataProvider, manipulator, writer);

                processor.Process();

            }
        }
    }
}