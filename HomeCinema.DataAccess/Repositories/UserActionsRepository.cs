using HomeCinema.DataAccess.Interfaces;
using HomeCinema.Domain.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace HomeCinema.DataAccess.Repositories
{
    public class UserActionsRepository : BaseRepository<HomeCinemaDbContext>, IRepository<UserActions>
    {
        public UserActionsRepository(HomeCinemaDbContext context) : base(context) { }
        public int Delete(int id)
        {
            UserActions userActions = _dbcontext.UserActions.FirstOrDefault(a => a.Id == id);
            if (userActions == null) return -1;
            _dbcontext.UserActions.Remove(userActions);
            return _dbcontext.SaveChanges();
        }

        public IEnumerable<UserActions> GetAll()
        {
            return _dbcontext.UserActions.
                Include(a => a.User).
                Include(a => a.UserActionMovies).
                ThenInclude(a => a.Movie).ToList();
        }

        public UserActions GetById(int id)
        {
            return _dbcontext.UserActions.
                Include(a => a.User).
                Include(a => a.UserActionMovies).
                ThenInclude(a => a.Movie).FirstOrDefault(a=>a.Id==id);
        }

        public int Insert(UserActions entity)
        {
            _dbcontext.UserActions.Add(entity);
            return _dbcontext.SaveChanges();
        }

        public int Update(UserActions entity)
        {
            UserActions userActions = _dbcontext.UserActions.FirstOrDefault(a => a.Id == entity.Id);
            if (userActions == null) return -1;
            userActions.Action = entity.Action;
            userActions.UserId = entity.UserId;
            return _dbcontext.SaveChanges();
        }
    }
}
