using VotePreferentiel;
using VotePreferentiel.Worker;
using Microsoft.Extensions.DependencyInjection;

var builder = Host.CreateApplicationBuilder(args);

builder.Services.AddHttpClient<VoteProvider>();

builder.Services.AddHostedService<Worker>();

var host = builder.Build();
host.Run();