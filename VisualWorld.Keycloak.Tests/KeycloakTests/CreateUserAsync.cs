using System.Diagnostics;
using AutoFixture;
using FluentAssertions;
using NSubstitute;
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

        KeycloakClientSubstitute.ReceivedCalls().Should().BeEmpty();
    }

    [Fact]
    public async Task CallsTheKeycloakClient()
    {
        // Act
        await CallAsync();

        // Assert
        await KeycloakClientSubstitute.Received(1)
            .UsersPOSTAsync(
                Arg.Is<UserRepresentation>(ur
                    => ur.Username == createKeycloakUserRequest.Username
                       && ur.Email == createKeycloakUserRequest.Email
                       && ur.FirstName == createKeycloakUserRequest.FirstName
                       && ur.LastName == createKeycloakUserRequest.LastName
                       && ur.Enabled == createKeycloakUserRequest.Enabled
                       && ur.RequiredActions == createKeycloakUserRequest.RequiredActions),
                KeycloakOptions.Realm,
                Arg.Any<CancellationToken>()
            );
    }

    [DebuggerStepThrough]
    private Task CallAsync()
    {
        return Keycloak.CreateUserAsync(createKeycloakUserRequest);
    }
}