using Unity.Entities;
using Unity.Mathematics;
using Unity.Transforms;

public readonly partial struct EnemyMovementAspect : IAspect
{
    public readonly Entity Entity;

    private readonly RefRW<LocalTransform> _transform;
    private readonly RefRW<EnemyTimer> _walkTimer;
    private readonly RefRO<EnemyHeading> _Heading;
    private readonly RefRO<EnemyMovementProperties> _movementProperties;

    private float walkSpeed => _movementProperties.ValueRO.WalkSpeed;
    private float walkAmplitude => _movementProperties.ValueRO.WalkAmplitude;
    private float walkFrequency => _movementProperties.ValueRO.WalkFrequency;
    private float heading => _Heading.ValueRO.Value;

    private float walkTimer
    {
        get => _walkTimer.ValueRO.Value;
        set => _walkTimer.ValueRW.Value = value;
    }

    public void Walk(float deltaTime)
    {
        walkTimer += deltaTime;
        _transform.ValueRW.Position += _transform.ValueRW.Forward() * walkSpeed * deltaTime;

        var swayAngle = walkAmplitude * math.sin(walkFrequency * walkTimer);
        _transform.ValueRW.Rotation = quaternion.Euler(0, heading, swayAngle);
    }

    public bool IsInStoppingRange(float3 stoppingPosition, float stoppingRadiusSq)
    {
        return math.distancesq(stoppingPosition, _transform.ValueRO.Position) <= stoppingRadiusSq;
    }
}