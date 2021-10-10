using Leopotam.Ecs;
using System;

namespace Labirintus.ECS
{
    //система защиты в бою
    internal class BattleDefenseSystem : IEcsRunSystem
    {
        private PlayerData _playerData;

        private EcsFilter<BattleDefenseComponent, PlayerRef> _playerDefense;
        private EcsFilter<BattleDefenseComponent, EnemyComponent> _enemyDefense;

        public void Run()
        {
            //если защищается игрок
            if (!_playerDefense.IsEmpty())
            {
                //если защита максимальная
                if (_playerData.DefensePlayer == _playerData.MaxDefensePlayer) return;

                ref var player = ref _playerDefense.Get1(0); //получаем игрока для анимации
                _playerData.DefensePlayer += _playerData.DefensePointRestore; //добавляем защиту
                //ограничение на максимум
                _playerData.DefensePlayer = Math.Min(_playerData.DefensePlayer, _playerData.MaxDefensePlayer);

                //обновляем информацию на экране боя
                _playerDefense.GetEntity(0).Get<UpdateBattleScreenInfoComponent>();
            }

            //если защищается враг
            if (!_enemyDefense.IsEmpty())
            {
                ref var enemy = ref _enemyDefense.Get2(0);
                //если защита максимальная
                if (enemy.enemyData.DefenseEnemy == enemy.enemyData.MaxDefenseEnemy) return;

                ref var enemyObject = ref _playerDefense.Get2(0); //получаем игрока для анимации
                enemy.enemyData.HealthPoint += enemy.enemyData.DefensePointRestore; //добавляем защиту
                //ограничение на максимум
                enemy.enemyData.DefenseEnemy = Math.Min(enemy.enemyData.DefenseEnemy, enemy.enemyData.MaxDefenseEnemy);

                //обновляем информацию на экране боя
                _enemyDefense.GetEntity(0).Get<UpdateBattleScreenInfoComponent>();
            }
        }
    }

    //компонент защиты персонажа (врага) в бою
    internal struct BattleDefenseComponent
    {
    }
}