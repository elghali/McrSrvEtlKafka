using System.Text.Json;
using Confluent.Kafka;
using Parser.API;

namespace Loader.API
{
    public class CsvLoader : BackgroundService
    {
        private readonly IConfiguration _configuration;
        private readonly ILogger<CsvLoader> _logger;
        private readonly string _consumerTopicName;

        public CsvLoader(IConfiguration configuration, ILogger<CsvLoader> logger)
        {
            _configuration = configuration;
            _logger = logger;
            _consumerTopicName = configuration.GetValue<string>("Kafka:ConsumerSettings:Topic"); ;
        }

        public void LoadFiles(string message)
        {
            //Generate event
            ParserFile parserFile = new ParserFile();
            if(message != null)
                parserFile = JsonSerializer.Deserialize<ParserFile>(message)!;

            var incomingPath = parserFile.IncomingPath;

            if (!Directory.Exists(incomingPath))
            {
                _logger.LogError("Source Directory does not exist");
                throw new IOException("Directory does not exist!");
            }
            _logger.LogInformation("Loading Data for files: " + parserFile.OutputFileName);
        }
        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            ConsumeResult<Null, string> consumerResult = new ConsumeResult<Null, string>();
            _logger.LogInformation("Loader Worker running at: {time}", DateTimeOffset.Now);
            using (var consumer = CreateConsumer())
            {
                while (!stoppingToken.IsCancellationRequested)
                {
                    while (!stoppingToken.IsCancellationRequested)
                    {
                        consumerResult = ConsumeMessage(consumer, stoppingToken);
                        _logger.LogDebug("Loader Receiving message at: {time}", DateTimeOffset.UtcNow);
                        //var data = consumerResult.Message.Value; Commented out since we only want to measure the delay between producing and consuming the message
                        //LoadFiles(data);
                    }
                    await Task.Delay(1000, stoppingToken);
                }
            }
        }
        virtual public ConsumeResult<Null, string> ConsumeMessage(IConsumer<Null, string> consumer, CancellationToken cts)
        {
            try
            {
                var consumeResult = consumer.Consume(cts);
                return consumeResult;
            }
            catch
            {
                throw;
            }
        }
        virtual public ConsumerBuilder<Null, string> GetConsumerBuilder()
        {
            if (_configuration is null || _logger is null)
                throw new ArgumentNullException(nameof(_configuration));

            var conf = new ConsumerConfig();
            _configuration.GetSection("Kafka:ConsumerSettings").Bind(conf);

            return new ConsumerBuilder<Null, string>(conf)
                .SetErrorHandler((_, e) => _logger.LogError($"Error: {e.Reason}"));
        }
        virtual public IConsumer<Null, string> CreateConsumer()
        {
            if (_logger is null)
                throw new ArgumentNullException(nameof(_logger));
            try
            {
                IConsumer<Null, string> consumer =
                    GetConsumerBuilder().Build();
                //Subscribe once to Topic
                consumer.Subscribe(_consumerTopicName);
                return consumer;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message);
                throw;
            }
        }
    }
}
