using System.Diagnostics;
using AutoFixture;
using FluentAssertions;
using Moq;
using Xunit;

namespace VisualWorld.Keycloak.Tests.KeycloakTests;

public sealed class GetUserAsync : Infrastructure
{
    private string usernameAndEmail;

    public GetUserAsync()
    {
        usernameAndEmail = Fixture.Create<string>();
    }

    [Theory]
    [InlineData(null)]
    [InlineData("")]
    [InlineData(" ")]
    public async Task ThrowsArgumentExceptionIfUsernameAndEmailIs(string usernameAndEmail)
    {
        // Arrange
        this.usernameAndEmail = usernameAndEmail;
        
        // Act, Assert
        var exception = await Assert.ThrowsAsync<ArgumentException>(() => CallAsync());
        exception.ParamName.Should().Be("usernameAndEmail");
        
        KeycloakClientMock.Invocations.Should().BeEmpty();
    }

    [Fact]
    public async Task CallsTheKeycloakClientAndReturnsNull()
    {
        // Arrange
        KeycloakClientMock.Setup(m => m.UsersAll3Async(
                KeycloakOptions.Realm,
                null, // briefRepresentation
                usernameAndEmail,
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
                usernameAndEmail,
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
                    usernameAndEmail,
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
                    usernameAndEmail,
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
            usernameAndEmail,
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
            usernameAndEmail,
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
                    usernameAndEmail,
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
                    usernameAndEmail,
                    It.IsAny<CancellationToken>()
                ),
            Times.Once);
    }

    [DebuggerStepThrough]
    private Task<KeycloakUser?> CallAsync() => Keycloak.GetUserAsync(usernameAndEmail);
}