var builder = DistributedApplication.CreateBuilder(args);

builder.AddProject<Projects.ApiDocDemo>("apidocdemo");

builder.Build().Run();
