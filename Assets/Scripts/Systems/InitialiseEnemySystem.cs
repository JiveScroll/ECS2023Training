using Unity.Burst;
using Unity.Collections;
using Unity.Entities;
using Unity.Transforms;

[BurstCompile]
[UpdateInGroup(typeof(InitializationSystemGroup))]

public partial struct IntialiseEnemySystem : ISystem
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
        var ecb = new EntityCommandBuffer(Allocator.Temp);
        foreach (EnemyMovementAspect enemy in SystemAPI.Query<EnemyMovementAspect>().WithAll<NewEnemyTag>())
        {
            ecb.RemoveComponent<NewEnemyTag>(enemy.Entity);
            ecb.SetComponentEnabled<EnemyMovementProperties>(enemy.Entity, false);
            ecb.SetComponentEnabled<EnemyAttackProperties>(enemy.Entity, false);
        }

        ecb.Playback(state.EntityManager);
    }
}