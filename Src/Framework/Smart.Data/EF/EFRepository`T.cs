using Smart.Data.Extensions;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Data.Entity.Validation;
using System.Linq;
using System.Linq.Expressions;
using Smart.Core.Extensions;

namespace Smart.Data.EF
{
    /// <summary>
    /// ���� EntityFramework �� Repository
    /// </summary>
    public class EFRepository<T> : IEFRepository<T> where T : class
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="dbContext"></param>
        public EFRepository(DbContext dbContext)
        {
            this._dbContext = dbContext;
        }

        #region ˽�г�Ա
        /// <summary>
        /// 
        /// </summary>
        protected DbContext _dbContext;
        private DbSet<T> _entities;
        /// <summary>
        /// 
        /// </summary>
        protected virtual IDbSet<T> Entities
        {
            get
            {
                if (_entities == null)
                    _entities = _dbContext.Set<T>();
                return _entities;
            }
        }

        #endregion

        #region  IRepository<T> �ӿڳ�Ա
        /// <summary>
        /// 
        /// </summary>
        /// <param name="entity"></param>
        /// <returns></returns>
        public int Insert(T entity)
        {
            try
            {
                if (entity == null)
                    throw new ArgumentNullException("entity");

                _dbContext.DetachOther(entity);
                this.Entities.Add(entity);

                return this._dbContext.SaveChanges();
            }
            catch (DbEntityValidationException dbEx)
            {
                throw new Exception(dbEx.GetFullErrorText(), dbEx);
            }
        }

        public int Delete(T entity)
        {
            try
            {
                if (entity == null)
                    throw new ArgumentNullException("entity");
                this._dbContext.Remove(entity);
                return this._dbContext.SaveChanges();
            }
            catch (DbEntityValidationException dbEx)
            {
                throw new Exception(dbEx.GetFullErrorText(), dbEx);
            }
        }

        public int Update(T entity)
        {
            try
            {
                if (entity == null)
                    throw new ArgumentNullException("entity");
                this._dbContext.Update(entity);
                return this._dbContext.SaveChanges();
            }
            catch (DbEntityValidationException dbEx)
            {
                throw new Exception(dbEx.GetFullErrorText(), dbEx);
            }
        }
        public int Execute(string sql, params object[] args)
        {
            var ps = this._dbContext.ProcessParams(sql, args);
            sql = ps.Item1;
            args = ps.Item2.ToArray();
            return this._dbContext.Database.ExecuteSqlCommand(sql, args);
        }
        public T GetById(object id)
        {
            return this.Entities.Find(id);
        }

        public T Get(string predicate, params object[] args)
        {
            var sql = $"select t.* from {typeof(T).Name} t where {predicate}";
            var ps = this._dbContext.ProcessParams(sql, args);
            sql = ps.Item1;
            args = ps.Item2.ToArray();
            return this._dbContext.Database.SqlQuery<T>(sql, args).FirstOrDefault();
        }

        public IEnumerable<T> Query(string sql, params object[] args)
        {
            return this._dbContext.Query<T>(sql, args);
        }

        /// <summary>
        /// ִ�з�ҳ��ѯ,���ط�ҳ���
        /// </summary>
        /// <param name="pageIndex">��ǰҳ�룬��1��ʼ</param>
        /// <param name="pageSize">��ҳ��С</param>
        /// <param name="sqlText">�����ķ�ҳǰ�Ĳ�ѯ���</param>
        /// <param name="parameters">��ѯSQL�Ĳ����б�</param>
        /// <returns>��ҳ���ݶ���</returns>
        public virtual Page<T> GetPage(int pageIndex, int pageSize, string sqlText, params object[] parameters)
        {
            return this._dbContext.QueryPage<T>(pageIndex, pageSize, sqlText, parameters);
        }

        #endregion

        #region  IEFRepository<T> �ӿڳ�Ա

        public IQueryable<T> Table
        {
            get
            {
                return this.Entities;
            }
        }

        public virtual IQueryable<T> TableNoTracking
        {
            get
            {
                return this.Entities.AsNoTracking();
            }
        }

        public T Get(Expression<Func<T, bool>> predicate)
        {
            return this.Entities.FirstOrDefault(predicate);
        }

        public IEnumerable<T> Query(Expression<Func<T, bool>> predicate)
        {
            return this.Entities.Where(predicate);
        }

        public IEnumerable<T> Query(Expression<Func<T, bool>> predicate, Action<Orderable<T>> order)
        {
            var orderable = new Orderable<T>(Query(predicate).AsQueryable<T>());
            order(orderable);
            return orderable.Queryable;
        }

        public IEnumerable<T> Query(Expression<Func<T, bool>> predicate, Action<Orderable<T>> order, int skip, int count)
        {
            var orderable = new Orderable<T>(Query(predicate).AsQueryable<T>());
            order(orderable);
            return orderable.Queryable.Skip(skip).Take(count);
        }

        public int Count(Expression<Func<T, bool>> predicate)
        {
            return this.Entities.Count(predicate);
        }

        public bool Exists(Expression<Func<T, bool>> predicate)
        {
            return this.Entities.Any(predicate);
        }

        #endregion

    }

    public class EFRepository<TEntity, TDbCoutext> : EFRepository<TEntity>, IEFRepository<TEntity, TDbCoutext> where TEntity : class
    {
        public EFRepository(TDbCoutext dbContext) : base(dbContext as DbContext)
        {
        }
    }
}