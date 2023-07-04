namespace VisualWorld.Keycloak;

public sealed record KeycloakUser(string Id)
{
    public string? Username { get; init; }

    public bool Enabled { get; init; }

    public string? Email { get; init; } 
    
    public bool EmailVerified { get; init; }
    
    public string? FirstName { get; init; }
    
    public string? LastName { get; init; }

    public IReadOnlyList<KeycloakFederatedIdentity> FederatedIdentities { get; init; } = Array.Empty<KeycloakFederatedIdentity>();
    
    public IReadOnlyList<string> RequiredActions { get; init; } = Array.Empty<string>();
}