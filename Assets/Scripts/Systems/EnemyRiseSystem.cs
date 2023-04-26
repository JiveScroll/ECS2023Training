using Unity.Burst;
using Unity.Entities;
using Unity.Transforms;

[BurstCompile]
[UpdateAfter(typeof(EnemySpawnerSystem))]
[UpdateBefore(typeof(TransformSystemGroup))]
public partial struct EnemyRiseSystem : ISystem
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
        var ecbSingleton = SystemAPI.GetSingleton<EndSimulationEntityCommandBufferSystem.Singleton>();

        new EnemyRiseJob
        {
            DeltaTime = deltaTime,
            ECB = ecbSingleton.CreateCommandBuffer(state.WorldUnmanaged).AsParallelWriter()
        }.ScheduleParallel();
    }
}

[BurstCompile]
public partial struct EnemyRiseJob : IJobEntity
{
    public float DeltaTime;
    public EntityCommandBuffer.ParallelWriter ECB;

    [BurstCompile]
    private void Execute(EnemyRiseAspect enemy, [ChunkIndexInQuery]int sortKey)
    {
        enemy.Rise(DeltaTime);

        if (enemy.IsAboveGround)
        {
            enemy.SetAtGroundLevel();
            ECB.RemoveComponent<EnemyRiseRate>(sortKey,enemy.Entity);
            ECB.SetComponentEnabled<EnemyMovementProperties>(sortKey, enemy.Entity, true);
        }
    }
}