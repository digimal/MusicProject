using AutoMapper;
using MvcProject.Dal;
using MvcProject.Domain;

namespace MvcProject.Bll.Services.Concrete
{
    public class AuthorshipService : BaseService<Author>
    {
        public AuthorshipService(MusicContext context, IMapper mapper) : base(context, mapper) { }

        public void Create(int id)
        {
            if (!AlreadyExists(x => x.Id == id))
            {
                Create(new Author() { Id = id });
            }
        }

        public void Delete(int id)
        {
            DeleteOneIf(x => x.Id == id);
        }
    }
}