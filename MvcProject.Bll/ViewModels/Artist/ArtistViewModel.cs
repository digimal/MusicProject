﻿using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MvcProject.Bll.ViewModels.Artist
{
    public class ArtistViewModel : ArtistBaseViewModel
    {
        [Required(AllowEmptyStrings = false, ErrorMessage = "Name field is obligatory.")]
        public string Aliases { get; set; }

        [Required(AllowEmptyStrings = false, ErrorMessage = "Name field is obligatory.")]
        public string Description { get; set; }

        public Common.TimeIntervalViewModel Interval { get; set; }
    }

    public class ArtistCreateViewModel : ArtistViewModel
    {
        public string TempPicId { get; set; }
    }

    public class ArtistShowViewModel : ArtistViewModel
    {
        public string PicturePath { get; set; }
    }
}
