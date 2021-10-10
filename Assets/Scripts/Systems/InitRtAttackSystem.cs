using Leopotam.Ecs;
using UnityEngine;

namespace Labirintus.ECS
{
    //система реалтайм боя
    internal class InitRtAttackSystem : IEcsRunSystem
    {
        private readonly EcsWorld _world = null;
        private GameData _gameData = default;

        private EcsFilter<Weapon>.Exclude<Reload> _filter = null; //оружие не перезаряжается

        public void Run()
        {
            if (_filter.IsEmpty()) return;

            //если есть враги в поле зрения
            if (_gameData.hashViewEnemies.Count > 0)
            {
                foreach (var item in _gameData.hashViewEnemies)
                {
                    //Debug.Log(item.position);
                    //Атака
                    _filter.GetEntity(0).Get<Reload>(); //добавляем перезарядку
                    ref var shoot = ref _filter.GetEntity(0).Get<Shoot>(); //добавляем игроку компонент выстрела
                    shoot.targetPosition = item.position; //передаем позицию врага
                }
            }
        }
    }

}