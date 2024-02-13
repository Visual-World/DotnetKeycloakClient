using System.Diagnostics;
using AutoFixture;
using FluentAssertions;
using NSubstitute;
using Xunit;

namespace VisualWorld.Keycloak.Tests.KeycloakTests;

public sealed class GetUserByIdAsync : Infrastructure
{
    private string userId;

    public GetUserByIdAsync()
    {
        userId = Fixture.Create<string>();
    }

    [Theory]
    [InlineData(null)]
    [InlineData("")]
    [InlineData(" ")]
    public async Task ThrowsArgumentExceptionIfUserIdIs(string userId)
    {
        // Arrange
        this.userId = userId;

        // Act, Assert
        var exception = await Assert.ThrowsAsync<ArgumentException>(() => CallAsync());
        exception.ParamName.Should().Be("userId");

        KeycloakClientSubstitute.ReceivedCalls().Should().BeEmpty();
    }

    [Fact]
    public async Task CallsTheKeycloakClientAndReturnsTheKeycloakUser()
    {
        // Arrange
        KeycloakClientSubstitute.UsersGET2Async(
                KeycloakOptions.Realm,
                userId,
                Arg.Any<CancellationToken>())
            .Returns(Fixture.Create<UserRepresentation>());

        // Act
        var keycloakUser = await CallAsync();

        // Assert
        keycloakUser.Should().NotBeNull();

        KeycloakClientSubstitute.Received(1)
            .UsersGET2Async(
                KeycloakOptions.Realm,
                userId,
                Arg.Any<CancellationToken>()
            );
    }

    [DebuggerStepThrough]
    private Task<KeycloakUserWithFederatedIdentities> CallAsync()
    {
        return Keycloak.GetUserByIdAsync(userId);
    }
}