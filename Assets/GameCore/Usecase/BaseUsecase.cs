using Cysharp.Threading.Tasks;
using GameCore.APIGateway;

namespace GameCore.Usecase
{
    public class BaseUsecase<TAPI, TModel> where TAPI : BaseAPIGateway<TModel> where TModel : class
    {
        protected TAPI _apiGateway;
        protected virtual async UniTask Update(TModel model) => await _apiGateway.Update(model);
        public TModel GetModel() => _apiGateway.GetModel();
    }
}