using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Perimetr.WindowsUniversal.Services
{
    public interface ILoginService
    {
        Task<string> GetAccessTokenAsync(string username, string password);
    }


}
