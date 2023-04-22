using Unity.Entities;
using Unity.Mathematics;
using UnityEngine;
using Random = Unity.Mathematics.Random;

public class GameWorldManagerMono : MonoBehaviour
{
    public float2 GameWorldDimensions;
    
    public int SpawnersToSpawn;
    public GameObject SpawnerPrefab;

    public GameObject EnemyPrefab;
    public float EnemySpawnRateSeconds;

    public uint RandomSeed;
}

public class GameWorldManagerBaker : Baker<GameWorldManagerMono>
{
    public override void Bake(GameWorldManagerMono authoring)
    {
        Entity gameWorldManagerEntity = GetEntity(TransformUsageFlags.Dynamic);

        AddComponent(gameWorldManagerEntity, new GameWorldProperties
        {
            GameWorldDimensions = authoring.GameWorldDimensions,
            SpawnersToSpawn = authoring.SpawnersToSpawn,
            SpawnerPrefab = GetEntity(authoring.SpawnerPrefab, TransformUsageFlags.Dynamic),
            EnemyPrefab = GetEntity(authoring.EnemyPrefab, TransformUsageFlags.Dynamic),
            EnemySpawnRateSeconds = authoring.EnemySpawnRateSeconds
        });

        AddComponent(gameWorldManagerEntity, new GameWorldRandom
        {
            Value = Random.CreateFromIndex(authoring.RandomSeed)
        });

        AddComponent<EnemySpawnPoints>(gameWorldManagerEntity);
        AddComponent<EnemySpawnTimer>(gameWorldManagerEntity); 
    }
}
