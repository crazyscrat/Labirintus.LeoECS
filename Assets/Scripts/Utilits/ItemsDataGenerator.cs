using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Labirintus.ECS
{
    public class ItemsDataGenerator
    {
        Vector2Int _itemPos = Vector2Int.zero;
        Vector2Int _startPos = Vector2Int.zero;
        Vector2Int _finishPos = Vector2Int.zero;
        List<Vector2Int> _items;

        public void Generate(int[,] data, Vector2Int startPos, Vector2Int finishPos)
        {
            int[,] maze = data;
            this._startPos = startPos;

            _items = new List<Vector2Int>();

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
                    _itemPos.x = rx;
                    _itemPos.y = ry;
                    //Debug.Log(enemyPos);
                } while (!CheckNearEnemy(_itemPos));

                _items.Add(_itemPos);
            }

            //помечаем клетки с врагами
            foreach (Vector2Int p in _items)
            {
                data[p.y, p.x] = 2;
            }
        }

        //проверка расстояний
        bool CheckNearEnemy(Vector2Int pos)
        {
            foreach (Vector2Int p in _items)
            {
                if (Vector2Int.Distance(pos, p) < 3) return false; //расстояние до других врагов
            }
            if (Vector2Int.Distance(pos, _startPos) < 3) return false; //расстояние до точки старта где стоит игрок


            return true;
        }
    }
}
