using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using UserManagement.Data.Entities;
using UserManagement.Models;

namespace UserManagement.Data;

public class DataContext : DbContext, IDataContext
{
    public DataContext(DbContextOptions<DataContext> options)
        : base(options)
    {
        Database.EnsureCreated(); // ensures in-memory DB is created
    }

    // EF DbSets
    public DbSet<User> Users { get; set; }
    public DbSet<UserActionLog> UserActionLogs { get; set; }

    protected override void OnModelCreating(ModelBuilder model)
    {
        // Seed Users
        model.Entity<User>().HasData(new[]
        {
            new User { Id = 1, Forename = "Peter", Surname = "Loew", Email = "ploew@example.com", IsActive = true, DateOfBirth = new DateOnly(1980, 3, 12) },
            new User { Id = 2, Forename = "Benjamin Franklin", Surname = "Gates", Email = "bfgates@example.com", IsActive = true, DateOfBirth = new DateOnly(1974, 11, 28) },
            new User { Id = 3, Forename = "Castor", Surname = "Troy", Email = "ctroy@example.com", IsActive = false, DateOfBirth = new DateOnly(1976, 8, 19) },
            new User { Id = 4, Forename = "Memphis", Surname = "Raines", Email = "mraines@example.com", IsActive = true, DateOfBirth = new DateOnly(1972, 5, 3) },
            new User { Id = 5, Forename = "Stanley", Surname = "Goodspeed", Email = "sgodspeed@example.com", IsActive = true, DateOfBirth = new DateOnly(1982, 9, 17) },
            new User { Id = 6, Forename = "H.I.", Surname = "McDunnough", Email = "himcdunnough@example.com", IsActive = true, DateOfBirth = new DateOnly(1970, 12, 25) },
            new User { Id = 7, Forename = "Cameron", Surname = "Poe", Email = "cpoe@example.com", IsActive = false, DateOfBirth = new DateOnly(1977, 1, 10) },
            new User { Id = 8, Forename = "Edward", Surname = "Malus", Email = "emalus@example.com", IsActive = false, DateOfBirth = new DateOnly(1985, 6, 7) },
            new User { Id = 9, Forename = "Damon", Surname = "Macready", Email = "dmacready@example.com", IsActive = false, DateOfBirth = new DateOnly(1990, 4, 2) },
            new User { Id = 10, Forename = "Johnny", Surname = "Blaze", Email = "jblaze@example.com", IsActive = true, DateOfBirth = new DateOnly(1983, 9, 30) },
            new User { Id = 11, Forename = "Robin", Surname = "Feld", Email = "rfeld@example.com", IsActive = true, DateOfBirth = new DateOnly(1978, 2, 11) }
        });

        // Seed Logs
        model.Entity<UserActionLog>().HasData(new[]
        {
            new UserActionLog { Id = 1, UserId = 1, Action = "Created user", Timestamp = DateTime.Now.AddDays(-5) },
            new UserActionLog { Id = 2, UserId = 1, Action = "Updated email", Timestamp = DateTime.Now.AddDays(-2) },
            new UserActionLog { Id = 3, UserId = 2, Action = "Deactivated account", Timestamp = DateTime.Now.AddDays(-1) },
            new UserActionLog { Id = 4, UserId = 3, Action = "Viewed profile", Timestamp = DateTime.Now },
        });
    }

    // Generic CRUD methods
    public async Task<List<TEntity>> GetAllAsync<TEntity>() where TEntity : class
    {
        return await Set<TEntity>().ToListAsync();
    }

    public async Task CreateAsync<TEntity>(TEntity entity) where TEntity : class
    {
        await Set<TEntity>().AddAsync(entity);
        await SaveChangesAsync();
    }

    public async Task UpdateAsync<TEntity>(TEntity entity) where TEntity : class
    {
        Set<TEntity>().Update(entity);
        await SaveChangesAsync();
    }

    public async Task DeleteAsync<TEntity>(TEntity entity) where TEntity : class
    {
        Set<TEntity>().Remove(entity);
        await SaveChangesAsync();
    }


    //Helper method to delete all entities before initializing logs
    public async Task DeleteAllAsync<TEntity>(IEnumerable<TEntity> entities) where TEntity : class
    {
        Set<TEntity>().RemoveRange(entities);
        await SaveChangesAsync();
    }
}
