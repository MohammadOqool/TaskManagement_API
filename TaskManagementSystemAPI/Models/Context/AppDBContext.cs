using Microsoft.EntityFrameworkCore;
using System;
using TaskManagementSystemAPI.Models.Entities;

namespace TaskManagementSystemAPI.Models.Context
{
    public class AppDBContext : DbContext
    {
        public AppDBContext(DbContextOptions<AppDBContext> options) : base(options) { }

        public DbSet<User> Users { get; set; }
        public DbSet<UserTask> UserTasks { get; set; }
    }
}
