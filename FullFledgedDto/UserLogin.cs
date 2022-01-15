using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FullFledgedDto
{
    public class UserLogin
    {
        public string? UserName { get; set; }
        public string? Password { get; set; }

    }
    public class UserLoginResult
    {
        public string? UserName { get; set;}
        public string? Token { get; set; }
    }
}
