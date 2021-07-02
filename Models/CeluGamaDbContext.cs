using System;
using Microsoft.EntityFrameworkCore;

namespace CeluGamaSystem.Models
{
    public partial class CeluGamaDbContext : DbContext
    {
        public CeluGamaDbContext(DbContextOptions<CeluGamaDbContext> options)
            : base(options) { }

        public DbSet<Token> Tokens { get; set; }
        public DbSet<Order> Orders { get; set; }
        public DbSet<OrderItem> OrderItems { get; set; }
        public DbSet<Payment> Payments { get; set; }
        public DbSet<Log> Logs { get; set; }
        public DbSet<User> Users { get; set; }
    }
}
