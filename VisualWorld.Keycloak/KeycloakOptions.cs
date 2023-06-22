using System.ComponentModel.DataAnnotations;

namespace VisualWorld.Keycloak;

public sealed class KeycloakOptions
{
    [Required] 
    public string ClientId { get; set; } = string.Empty;
    
    [Required]
    public string ClientSecret { get; set; }= string.Empty;
    
    [Required]
    public string BaseUrl { get; set; }= string.Empty;
    
    [Required]
    public string Realm { get; set; }= string.Empty;

    public string ApiUrl => BaseUrl + "/admin/realms/";
    
    public string TokenUrl => BaseUrl + "/realms/" + Realm + "/protocol/openid-connect/token";
}