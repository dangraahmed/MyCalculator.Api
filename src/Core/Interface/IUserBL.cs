using Core.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Core.Interface
{
    public interface IUserBL : IUserRepository
    {
        UserMaster Authenticate(string userName, string password);
    }
}
