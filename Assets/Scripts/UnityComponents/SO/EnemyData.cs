using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Labirintus.ECS
{
    [CreateAssetMenu(menuName ="SO/Enemy data")]
    public class EnemyData : ScriptableObject
    {
        [Header("Энергия")]
        public int PowerBlueAmount;
        public int PowerGreenAmount;
        public int PowerPinkAmount;
        [Header("Вещи")]
        public int CoinsAmount;
        [Header("Характеристики")]
        public int HealthPoint;
        public int MaxHealthPoint;
        public int LevelEnemy;
        public int DamageEnemy;
        public int DefenseEnemy;
        public int MaxDefenseEnemy;
        [Header("Спрайт")]
        public Sprite Sprite;
        [Header("Бой")]
        public int HealthPointRestore;
        public int DefensePointRestore;
    }
}
