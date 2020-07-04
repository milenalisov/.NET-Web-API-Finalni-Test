using MilenaLisov.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using MilenaLisov.Models;

namespace MilenaLisov.Repository
{
    public class LanacRepository : IDisposable, ILanacRepository
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

        public IEnumerable<Lanac> GetAll()
        {
            return db.Lanci;
        }

        public Lanac GetById(int id)
        {
            return db.Lanci.Find(id) ;
        }

        public IEnumerable<Lanac> Najstariji()
        {
            var lanci = GetAll().OrderBy(x => x.Godina);
            List<Lanac> rezultat = new List<Lanac>();
            rezultat.Add(lanci.ElementAt(0));
            rezultat.Add(lanci.ElementAt(1));
            return rezultat.AsEnumerable();
        }
    }
}