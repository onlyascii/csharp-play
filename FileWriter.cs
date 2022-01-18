using System;
using System.IO;

namespace Architecture
{
    public class FileWriter : IWriter<int>, IDisposable
    {
        private readonly string _fileName;
        private readonly Stream _fileStream;
        private readonly StreamWriter _streamWriter;
        private readonly ILogger _logger;

        public FileWriter(IConfig config, ILogger logger)
        {
            _fileName = config.FileWriterDestination;
            _fileStream = new FileStream(_fileName, FileMode.OpenOrCreate);
            _streamWriter = new StreamWriter(_fileStream);
            _logger = logger;
        }

        public void Dispose()
        {
            _logger.Log("disposing");
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