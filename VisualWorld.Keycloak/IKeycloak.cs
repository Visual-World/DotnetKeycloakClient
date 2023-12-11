﻿namespace VisualWorld.Keycloak;

public interface IKeycloak
{    Task SendVerifyEmailAsync(
        string userId, 
        string? clientId = null,
        string? redirectUri = null,
        CancellationToken cancellationToken = default
    );

    Task<KeycloakUser?> GetUserByUsernameAsync(
        string username,
        CancellationToken cancellationToken = default);
    
    Task<KeycloakUser> GetUserByIdAsync(string userId,
        CancellationToken cancellationToken = default);

    Task CreateUserAsync(
        CreateKeycloakUserRequest createKeycloakUserRequest,
        CancellationToken cancellationToken = default);

    Task<IReadOnlySet<(string Username, string UserId)>> GetUsersMapAsync(
        bool? enabled,
        CancellationToken cancellationToken = default);

    Task DeleteUserAsync(string userId, CancellationToken cancellationToken = default);
}