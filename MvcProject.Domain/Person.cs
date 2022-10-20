using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MvcProject.Domain
{
    [Table("Persons")]
    public class Person : Artist
    {
        public virtual ICollection<GroupMember> Groups { get; set; }
    }
}
