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
            GlobalContext.Properties["LogFileName"] = @"C:\ProjectLogs\SimpleGameLog"; //log file path

            XmlConfigurator.Configure();


            return logger;

        }
    }
}
