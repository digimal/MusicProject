using MvcProject.Bll.ViewModels.Artist;
using System.Collections.Generic;

namespace MvcProject.Bll.Services.Abstract
{
    public interface IArtistRelationTypeService
    {
        RelationChoiceViewModel GetAll();
    }
}
