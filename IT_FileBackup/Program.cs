using IT_FileBackup.Configuration;
using IT_FileBackup_BL;
using IT_FileBackup_BL.Logging;

var config = Config.GetInstance();
using (var backupSession = new BackupSession(config.TargetFolder))
{
    var logger = new FileLogger(backupSession.DestinationFolder, config.LogLevel);
    backupSession.SetupLogger(logger);
    backupSession.Backup(config.SourceFolders);
}
