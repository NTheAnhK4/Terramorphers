using Cysharp.Threading.Tasks;

namespace GameCore.Domain.Shared
{
    public interface IMasterGateway<in TKey, TModel>
    {
        UniTask<TModel> GetModel(TKey key);
        UniTask Update(TModel model);
    }
}