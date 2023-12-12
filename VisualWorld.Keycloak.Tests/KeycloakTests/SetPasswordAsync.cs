using System.Diagnostics;
using AutoFixture;
using FluentAssertions;
using NSubstitute;
using Xunit;

namespace VisualWorld.Keycloak.Tests.KeycloakTests;

public sealed class SetPasswordAsync : Infrastructure
{
    private string password;
    private string userId;

    public SetPasswordAsync()
    {
        userId = Fixture.Create<string>();
        password = Fixture.Create<string>();
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

    [Theory]
    [InlineData(null)]
    [InlineData("")]
    [InlineData(" ")]
    public async Task ThrowsArgumentExceptionIfPasswordIs(string password)
    {
        // Arrange
        this.password = password;

        // Act, Assert
        var exception = await Assert.ThrowsAsync<ArgumentException>(() => CallAsync());
        exception.ParamName.Should().Be("password");

        KeycloakClientSubstitute.ReceivedCalls().Should().BeEmpty();
    }

    [Fact]
    public async Task CallsTheKeycloakClient()
    {
        // Act
        await CallAsync();

        // Assert
        KeycloakClientSubstitute.Received(1)
            .ResetPasswordAsync(
                Arg.Is<CredentialRepresentation>(cr => cr.Value == password && cr.Temporary == true),
                KeycloakOptions.Realm,
                userId,
                Arg.Any<CancellationToken>()
            );
    }

    [DebuggerStepThrough]
    private Task CallAsync()
    {
        return Keycloak.SetPasswordAsync(userId, password);
    }
}