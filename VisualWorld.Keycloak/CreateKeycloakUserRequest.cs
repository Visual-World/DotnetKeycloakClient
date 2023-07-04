namespace VisualWorld.Keycloak;

public sealed record CreateKeycloakUserRequest(string UsernameAndEmail)
{
    public IReadOnlyList<string> RequiredActions { get; init; } = Array.Empty<string>();
    
    public IReadOnlyList<string> Roles { get; init; } = Array.Empty<string>();
    
    public bool Enabled { get; init; }
}