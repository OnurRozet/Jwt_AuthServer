using Jwt.Core.Dtos;
using Jwt.Core.Model;
using Jwt.Core.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Jwt.API.Controllers
{

	[Authorize]
	public class ProductController : CustomBaseController
	{
		private IGenericService<Product, ProductDto> _productService;

		public ProductController(IGenericService<Product, ProductDto> productService)
		{
			_productService = productService;
		}


		[HttpGet]
		public async Task<IActionResult> GetProducts()
		{
			return ActionResultInstance(await _productService.GetAllAsync());
		}

		[HttpPost]
		public async Task<IActionResult> SaveProduct(ProductDto productDto)
		{
			return ActionResultInstance(await _productService.AddAsync(productDto));
		}

		[HttpPut]
		public async Task<IActionResult> UpdateProduct(ProductDto productDto)
		{
			return ActionResultInstance(await _productService.Update(productDto, productDto.Id));
		}

		[HttpDelete("{id}")]
		public async Task<IActionResult> DeleteProduct(int id)
		{
			return ActionResultInstance(await _productService.Delete(id));
		}

	}
}
