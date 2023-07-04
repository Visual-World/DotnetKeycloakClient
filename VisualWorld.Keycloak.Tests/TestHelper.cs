using AutoFixture;
using Microsoft.Extensions.Options;
using Moq;

namespace VisualWorld.Keycloak.Tests;

public static class TestHelper
{
    public static (IOptionsSnapshot<T> OptionsSnapshot, T Options) CreateOptionsSnapshotMock<T>(this IFixture fixture)
        where T : class
    {
        var options = fixture.Create<T>();
        
        var optionsSnapshotMock = new Mock<IOptionsSnapshot<T>>();
        optionsSnapshotMock.SetupGet(m => m.Value)
            .Returns(options);
        
        fixture.Inject(optionsSnapshotMock.Object);

        return (optionsSnapshotMock.Object, options);
    }
}