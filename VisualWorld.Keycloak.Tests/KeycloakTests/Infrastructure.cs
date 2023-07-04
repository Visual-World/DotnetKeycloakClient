using AutoFixture;
using AutoFixture.AutoMoq;
using Moq;

namespace VisualWorld.Keycloak.Tests.KeycloakTests;

public abstract class Infrastructure
{
    protected readonly IFixture Fixture;

    protected readonly Mock<IKeycloakClient> KeycloakClientMock = new ();

    protected readonly KeycloakOptions KeycloakOptions;

    protected readonly Keycloak Keycloak;

    protected Infrastructure()
    {
        Fixture = new Fixture().Customize(new AutoMoqCustomization());
        
        Fixture.Inject(KeycloakClientMock.Object);
        
        var (_, options) = Fixture.CreateOptionsSnapshotMock<KeycloakOptions>();
        KeycloakOptions = options;

        Keycloak = Fixture.Create<Keycloak>();
    }
}