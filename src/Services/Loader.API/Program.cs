using Loader.API;

IHost host = Host.CreateDefaultBuilder(args)
    .ConfigureServices(services =>
    {
        //services.AddHostedService<Worker>();
        services.AddHostedService<CsvLoader>();
    })
    .Build();

await host.RunAsync();
