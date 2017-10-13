using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using log4net;
using log4net.Config;

namespace SimpleGame
{
    static class SimpleGameLoggerManager
    {
        public static ILog SetupLogger()
        {
            ILog logger = LogManager.GetLogger("SimpleGameFileAppender");
            var now = DateTime.Now;
            GlobalContext.Properties["LogFileName"] = @"C:\ProjectLogs\SimpleGameLog_" + now.Day + "-" + now.Month + "-" + now.Year; //log file path

            XmlConfigurator.Configure();

            logger.Debug("#########################################################################");
            logger.Debug("#########################################################################");
            logger.Debug("SimpleGame logging session started on " + DateTime.Now);
            logger.Debug("#########################################################################");
            logger.Debug("#########################################################################");

            return logger;

        }
    }
}
