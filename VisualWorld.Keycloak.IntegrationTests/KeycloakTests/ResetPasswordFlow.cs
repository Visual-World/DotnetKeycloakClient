using FluentAssertions;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
using Xunit;

namespace VisualWorld.Keycloak.IntegrationTests.KeycloakTests;

public sealed class ResetPasswordFlow : Infrastructure
{
    [Fact]
    public async Task ResetsThePasswordCorrectly()
    {
        var keycloak = LazyServiceProvider.Value.GetRequiredService<IKeycloak>();
        
        // Create test-user
        await keycloak.CreateUserAsync(new CreateKeycloakUserRequest("test-user")
        {
            Enabled = true
        });

        // Get test-user
        var testUser = await keycloak.GetUserByUsernameAsync("test-user");
        testUser.Should().NotBeNull();
        testUser!.Id.Should().NotBeNullOrEmpty();
        testUser.Username.Should().Be("test-user");
        testUser.Email.Should().BeNullOrEmpty();
        testUser.EmailVerified.Should().BeFalse();
        testUser.Enabled.Should().BeTrue();
        testUser.FirstName.Should().BeNullOrEmpty();
        testUser.LastName.Should().BeNullOrEmpty();

        // Set password
        await keycloak.SetPasswordAsync(testUser.Id, "test-password");
        // check if calling the method twice will not throw
        await keycloak.SetPasswordAsync(testUser.Id, "test-password");
        
        // Check if credential is set
        var keycloakClient = LazyServiceProvider.Value.GetRequiredService<IKeycloakClient>();
        var keycloakOptions = LazyServiceProvider.Value.GetRequiredService<IOptionsSnapshot<KeycloakOptions>>();
        
        var credentials = await keycloakClient.CredentialsAllAsync(keycloakOptions.Value.Realm, testUser.Id);
        
        credentials.Should()
            .NotBeNullOrEmpty()
            .And.HaveCount(1)
            .And.ContainSingle(cr => cr.Type == "password");
    }
}