using System.Diagnostics;
using AutoFixture;
using FluentAssertions;
using NSubstitute;
using Xunit;

namespace VisualWorld.Keycloak.Tests.KeycloakTests;

public sealed class GetUserByUsernameAsync : Infrastructure
{
    private string username;

    public GetUserByUsernameAsync()
    {
        username = Fixture.Create<string>();
    }

    [Theory]
    [InlineData(null)]
    [InlineData("")]
    [InlineData(" ")]
    public async Task ThrowsArgumentExceptionIfUsernameAndEmailIs(string username)
    {
        // Arrange
        this.username = username;

        // Act, Assert
        var exception = await Assert.ThrowsAsync<ArgumentException>(() => CallAsync());
        exception.ParamName.Should().Be("username");

        KeycloakClientSubstitute.ReceivedCalls().Should().BeEmpty();
    }

    [Fact]
    public async Task CallsTheKeycloakClientAndReturnsNull()
    {
        // Arrange
        KeycloakClientSubstitute.UsersAll3Async(
                KeycloakOptions.Realm,
                null, // briefRepresentation
                null,
                null, // emailVerified
                true, // enabled
                null, // exact
                null, // first
                null, // firstName
                null, //idpAlias
                null, // idpUserId
                null, // lastName
                1, // max
                null, // q
                null, // search
                username,
                Arg.Any<CancellationToken>())
            .Returns(Array.Empty<UserRepresentation>());

        // Act
        var keycloakUser = await CallAsync();

        // Assert
        keycloakUser.Should().BeNull();

        KeycloakClientSubstitute.Received(1)
            .UsersAll3Async(
                KeycloakOptions.Realm,
                null, // briefRepresentation
                null,
                null, // emailVerified
                true, // enabled
                null, // exact
                null, // first
                null, // firstName
                null, //idpAlias
                null, // idpUserId
                null, // lastName
                1, // max
                null, // q
                null, // search
                username,
                Arg.Any<CancellationToken>());
    }

    [Fact]
    public async Task CallsTheKeycloakClientAndReturnsTheKeycloakUser()
    {
        // Arrange
        KeycloakClientSubstitute.UsersAll3Async(
                KeycloakOptions.Realm,
                null, // briefRepresentation
                null,
                null, // emailVerified
                true, // enabled
                null, // exact
                null, // first
                null, // firstName
                null, //idpAlias
                null, // idpUserId
                null, // lastName
                1, // max
                null, // q
                null, // search
                username,
                Arg.Any<CancellationToken>())
            .Returns(Fixture.CreateMany<UserRepresentation>(1).ToList());

        // Act
        var keycloakUser = await CallAsync();

        // Assert
        keycloakUser.Should().NotBeNull();

        KeycloakClientSubstitute.Received(1)
            .UsersAll3Async(
                KeycloakOptions.Realm,
                null, // briefRepresentation
                null,
                null, // emailVerified
                true, // enabled
                null, // exact
                null, // first
                null, // firstName
                null, //idpAlias
                null, // idpUserId
                null, // lastName
                1, // max
                null, // q
                null, // search
                username,
                Arg.Any<CancellationToken>()
            );
    }

    [DebuggerStepThrough]
    private Task<KeycloakUser?> CallAsync()
    {
        return Keycloak.GetUserByUsernameAsync(username);
    }
}