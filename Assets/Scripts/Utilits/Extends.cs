using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Labirintus.ECS
{
    public static class Extends
    {
        public static void Deactivate(this GameObject gameObject)
        {
            gameObject.SetActive(false);
        }
        public static void Activate(this GameObject gameObject)
        {
            gameObject.SetActive(true);
        }
    }
}
