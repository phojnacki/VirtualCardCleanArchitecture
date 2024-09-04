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
            Email = "seed.record@test.com",
            Password = "Rgrtgr#$543gfregeg",
            FirstName = "Seed",
            Surname = "Seed",
            Created = DateTime.Now,
            Updated = DateTime.Now
        });

        await dbContext.SaveChangesAsync();
    }
}