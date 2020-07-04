using MilenaLisov.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MilenaLisov.Interfaces
{
    public interface IHotelRepository
    {
        IEnumerable<Hotel> GetAll();
        Hotel GetById(int id);
        IEnumerable<Hotel> GetZaposleni(int zaposleni);
        void Add(Hotel hotel);
        void Update(Hotel hotel);
        void Delete(Hotel hotel);
        IEnumerable<Hotel> Kapacitet(int najmanje, int najvise);
        IEnumerable<LanacDTO> ProsekZaposleni();
        IEnumerable<LanciSobeDTO> PostLanci(int broj);
        IEnumerable<Hotel> PrikazHotela(Hotel hotel);
    }
}
