﻿using Jwt.Core.Dtos;
using Jwt.Core.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Jwt.API.Controllers
{
	
	public class UserController : CustomBaseController
	{
		private readonly IUserService _userService;

		public UserController(IUserService userService)
		{
			_userService = userService;
		}



		[HttpPost]

		public async Task<IActionResult> CreateUser(CreateUserDto createUserDto)
		{
			var result = await _userService.CreateUserAsync(createUserDto);

			return ActionResultInstance(result);
		}

		[Authorize]
		[HttpGet]

		public async Task<IActionResult> GetUser()
		{
			return ActionResultInstance(await _userService.GetUserByNameAsync(HttpContext.User.Identity.Name));
		}


	}
}
