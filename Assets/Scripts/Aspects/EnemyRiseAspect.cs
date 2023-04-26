using Unity.Entities;
using Unity.Mathematics;
using Unity.Transforms;

public readonly partial struct EnemyRiseAspect : IAspect
{
    public readonly Entity Entity;

    private readonly RefRW<LocalTransform> _transform;
    private readonly RefRO<EnemyRiseRate> _enemyRiseRate;

    public void Rise(float deltaTime)
    {
        _transform.ValueRW.Position += math.up() * _enemyRiseRate.ValueRO.Value * deltaTime;
    }

    public bool IsAboveGround => _transform.ValueRO.Position.y >= 0f;

    public void SetAtGroundLevel()
    {
        var position = _transform.ValueRO.Position;
        position.y = 0f;
        _transform.ValueRW.Position = position;
    }
}