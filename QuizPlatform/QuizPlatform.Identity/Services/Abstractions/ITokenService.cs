using QuizPlatform.Identity.Models;

namespace QuizPlatform.Identity.Services.Abstractions;

public interface ITokenService
{
    string GenerateTokenFor(ApplicationUser user);
}