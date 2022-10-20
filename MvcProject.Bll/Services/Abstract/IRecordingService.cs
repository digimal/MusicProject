using MvcProject.Bll.ViewModels.Common;

namespace MvcProject.Bll.Services.Abstract
{
    public interface IRecordingService
    {
        LikesViewModel GetLikesViewModel(int recordingId, int userId);
    }
}
