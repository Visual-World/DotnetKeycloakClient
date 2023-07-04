namespace VisualWorld.Keycloak;

public static class Mapper
{
    public static KeycloakUser ToKeycloakUser(this UserRepresentation userRepresentation)
    {
        if (userRepresentation == null)
        {
            throw new ArgumentNullException(nameof(userRepresentation));
        }

        var keycloakUser = new KeycloakUser(userRepresentation.Id)
        {
            Email = userRepresentation.Email,
            Enabled = userRepresentation.Enabled ?? false,
            Username = userRepresentation.Username,
            EmailVerified = userRepresentation.EmailVerified ?? false,
            FirstName = userRepresentation.FirstName,
            LastName = userRepresentation.LastName,
            FederatedIdentities = userRepresentation.FederatedIdentities is not null
                ? userRepresentation.FederatedIdentities
                    .Select(f => f.ToKeycloakFederatedIdentity())
                    .ToList()
                : Array.Empty<KeycloakFederatedIdentity>(),
        };
        
        return keycloakUser;
    }

    public static KeycloakFederatedIdentity ToKeycloakFederatedIdentity(
        this FederatedIdentityRepresentation federatedIdentityRepresentation)
    {
        if (federatedIdentityRepresentation == null)
        {
            throw new ArgumentNullException(nameof(federatedIdentityRepresentation));
        }

        return new KeycloakFederatedIdentity(federatedIdentityRepresentation.IdentityProvider)
        {
            Username = federatedIdentityRepresentation.UserName,
            UserId = federatedIdentityRepresentation.UserId
        };
    }
}