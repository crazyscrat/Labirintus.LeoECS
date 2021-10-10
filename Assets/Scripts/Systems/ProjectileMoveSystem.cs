using Leopotam.Ecs;
using UnityEngine;

namespace Labirintus.ECS
{
    //система движения снаряда и его попадания
    internal class ProjectileMoveSystem : IEcsRunSystem
    {
        EcsFilter<Projectile, Moving> _filter;

        public void Run()
        {
            foreach (var i in _filter)
            {
                ref var projectile = ref _filter.Get1(i);

                var position = projectile.transform.position;
                position += projectile.direction.normalized * projectile.speed * Time.deltaTime;
                projectile.transform.position = position;

                var displacementSinceLastFrame = position - projectile.previusPos;
                var hit = Physics2D.CircleCast(projectile.previusPos, projectile.radius, displacementSinceLastFrame.normalized, displacementSinceLastFrame.magnitude);
                if (hit)
                {
                    ref var entity = ref _filter.GetEntity(i);
                    ref var projectileHit = ref entity.Get<ProjectileHit>(); //добавляем компонент попадания
                    projectileHit.raycastHit = hit;
                    //Debug.Log("Попали");
                }

                projectile.previusPos = projectile.transform.position;
            }
        }
    }

    internal struct Moving: IEcsIgnoreInFilter
    {
    }

    internal struct ProjectileHit
    {
        internal RaycastHit2D raycastHit;
    }
}