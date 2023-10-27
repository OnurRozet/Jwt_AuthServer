using Jwt.Service.Services;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.IdentityModel.Tokens;
using SharedLibrary.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SharedLibrary.Extension
{
	public static class CustomTokenAuth
	{
		public static void AddCustomTokenAuth(this IServiceCollection services,CustomTokenOptions tokens)
		{
			services.AddAuthentication(options =>
			{
				options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
				options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;

			}).AddJwtBearer(JwtBearerDefaults.AuthenticationScheme, opt =>
			{

				opt.TokenValidationParameters = new TokenValidationParameters()
				{
					ValidIssuer = tokens.Issuer,
					ValidAudience = tokens.Audience[0],
					IssuerSigningKey = SignService.GetSymetricSecurityKey(tokens.SecurityKey),
					ValidateIssuerSigningKey = true,
					ValidateAudience = true,
					ValidateIssuer = true,
					ValidateLifetime = true,
					ClockSkew = TimeSpan.Zero,
				};
			});

		}
	}
}
