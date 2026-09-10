using System.Security.Cryptography;
using UrlShortener.Api.Services.Interfaces;

namespace UrlShortener.Api.Services;

public sealed class ShortCodeGenerator : IShortCodeGenerator
{
    private const string Characters =
        "0123456789abcdefghijklmnopqrstuvwxyzABCDEFGHIJKLMNOPQRSTUVWXYZ";

    private const int CodeLength = 7;

    public string Generate()
    {
        Span<char> result = stackalloc char[CodeLength];

        for (var i = 0; i < CodeLength; i++)
        {
            var index = RandomNumberGenerator.GetInt32(Characters.Length);
            result[i] = Characters[index];
        }

        return new string(result);
    }
}