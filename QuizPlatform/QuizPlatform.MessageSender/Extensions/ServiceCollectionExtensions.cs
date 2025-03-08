using QuizPlatform.MessageSender.Services;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Configuration;
using QuizPlatform.MessageSender.Abstractions.Services;
using QuizPlatform.MessageSender.Abstractions.Services.Models;

namespace QuizPlatform.MessageSender.Extensions;

public static class ServiceCollectionExtensions
{
    public static IServiceCollection AddMailSender(this IServiceCollection services, IConfiguration configuration)
    {
        services.AddScoped<IEmailSender, EmailSender>();
        services.Configure<EmailSenderOptions>(configuration.GetSection(EmailSenderOptions.Key));

        return services;
    }
}