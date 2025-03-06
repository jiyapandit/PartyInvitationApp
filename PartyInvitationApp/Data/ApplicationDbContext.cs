using Microsoft.EntityFrameworkCore;
using PartyInvitationApp.Models;
using System;

namespace PartyInvitationApp.Data
{
    public class ApplicationDbContext : DbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options) { }

        public DbSet<Party> Parties { get; set; }
        public DbSet<Invitation> Invitations { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            // Seed Parties 
            modelBuilder.Entity<Party>().HasData(
                new Party { Id = 11, Description = "Cricket Champions Trophy", DateOfParty = new DateTime(2025, 3, 9), Location = "Dubai International Stadium" }
             
            );

            // Seed Invitations 
            modelBuilder.Entity<Invitation>().HasData(
                new Invitation { Id = 20, GuestName = "Virat Kohli", GuestEmail = "jpandit5253@conestogac.on.ca", PartyId = 11, Status = InvitationStatus.InviteNotSent },

                new Invitation { Id = 21, GuestName = "MS Dhoni", GuestEmail = "jpandit5253@conestogac.on.ca", PartyId = 11, Status = InvitationStatus.InviteNotSent },

                new Invitation { Id = 22, GuestName = "Rohit Sharma", GuestEmail = "jpandit5253@conestogac.on.ca", PartyId = 11, Status = InvitationStatus.InviteNotSent }


            );
        }
    }
}
