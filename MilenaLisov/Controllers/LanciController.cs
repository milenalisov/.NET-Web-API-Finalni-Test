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
    public class LanciController : ApiController
    {
        ILanacRepository _repository { get; set; }

        public LanciController(ILanacRepository repository)
        {
            _repository = repository;
        }

        public IEnumerable<Lanac> GetLanci()
        {
            return _repository.GetAll();
        }

        [ResponseType(typeof(Lanac))]
        public IHttpActionResult GetLanac(int id)
        {
            var lanac = _repository.GetById(id);
            if(lanac==null)
            {
                return NotFound();

            }

            return Ok(lanac);
        }

        [Route("api/tradicija")]
        [Authorize]
        public IEnumerable<Lanac> GetTradicija()
        {
            return _repository.Najstariji();
        }

    }
}
