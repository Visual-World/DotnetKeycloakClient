namespace VisualWorld.Keycloak;

public static class Mapper
{
    public static KeycloakUser ToKeycloakUser(this UserRepresentation userRepresentation)
    {
        if (userRepresentation?.Id == null)
        {
            throw new ArgumentNullException(nameof(userRepresentation));
        }

        var keycloakUser = new KeycloakUser
        {
            Id = userRepresentation.Id,
            Email = userRepresentation.Email,
            Enabled = userRepresentation.Enabled ?? false,
            Username = userRepresentation.Username,
            EmailVerified = userRepresentation.EmailVerified ?? false,
            FirstName = userRepresentation.FirstName,
            LastName = userRepresentation.LastName,
            RequiredActions = userRepresentation.RequiredActions ?? Array.Empty<string>()
        };

        return keycloakUser;
    }
    public static KeycloakUserWithFederatedIdentities ToKeycloakUserWithFederatedIdentities(this UserRepresentation userRepresentation)
    {
        if (userRepresentation?.Id == null)
        {
            throw new ArgumentNullException(nameof(userRepresentation));
        }

        var keycloakUser = new KeycloakUserWithFederatedIdentities
        {
            Id = userRepresentation.Id,
            Email = userRepresentation.Email,
            Enabled = userRepresentation.Enabled ?? false,
            Username = userRepresentation.Username,
            EmailVerified = userRepresentation.EmailVerified ?? false,
            FirstName = userRepresentation.FirstName,
            LastName = userRepresentation.LastName,
            FederatedIdentities = userRepresentation.FederatedIdentities is not null
                ? userRepresentation.FederatedIdentities
                    .Select(f => f.ToKeycloakFederatedIdentity())
                    .ToArray()
                : Array.Empty<KeycloakFederatedIdentity>(),
            RequiredActions = userRepresentation.RequiredActions ?? Array.Empty<string>()
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

        return new KeycloakFederatedIdentity
        {
            IdentityProvider = federatedIdentityRepresentation.IdentityProvider ?? string.Empty,
            Username = federatedIdentityRepresentation.UserName,
            UserId = federatedIdentityRepresentation.UserId
        };
    }
}