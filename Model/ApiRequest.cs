using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CSM_UserAuthentication.Model
{
    public class ApiRequest
    {
        public string Domain { get; set; }
        public string Username { get; set; }
        public string Password { get; set; }
        public string AuthType { get; set; }

    }
}
