using Jwt.Core.Dtos;
using Jwt.Core.Model;
using Jwt.Core.Services;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc.Filters;
using SharedLibrary.Dtos;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Jwt.Service.Services
{
	public class UserService : IUserService
	{

		private readonly UserManager<UserApp> _userManager;

		public UserService(UserManager<UserApp> userManager)
		{
			_userManager = userManager;
		}

		public async Task<Response<UserAppDto>> CreateUserAsync(CreateUserDto createUserDto)
		{
			var user = new UserApp
			{
				Email = createUserDto.Email,
				UserName = createUserDto.UserName,
			};

			var result = await _userManager.CreateAsync(user);

			if (!result.Succeeded)
			{
				var errors = result.Errors.Select(x => x.Description).ToList();

				return Response<UserAppDto>.Fail(new ErrorDto(errors,true), 400);
			}

			return Response<UserAppDto>.Success(ObjectMapper.mapper.Map<UserAppDto>(user), 200);
		}

		public async Task<Response<UserAppDto>> GetUserByNameAsync(string userName)
		{
			var user = await _userManager.FindByNameAsync(userName);

			if (user==null)
			{
				return Response<UserAppDto>.Fail(404, true, "Username nor found");
			}

			return Response<UserAppDto>.Success(ObjectMapper.mapper.Map<UserAppDto>(user),200);

		}
	}
}
