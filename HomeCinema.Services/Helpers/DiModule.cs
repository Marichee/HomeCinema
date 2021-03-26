using HomeCinema.DataAccess;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.AspNetCore.Identity;
using HomeCinema.Domain;
using HomeCinema.DataAccess.Interfaces;
using HomeCinema.DataAccess.Repositories;
using HomeCinema.Domain.Models;

namespace HomeCinema.Services.Helpers
{
   public class DiModule
    {
        public static IServiceCollection RegisterModules(IServiceCollection services,string connectionString)
        {
            services.AddDbContext<HomeCinemaDbContext>(op => op.UseSqlServer(connectionString));
            services.AddIdentity<User, IdentityRole>(op =>
            {
                op.User.RequireUniqueEmail = true;
                op.Password.RequireNonAlphanumeric = true;
            })
          .AddRoleManager<RoleManager<IdentityRole>>()
          .AddRoles<IdentityRole>()
           .AddEntityFrameworkStores<HomeCinemaDbContext>()
           .AddDefaultTokenProviders();
            services.AddTransient<IUserRepository<User>, UserRepository>();
            services.AddTransient<IRepository<Movie>, MovieRepository>();
            services.AddTransient<IRepository<UserActions>, UserActionsRepository>();
            services.AddTransient<IRepository<UserActionMovies>, UserActionMoviesRepository>();
     
            
            return services;
        }
    }
}
