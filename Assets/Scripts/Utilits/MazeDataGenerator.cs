using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Labirintus.ECS
{
    /// <summary>
    ///  ласс генератор массива нахождени€ стен
    /// </summary>
    public class MazeDataGenerator
    {
        private float placementThreshold; //шанс пустой клетки

        /// <summary>
        ///  онструктор класса
        /// </summary>
        //public MazeDataGenerator()
        //{
        //    placementThreshold = GameObject.FindObjectOfType<MazeController>().placementThreshold;
        //}

        /// <summary>
        /// ‘ункци€ генерации массива нахождени€ стен
        /// </summary>
        /// <param name="sizeRows">количество строк</param>
        /// <param name="sizeCols">количество столбцов</param>
        /// <returns>массив [int,int]</returns>
        public int[,] FromDimensions(int sizeRows, int sizeCols, float placementThreshold)
        {
            int[,] maze = new int[sizeRows, sizeCols]; //пустой массив заданных размеров
            this.placementThreshold = placementThreshold;

            int rMax = maze.GetUpperBound(0); //индекс последнего р€да
            int cMax = maze.GetUpperBound(1); //индекс последнего столбца

            for (int y = 0; y <= rMax; y++)
            {
                for (int x = 0; x <= cMax; x++)
                {
                    //определ€ем границы лабиринта, выставл€ем стены
                    if (y == 0 || x == 0 || y == rMax || x == cMax)
                    {
                        maze[y, x] = 1;
                    }
                    //находим каждую вторую (четную) €чейку
                    else if (y % 2 == 0 && x % 2 == 0)
                    {
                        if (Random.value > placementThreshold) //если шанс поставить стену
                        {
                            maze[y, x] = 1; //записываем стену
                                            //код присваивает значение 1 текущей €чейке и случайно выбранной соседней €чейке.  од использует несколько тернарных операций дл€ прибавлени€ к индексу массива 0, 1 или -1, получа€ таким образом индекс соседней €чейки.
                            int a = Random.value < .5f ? 0 : (Random.value < .5f ? -1 : 1); //смещение по вертикали
                            int b = a != 0 ? 0 : (Random.value < .5f ? -1 : 1); //смещение по горизонтали

                            maze[y + a, x + b] = 1;
                        }
                    }
                }
            }

            return maze;
        }
    }
}
