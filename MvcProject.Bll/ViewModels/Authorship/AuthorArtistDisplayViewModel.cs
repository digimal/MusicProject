namespace MvcProject.Bll.ViewModels.Authorship
{
    public class AuthorArtistDisplayViewModel
    {
        public Artist.ArtistBaseViewModel Artist { get; set; }
        public int Position { get; set; }
        public string DisplayName { get; set; }
        public string JoinPhrase { set; get; }
    }

    public class AuthorArtistEditViewModel : AuthorArtistDisplayViewModel
    {
        public int Id { get; set; }
    }

    public class ArtistAuthorDisplayViewModel
    {
        public Artist.ArtistBaseViewModel Artist { get; set; }
        public int Position { get; set; }
        public string DisplayName { get; set; }
        public string JoinPhrase { set; get; }
    }

    public class ArtistAuthorEditViewModel : ArtistAuthorDisplayViewModel
    {
        public Artist.ArtistBaseViewModel Author { get; set; }
    }

}
