using log4net;
using log4net.Config;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Pacman
{
    static class PacmanLoggerManager
    {
        public static ILog SetupLogger(ActualPacmanGameInstance instance)
        {
            ILog logger = LogManager.GetLogger("PacmanFileAppender");
            GlobalContext.Properties["LogFileName"] = @"C:\ProjectLogs\PacmanLogs\PacmanLog"; //log file path

            XmlConfigurator.Configure();

            logger.Debug("New logger created for Pacman " + (ActualPacmanGameInstance.IS_HEADLESS ? "HEADLESS" : "DEMONSTRATION") + " instance");
            logger.Debug("Timercounter is set to " + ActualPacmanGameInstance.maxtimerMax);


            return logger;
        }
    }
}
