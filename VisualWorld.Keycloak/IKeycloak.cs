﻿namespace VisualWorld.Keycloak;

public interface IKeycloak
{
    Task SendVerifyEmailAsync(
        string userId,
        string? clientId = null,
        string? redirectUri = null,
        CancellationToken cancellationToken = default
    );

    Task<KeycloakUser?> GetUserByUsernameAsync(
        string username,
        CancellationToken cancellationToken = default);

    Task<KeycloakUserWithFederatedIdentities> GetUserByIdAsync(string userId,
        CancellationToken cancellationToken = default);

    Task CreateUserAsync(
        CreateKeycloakUserRequest createKeycloakUserRequest,
        CancellationToken cancellationToken = default);

    Task<IReadOnlySet<(string Username, string UserId)>> GetUsersMapAsync(
        bool? enabled,
        CancellationToken cancellationToken = default);

    Task DeleteUserAsync(string userId, CancellationToken cancellationToken = default);

    Task SetPasswordAsync(string userId, string password, CancellationToken cancellationToken = default);
    Task<IReadOnlyList<KeycloakUser>> GetUsersAsync(bool? enabled, CancellationToken cancellationToken = default);
}