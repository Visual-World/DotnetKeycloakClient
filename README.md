# Introduction 
This lib provides an api interface to call the keycloak api from dotnet. 

Attention: Only a few set of functions was already implemented.

# Usage
```csharp
webApplicationBuilder.Services.AddKeycloak(options => webApplicationBuilder.Configuration.Bind("Keycloak", options))
```