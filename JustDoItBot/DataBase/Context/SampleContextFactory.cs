using FoodDiaryBot.DataBase.Context;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;

namespace FoodDiaryBot.DataBase.Context
{
    class SampleContextFactory : IDesignTimeDbContextFactory<BotDbContext>
    {
        public BotDbContext CreateDbContext(string[] args)
        {
            DbContextOptions<BotDbContext> options = HelperDataBase.DB_OPTIONS;
            return new BotDbContext(options);
        }
    }
}
