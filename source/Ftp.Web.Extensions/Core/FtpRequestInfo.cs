
using System.Data;

namespace Ftp.WebJobs.Extensions
{
    public class FtpRequestInfo
    {
        public FtpRequestInfo(string userName, string password, string rootPath, string filename)
        {
            UserName = userName;
            Password = password;
            FileName = filename;
            RequestUri = rootPath + filename;
        }

        public string UserName { get; set; }
        public string Password { get; set; }
        public string RequestUri { get; set; }
        public string FileName { get; set; }
    }
}
