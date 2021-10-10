using Leopotam.Ecs;
using LeopotamGroup.Globals;
using UnityEngine;

namespace Labirintus.ECS
{
    //система обработки окончания боя
    internal class EndBattleSystem : IEcsRunSystem
    {
        private EcsWorld _world;
        private UIData _uiData = default;

        private EcsFilter<EndBattle> _filter;
        public void Run()
        {
            if (_filter.IsEmpty()) return;

            //деативируем панели
            Service<GameData>.Get().panelBattleCharacter.gameObject.Deactivate();
            Service<GameData>.Get().panelBattleEnemy.gameObject.Deactivate();
            _uiData.panelBattle.gameObject.Deactivate(); //прячем панель

            Vector2Int position = Service<GameData>.Get().enemyPosition;
            //Debug.Log(position);
            //Debug.Log(Service<GameData>.Get().dataMaze[position.y, position.x]);
            Service<GameData>.Get().dataMaze[position.y, position.x] = 0;

            _world.NewEntity().Get<CheckObstacle>();
        }
    }

    public struct EndBattle { }
}