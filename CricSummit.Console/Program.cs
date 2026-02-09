using CricSummit.Domain.Rules;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Serilog;

var services = new ServiceCollection();

services.AddLogging(builder =>
{
    builder.ClearProviders();
    builder.AddSerilog();
});

services.AddSingleton<CombinationRuleProvider>();

var serviceProvider = services.BuildServiceProvider();

var ruleProvider = serviceProvider.GetRequiredService<CombinationRuleProvider>();
