using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MvcProject.Bll.ViewModels.Artist
{
    public class RelationKeyViewModel
    {
        public int Id { get; set; }
        public bool IsSource { get; set; }
    }

    public class RelationOptionViewModel : RelationKeyViewModel
    {
        public string Title { get; set; }
    }

    public class RelationChoiceViewModel
    {
        public Dictionary<int, RelationKeyViewModel> RelationKeys { get; set; }

        public Dictionary<int, string> RelationTitles { get; set; }
    }
}
