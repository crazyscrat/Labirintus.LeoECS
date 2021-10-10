using Leopotam.Ecs;
using UnityEngine;

namespace Labirintus.ECS
{
    internal class WeaponSystem : IEcsRunSystem
    {
        private EcsFilter<Weapon, Reload> _filter;

        public void Run()
        {
            foreach (var i in _filter)
            {
                ref var weapon = ref _filter.Get1(i);

                weapon.cooldownTime -= Time.deltaTime;
                if(weapon.cooldownTime <= 0)
                {
                    weapon.cooldownTime = weapon.reloadTime;
                    _filter.GetEntity(i).Del<Reload>();
                }
            }
        }
    }

    //компонент перезарядки
    internal struct Reload
    {
    }

    public struct Weapon
    {
        public EcsEntity owner;
        public GameObject projectilePrefab;
        public Transform projectileSocket;
        public float projectileSpeed;
        public float projectileRadius;
        public int weaponDamage;
        public float reloadTime; //время перезарадяки
        public float cooldownTime; //оставшее время до выстрела
        public int currentInMagazine;
        public int maxInMagazine;
        public int totalAmmo;
    }

}