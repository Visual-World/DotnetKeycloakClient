using System.Diagnostics;
using AutoFixture;
using FluentAssertions;
using Xunit;

namespace VisualWorld.Keycloak.Tests.MapperTests;

public sealed class ToKeycloakUser
{
    private readonly IFixture fixture;

    private UserRepresentation userRepresentation;

    public ToKeycloakUser()
    {
        fixture = new Fixture();

        userRepresentation = fixture.Create<UserRepresentation>();
    }

    [Fact]
    public void ThrowsArgumentNullExceptionIfUserRepresentationIsNull()
    {
        // Arrange
        userRepresentation = null;

        // Act, Assert
        var exception = Assert.Throws<ArgumentNullException>(() => Call());
        exception.ParamName.Should().Be("userRepresentation");
    }

    [Fact]
    public void MapsToKeycloakUser()
    {
        // Act
        var mapped = Call();

        // Assert
        mapped.Email.Should().Be(userRepresentation.Email);
        mapped.Enabled.Should().Be(userRepresentation.Enabled ?? false);
        mapped.Username.Should().Be(userRepresentation.Username);
        mapped.EmailVerified.Should().Be(userRepresentation.EmailVerified ?? false);
        mapped.FirstName.Should().Be(userRepresentation.FirstName);
        mapped.LastName.Should().Be(userRepresentation.LastName);
        mapped.FederatedIdentities.Should().HaveCount(userRepresentation.FederatedIdentities.Count);
        mapped.RequiredActions.Should().BeEquivalentTo(userRepresentation.RequiredActions);
    }

    [DebuggerStepThrough]
    private KeycloakUser Call()
    {
        return userRepresentation.ToKeycloakUser();
    }
}