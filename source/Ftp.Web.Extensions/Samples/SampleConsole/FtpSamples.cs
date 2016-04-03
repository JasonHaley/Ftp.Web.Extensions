using System.IO;
using Ftp.WebJobs.Extensions;
using Microsoft.Azure.WebJobs;

namespace SampleConsole
{
    public static class FtpSamples
    {
        //public static void ProcessFile(
        //    [FileTrigger(@"import\{name}", "*.txt", autoDelete: true)] Stream input,
        //    [Ftp(@"{name}")] Stream output)
        //{
        //    input.CopyTo(output);
        //}

        //public static void ProcessFile(
        //    [FileTrigger(@"import\{name}", "*.txt", autoDelete: true)] string input,
        //    [Ftp(@"{name}")] out string output)
        //{
        //    output = input;
        //}
        public static void ReadWrite(
            [File(@"input.txt", FileAccess.Read, FileMode.OpenOrCreate)] Stream input,
            [Ftp(@"output.txt")] Stream output)
        {
            input.CopyTo(output);
        }
    }
}
