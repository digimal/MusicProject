using AutoMapper;
using Microsoft.EntityFrameworkCore;
using Moq;
using MvcProject.Bll.Services.Concrete;
using MvcProject.Bll.ViewModels.Artist;
using MvcProject.Dal;
using MvcProject.Domain;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MvcProject.UnitTests.Services
{
    internal class ArtistServiceTests
    {
        private MusicContext musicContext;
        private IMapper mapper;
        private ArtistService artistService;

        public ArtistServiceTests()
        {
            mapper = Mocks.AutoMapperMock.InitializeAutoMapper().CreateMapper();
        }

        [SetUp]
        public void Setup()
        {
            musicContext = Mocks.DbContextMock.GetContext();
            artistService = new ArtistService(musicContext, mapper);
        }

        [TearDown]
        public void Teardown()
        {
            musicContext.Dispose();
        }

        [Test]
        public void Should_Create_Artist()
        {
            // AAA
            // assign
            var artist = new ArtistViewModel
            {
                Aliases = "test",
                Description = "test",
                Name = "Test",
                Interval = new Bll.ViewModels.Common.TimeIntervalViewModel()
            };

            // act
            var artistModel = artistService.Create(artist);

            // assert
            Assert.IsTrue(musicContext.Artists.Count() == 1);
            Assert.AreEqual(artistModel.Name, artist.Name);
        }

        [Test]
        public void Should_Update_Artist_Moq()
        {

            // assign
            var artistMock = new Mock<ArtistViewModel>();

            var artist = artistMock.Object;
            artist.Interval = new Bll.ViewModels.Common.TimeIntervalViewModel();

            // act
            artistService.Create(artist);

            // assert
            Assert.IsTrue(musicContext.Artists.Count() == 1);
        }
    }
}
