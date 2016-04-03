
using Ftp.WebJobs.Extensions.Framework;

namespace Ftp.WebJobs.Extensions.Config
{
    public class FtpConfiguration
    {
        private readonly string _userNameSetting = "UserName";
        private readonly string _passwordSetting = "Password";
        private readonly string _ftpHostPathSetting = "FtpHostPath";

        public FtpConfiguration()
        {
            UserName = AmbientSettingsProvider.Instance.GetSetting(_userNameSetting);
            Password = AmbientSettingsProvider.Instance.GetSetting(_passwordSetting);
            RootPath = AmbientSettingsProvider.Instance.GetSetting(_ftpHostPathSetting);
        }

        public FtpConfiguration(string userName, string password, string ftpHostPath)
        {
            UserName = userName;
            Password = password;
            RootPath = ftpHostPath;
        }

        public string UserName { get; set; }
        public string Password { get; set; }
        public string RootPath { get; set; }
    }
}
