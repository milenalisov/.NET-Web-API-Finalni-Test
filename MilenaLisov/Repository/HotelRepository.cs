using MilenaLisov.Interfaces;
using MilenaLisov.Models;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Data.Entity.Infrastructure;
using System.Linq;
using System.Web;

namespace MilenaLisov.Repository
{
    public class HotelRepository :IDisposable, IHotelRepository
    {
        private ApplicationDbContext db = new ApplicationDbContext();

        protected void Dispose(bool disposing)
        {
            if (disposing)
            {
                if (db != null)
                {
                    db.Dispose();
                    db = null;
                }
            }
        }
        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        public IEnumerable<Hotel> GetAll()
        {
            return db.Hoteli.Include(x => x.Lanac).OrderBy(x => x.Godina);
        }

        public Hotel GetById(int id)
        {
            return db.Hoteli.Include(x => x.Lanac).SingleOrDefault(x => x.Id==id);
        }

        public IEnumerable<Hotel> GetZaposleni(int zaposleni)
        {
            return db.Hoteli.Include(x => x.Lanac).Where(x => x.Zaposleni >= zaposleni).OrderBy(x => x.Zaposleni);
        }

        public void Add(Hotel hotel)
        {
            db.Hoteli.Add(hotel);
            db.SaveChanges();
        }

        public IEnumerable<Hotel> PrikazHotela(Hotel hotel)
        {
            return db.Hoteli.Include(x => x.Lanac).Where(x => x.Id == hotel.Id);
        }
        public void Update(Hotel hotel)
        {
            db.Entry(hotel).State = EntityState.Modified;
            try
            {
                db.SaveChanges();
            }
            catch (DbUpdateConcurrencyException)
            {

                throw;
            }
        }

        public void Delete(Hotel hotel)
        {
            db.Hoteli.Remove(hotel);
            db.SaveChanges();
        }

        public IEnumerable<Hotel> Kapacitet(int najmanje, int najvise)
        {
            return db.Hoteli.Include(x => x.Lanac).Where(x => x.Sobe >= najmanje && x.Sobe<= najvise).OrderByDescending(x => x.Sobe);
        }

        public IEnumerable<LanacDTO> ProsekZaposleni()
        {
            var hoteli = GetAll();
            var resenje = hoteli.GroupBy(
                x => x.Lanac,
                x => x.Zaposleni,
                (lanac, zaposleni) => new LanacDTO
                {
                    Lanac = lanac,
                    ProsekZaposlenih = zaposleni.Sum()/zaposleni.Count()
                }).OrderByDescending(x => x.ProsekZaposlenih);

            return resenje;
        }

        public IEnumerable<LanciSobeDTO> PostLanci(int broj)
        {
            var hoteli = GetAll();
            var resenje = hoteli.GroupBy(
                x => x.Lanac,
                x => x.Sobe,
                (lanac, sobe) => new LanciSobeDTO
                {
                    Lanac = lanac,
                    SumSoba = sobe.Sum().GetValueOrDefault()
                }).OrderBy(x => x.SumSoba);

            var prikazi = resenje.Where(x => x.SumSoba > broj);
            return prikazi;
        }
    }
}
