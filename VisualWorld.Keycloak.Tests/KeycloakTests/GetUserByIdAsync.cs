using System.Diagnostics;
using AutoFixture;
using AutoFixture.AutoMoq;
using FluentAssertions;
using Moq;
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
        
        KeycloakClientMock.Invocations.Should().BeEmpty();
    }

    [Fact]
    public async Task CallsTheKeycloakClientAndReturnsTheKeycloakUser()
    {
        // Arrange
        KeycloakClientMock.Setup(m => m.UsersGET2Async(
                KeycloakOptions.Realm,
                userId,
                It.IsAny<CancellationToken>()
            )
        ).ReturnsUsingFixture(Fixture);
        
        // Act
        var keycloakUser = await CallAsync();
        
        // Assert
        keycloakUser.Should().NotBeNull();
        
        KeycloakClientMock.Verify(m
                => m.UsersGET2Async(
                    KeycloakOptions.Realm,
                    userId,
                    It.IsAny<CancellationToken>()
                ),
            Times.Once);
    }

    [DebuggerStepThrough]
    private Task<KeycloakUser> CallAsync() => Keycloak.GetUserByIdAsync(userId);
}