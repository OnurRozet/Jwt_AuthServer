﻿using Jwt.Core.Repository;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace Jwt.Data.Repositories
{
	public class GenericRepository<TEntity> : IGenericRepository<TEntity> where TEntity : class
	{

		private readonly DbContext _context;
		private readonly DbSet<TEntity> _entities;

		public GenericRepository(AppDbContext context)
		{
			_context = context;
			_entities = _context.Set<TEntity>();
		}

		public async Task AddAsync(TEntity entity)
		{
			await _entities.AddAsync(entity);
		}

		public void Delete(TEntity entity)
		{
			_entities.Remove(entity);
		}

		public async Task<IEnumerable<TEntity>> GetAllAsync()
		{
			return await _entities.ToListAsync();
		}

		public async Task<TEntity> GetByIdAsync(int id)
		{
			var entity=await _entities.FindAsync(id);
			if (entity != null)
			{
				_entities.Entry(entity).State = EntityState.Detached;
			}

			return entity;
		}

		public TEntity Update(TEntity entity)
		{
			_entities.Entry(entity).State= EntityState.Modified;
			return entity;
		}

		public IQueryable<TEntity> Where(Expression<Func<TEntity, bool>> expression)
		{
			return  _entities.Where(expression);	
		}
	}
}
