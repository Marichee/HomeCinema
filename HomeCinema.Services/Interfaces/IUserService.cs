using HomeCinema.WebModels.WebModels;
using System;
using System.Collections.Generic;
using System.Text;

namespace HomeCinema.Services.Interfaces
{
   public interface IUserService
    {
        void Register(RegisterViewModel registerModel);
        void Login(LoginViewModel loginModel);
        void Logout();
        bool CheckUsername(string username);
        bool CheckEmail(string email); 
        UserViewModel GetCurrentUser(string username);
        IEnumerable<UserViewModel> GetAllUsers();
    }
}
