using Jwt.Core.Dtos;
using SharedLibrary.Dtos;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Jwt.Core.Services
{
	public interface IAuthenticationService
	{
		Task<Response<TokenDto>> CreateToken(LoginDto loginDtos);

		Task<Response<TokenDto>> CreateTokenByRefreshToken(string refreshToken);

		Task<Response<NoContentDto>> RevokeToken(string refreshToken);

		Response<ClientTokenDto> CreateTokenByClient(ClientLoginDto clientLoginDto);
	}
}
