using HomeCinema.DataAccess.Interfaces;
using HomeCinema.Domain;
using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace HomeCinema.DataAccess.Repositories
{
    public class UserRepository : BaseRepository<HomeCinemaDbContext>, IUserRepository<User>
    {
        public UserRepository(HomeCinemaDbContext dbContext) : base(dbContext) { }
        public int Delete(string id)
        {
            User user = _dbcontext.Users.FirstOrDefault(u => u.Id == id);
            if (user == null) return -1;
            _dbcontext.Remove(user);
            return _dbcontext.SaveChanges();
        }

        public IEnumerable<User> GetAll()
        {
            return _dbcontext.Users.ToList();
        }

        public User GetById(string id)
        {
            return _dbcontext.Users.FirstOrDefault(u => u.Id == id);
        }

        public User GetByUsername(string username)
        {
            return _dbcontext.Users.FirstOrDefault(u => u.UserName == username);
        }
        public User GetByEmail(string email)
        {
            return _dbcontext.Users.FirstOrDefault(u => u.Email == email);
        }
        public int Insert(User user)
        {
            _dbcontext.Users.Add(user);
           return _dbcontext.SaveChanges();
        }

        public int Update(User user)
        {
            User userFound = _dbcontext.Users.FirstOrDefault(u => u.Id == user.Id);
            if (userFound == null) return -1;
            userFound.UserName = user.UserName;
            userFound.NormalizedEmail = user.UserName.ToUpper();
            userFound.Email = user.Email;
            userFound.NormalizedEmail = user.Email.ToUpper();
            return _dbcontext.SaveChanges();
        }
    }
}
