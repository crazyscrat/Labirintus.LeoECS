using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Labirintus.ECS
{
    [CreateAssetMenu(menuName ="SO/Player Game State Data")]
    public class PlayerData : ScriptableObject
    {
        public bool FirstStart; //������ ������ ����
        [Header("�������")]
        public int PowerBlueAmountPlayer;
        public int PowerGreenAmountPlayer;
        public int PowerPinkAmountPlayer;
        [Header("����")]
        public int CoinsAmountPlayer;
        [Header("��������������")]
        public int HealthPointPlayer;
        public int MaxHealthPointPlayer;
        public int LevelPlayer;
        public int Damage;
        public int DefensePlayer;
        public int MaxDefensePlayer;
        [Header("������")]
        public Sprite Sprite;
        [Header("���")]
        public int HealthPointRestore;
        public int DefensePointRestore;
        [Header("���� ������")]
        public float viewRadius; //������ ������
        public float viewAngle; //���� ������
        public int rayCount; //�������� ���� ������
        public int edgeResolveIterations; //���������� �������� ��� ������� ����������� ���� �����������
        public float edgeDistThreshold; //���������� ���������� ����� ������� ��������������

        
    }
}
