using MilenaLisov.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MilenaLisov.Interfaces
{
    public interface ILanacRepository
    {
        IEnumerable<Lanac> GetAll();
        Lanac GetById(int id);
        IEnumerable<Lanac> Najstariji();
    }
}
