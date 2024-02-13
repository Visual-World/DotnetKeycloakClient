namespace VisualWorld.Keycloak;

public sealed record KeycloakUserWithFederatedIdentities : KeycloakUser
{
    public IReadOnlyList<KeycloakFederatedIdentity> FederatedIdentities { get; init; } =
        Array.Empty<KeycloakFederatedIdentity>();
}