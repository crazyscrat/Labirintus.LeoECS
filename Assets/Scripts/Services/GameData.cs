using Leopotam.Ecs;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Labirintus.ECS
{
    [System.Serializable]
    class GameData
    {
        [SerializeField] internal bool NewGame; //������� ����� ����
        [SerializeField] internal int[,] dataMaze; //��������
        [SerializeField] internal Vector2Int startPosition; //��������� ������� � ���������
        [SerializeField] internal Vector2Int finishPosition; //�������� �������
        [SerializeField] internal Vector3 finishPositionV3; //�������� ������� ������3
        [Space]
        [SerializeField] internal Vector2Int currentPlayerPosition; //������� ������� ������ � ���������
        [SerializeField] internal Vector3 currentVelocity; //�������� �������� ������
        [Space]
        [SerializeField] internal bool canMoveLeft = true; //���� ����������� ���������
        [SerializeField] internal bool canMoveRight = true;//���� ����������� ���������
        [SerializeField] internal bool canMoveUp = true;//���� ����������� ���������
        [SerializeField] internal bool canMoveDown = true;//���� ����������� ���������
        [Space]
        [SerializeField] internal StateAttack stateAttack; //����������� �����
        [SerializeField] internal bool canAttackLeft;//���� ����������� ���������
        [SerializeField] internal bool canAttackRight;//���� ����������� ���������
        [SerializeField] internal bool canAttackUp;//���� ����������� ���������
        [SerializeField] internal bool canAttackDown;//���� ����������� ���������
        [SerializeField] internal Vector2Int moveDirection; //����������� �������� ��� �����
        [Space]
        //��������� ���
        [SerializeField] internal PanelBattleCharacter panelBattleCharacter; //������ ��� ������
        [SerializeField] internal PanelBattleEnemy panelBattleEnemy; //������ ��� �����
        [SerializeField] internal PanelBattleButtons panelBattleButtons; //������ ������ ���
        [SerializeField] internal EnemyData enemyData; //������ ����� � ���
        [SerializeField] internal Vector2Int enemyPosition; //������� �������� �����
        //�������� ���
        [SerializeField] internal HashSet<Transform> hashViewEnemies = new HashSet<Transform>();
        [SerializeField] internal List<EcsEntity> projectilesPlayer = new List<EcsEntity>();
    }
}
