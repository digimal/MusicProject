using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace MvcProject.Domain
{
    public class ArtistRelation
    {
        public int SourceId { get; set; }
        public virtual Artist Source { get; set; }

        public int TargetId { get; set; }
        public virtual Artist Target { get; set; }

        public int TypeId { get; set; }
        public virtual ArtistRelationType Type { get; set; }

        public TimeInterval Interval { get; set; }
    }
}
