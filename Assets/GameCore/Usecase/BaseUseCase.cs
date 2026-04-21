using Cysharp.Threading.Tasks;
using GameCore.APIGateway;

namespace GameCore.Usecase
{
    public class BaseUseCase<TAPI, TModel> where TAPI : BaseAPIGateway<TModel> where TModel : class
    {
        protected TAPI _apiGateway;
        public virtual async UniTask Update(TModel model) => await _apiGateway.Update(model);
        public virtual TModel GetModel() => _apiGateway.GetModel();
    }
}