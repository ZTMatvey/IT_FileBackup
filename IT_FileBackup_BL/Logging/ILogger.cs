using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IT_FileBackup_BL.Logging
{
    public interface ILogger
    {
        public void Log(string message, LogLevel messageLogLevel);
    }
}
