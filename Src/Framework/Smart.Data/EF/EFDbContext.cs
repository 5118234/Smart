using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Threading.Tasks;

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
            ////this.Configuration.AutoDetectChangesEnabled = false;//�ر��Զ����ٶ�������Ա仯
            //this.Configuration.LazyLoadingEnabled = false; //�ر��ӳټ���
            //this.Configuration.ProxyCreationEnabled = false; //�رմ�����
            ////this.Configuration.ValidateOnSaveEnabled = false; //�رձ���ʱ��ʵ����֤
            //this.Configuration.UseDatabaseNullSemantics = true; //�ر����ݿ�null�Ƚ���Ϊ
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
        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public override int SaveChanges()
        {
            try
            {
                return base.SaveChanges();
            }
            catch (System.Data.Entity.Infrastructure.DbUpdateConcurrencyException cex)
            {
                return 0;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        #endregion
    }
}