using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Text;

namespace HomeCinema.DataAccess.Interfaces
{
   public interface IUserRepository<TUser>
        where TUser:IdentityUser
    {
        IEnumerable<TUser> GetAll();
        TUser GetById(string id);
        TUser GetByUsername(string username);
        TUser GetByEmail(string email);
        int Insert(TUser user);
        int Update(TUser user);
        int Delete(string id);
    }
}
