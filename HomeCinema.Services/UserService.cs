using AutoMapper;
using HomeCinema.DataAccess.Interfaces;
using HomeCinema.Domain;
using HomeCinema.Domain.Models;
using HomeCinema.Services.Interfaces;
using HomeCinema.WebModels.WebModels;
using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace HomeCinema.Services
{
    public class UserService : IUserService
    {
        private readonly IUserRepository<User> _userRepository;
        private readonly UserManager<User> _userManager;
        private readonly SignInManager<User> _signInManager;
        private readonly IMapper _mapper;
        public UserService(IUserRepository<User> userRepository, UserManager<User> userManager, SignInManager<User> signInManager, IMapper mapper)
        {
            _mapper = mapper;
            _signInManager = signInManager;
            _userManager = userManager;
            _userRepository = userRepository;
        }

        public bool CheckEmail(string email)
        {
            var user = _userRepository.GetByEmail(email);
            if (user == null)
            {
                return false;
            }
            else
            {
                return true;
            }

        }

        public bool CheckUsername(string username)
        {
            var user = _userRepository.GetByUsername(username);
            if (user == null)
            {
                return false;
            }
            else
            {
                return true;
            }

        }

        public IEnumerable<UserViewModel> GetAllUsers()
        {
            return _mapper.Map<IEnumerable<UserViewModel>>(_userRepository.GetAll());
        }

        public UserViewModel GetCurrentUser(string username)
        {
            User user = _userRepository.GetByUsername(username);
            if (user == null) throw new Exception("User does not exist");
            return _mapper.Map<UserViewModel>(user);
        }

        public  void Login(LoginViewModel loginModel)
        {
            SignInResult signInRes =_signInManager.PasswordSignInAsync(
                loginModel.Username, loginModel.Password, false, false).Result;

            if (signInRes.IsNotAllowed)
            {
                throw new Exception("Username or password is wrong");
            }
        }

        public void Logout()
        {
            _signInManager.SignOutAsync();
        }

        public void Register(RegisterViewModel registerModel)
        {
            if (_userRepository.GetByUsername(registerModel.Username) != null)
                throw new Exception("Username already exists!");
            if (registerModel.Password != registerModel.ConfirmPassword)
                throw new Exception("Paswords does not match!");
            User user = _mapper.Map<User>(registerModel);

            IdentityResult identityResult =  _userManager.CreateAsync(user, registerModel.Password).Result;
            if (identityResult.Succeeded)
            {
                User currentUser = _userManager.FindByNameAsync(user.UserName).Result;
                _userManager.AddToRoleAsync(currentUser, "user");
            }

            else
            {
                throw new Exception(identityResult.Errors.ToString());
            }
            Login(new LoginViewModel
            {
                Username = registerModel.Username,
                Password = registerModel.Password
            });

        }


    }
}
