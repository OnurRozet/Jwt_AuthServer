using Jwt.Core.Configuration;
using Jwt.Core.Dtos;
using Jwt.Core.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Jwt.Core.Services
{
	public interface ITokenService
	{
		TokenDto CreateToken(UserApp userApp);
		ClientTokenDto CreateTokenByClient(Client client);
	}
}
