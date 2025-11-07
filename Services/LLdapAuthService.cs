using CSM_UserAuthentication.Interface;
using CSM_UserAuthentication.Model;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using Novell.Directory.Ldap;
namespace CSM_UserAuthentication.Services
{
    public class LLdapAuthService : IAuthenticationRepository
    {
        private readonly HttpClient _httpClient;
        private readonly IConfiguration _configuration;
        private readonly ILogger<LLdapAuthService> _logger;

        public LLdapAuthService(HttpClient httpClient, IConfiguration configuration, ILogger<LLdapAuthService> logger)
        {
            _httpClient = httpClient;
            _configuration = configuration;
            _logger = logger;
        }

        public async Task<bool> AuthenticateAsync(ApiRequest request)
        {
            try
            {
                string ldapHost = _configuration["LLDAP:Host"] ?? "localhost"; 
                int ldapPort = int.Parse(_configuration["LLDAP:Port"] ?? "389");
                string baseDn = _configuration["LLDAP:BaseDn"]?? "dc=example,dc=com";
                string userDn = $"uid={request.Username},ou=people,{baseDn}";

                _logger.LogInformation($"Attempting LLDAP bind for user: {userDn} on {ldapHost}:{ldapPort}");

                using (var ldapConnection = new LdapConnection())
                {
                    await ldapConnection.ConnectAsync(ldapHost, ldapPort);

                    await ldapConnection.BindAsync(userDn, request.Password);
                    bool isAuthenticated = ldapConnection.Bound;

                    if (isAuthenticated)
                    {
                        _logger.LogInformation("User '{Username}' authenticated successfully.", request.Username);
                    }
                    else
                    {
                        _logger.LogWarning("User '{Username}' failed to authenticate.", request.Username);
                    }

                    return isAuthenticated;
                }
            }
            catch(LdapException ex)
            {
                _logger.LogError($"Athentication Failed: {ex.Message}");
                return false;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Unexpected error during LLDAP authentication for {request.Username}");
                return false;
            }
        }
    }
}
