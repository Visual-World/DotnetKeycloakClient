namespace VisualWorld.Keycloak;

public record KeycloakUser
{
    public string Id { get; init; } = string.Empty;
    public string? Username { get; init; }

    public bool Enabled { get; init; }

    public string? Email { get; init; }

    public bool EmailVerified { get; init; }

    public string? FirstName { get; init; }

    public string? LastName { get; init; }

    public IReadOnlyList<string> RequiredActions { get; init; } = Array.Empty<string>();
}