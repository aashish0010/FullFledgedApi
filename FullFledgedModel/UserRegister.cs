using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace FullFledgedModel
{
    public class UserRegister
    {
        public int Id { get; set; }
        public string? UserName { get; set; }
        public byte[]? PasswordHash { get; set; }
        public byte[]? PasswordSalt { get; set; }
        public string? Email { get; set; }
        public DateTime Created { get; set; }
        public int Status { get; set; }
        public DateTime UpdateDate { get; set; }

    }
}
