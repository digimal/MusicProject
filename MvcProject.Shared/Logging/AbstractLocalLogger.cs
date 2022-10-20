using Serilog.Core;

namespace MvcProject.Shared.Logging
{
    public abstract class AbstractLocalLogger : ILocalLogger
    {
        protected Logger _log;

        public void Write(Log log)
        {
            _log.Write(log.Level, log.TemplateMessage, log.TemplateOutput);
        }

        
    }
}
