using System.Data.Entity;

namespace Smart.Data.EF
{
    /// <summary>
    /// DbContext
    /// </summary>
    public class EFDbContext : DbContext
    {
        #region ���캯��

        /// <summary>
        /// 
        /// </summary>
        public EFDbContext() : this("name=DefaultConnection")
        {
        }

        /// <summary>
        /// ���캯��
        /// </summary>
        /// <param name="nameOrConnectionString">ʾ����name=DefaultConnection</param>
        public EFDbContext(string nameOrConnectionString)
            : base(nameOrConnectionString)
        {
            //((IObjectContextAdapter) this).ObjectContext.ContextOptions.LazyLoadingEnabled = true;
        }

        #endregion
    }
}