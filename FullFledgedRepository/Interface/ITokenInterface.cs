
using FullFledgedDto;
using FullFledgedModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FullFledgedRepository.Interface
{
    public interface ITokenInterface
    {
        string TokenGenerateString(UserLogin register);
    }
}
