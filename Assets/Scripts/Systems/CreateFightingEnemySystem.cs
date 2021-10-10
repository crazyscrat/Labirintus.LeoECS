using Leopotam.Ecs;
using LeopotamGroup.Globals;
using System;
using UnityEngine;

namespace Labirintus.ECS
{
    //инициализируем врага для боя
    internal class CreateFightingEnemySystem : IEcsRunSystem
    {
        private EcsWorld _world = default;
        private UIData _uiData = default;
        private SceneData _sceneData = default;

        private EcsFilter<StartBattleComponent> _enemy;

        public void Run()
        {
            if (_enemy.IsEmpty()) return;

            //находим объект врага
            var enemy = GetEnemyGameObject();
            if (enemy != null)
            {
                ref var _enemyComponent = ref _world.NewEntity().Get<EnemyComponent>();
                _enemyComponent.enemyView = enemy;
                //Debug.Log(_enemyComponent.enemyView);
                _enemyComponent.enemyData = GameObject.Instantiate(enemy.GetComponent<EnemyView>().enemyData);

                //если панели нет, то создаем
                if (Service<GameData>.Get().panelBattleEnemy == null)
                {
                    Service<GameData>.Get().panelBattleEnemy = GameObject.Instantiate(_uiData.prefabPanelBattleEnemy, _uiData.panelBattle.transform);
                }
                else
                {
                    Service<GameData>.Get().panelBattleEnemy.gameObject.Activate();
                }
                Service<GameData>.Get().panelBattleEnemy.imageCharacter.sprite = _enemyComponent.enemyData.Sprite; //устанавливаем спрайт
                Service<GameData>.Get().panelBattleEnemy.hpBar.fillAmount =
                    (float)_enemyComponent.enemyData.HealthPoint / _enemyComponent.enemyData.MaxHealthPoint; //устанавливаем жизни
                Service<GameData>.Get().panelBattleEnemy.defenseBar.fillAmount =
                    (float)_enemyComponent.enemyData.DefenseEnemy / _enemyComponent.enemyData.MaxDefenseEnemy; //устанавливаем защиту
                Service<GameData>.Get().enemyData = _enemyComponent.enemyData;
            }
        }

        //получаем врагаи из спсика по координатам
        private EnemyView GetEnemyGameObject()
        {
            //получаем координаты врага
            Vector2Int position = Service<GameData>.Get().currentPlayerPosition + Service<GameData>.Get().moveDirection;
            Service<GameData>.Get().enemyPosition = position;
            EnemyView enemy = null;

            //перебираем список врагов
            for (int i = 0; i < _sceneData.mazeConstructor._enemies.Count; i++)
            {
                if ((_sceneData.mazeConstructor._enemies[i]._position - position).magnitude < 0.1f)
                {
                    enemy = _sceneData.mazeConstructor._enemies[i];

                    return enemy;
                }
            }
            return null;
        }
    }

    public struct EnemyComponent
    {
        public EnemyView enemyView;
        public EnemyData enemyData;
        public Animator animator;
    }
}