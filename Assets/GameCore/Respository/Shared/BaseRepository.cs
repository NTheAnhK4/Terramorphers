using System;

using Cysharp.Threading.Tasks;
using GameCore.Domain.Shared;

using UnityEngine.AddressableAssets;

using VContainer.Unity;

namespace GameCore.Respository.Shared
{
    public abstract class BaseRepository<TType, TData, TDatabase> : IDisposable, IInitializable,
        IMasterRepository<TDatabase> where TDatabase : IMasterDatabase<TType, TData>
    {
        private TDatabase _database;

        public abstract string AddressPath { get; }

        public void Initialize()
        {
            InitializeAsync().Forget();
        }
       

        public void Dispose()
        {
            if (_database != null) Addressables.Release(_database);
        }

        public async UniTask InitializeAsync()
        {
            _database = await Addressables.LoadAssetAsync<TDatabase>(AddressPath);
        }

        public TDatabase Get()
        {
            
            return _database;
        }
    }
}