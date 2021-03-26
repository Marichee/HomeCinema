using System;
using System.Collections.Generic;
using System.Text;

namespace HomeCinema.DataAccess.Interfaces
{
   public interface IRepository<Tentity>
        where Tentity:class
    {
        IEnumerable<Tentity> GetAll();
        Tentity GetById(int id);
        int Insert(Tentity entity);
        int Delete(int id);
        int Update(Tentity entity);
    }
}
