using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace DeltaShareApi.Data
{
    public class AppDbContext(DbContextOptions options) : IdentityDbContext(options)
    {
    }
}
