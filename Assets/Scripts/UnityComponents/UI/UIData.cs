using UnityEngine;
using UnityEngine.UI;
using TMPro;

namespace Labirintus.ECS
{
    public class UIData : MonoBehaviour
    {
        [Header("������� ���������� ���������")]
        [SerializeField] internal Button btnMoveLeft;
        [SerializeField] internal Image btnMoveLeftImage;
        [SerializeField] internal Sprite leftArrow;
        [SerializeField] internal Text textBtnMoveLeft;

        [Space]
        [SerializeField] internal Button btnMoveRight;
        [SerializeField] internal Image btnMoveRightImage;
        [SerializeField] internal Sprite rightArrow;
        [SerializeField] internal Text textBtnMoveRight;

        [Space]
        [SerializeField] internal Button btnMoveUp;
        [SerializeField] internal Image btnMoveUpImage;
        [SerializeField] internal Sprite upArrow;
        [SerializeField] internal Text textBtnMoveUp;

        [Space]
        [SerializeField] internal Button btnMoveDown;
        [SerializeField] internal Image btnMoveDownImage;
        [SerializeField] internal Sprite downArrow;
        [SerializeField] internal Text textBtnMoveDown;

        [Space]
        [SerializeField] internal Sprite attackSprite;

        [Space]
        [SerializeField] internal Joystick joystick;
        
        [Header("������� ������ ���������")]
        [SerializeField] internal Button btnNextMaze;

        [Header("������� ���������� �� ������")]
        [SerializeField] internal TMP_Text textPowerBlueAmount;
        [SerializeField] internal TMP_Text textPowerGreenAmount;
        [SerializeField] internal TMP_Text textPowerPinkAmount;

        [Header("������� �����")]
        [SerializeField] internal GameObject panelBattle; //������ �����
        [SerializeField] internal Button btnExitBattle; //������ �����
        [SerializeField] internal PanelBattleCharacter prefabPanelBattleCharacter; //������ ����� ���������, ������
        [SerializeField] internal PanelBattleEnemy prefabPanelBattleEnemy; //������ ����� �����, ������
        [SerializeField] internal PanelBattleButtons prefabPanelBattleButtons; //������ ������ ����� , ������
    }
}
