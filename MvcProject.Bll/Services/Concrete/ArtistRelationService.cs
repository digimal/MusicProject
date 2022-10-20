using MvcProject.Domain;
using MvcProject.Bll.ViewModels.Artist;
using MvcProject.Bll.ViewModels.Common;
using AutoMapper;
using MvcProject.Bll.Services.Abstract;
using MvcProject.Dal;

namespace MvcProject.Bll.Services.Concrete
{
    public class ArtistRelationService : BaseService<ArtistRelation>, IArtistRelationService
    {
        public ArtistRelationService(MusicContext context, IMapper mapper) : base(context, mapper) { }

        public MemberViewModel Create(MemberViewModel viewModel, RelationKeyViewModel keyModel)
        {
            Create(Generate(viewModel, keyModel));
            return viewModel;
        }

        public void Update(MemberViewModel viewModel, RelationKeyViewModel keyModel)
        {

            Update(Generate(viewModel, keyModel));
        }

        private ArtistRelation Generate(MemberViewModel viewModel, RelationKeyViewModel keyModel)
        {
            var artistRelation = new ArtistRelation()
            {
                TypeId = keyModel.Id,
                Interval = mapper.Map<TimeIntervalViewModel, TimeInterval>(viewModel.Interval)
            };
            if (keyModel.IsSource)
            {
                artistRelation.SourceId = viewModel.ArtistId;
                artistRelation.TargetId = viewModel.Id;
            }
            else
            {
                artistRelation.SourceId = viewModel.Id;
                artistRelation.TargetId = viewModel.ArtistId;
            }
            return artistRelation;
        }

        public void Delete(MemberViewModel viewModel, RelationKeyViewModel keyModel)
        {
            if (keyModel.IsSource)
            {
                DeleteOneIf(x => x.TypeId == keyModel.Id && x.SourceId == viewModel.ArtistId && x.TargetId == viewModel.Id);
            }
            else
            {
                DeleteOneIf(x => x.TypeId == keyModel.Id && x.SourceId == viewModel.Id && x.TargetId == viewModel.ArtistId);
            }
        }

        public void DeleteAllArtistRelations(int artistId)
        {
            DeleteManyIf(x => artistId == x.SourceId || artistId == x.TargetId);
        }
    }
}
