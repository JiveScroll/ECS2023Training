using Unity.Burst;
using Unity.Entities;
using Unity.Mathematics;
using Unity.Transforms;

[BurstCompile]
[UpdateAfter(typeof(EnemyRiseSystem))]
[UpdateBefore(typeof(TransformSystemGroup))]
public partial struct EnemyWalkSystem: ISystem
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
        var playerBaseEntity = SystemAPI.GetSingletonEntity<PlayerBaseTag>();

        LocalTransform playerBaseTransform = SystemAPI.GetComponent<LocalTransform>(playerBaseEntity);

        float3 playerBasePosition = playerBaseTransform.Position;
        float playerBaseRadius = playerBaseTransform.Scale * 5f + 2f;

        new EnemyWalkJob
        {
            DeltaTime = deltaTime,
            StoppingPosition = playerBasePosition,
            StoppingPositionRadiusSq = playerBaseRadius,
            ECB = ecbSingleton.CreateCommandBuffer(state.WorldUnmanaged).AsParallelWriter()
        }.ScheduleParallel();
    }
}

[BurstCompile]
public partial struct EnemyWalkJob : IJobEntity
{
    public float DeltaTime;
    public float3 StoppingPosition;
    public float StoppingPositionRadiusSq;
    public EntityCommandBuffer.ParallelWriter ECB;

    [BurstCompile]
    private void Execute(EnemyMovementAspect enemy, [ChunkIndexInQuery] int sortKey)
    {
        enemy.Walk(DeltaTime);
        if (enemy.IsInStoppingRange(StoppingPosition, StoppingPositionRadiusSq))
        {
            ECB.SetComponentEnabled<EnemyMovementProperties>(sortKey, enemy.Entity, false);
            ECB.SetComponentEnabled<EnemyAttackProperties>(sortKey, enemy.Entity, true);

        }
    }
}