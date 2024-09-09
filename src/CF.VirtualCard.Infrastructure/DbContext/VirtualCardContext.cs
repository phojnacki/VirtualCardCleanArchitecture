using CF.VirtualCard.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;

namespace CF.VirtualCard.Infrastructure.DbContext;

public class VirtualCardContext(DbContextOptions<VirtualCardContext> options)
    : Microsoft.EntityFrameworkCore.DbContext(options)
{
    public DbSet<Domain.Entities.VirtualCard> VirtualCards { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        VirtualCardModelBuilder(modelBuilder);
    }

    private static void VirtualCardModelBuilder(ModelBuilder modelBuilder)
    {
        var model = modelBuilder.Entity<Domain.Entities.VirtualCard>();

        model.ToTable("VirtualCard");

        model.OwnsOne(x => x.CardNumber);
        //    , cardNumber =>
        //{
        //    cardNumber.Property(c => c.Value).HasColumnName("CardNumber").IsRequired();
        //    cardNumber.HasIndex(c => c.Value);
        //});

        model.Property(x => x.FirstName)
            .HasMaxLength(100)
            .IsRequired();

        model.Property(x => x.Surname)
            .HasMaxLength(100)
            .IsRequired();
    }
}