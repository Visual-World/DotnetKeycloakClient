using AutoFixture;
using AutoFixture.AutoNSubstitute;
using DotNet.Testcontainers.Builders;
using DotNet.Testcontainers.Containers;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Xunit;

namespace VisualWorld.Keycloak.IntegrationTests;

public abstract class Infrastructure : IAsyncLifetime
{
    protected readonly IFixture Fixture = new Fixture().Customize(new AutoNSubstituteCustomization());
    
    protected readonly IServiceCollection Services;

    protected readonly IConfigurationBuilder ConfigurationBuilder;
    
    protected IContainer KeycloakContainer = default!;

    protected readonly Lazy<IConfiguration> LazyConfiguration;
    
    protected readonly Lazy<IServiceProvider> LazyServiceProvider;

    public Infrastructure()
    {
        ConfigurationBuilder = new ConfigurationBuilder();
        
        Services = new ServiceCollection();

        LazyConfiguration = new Lazy<IConfiguration>(() => ConfigurationBuilder.Build());
        LazyServiceProvider = new Lazy<IServiceProvider>(() => Services.BuildServiceProvider());

        Services.AddKeycloak(options => LazyConfiguration.Value.Bind("Keycloak", options));
    }

    public async Task InitializeAsync()
    {
        KeycloakContainer = new ContainerBuilder()
            .WithImage("bitnami/keycloak:latest")
            .WithEnvironment("KEYCLOAK_ADMIN", "admin")
            .WithEnvironment("KEYCLOAK_ADMIN_PASSWORD", "admin")
            .WithEnvironment("KEYCLOAK_DATABASE_VENDOR", "dev-mem")
            .WithEnvironment("KEYCLOAK_ENABLE_HEALTH_ENDPOINTS", "true")
            .WithEnvironment("KEYCLOAK_EXTRA_ARGS", "--import-realm")
            .WithResourceMapping(Path.Join(CommonDirectoryPath.GetSolutionDirectory().DirectoryPath, "integrationtest-environment"), "/opt/bitnami/keycloak/data/import")
            .WithExposedPort(8080)
            .WithPortBinding(8080,8080)
            .WithWaitStrategy(
                Wait.ForUnixContainer()
                    .UntilHttpRequestIsSucceeded(request => request.ForPath("/health/ready").ForPort(8080)))
            .Build();
        
        await KeycloakContainer.StartAsync();

        var keycloakBaseUrl = new UriBuilder(Uri.UriSchemeHttp, KeycloakContainer.Hostname, KeycloakContainer.GetMappedPublicPort(8080)).ToString();
        
        ConfigurationBuilder.AddInMemoryCollection(new []
        {
            new KeyValuePair<string, string?>("Keycloak:ClientId", "test-admin-client"),
            new KeyValuePair<string, string?>("Keycloak:ClientSecret", "1nZh8UYWrTD3BfrPwoiU70dKCtrJRgYi"),
            new KeyValuePair<string, string?>("Keycloak:BaseUrl", keycloakBaseUrl),
            new KeyValuePair<string, string?>("Keycloak:Realm", "Test"),
        });
    }

    public async Task DisposeAsync() => await KeycloakContainer.DisposeAsync();
}
