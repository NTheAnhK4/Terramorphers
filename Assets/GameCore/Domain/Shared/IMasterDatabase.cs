namespace GameCore.Domain.Shared
{
    public interface IMasterDatabase<in TType, out TData>
    {
        TData GetByType(TType type);
    }
}