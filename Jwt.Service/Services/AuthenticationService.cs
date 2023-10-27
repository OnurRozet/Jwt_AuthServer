using Jwt.Core.Configuration;
using Jwt.Core.Dtos;
using Jwt.Core.Model;
using Jwt.Core.Repository;
using Jwt.Core.Services;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using SharedLibrary.Dtos;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Jwt.Service.Services
{
	public class AuthenticationService : IAuthenticationService
	{

		private readonly List<Client> _client;
		private readonly UserManager<UserApp> _userManager;
		private readonly ITokenService _tokenService;
		private readonly IUnitOfWork _unitOfWork;
		private readonly IGenericRepository<UserRefreshToken> _userRefreshTokenService;

		public AuthenticationService(IGenericRepository<UserRefreshToken> userRefreshTokenService, IUnitOfWork unitOfWork, ITokenService tokenService, UserManager<UserApp> userManager, IOptions<List<Client>> optionsClient)
		{
			_userRefreshTokenService = userRefreshTokenService;
			_unitOfWork = unitOfWork;
			_tokenService = tokenService;
			_userManager = userManager;
			_client = optionsClient.Value;
		}

		public async Task<Response<TokenDto>> CreateToken(LoginDto loginDtos)
		{
			if (loginDtos == null)
			{
				throw new ArgumentNullException(nameof(loginDtos));
			}

			var user = await _userManager.FindByEmailAsync(loginDtos.Email);

			if (user == null)
			{
				return Response<TokenDto>.Fail(400, true, "Email or Password is wrong");
			}

			if (!await _userManager.CheckPasswordAsync(user,loginDtos.Password))
			{
				return Response<TokenDto>.Fail(400, true, "Email or Password is wrong");
			}


			var token = _tokenService.CreateToken(user);

			var userRefreshToken = await _userRefreshTokenService.Where(x => x.UserId == user.Id).SingleOrDefaultAsync();

			if (userRefreshToken == null)
			{
				await _userRefreshTokenService.AddAsync(new UserRefreshToken
				{
					UserId = user.Id,
					Code = token.RefreshToken,
					Expiration = token.RefreshTokenExpiration
				});
			}
			else
			{
				userRefreshToken.Code = token.RefreshToken;
				userRefreshToken.Expiration = token.RefreshTokenExpiration;

			}

			await _unitOfWork.CommitAsync();

			return Response<TokenDto>.Success(token, 200);
		}

		public Response<ClientTokenDto> CreateTokenByClient(ClientLoginDto clientLoginDto)
		{
			var client = _client.SingleOrDefault(x => x.Id == clientLoginDto.ClientId && x.Secret == clientLoginDto.ClientSecret);

			if (client == null)
			{
				return Response<ClientTokenDto>.Fail(404, true, "ClientId or ClientSecret nor found");
			}

			var token = _tokenService.CreateTokenByClient(client);

			return Response<ClientTokenDto>.Success(token, 200);
		}

		public async Task<Response<TokenDto>> CreateTokenByRefreshToken(string refreshToken)
		{
			var existRefreshToken = await _userRefreshTokenService.Where(x => x.Code == refreshToken).SingleOrDefaultAsync();

			if (existRefreshToken == null)
			{
				return Response<TokenDto>.Fail(404, true, "RefreshToken not found");
			}


			var user = await _userManager.FindByIdAsync(existRefreshToken.UserId);

			if (user == null)
			{
				return Response<TokenDto>.Fail(404, true, "User ıd not found");
			}

			var tokenDto = _tokenService.CreateToken(user);

			existRefreshToken.Code = tokenDto.RefreshToken;
			existRefreshToken.Expiration = tokenDto.RefreshTokenExpiration;

			await _unitOfWork.CommitAsync();

			return Response<TokenDto>.Success(tokenDto, 200);

		}

		public async Task<Response<NoContentDto>> RevokeToken(string refreshToken)
		{
			var existRefreshToken = await _userRefreshTokenService.Where(x => x.Code == refreshToken).SingleOrDefaultAsync();
			if (existRefreshToken == null)
			{
				return Response<NoContentDto>.Fail(404, true, "RefreshToken not found");
			}

			_userRefreshTokenService.Delete(existRefreshToken);

			_unitOfWork.CommitAsync();

			return Response<NoContentDto>.Success(200);
		}
	}
}
