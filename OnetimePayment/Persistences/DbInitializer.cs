using Microsoft.EntityFrameworkCore;

namespace OnetimePayment;

public static class DbInitializer
{
    public static async Task InitializeAsync(this WebApplication app)
    {
        var dbContext = app.Services.GetRequiredService(typeof(ApplicationDbContext)) as ApplicationDbContext ?? throw new Exception("ApplicationDbContext not found");

        if (dbContext.Database.GetPendingMigrations().Any())
        {
            await dbContext.Database.MigrateAsync();
        }
    }
}
