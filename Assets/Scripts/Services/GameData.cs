using Leopotam.Ecs;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Labirintus.ECS
{
    [System.Serializable]
    class GameData
    {
        [SerializeField] internal bool NewGame; //признак новой игры
        [SerializeField] internal int[,] dataMaze; //лабиринт
        [SerializeField] internal Vector2Int startPosition; //начальная позиция в лабиринте
        [SerializeField] internal Vector2Int finishPosition; //конечная позиция
        [SerializeField] internal Vector3 finishPositionV3; //конечная позиция вектор3
        [Space]
        [SerializeField] internal Vector2Int currentPlayerPosition; //текущая позиция игрока в лабиринте
        [SerializeField] internal Vector3 currentVelocity; //скорость движения игрока
        [Space]
        [SerializeField] internal bool canMoveLeft = true; //флаг возможности двигаться
        [SerializeField] internal bool canMoveRight = true;//флаг возможности двигаться
        [SerializeField] internal bool canMoveUp = true;//флаг возможности двигаться
        [SerializeField] internal bool canMoveDown = true;//флаг возможности двигаться
        [Space]
        [SerializeField] internal StateAttack stateAttack; //направление атаки
        [SerializeField] internal bool canAttackLeft;//флаг возможности атаковать
        [SerializeField] internal bool canAttackRight;//флаг возможности атаковать
        [SerializeField] internal bool canAttackUp;//флаг возможности атаковать
        [SerializeField] internal bool canAttackDown;//флаг возможности атаковать
        [SerializeField] internal Vector2Int moveDirection; //направление движения при атаке
        [Space]
        //пошаговый бой
        [SerializeField] internal PanelBattleCharacter panelBattleCharacter; //панель боя игрока
        [SerializeField] internal PanelBattleEnemy panelBattleEnemy; //панель боя врага
        [SerializeField] internal PanelBattleButtons panelBattleButtons; //панель кнопок боя
        [SerializeField] internal EnemyData enemyData; //данные врага в бою
        [SerializeField] internal Vector2Int enemyPosition; //позиция текущего врага
        //реалтайм бой
        [SerializeField] internal HashSet<Transform> hashViewEnemies = new HashSet<Transform>();
        [SerializeField] internal List<EcsEntity> projectilesPlayer = new List<EcsEntity>();
    }
}
