using System;
using System.Web.Mvc;
using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Collections.Generic;
using Mobsway.Data.Persistence;
using Mobsway.Web.Controllers;
using Mobsway.Web.Models;
using Moq;
using Mobsway.Business.Entity;

namespace Mobsway.Web.Tests.Controllers
{
    [TestClass]
    public class PollControllerTest
    {
        private List<Poll> polls = null;
        private ExtraData ed = null;

        [TestInitialize]
        public void Setup()
        {
            polls = new List<Poll>()
            {
                {new Poll(){
                    PollNumber = "weruhwe",
                    HashTag = "TestTag",
                    StartDate = DateTime.Today,
                    EndDate = DateTime.Today.AddDays(3),
                     CreatedBy = "facebook/gyochum",
                     CreatedDate = DateTime.Today,
                     IsActive = true,
                     Description = "test poll 1",
                     ImageProvider = ImageProviderTypes.Facebook
                }},
                {new Poll(){
                    PollNumber = "dfdydy",
                    HashTag = "TestTag1",
                    StartDate = DateTime.Today,
                    EndDate = DateTime.Today.AddDays(3),
                     CreatedBy = "twitter/gyochum",
                     CreatedDate = DateTime.Today,
                     IsActive = true,
                     Description = "test poll 2",
                     ImageProvider = ImageProviderTypes.Flickr
                }}
            };

            ed = new ExtraData()
            {
                Id = "gyochum",
                Email = "gyochum@gmail.com",
                Name = "Gary Yochum"
            };
        }

        [TestMethod]
        public void PollController_Index_Returns_ActionResult()
        {
            ViewResult a = null;

            PollsController controller = new PollsController(null);

            a = controller.Index(null) as ViewResult;

            Assert.IsNotNull(controller);
            Assert.IsNotNull(a);

        }

        [TestMethod]
        public void PollsController_Index_Returns_PollViewModel()
        {
            ViewResult actual = null;

            PollsController controller = new PollsController(null);

            actual = controller.Index(null) as ViewResult;

            Assert.IsNotNull(actual.Model);
            Assert.IsInstanceOfType(actual.Model, typeof(PollViewModel));
        }

        [TestMethod]
        public void PollsController_Index_Returns_PollViewModel_With_Polls()
        {
            ViewResult actual = null;

            Mock<IPollRepository> repository = new Mock<IPollRepository>();

            repository.Setup(x => x.GetUserPolls("gyochum")).Returns(polls);

            PollsController controller = new PollsController(repository.Object);

            actual = controller.Index(ed) as ViewResult;

            Assert.IsNotNull(actual.Model);
            Assert.IsInstanceOfType(actual.Model, typeof(PollViewModel));

            PollViewModel model = actual.Model as PollViewModel;

            Assert.IsNotNull(model.Polls);
        }

        [TestMethod]
        public void PollsController_Index_Returns_PollViewModel_With_3Polls_1Expired()
        {
            ViewResult actual = null;

            polls.Add(new Poll()
            {
                PollNumber = "kdsjsj",
                HashTag = "TestTag2",
                StartDate = DateTime.Today.AddDays(-10),
                EndDate = DateTime.Today.AddDays(-3),
                CreatedBy = "twitter/gyochum",
                CreatedDate = DateTime.Today,
                IsActive = true,
                Description = "test poll 2",
                ImageProvider = ImageProviderTypes.Flickr
            });

            Mock<IPollRepository> r = new Mock<IPollRepository>();

            r.Setup(x => x.GetUserPolls("gyochum")).Returns(polls);

            var controller = new PollsController(r.Object);

            actual = controller.Index(ed) as ViewResult;

            Assert.IsNotNull(actual.Model);
            Assert.IsInstanceOfType(actual.Model, typeof(PollViewModel));

            PollViewModel model = actual.Model as PollViewModel;

            Assert.IsNotNull(model.Polls);
            Assert.AreEqual<int>(3, model.Polls.Count);

            var expired = from p in model.Polls
                          where p.EndDate < DateTime.Today
                          select p;

            Assert.IsNotNull(expired);
            Assert.AreEqual<int>(1, expired.Count<Poll>());
                          

        }

        [TestCleanup]
        public void TearDown()
        {
            polls = null;
            ed = null;
        }
    }
}
