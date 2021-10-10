using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

namespace Labirintus.ECS
{
    public class MazeConstructor : MonoBehaviour
    {
        //[SerializeField] GameObject prefabWall; //префаб стены без концов
        [SerializeField] private Transform _parentMaze; //родитель элементов либиринта
        [SerializeField] private Transform _parentEnemies; //родитель врагов
        [SerializeField] private Transform _parentItems; //родитель предметов
        [SerializeField] private Config _config; //конфиг игры
        [SerializeField] private EnemyView _prefabEnemy; //префаб врага
        [SerializeField] private GameObject _prefabStartPoint; //префаб стартовой точки
        [SerializeField] private GameObject _prefabFinishPoint; //префаб конечной точки
        [SerializeField] private GameObject[] _prefabsItems; //префабы поднимаемых предметов

        public int[,] data { get; set; }
        //public List<Enemy> enemies { get; private set; }

        private List<GameObject> _allMaze = new List<GameObject>(); //список всех элементов лабиринта
        public List<EnemyView> _enemies = new List<EnemyView>();  //список всех врагов
        private List<GameObject> _items = new List<GameObject>();  //список всех предметов

        private Vector2Int _finishPoint;

        //internal MazeController mazeController;

        public void GenerateNewMaze(Vector2Int finishPoint)
        {
            _finishPoint = finishPoint;
            //проверка на четность размеров
            if (_config.heightMaze % 2 == 0 && _config.widthMaze % 2 == 0)
            {
                Debug.LogError("Odd numbers work better for dungeon size.");
            }

            DisplayMaze();
        }

        /// <summary>
        /// Функция отображения лабиринта
        /// </summary>
        private void DisplayMaze()
        {
            //enemies = new List<Enemy>();
            int cell = 0;

            for (int y = 0; y <= data.GetUpperBound(0); y++)
            {
                for (int x = 0; x <= data.GetUpperBound(1); x++)
                {
                    cell = data[y, x]; //значение ячейки массива

                    if (cell != 1) //СОЗДАЕМ ЗЕМЛЮ
                    {
                        //создаем землю из префаба
                        GameObject ground = Instantiate(Resources.Load<GameObject>("Environment/grass"), _parentMaze);
                        ground.transform.position = new Vector3(x, y, 0f);

                        _allMaze.Add(ground);

                        //СОЗДАЕМ ПРЕДМЕТ НА ЗЕМЛЕ
                        if (cell != 2 && cell != 9)
                        {
                            GenerateItem(x, y);
                        }
                    }

                    if (cell == 1) //СОЗДАЕМ СТЕНЫ
                    {
                        //создаем стену из префаба
                        GetPrefabWall(y, x);
                    }
                    else if (cell == 2) //СОЗДАЕМ ВРАГОВ
                    {
                        EnemyView enemy = Instantiate(_prefabEnemy, _parentEnemies);
                        enemy.transform.position = new Vector3(x, y, 0f);
                        enemy._position = new Vector2Int(x, y);
                        _enemies.Add(enemy);
                    }
                    else if (cell == 9 && _finishPoint.x == x && _finishPoint.y == y) //СОЗДАЕМ ФИНИШНУЮ ТОЧКУ ПУТИ
                    {
                        GameObject start = Instantiate(_prefabFinishPoint, _parentMaze);
                        start.transform.position = new Vector3(x, y, 0f);
                        _allMaze.Add(start);
                    }
                }
            }

            //mazeController.mazeLoad = true;
        }

        /// <summary>
        /// Функция создания предмета на земле лабиринта
        /// </summary>
        /// <param name="x">позиция Х в массиве</param>
        /// <param name="y">позиция Y в массиве</param>
        private void GenerateItem(int x, int y)
        {
            int index = Random.Range(0, _prefabsItems.Length);

            GameObject item = Instantiate(_prefabsItems[index], _parentMaze);
            item.transform.position = new Vector3(x, y, 0f);
            _items.Add(item);
        }

        /// <summary>
        /// Функция определения необходимого префаба относительно соседних стен
        /// </summary>
        /// <param name="y"></param>
        /// <param name="x"></param>
        void GetPrefabWall(int y, int x)
        {
            GameObject go = null;

            string s = "wall_";
            //WNES
            if (x > 0 && data[y, x - 1] != 1)
            {
                s += "w";
            }
            if (y < data.GetUpperBound(0) && data[y + 1, x] != 1)
            {
                s += "n";
            }
            if (x < data.GetUpperBound(1) && data[y, x + 1] != 1)
            {
                s += "e";
            }
            if (y > 0 && data[y - 1, x] != 1)
            {
                s += "s";
            }

            //Debug.Log($"x={x}, z={z}, name = {s}");

            go = Instantiate(Resources.Load<GameObject>("Walls/" + s), _parentMaze);

            go.transform.position = new Vector3(x, y, 0f);
            go.name = s;
            go.tag = "Wall";

            _allMaze.Add(go);
        }

        /// <summary>
        /// Функция нахождения и удаления всех стен лабиринта
        /// </summary>
        internal void DisposeOldMaze()
        {
            foreach (var g in _allMaze)
            {
                Destroy(g);
            }

            _allMaze.Clear();

            foreach (var g in _enemies)
            {
                Destroy(g.gameObject);
            }

            _enemies.Clear();

            foreach (var g in _items)
            {
                Destroy(g.gameObject);
            }

            _items.Clear();

            //GameObject[] objects = GameObject.FindGameObjectsWithTag("Wall");
            //foreach (GameObject g in objects)
            //{
            //    Destroy(g);
            //}

            //GameObject go = GameObject.FindGameObjectWithTag("Finish");
            //Destroy(go);
            //go = GameObject.FindGameObjectWithTag("Start");
            //Destroy(go);

            //objects = GameObject.FindGameObjectsWithTag("Enemy");
            //foreach (GameObject g in objects)
            //{
            //    Destroy(g);
            //}
        }

    }
}
