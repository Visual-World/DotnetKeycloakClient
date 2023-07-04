﻿using System.Diagnostics;
using AutoFixture;
using FluentAssertions;
using Moq;
using Xunit;

namespace VisualWorld.Keycloak.Tests.KeycloakTests;

public sealed class SendVerifyEmailAsync : Infrastructure
{
    private string userId;

    public SendVerifyEmailAsync()
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
    public async Task CallsTheKeycloakClient()
    {
        // Act
        await CallAsync();
        
        // Assert
        KeycloakClientMock.Verify(m
                => m.SendVerifyEmailAsync(
                    KeycloakOptions.Realm,
                    userId,
                    null, // client_id
                    null, // redirect_uri
                    It.IsAny<CancellationToken>()
                ),
            Times.Once);
    }

    [DebuggerStepThrough]
    private Task CallAsync() => Keycloak.SendVerifyEmailAsync(userId);
}