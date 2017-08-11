using System.Collections.Generic;
using Core.Model;

namespace Core.Interface
{
    public interface IUserRepository
    {
        #region User Master
        IList<UserMaster> GetUsers();
        UserMaster GetUserDetails(int userId);
        bool InsertUpdateUserMaster(UserMaster userMaster);
        bool DeleteUserMaster(int userId);
        #endregion
    }
}
