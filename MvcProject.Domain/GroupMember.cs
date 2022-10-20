using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MvcProject.Domain
{
    public class GroupMember
    {
        [Key, Column(Order = 1)]
        public int GroupId { get; set; }
        public virtual Group Group { get; set; }

        [Key, Column(Order = 2)]
        public int PersonId { get; set; }
        public Person Person { get; set; }

        public TimeInterval Interval { get; set; }

    }
}
