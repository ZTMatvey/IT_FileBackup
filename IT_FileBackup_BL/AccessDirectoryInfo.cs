using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IT_FileBackup_BL
{
    public sealed class AccessDirectoryInfo
    {
        private readonly string _path;
        public string Path => _path;
        private readonly bool _hasAccess;
        public bool HasAccess => _hasAccess;
        public AccessDirectoryInfo(string path, bool hasAccess)
        {
            _path = path;
            _hasAccess = hasAccess;
        }
    }
}
