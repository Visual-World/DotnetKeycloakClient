namespace VisualWorld.Keycloak;

public sealed record KeycloakFederatedIdentity
{
    public string IdentityProvider { get; init; } = string.Empty;
    
    public string? UserId { get; init; }

    public string? Username { get; init; }
}