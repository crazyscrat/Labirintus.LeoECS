using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Labirintus.ECS
{
    public class WeaponSettings : MonoBehaviour
    {
        public GameObject projectilePrefab;
        public Transform projectileSocket;
        public float projectileSpeed;
        public float projectileRadius;
        public int weaponDamage;
        public float reloadTime;
        public int currentInMagazine;
        public int maxInMagazine;
        public int totalAmmo;
    }
}
