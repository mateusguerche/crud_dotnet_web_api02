namespace WebAPI_Projeto02.Logging
{
    public class CustomerLogger : ILogger
    {
        readonly string loggerName;
        readonly CustomLoggerProviderConfigration loggerConfig;

        public CustomerLogger(string name, CustomLoggerProviderConfigration config)
        {
            loggerName = name;
            loggerConfig = config;
        }
        
        public IDisposable BeginScope<TState>(TState state)
        {
            return null;
        }

        public bool IsEnabled(LogLevel logLevel)
        {
            return logLevel == loggerConfig.LogLevel;
        }

        public void Log<TState>(LogLevel logLevel, EventId eventId, TState state, Exception exception, Func<TState, Exception, string> formatter)
        {
            string message = $"{logLevel.ToString()}: {eventId.Id} - {formatter(state, exception)}";
            WriteToFile(message);
        }

        private void WriteToFile(string message) 
        {
            string logFilePath = @"c:\Projetos\WebAPI_Projeto02\logs\projeto02.txt";

            using (StreamWriter streamWriter = new StreamWriter(logFilePath, true))
            {
                try
                {
                    streamWriter.WriteLine(message);
                    streamWriter.Close();
                }
                catch (Exception ex)
                {
                    throw;
                }
            }
        }
    }
}
