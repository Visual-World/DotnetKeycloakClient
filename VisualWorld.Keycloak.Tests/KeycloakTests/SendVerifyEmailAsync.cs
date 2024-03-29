﻿using System.Diagnostics;
using AutoFixture;
using FluentAssertions;
using NSubstitute;
using Xunit;

namespace VisualWorld.Keycloak.Tests.KeycloakTests;

public sealed class SendVerifyEmailAsync : Infrastructure
{
    private readonly string clientId;
    private readonly string redirectUri;
    private string userId;

    public SendVerifyEmailAsync()
    {
        userId = Fixture.Create<string>();
        clientId = Fixture.Create<string>();
        redirectUri = Fixture.Create<string>();
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
            .SendVerifyEmailAsync(
                KeycloakOptions.Realm,
                userId,
                clientId, // client_id
                redirectUri, // redirect_uri
                Arg.Any<CancellationToken>()
            );
    }

    [DebuggerStepThrough]
    private Task CallAsync()
    {
        return Keycloak.SendVerifyEmailAsync(userId, clientId, redirectUri);
    }
}