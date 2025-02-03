using CourseManagement.Infrastructure.DbModel;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CourseManagement.Infrastructure
{
    public abstract class BaseDataRepository
    {
        protected CourseManagementDbContext dbModel;
        public BaseDataRepository(CourseManagementDbContext model)
        {
            dbModel = model;
        }

        protected T FindEntity<T>(int primaryKey) where T : class
        {
            try
            {
                return dbModel.Set<T>().Find(primaryKey);
            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex.ToString());
                throw;
            }
        }

        protected void AddUpdateEntity<T>(T entity, bool keepDettached = true) where T : class
        {
            try
            {
                if (dbModel.Entry<T>(entity).IsKeySet)
                    dbModel.Update<T>(entity);
                else
                    dbModel.Add<T>(entity);

                dbModel.SaveChanges();

                if (keepDettached)
                {
                    dbModel.Entry<T>(entity).State = EntityState.Detached;
                }

            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex.ToString());
                throw;
            }
        }

        protected void RemoveEntity<T>(int id) where T : class
        {
            try
            {
                var entity = dbModel.Set<T>().Find(id);
                dbModel.Remove<T>(entity);
                dbModel.SaveChanges();
            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex.ToString());
                throw;
            }
        }
        protected IList<T> GetListData<T>() where T : class
        {
            try
            {
                return dbModel.Set<T>().ToList();
            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex.ToString());
                throw;
            }
        }
        protected IList<T> GetListData<T>(string interpolatedStoredProc, params object[] parameters) where T : class
        {
            try
            {
                return dbModel.Set<T>().FromSqlRaw(interpolatedStoredProc, parameters).AsQueryable().ToList();
            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex.ToString());
                throw;
            }
        }
    }

}
