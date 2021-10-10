using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Labirintus.ECS
{
    [CreateAssetMenu(menuName ="SO/Player Game State Data")]
    public class PlayerData : ScriptableObject
    {
        public bool FirstStart; //первый запуск игры
        [Header("Энергия")]
        public int PowerBlueAmountPlayer;
        public int PowerGreenAmountPlayer;
        public int PowerPinkAmountPlayer;
        [Header("Вещи")]
        public int CoinsAmountPlayer;
        [Header("Характеристики")]
        public int HealthPointPlayer;
        public int MaxHealthPointPlayer;
        public int LevelPlayer;
        public int Damage;
        public int DefensePlayer;
        public int MaxDefensePlayer;
        [Header("Спрайт")]
        public Sprite Sprite;
        [Header("Бой")]
        public int HealthPointRestore;
        public int DefensePointRestore;
        [Header("Поле обзора")]
        public float viewRadius; //радиус обзора
        public float viewAngle; //угол обзора
        public int rayCount; //качество поля зрения
        public int edgeResolveIterations; //количество итерация для точного определения угла препятствия
        public float edgeDistThreshold; //предельное расстояние между разными препятстсвиями

        
    }
}
