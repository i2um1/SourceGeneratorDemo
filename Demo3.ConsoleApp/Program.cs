using Demo3.ConsoleApp;

using Microsoft.Extensions.Logging;

using Serilog;

using var loggerFactory = LoggerFactory.Create(builder =>
{
    var logger = new LoggerConfiguration()
        .WriteTo.Console()
        .CreateLogger();

    builder.AddSerilog(logger, dispose: true);
});

var logger = loggerFactory.CreateLogger<Program>();

logger.LogMessage1(1);
logger.LogMessage2(2);