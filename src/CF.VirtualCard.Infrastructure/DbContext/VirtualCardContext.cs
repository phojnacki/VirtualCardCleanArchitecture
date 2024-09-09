using CF.VirtualCard.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;

namespace CF.VirtualCard.Infrastructure.DbContext;

public class VirtualCardContext(DbContextOptions<VirtualCardContext> options)
    : Microsoft.EntityFrameworkCore.DbContext(options)
{
    public DbSet<Domain.Entities.VirtualCard> VirtualCards { get; set; }
	public DbSet<Domain.Entities.BillingCycle> BillingCycle { get; set; }

	protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        VirtualCardModelBuilder(modelBuilder);
	}

    private static void VirtualCardModelBuilder(ModelBuilder modelBuilder)
    {
        var cardBuilder = modelBuilder.Entity<Domain.Entities.VirtualCard>();

        cardBuilder.ToTable("VirtualCard");

        cardBuilder.OwnsOne(vc => vc.CardNumber, cardNumberBuilder =>
        {
			cardNumberBuilder.Property(n => n.Value).HasColumnName("CardNumber").IsRequired().HasMaxLength(19);
			cardNumberBuilder.HasIndex(n => n.Value);
        });

        cardBuilder.Property(vc => vc.FirstName)
            .HasMaxLength(100)
            .IsRequired();

        cardBuilder.Property(vc => vc.Surname)
            .HasMaxLength(100)
            .IsRequired();

		var cycleBuilder = modelBuilder.Entity<Domain.Entities.BillingCycle>();

		cycleBuilder.ToTable("BillingCycle");

		cycleBuilder.Property(bc => bc.From)
			.IsRequired();

		cycleBuilder.Property(bc => bc.To)
			.IsRequired();

		cycleBuilder
			.Property(x => x.VirtualCardId)
			.IsRequired();
			//.HasConversion(x => x.Id, id => CatalogId.Of(id));

	}
}