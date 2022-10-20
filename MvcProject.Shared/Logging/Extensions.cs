using System.Collections.Generic;
using System.Linq;

namespace MvcProject.Shared.Logging
{

    public static class Extensions
    {
        private class AgregatedLogger : ILocalLogger
        {
            ILocalLogger[] _loggers;

            public AgregatedLogger(params ILocalLogger[] loggers)
            {
                _loggers = loggers;
            }

            public void Write(Log log)
            {
                foreach (var logger in _loggers)
                {
                    logger.Write(log);
                }
            }
        }

        public static ILocalLogger Unite(params ILocalLogger[] loggers)
        {
            return new AgregatedLogger(loggers);
        }

        public static ILocalLogger Unite(this IEnumerable<ILocalLogger> loggers)
        {
            return new AgregatedLogger(loggers.ToArray());
        }

    }
}
