﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace Jwt.Core.Repository
{
	public interface IGenericRepository<T> where T : class
	{
		Task<T> GetByIdAsync(int id);

		Task<IEnumerable<T>> GetAllAsync();

		IQueryable<T> Where(Expression<Func<T,bool>> expression);

		Task AddAsync(T entity);
		T Update(T entity);
		void Delete(T entity);



	}
}
