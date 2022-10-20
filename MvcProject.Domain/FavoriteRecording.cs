using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace MvcProject.Domain
{
    public class FavoriteRecording
    {
        public int UserId { get; set; }
        public virtual User User { get; set; }

        public int RecordingId { get; set; }
        public virtual Recording Recording { get; set; }
    }
}
