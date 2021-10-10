using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Labirintus.ECS
{
    [CreateAssetMenu(menuName = "SO/Config")]
    public class Config : ScriptableObject
    {
        public float PlayerMoveSpeed = 1f;
        [Space]
        public int widthMaze = 5;
        public int heightMaze = 5;
        public float placementThreshold = 0.2f;
        public bool noClosed = true;

        [Space]
        public Vector3 cameraOffset;
        public float cameraSmoothness;
        public string fileName;
    }
}
