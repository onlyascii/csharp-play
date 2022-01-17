namespace Architecture
{
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
}