using System;
using System.Collections.Generic;
using Cysharp.Threading.Tasks;
using R3;
using UnityEngine;
using ZBase.UnityScreenNavigator.Core.Controls;
using ZBase.UnityScreenNavigator.Core.Sheets;

namespace WEngine.GUIComponents.TabGroup
{
    public sealed class TabGroup : MonoBehaviour
    {
        [SerializeField] private SheetContainer sheetContainer;
        private readonly Dictionary<int, int> _indexToSheetIdMap = new();
        private readonly ReactiveCommand<TabLoadedEvent> _onTabLoadedSubject = new();

        public bool IsInitializing { get; private set; }
        public bool IsInitialized { get; private set; }

        public ReactiveCommand<TabLoadedEvent> OnTabLoaded => _onTabLoadedSubject;
        public bool IsInTransition => sheetContainer.IsInTransition;

        private void Awake()
        {
            _onTabLoadedSubject.AddTo(this);
            _onTabLoadedSubject.Subscribe(x => { x.Sheet.AddLifecycleEvent(); });
        }

        public async UniTask InitializeAsync(List<TabSource> sources)
        {
            if (IsInitialized)
                throw new InvalidOperationException($"{nameof(TabGroup)} is Already initialized.");

            if (IsInitializing)
                throw new InvalidOperationException($"{nameof(TabGroup)} is initializing.");

            IsInitializing = true;

            var registerTasks = new List<UniTask>();
            int initialIndex = 0;
            
            for (var i = 0; i < sources.Count; i++)
            {
                // Register sheets.
                var source = sources[i];
                var index = i;
                if (source.IsInitial)
                    initialIndex = index;
                
                var registerTask = sheetContainer.RegisterAsync(new SheetOptions(
                    resourcePath: source.SheetResourceKey,
                    onLoaded: (sheetId, sheet, args) =>
                    {
                        _indexToSheetIdMap.Add(index, sheetId);
                        _onTabLoadedSubject.Execute(new TabLoadedEvent(index, sheetId, sheet));
                        if (sheet is ITabContent tabContent)
                            tabContent.TabIndex = index;
                    }
                ));
                registerTasks.Add(registerTask);

                // Setup buttons.
                source.SwitchTabCommand
                      .Subscribe(_ =>
                      {
                          if (sheetContainer.IsInTransition)
                              return;

                          if (sheetContainer.ActiveSheetId == _indexToSheetIdMap[index])
                              return;

                          SetActiveTabAsync(index, true).Forget();
                      })
                      .AddTo(this);
            }

            await UniTask.WhenAll(registerTasks);

            // Set initial sheet.
            
            await SetActiveTabAsync(initialIndex, false);

            IsInitialized = true;
            IsInitializing = false;
        }

        public int GetSheetIdFromIndex(int index)
        {
            return _indexToSheetIdMap[index];
        }

        public UniTask SetActiveTabAsync(int index, bool playAnimation)
        {
            var sheetId = GetSheetIdFromIndex(index);
            return sheetContainer.ShowAsync(sheetId, playAnimation);
        }

        public readonly struct TabLoadedEvent
        {
            public TabLoadedEvent(int index, int sheetId, Sheet sheet)
            {
                Index = index;
                SheetId = sheetId;
                Sheet = sheet;
            }

            public int Index { get; }
            public int SheetId { get; }
            public Sheet Sheet { get; }
        }

        [Serializable]
        public sealed class TabSource
        {
            public Observable<Unit> SwitchTabCommand;
            public string SheetResourceKey;
            public bool IsInitial;
        }
    }
}