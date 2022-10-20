using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MvcProject.Domain
{
    [Table("Groups")]
    public class Group : Artist
    {
        public virtual ICollection<GroupMember> Members { get; set; }
    }
}
