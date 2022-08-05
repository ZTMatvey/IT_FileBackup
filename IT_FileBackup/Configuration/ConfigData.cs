using IT_FileBackup_BL.Logging;

namespace IT_FileBackup.Configuration
{
    public sealed class ConfigData
    {
        public string[] SourceFolders { get; set; }
        public string TargetFolder { get; set; }
        public LogLevel LogLevel { get; set; }
    }
}
