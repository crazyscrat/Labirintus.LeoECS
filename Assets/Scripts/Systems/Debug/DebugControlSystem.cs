using Leopotam.Ecs;
using UnityEngine;

namespace Labirintus.ECS
{
    internal sealed class DebugControlSystem : IEcsInitSystem, IEcsRunSystem
    {
        private EcsWorld _world = default;
        private PlayerData _playerData;
        private UIDebug _uiDebug;

        public void Init()
        {
            _uiDebug.btnReset.onClick.AddListener(() =>
            {
                OnClickBtnReset();
            });

            _uiDebug.btnNewMaze.onClick.AddListener(() =>
            {
                _world.NewEntity().Get<MazeDispoze>();
            });
        }

        public void Run()
        {
            
        }

        void OnClickBtnReset()
        {
            //Debug.Log("RESET");
            _playerData.PowerBlueAmountPlayer = 0;
            _playerData.PowerGreenAmountPlayer = 0;
            _playerData.PowerPinkAmountPlayer = 0;
            _world.NewEntity().Get<UpdateScreenInfoComponent>();
        }
    }
}