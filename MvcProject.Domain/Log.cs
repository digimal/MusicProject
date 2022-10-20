using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace MvcProject.Domain
{
    public class Log
    {
        [Key]
        public int Id { get; set; }

        public DateTime TimeStamp { get; set; }

        [StringLength(128)]
        public string Level { get; set; }

        public string Message { get; set; }      

        public int UserId { get; set; }
        public virtual User User {get; set;}

        [NotMapped]
        public string TemplateMessage => "{Message}{UserId}{TimeStamp}{Level}";

        [NotMapped]
        public object[] TemplateOutput => new object[]  { Message, UserId, TimeStamp, Level };
    }
}
