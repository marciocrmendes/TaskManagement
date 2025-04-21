using TaskManagement.API.Extensions;

var builder = WebApplication.CreateBuilder(args);

builder.AddApiDependencies();

var app = builder.Build();

app.UseApiDependencies();

await app.RunAsync();
