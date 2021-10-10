using Leopotam.Ecs;
using System;
using UnityEngine;

namespace Labirintus.ECS
{
    //система генерации врагов
    internal class EnemyGeneratorSystem : IEcsRunSystem
    {
        private GameData _gameData = default;
        private Config _config;
        private SceneData _sceneData;

        private EcsFilter<MazeGenerator> _filter = default;

        public void Run()
        {
            if(!_filter.IsEmpty())
                GenerateEnemyes();
        }

        private void GenerateEnemyes()
        {
            //Debug.Log("генерируем позиции врагов");
            //генерируем позиции врагов
            new EnemyDataGenerator().Generate(_gameData.dataMaze, _gameData.startPosition);
        }
    }
}