using System;
using System.Threading.Tasks;
using CF.VirtualCard.Infrastructure.DbContext;

namespace CF.IntegrationTest.Seeds;

public class VirtualCardSeed
{
    public static async Task Populate(VirtualCardContext dbContext)
    {
        await dbContext.VirtualCards.AddAsync(new VirtualCard.Domain.Entities.VirtualCard
        {
            CardNumber = "1234-1234-1234-1232",
            FirstName = "Seed",
            Surname = "Seed",
            ExpiryDate = DateTime.Now
        });

        await dbContext.SaveChangesAsync();
    }
}