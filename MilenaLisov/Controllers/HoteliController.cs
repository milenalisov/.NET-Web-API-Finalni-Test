using MilenaLisov.Interfaces;
using MilenaLisov.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using System.Web.Http.Description;

namespace MilenaLisov.Controllers
{
    public class HoteliController : ApiController
    {
        IHotelRepository _repository { get; set; }

        public HoteliController(IHotelRepository repository)
        {
            _repository = repository;
        }

        public IEnumerable<Hotel> GetHoteli()
        {
            return _repository.GetAll();
        }

        public IHttpActionResult GetHotel(int id)
        {
            var hotel = _repository.GetById(id);

            if (hotel == null)
            {
                return NotFound();
            }

            return Ok(hotel);
        }

        public IHttpActionResult GetZaposleni(int zaposleni)
        {
            var hotel = _repository.GetZaposleni(zaposleni);

            if (hotel == null)
            {
                return NotFound();
            }

            return Ok(hotel);
        }

        [ResponseType(typeof(Hotel))]
        [Authorize]
        public IHttpActionResult PostHotel(Hotel hotel)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            _repository.Add(hotel);
            var resenje = _repository.PrikazHotela(hotel).ToList();
 
            return CreatedAtRoute("DefaultApi", new { id = resenje[0].Id }, resenje);
        }

        private bool HotelExists(int id)
        {
            return _repository.GetAll().Count(x => x.Id == id) > 0;
        }

        [ResponseType(typeof(Hotel))]
     
        public IHttpActionResult PutHotel(int id, Hotel hotel)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (id != hotel.Id)
            {
                return BadRequest();
            }

            try
            {
                _repository.Update(hotel);
            }
            catch
            {
                if (!HotelExists(id))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }

            }

            return Ok(_repository.PrikazHotela(hotel));
        }

        [ResponseType(typeof(void))]
        [Authorize]
        public IHttpActionResult DeleteHotel(int id)
        {
            var hotel = _repository.GetById(id);

            if (hotel == null)
            {
                return NotFound();
            }

            _repository.Delete(hotel);
            return Ok();
        }

        public class ApiModel
        {
            public int najmanje { get; set; }
            public int najvise { get; set; }
        }

    
        [Route("api/kapacitet")]
        [Authorize]
        public IHttpActionResult PostKapacitet([FromBody]ApiModel model)
        {
            var rezultat = _repository.Kapacitet(model.najmanje, model.najvise);
            if (rezultat == null)
            {
                return NotFound();
            }

            return Ok(rezultat);
        }

        [Route("api/zaposleni")]
        public IEnumerable<LanacDTO> GetLanci()
        {
            return _repository.ProsekZaposleni();
        }

        public class ApiModel2
        {
            public int broj { get; set; }
        }


        [Route("api/sobe")]
        public IHttpActionResult PostSobe([FromBody]ApiModel2 model)
        {
            var rezultat = _repository.PostLanci(model.broj);
            if (rezultat == null)
            {
                return NotFound();
            }

            return Ok(rezultat);
        }

    }
}
