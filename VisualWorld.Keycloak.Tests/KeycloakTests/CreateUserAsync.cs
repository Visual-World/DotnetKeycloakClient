using System.Diagnostics;
using AutoFixture;
using FluentAssertions;
using Moq;
using Xunit;

namespace VisualWorld.Keycloak.Tests.KeycloakTests;

public sealed class CreateUserAsync : Infrastructure
{
    private CreateKeycloakUserRequest createKeycloakUserRequest;

    public CreateUserAsync()
    {
        createKeycloakUserRequest = Fixture.Create<CreateKeycloakUserRequest>();
    }

    [Fact]
    public async Task ThrowsArgumentNullIfCreateKeycloakUserRequestIsNull()
    {
        // Arrange
        createKeycloakUserRequest = null;
        
        // Act, Assert
        var exception = await Assert.ThrowsAsync<ArgumentNullException>(() => CallAsync());
        exception.ParamName.Should().Be("createKeycloakUserRequest");
        
        KeycloakClientMock.Invocations.Should().BeEmpty();
    }

    [Fact]
    public async Task CallsTheKeycloakClient()
    {
        // Act
        await CallAsync();
        
        // Assert
        KeycloakClientMock.Verify(m
                => m.UsersPOSTAsync(
                    It.Is<UserRepresentation>(ur
                        => ur.Username == createKeycloakUserRequest.Username
                           && ur.Email == createKeycloakUserRequest.Email
                           && ur.Enabled == createKeycloakUserRequest.Enabled
                           && ur.RequiredActions == createKeycloakUserRequest.RequiredActions),
                    KeycloakOptions.Realm,
                    It.IsAny<CancellationToken>()
                ),
            Times.Once);
    }
    
    [DebuggerStepThrough]
    private Task CallAsync() => Keycloak.CreateUserAsync(createKeycloakUserRequest);
}