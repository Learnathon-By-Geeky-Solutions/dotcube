using AvantiPoint.MobileAuth.Stores;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace DeltaShareApi.Data;

public class UserContext : IdentityDbContext, ITokenStore
{
    public UserContext(DbContextOptions options)
        : base(options)
    {
    }

    public DbSet<AuthorizedToken> AuthorizedTokens { get; set; }

    public DbSet<UserRole> UserRoles { get; set; }

    public async ValueTask AddToken(string jwt, DateTimeOffset expires)
    {
        await AuthorizedTokens.AddAsync(new AuthorizedToken()
        {
            Token = jwt
        });
        await SaveChangesAsync();
    }

    public async ValueTask RemoveToken(string jwt)
    {
        var tokens = await AuthorizedTokens.Where(x => x.Token == jwt).ToArrayAsync();
        if (tokens.Any())
        {
            AuthorizedTokens.RemoveRange(tokens);
            await SaveChangesAsync();
        }
    }

    public async ValueTask<bool> TokenExists(string jwt) =>
        await AuthorizedTokens.AnyAsync(x => x.Token == jwt);
}
