using Unity.Entities;
using Unity.Mathematics;
using Unity.Transforms;

public readonly partial struct EnemyAttackAspect : IAspect
{
    public readonly Entity Entity;

    private readonly RefRW<LocalTransform> _transform;

    private readonly RefRW<EnemyTimer> _enemyTimer;
    private readonly RefRO<EnemyAttackProperties> _attackProperties;
    private readonly RefRO<EnemyHeading> _heading;

    private float AttackDamagePerSecond => _attackProperties.ValueRO.DamagePerSecond;
    private float AttackMovementAmplitude => _attackProperties.ValueRO.AttackMovementAmplitude;
    private float AttackMovementFrequency => _attackProperties.ValueRO.AttackMovementFrequency;
    private float Heading => _heading.ValueRO.Value;

    private float EnemyTimer
    {
        get => _enemyTimer.ValueRO.Value;
        set => _enemyTimer.ValueRW.Value = value;
    }

    public void Attack(float deltaTime, EntityCommandBuffer.ParallelWriter ecb, int sortKey, Entity baseEntity)
    {
        EnemyTimer += deltaTime;
        float attackAngle = AttackMovementAmplitude * math.sin(AttackMovementFrequency * EnemyTimer);
        _transform.ValueRW.Rotation = quaternion.Euler(attackAngle, Heading, 0);

        float attackDamage = AttackDamagePerSecond * deltaTime;
        var currentBaseDamage = new BaseDamageBufferElement { Value = attackDamage };

        ecb.AppendToBuffer(sortKey, baseEntity, currentBaseDamage);
    }

    public bool IsInAttackRange(float3 playerBasePosition, float playerBaseRadiusSq)
    {
        return math.distancesq(playerBasePosition, _transform.ValueRO.Position) <= playerBaseRadiusSq - 1;
    }
}


