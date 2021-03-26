using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Text;

namespace HomeCinema.DataAccess.Repositories
{
   public abstract class BaseRepository<TDbcontext>
        where TDbcontext:DbContext
    {
        protected readonly TDbcontext _dbcontext;
        public BaseRepository(TDbcontext dbcontext)
        {
            _dbcontext = dbcontext;
        }
    }
}
