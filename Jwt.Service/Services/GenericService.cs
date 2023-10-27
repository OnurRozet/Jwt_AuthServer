using Jwt.Core.Repository;
using Jwt.Core.Services;
using Microsoft.EntityFrameworkCore;
using SharedLibrary.Dtos;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace Jwt.Service.Services
{
	public class GenericService<TEntity, TDto> : IGenericService<TEntity, TDto> where TEntity : class where TDto : class
	{
		private readonly IUnitOfWork _unitOfWork;
		private readonly IGenericRepository<TEntity> _repository;

		public GenericService(Core.Repository.IGenericRepository<TEntity> repository, IUnitOfWork unitOfWork)
		{
			_repository = repository;
			_unitOfWork = unitOfWork;
		}

		public async Task<Response<TDto>> AddAsync(TDto entity)
		{
			var newEntity = ObjectMapper.mapper.Map<TEntity>(entity);
			await _repository.AddAsync(newEntity);
			await _unitOfWork.CommitAsync();

			var newDto = ObjectMapper.mapper.Map<TDto>(newEntity);
			return Response<TDto>.Success(newDto, 200);
		}

		public async Task<Response<NoContentDto>> Delete(int id)
		{
			var isExistEntity = await _repository.GetByIdAsync(id);
			if (isExistEntity == null)
			{
				return Response<NoContentDto>.Fail(404, true, "id not found");
			}

			_repository.Delete(isExistEntity);
			await _unitOfWork.CommitAsync();

			return Response<NoContentDto>.Success(204);
		}

		public async Task<Response<IEnumerable<TDto>>> GetAllAsync()
		{
			var entities = ObjectMapper.mapper.Map<List<TDto>>(await _repository.GetAllAsync());
			return Response<IEnumerable<TDto>>.Success(entities, 200);
		}

		public async Task<Response<TDto>> GetByIdAsync(int id)
		{
			var entity = await _repository.GetByIdAsync(id);
			if (entity == null)
			{
				return Response<TDto>.Fail(404, true, "id not found");
			}

			return Response<TDto>.Success(ObjectMapper.mapper.Map<TDto>(entity), 200);
		}

		public async Task<Response<NoContentDto>> Update(TDto entity,int id)
		{

			var existEntity= await _repository.GetByIdAsync(id);

			if (existEntity==null)
			{
				return Response<NoContentDto>.Fail(404, true, "Entity is not found");
			}

			var updatedEntity=ObjectMapper.mapper.Map<TEntity>(entity);

			_repository.Update(updatedEntity);

			await _unitOfWork.CommitAsync();

			return Response<NoContentDto>.Success(204);


		}

		public async Task<Response<IEnumerable<TDto>>> Where(Expression<Func<TEntity, bool>> expression)
		{
			var list=_repository.Where(expression);

			return Response<IEnumerable<TDto>>.Success(ObjectMapper.mapper.Map<IEnumerable<TDto>>(await  list.ToListAsync()),200);

		}
	}
}
