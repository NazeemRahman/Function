using CSM_UserAuthentication.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CSM_UserAuthentication.Interface
{
    public interface IAuthenticationRepository
    {
        Task<bool> AuthenticateAsync(ApiRequest request);
    }
}
