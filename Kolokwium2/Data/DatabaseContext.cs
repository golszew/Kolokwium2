using Kolokwium2.Models;
using Microsoft.EntityFrameworkCore;

namespace Kolokwium2.Data;

public class DatabaseContext : DbContext
{
    protected DatabaseContext()
    {
    }

    public DatabaseContext(DbContextOptions options) : base(options)
    {
    }

    public DbSet<Backpack> Backpacks { get; set; }
    public DbSet<Character> Characters { get; set; }
    public DbSet<CharacterTitle> CharacterTitles { get; set; }
    public DbSet<Item> Items { get; set; }
    public DbSet<Title> Titles { get; set; }
    
    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        modelBuilder.Entity<Backpack>().HasData(new List<Backpack>
        {
            new Backpack
            {
                CharacterId = 1,
                ItemId = 1,
                Amount = 20
            },
            new Backpack
            {
                CharacterId = 1,
                ItemId = 2,
                Amount = 5
            },
            new Backpack
            {
                CharacterId = 2,
                ItemId = 3,
                Amount = 1
            },
        });
        
        modelBuilder.Entity<Character>().HasData(new List<Character>
        {
            new Character
            {
                Id = 1,
                FirstName = "Wojciech",
                LastName = "Pazucha",
                CurrentWeight = 600,
                MaxWeight = 1000
            },
            new Character
            {
                Id = 2,
                FirstName = "Wiktor",
                LastName = "Zdobywac",
                CurrentWeight = 30,
                MaxWeight = 100
            },
            new Character
            {
                Id = 3,
                FirstName = "Witold",
                LastName = "Niemiec",
                CurrentWeight = 10,
                MaxWeight = 150
            },
        });
        
        modelBuilder.Entity<CharacterTitle>().HasData(new List<CharacterTitle>
        {
            new CharacterTitle
            {
                CharacterId = 1,
                TitleId = 1,
                AcquiredAt = DateTime.Now
            },
            new CharacterTitle
            {
                CharacterId = 2,
                TitleId = 2,
                AcquiredAt = DateTime.Now
            },
            new CharacterTitle
            {
                CharacterId = 3,
                TitleId = 3,
                AcquiredAt = DateTime.Now
            },
        });
        
        modelBuilder.Entity<Item>().HasData(new List<Item>
        {
            new Item
            {
                Id = 1,
                Name = "Miecz",
                Weight = 20
            },
            new Item
            {
                Id = 2,
                Name = "Eliksir zdrowia",
                Weight = 40
            },
            new Item
            {
                Id = 3,
                Name = "Tarcza",
                Weight = 10
            },
        });
        
        modelBuilder.Entity<Title>().HasData(new List<Title>
        {
            new Title
            {
                Id = 1,
                Name = "Paladyn"
            },
            new Title
            {
                Id = 2,
                Name = "Wiedzmin"
            },
            new Title
            {
                Id = 3,
                Name = "Tancerz ostrzy"
            },
        });
    }

}