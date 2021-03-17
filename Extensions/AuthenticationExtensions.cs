using BistHub.Api.Filters;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.OpenApi.Any;
using Microsoft.OpenApi.Interfaces;
using Microsoft.OpenApi.Models;
using Swashbuckle.AspNetCore.SwaggerGen;
using Swashbuckle.AspNetCore.SwaggerUI;
using System;
using System.Collections.Generic;

namespace BistHub.Api.Extensions
{
    public static class AuthenticationExtensions
    {
        public static void AddFirebaseAuthentication(this IServiceCollection services, string appName)
        {
            services
                    .AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
                    .AddJwtBearer(options =>
                    {
                        options.Authority = $"https://securetoken.google.com/{appName}";
                        options.TokenValidationParameters = new Microsoft.IdentityModel.Tokens.TokenValidationParameters
                        {
                            ValidateIssuer = true,
                            ValidIssuer = $"https://securetoken.google.com/{appName}",
                            ValidateAudience = true,
                            ValidAudience = appName,
                            ValidateLifetime = true
                        };
                    });
            //services.AddAuthorization();
        }

        public static void AddOAuthUi(this SwaggerUIOptions c, string clientId)
        {
            c.OAuthClientId(clientId);
            c.OAuthAppName("bisthub-51469");
            c.OAuthUsePkce();
        }

        public static void AddSwaggerSecurityDefinition(this SwaggerGenOptions c, IConfiguration configuration)
        {
            c.AddSecurityDefinition("oauth2", new OpenApiSecurityScheme
            {
                Type = SecuritySchemeType.OAuth2,
                Flows = new OpenApiOAuthFlows
                {
                    AuthorizationCode = new OpenApiOAuthFlow
                    {
                        AuthorizationUrl = new Uri("https://accounts.google.com/o/oauth2/auth"),
                        TokenUrl = new Uri("https://oauth2.googleapis.com/token"),
                    }
                }
            });
            c.OperationFilter<AuthorizeCheckOperationFilter>();
        }

        public static void AddSwaggerSecurityDefinition(this SwaggerGenOptions c)
        {
            c.AddSecurityDefinition("oauth2", new OpenApiSecurityScheme
            {

                Type = SecuritySchemeType.OAuth2,

                Flows = new OpenApiOAuthFlows
                {
                    Password = new OpenApiOAuthFlow
                    {
                        TokenUrl = new Uri("/api/v1/auth", UriKind.Relative),
                        Extensions = new Dictionary<string, IOpenApiExtension>
                                {
                                    { "returnSecureToken", new OpenApiBoolean(true) },
                                },

                    }

                }
            });
            c.OperationFilter<AuthorizeCheckOperationFilter>();
        }
    }
}
