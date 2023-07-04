﻿namespace VisualWorld.Keycloak;

public interface IKeycloak
{
    Task SendVerifyEmailAsync(
        string userId,
        CancellationToken cancellationToken = default);

    Task<KeycloakUser?> GetUserAsync(
        string usernameAndEmail,
        CancellationToken cancellationToken = default);
    
    Task<KeycloakUser> GetUserByIdAsync(string userId,
        CancellationToken cancellationToken = default);

    Task CreateUserAsync(
        CreateKeycloakUserRequest createKeycloakUserRequest,
        CancellationToken cancellationToken = default);

    Task<IReadOnlySet<(string Email, string UserId)>> GetEmailAddressesMapAsync(
        bool? enabled,
        CancellationToken cancellationToken = default);

    Task DeleteUserAsync(string userId, CancellationToken cancellationToken = default);
}