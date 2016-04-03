using System;

namespace Ftp.WebJobs.Extensions
{
    [AttributeUsage(AttributeTargets.Parameter)]
    public class FtpAttribute : Attribute
    {
        public FtpAttribute(string path)
        {
            if (string.IsNullOrEmpty(path))
            {
                throw new ArgumentNullException(nameof(path));
            }

            Path = path;
        }

        public string Path { get; private set; }
    }
}
