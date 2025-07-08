using AM.API.Setup;

WebApplication
    .CreateBuilder(args)
    .ConfigureServices()
    .Build()
    .PiplineSetup()
    .Run();