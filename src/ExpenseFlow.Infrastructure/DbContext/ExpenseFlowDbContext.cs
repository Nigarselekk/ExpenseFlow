using ExpenseFlow.Domain.Entities;
using ExpenseFlow.Infrastructure.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace ExpenseFlow.Infrastructure.DbContext
{
    public class ExpenseFlowDbContext : IdentityDbContext<ApplicationUser>
    {
        public ExpenseFlowDbContext(DbContextOptions<ExpenseFlowDbContext> options)
            : base(options) { }

        public DbSet<Personnel> Personnels { get; set; } = null!;
        public DbSet<AccountInfo> AccountInfos { get; set; } = null!;
        public DbSet<ExpenseCategory> ExpenseCategories { get; set; } = null!;
        public DbSet<Expense> Expenses { get; set; } = null!;
        public DbSet<ExpenseAttachment> ExpenseAttachments { get; set; } = null!;
        public DbSet<PaymentTransaction> PaymentTransactions { get; set; } = null!;

        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);

            // Personnel - ApplicationUser (1-1)
            builder.Entity<Personnel>()
                .HasOne<ApplicationUser>()
                .WithOne()
                .HasForeignKey<Personnel>(p => p.ApplicationUserId);

            // Personnel - AccountInfo (1-n)
            builder.Entity<AccountInfo>()
                .HasOne<Personnel>()
                .WithMany(p => p.Accounts)
                .HasForeignKey(a => a.PersonnelId);

            // Personnel - Expense (1-n)
            builder.Entity<Expense>()
                .HasOne<Personnel>()
                .WithMany(p => p.Expenses)
                .HasForeignKey(e => e.PersonnelId);

            // Expense - ExpenseCategory (n-1)
            builder.Entity<Expense>()
                .HasOne<ExpenseCategory>()
                .WithMany(c => c.Expenses)
                .HasForeignKey(e => e.CategoryId);

            // Expense - ExpenseAttachment (1-n)
            builder.Entity<ExpenseAttachment>()
                .HasOne<Expense>()
                .WithMany(e => e.Attachments)
                .HasForeignKey(a => a.ExpenseId);

            // Expense - PaymentTransaction (1-n)
            builder.Entity<PaymentTransaction>()
                .HasOne<Expense>()
                .WithMany(e => e.Transactions)
                .HasForeignKey(t => t.ExpenseId);

                builder.Entity<PaymentTransaction>()
                .HasOne(pt => pt.AccountInfo)
                .WithMany()
                .HasForeignKey(pt => pt.AccountInfoId)
                .OnDelete(DeleteBehavior.Restrict);    
        }
    }
}
