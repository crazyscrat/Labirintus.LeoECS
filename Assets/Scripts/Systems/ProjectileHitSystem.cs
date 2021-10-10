using Leopotam.Ecs;
using UnityEngine;

namespace Labirintus.ECS
{
    //система обработки попадания снаряда
    internal class ProjectileHitSystem : IEcsRunSystem
    {
        private EcsFilter<Projectile, ProjectileHit> _filter;

        public void Run()
        {
            foreach (var i in _filter)
            {
                ref var entity = ref  _filter.GetEntity(i);
                ref var projectileHit = ref _filter.Get2(i);

                //Debug.Log(projectileHit.raycastHit.collider.name);

                if (projectileHit.raycastHit.collider.tag == "Enemy")
                {
                    Debug.Log("Да-да, это враг");
                    DeactivateProjectile(entity);
                }
                else if (projectileHit.raycastHit.collider.tag == "Wall")
                {
                    Debug.Log("Стена");
                    DeactivateProjectile(entity);
                }

            }
        }

        void DeactivateProjectile(EcsEntity entity)
        {
            ref var projectile = ref entity.Get<Projectile>();
            //ref var projectileHit = ref entity;

            projectile.projectileGO.Deactivate(); //выключаем снаряд
            entity.Del<ProjectileHit>(); //удаляем компонент столкновения
            entity.Del<Moving>(); //удаляем компонент столкновения
        }
    }
}