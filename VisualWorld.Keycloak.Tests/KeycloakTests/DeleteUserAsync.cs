using System.Diagnostics;
using AutoFixture;
using FluentAssertions;
using NSubstitute;
using Xunit;

namespace VisualWorld.Keycloak.Tests.KeycloakTests;

public sealed class DeleteUserAsync : Infrastructure
{
    private string userId;

    public DeleteUserAsync()
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
    public async Task CallsTheKeycloakClient()
    {
        // Act
        await CallAsync();

        // Assert
        KeycloakClientSubstitute.Received(1)
            .UsersDELETE3Async(
                KeycloakOptions.Realm,
                userId,
                Arg.Any<CancellationToken>()
            );
    }

    [DebuggerStepThrough]
    private Task CallAsync()
    {
        return Keycloak.DeleteUserAsync(userId);
    }
}