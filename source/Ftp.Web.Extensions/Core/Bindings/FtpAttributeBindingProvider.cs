using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Reflection;
using System.Threading.Tasks;
using Ftp.WebJobs.Extensions.Config;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Extensions.Bindings;
using Microsoft.Azure.WebJobs.Host;
using Microsoft.Azure.WebJobs.Host.Bindings;
using Microsoft.Azure.WebJobs.Host.Bindings.Path;

namespace Ftp.WebJobs.Extensions.Bindings
{
    internal class FtpAttributeBindingProvider : IBindingProvider
    {
        private readonly FtpConfiguration _config;
        private readonly INameResolver _nameResolver;
        private readonly TraceWriter _trace;

        public FtpAttributeBindingProvider(FtpConfiguration config, INameResolver nameResolver, TraceWriter trace)
        {
            if (config == null)
            {
                throw new ArgumentNullException(nameof(config));
            }

            if (nameResolver == null)
            {
                throw new ArgumentNullException(nameof(nameResolver));
            }

            if (trace == null)
            {
                throw new ArgumentNullException(nameof(trace));
            }

            _config = config;
            _nameResolver = nameResolver;
            _trace = trace;
        }

        public Task<IBinding> TryCreateAsync(BindingProviderContext context)
        {
            if (context == null)
            {
                throw new ArgumentNullException(nameof(context));
            }

            ParameterInfo parameter = context.Parameter;
            FtpAttribute attribute = parameter.GetCustomAttribute<FtpAttribute>(inherit: false);
            if (attribute == null)
            {
                return Task.FromResult<IBinding>(null);
            }

            string path = attribute.Path;
            if (_nameResolver != null)
            {
                path = _nameResolver.ResolveWholeString(path);
            }
            BindingTemplate bindingTemplate = BindingTemplate.FromString(path);
            bindingTemplate.ValidateContractCompatibility(context.BindingDataContract);

            IEnumerable<Type> types = StreamValueBinder.GetSupportedTypes(FileAccess.Read);
            if (!ValueBinder.MatchParameterType(context.Parameter, types))
            {
                throw new InvalidOperationException(string.Format(CultureInfo.CurrentCulture,
                    "Can't bind FtpAttribute to type '{0}'.", parameter.ParameterType));
            }

            return Task.FromResult<IBinding>(new FtpBinding(_config, parameter, bindingTemplate));
        }
    }
}
