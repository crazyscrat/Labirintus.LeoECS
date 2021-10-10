using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;
using Random = UnityEngine.Random;

namespace Labirintus.ECS
{
    class EnemyDataGenerator
    {
        private Vector2Int _enemyPos = Vector2Int.zero;
        private Vector2Int _startPos = Vector2Int.zero;
        private List<Vector2Int> _enemies;

        public void Generate(int[,] data, Vector2Int startPos)
        {
            int[,] maze = data;
            this._startPos = startPos;

            _enemies = new List<Vector2Int>();

            int minRow = 1;
            int minCol = 1;
            int maxRow = maze.GetUpperBound(0);
            int maxCol = maze.GetUpperBound(1);

            int enemiesCount = (maxRow - 1) * (maxCol - 1) / 15;
            //Debug.Log($"counts = {enemiesCount}, all = {(maxRow - 1) * (maxCol - 1)}");

            for (int i = 0; i < enemiesCount; i++)
            {
                do
                {
                    int rx = Random.Range(minCol, maxCol);
                    int ry = Random.Range(minRow, maxRow);

                    while (maze[ry, rx] != 0)
                    {
                        rx = Random.Range(minCol, maxCol);
                        ry = Random.Range(minRow, maxRow);
                    }
                    _enemyPos.x = rx;
                    _enemyPos.y = ry;
                    //Debug.Log(enemyPos);
                } while (!CheckNearEnemy(_enemyPos));

                _enemies.Add(_enemyPos);
            }

            //помечаем клетки с врагами
            foreach (Vector2Int p in _enemies)
            {
                data[p.y, p.x] = 2;
            }
        }

        //проверка расстояний
        bool CheckNearEnemy(Vector2Int pos)
        {
            foreach (Vector2Int p in _enemies)
            {
                if (Vector2Int.Distance(pos, p) < 3) return false; //расстояние до других врагов
            }
            if (Vector2Int.Distance(pos, _startPos) < 3) return false; //расстояние до точки старта где стоит игрок


            return true;
        }
    }
}
