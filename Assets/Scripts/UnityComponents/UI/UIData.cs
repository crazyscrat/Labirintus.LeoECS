using UnityEngine;
using UnityEngine.UI;
using TMPro;

namespace Labirintus.ECS
{
    public class UIData : MonoBehaviour
    {
        [Header("Система управления движением")]
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
        
        [Header("Система нового лабиринта")]
        [SerializeField] internal Button btnNextMaze;

        [Header("Система информации на экране")]
        [SerializeField] internal TMP_Text textPowerBlueAmount;
        [SerializeField] internal TMP_Text textPowerGreenAmount;
        [SerializeField] internal TMP_Text textPowerPinkAmount;

        [Header("Система атаки")]
        [SerializeField] internal GameObject panelBattle; //панель битвы
        [SerializeField] internal Button btnExitBattle; //панель битвы
        [SerializeField] internal PanelBattleCharacter prefabPanelBattleCharacter; //панель битвы персонажа, префаб
        [SerializeField] internal PanelBattleEnemy prefabPanelBattleEnemy; //панель битвы врага, префаб
        [SerializeField] internal PanelBattleButtons prefabPanelBattleButtons; //панель кнопок битвы , префаб
    }
}
