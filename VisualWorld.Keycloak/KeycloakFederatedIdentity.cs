namespace VisualWorld.Keycloak;

public sealed record KeycloakFederatedIdentity(string IdentityProvider)
{
    public string? UserId { get; init; }

    public string? Username { get; init; }
}