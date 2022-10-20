using AutoMapper;
using MvcProject.Bll.ViewModels.Artist;
using MvcProject.Bll.ViewModels.Authorship;
using MvcProject.Bll.ViewModels.Common;
using MvcProject.Bll.ViewModels.Recording;
using MvcProject.Bll.ViewModels.User;
using MvcProject.Domain;

namespace MvcProject.Bll
{
    public class MapperProfile : Profile
    {
        public MapperProfile()
        {
            // Artist
            CreateMap<ArtistBaseViewModel, Artist>().ReverseMap();
            CreateMap<ArtistViewModel, Artist>().ReverseMap();
            CreateMap<Artist, ArtistShowViewModel>().ForMember(x => x.PicturePath, opt => opt.MapFrom(y => y.Picture.Path));
            CreateMap<TagViewModel, ArtistTag>().ReverseMap();
            CreateMap<ArtistViewModel, ArtistCreateViewModel>();
            // Authorship
            CreateMap<AuthorArtist, AuthorArtistDisplayViewModel>().ReverseMap();
            CreateMap<AuthorArtist, AuthorArtistEditViewModel>().ReverseMap();
            // Recording
            CreateMap<Recording, RecordingBaseViewModel>().ReverseMap();
            CreateMap<Recording, RecordingViewModel>().ReverseMap();
            // User
            CreateMap<UserRegistrationViewModel, User>()
                    .ForMember(vm => vm.UserName, opt => opt.MapFrom(t => t.UserName))
                    .ForSourceMember(vm => vm.Password, opt => opt.DoNotValidate())
                    .ForSourceMember(vm => vm.ConfirmPassword, opt => opt.DoNotValidate());
            CreateMap<User, ProfileViewModel>();
            // Common
            CreateMap<PictureViewModel, Picture>().ReverseMap();
            // Common : TimeInterval (Issue: Is TimeIntervalViewModel obsolete?)
            CreateMap<TimeIntervalViewModel, TimeInterval>().ReverseMap();
        }
    }
}
