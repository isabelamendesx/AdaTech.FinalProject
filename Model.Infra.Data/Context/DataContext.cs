using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Model.Domain.Entities;


namespace Model.Infra.Data.Context
{
    public class DataContext : DbContext
    {
        public DataContext(DbContextOptions<DataContext> options) : base(options)
        {
        }

        public DbSet<Refund> Refunds { get; set; }
        public DbSet<Rule> Rules { get; set; }
        public DbSet<RefundOperation> RefundOperations { get; set; }
        public DbSet<Category> Categories { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Refund>()
                .HasOne(e => e.CategoryID)
                .WithMany()
                .HasForeignKey(e => e.CategoryID);D

            modelBuilder.Entity<Refund>()
                .HasMany(e => e.Operations)
                .WithOne()
                .HasForeignKey(op => op.RefundId);


            modelBuilder.Entity<Rule>()
                .HasOne(e => e.Category)
                .WithMany()
                .HasForeignKey(e => e.CategoryID);           


            modelBuilder.Entity<RefundOperation>()
                .HasMany(e => e.Refund)
                .WithOne()
                .HasForeignKey(e => e.RefundId);           
        }
    }
}
