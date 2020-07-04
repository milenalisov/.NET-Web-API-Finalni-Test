namespace MilenaLisov.Migrations
{
    using MilenaLisov.Models;
    using System;
    using System.Data.Entity;
    using System.Data.Entity.Migrations;
    using System.Linq;

    internal sealed class Configuration : DbMigrationsConfiguration<MilenaLisov.Models.ApplicationDbContext>
    {
        public Configuration()
        {
            AutomaticMigrationsEnabled = false;
        }

        protected override void Seed(MilenaLisov.Models.ApplicationDbContext context)
        {
            context.Lanci.AddOrUpdate(
                new Lanac { Id=1, Naziv="Hilton Worldwide", Godina=1919},
                new Lanac { Id = 2, Naziv = "Marriott International", Godina = 1927 },
                new Lanac { Id = 3, Naziv = "Kempinski", Godina = 1897 }
                );
            context.SaveChanges();

            context.Hoteli.AddOrUpdate(
                new Hotel { Id=1, Naziv="Sheraton Novi Sad", Godina=2018, Zaposleni=70, Sobe=150, LanacId=2},
                new Hotel { Id = 2, Naziv = "Hilton Belgrade", Godina = 2017, Zaposleni = 100, Sobe = 242, LanacId = 1 },
                new Hotel { Id = 3, Naziv = "Palais Hansen", Godina = 2013, Zaposleni = 80, Sobe = 152, LanacId = 3 },
                new Hotel { Id = 4, Naziv = "Budapest Marriott", Godina = 1994, Zaposleni = 130, Sobe = 364, LanacId = 2 },
                new Hotel { Id = 5, Naziv = "Hilton Berlin", Godina = 1991, Zaposleni = 200, Sobe = 601, LanacId = 1 }
                );
            context.SaveChanges();

        }
    }
}
