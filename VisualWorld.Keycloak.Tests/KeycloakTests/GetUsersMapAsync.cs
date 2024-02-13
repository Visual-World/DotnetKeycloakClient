using System.Diagnostics;
using AutoFixture;
using FluentAssertions;
using NSubstitute;
using Xunit;

namespace VisualWorld.Keycloak.Tests.KeycloakTests;

public sealed class GetUsersMapAsync : Infrastructure
{
    private readonly bool? enabled;

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

        KeycloakClientSubstitute.UsersAll3Async(
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
                -1, // max
                null, // q
                null, // search
                null, // username
                Arg.Any<CancellationToken>())
            .Returns(new[]
            {
                userWithEmailAddressAndUsername,
                userWitNullEmailAddressAndNullUsername,
                userWitEmptyEmailAddressAndEmptyUsername,
                userWitWhiteSpaceEmailAddressAndWhitespaceUsername
            });

        // Act
        var emailAddressesMap = await CallAsync();

        // Assert
        emailAddressesMap.Should().NotBeEmpty()
            .And.HaveCount(1)
            .And.ContainSingle(i => i.UserId == userWithEmailAddressAndUsername.Id
                                    && i.Username == userWithEmailAddressAndUsername.Username);

        KeycloakClientSubstitute.Received(1)
            .UsersAll3Async(
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
                -1, // max
                null, // q
                null, // search
                null, // username
                Arg.Any<CancellationToken>());
    }

    [DebuggerStepThrough]
    private Task<IReadOnlySet<(string Username, string UserId)>> CallAsync()
    {
        return Keycloak.GetUsersMapAsync(enabled);
    }
}