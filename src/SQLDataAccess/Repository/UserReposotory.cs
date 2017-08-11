using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Core.Interface;
using Core.Model;

namespace SQLDataAccess.Repository
{
    public class UserRepository : IUserRepository
    {
        public bool DeleteUserMaster(int userId)
        {
            throw new NotImplementedException();
        }

        public UserMaster GetUserDetails(int userId)
        {
            throw new NotImplementedException();
        }

        public IList<UserMaster> GetUsers()
        {
            return new List<UserMaster>()
            {
                new UserMaster {Id = 4, UserName = "a", UserPassword = "s"},
                new UserMaster {Id = 1, UserName = "user1", UserPassword = "user1psd"},
                new UserMaster {Id = 2, UserName = "user2", UserPassword = "user2psd"},
                new UserMaster {Id = 3, UserName = "user3", UserPassword = "user3psd"}
            };
        }

        public bool InsertUpdateUserMaster(UserMaster userMaster)
        {
            throw new NotImplementedException();
        }
    }
}
