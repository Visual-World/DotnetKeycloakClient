using AutoFixture;
using Microsoft.Extensions.Options;
using NSubstitute;

namespace VisualWorld.Keycloak.Tests;

public static class TestHelper
{
    public static (IOptionsSnapshot<T> OptionsSnapshot, T Options) CreateOptionsSnapshotMock<T>(this IFixture fixture)
        where T : class
    {
        var options = fixture.Create<T>();

        var optionsSnapshotSubstitute = Substitute.For<IOptionsSnapshot<T>>();
        optionsSnapshotSubstitute.Value.Returns(options);

        fixture.Inject(optionsSnapshotSubstitute);

        return (optionsSnapshotSubstitute, options);
    }
}