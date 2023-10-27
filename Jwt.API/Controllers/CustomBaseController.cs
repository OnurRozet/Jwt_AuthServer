using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using SharedLibrary.Dtos;

namespace Jwt.API.Controllers
{
	[Route("api/[controller]/[action]")]
	[ApiController]
	public class CustomBaseController : ControllerBase
	{
		[NonAction]

		public IActionResult ActionResultInstance<T>(Response<T> response)where T : class 
		{
			return new ObjectResult(response)
			{
				StatusCode = response.StatusCode
			};

		
		}
	}
}
