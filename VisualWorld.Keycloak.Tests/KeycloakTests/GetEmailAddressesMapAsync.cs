using System.Diagnostics;
using AutoFixture;
using FluentAssertions;
using Moq;
using Xunit;

namespace VisualWorld.Keycloak.Tests.KeycloakTests;

public sealed class GetEmailAddressesMapAsync : Infrastructure
{
    private bool? enabled;

    public GetEmailAddressesMapAsync()
    {
        enabled = Fixture.Create<bool?>();
    }

    [Fact]
    public async Task CallsTheKeycloakClientAndReturnsAnyWithAnEmailAddress()
    {
        // Arrange
        var userWithEmailAddress = Fixture.Create<UserRepresentation>();
        var userWitNullEmailAddress = Fixture.Build<UserRepresentation>()
            .With(f => f.Email, (string)null)
            .Create();
        var userWitEmptyEmailAddress = Fixture.Build<UserRepresentation>()
            .With(f => f.Email, string.Empty)
            .Create();
        var userWitWhiteSpaceEmailAddress = Fixture.Build<UserRepresentation>()
            .With(f => f.Email, " ")
            .Create();

        KeycloakClientMock.Setup(m => m.UsersAll3Async(
            KeycloakOptions.Realm,
            true, // briefRepresentation
            null, // email
            null, // emailVerified
            enabled, // enabled
            null, // exact
            null, // first
            null, // firstName
            null, //idpAlias
            null, // idpUserId
            null, // lastName
            null, // max
            null, // q
            null, // search
            null, // username
            It.IsAny<CancellationToken>())
        ).ReturnsAsync(new[]
        {
            userWithEmailAddress,
            userWitNullEmailAddress,
            userWitEmptyEmailAddress,
            userWitWhiteSpaceEmailAddress,
        });
        
        // Act
        var emailAddressesMap = await CallAsync();
        
        // Assert
        emailAddressesMap.Should().NotBeEmpty()
            .And.HaveCount(1)
            .And.ContainSingle(i => i.Email == userWithEmailAddress.Email
                                    && i.UserId == userWithEmailAddress.Id);
        
        KeycloakClientMock.Verify(m
                => m.UsersAll3Async(
                    KeycloakOptions.Realm,
                    true, // briefRepresentation
                    null, // email
                    null, // emailVerified
                    enabled, // enabled
                    null, // exact
                    null, // first
                    null, // firstName
                    null, //idpAlias
                    null, // idpUserId
                    null, // lastName
                    null, // max
                    null, // q
                    null, // search
                    null, // username
                    It.IsAny<CancellationToken>()),
            Times.Once);
    }

    [DebuggerStepThrough]
    private Task<IReadOnlySet<(string Email, string UserId)>> CallAsync() => Keycloak.GetEmailAddressesMapAsync(enabled);
}