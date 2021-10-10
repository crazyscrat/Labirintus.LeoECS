using Leopotam.Ecs;

namespace Labirintus.ECS
{
    //система удаления старого лабиринта
    internal sealed class MazeDisposeSystem : IEcsInitSystem, IEcsRunSystem
    {
        private EcsWorld _world;
        private SceneData _sceneData;

        private EcsFilter<MazeDispoze> _filter = default;

        public void Init()
        {
            _sceneData.mazeConstructor.DisposeOldMaze();
        }

        public void Run()
        {
            if (_filter.IsEmpty()) return;
            _sceneData.mazeConstructor.DisposeOldMaze();
            _world.NewEntity().Get<MazeGenerator>();
        }
    }

    internal struct MazeDispoze
    {
    }
}