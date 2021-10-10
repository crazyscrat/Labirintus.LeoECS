using Leopotam.Ecs;
using LeopotamGroup.Globals;
using UnityEngine;

namespace Labirintus.ECS
{
    //система генерации снарядов
    internal class SpawnProjectileSystem : IEcsRunSystem
    {
        private EcsWorld _world;

        private EcsFilter<Weapon, SpawnProjectile> _filter;

        public void Run()
        {
            foreach (var i in _filter)
            {
                ref var weapon = ref _filter.Get1(i);
                ref var spawn = ref _filter.Get2(i);
                bool fromQueue = false;

                //ищем в очереди
                if (Service<GameData>.Get().projectilesPlayer.Count > 0)
                {
                    foreach (var entity in Service<GameData>.Get().projectilesPlayer)
                    {
                        ref var projectile = ref entity.Get<Projectile>();
                        if (!projectile.projectileGO.activeInHierarchy)
                        {
                            projectile.previusPos = weapon.projectileSocket.position;
                            projectile.transform.position = weapon.projectileSocket.position;
                            projectile.direction = spawn.targetPosition - projectile.previusPos;
                            projectile.projectileGO.Activate();
                            entity.Get<Moving>(); //компонент движения снаряда
                            fromQueue = true;
                            //Debug.Log("взяли из очереди");
                            break;
                        }
                    }
                }
                //создаем новый
                if (!fromQueue)
                {
                    //создаем геймобжект и сущность
                    var projectileGO = GameObject.Instantiate(weapon.projectilePrefab, weapon.projectileSocket.position, Quaternion.identity);
                    var projectileEntity = _world.NewEntity();
                    projectileEntity.Get<Moving>(); //компонент движения снаряда

                    //создаем копонент снаряда и заполняем
                    ref var projectile = ref projectileEntity.Get<Projectile>();
                    projectile.damage = weapon.weaponDamage;
                    projectile.radius = weapon.projectileRadius;
                    projectile.speed = weapon.projectileSpeed;
                    projectile.projectileGO = projectileGO;
                    projectile.transform = projectileGO.transform;
                    projectile.previusPos = projectile.transform.position;
                    projectile.direction = spawn.targetPosition - projectile.previusPos;
                    
                    Service<GameData>.Get().projectilesPlayer.Add(projectileEntity);
                }

                //удаляем компонент
                _filter.GetEntity(i).Del<SpawnProjectile>();

                Debug.Log(Service<GameData>.Get().projectilesPlayer.Count);
            }
        }
    }

    //компонент снаряда
    internal struct Projectile
    {
        public float damage;
        public Vector3 direction;
        public float radius;
        public float speed;
        public Vector3 previusPos;
        public GameObject projectileGO;
        public Transform transform;
    }
}