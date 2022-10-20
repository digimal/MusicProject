using System;

namespace MvcProject.Shared.Logging
{
    public class Log
    {
        public DateTime TimeStamp { get; set; }


        public Serilog.Events.LogEventLevel Level { get; set; }

        public string Message { get; set; }

        public int UserId { get; set; }

        public string TemplateMessage => "{Message}{UserId}{TimeStamp}{Level}";

        public object[] TemplateOutput => new object[] { Message, UserId, TimeStamp, Level };
    }
}
