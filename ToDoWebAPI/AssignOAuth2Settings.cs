using System.Collections.Generic;
using System.Linq;
using System.Web.Http;
using System.Web.Http.Description;
using Swashbuckle.Swagger;

namespace ToDoWebAPI
{
  public class AssignOAuth2Settings : IOperationFilter
  {
    public void Apply(Operation operation, SchemaRegistry schemaRegistry,
          ApiDescription apiDescription)
    {
      var actFilters = apiDescription.ActionDescriptor.GetFilterPipeline();
      var allowAnonymous = actFilters
        .Select(f => f.Instance)
        .OfType<OverrideAuthorizationAttribute>().Any();

      if (allowAnonymous) return; // must be an anonymous method

      if (operation.security == null) operation.security =
                  new List<IDictionary<string, IEnumerable<string>>>();

      // first string is your c.Oauth2 value in swagger config
      // second list of string is the scopes you are requesting.
      //          Should match swagger cofig "scioes" setting
      var oAuthRequirements = new Dictionary<string, IEnumerable<string>>
      { { "oauth2", new List<string> { "api" } } };

      operation.security.Add(oAuthRequirements);
    }
  }
}