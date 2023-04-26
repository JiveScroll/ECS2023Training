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

        new EnemyAttackJob
        {
            DeltaTime = deltaTime,     
        }.ScheduleParallel();
    }
}

[BurstCompile]
public partial struct EnemyAttackJob : IJobEntity
{
    public float DeltaTime;

    [BurstCompile]
    private void Execute(EnemyAttackAspect enemy)
    {
        enemy.Attack(DeltaTime);        
    }
}