namespace GameCore.Domain.Shared
{
    public interface IMasterRepository<T>
    {
        T Get();
    }
}