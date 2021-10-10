using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Labirintus.ECS
{
    [CreateAssetMenu(menuName ="SO/Enemy data")]
    public class EnemyData : ScriptableObject
    {
        [Header("�������")]
        public int PowerBlueAmount;
        public int PowerGreenAmount;
        public int PowerPinkAmount;
        [Header("����")]
        public int CoinsAmount;
        [Header("��������������")]
        public int HealthPoint;
        public int MaxHealthPoint;
        public int LevelEnemy;
        public int DamageEnemy;
        public int DefenseEnemy;
        public int MaxDefenseEnemy;
        [Header("������")]
        public Sprite Sprite;
        [Header("���")]
        public int HealthPointRestore;
        public int DefensePointRestore;
    }
}
