namespace UrlShortener.Api.Exceptions;

public sealed class ShortCodeNotFoundException(string shortCode) : Exception($"Short URL with code '{shortCode}' was not found.");

public sealed class ShortUrlNotFoundException(Guid id) : Exception($"Short URL with id '{id}' was not found.");

public sealed class ForbiddenOperationException() : Exception("You are not allowed to perform this operation.");

public sealed class DuplicateUrlException(string url) : Exception($"A short URL for '{url}' already exists.");