using Microsoft.AspNetCore.Authorization;
using Microsoft.OpenApi.Models;
using Swashbuckle.AspNetCore.SwaggerGen;
using System.Collections.Generic;
using System.Linq;
using System.Net;

namespace FreelancerWeb.Authentication
{
    public class AuthOperationAttribute : Swashbuckle.AspNetCore.SwaggerGen.IOperationFilter
    {
        public void Apply(OpenApiOperation operation, OperationFilterContext context)
        {
            var authAttributes = context.MethodInfo
            .GetCustomAttributes(true)
            .OfType<AuthorizeAttribute>()
            .Distinct();

            if (authAttributes.Any())
            {
                if (operation.Parameters == null)
                    operation.Parameters = new List<OpenApiParameter>();

                operation.Parameters.Add(new OpenApiParameter()
                {
                    Name = "Authorization",
                    In = ParameterLocation.Header,
                    Description = "JWT access token",
                    Required = true
                });
                string unathorized = ((int)HttpStatusCode.Unauthorized).ToString();
                string forbidden = ((int)HttpStatusCode.Forbidden).ToString();
                operation.Responses.Add(unathorized, new Microsoft.OpenApi.Models.OpenApiResponse { Description = "Unauthorized" });
                operation.Responses.Add(forbidden, new Microsoft.OpenApi.Models.OpenApiResponse { Description = "Forbidden" });

                operation.Security = new List<OpenApiSecurityRequirement>();
                var reference = new OpenApiReference
                {
                    Id = FreelancerAuthDefaults.AuthenticationScheme,
                    Type = ReferenceType.SecurityScheme
                };
                var secScheme = new OpenApiSecurityScheme
                {
                    Name = "JWT Auth",
                    Description = "Pass JWT Bearer token to the Authrozation section of header",
                    In = ParameterLocation.Header,
                    Type = SecuritySchemeType.Http,
                    Scheme = FreelancerAuthDefaults.AuthenticationScheme,
                    BearerFormat = "JWT",
                    Reference = reference
                };
                
                //Add JWT bearer type
                operation.Security.Add(new OpenApiSecurityRequirement
                {
                  { secScheme, new List<string>() }
                });
            }
        }
    }
}
