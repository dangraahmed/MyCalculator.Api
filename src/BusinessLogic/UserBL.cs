using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Core.Interface;
using Core.Model;

namespace BusinessLogic
{
    public class UserBL : IUserBL
    {
        private readonly IUserRepository _repository;

        public UserBL(IUserRepository repository)
        {
            if (repository == null)
            {
                throw new ArgumentNullException(nameof(repository));
            }

            _repository = repository;
        }

        #region User Master

        public IList<UserMaster> GetUsers()
        {
            return _repository.GetUsers();
        }

        public UserMaster GetUserDetails(int userId)
        {
            return _repository.GetUserDetails(userId);
        }

        public bool InsertUpdateUserMaster(UserMaster userMaster)
        {
            return _repository.InsertUpdateUserMaster(userMaster);
        }

        public bool DeleteUserMaster(int userId)
        {
            return _repository.DeleteUserMaster(userId);
        }

        #endregion
    }
}
