using System;
using System.IO;
using System.Reflection;
using System.Threading;
using System.Threading.Tasks;
using Ftp.WebJobs.Extensions.Config;
using Microsoft.Azure.WebJobs.Extensions.Bindings;
using Microsoft.Azure.WebJobs.Host.Bindings;
using Microsoft.Azure.WebJobs.Host.Bindings.Path;
using Microsoft.Azure.WebJobs.Host.Protocols;

namespace Ftp.WebJobs.Extensions.Bindings
{
    internal class FtpBinding : IBinding
    {
        private readonly ParameterInfo _parameter;
        private readonly FtpConfiguration _config;
        private readonly BindingTemplate _bindingTemplate;
        
        public FtpBinding(FtpConfiguration config, ParameterInfo parameter, BindingTemplate bindingTemplate)
        {
            _config = config;
            _parameter = parameter;
            _bindingTemplate = bindingTemplate;
        }

        public bool FromAttribute => true;

        public async Task<IValueProvider> BindAsync(BindingContext context)
        {
            if (context == null)
            {
                throw new ArgumentNullException(nameof(context));
            }

            context.CancellationToken.ThrowIfCancellationRequested();

            string boundFileName = _bindingTemplate.Bind(context.BindingData);
            var ftpRequestInfo = new FtpRequestInfo(_config.UserName, _config.Password, _config.RootPath, boundFileName);
            
            return await BindAsync(ftpRequestInfo, context.ValueContext);
        }

        public Task<IValueProvider> BindAsync(object value, ValueBindingContext context)
        {
            FtpRequestInfo requestInfo = (FtpRequestInfo)value;

            return Task.FromResult<IValueProvider>(new FtpValueBinder(_parameter, requestInfo));
        }

        public ParameterDescriptor ToParameterDescriptor()
        {
            return new ParameterDescriptor
            {
                Name = _parameter.Name
            };
        }
        
        public class FtpValueBinder : StreamValueBinder
        {
            private readonly FtpRequestInfo _ftpRequestInfo;
            private readonly FtpClient _ftpClient;
            private Stream _stream;

            public FtpValueBinder(ParameterInfo parameterInfo, FtpRequestInfo ftpRequestInfo)
                : base(parameterInfo)
            {
                _ftpRequestInfo = ftpRequestInfo;
                _ftpClient = new FtpClient(ftpRequestInfo);
            }

            protected override Stream GetStream()
            {
                if (_stream == null)
                {
                    _stream = new MemoryStream();
                }
                return _stream;
            }

            public override Task SetValueAsync(object value, CancellationToken cancellationToken)
            {
                if (value == null)
                {
                    return Task.FromResult(0);
                }

                //if (typeof(Stream).IsAssignableFrom(value.GetType()))
                //{
                //    Stream stream = (Stream)value;
                //    stream.Close();
                //}
                //else if (typeof(TextWriter).IsAssignableFrom(value.GetType()))
                //{
                //    TextWriter writer = (TextWriter)value;
                //    writer.Close();
                //}
                //else if (typeof(TextReader).IsAssignableFrom(value.GetType()))
                //{
                //    TextReader reader = (TextReader)value;
                //    reader.Close();
                //}
                //else
                //{
                //    if (_parameter.IsOut)
                //    {
                //        // convert the value as needed into a byte[]
                //        byte[] bytes = null;
                //        if (value.GetType() == typeof(string))
                //        {
                //            bytes = Encoding.UTF8.GetBytes((string)value);
                //        }
                //        else if (value.GetType() == typeof(byte[]))
                //        {
                //            bytes = (byte[])value;
                //        }

                //        // open the file using the declared file options, and write the bytes
                //        using (Stream stream = GetStream())
                //        {
                //            stream.Write(bytes, 0, bytes.Length);
                //        }
                //    }
                //}

                _stream.Position = 0;

                _ftpClient.Send(_stream);

                return Task.FromResult(0);
            }

            public override string ToInvokeString()
            {
                return _ftpRequestInfo.FileName;
            }
        }
    }
}
