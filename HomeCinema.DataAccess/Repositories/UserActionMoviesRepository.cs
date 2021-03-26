using HomeCinema.DataAccess.Interfaces;
using HomeCinema.Domain.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace HomeCinema.DataAccess.Repositories
{
    public class UserActionMoviesRepository : BaseRepository<HomeCinemaDbContext>, IRepository<UserActionMovies>
    {
        public UserActionMoviesRepository(HomeCinemaDbContext context) : base(context) { }
        public int Delete(int id)
        {
            UserActionMovies userActionMovies = _dbcontext.UserActionMovies.FirstOrDefault(m => int.Parse($"{m.UserActionId}{m.MovieId}") == id);
            if (userActionMovies == null) return -1;
            _dbcontext.Remove(userActionMovies);
            return _dbcontext.SaveChanges();
        }

        public IEnumerable<UserActionMovies> GetAll()
        {
           return _dbcontext.UserActionMovies.ToList();
        }

        public UserActionMovies GetById(int id)
        {
            return _dbcontext.UserActionMovies.FirstOrDefault(m => int.Parse($"{m.UserActionId}{m.MovieId}") == id);
        }

        public int Insert(UserActionMovies entity)
        {
            _dbcontext.UserActionMovies.Add(entity);
            return _dbcontext.SaveChanges();
        }

        public int Update(UserActionMovies entity)
        {
            _dbcontext.UserActionMovies.Update(entity);
            return _dbcontext.SaveChanges();
        }
    }
}
