var builder = DistributedApplication.CreateBuilder(args);

builder.AddProject<Projects.MeDesenvolveIA_API>("medesenvolveia-api");

builder.Build().Run();
