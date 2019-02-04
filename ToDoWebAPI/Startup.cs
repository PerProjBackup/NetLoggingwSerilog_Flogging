using System;
using System.Collections.Generic;
using System.Linq;
using IdentityServer3.AccessTokenValidation;
using Microsoft.Owin;
using Owin;

[assembly: OwinStartup(typeof(ToDoWebAPI.Startup))]

namespace ToDoWebAPI
{
  public partial class Startup
  {
    public void Configuration(IAppBuilder app)
    {
      //ConfigureAuth(app);
      // many times this would be in a "ConfigurationAuth" method
      app.UseIdentityServerBearerTokenAuthentication(
        new IdentityServerBearerTokenAuthenticationOptions
        {
          Authority = "https://demo.identityserver.io",
          RequiredScopes = new[] { "api" }
        });

    }
  }
}
