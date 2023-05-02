using Unity.Entities;
using Unity.Transforms;

public readonly partial struct PlayerBaseAspect : IAspect
{
    public readonly Entity Entity;

    private readonly RefRW<LocalTransform> _transform;
    private readonly RefRW<BaseHealth> _baseHealth;
    private readonly DynamicBuffer<BaseDamageBufferElement> _baseDamageBuffer;

    public void DamageBase()
    {
        foreach (var baseDamageBufferElement in _baseDamageBuffer)
        {
            _baseHealth.ValueRW.Value -= baseDamageBufferElement.Value;
        }

        _baseDamageBuffer.Clear();

        _transform.ValueRW.Scale = _baseHealth.ValueRO.Value / _baseHealth.ValueRO.MaxValue;
    }
}