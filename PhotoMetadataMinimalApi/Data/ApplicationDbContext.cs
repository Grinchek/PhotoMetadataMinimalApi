using Microsoft.EntityFrameworkCore;
using PhotoMetadataMinimalApi.Models;
using System.Collections.Generic;

namespace PhotoMetadataMinimalApi.Data;

public class ApplicationDbContext : DbContext
{
    public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options) { }

    public DbSet<User> Users => Set<User>();
    public DbSet<Photo> Photos => Set<Photo>();
}
