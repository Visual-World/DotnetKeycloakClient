using Microsoft.Extensions.Options;

namespace VisualWorld.Keycloak;

public class Keycloak : IKeycloak
{
    private readonly IKeycloakClient keycloakClient;
    
    private readonly IOptionsSnapshot<KeycloakOptions> keycloakOptions;

    public Keycloak(
        IKeycloakClient keycloakClient,
        IOptionsSnapshot<KeycloakOptions> keycloakOptions)
    {
        this.keycloakClient = keycloakClient ?? throw new ArgumentNullException(nameof(keycloakClient));
        this.keycloakOptions = keycloakOptions ?? throw new ArgumentNullException(nameof(keycloakOptions));
    }
    
    public async Task SendVerifyEmailAsync(
        string userId, 
        string? clientId = null,
        string? redirectUri = null,
        CancellationToken cancellationToken = default
    )
    {
        if (string.IsNullOrWhiteSpace(userId))
        {
            throw new ArgumentException("Value cannot be null or whitespace.", nameof(userId));
        }

        await keycloakClient.SendVerifyEmailAsync(
            keycloakOptions.Value.Realm,
            userId,
            clientId,
            redirectUri,
            cancellationToken: cancellationToken);
    }

    public async Task<KeycloakUser?> GetUserByUsernameAsync(
        string username,
        CancellationToken cancellationToken = default)
    {
        if (string.IsNullOrWhiteSpace(username))
        {
            throw new ArgumentException("Value cannot be null or whitespace.", nameof(username));
        }
        
        var users = await keycloakClient.UsersAll3Async(
            keycloakOptions.Value.Realm,
            enabled: true,
            max: 1,
            username: username,
            cancellationToken: cancellationToken);

        return users.SingleOrDefault()?.ToKeycloakUser();
    }

    public async Task<KeycloakUser> GetUserByIdAsync(string userId,
        CancellationToken cancellationToken = default)
    {
        if (string.IsNullOrWhiteSpace(userId))
        {
            throw new ArgumentException("Value cannot be null or whitespace.", nameof(userId));
        }
        
        var user = await keycloakClient.UsersGET2Async(
            keycloakOptions.Value.Realm,
            userId,
            cancellationToken);

        return user.ToKeycloakUser();
    }

    public async Task CreateUserAsync(
        CreateKeycloakUserRequest createKeycloakUserRequest,
        CancellationToken cancellationToken = default)
    {
        if (createKeycloakUserRequest == null)
        {
            throw new ArgumentNullException(nameof(createKeycloakUserRequest));
        }
        
        await keycloakClient.UsersPOSTAsync(
            new UserRepresentation
            {
                Username = createKeycloakUserRequest.Username, 
                Email = createKeycloakUserRequest.Email, 
                Enabled = createKeycloakUserRequest.Enabled,
                RequiredActions = createKeycloakUserRequest.RequiredActions,
            },
            keycloakOptions.Value.Realm,
            cancellationToken);
    }

    public async Task<IReadOnlySet<(string Username, string UserId)>> GetUsersMapAsync(
        bool? enabled,
        CancellationToken cancellationToken = default)
    {
        var users = await keycloakClient.UsersAll3Async(
            keycloakOptions.Value.Realm,
            briefRepresentation: true,
            enabled: enabled,
            cancellationToken: cancellationToken);

        return users
            .Where(u => !string.IsNullOrWhiteSpace(u.Username))
            .Select(u => (u.Username!, u.Id!)).ToHashSet();
    }

    public async Task DeleteUserAsync(string userId, CancellationToken cancellationToken = default)
    {
        if (string.IsNullOrWhiteSpace(userId))
        {
            throw new ArgumentException("Value cannot be null or whitespace.", nameof(userId));
        }
        
        await keycloakClient.UsersDELETE3Async(keycloakOptions.Value.Realm, userId, cancellationToken);
    }
}