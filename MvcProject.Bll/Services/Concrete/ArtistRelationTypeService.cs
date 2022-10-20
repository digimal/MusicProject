using AutoMapper;
using MvcProject.Bll.Services.Abstract;
using MvcProject.Bll.ViewModels.Artist;
using MvcProject.Dal;
using MvcProject.Domain;
using System.Collections.Generic;
using System.Linq;

namespace MvcProject.Bll.Services.Concrete
{
    public class ArtistRelationTypeService : BaseService<ArtistRelationType>, IArtistRelationTypeService
    {
        public ArtistRelationTypeService(MusicContext context, IMapper mapper) : base(context, mapper) { }

        public RelationChoiceViewModel GetAll()
        {
            var result = new RelationChoiceViewModel()
            {
                RelationKeys = new Dictionary<int, RelationKeyViewModel>(),
                RelationTitles = new Dictionary<int, string>()
            };
            var relationTypes = GetMany().ToArray();
            int relationId = 0;
            foreach (var type in relationTypes)
            {
                AddToModel(result, type, ref relationId, isSource: true);
                AddToModel(result, type, ref relationId, isSource: false);
            }
            return result;
        }

        private void AddToModel(RelationChoiceViewModel model, ArtistRelationType type, ref int relationId, bool isSource)
        {
            model.RelationKeys.Add(relationId, new RelationKeyViewModel() { Id = type.Id, IsSource = isSource });
            model.RelationTitles.Add(relationId, isSource ? type.SourceMessage: type.TargetMessage);
            ++relationId;
        }
    }
}
