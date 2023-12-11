using System.Diagnostics;
using AutoFixture;
using FluentAssertions;
using Moq;
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
        
        KeycloakClientMock.Invocations.Should().BeEmpty();
    }

    [Fact]
    public async Task CallsTheKeycloakClientAndReturnsNull()
    {
        // Arrange
        KeycloakClientMock.Setup(m => m.UsersAll3Async(
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
                It.IsAny<CancellationToken>()
            )
        ).ReturnsAsync(Array.Empty<UserRepresentation>());
        
        // Act
        var keycloakUser = await CallAsync();
        
        // Assert
        keycloakUser.Should().BeNull();
        
        KeycloakClientMock.Verify(m
                => m.UsersAll3Async(
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
                    It.IsAny<CancellationToken>()
                ),
            Times.Once);
    }
    
    [Fact]
    public async Task CallsTheKeycloakClientAndReturnsTheKeycloakUser()
    {
        // Arrange
        KeycloakClientMock.Setup(m => m.UsersAll3Async(
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
            It.IsAny<CancellationToken>()
            )
        ).ReturnsAsync(Fixture.CreateMany<UserRepresentation>(1).ToList);
        
        // Act
        var keycloakUser = await CallAsync();
        
        // Assert
        keycloakUser.Should().NotBeNull();
        
        KeycloakClientMock.Verify(m
                => m.UsersAll3Async(
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
                    It.IsAny<CancellationToken>()
                ),
            Times.Once);
    }

    [DebuggerStepThrough]
    private Task<KeycloakUser?> CallAsync() => Keycloak.GetUserByUsernameAsync(username);
}