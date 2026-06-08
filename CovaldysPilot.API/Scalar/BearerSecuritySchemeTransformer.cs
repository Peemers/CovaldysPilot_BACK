using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.OpenApi;
using Microsoft.OpenApi;

namespace CovaldysPilot.API.Scalar;

internal sealed class BearerSecuritySchemeTransformer(IAuthenticationSchemeProvider authenticationSchemeProvider): IOpenApiDocumentTransformer
{
  public async Task TransformAsync(
    OpenApiDocument document,
    OpenApiDocumentTransformerContext context,
    CancellationToken cancellationToken)
  {
    IEnumerable<AuthenticationScheme> authenticationSchemes =
      await authenticationSchemeProvider.GetAllSchemesAsync();

    if (!authenticationSchemes.Any(a => a.Name == "Bearer"))
      return;

    OpenApiSecurityScheme bearerScheme = new OpenApiSecurityScheme
    {
      Type = SecuritySchemeType.Http,
      Scheme = "bearer",
      BearerFormat = "JWT",
      In = ParameterLocation.Header,
      Description = "JWT Authorization header using the Bearer scheme."
    };

    document.Components ??= new OpenApiComponents();
    document.AddComponent("Bearer", bearerScheme);

    OpenApiSecurityRequirement securityRequirement = new OpenApiSecurityRequirement
    {
      [new OpenApiSecuritySchemeReference("Bearer", document)] = []
    };

    foreach (OpenApiPathItem path in document.Paths.Values)
    {
      if (path.Operations == null) continue;
      foreach (OpenApiOperation operation in path.Operations.Values)
      {
        operation.Security ??= new List<OpenApiSecurityRequirement>();
        operation.Security.Add(securityRequirement);
      }
    }
  }
}