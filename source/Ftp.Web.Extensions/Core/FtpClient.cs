
using System;
using System.Diagnostics;
using System.IO;
using System.Net;

namespace Ftp.WebJobs.Extensions
{
    public class FtpClient
    {
        private FtpWebRequest _request;

        public FtpClient(FtpRequestInfo requestInfo)
        {
            Initialize(requestInfo);
        }

        private void Initialize(FtpRequestInfo requestInfo)
        { 
            _request= (FtpWebRequest)WebRequest.Create(requestInfo.RequestUri);
            _request.Credentials = new NetworkCredential(requestInfo.UserName, requestInfo.Password);
            _request.UseBinary = true;
            _request.Method = WebRequestMethods.Ftp.UploadFile;
            _request.KeepAlive = false;
        }

        public string FileName { get; private set; }
        
        public void Send(Stream stream)
        {
            //var len = (int)stream.Length;
            try
            {
                Stream outStream = _request.GetRequestStream();
                stream.CopyTo(outStream);
                outStream.Close();
            }
            catch (Exception ex)
            {
                Trace.WriteLine(ex.Message);
            }
        }
        
    }
}
