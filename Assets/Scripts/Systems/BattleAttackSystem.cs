using Leopotam.Ecs;
using LeopotamGroup.Globals;
using UnityEngine;

namespace Labirintus.ECS
{
    //система атаки в бою
    internal class BattleAttackSystem : IEcsRunSystem
    {
        private EcsWorld _world;
        private PlayerData _playerData = default;

        private EcsFilter<PlayerRef, BattleAttackComponent> _filterPlayer;
        private EcsFilter<EnemyComponent> _filterEnemy;

        public void Run()
        {
            //если игрок атакует
            if (!_filterPlayer.IsEmpty())
            {
                //наносим урон врагу
                //Debug.Log(Service<GameData>.Get().enemyData.HealthPoint);
                //Debug.Log(_playerData.Damage);

                if (Service<GameData>.Get().enemyData.DefenseEnemy > 0)
                {
                    var difference = Service<GameData>.Get().enemyData.DefenseEnemy - _playerData.Damage;
                    if (difference >= 0) //если защиты хватает
                    {
                        Service<GameData>.Get().enemyData.DefenseEnemy -= _playerData.Damage;
                    }
                    else //иначе отнимаем и жизни
                    {
                        Service<GameData>.Get().enemyData.DefenseEnemy = 0;
                        Service<GameData>.Get().enemyData.HealthPoint += difference;
                    }
                }
                else
                {
                    Service<GameData>.Get().enemyData.HealthPoint -= _playerData.Damage;
                }
                
                //Debug.Log(_filterEnemy.IsEmpty());
                //Debug.Log(_filterEnemy.Get1(0).enemyView);

                //если враг умер
                if (Service<GameData>.Get().enemyData.HealthPoint <= 0)
                {
                    _filterEnemy.Get1(0).enemyView.transform.gameObject.Deactivate();
                    _filterEnemy.GetEntity(0).Del<EnemyComponent>();
                    _world.NewEntity().Get<EndBattle>(); //заканчиваем бой
                }
                //обновляем экран
                _world.NewEntity().Get<UpdateBattleScreenInfoComponent>();
            }
        }
    }

    internal struct BattleAttackComponent
    {
    }

    internal struct StepComponent
    {
    }
}