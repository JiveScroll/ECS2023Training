using Unity.Burst;
using Unity.Entities;

[BurstCompile]
public struct EnemyMovementProperties : IComponentData, IEnableableComponent
{
    public float WalkSpeed;
    public float WalkAmplitude;
    public float WalkFrequency;
}

public struct EnemyAttackProperties : IComponentData, IEnableableComponent
{
    public float DamagePerSecond;
    public float AttackMovementAmplitude;
    public float AttackMovementFrequency;
}

public struct EnemyTimer : IComponentData
{
    public float Value;
}

public struct EnemyHeading : IComponentData
{
    public float Value;
}

public struct NewEnemyTag : IComponentData { }

