using Leopotam.Ecs;
using System;
using UnityEngine;

namespace Labirintus.ECS
{
    //система запуска визуальной генерации лабиринта
    internal class ViewMazeConstructorSystem : IEcsRunSystem
    {
        private GameData _gameData = default;
        private Config _config;
        private SceneData _sceneData;

        private EcsFilter<MazeGenerator> _filter = default;

        public void Run()
        {
            if (!_filter.IsEmpty())
            {
                ViewGenerateMaze();
            }
        }

        private void ViewGenerateMaze()
        {
            //Debug.Log("передаем в конструктор и строим лабиринт");
            //передаем в конструктор и строим лабиринт
            _sceneData.mazeConstructor.data = _gameData.dataMaze;
            _sceneData.mazeConstructor.GenerateNewMaze(_gameData.finishPosition);
        }
    }
}