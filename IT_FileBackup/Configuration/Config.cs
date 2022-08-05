using Microsoft.Extensions.Configuration;

namespace IT_FileBackup.Configuration
{
    public sealed class Config
    {
        private readonly static string _configerationPath =
            $"{Directory.GetParent(@$"../../../")}/Configuration/config.json";
        private static ConfigData _instance;
        public static ConfigData GetInstance()
        {
            if (_instance == null)
            {
                _instance = new ConfigData();
                var builder = new ConfigurationBuilder();
                builder.AddJsonFile(_configerationPath);
                var configuration = builder.Build();
                configuration.Bind(_instance);
            }
            return _instance;
        }
    }
}
