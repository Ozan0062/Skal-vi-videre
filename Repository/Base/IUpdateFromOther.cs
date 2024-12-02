namespace Skal_vi_videre.Repository.Base
{
    public interface IUpdateFromOther<T>
    {
        void Update(T tOther);
    }
}