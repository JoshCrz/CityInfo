using CityInfo.API.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CityInfo.API.Contexts
{
    public class CityInfoContext : DbContext
    {
        //db sets can be used to create queries and save instances of that entity type, so linq queries against the dbset will be translated into queries
        //against the db
        public DbSet<City> Cities { get; set; }
        public DbSet<PointOfInterest> PointsOfInterest { get; set; }

        public CityInfoContext(DbContextOptions<CityInfoContext> options)
            : base(options)
        {
         //   Database.EnsureCreated();
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            //seeding the database, seeding is creating records (usually master data) entered on initial creation
            modelBuilder.Entity<City>()
                .HasData(
                new City()
                {
                    Id = 1,
                    Name = "New York City",
                    Description = "The one with the big park."
                },
                new City()
                {
                    Id = 2,
                    Name = "Antwerp",
                    Description = "The one with the cathedral that was never really finished."
                },
                new City()
                {
                    Id = 3,
                    Name = "Paris",
                    Description = "The one with that big tower."
                });

            modelBuilder.Entity<PointOfInterest>()
                .HasData(
                new PointOfInterest()
                {
                    Id = 1,
                    CityId = 1,
                    Name = "Central Park",
                    Description = "A very big park..."
                },
                new PointOfInterest()
                {
                    Id = 3,
                    CityId = 2,
                    Name = "Cathedral of our Lady",
                    Description = "A very big cathedral..."
                },
                new PointOfInterest()
                {
                    Id = 5,
                    CityId = 3,
                    Name = "Eiffel Tower",
                    Description = "A very big tower..."
                });

            //after entering this, we added a migration so this dummy data was used for seeding the database

            base.OnModelCreating(modelBuilder);
        }


    }
}
