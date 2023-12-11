namespace VisualWorld.Keycloak;

public sealed record CreateKeycloakUserRequest(string Username, string? Email = null)
{
    public IReadOnlyList<string> RequiredActions { get; init; } = Array.Empty<string>();
    
    public bool Enabled { get; init; }
}