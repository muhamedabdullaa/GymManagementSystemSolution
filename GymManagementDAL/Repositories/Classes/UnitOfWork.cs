using GymManagementDAL.Data.Context;
using GymManagementDAL.Entities;
using GymManagementDAL.Repositories.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GymManagementDAL.Repositories.Classes
{
	public class UnitOfWork : IUnitOfWork
	{
		private readonly Dictionary<Type, object> _repositories = new();
		private readonly GymDbContext _dbContext;
		public UnitOfWork(GymDbContext dbContext , ISessionRepository sessionRepository)
		{
			_dbContext = dbContext;
			SessionRepository = sessionRepository;
		}

		public ISessionRepository SessionRepository { get; }

		public IGenericRepository<TEntity> GetRepository<TEntity>() where TEntity : BaseEntity, new()
		{
			var EntityType = typeof(TEntity);
			if (_repositories.TryGetValue(EntityType, out var repo))
				return (IGenericRepository<TEntity>) repo;
			var NewRepo = new GenericRepository<TEntity>(_dbContext);
			_repositories.Add(EntityType, NewRepo);
			_repositories[EntityType] = NewRepo;
			return NewRepo;
		}

		public int SaveChanges()
		{
			return _dbContext.SaveChanges();
		}
	}
}
