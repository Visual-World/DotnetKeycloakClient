using System.Diagnostics;
using AutoFixture;
using FluentAssertions;
using Xunit;

namespace VisualWorld.Keycloak.Tests.MapperTests;

public sealed class ToKeycloakFederatedIdentity
{
    private readonly IFixture fixture;
    
    private FederatedIdentityRepresentation federatedIdentityRepresentation;

    public ToKeycloakFederatedIdentity()
    {
        fixture = new Fixture();

        federatedIdentityRepresentation = fixture.Create<FederatedIdentityRepresentation>();
    }
    
    [Fact]
    public void ThrowsArgumentNullExceptionIfFederatedIdentityRepresentationIsNull()
    {
        // Arrange
        federatedIdentityRepresentation = null;
        
        // Act, Assert
        var exception = Assert.Throws<ArgumentNullException>(() => Call());
        exception.ParamName.Should().Be("federatedIdentityRepresentation");
    }

    [Fact]
    public void MapsToFederatedIdentity()
    {
        // Act
        var mapped = Call();
        
        // Assert
        mapped.Username.Should().Be(federatedIdentityRepresentation.UserName);
        mapped.UserId.Should().Be(federatedIdentityRepresentation.UserId);
        mapped.IdentityProvider.Should().Be(federatedIdentityRepresentation.IdentityProvider);
    }
    
    [DebuggerStepThrough]
    private KeycloakFederatedIdentity Call() => federatedIdentityRepresentation.ToKeycloakFederatedIdentity();
}