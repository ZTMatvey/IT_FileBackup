using IT_FileBackup_BL.Logging;
using System.IO.Compression;

namespace IT_FileBackup_BL
{
    public class BackupSession: IDisposable
    {
        public readonly string DestinationFolder;
        private readonly string _targetFolder;
        private ILogger _logger;
        public BackupSession(string targetFolder)
        {
            var destinationFolder = $"{targetFolder}/backup [{DateTime.Now.ToString("yyyyMMddHHmmssfff")}]";
            DestinationFolder = destinationFolder;
            if (Directory.Exists(DestinationFolder))
                throw new Exception("Error. Directory with backup time already exists. Try again");
            Directory.CreateDirectory(destinationFolder);
            _targetFolder = targetFolder;
        }
        public void SetupLogger(ILogger logger)
        {
            _logger = logger;
            _logger?.Log("Журналирование начато", LogLevel.Info);
        }
        public void Backup(string sourceFolder)
        {
            if (!Directory.Exists(sourceFolder))
            {
                _logger?.Log($"Ошибка! Исходная папка \"{sourceFolder}\" не существует", LogLevel.Error);
                return;
            }
            _logger?.Log($"Начато копирование папки \"{sourceFolder}\"", LogLevel.Debug);
            var sourceFolderName = Path.GetFileName(sourceFolder);
            var folderToBackup = CreateDirectoryInDestinationFolder(sourceFolderName);
            CopyAllDirectories(sourceFolder, folderToBackup);
            CopyAllFiles(sourceFolder, folderToBackup);
        }
        private void CopyAllDirectories(string sourceFolder, string folderToBackup)
        {
            var directoriesInSource = SafelyIO.GetAllDirectories(sourceFolder);
            foreach (var directoryInfo in directoriesInSource)
            {
                if (directoryInfo.HasAccess)
                {
                    var backupFolderLocation = directoryInfo.Path.Replace(sourceFolder, folderToBackup);
                    Directory.CreateDirectory(backupFolderLocation);
                    _logger?.Log($"Папка \"{directoryInfo.Path}\" успешно скопирована", LogLevel.Debug);
                }
                else
                    _logger?.Log($"Ошибка при копировании папки \"{directoryInfo.Path}\", вызванная ограничениями доступа", LogLevel.Error);
            }
        }
        private void CopyAllFiles(string sourceFolder, string folderToBackup)
        {
            var directoriesInSource = SafelyIO.GetAllDirectories(sourceFolder);
            var files = new List<string>();
            foreach (var directoryInfo in directoriesInSource)
                if(directoryInfo.HasAccess)
                    files.AddRange(Directory.GetFiles(directoryInfo.Path));
            foreach (string file in files)
            {
                var backupFileLocation = file.Replace(sourceFolder, folderToBackup);
                var result = SafelyIO.TryToCopyFile(file, backupFileLocation);
                var fileName = Path.GetFileName(file);
                if(result)
                    _logger?.Log($"Файл \"{fileName}\" успешно скопирован", LogLevel.Debug);
                else
                    _logger?.Log($"Ошибка при копировании файла \"{fileName}\", вызванная ограничениями доступа", LogLevel.Error);
            }
        }
        private string CreateDirectoryInDestinationFolder(string directoryName)
        {
            var directoryNameWithIndex = (int index) =>
            {
                if (index == 0)
                    return $@"{DestinationFolder}/{directoryName}";
                return $@"{DestinationFolder}/{directoryName} ({index + 1})";
            };
            var directoryIndex = 0;
            while (true)
            {
                var finalDirectoryNameSuggestion = directoryNameWithIndex(directoryIndex);
                if (!Directory.Exists(finalDirectoryNameSuggestion))
                {
                    Directory.CreateDirectory(finalDirectoryNameSuggestion);
                    return finalDirectoryNameSuggestion;
                }
                directoryIndex++;
            }
        }
        public void Backup(IEnumerable<string> sourceFolders)
        {
            foreach (var sourceFolder in sourceFolders)
                Backup(sourceFolder);
        }

        public void Dispose()
        {
            var archiveName = $"{_targetFolder}/{Path.GetFileName(DestinationFolder)}.zip";
            _logger?.Log($"Создан архив \"{archiveName}\"", LogLevel.Info);
            ZipFile.CreateFromDirectory(DestinationFolder, archiveName);
            Directory.Delete(DestinationFolder, true);
        }
    }
}
