using Jwt.Core.Configuration;
using Jwt.Core.Dtos;
using Jwt.Core.Model;
using Jwt.Core.Services;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using SharedLibrary.Configuration;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Security.Cryptography;

namespace Jwt.Service.Services
{
	public class TokenService : ITokenService
	{

		private readonly UserManager<UserApp> _userManager;

		private readonly CustomTokenOptions _customTokenOptions;
		public TokenService(UserManager<UserApp> userManager, IOptions<CustomTokenOptions> options)
		{
			_userManager = userManager;
			_customTokenOptions = options.Value;
		}



		private string CreateRefreshToken()
		{
			var numberByte = new Byte[32];

			using var rnd = RandomNumberGenerator.Create();

			rnd.GetBytes(numberByte);

			return Convert.ToBase64String(numberByte);

		}


		//üyelik sistemi gerektiğinde kullanılır

		private IEnumerable<Claim> GetClaims(UserApp userApp, List<string> audiences)
		{
			var userList = new List<Claim>
			{
			 new Claim(ClaimTypes.NameIdentifier, userApp.Id),
			 new Claim(JwtRegisteredClaimNames.Email, userApp.Email),
			 new Claim(ClaimTypes.Name, userApp.UserName),
			 new Claim(JwtRegisteredClaimNames.Jti,Guid.NewGuid().ToString())
			};


			userList.AddRange(audiences.Select(x => new Claim(JwtRegisteredClaimNames.Aud, x)));

			return userList;

		}




		//Üyelik sistemi gerekmediği duurmlarda kullanılır
		private IEnumerable<Claim> GetClaimsByClient(Client client)
		{
			var claims = new List<Claim>();
			claims.AddRange(client.Audiences.Select(x => new Claim(JwtRegisteredClaimNames.Aud, x)));


			new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString());
			new Claim(JwtRegisteredClaimNames.Sub, client.Id.ToString());

			return claims;
		}



		public TokenDto CreateToken(UserApp userApp)
		{
			var accessTokenExpiration = DateTime.Now.AddMinutes(_customTokenOptions.AccessTokenExpiration);

			var securityKey = SignService.GetSymetricSecurityKey(_customTokenOptions.SecurityKey);

			var refreshTokenExpiration = DateTime.Now.AddMinutes(_customTokenOptions.RefreshTokenExpiration);

			SigningCredentials signingCredentials = new SigningCredentials(securityKey, SecurityAlgorithms.HmacSha256Signature);

			JwtSecurityToken jwtSecurityToken = new JwtSecurityToken(
			issuer: _customTokenOptions.Issuer,
			expires: accessTokenExpiration,
			notBefore: DateTime.Now,
			claims: GetClaims(userApp, _customTokenOptions.Audience)
				);

			var handler = new JwtSecurityTokenHandler();

			var token = handler.WriteToken(jwtSecurityToken);

			var tokenDto = new TokenDto
			{
				AccessToken = token,
				RefreshToken = CreateRefreshToken(),
				AccessTokenExpiration = accessTokenExpiration,
				RefreshTokenExpiration = refreshTokenExpiration,
			};

			return tokenDto;
		}

		public ClientTokenDto CreateTokenByClient(Client client)
		{
			var accessTokenExpiration = DateTime.Now.AddMinutes(_customTokenOptions.AccessTokenExpiration);

			var securityKey = SignService.GetSymetricSecurityKey(_customTokenOptions.SecurityKey);



			SigningCredentials signingCredentials = new SigningCredentials(securityKey, SecurityAlgorithms.HmacSha256Signature);

			JwtSecurityToken jwtSecurityToken = new JwtSecurityToken(
			issuer: _customTokenOptions.Issuer,
			expires: accessTokenExpiration,
			notBefore: DateTime.Now,
			claims: GetClaimsByClient(client)
				);

			var handler = new JwtSecurityTokenHandler();

			var token = handler.WriteToken(jwtSecurityToken);

			var tokenDto = new ClientTokenDto
			{
				AccessToken = token,

				AccessTokenExpiration = accessTokenExpiration,

			};

			return tokenDto;
		}
	}
}
