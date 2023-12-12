using AutoFixture;
using AutoFixture.AutoNSubstitute;

namespace VisualWorld.Keycloak.Tests.KeycloakTests;

public abstract class Infrastructure
{
    protected readonly IFixture Fixture = new Fixture().Customize(new AutoNSubstituteCustomization());

    protected readonly Keycloak Keycloak;

    protected readonly IKeycloakClient KeycloakClientSubstitute;

    protected readonly KeycloakOptions KeycloakOptions;

    protected Infrastructure()
    {
        KeycloakClientSubstitute = Fixture.Freeze<IKeycloakClient>();

        var (_, options) = Fixture.CreateOptionsSnapshotMock<KeycloakOptions>();
        KeycloakOptions = options;

        Keycloak = Fixture.Create<Keycloak>();
    }
}