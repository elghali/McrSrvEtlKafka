using Parser.API.Parsers;

namespace Parser.API
{
    internal class Worker : BackgroundService
    {
        private readonly ILogger<Worker> _logger;
        private readonly IParser _parser;

        public Worker(ILogger<Worker> logger, IParser parser)
        {
            _logger = logger;
            _parser = parser;
        }

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            while (!stoppingToken.IsCancellationRequested)
            {
                _logger.LogInformation("Worker running at: {time}", DateTimeOffset.Now);
                _parser.ParserData(stoppingToken);
                await Task.Delay(1000, stoppingToken);
            }
        }
    }
}