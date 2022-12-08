using Microsoft.EntityFrameworkCore;
using MvcProject.Dal;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MvcProject.UnitTests.Mocks
{
    internal static class DbContextMock
    {
        public static MusicContext GetContext()
        {
            var contextOptions = new DbContextOptionsBuilder<MusicContext>()
                .UseInMemoryDatabase("MusicProject")
                .Options;

            var context = new MusicContext(contextOptions);

            return context;
        }
    }
}
