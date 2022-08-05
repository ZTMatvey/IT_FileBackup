using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IT_FileBackup_BL.Logging
{
    public sealed class FileLogger : ILogger
    {
        private readonly string _filePath;
        private readonly LogLevel _logLevel;
        public FileLogger(string fileLocation, LogLevel logLevel)
        {
            _filePath =  $@"{fileLocation}/log.txt";
            _logLevel = logLevel;
        }
        public void Log(string message, LogLevel messageLogLevel)
        {
            if (messageLogLevel == LogLevel.None || messageLogLevel > _logLevel)
                return;
            if (!File.Exists(_filePath))
                File.Create(_filePath).Dispose();
            using var streamWriter = new StreamWriter(_filePath, true, Encoding.UTF8);
            streamWriter.WriteLine($"{DateTime.UtcNow.ToString("yyyy-MM-dd HH:mm:ss.fff")}: {message}");
        }
    }
}
