using Application_Auth.Models;
using Microsoft.EntityFrameworkCore;

namespace Application_Auth.Data;

public class AppDbContext : DbContext
{
    public AppDbContext(DbContextOptions<AppDbContext> options) : base(options)
    {
        
    }
    
    public DbSet<AppUser> AppUsers { get; set; }
     
    public DbSet<UserNote> UserNotes { get; set; }


    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
       modelBuilder.Entity<AppUser>().HasMany(usn => usn.UserNotes)
           .WithOne(apu => apu.AppUser)
           .HasForeignKey(apuI => apuI.AppUserId)
           .OnDelete(DeleteBehavior.Cascade); // delete notes if the user is deleted 
       
       modelBuilder.Entity<AppUser>().HasIndex(u => u.Email).IsUnique();
    }
    
}