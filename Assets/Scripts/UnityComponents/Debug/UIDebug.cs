using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Labirintus.ECS
{
    public class UIDebug : MonoBehaviour
    {
        public Button btnReset; //кнопка сброса информации
        public Button btnNewMaze; //кнопка генерации лабиринта
        public TMP_Text textDebug;
    }
}
