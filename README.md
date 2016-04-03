#NOTE: I will be moving this to a repo named Ftp.WebJobs.Extensions ...

# Ftp.Web.Extensions
Web job extension for sending files with ftp

NOTE: you will need to create two files in the Config directory to store your storage connection strings and Ftp connection settings.

#Ftp
A binding that will send files to an ftp site

Example of reading a file using the File binding from the WebJobs SDK Extensions to then send that file via FTP
```
public static void ReadWrite(
    [File(@"input.txt", FileAccess.Read, FileMode.OpenOrCreate)] Stream input,
    [Ftp(@"output.txt")] Stream output)
{
    input.CopyTo(output);
}
```
