using System.ComponentModel.DataAnnotations.Schema;


namespace ExpenseFlow.Domain.Entities;

    public enum ExpenseStatus { Pending, Approved, Rejected }

//[Table("Expenses"), Schema("dbo")]
    public class Expense
    {
        public Guid Id { get; set; }
        public Guid PersonnelId { get; set; }

        public virtual Personnel Personnel { get; set; } 
        public int CategoryId { get; set; }

        public virtual ExpenseCategory ExpenseCategory { get; set; }

        public decimal Amount { get; set; }
        public string Description { get; set; } = null!;
        public string? Location { get; set; }
        public DateTime Date { get; set; } = DateTime.UtcNow;

        public ExpenseStatus Status { get; set; } = ExpenseStatus.Pending;
        public string? RejectReason { get; set; }


    

        public ICollection<ExpenseAttachment>? Attachments { get; set; }
        public ICollection<PaymentTransaction>? Transactions { get; set; }
    
    }


/*
    public class ExpenseConfiguration : IEntityTypeConfiguration<Expense>
    {
        public void Configure(EntityTypeBuilder<Expense> builder){
            builder.ToTable("Expenses");
            builder.HasKey(e => e.Id);
            builder.Property(e => e.Id).ValueGeneratedOnAdd();
            builder.Property(e => e.Description).IsRequired();
            builder.Property(e => e.Amount).HasColumnType("decimal(18,2)");
            builder.Property(e => e.Location).HasMaxLength(100);
            builder.Property(e => e.RejectReason).HasMaxLength(200);
        }
    }
*/
