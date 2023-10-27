using Jwt.Core.Dtos;
using Jwt.Core.Services;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Jwt.API.Controllers
{
	
	public class AuthController : CustomBaseController
	{
		private readonly IAuthenticationService _authenticationService;

		public AuthController(IAuthenticationService authenticationService)
		{
			_authenticationService = authenticationService;
		}


		[HttpPost]

		public async Task<IActionResult> CreateToken(LoginDto loginDto)
		{

			var result=await _authenticationService.CreateToken(loginDto);

			return ActionResultInstance(result);
		}


		[HttpPost]

		public  IActionResult CreateTokenByClient(ClientLoginDto clientLoginDto)
		{
			var result= _authenticationService.CreateTokenByClient(clientLoginDto);

			return ActionResultInstance(result);
		}



		[HttpPost]

		public async Task<IActionResult> RevokeRefreshToken(RefreshTokenDto refreshTokenDto)
		{
			var result = await _authenticationService.RevokeToken(refreshTokenDto.Token);

			return ActionResultInstance(result);
		}


		[HttpPost]

		public async Task<IActionResult> CreateTokenByRefreshToken(RefreshTokenDto refreshTokenDto)
		{
			var result = await _authenticationService.CreateTokenByRefreshToken(refreshTokenDto.Token);
			return ActionResultInstance(result);
		}

















	}
}
