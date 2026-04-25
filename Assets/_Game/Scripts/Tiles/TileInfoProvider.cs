
using R3;
using UnityEngine;

namespace Terramorphers
{
    public class TileInfoProvider : MonoBehaviour, IInfoProvider
    {
        public ReactiveProperty<bool> IsShowInfo { get; } = new ReactiveProperty<bool>();
        private DisposableBag _bag;
        private void Awake()
        {
            IsShowInfo.Skip(1).Subscribe(ShowInfo).AddTo(ref _bag);
        }

        private void OnDestroy()
        {
            _bag.Dispose();
        }

        public void ShowInfo(bool isShow)
        {
            Debug.Log($"[Test] show info with : {isShow}");
        }
    }
}