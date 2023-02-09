
using Microsoft.EntityFrameworkCore;

var db = new Database();
db.Database.Migrate();

var now = DateTime.Now;
var dance = db.Dances.First(d => d.DayOfWeek == DayOfWeek.Saturday);

var next = Next(now, dance.DayOfWeek);

Console.WriteLine($"The next \"{dance.Name}\" is on {next:d}");

static DateTime Next(DateTime from, DayOfWeek dayOfTheWeek)
{
    var date = from.Date.AddDays(1);
    var days = ((int)dayOfTheWeek - (int)date.DayOfWeek + 7) % 7;
    return date.AddDays(days);
}

public class Database : DbContext
{
    public DbSet<Dance> Dances => Set<Dance>();

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        => optionsBuilder.UseSqlite("Data Source=database.db");

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder
            .Entity<Dance>()
            .Property(p => p.DayOfWeek)
            .HasColumnType("TINYINT")
            .HasConversion(
                v => (byte)v,
                v => (DayOfWeek)v    
            );
            
        modelBuilder
            .Entity<Dance>()
            .HasData(new Dance
            {
                Id = 1,
                Name = "Ho Down Spectacular!",
                DayOfWeek = DayOfWeek.Saturday
            });
    }
}

public class Dance
{
    public int Id { get; set; }
    public string Name { get; set; } = "";
    public DayOfWeek DayOfWeek { get; set; } = DayOfWeek.Sunday;
}