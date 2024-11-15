using System.Configuration;
using System.Data;
using System.Windows;
using log4net;
using log4net.Appender;
using log4net.Layout;
using log4net.Repository.Hierarchy;
namespace Calculator
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App : Application
    {
        private static readonly ILog log = LogManager.GetLogger(typeof(App));

        static App()
        {
            ConfigureLogging();
        }

        private static void ConfigureLogging()
        {
            var layout = new PatternLayout
            {
                ConversionPattern = "%date [%thread] %-5level %logger - %message%newline"
            };
            layout.ActivateOptions();



#if DEBUG
            var debugAppender = new TraceAppender { Layout = layout };
            debugAppender.ActivateOptions();
            log4net.Config.BasicConfigurator.Configure(debugAppender);

           //  logging messages
            log.Debug("Debug log message from App.xaml.cs");
            log.Info("Info log message from App.xaml.cs");

        
            log.Warn("Warning log message");
        
            log.Fatal("Fatal log message");
#else
            var fileAppender = new RollingFileAppender
        {
            Layout = layout,
            File = "log.txt",
            AppendToFile = true,
            Encoding = System.Text.Encoding.UTF8,
            StaticLogFileName = true
        };
        fileAppender.ActivateOptions();
        log4net.Config.BasicConfigurator.Configure(fileAppender);

            //  logging messages
            log.Debug("Debug log message from App.xaml.cs");
            log.Info("Info log message from App.xaml.cs");

      
            log.Warn("Warning log message");
         
            log.Fatal("Fatal log message");
#endif

         
        }

    }
}