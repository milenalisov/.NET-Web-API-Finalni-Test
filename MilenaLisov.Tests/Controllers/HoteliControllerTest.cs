using Microsoft.VisualStudio.TestTools.UnitTesting;
using MilenaLisov.Controllers;
using MilenaLisov.Interfaces;
using MilenaLisov.Models;
using Moq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web.Http;
using System.Web.Http.Results;
using static MilenaLisov.Controllers.HoteliController;

namespace MilenaLisov.Tests.Controllers
{
    [TestClass]
    public class HoteliControllerTest
    {
        [TestMethod]
        public void GetReturnsHotelWithSameId()
        {
            var mockRepository = new Mock<IHotelRepository>();
            mockRepository.Setup(x => x.GetById(1)).Returns(new Hotel { Id = 1, Naziv = "Hotel1", Godina=1995, Sobe=250, Zaposleni=156 });

            var controller = new HoteliController(mockRepository.Object);

            IHttpActionResult actionResult = controller.GetHotel(1);
            var contentResult = actionResult as OkNegotiatedContentResult<Hotel>;


            Assert.IsNotNull(contentResult);
            Assert.IsNotNull(contentResult.Content);
            Assert.AreEqual(1, contentResult.Content.Id);
        }

        [TestMethod]
        public void PutReturnBadRequest()
        {

            var mockRepository = new Mock<IHotelRepository>();
            var controller = new HoteliController(mockRepository.Object);

            IHttpActionResult actionResult = controller.PutHotel(12, new Hotel { Id = 1, Naziv = "Hotel1", Godina = 1995, Sobe = 250, Zaposleni = 156 });

            Assert.IsInstanceOfType(actionResult, typeof(BadRequestResult));
        }

        [TestMethod]
        public void GetReturnsMultipleObjects()
        {
            List<Hotel> hoteli = new List<Hotel>();
            hoteli.Add(new Hotel { Id = 1, Naziv = "Hotel1", Godina = 1995, Sobe = 250, Zaposleni = 156 });
            hoteli.Add(new Hotel { Id = 2, Naziv = "Hotel2", Godina = 1996, Sobe = 252, Zaposleni = 158 });

            var mockRepository = new Mock<IHotelRepository>();
            mockRepository.Setup(x => x.GetAll()).Returns(hoteli.AsEnumerable());

            var controller = new HoteliController(mockRepository.Object);

            IEnumerable<Hotel> result = controller.GetHoteli();

            Assert.IsNotNull(result);
            Assert.AreEqual(hoteli.Count, result.ToList().Count);
            Assert.AreEqual(hoteli.ElementAt(0), result.ElementAt(0));
            Assert.AreEqual(hoteli.ElementAt(1), result.ElementAt(1));

        }

        [TestMethod]
        public void KapacitetReturnsMultipleObjects()
        {
            List<Hotel> hoteli = new List<Hotel>();
            hoteli.Add(new Hotel { Id = 1, Naziv = "Hotel1", Godina = 1995, Sobe = 250, Zaposleni = 156 });
            hoteli.Add(new Hotel { Id = 2, Naziv = "Hotel2", Godina = 1996, Sobe = 252, Zaposleni = 158 });

            var mockRepository = new Mock<IHotelRepository>();
            mockRepository.Setup(x => x.Kapacitet(200, 500)).Returns(hoteli.OrderBy(x => x.Sobe).AsEnumerable());

            var controller = new HoteliController(mockRepository.Object);

            var model = new ApiModel { najmanje = 200, najvise = 500 };

            IHttpActionResult result = controller.PostKapacitet(model);
            var createdResult = result as OkNegotiatedContentResult<IEnumerable<Hotel>>;

            Assert.IsNotNull(createdResult);
            Assert.IsNotNull(createdResult.Content);
            Assert.AreEqual(createdResult.Content.ToList().Count(), hoteli.Count());
            Assert.AreEqual(createdResult.Content.ElementAt(0), hoteli.ElementAt(0));
            Assert.AreEqual(createdResult.Content.ElementAt(1), hoteli.ElementAt(1));
        }

    }
}
