using Unity.Burst;
using Unity.Entities;
using Unity.Mathematics;
using Unity.Transforms;

[BurstCompile]
[UpdateAfter(typeof(EnemyWalkSystem))]
[UpdateBefore(typeof(TransformSystemGroup))]
public partial struct EnemyAttackSystem : ISystem
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
        var baseEntity = SystemAPI.GetSingletonEntity<PlayerBaseTag>();

        LocalTransform baseLocalTransform = SystemAPI.GetComponent<LocalTransform>(baseEntity);
        float3 basePosition = baseLocalTransform.Position;
        float baseScale = baseLocalTransform.Scale;
        float baseRadius = baseScale * 5f + 3f;

        new EnemyAttackJob
        {
            DeltaTime = deltaTime,
            ECB = ecbSingleton.CreateCommandBuffer(state.WorldUnmanaged).AsParallelWriter(),
            BaseEntity = baseEntity,
            BasePosition = basePosition,
            BaseRadiusSq = baseRadius
        }.ScheduleParallel();
    }
}

[BurstCompile]
public partial struct EnemyAttackJob : IJobEntity
{
    public float DeltaTime;
    public EntityCommandBuffer.ParallelWriter ECB;
    public Entity BaseEntity;
    public float3 BasePosition;
    public float BaseRadiusSq;

    [BurstCompile]
    private void Execute(EnemyAttackAspect enemy, [ChunkIndexInQuery]int sortKey)
    {
        if (enemy.IsInAttackRange(BasePosition, BaseRadiusSq))
        {
            enemy.Attack(DeltaTime, ECB, sortKey, BaseEntity);

        }
        else
        {
            ECB.SetComponentEnabled<EnemyAttackProperties>(sortKey, enemy.Entity, false);
            ECB.SetComponentEnabled<EnemyMovementProperties>(sortKey, enemy.Entity, true);
        }
    }
}