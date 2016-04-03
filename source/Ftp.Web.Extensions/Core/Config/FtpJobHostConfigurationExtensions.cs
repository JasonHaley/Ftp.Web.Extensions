
using System;
using Ftp.WebJobs.Extensions.Bindings;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Host.Config;

namespace Ftp.WebJobs.Extensions.Config
{
    public static class FtpJobHostConfigurationExtensions
    {
        public static void UseFtp(this JobHostConfiguration config, FtpConfiguration ftpConfig = null)
        {
            if (config == null)
            {
                throw new ArgumentNullException(nameof(config));
            }

            if (ftpConfig == null)
            {
                ftpConfig = new FtpConfiguration();
            }

            config.RegisterExtensionConfigProvider(new FtpExtensionConfig(ftpConfig));
        }

        private class FtpExtensionConfig : IExtensionConfigProvider
        {
            private readonly FtpConfiguration _ftpConfig;

            public FtpExtensionConfig(FtpConfiguration ftpConfig)
            {
                _ftpConfig = ftpConfig;
            }

            public void Initialize(ExtensionConfigContext context)
            {
                if (context == null)
                {
                    throw new ArgumentNullException(nameof(context));
                }

                context.Config.RegisterBindingExtension(new FtpAttributeBindingProvider(_ftpConfig, context.Config.NameResolver, context.Trace));
            }
        }
    }
}
