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

    public void Attack(float deltaTime)
    {
        EnemyTimer += deltaTime;
        float attackAngle = AttackMovementAmplitude * math.sin(AttackMovementFrequency * EnemyTimer);
        _transform.ValueRW.Rotation = quaternion.Euler(attackAngle, Heading, 0);

        //var eatDamage = AttackDamagePerSecond * deltaTime;
        
        //var curBrainDamage = new BrainDamageBufferElement { Value = eatDamage };
        //ecb.AppendToBuffer(sortKey, playerBaseEntity, curBrainDamage);
    }

    public bool IsInEatingRange(float3 playerBasePosition, float playerBaseRadiusSq)
    {
        return math.distancesq(playerBasePosition, _transform.ValueRO.Position) <= playerBaseRadiusSq - 1;
    }
}


