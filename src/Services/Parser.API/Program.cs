using Parser.API;
using Parser.API.Parsers;

IHost host = Host.CreateDefaultBuilder(args)
    .ConfigureServices((context, services) =>
    {
        services.AddHostedService<Worker>();

        //Implementation to be used for parsing
        string _parserClass = context.Configuration.GetValue<string>("ParserSettings:ParserSignature");

        Type? _parserClassType = Type.GetType(_parserClass, true);

        if (_parserClassType != null)
            services.AddSingleton<IParser>(service =>
                (IParser)ActivatorUtilities.CreateInstance(service, _parserClassType));
    })
    .Build();

await host.RunAsync();
