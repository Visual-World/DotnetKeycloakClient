using IdentityModel.Client;
using Microsoft.Extensions.Options;
using VisualWorld.Keycloak;

namespace Microsoft.Extensions.DependencyInjection;

public static class Registrations
{
    public static IServiceCollection AddKeycloak(this IServiceCollection services, Action<KeycloakOptions>? configure = null)
    {
        var keycloakOptionsBuilder = services.AddOptions<KeycloakOptions>();
        if (configure is not null)
        {
            keycloakOptionsBuilder.Configure(configure);
        }

        services.AddAccessTokenManagement((sp, options) =>
        {
            var keycloakOptions = sp.GetRequiredService<IOptions<KeycloakOptions>>().Value;
            
            options.Client.Clients.Add("KeycloakClient", new ClientCredentialsTokenRequest
            {
                ClientId = keycloakOptions.ClientId,
                ClientSecret = keycloakOptions.ClientSecret,
                Address = keycloakOptions.TokenUrl,
            });
        });
        services.AddKeycloakApiClient().AddClientAccessTokenHandler();

        return services.AddScoped<IKeycloak, Keycloak>();
    }

    private static IHttpClientBuilder AddKeycloakApiClient(this IServiceCollection services)
        => services
            .AddScoped<IKeycloakClient, KeycloakClient>()
            .AddHttpClient<IKeycloakClient, KeycloakClient>((sp, httpClientBuilder) =>
            {
                var keycloakOptions = sp.GetRequiredService<IOptions<KeycloakOptions>>().Value;

                httpClientBuilder.BaseAddress = new Uri(keycloakOptions.ApiUrl);
            });
}