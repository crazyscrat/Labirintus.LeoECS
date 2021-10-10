using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

namespace Labirintus.ECS
{
    public class MazeConstructor : MonoBehaviour
    {
        //[SerializeField] GameObject prefabWall; //������ ����� ��� ������
        [SerializeField] private Transform _parentMaze; //�������� ��������� ���������
        [SerializeField] private Transform _parentEnemies; //�������� ������
        [SerializeField] private Transform _parentItems; //�������� ���������
        [SerializeField] private Config _config; //������ ����
        [SerializeField] private EnemyView _prefabEnemy; //������ �����
        [SerializeField] private GameObject _prefabStartPoint; //������ ��������� �����
        [SerializeField] private GameObject _prefabFinishPoint; //������ �������� �����
        [SerializeField] private GameObject[] _prefabsItems; //������� ����������� ���������

        public int[,] data { get; set; }
        //public List<Enemy> enemies { get; private set; }

        private List<GameObject> _allMaze = new List<GameObject>(); //������ ���� ��������� ���������
        public List<EnemyView> _enemies = new List<EnemyView>();  //������ ���� ������
        private List<GameObject> _items = new List<GameObject>();  //������ ���� ���������

        private Vector2Int _finishPoint;

        //internal MazeController mazeController;

        public void GenerateNewMaze(Vector2Int finishPoint)
        {
            _finishPoint = finishPoint;
            //�������� �� �������� ��������
            if (_config.heightMaze % 2 == 0 && _config.widthMaze % 2 == 0)
            {
                Debug.LogError("Odd numbers work better for dungeon size.");
            }

            DisplayMaze();
        }

        /// <summary>
        /// ������� ����������� ���������
        /// </summary>
        private void DisplayMaze()
        {
            //enemies = new List<Enemy>();
            int cell = 0;

            for (int y = 0; y <= data.GetUpperBound(0); y++)
            {
                for (int x = 0; x <= data.GetUpperBound(1); x++)
                {
                    cell = data[y, x]; //�������� ������ �������

                    if (cell != 1) //������� �����
                    {
                        //������� ����� �� �������
                        GameObject ground = Instantiate(Resources.Load<GameObject>("Environment/grass"), _parentMaze);
                        ground.transform.position = new Vector3(x, y, 0f);

                        _allMaze.Add(ground);

                        //������� ������� �� �����
                        if (cell != 2 && cell != 9)
                        {
                            GenerateItem(x, y);
                        }
                    }

                    if (cell == 1) //������� �����
                    {
                        //������� ����� �� �������
                        GetPrefabWall(y, x);
                    }
                    else if (cell == 2) //������� ������
                    {
                        EnemyView enemy = Instantiate(_prefabEnemy, _parentEnemies);
                        enemy.transform.position = new Vector3(x, y, 0f);
                        enemy._position = new Vector2Int(x, y);
                        _enemies.Add(enemy);
                    }
                    else if (cell == 9 && _finishPoint.x == x && _finishPoint.y == y) //������� �������� ����� ����
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
        /// ������� �������� �������� �� ����� ���������
        /// </summary>
        /// <param name="x">������� � � �������</param>
        /// <param name="y">������� Y � �������</param>
        private void GenerateItem(int x, int y)
        {
            int index = Random.Range(0, _prefabsItems.Length);

            GameObject item = Instantiate(_prefabsItems[index], _parentMaze);
            item.transform.position = new Vector3(x, y, 0f);
            _items.Add(item);
        }

        /// <summary>
        /// ������� ����������� ������������ ������� ������������ �������� ����
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
        /// ������� ���������� � �������� ���� ���� ���������
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
