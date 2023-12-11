using System.Diagnostics;
using AutoFixture;
using FluentAssertions;
using Moq;
using Xunit;

namespace VisualWorld.Keycloak.Tests.KeycloakTests;

public sealed class GetUsersMapAsync : Infrastructure
{
    private bool? enabled;

    public GetUsersMapAsync()
    {
        enabled = Fixture.Create<bool?>();
    }

    [Fact]
    public async Task CallsTheKeycloakClientAndReturnsAnyWithAnEmailAddress()
    {
        // Arrange
        var userWithEmailAddressAndUsername = Fixture.Create<UserRepresentation>();
        var userWitNullEmailAddressAndNullUsername = Fixture.Build<UserRepresentation>()
            .With(f => f.Email, (string)null)
            .With(f => f.Username, (string)null)
            .Create();
        var userWitEmptyEmailAddressAndEmptyUsername = Fixture.Build<UserRepresentation>()
            .With(f => f.Email, string.Empty)
            .With(f => f.Username, string.Empty)
            .Create();
        var userWitWhiteSpaceEmailAddressAndWhitespaceUsername = Fixture.Build<UserRepresentation>()
            .With(f => f.Email, " ")
            .With(f => f.Username, " ")
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
            userWithEmailAddressAndUsername,
            userWitNullEmailAddressAndNullUsername,
            userWitEmptyEmailAddressAndEmptyUsername,
            userWitWhiteSpaceEmailAddressAndWhitespaceUsername,
        });
        
        // Act
        var emailAddressesMap = await CallAsync();
        
        // Assert
        emailAddressesMap.Should().NotBeEmpty()
            .And.HaveCount(1)
            .And.ContainSingle(i => i.Email == userWithEmailAddressAndUsername.Email
                                    && i.UserId == userWithEmailAddressAndUsername.Id
                                    && i.Username == userWithEmailAddressAndUsername.Username);
        
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
    private Task<IReadOnlySet<(string? Email, string? Username, string UserId)>> CallAsync() => Keycloak.GetUsersMapAsync(enabled);
}