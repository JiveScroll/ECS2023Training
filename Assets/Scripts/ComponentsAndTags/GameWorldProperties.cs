using Unity.Burst;
using Unity.Entities;
using Unity.Mathematics;

[BurstCompile]
public struct GameWorldProperties : IComponentData
{
    public float2 GameWorldDimensions;
    public int SpawnersToSpawn;
    public Entity SpawnerPrefab;
    public Entity EnemyPrefab;
    public float EnemySpawnRateSeconds;
}

public struct EnemySpawnTimer : IComponentData
{
    public float Value;
}
