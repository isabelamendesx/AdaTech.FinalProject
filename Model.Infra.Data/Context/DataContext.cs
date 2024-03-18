using Microsoft.EntityFrameworkCore;
using Model.Domain.Entities;


namespace Model.Infra.Data.Context
{
    public class DataContext : DbContext
    {
        public DataContext(DbContextOptions<DataContext> options) : base(options) { }

        public DbSet<Refund> Refunds { get; set; }
        public DbSet<Rule> Rules { get; set; }
        public DbSet<RefundOperation> RefundOperations { get; set; }
        public DbSet<Category> Categories { get; set; }

    }
}
