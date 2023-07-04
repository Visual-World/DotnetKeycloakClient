using System.Diagnostics;
using AutoFixture;
using FluentAssertions;
using Microsoft.Extensions.Options;
using Moq;
using Xunit;

namespace VisualWorld.Keycloak.Tests.KeycloakTests;

public sealed class Constructor
{
    private readonly IFixture fixture = new Fixture();
    
    private IKeycloakClient keycloakClient;
    
    private IOptionsSnapshot<KeycloakOptions> keycloakOptions;
    
    [Fact]
    public void ThrowsArgumentNullExceptionIfKeycloakClientIsNull()
    {
        // Arrange
        var (optionsSnapshot, _) = fixture.CreateOptionsSnapshotMock<KeycloakOptions>();
        keycloakOptions = optionsSnapshot;
        
        // Act, Assert
        var exception = Assert.Throws<ArgumentNullException>(() => Call());
        exception.ParamName.Should().Be("keycloakClient");
    }
    
    [Fact]
    public void ThrowsArgumentNullExceptionIfKeycloakOptionsIsNull()
    {
        // Arrange
        keycloakClient = new Mock<IKeycloakClient>().Object;
        
        // Act, Assert
        var exception = Assert.Throws<ArgumentNullException>(() => Call());
        exception.ParamName.Should().Be("keycloakOptions");
    }

    [DebuggerStepThrough]
    private Keycloak Call() => new (keycloakClient, keycloakOptions);
}