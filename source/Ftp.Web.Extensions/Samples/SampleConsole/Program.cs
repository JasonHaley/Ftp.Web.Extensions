using System.IO;
using Ftp.WebJobs.Extensions.Config;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Extensions.Files;

namespace SampleConsole
{
    class Program
    {
        static void Main(string[] args)
        {
            var config = new JobHostConfiguration();

            var filesConfig = new FilesConfiguration()
            {
                RootPath = @"d:\temp\files"
            };
            config.UseFiles(filesConfig);
            EnsureSampleDirectoriesExist(filesConfig.RootPath);
            
            config.UseFtp();
            
            JobHost host = new JobHost(config);

            host.Call(typeof(FtpSamples).GetMethod("ReadWrite"));

            host.RunAndBlock();
        }

        private static void EnsureSampleDirectoriesExist(string rootFilesPath)
        {
            // Ensure all the directories referenced by the file sample bindings
            // exist
            Directory.CreateDirectory(rootFilesPath);
            
            File.WriteAllText(Path.Combine(rootFilesPath, "input.txt"), "WebJobs SDK Extensions!");
        }
    }
}
