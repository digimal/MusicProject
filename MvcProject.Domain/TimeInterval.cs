using System;
using System.ComponentModel.DataAnnotations.Schema;

namespace MvcProject.Domain
{
    [ComplexType]
    public class TimeInterval
    {
        public DateTime? StartDate { get; set; }

        public DateTime? EndDate { get; set; }
    }
}
