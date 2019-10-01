using NHibernate;
using NHibernate.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;
using zAppDev.DotNet.Framework.Data;
using zAppDev.DotNet.Framework.Data.DAL;
using zAppDev.DotNet.Framework.Utilities;

namespace IdentityDemo.DAL
{
    public class Repository : ICreateRepository
    {
        private readonly ISession _session;
        private readonly IMiniSessionService _sessionManager;

        public Repository(ISession session)
        {
            _sessionManager = new MiniSessionService(null);
            _session = session;
        }

        public Repository(IMiniSessionService manager = null)
        {
            _sessionManager = manager ?? ServiceLocator.Current.GetInstance<IMiniSessionService>();
            // Make sure the session is open
            _sessionManager.OpenSession();
            _session = _sessionManager.Session;
        }

        public List<T> Get<T>(Expression<Func<T, bool>> predicate, bool cacheQuery = true)
        {
            var list = GetAsQueryable(predicate, cacheQuery).ToList();
            return list;
        }

        public List<T> Get<T>(Expression<Func<T, bool>> predicate, int startRowIndex, int pageSize, Dictionary<Expression<Func<T, object>>, bool> orderBy, out int totalRecords, bool cacheQuery = true)
        {
            throw new NotImplementedException();
        }

        public List<T> GetAll<T>(bool cacheQuery = true)
        {
            return Get<T>(null, cacheQuery);
        }

        public List<T> GetAll<T>(int startRowIndex, int pageSize, out int totalRecords, bool cacheQuery = true)
        {
            throw new NotImplementedException();
        }

        public IQueryable<T> GetAsQueryable<T>(Expression<Func<T, bool>> predicate = null, bool cacheQuery = true)
        {
            var query = GetMainQuery<T>();
            if (predicate != null)
            {
                query = query.Where(predicate);
            }
            else
            {
                query = query.WithOptions(options => options.SetCacheable(true));
            }
            return query;
        }

        public T GetById<T>(object id, bool throwIfNotFound = true) where T : class
        {
            var obj = _session.Get<T>(id);
            if (throwIfNotFound && obj == null)
            {
                throw new ApplicationException($"No {typeof(T).Name} was found with key: {id}.");
            }
            return obj;
        }

        public IQueryable<T> GetMainQuery<T>()
        {
            return _session.Query<T>();
        }

        public void Insert<T>(T entity) where T : class
        {
            _session.Save(entity);
            _session.Flush();
        }

        public void Save<T>(T entity) where T : class
        {
            if (entity == null)
            {
                throw new ArgumentNullException(nameof(entity), $"No {typeof(T).Name} was specified.");
            }
            _session.SaveOrUpdate(entity);
            _session.Flush();
        }

        public void SaveWithoutTransaction<T>(T entity) where T : class
        {
            throw new NotImplementedException();
        }
    }
}
