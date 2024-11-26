namespace Skal_vi_videre.Repository.Base
{
    public interface IRepository<T> where T : class
    {
        List<T> GetAll();

        T Get(int id);

        int Create(T t);

        bool Update(int id, T t);

        bool Delete(int id);
    }
}
