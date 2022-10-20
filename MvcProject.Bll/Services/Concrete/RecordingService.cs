using System.Collections.Generic;
using System.Linq;
using MvcProject.Domain;
using MvcProject.Bll.Services.Abstract;
using MvcProject.Bll.ViewModels.Recording;
using MvcProject.Bll.ViewModels.Common;
using AutoMapper;
using MvcProject.Dal;

namespace MvcProject.Bll.Services.Concrete
{
    public class RecordingService : BaseService<Recording>,  IRecordingService
    {
        public RecordingService(MusicContext context, IMapper mapper) : base(context, mapper) { }

        public LikesViewModel GetLikesViewModel(int recordingId, int userId)
        {
            var recording = GetOneIf(x => x.Id == recordingId);
            return new LikesViewModel()
            {
                LikesCount = recording?.Fans.Count ?? default(int),
                IsLiked = recording?.Fans.Any(x => x.UserId == userId) ?? false
            };
        }

        public RecordingViewModel Create(RecordingViewModel model)
        {
            return mapper.Map<Recording, RecordingViewModel>(Create(mapper.Map<RecordingViewModel, Recording>(model)));
        }

        public void Update(RecordingViewModel model)
        {
            Update(mapper.Map<RecordingViewModel, Recording>(model));
        }

        public void Delete(RecordingViewModel model)
        {
            Delete(mapper.Map<RecordingViewModel, Recording>(model));
        }

        public IEnumerable<RecordingBaseViewModel> GetRecordings(int skip, int take)
        {
            return mapper.Map<IEnumerable<Recording>, IEnumerable<RecordingBaseViewModel>>(GetMany().Skip(skip).Take(take));

        }
    }
}
