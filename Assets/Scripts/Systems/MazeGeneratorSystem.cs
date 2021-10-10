using Leopotam.Ecs;
using LeopotamGroup.Globals;
using UnityEngine;

namespace Labirintus.ECS
{
    //система генерации лабиринта
    internal class MazeGeneratorSystem : IEcsInitSystem, IEcsRunSystem
    {
        private EcsWorld _world;
        private GameData _gameData = default;
        private Config _config;
        private SceneData _sceneData;

        private EcsFilter<MazeGenerator> _filter = default;

        //public int[,] dataMaze { get; private set; }
        private int[,] _dataMaze;

        public void Init()
        {
            //GenerateNewMaze();
            _world.NewEntity().Get<MazeGenerator>();
        }

        public void Run()
        {
            if (_filter.IsEmpty()) return;

            GenerateNewMaze();

            _gameData.NewGame = true;
        }

        //генерация двумерного массива, путевых точек, врагов
        private void GenerateNewMaze()
        {
            //0 - трава
            //1 - стена
            //2 - враг
            //9 - начало и конец пути
            //генерируем лабиринт
            _dataMaze = new MazeDataGenerator().FromDimensions(_config.heightMaze, _config.widthMaze, _config.placementThreshold);

            if (_config.noClosed)
            {
                FindClosePosition();
            }

            //находим стартовую позицию
            FindStartPosition();
            //находим конечную позицию
            FindGoalPosition();

            //генерируем позиции врагов
            //new EnemyDataGenerator().Generate(_dataMaze, _gameData.startPosition);

            //передаем в конструктор и строим лабиринт
            //_sceneData.mazeConstructor.data = _dataMaze;
            //_sceneData.mazeConstructor.GenerateNewMaze(_gameData.finishPosition);

            _gameData.dataMaze = _dataMaze; //сохраняем массив в глобальное хранилище
        }

        //Этот код начинает с 0,0 и проходит по всем данных лабиринта, пока не находит открытое пространство. Затем эти координаты сохраняются как начальная позиция лабиринта.
        private void FindStartPosition()
        {
            int[,] maze = _dataMaze;
            int rMax = maze.GetUpperBound(0);
            int cMax = maze.GetUpperBound(1);

            for (int y = 0; y <= rMax; y++)
            {
                for (int x = 0; x <= cMax; x++)
                {
                    if (maze[y, x] == 0)
                    {
                        _gameData.startPosition.y = y;
                        _gameData.startPosition.x = x;

                        //GameObject start = Instantiate(Resources.Load<GameObject>("Start"));
                        //start.transform.position = new Vector3(x, y, 0f);
                        //allMaze.Add(start);
                        _dataMaze[y, x] = 9; //помеяаем ячейку как начало пути
                        return;
                    }
                }
            }
        }

        //начинает с максимальных значений и выполняет обратный отсчёт. Добавим и этот метод тоже.
        private void FindGoalPosition()
        {
            int[,] maze = _dataMaze;
            int rMax = maze.GetUpperBound(0);
            int cMax = maze.GetUpperBound(1);

            // loop top to bottom, right to left
            for (int y = rMax; y >= 0; y--)
            {
                for (int x = cMax; x >= 0; x--)
                {
                    if (maze[y, x] == 0)
                    {
                        _gameData.finishPosition.y = y;
                        _gameData.finishPosition.x = x;

                        //GameObject finish = Instantiate(Resources.Load<GameObject>("Finish"));
                        //finish.transform.position = new Vector3(x, y, 0f);
                        //allMaze.Add(finish);
                        _dataMaze[y, x] = 9; //помечаем ячейку как конец пути
                        return;
                    }
                }
            }
        }

        /// <summary>
        /// Функция поиска замкнутых клеток и удаления лишних стен
        /// </summary>
        private void FindClosePosition()
        {
            int[,] maze = _dataMaze;
            int rMax = maze.GetUpperBound(0);
            int cMax = maze.GetUpperBound(1);

            for (int i = 1; i < rMax; i++)
            {
                for (int j = 1; j < cMax; j++)
                {
                    if (maze[i, j] == 0)
                    {
                        if (CheckCell(i, j))
                        {
                            int a = Random.value < .5f ? 0 : (Random.value < .5f ? -1 : 1);
                            //проверка на приграничные клетки
                            if (i == 1 && a != 0 && a == -1) a = 1;
                            else if (i == rMax - 1 && a != 0 && a == 1) a = -1;

                            int b = a != 0 ? 0 : (Random.value < .5f ? -1 : 1);
                            //проверка на приграничные клетки
                            if (j == 1 && b != 0 && b == -1) b = 1;
                            else if (j == rMax - 1 && b != 0 && b == 1) b = -1;

                            maze[i + a, j + b] = 0;
                        }
                    }
                }
            }
        }

        /// <summary>
        /// Функция проверки на замкнутость ячейки
        /// </summary>
        /// <param name="y"></param>
        /// <param name="x"></param>
        /// <returns></returns>
        bool CheckCell(int y, int x)
        {
            bool isClose = true;
            if (_dataMaze[y + 1, x] == 0) isClose = false;
            if (_dataMaze[y, x + 1] == 0) isClose = false;
            if (_dataMaze[y - 1, x] == 0) isClose = false;
            if (_dataMaze[y, x - 1] == 0) isClose = false;

            return isClose;
        }

    }

    internal struct MazeGenerator{}
}