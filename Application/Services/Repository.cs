using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;
using Application.Contracts;
using MongoDB.Driver;
using MongoDB.Driver.Linq;

namespace Application.Services
{
    public class Repository<T> : IRepository<T> where T : IMongoCommon, new()
    {
        private readonly IMongoDatabase _database;
        private IMongoCollection<T> _collection => _database.GetCollection<T>(typeof(T).Name);

        public Repository(IMongoDatabase database)
        {
            _database = database;
        }

        public IQueryable<T> Collection => _collection.AsQueryable();
        // public IMongoCollection<T> Collection => _collection;

        public async Task Add(T entity)
        {
            try
            {
                if (entity == null)
                {
                    throw new ArgumentException("entity");
                }
                await _collection.InsertOneAsync(entity);
            }
            catch (Exception dbException)
            {
                throw dbException;
            }
        }

        public T Create()
        {
            var entity = new T();
            return entity;
        }

        public async Task Delete(T entity)
        {
            try
            {
                if (entity == null)
                {
                    throw new ArgumentException("entity");
                }
                await _collection.DeleteOneAsync(w => w.Id.Equals(entity.Id));
            }
            catch (Exception dbException)
            {
                throw dbException;
            }
        }

        public IEnumerable<T> Get(Expression<Func<T, bool>> predicate)
        {
            return _collection.AsQueryable().Where(predicate).AsEnumerable();
        }

        public Task<T> GetByIdAsync(object id)
        {
            var foundById = _collection.Find(x => x.Id == id.ToString()).FirstOrDefaultAsync();
            return foundById;
        }

        // public IQueryable<T> GetQueryable(bool includeDeleted = false) => _collection.AsQueryable();

        public async Task Update(T entity)
        {
            try
            {
                if (entity == null)
                {
                    throw new ArgumentException("entity");
                }
                await _collection.ReplaceOneAsync(w => w.Id.Equals(entity.Id),
                    entity, new ReplaceOptions { IsUpsert = true });
            }
            catch (Exception dbException)
            {
                throw dbException;
            }
        }

        public IQueryable<T> GetQueryable(Expression<Func<T, bool>> predicate = null, bool includeDeleted = false)
        {
            IMongoQueryable<T> query;

            if (includeDeleted)
            {
                query = _collection.AsQueryable();
            }
            else
            {
                query = _collection.AsQueryable().Where(x => !x.IsDelete);
            }

            if (predicate is object)
            {
                query = query.Where(predicate);
            }

            return query;
        }
    }
}