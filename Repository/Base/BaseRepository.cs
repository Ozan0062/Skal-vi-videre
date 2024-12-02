using Microsoft.EntityFrameworkCore;

namespace Skal_vi_videre.Repository.Base
{
    public class BaseRepository<T> : IRepository<T> where T : class, IHasId, IUpdateFromOther<T>
    {
        /// <summary>
        /// Saves newItem To the database
        /// </summary>
        /// <param name="newItem"></param>
        /// <returns>The number of state entries written to the database</returns>
        public virtual int Create(T newItem)
        {
            using DbContext context = CreateDbContext();

            context.Set<T>().Add(newItem);
            context.SaveChanges();

            return newItem.Id;
        }

        public List<T> GetAll()
        {
            List<T> list = new List<T>();
            // using sørger for at dispose når vi er færdige med metoden, hvilket er vigtigt fordi objektet rækker ud fra programmet selv.
            using (DbContext context = CreateDbContext())
            {
                list = GetAllWithIncludes(context).ToList();
            }

            return list;
        }

        public T? Get(int id)
        {
            using DbContext context = CreateDbContext();
            T? result = GetAllWithIncludes(context).FirstOrDefault(t => t.Id == id);

            return result;
        }

        protected DbContext CreateDbContext()
        {
            DbContext result = new DBContext();

            return result;
        }

        protected virtual IQueryable<T> GetAllWithIncludes(DbContext context)
        {
            return context.Set<T>();
        }

        public virtual bool Update(int id, T t)
        {
            using DbContext context = CreateDbContext();
            T? existing = context.Set<T>().Find(id);

            if (existing == null)
            {
                return false;
            }

            existing.Update(t);

            return (context.SaveChanges() > 0);
        }

        public virtual bool Delete(int id)
        {
            using DbContext context = CreateDbContext();

            try
            {
                T tToDelete = context.Set<T>().Find(id);
                if (tToDelete == null) { return false; }
                context.Remove(tToDelete);
                return context.SaveChanges() > 0;
            }
            catch (Exception ex)
            {
                throw new Exception("kunne ikke slette objektet" + ex.Message);
            }
        }
    }
}