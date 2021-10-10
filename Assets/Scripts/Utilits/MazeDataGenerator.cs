using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Labirintus.ECS
{
    /// <summary>
    /// ����� ��������� ������� ���������� ����
    /// </summary>
    public class MazeDataGenerator
    {
        private float placementThreshold; //���� ������ ������

        /// <summary>
        /// ����������� ������
        /// </summary>
        //public MazeDataGenerator()
        //{
        //    placementThreshold = GameObject.FindObjectOfType<MazeController>().placementThreshold;
        //}

        /// <summary>
        /// ������� ��������� ������� ���������� ����
        /// </summary>
        /// <param name="sizeRows">���������� �����</param>
        /// <param name="sizeCols">���������� ��������</param>
        /// <returns>������ [int,int]</returns>
        public int[,] FromDimensions(int sizeRows, int sizeCols, float placementThreshold)
        {
            int[,] maze = new int[sizeRows, sizeCols]; //������ ������ �������� ��������
            this.placementThreshold = placementThreshold;

            int rMax = maze.GetUpperBound(0); //������ ���������� ����
            int cMax = maze.GetUpperBound(1); //������ ���������� �������

            for (int y = 0; y <= rMax; y++)
            {
                for (int x = 0; x <= cMax; x++)
                {
                    //���������� ������� ���������, ���������� �����
                    if (y == 0 || x == 0 || y == rMax || x == cMax)
                    {
                        maze[y, x] = 1;
                    }
                    //������� ������ ������ (������) ������
                    else if (y % 2 == 0 && x % 2 == 0)
                    {
                        if (Random.value > placementThreshold) //���� ���� ��������� �����
                        {
                            maze[y, x] = 1; //���������� �����
                                            //��� ����������� �������� 1 ������� ������ � �������� ��������� �������� ������. ��� ���������� ��������� ��������� �������� ��� ����������� � ������� ������� 0, 1 ��� -1, ������� ����� ������� ������ �������� ������.
                            int a = Random.value < .5f ? 0 : (Random.value < .5f ? -1 : 1); //�������� �� ���������
                            int b = a != 0 ? 0 : (Random.value < .5f ? -1 : 1); //�������� �� �����������

                            maze[y + a, x + b] = 1;
                        }
                    }
                }
            }

            return maze;
        }
    }
}
