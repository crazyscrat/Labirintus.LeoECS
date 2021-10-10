using Leopotam.Ecs;
using LeopotamGroup.Globals;
using System;
using UnityEngine;

namespace Labirintus.ECS
{
    //система лечения во время боя
    internal class BattleHealthSystem : IEcsRunSystem
    {
        private PlayerData _playerData = default;

        private EcsFilter<BattleHealthComponent, PlayerRef> _playerHealth;
        private EcsFilter<BattleHealthComponent, EnemyComponent> _enemyHealth;

        public void Run()
        {
            //если лечится игрок
            if (!_playerHealth.IsEmpty())
            {
                //если здоровье максимальное
                if (_playerData.HealthPointPlayer == _playerData.MaxHealthPointPlayer) return;

                ref var player = ref _playerHealth.Get1(0); //получаем игрока для анимации
                _playerData.HealthPointPlayer += _playerData.HealthPointRestore; //добавляем здоровье
                //ограничение на максимум
                _playerData.HealthPointPlayer = Math.Min(_playerData.HealthPointPlayer, _playerData.MaxHealthPointPlayer);

                //обновляем информацию на экране боя
                _playerHealth.GetEntity(0).Get<UpdateBattleScreenInfoComponent>();
            }

            //если лечится враг
            if (!_enemyHealth.IsEmpty())
            {
                ref var enemy = ref _enemyHealth.Get2(0);
                //если здоровье максимальное
                if (enemy.enemyData.HealthPoint == enemy.enemyData.MaxHealthPoint) return;

                ref var enemyObject = ref _playerHealth.Get2(0); //получаем игрока для анимации
                enemy.enemyData.HealthPoint += enemy.enemyData.HealthPointRestore; //добавляем здоровье
                //ограничение на максимум
                enemy.enemyData.HealthPoint = Math.Min(enemy.enemyData.HealthPoint, enemy.enemyData.MaxHealthPoint);

                //обновляем информацию на экране боя
                _enemyHealth.GetEntity(0).Get<UpdateBattleScreenInfoComponent>();
            }
        }
    }

    //компонент лечения персонажа (врага) в бою
    public struct BattleHealthComponent
    {

    }
}