using System;
using Unity.Collections;
using Unity.Entities;
using Unity.Mathematics;
using Unity.Transforms;

public readonly partial struct GameWorldAspect : IAspect
{
    public readonly Entity Entity;

    private readonly RefRO<GameWorldProperties> _gameWorldProperties;
    private readonly RefRW<GameWorldRandom> _gameWorldRandom;
    private readonly RefRO<LocalTransform> _transform;

    private readonly RefRW<EnemySpawnPoints> _enemySpawnPoints;
    private readonly RefRW<EnemySpawnTimer> _enemySpawnTimer;

    /// <summary>
    /// The minimum required distance squared that should be between our base and any spawned objects.
    /// </summary>
    private const float BASE_SAFETY_RADIUS_SQ = 100;

    private float3 minCorner => _transform.ValueRO.Position - halfDimensions;
    private float3 maxCorner => _transform.ValueRO.Position + halfDimensions;

    private float3 halfDimensions => new()
    {
        x = _gameWorldProperties.ValueRO.GameWorldDimensions.x * 0.5f,
        y = 0,
        z = _gameWorldProperties.ValueRO.GameWorldDimensions.y * 0.5f
    };

    public Entity SpawnerPrefab => _gameWorldProperties.ValueRO.SpawnerPrefab;
    public Entity EnemyPrefab => _gameWorldProperties.ValueRO.EnemyPrefab;

    public int NumberSpawnersToSpawn => _gameWorldProperties.ValueRO.SpawnersToSpawn;
    private int enemySpawnPointCount => _enemySpawnPoints.ValueRO.Value.Value.Value.Length;


    public LocalTransform GetRandomSpawnerTransform()
    {
        return new LocalTransform
        {
            Position = GetRandomPosition(),
            Rotation = GetRandomRotation(),
            Scale = GetRandomScale(0.75f),
        };
    }

    public bool EnemySpawnPointInitialized()
    {
        return _enemySpawnPoints.ValueRO.Value.IsCreated && enemySpawnPointCount > 0;
    }

    public float EnemySpawnTimer
    {
        get => _enemySpawnTimer.ValueRO.Value;
        set => _enemySpawnTimer.ValueRW.Value = value;
    }

    public bool TimeToSpawnEnemy => EnemySpawnTimer <= 0f;

    public float EnemySpawnRate => _gameWorldProperties.ValueRO.EnemySpawnRateSeconds;

    public LocalTransform GetEnemySpawnPoint()
    {
        float3 position = GetRandomEnemySpawnPoint();

        return new LocalTransform
        {
            Position = position,
            Rotation = quaternion.RotateY(MathHelpers.GetHeading(position, _transform.ValueRO.Position)),
            Scale = 1f
        };
    }

    private float3 GetEnemySpawnPointByIndex(int i) => _enemySpawnPoints.ValueRO.Value.Value.Value[i];

    private float3 GetRandomEnemySpawnPoint()
    {
        return GetEnemySpawnPointByIndex(_gameWorldRandom.ValueRW.Value.NextInt(enemySpawnPointCount));
    }

    private float3 GetRandomPosition()
    {
        float3 randomPosition;

        do
        {
            randomPosition = _gameWorldRandom.ValueRW.Value.NextFloat3(minCorner, maxCorner);
        }        
        while (math.distancesq(_transform.ValueRO.Position, randomPosition) <= BASE_SAFETY_RADIUS_SQ);

        return randomPosition;
    }

    private quaternion GetRandomRotation() => quaternion.RotateY(_gameWorldRandom.ValueRW.Value.NextFloat(-0.25f, 0.25f));
    private float GetRandomScale(float minScale) => _gameWorldRandom.ValueRW.Value.NextFloat(minScale, 1f);



}
