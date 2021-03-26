using HomeCinema.DataAccess.Interfaces;
using HomeCinema.Domain.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace HomeCinema.DataAccess.Repositories
{
    public class MovieRepository : BaseRepository<HomeCinemaDbContext>, IRepository<Movie>
    {
        public MovieRepository(HomeCinemaDbContext context) : base(context) { }
        public int Delete(int id)
        {
            Movie movie = _dbcontext.Movie.FirstOrDefault(m => m.Id == id);
            if (movie == null) return -1;
            _dbcontext.Movie.Remove(movie);
            return _dbcontext.SaveChanges();
        }

        public IEnumerable<Movie> GetAll()
        {
            return _dbcontext.Movie.ToList();
        }

        public Movie GetById(int id)
        {
            return _dbcontext.Movie.FirstOrDefault(m => m.Id == id);
        }

        public int Insert(Movie entity)
        {
            _dbcontext.Movie.Add(entity);
            return _dbcontext.SaveChanges();
        }

        public int Update(Movie entity)
        {
            Movie movie = _dbcontext.Movie.FirstOrDefault(m => m.Id == entity.Id);
            if (movie == null) return -1;
            movie.Name = entity.Name;
            movie.Genre = entity.Genre;
            return _dbcontext.SaveChanges();
        }
    }
}
