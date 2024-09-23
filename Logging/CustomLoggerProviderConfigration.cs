namespace WebAPI_Projeto02.Logging
{
    public class CustomLoggerProviderConfigration
    {
        public LogLevel LogLevel { get; set; } = LogLevel.Warning;
        public int EventId { get; set; } = 0;
    }
}
