namespace Architecture
{
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