using Leopotam.Ecs;
using UnityEngine;

namespace Labirintus.ECS
{
    //Система инициализации камеры и следования за игроком
    internal sealed class CameraFollowSystem : IEcsInitSystem, IEcsRunSystem
    {
        private readonly EcsWorld _world;
        private SceneData _sceneData;
        private Config _config;

        private EcsFilter<PlayerRef> _player = default;
        private EcsFilter<CameraComponent> _camera = default;

        public void Init()
        {
            ref var camera = ref _world.NewEntity().Get<CameraComponent>();
            camera.cameraTransform = _sceneData._mainCamera.transform;
            camera.offset = _config.cameraOffset;
            camera.cameraSmoothness = _config.cameraSmoothness;
        }

        public void Run()
        {
            foreach (var i in _player)
            {
                ref var player = ref _player.Get1(i);

                foreach (var i2 in _camera)
                {
                    ref var camera = ref _camera.Get1(i2);

                    camera.cameraTransform.position = Vector3.SmoothDamp(camera.cameraTransform.position, player.transform.position, ref camera.cameraSpeed, camera.cameraSmoothness);
                    camera.cameraTransform.position = new Vector3(camera.cameraTransform.position.x, camera.cameraTransform.position.y, camera.offset.z);
                }
            }
        }
    }

    public struct CameraComponent
    {
        public Transform cameraTransform;
        public Vector3 offset;
        public Vector3 cameraSpeed;
        public float cameraSmoothness;
    }
}