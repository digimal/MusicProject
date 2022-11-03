using System.Collections.Generic;
using System.Linq;
using MvcProject.Domain;
using MvcProject.Bll.Services.Abstract;
using MvcProject.Bll.ViewModels.Artist;
using MvcProject.Bll.ViewModels.Common;
using AutoMapper;
using MvcProject.Dal;
using Microsoft.EntityFrameworkCore;

namespace MvcProject.Bll.Services.Concrete
{
    public class ArtistService : BaseService<Artist>, IArtistService
    {
        public ArtistService(MusicContext context, IMapper mapper) : base(context, mapper) { }

        public ArtistViewModel Create(ArtistViewModel artistViewModel)
        {
            return mapper.Map<Artist, ArtistViewModel>(
                Create(
                    mapper.Map<ArtistViewModel, Artist>(
                        artistViewModel)));
        }

        public void Update(ArtistViewModel artistViewModel)
        {
            Update(
                    mapper.Map<ArtistViewModel, Artist>(
                        artistViewModel));
        }

        public void Delete(int artistId)
        {
            DeleteOneIf(x => x.Id == artistId);
        }

        public ArtistShowViewModel GetArtist(int artistId)
        {
            var model = mapper.Map<Artist, ArtistShowViewModel>(GetArtistDomain(artistId));

            return model;
        }

        private Artist GetArtistDomain(int artistId)
        {
            return dbSet.Include(x => x.Picture).FirstOrDefault(x => x.Id == artistId);
        }

        public IEnumerable<ArtistBaseViewModel> GetArtists(int skip, int take)
        {
            return mapper.Map<IEnumerable<Artist>, IEnumerable<ArtistBaseViewModel>>(dbSet.Skip(skip).Take(take));
        }

        public IEnumerable<ArtistBaseViewModel> GetArtists()
        {
            return mapper.Map<IEnumerable<Artist>, IEnumerable<ArtistBaseViewModel>>(dbSet);
        }

        public IEnumerable<TagViewModel> GetTags(int artistId)
        {
            return mapper.Map<IEnumerable<ArtistTag>, IEnumerable<TagViewModel>>(GetOneIf(x => x.Id == artistId)?.AssignedTags.Select(x => x.ArtistTag));
        }

        public IEnumerable<RelationOptionViewModel> GetArtistRelationTypesWhereSource(int artistId)
        {
            return
                GetOneIf(x => x.Id == artistId).RelationsWhereSource.Select(x => x.Type).GroupBy(x => x.Id).Select(x => x.First())
                .Select(x => new RelationOptionViewModel()
                {
                    Id = x.Id,
                    Title = string.IsNullOrEmpty(x.SourceMessage) ? x.Name : x.SourceMessage,
                    IsSource = true
                });

        }

        public IEnumerable<RelationOptionViewModel> GetArtistRelationTypesWhereTarget(int artistId)
        {
            return
                GetOneIf(x => x.Id == artistId).RelationsWhereTarget.Select(x => x.Type).GroupBy(x => x.Id).Select(x => x.First())
                .Select(x => new RelationOptionViewModel()
                {
                    Id = x.Id,
                    Title = string.IsNullOrEmpty(x.TargetMessage) ? x.Name : x.TargetMessage,
                    IsSource = false
                });
        }

        public IEnumerable<RelationOptionViewModel> GetRelationOptionTypes(int artistId)
        {
            return GetArtistRelationTypesWhereSource(artistId).Concat(GetArtistRelationTypesWhereTarget(artistId)).OrderBy(x => x.Id);
        }

        public RelationChoiceViewModel GetRelationChoiceViewModel(int artistId)
        {
            var options = GetRelationOptionTypes(artistId).ToArray();
            var result = new RelationChoiceViewModel()
            {
                RelationKeys = new Dictionary<int, RelationKeyViewModel>(options.Length),
                RelationTitles = new Dictionary<int, string>(options.Length)
            };
            for (int relationId = 0; relationId < options.Length; relationId++)
            {
                result.RelationKeys.Add(relationId, options[relationId]);
                result.RelationTitles.Add(relationId, options[relationId].Title);
            }
            return result;
        }

        public IEnumerable<MemberViewModel> GetRelationMembers(int artistId, int relationId, RelationKeyViewModel model)
        {
            var artist = GetArtistDomain(artistId);
            return
                model.IsSource
                    ? artist.RelationsWhereSource.Where(x => x.TypeId == model.Id).Select(x => new MemberViewModel()
                    {
                        Id = x.TargetId,
                        Name = x.Target.Name,
                        Interval = mapper.Map<TimeInterval, TimeIntervalViewModel>(x.Interval),
                        PictureId = x.Target.PictureId,
                        ArtistId = artistId,
                        RelationId = relationId
                    })
                    : artist.RelationsWhereTarget.Where(x => x.TypeId == model.Id).Select(x => new MemberViewModel()
                    {
                        Id = x.SourceId,
                        Name = x.Source.Name,
                        Interval = mapper.Map<TimeInterval, TimeIntervalViewModel>(x.Interval),
                        PictureId = x.Source.PictureId,
                        ArtistId = artistId,
                        RelationId = relationId
                    });
        }

        public IEnumerable<ArtistBaseViewModel> GetRelationCandidates(int artistId, RelationKeyViewModel model, string query)
        {
            var candidates = GetMany().ToArray()
                .Except(
                    model.IsSource
                    ? GetOneIf(x => x.Id == artistId).RelationsWhereSource.Where(x => x.TypeId == model.Id).Select(x => x.Target)
                    : GetOneIf(x => x.Id == artistId).RelationsWhereTarget.Where(x => x.TypeId == model.Id).Select(x => x.Source)
                )
                .Where(x => x.Id != artistId && x.Name.StartsWith(query)).ToArray();
            return mapper.Map<IEnumerable<Artist>, IEnumerable<ArtistBaseViewModel>>(candidates);
        }
    }
}
