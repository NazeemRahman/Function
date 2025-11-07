using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CSM_UserAuthentication.Model
{
    public class ApiResponse
    {
        public bool IsUserAuthenticated { get; set; }
        public string Message { get; set; } 
    }
}
