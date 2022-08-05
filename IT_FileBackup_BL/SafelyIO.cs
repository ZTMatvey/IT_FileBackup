using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.AccessControl;
using System.Text;
using System.Threading.Tasks;

namespace IT_FileBackup_BL
{
    internal static class SafelyIO
    {
        public static IEnumerable<AccessDirectoryInfo> GetAllDirectories(string path)
        {
            if (!Directory.Exists(path))
                yield break;
            var hasAccess = HasAccessToDirectory(path);
            if (hasAccess)
            {
                var directories = Directory.GetDirectories(path);
                foreach (var directory in directories)
                {
                    var subdirectories = GetAllDirectories(directory).ToList();
                    foreach (var subdirectory in subdirectories)
                        yield return subdirectory;
                }
            }
            yield return new AccessDirectoryInfo(path, hasAccess);
        }
        public static bool HasAccessToDirectory(string path)
        {
            try
            {
                var directories = Directory.GetDirectories(path);
                return true;
            }
            catch (UnauthorizedAccessException)
            {
                return false;
            }
        }
        public static bool TryToCopyFile(string sourceFolder, string targetFolder)
        {
            try
            {
                File.Copy(sourceFolder, targetFolder);
                return true;
            }
            catch (Exception)
            {
                return false;
            }
        }
    }
}
