using Unity.Burst;
using Unity.Entities;
using Unity.Transforms;

[BurstCompile]
public partial struct EnemySpawnerSystem : ISystem
{
    [BurstCompile]
    public void OnCreate(ref SystemState state)
    {

    }

    [BurstCompile]
    public void OnDestroy(ref SystemState state)
    {

    }

    [BurstCompile]
    public void OnUpdate(ref SystemState state)
    {

        float deltaTime = SystemAPI.Time.DeltaTime;
        var ecbSingleton = SystemAPI.GetSingleton<BeginInitializationEntityCommandBufferSystem.Singleton>();

        new SpawnEnemyJob
        {
            DeltaTime = deltaTime,
            ECB = ecbSingleton.CreateCommandBuffer(state.WorldUnmanaged)
        }.Run();
    }
}

[BurstCompile]
public partial struct SpawnEnemyJob : IJobEntity
{
    public float DeltaTime;
    public EntityCommandBuffer ECB;

    private void Execute(GameWorldAspect gameWorldAspect)
    {
        gameWorldAspect.EnemySpawnTimer -= DeltaTime;
        
        if (!gameWorldAspect.TimeToSpawnEnemy) return;
        if (!gameWorldAspect.EnemySpawnPointInitialized()) return;

        gameWorldAspect.EnemySpawnTimer = gameWorldAspect.EnemySpawnRate;

        Entity newEnemy = ECB.Instantiate(gameWorldAspect.EnemyPrefab);
        LocalTransform newEnemyTransform = gameWorldAspect.GetEnemySpawnPoint();
        ECB.SetComponent(newEnemy, newEnemyTransform);
    }
}