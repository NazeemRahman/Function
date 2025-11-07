using Azure.Core;
using CSM_UserAuthentication.Interface;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CSM_UserAuthentication.Services
{
    public class AuthRepositoryFactory
    {
        private readonly IServiceProvider _serviceProvider;

        public AuthRepositoryFactory(IServiceProvider serviceProvider)
        {
            _serviceProvider = serviceProvider;
        }

        public IAuthenticationRepository createfactory(string Authtype)
        {
            return Authtype.ToLower() switch
            {
                "lldap" => ActivatorUtilities.CreateInstance<LLdapAuthService>(_serviceProvider),
                _ => ActivatorUtilities.CreateInstance<LLdapAuthService>(_serviceProvider)
            };
        }
    }
}
